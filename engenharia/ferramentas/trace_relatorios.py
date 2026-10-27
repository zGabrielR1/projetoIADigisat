#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Rastrea o catálogo de relatórios do DigiSat Suite G6 nos assemblies descompilados.

Para cada entrada do inventário marcada como "execução interna pendente", resolve a cadeia:

    Reports/<Classe>.cs            (parâmetros de tela + chamada de canal)
      -> I<Area>ModuleService.Metodo            (contrato do canal)
      -> Services/.../<Area>ModuleService.cs    (ponte WCF + permissão)
      -> <Area>AppService.Metodo                (filtros, repositório, projeção)

O resultado é um relatório de texto por entrada, com a evidência local de cada degrau e
a marcação explícita dos limites de evidência (trechos que não existem nos assemblies
instalados). Não inventa regras: tudo que aparece sai literalmente dos arquivos.

Uso:
    python engenharia/ferramentas/trace_relatorios.py [catalogo.md] [saida.txt]

Também grava um TSV de classificação (uma linha por entrada do catálogo) no caminho
`saida.txt` com extensão `.tsv`, usado por `atualizar_catalogo.py` para reescrever a
coluna "Cobertura da regra" sem edição manual.
"""

import functools
import os
import re
import sys

RAIZ = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", ".."))
VERSAO = "2.0.26273.6"
BASE = os.path.join(RAIZ, "engenharia", "assemblies-descompilados", VERSAO)
REL = os.path.join(BASE, "Modulo.Relatorios", "Modulo.Relatorios", "Reports")
WCF = os.path.join(BASE, "DigisatServer.Distributed.Wcf.Main", "DigisatServer.Distributed.Wcf.Main")
APPS = os.path.join(BASE, "DigisatServer.Application.Main", "DigisatServer.Application.Main")
# Camada de persistencia: implementacoes dos contratos de repositorio de leitura.
# Sem ela a regra de varios relatórios para na assinatura do serviço de aplicacao.
DATA_MAIN = os.path.join(BASE, "DigisatServer.Infrastructure.Data.Main")
DATA_CORE = os.path.join(BASE, "DigisatServer.Infrastructure.Data.Core")
DATA = [DATA_MAIN, DATA_CORE]

# Tela -> canal. Há duas formas no código decompilado:
#   (a) lambda:  Canal<I...ModuleService, Task<TDto>>(x => x.Metodo(args))
#   (b) delegate: Canal<I...ModuleService, Task<TDto>>(delegate(I...ModuleService x) { ... return x.Metodo(args); })
CANAL_INICIO = re.compile(r"Canal<\s*(I\w+ModuleService)\b")
CANAL_LAMBDA = re.compile(r"=>\s*\w+\.(\w+)\s*\(")
CANAL_DELEGATE = re.compile(r"delegate\s*\(\s*I\w+ModuleService\s+(\w+)\s*\)")
DELEGATE_RETORNO = re.compile(r"\breturn\s+(\w+)\.(\w+)\s*\(")
# Títulos dos parâmetros de tela (ViewModels expostos ao usuário).
PARAMTIT = re.compile(r"Titulo\s*=\s*\"([^\"]{2,60})\"")
# Chamada a um serviço de aplicação dentro do corpo da ponte WCF.
APPCALL = re.compile(r"\._(\w*[Aa]pp[Ss]ervice)\.(\w+)\s*\(")
# Qualquer chamada de método (usado para casar nomes genéricos como _appService).
CALL = re.compile(r"\.\s*(\w+)\s*\(")
CLASSE = re.compile(r"\bclass\s+(\w+)")
# Declaracao de classe com a lista de interfaces implementadas.
DECLARACAO = re.compile(r"\bclass\s+(\w+)\s*:\s*([^\{]+)\{")
IMPLEMENTA = re.compile(r"\bI(\w+ModuleService)\b")
# Cadeia de filtros de Specification: .PorDataEmissao(...), .ComTexto(...), .AtivoPorEmpresa(...)
FLT = re.compile(r"\.(Ativo|Inativo|Por|Com|Entre|Na|No|Para|Do|Da|Sem|Apenas|Somente|Todas|"
                  r"Qualquer|Nao|Avancado|Versao|Clientes|Todos|Exceto|Varios|Multiplos)\w*\s*\(")
# Tipo da especificacao e entidade alvo
SPEC_TIPO = re.compile(r"\b(\w*Specification)<(\w+)>")
SPEC_ESTATICA = re.compile(r"\b(\w*Specification)\.")
# Projecao aplicada no resultado da consulta
PROJECAO = re.compile(r"\.(ProjectedAs\w*|Morfar\w*)\s*\(")
METODO_PUB = re.compile(r"\bpublic\s+[\w<>\[\],\s\.]+?\b(\w+)\s*\(")
CONSTRUTOR = re.compile(r"public\s+\w+ModuleService\s*\(([^)]*)\)")
# Chamadas de rede / API de terceiros dentro do servico de aplicacao.
EXTERNO = re.compile(r"(PostAsync|GetAsync|PutAsync|DeleteAsync|HttpClient|RestClient|WebRequest|"
                     r"Api\w*Service\b|CriarApi\()")
# Contrato de repositorio de leitura. O campo pode ser `_repositoryRead` (generico,
# sem a palavra "Repository" com inicial maiuscula) ou `_algoRepositoryRead`.
# A chamada pode ser `this._xRepositoryRead.Metodo()` ou `this.RepositoryRead.Metodo()`
# (propriedade herdada da classe base do serviço).
REPO_CAMPO = re.compile(r"(?:\bthis\.)?_(\w*[Rr]epository\w*)\b")
# Metodo chamado no repositorio: Find/FindAsync/GetById/ObterPara...
REPO_METODO = re.compile(r"(?:\bthis\.)?_(\w*[Rr]epository\w*)\s*\.\s*(\w+)\s*\(")
# Tipo declarado de um campo de repositorio: `private readonly IMovimentacaoRepositoryRead _x;`
CAMPO_REPO = re.compile(r"\bI(\w*Repository(?:Read|Write))\s+_(\w+)\s*;")
# Tipo declarado de repositorio generico: `IRepositoryRead<Custo> _repositoryRead;`
CAMPO_REPO_GENERICO = re.compile(r"\bIRepositoryRead\s*<\s*(\w+)\s*>\s+_(\w+)\s*;")
# Propriedade de repositorio herdada da classe base: `this.RepositoryRead.Find(...)`
REPO_PROPRIEDADE = re.compile(r"\bthis\.(RepositoryRead|RepositoryEmpresaRead)\s*\.\s*(\w+)\s*\(")
# Predicados de GetByFields: `(Movimentacao x) => x.DataUtilizacao >= dataInicial`
PREDICADO = re.compile(r"x\s*\)\s*=>\s*([^,\n\)]+)")
# Cláusulas LINQ de consulta (query syntax): `where x.Campo == valor`, `join y in coll on ...`
LINQ_WHERE = re.compile(r"\bwhere\s+([^\n]{3,140}?)(?=\s*(?:from|join|select|let|orderby|group)\b|\s*$)",
                         re.IGNORECASE | re.MULTILINE)
LINQ_JOIN = re.compile(r"\bjoin\s+(\w+)\s+in\s+(\w+)\s+on\s+([^\n]{3,120}?)(?=\s*(?:from|join|where|select|let|orderby|group)\b|\s*$)",
                        re.IGNORECASE | re.MULTILINE)
# Chamada interna do proprio servico: `this.ObterComparativoMes(...)`
CHAMADA_INTERNA = re.compile(r"\bthis\.(\w+)\s*\(")


@functools.lru_cache(maxsize=None)
def arquivos(base):
    achados = []
    for raiz, _, arqs in os.walk(base):
        for arq in arqs:
            if arq.endswith(".cs"):
                achados.append(os.path.join(raiz, arq))
    return tuple(sorted(achados))


@functools.lru_cache(maxsize=None)
def carregar(caminho):
    with open(caminho, encoding="utf-8-sig", errors="replace") as fh:
        return fh.read()


@functools.lru_cache(maxsize=None)
def indice_por_nome(base):
    """Nome do arquivo sem extensão (minúsculo) -> caminhos com esse nome."""
    mapa = {}
    for caminho in arquivos(base):
        mapa.setdefault(os.path.basename(caminho)[:-3].lower(), []).append(caminho)
    return mapa


@functools.lru_cache(maxsize=None)
def indice_ponte_por_interface():
    """Interface do canal -> implementacao (ponte WCF).

    O nome da classe nao segue o nome da interface: no codigo original existe
    `KitModulseService` (grafia divergente) como implementacao de `IKitModuleService`.
    Por isso a ligacao e feita pela interface declarada, nao pelo nome do arquivo.
    """
    mapa = {}
    for caminho in arquivos(WCF):
        texto = carregar(caminho)
        for classe, bases in DECLARACAO.findall(texto):
            for interface in IMPLEMENTA.findall(bases):
                mapa.setdefault(interface.lower(), []).append((classe, caminho))
    return mapa


def candidatos_ponte(nome_ponte):
    """Ponte WCF que pode implementar a interface: (classe, caminho).

    Junta a busca pelo nome do arquivo com a busca pela interface declarada, para
    pegar implementacoes cuja classe tem grafia diferente da interface.
    """
    achados = []
    vistos = set()
    for classe, caminho in indice_ponte_por_interface().get(nome_ponte.lower(), []):
        if caminho not in vistos:
            vistos.add(caminho)
            achados.append((classe, caminho))
    for caminho in indice_por_nome(WCF).get(nome_ponte.lower(), []):
        if caminho not in vistos:
            vistos.add(caminho)
            achados.append(("", caminho))
    return achados


@functools.lru_cache(maxsize=None)
def indice_app_service():
    """Classe de aplicação -> caminhos. Usado quando a ponte injeta a interface do contrato."""
    mapa = {}
    for caminho in arquivos(APPS):
        texto = carregar(caminho)
        for nome in CLASSE.findall(texto):
            if nome.lower().endswith("appservice"):
                mapa.setdefault(nome.lower(), []).append(caminho)
    return mapa


@functools.lru_cache(maxsize=None)
def indice_repositorio():
    """Interface de repositorio -> (classe, caminho da implementacao).

    A implementacao fica em `DigisatServer.Infrastructure.Data.Main` (contratos
    especificos) ou em `DigisatServer.Infrastructure.Data.Core` (base generica
    `RepositoryRead<TEntity>`). A ligacao e feita pela interface declarada na
    classe, e nao pelo nome do arquivo.
    """
    mapa = {}
    for base in DATA:
        if not os.path.isdir(base):
            continue
        for caminho in arquivos(base):
            texto = carregar(caminho)
            for classe, bases in DECLARACAO.findall(texto):
                for interface in re.findall(r"\bI(\w*Repository\w*)\b", bases):
                    mapa.setdefault(interface.lower(), []).append((classe, caminho))
    return mapa


def metodo_do_repositorio(interface_repo, metodo):
    """Localiza a implementacao de um metodo de repositorio e devolve o corpo.

    Devolve (caminho, corpo, classe) ou (None, None, None).
    """
    for classe, caminho in indice_repositorio().get(interface_repo.lower(), []):
        texto = carregar(caminho)
        corpo, _ = corpo_do_metodo(texto, metodo)
        if corpo:
            return caminho, corpo, classe
    return None, None, None


def contratos_do_servico(texto_app):
    """Mapeia nome do campo -> interface de repositorio, a partir das declaracoes.

    Cobre o contrato especifico (`IMovimentacaoRepositoryRead _movimentacaoRepositoryRead`)
    e o generico (`IRepositoryRead<Custo> _repositoryRead`), alem do `this.RepositoryRead`
    herdado da classe base.
    """
    tipos = {}
    for interface, campo in CAMPO_REPO.findall(texto_app):
        tipos[campo.lower()] = interface
    for entidade, campo in CAMPO_REPO_GENERICO.findall(texto_app):
        tipos[campo.lower()] = "RepositoryRead<%s>" % entidade
    return tipos


def descer_para_repositorio(corpo, texto_app):
    """Desce do serviço de aplicacao para a implementacao de repositorio.

    Devolve uma lista de (classe_repo, metodo_repo, caminho, resumo). Uma entrada
    por chamada de repositorio que teve a implementacao localizada.
    """
    tipos = contratos_do_servico(texto_app)
    achados = []
    for campo, metodo in dict.fromkeys(REPO_METODO.findall(corpo)):
        interface = tipos.get(campo.lower())
        if not interface:
            continue
        base = interface.split("<")[0]
        caminho, corpo_repo, classe = metodo_do_repositorio(base, metodo)
        if not corpo_repo:
            continue
        achados.append((classe, metodo, caminho, resumir_repositorio(corpo_repo)))
    return achados


def resumir_repositorio(corpo):
    """Extrai do repositorio: specification, filtros, agregacao e a consulta Mongo."""
    filtros = []
    for achado in FLT.finditer(corpo):
        nome = achado.group(0).strip().lstrip(".").split("(")[0].strip()
        if nome not in filtros:
            filtros.append(nome)
    predicados = []
    for achado in PREDICADO.finditer(corpo):
        texto = " ".join(achado.group(1).split())
        if texto and texto not in predicados:
            predicados.append(texto)
    where = []
    for achado in LINQ_WHERE.finditer(corpo):
        texto = " ".join(achado.group(1).split())
        if texto and texto not in where:
            where.append(texto)
    joins = []
    for achado in LINQ_JOIN.finditer(corpo):
        texto = "%s in %s on %s" % (achado.group(1), achado.group(2),
                                    " ".join(achado.group(3).split()))
        if texto not in joins:
            joins.append(texto)
    return {
        "filtros": filtros[:16],
        "specs": sorted(set([m[0] for m in SPEC_TIPO.findall(corpo)]
                            + [m for m in SPEC_ESTATICA.findall(corpo)]))[:8],
        "predicados": predicados[:8],
        "where": where[:8],
        "joins": joins[:6],
        "grupos": sorted(set(re.findall(r"GroupBy\s*\(\s*(\w+)", corpo))),
        "agreg": sorted(set(re.findall(r"\.(Sum|Count|Average|Max|Min)\s*\(", corpo))),
        "mongo": sorted(set(re.findall(r"Builders\s*<\s*(\w+)\s*>\s*\.\s*Filter\s*\.\s*(\w+)", corpo))),
        "ordem": sorted(set(" ".join(o.split())[:46]
                            for o in re.findall(r"OrderBy(?:Descending)?\s*\(\s*([^,)]{0,46})", corpo))),
        "pagina": bool(re.search(r"\b(Skip|Take)\s*\(", corpo)),
        "projecao": sorted(set(re.findall(r"new\s+(\w+Dto)\s*\(", corpo)))[:10],
    }


def canais_de(texto):
    """Lista (interface, metodo) de todas as chamadas de canal de serviço no relatório.

    As duas formas aceitas estão em CANAL_LAMBDA e CANAL_DELEGATE; a forma delegate
    resolve o nome do método pelo `return <parametro>.Metodo(...)` do corpo.
    """
    achados = []
    for inicio in CANAL_INICIO.finditer(texto):
        interface = inicio.group(1)
        trecho = texto[inicio.end():inicio.end() + 4000]
        metodo = None
        delegate = CANAL_DELEGATE.search(trecho)
        if delegate:
            parametro = delegate.group(1)
            for achado in DELEGATE_RETORNO.finditer(trecho):
                if achado.group(1) == parametro:
                    metodo = achado.group(2)
                    break
        else:
            achado = CANAL_LAMBDA.search(trecho)
            if achado:
                metodo = achado.group(1)
        if metodo:
            achados.append((interface, metodo))
    return list(dict.fromkeys(achados))


def resumo_apresentacao(texto):
    """Totais e pós-processamento feitos na própria tela, depois que o serviço devolve os DTOs."""
    corpo = texto
    agreg = sorted(set(re.findall(r"\.(Sum|Count|Average|Max|Min)\s*<", corpo)))
    grupos = sorted(set(re.findall(r"\.GroupBy\s*\(", corpo)))
    return {
        "agreg": agreg,
        "grupos": grupos,
        "resumo": sorted(set(re.findall(r"XRSummary\s*(\w+)", corpo)))[:6],
    }


def corpo_do_metodo(texto, metodo):
    """Devolve (corpo, assinatura) do método público `metodo`, ou (None, None)."""
    achado = re.search(r"public\s+[\w<>\[\],\s\.]+?\b%s\s*\(([^)]*)\)\s*\{" % re.escape(metodo), texto)
    if not achado:
        return None, None
    inicio = achado.start()
    profundidade = 0
    abre = texto.find("{", inicio)
    fecha = abre
    for pos in range(abre, len(texto)):
        if texto[pos] == "{":
            profundidade += 1
        elif texto[pos] == "}":
            profundidade -= 1
            if profundidade == 0:
                fecha = pos
                break
    return texto[inicio:fecha], " ".join(achado.group(1).split())


def resumir_servico(texto, metodo):
    """Extrai do serviço de aplicação: filtros, repositório, agrupamento, agregação e DTOs."""
    corpo, args = corpo_do_metodo(texto, metodo)
    if corpo is None:
        return None
    filtros = []
    for achado in FLT.finditer(corpo):
        nome = achado.group(0).strip().lstrip(".").split("(")[0].strip()
        if nome not in filtros:
            filtros.append(nome)
    specs = [m[0] for m in SPEC_TIPO.findall(corpo)] + [m for m in SPEC_ESTATICA.findall(corpo)]
    externo = sorted(set(EXTERNO.findall(corpo)))
    repos = sorted(set(REPO_CAMPO.findall(corpo)))
    repo_metodos = sorted(set("%s.%s" % (r, m) for r, m in REPO_METODO.findall(corpo)))
    predicados = []
    for achado in PREDICADO.finditer(corpo):
        texto = " ".join(achado.group(1).split())
        if texto and texto not in predicados:
            predicados.append(texto)
    return {
        "args": args[:190],
        "corpo": corpo,
        "externo": externo[:4],
        "specs": sorted(set(specs)),
        "filtros": filtros[:14],
        "predicados": predicados[:8],
        "repos": repos,
        "repo_metodos": repo_metodos[:4],
        "grupos": sorted(set(re.findall(r"GroupBy\s*\(\s*(\w+)", corpo))),
        "agreg": sorted(set(re.findall(r"\.(Sum|Count|Average|Max|Min)\s*\(", corpo))),
        "ordem": sorted(set(" ".join(o.split())[:46]
                            for o in re.findall(r"OrderBy(?:Descending)?\s*\(\s*([^,)]{0,46})", corpo))),
        "pagina": bool(re.search(r"\b(Skip|Take)\s*\(", corpo)),
        "projecao": sorted(set(re.findall(r"new\s+(\w+Dto)\s*\(", corpo)))[:10],
        "projecao_metodo": sorted(set(PROJECAO.findall(corpo)))[:6],
        "linq": sorted(set(re.findall(r"\.(from|join|where|select|orderby|group\s+by|let|take|skip)\b",
                                      corpo, re.IGNORECASE)))[:6],
    }


def resolver_servico_aplicacao(corpo_ponte, texto_ponte):
    """Descobre qual serviço de aplicação a ponte WCF chama.

    Primeiro tenta o nome do campo (ex.: `_movimentacaoRelatorioAppService`). Quando a ponte
    usa um campo genérico (`_appService`), usa o tipo declarado no construtor
    (ex.: `ICustoDiretoRelatorioAppService`) para achar a implementação.
    """
    candidatos = []
    for nome_campo, metodo in dict.fromkeys(APPCALL.findall(corpo_ponte)):
        for caminho in indice_por_nome(APPS).get(nome_campo.lower(), []):
            candidatos.append((nome_campo, metodo, caminho))

    if not candidatos:
        construtor = CONSTRUTOR.search(texto_ponte)
        if construtor:
            chamados = set(m.lower() for m in CALL.findall(corpo_ponte))
            for tipo in re.findall(r"I(\w*AppService)\s+\w+", construtor.group(1)):
                for caminho in indice_app_service().get(tipo.lower(), []):
                    texto = carregar(caminho)
                    for metodo in METODO_PUB.findall(texto):
                        if metodo.lower() in chamados:
                            candidatos.append((tipo, metodo, caminho))
    return candidatos


def main():
    catalogo = sys.argv[1] if len(sys.argv) > 1 else os.path.join(RAIZ, "docs", "catalogo-relatorios.md")
    saida_path = sys.argv[2] if len(sys.argv) > 2 else os.path.join(RAIZ, "engenharia", "ferramentas", "rastreio-relatorios.txt")
    indice_rel = indice_por_nome(REL)
    indice_wcf = indice_por_nome(WCF)

    pendentes = []
    with open(catalogo, encoding="utf-8") as fh:
        for linha in fh:
            if linha.startswith("|") and "execução interna pendente" in linha:
                pendentes.append([p.strip() for p in linha.strip("|").split("|")])

    classificacao = []
    with open(saida_path, "w", encoding="utf-8") as saida:
        for colunas in pendentes:
            modulo, titulo, classe = colunas[0], colunas[1], colunas[2].strip("`")
            print("=" * 96, file=saida)
            print("## %s  [%s]  %s" % (classe, modulo, titulo), file=saida)

            alvos = indice_rel.get(classe.lower(), [])
            if not alvos:
                print("   RELATORIO: ausente nos assemblies instalados", file=saida)
                classificacao.append((classe, modulo, "limite_relatorio"))
                continue
            nivel = "limite_ponte"

            for caminho in alvos:
                texto = carregar(caminho)
                pasta = os.path.basename(os.path.dirname(caminho))
                parametros = []
                for achado in PARAMTIT.finditer(texto):
                    titulo_param = achado.group(1)
                    if titulo_param not in parametros:
                        parametros.append(titulo_param)
                print("   RELATORIO: Reports/%s/%s.cs" % (pasta, classe), file=saida)
                print("   PARAMS  : %s" % " | ".join(parametros[:12]), file=saida)

                canais = canais_de(texto)
                if not canais:
                    print("   CANAL   : (nao localizado por padrao de texto)", file=saida)
                apresentacao = resumo_apresentacao(texto)
                if apresentacao["agreg"] or apresentacao["grupos"]:
                    print("   TELA    : agregacao local %s%s" % (
                        ", ".join(apresentacao["agreg"]) or "-",
                        ", agrupamento local" if apresentacao["grupos"] else ""), file=saida)
                for interface, metodo in canais:
                    print("   CANAL   : %s.%s" % (interface, metodo), file=saida)
                    nome_ponte = interface[1:] if interface.startswith("I") else interface

                    ponte = None
                    nome_classe_ponte = None
                    for classe_ponte, candidato in candidatos_ponte(nome_ponte):
                        if re.search(r"\b%s\s*\(" % re.escape(metodo), carregar(candidato)):
                            ponte = candidato
                            nome_classe_ponte = classe_ponte
                            break
                    if ponte is None:
                        print("   WCF     : (ponte nao localizada para %s)" % interface, file=saida)
                        continue

                    texto_ponte = carregar(ponte)
                    print("   WCF     : %s" % os.path.relpath(ponte, WCF), file=saida)
                    if nome_classe_ponte and nome_classe_ponte.lower() != nome_ponte.lower():
                        print("   NOTA    : classe %s implementa %s (grafia divergente no original)" % (
                            nome_classe_ponte, interface), file=saida)

                    permissao = re.search(r"\[Permissao\(([^)]*)\)\][\s\S]{0,400}?public\s+[\w<>\[\],\s\.]+?\b%s\s*\("
                                          % re.escape(metodo), texto_ponte)
                    if permissao:
                        mod = re.search(r"Modulo\s*=\s*(\d+),\s*Direito\s*=\s*(\d+)", permissao.group(1))
                        if mod:
                            print("   PERMISSAO: modulo=%s direito=%s" % (mod.group(1), mod.group(2)), file=saida)

                    corpo_ponte, _ = corpo_do_metodo(texto_ponte, metodo)
                    if not corpo_ponte:
                        continue
                    for nome_servico, metodo_servico, caminho_app in resolver_servico_aplicacao(corpo_ponte, texto_ponte):
                        texto_app = carregar(caminho_app)
                        if not re.search(r"\b%s\s*\(" % re.escape(metodo_servico), texto_app):
                            continue
                        print("   APP     : %s.%s  (%s)" % (nome_servico, metodo_servico,
                                                          os.path.relpath(caminho_app, APPS)), file=saida)
                        info = resumir_servico(texto_app, metodo_servico)
                        if info:
                            if info["externo"]:
                                print("        origem: integracao externa (%s)" % ", ".join(info["externo"]), file=saida)
                            if info["args"]:
                                print("        args: %s" % info["args"], file=saida)
                            if info["specs"]:
                                print("        specs: %s" % ", ".join(info["specs"][:8]), file=saida)
                            if info["filtros"]:
                                print("        filtro: %s" % " -> ".join(info["filtros"]), file=saida)
                            if info["predicados"]:
                                print("        campos: %s" % "; ".join(info["predicados"]), file=saida)
                            if info["repos"]:
                                print("        repo : %s" % ", ".join(info["repos"][:4]), file=saida)
                            if info["grupos"]:
                                print("        group: %s" % ", ".join(info["grupos"][:6]), file=saida)
                            if info["agreg"]:
                                print("        agreg: %s" % ", ".join(info["agreg"][:6]), file=saida)
                            if info["ordem"]:
                                print("        order: %s" % "; ".join(info["ordem"][:4]), file=saida)
                            if info["pagina"]:
                                print("        pagin: sim (Skip/Take)", file=saida)
                            if info["projecao"]:
                                print("        dto  : %s" % ", ".join(info["projecao"]), file=saida)
                            if info["projecao_metodo"]:
                                print("        proj : %s" % ", ".join(info["projecao_metodo"]), file=saida)

                            # Desce para a implementacao do repositorio: e la que
                            # ficam a Specification, a consulta Mongo e a agregacao.
                            reached_repo = False
                            repo_com_filtro = False
                            for classe_repo, metodo_repo, caminho_repo, resumo_repo in \
                                    descer_para_repositorio(info["corpo"], texto_app):
                                reached_repo = True
                                print("   REPO   : %s.%s  (%s)" % (classe_repo, metodo_repo,
                                                                  os.path.relpath(caminho_repo, BASE)), file=saida)
                                if resumo_repo["specs"]:
                                    print("        specs: %s" % ", ".join(resumo_repo["specs"]), file=saida)
                                if resumo_repo["filtros"]:
                                    print("        filtro: %s" % " -> ".join(resumo_repo["filtros"]), file=saida)
                                if resumo_repo["predicados"]:
                                    print("        campos: %s" % "; ".join(resumo_repo["predicados"]), file=saida)
                                if resumo_repo["where"]:
                                    print("        where: %s" % "; ".join(resumo_repo["where"]), file=saida)
                                if resumo_repo["joins"]:
                                    print("        join : %s" % "; ".join(resumo_repo["joins"]), file=saida)
                                if resumo_repo["mongo"]:
                                    print("        mongo: %s" % ", ".join("%s.%s" % m for m in resumo_repo["mongo"]), file=saida)
                                if resumo_repo["grupos"]:
                                    print("        group: %s" % ", ".join(resumo_repo["grupos"][:6]), file=saida)
                                if resumo_repo["agreg"]:
                                    print("        agreg: %s" % ", ".join(resumo_repo["agreg"][:6]), file=saida)
                                if resumo_repo["ordem"]:
                                    print("        order: %s" % "; ".join(resumo_repo["ordem"][:4]), file=saida)
                                if resumo_repo["pagina"]:
                                    print("        pagin: sim (Skip/Take)", file=saida)
                                if resumo_repo["projecao"]:
                                    print("        dto  : %s" % ", ".join(resumo_repo["projecao"]), file=saida)
                                if resumo_repo["filtros"] or resumo_repo["predicados"] or resumo_repo["where"]:
                                    repo_com_filtro = True
                                break

                            # Classificacao da cobertura da regra:
                            #   profundo  = a cadeia de filtros foi localizada (servico ou repositorio)
                            #   servico   = o servico e o repositorio foram localizados, mas a
                            #               cadeia de filtros nao aparece nos assemblies
                            #   integracao= a origem dos dados e uma API externa
                            if nivel != "profundo":
                                if info["externo"]:
                                    nivel = "integracao"
                                elif info["filtros"] or info["predicados"] or repo_com_filtro:
                                    nivel = "profundo"
                                elif reached_repo or info["repos"]:
                                    nivel = "servico"
                        break

            classificacao.append((classe, modulo, nivel))

    with open(os.path.splitext(saida_path)[0] + ".tsv", "w", encoding="utf-8") as tsv:
        for classe, modulo, nivel in classificacao:
            tsv.write("%s\t%s\t%s\n" % (classe, modulo, nivel))

    print("rastreio gravado em %s" % saida_path)


if __name__ == "__main__":
    main()
