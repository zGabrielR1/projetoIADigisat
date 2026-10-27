"""Bounded Mongo filters and a SELECT-only SQL subset. No shell, eval or SQL engine."""
from __future__ import annotations

import re
from typing import Literal

from bson import json_util, Code, Regex
from bson.errors import BSONError
from decimal import InvalidOperation
from fastapi import HTTPException
from pydantic import BaseModel, ConfigDict, Field

MAX_JSON = 1024 * 1024
FIELD = re.compile(r'^[\w-]+(?:\.[\w-]+)*$', re.UNICODE)


def fail(message):
    raise HTTPException(422, message)


def field_name(value):
    if not isinstance(value, str) or len(value) > 120 or not FIELD.fullmatch(value):
        fail('Campo inválido. Use nomes de campos e caminhos separados por ponto.')
    return value


def ejson(value):
    return json_util.dumps(value, json_options=json_util.CANONICAL_JSON_OPTIONS, ensure_ascii=False)


def parse_json(text, *, maximum=MAX_JSON):
    if len(text.encode('utf-8')) > maximum:
        fail('JSON acima do tamanho permitido.')
    try:
        return json_util.loads(text)
    except (ValueError, TypeError, OverflowError, RecursionError, KeyError, BSONError, InvalidOperation):
        fail('Extended JSON inválido. Confira aspas, tipos BSON e delimitadores.')


def literal(value, depth=0):
    if depth > 10:
        fail('A consulta excede dez níveis de profundidade.')
    if isinstance(value, Code):
        fail('JavaScript não é aceito nas consultas.')
    if isinstance(value, Regex):
        if not isinstance(value.pattern, str) or len(value.pattern) > 256 or int(value.flags) & ~int(re.IGNORECASE):
            fail('Regex aceita até 256 caracteres e somente a opção i.')
    if isinstance(value, dict):
        for key, child in value.items():
            if key.startswith('$') or '\x00' in key:
                fail('Operador não permitido dentro de um valor literal.')
            literal(child, depth + 1)
    elif isinstance(value, list):
        if len(value) > 100:
            fail('Use listas de até 100 valores.')
        for child in value:
            literal(child, depth + 1)


def validate_filter(query, depth=0, budget=None):
    if budget is None:
        budget = [0]
    if depth > 10 or not isinstance(query, dict):
        fail('O filtro precisa ser um objeto, com até dez níveis.')
    for field, value in query.items():
        budget[0] += 1
        if budget[0] > 100:
            fail('O filtro excede 100 condições.')
        if field in {'$and', '$or', '$nor'}:
            if not isinstance(value, list) or not 1 <= len(value) <= 30:
                fail('Grupos lógicos precisam de 1 a 30 filtros.')
            for child in value:
                validate_filter(child, depth + 1, budget)
            continue
        field_name(field)
        if not isinstance(value, dict) or not any(k.startswith('$') for k in value):
            literal(value, depth)
            continue
        for op, operand in value.items():
            if op in {'$eq', '$ne', '$gt', '$gte', '$lt', '$lte'}:
                literal(operand, depth + 1)
            elif op in {'$in', '$nin'}:
                if not isinstance(operand, list) or not 1 <= len(operand) <= 100:
                    fail('IN precisa de 1 a 100 valores.')
                literal(operand, depth + 1)
            elif op == '$exists':
                if not isinstance(operand, bool):
                    fail('$exists aceita apenas true ou false.')
            elif op == '$regex':
                if not isinstance(operand, str) or len(operand) > 256:
                    fail('Expressão regular inválida ou longa demais.')
            elif op == '$options':
                if '$regex' not in value or operand not in {'', 'i'}:
                    fail('Apenas a opção i de regex é permitida.')
            else:
                fail(f'Operador não permitido: {op}. Use apenas filtros de leitura.')
    return query


class Condition(BaseModel):
    model_config = ConfigDict(extra='forbid')
    field: str = Field(min_length=1, max_length=120)
    operator: Literal['eq', 'ne', 'gt', 'gte', 'lt', 'lte', 'in', 'nin', 'exists', 'contains'] = 'eq'
    value_json: str = Field(default='""', max_length=16384)


class QueryRequest(BaseModel):
    model_config = ConfigDict(extra='forbid')
    database: str = Field(min_length=1, max_length=120)
    collection: str = Field(default='', max_length=120)
    mode: Literal['builder', 'json', 'sql'] = 'builder'
    conditions: list[Condition] = Field(default_factory=list, max_length=30)
    conjunction: Literal['and', 'or'] = 'and'
    filter_json: str = Field(default='{}', max_length=16384)
    projection_json: str = Field(default='{}', max_length=4096)
    sort_json: str = Field(default='{"_id":1}', max_length=4096)
    sql: str = Field(default='', max_length=16384)
    limit: int = Field(default=50, ge=1, le=200)
    offset: int = Field(default=0, ge=0, le=100000)


# Quotes protect identifiers; single quotes protect SQL string values. Every token
# must belong to this grammar, and the complete input must be consumed.
TOKEN = re.compile(r"\s*(?:(?P<string>'(?:''|[^'])*')|(?P<quoted>`(?:``|[^`])+`)|(?P<number>-?(?:\d+(?:\.\d*)?|\.\d+)(?:[eE][+-]?\d+)?)|(?P<op>>=|<=|<>|!=|=|>|<)|(?P<word>[\w.-]+)|(?P<punct>[*,();]))", re.UNICODE)


class SelectParser:
    def __init__(self, text):
        self.tokens = []
        position = 0
        while position < len(text):
            if not text[position:].strip():
                break
            match = TOKEN.match(text, position)
            if not match:
                fail('SQL não reconhecido. Use SELECT, WHERE com AND, ORDER BY, LIMIT e OFFSET.')
            self.tokens.append((match.lastgroup, match.group(match.lastgroup)))
            position = match.end()
        self.position = 0

    def take(self):
        if self.position >= len(self.tokens):
            fail('SQL incompleto.')
        result = self.tokens[self.position]
        self.position += 1
        return result

    def accept(self, value):
        if self.position < len(self.tokens) and self.tokens[self.position][1].upper() == value:
            self.position += 1
            return True
        return False

    def require(self, value):
        if not self.accept(value):
            fail(f'SQL: esperado {value}.')

    def identifier(self):
        kind, value = self.take()
        if kind == 'quoted':
            return value[1:-1].replace('``', '`')
        if kind != 'word':
            fail('SQL: nome de campo ou coleção inválido.')
        return value

    def value(self):
        kind, value = self.take()
        if kind == 'string':
            return value[1:-1].replace("''", "'")
        if kind == 'number':
            try:
                result = float(value) if any(c in value.lower() for c in '.e') else int(value)
                import math
                if not math.isfinite(result) or (isinstance(result, int) and not -(2**63) <= result < 2**63):
                    fail('Número SQL fora do intervalo BSON.')
                return result
            except (ValueError, OverflowError):
                fail('Número SQL inválido.')
        if kind == 'word' and value.upper() in {'TRUE', 'FALSE', 'NULL'}:
            return {'TRUE': True, 'FALSE': False, 'NULL': None}[value.upper()]
        fail('SQL: use números, strings entre aspas simples, TRUE, FALSE ou NULL.')

    def integer(self, minimum, maximum):
        value = self.value()
        if type(value) is not int or not minimum <= value <= maximum:
            fail(f'SQL: use um inteiro entre {minimum} e {maximum}.')
        return value

    def parse(self):
        self.require('SELECT')
        projection = {}
        if not self.accept('*'):
            while True:
                projection[field_name(self.identifier())] = 1
                if len(projection) > 30:
                    fail('Selecione até 30 campos.')
                if not self.accept(','):
                    break
            projection['_id'] = 1
        self.require('FROM')
        collection = self.identifier()
        conditions = []
        if self.accept('WHERE'):
            while True:
                field = field_name(self.identifier())
                if self.accept('IS'):
                    negated = self.accept('NOT')
                    self.require('NULL')
                    condition = {field: {'$ne' if negated else '$eq': None}}
                elif self.accept('IN'):
                    self.require('(')
                    values = [self.value()]
                    while self.accept(','):
                        values.append(self.value())
                        if len(values) > 100:
                            fail('IN aceita até 100 valores.')
                    self.require(')')
                    condition = {field: {'$in': values}}
                elif self.accept('LIKE'):
                    value = self.value()
                    if not isinstance(value, str) or len(value) > 200:
                        fail('LIKE precisa de um texto de até 200 caracteres.')
                    pattern = '^' + ''.join('.*' if c == '%' else '.' if c == '_' else re.escape(c) for c in value) + '$'
                    condition = {field: {'$regex': pattern, '$options': 'i'}}
                else:
                    kind, op = self.take()
                    if kind != 'op':
                        fail('SQL: operador de comparação inválido.')
                    condition = {field: {{'=':'$eq', '!=':'$ne', '<>':'$ne', '>':'$gt', '>=':'$gte', '<':'$lt', '<=':'$lte'}[op]: self.value()}}
                conditions.append(condition)
                if len(conditions) > 30:
                    fail('Use até 30 condições SQL.')
                if not self.accept('AND'):
                    break
        sort = {}
        if self.accept('ORDER'):
            self.require('BY')
            while True:
                field = field_name(self.identifier())
                descending = self.accept('DESC')
                if not descending:
                    self.accept('ASC')
                sort[field] = -1 if descending else 1
                if len(sort) > 5:
                    fail('Ordene por até cinco campos.')
                if not self.accept(','):
                    break
        limit = self.integer(1, 200) if self.accept('LIMIT') else 50
        offset = self.integer(0, 100000) if self.accept('OFFSET') else 0
        self.accept(';')
        if self.position != len(self.tokens):
            fail('SQL fora do subconjunto suportado. Não são aceitos comandos de escrita, funções, JOIN, OR ou múltiplas instruções.')
        return collection, {'$and': conditions} if conditions else {}, projection, sort, limit, offset


def compile_query(req):
    if req.mode == 'sql':
        collection, query, projection, sort, limit, offset = SelectParser(req.sql).parse()
    else:
        collection, limit, offset = req.collection, req.limit, req.offset
        projection, sort = parse_json(req.projection_json, maximum=4096), parse_json(req.sort_json, maximum=4096)
        if req.mode == 'json':
            query = parse_json(req.filter_json, maximum=16384)
        else:
            conditions = []
            for condition in req.conditions:
                field = field_name(condition.field)
                value = parse_json(condition.value_json, maximum=16384)
                op = condition.operator
                if op == 'contains':
                    if not isinstance(value, str) or len(value) > 100:
                        fail('Contém precisa de um texto de até 100 caracteres.')
                    conditions.append({field: {'$regex': re.escape(value), '$options':'i'}})
                else:
                    conditions.append({field: {'$' + op: value}})
            query = {'$' + req.conjunction: conditions} if conditions else {}
    validate_filter(query)
    if not isinstance(projection, dict) or len(projection) > 30:
        fail('Projeção inválida; selecione até 30 campos.')
    for field, value in projection.items():
        field_name(field)
        if type(value) not in {int, bool} or value not in (0, 1):
            fail('A projeção aceita somente 0 ou 1.')
    modes = {int(v) for k, v in projection.items() if k != '_id'}
    if len(modes) > 1:
        fail('Não misture inclusão e exclusão de campos na projeção.')
    if not isinstance(sort, dict) or len(sort) > 6 or len(set(sort) - {'_id'}) > 5:
        fail('Ordenação inválida; use até cinco campos.')
    for field, value in sort.items():
        field_name(field)
        if type(value) is not int or value not in (-1, 1):
            fail('Ordenação aceita somente 1 ou -1.')
    sort.setdefault('_id', 1)
    return {'collection': collection, 'filter': query, 'projection': projection, 'sort': list(sort.items()), 'limit': limit, 'offset': offset}
