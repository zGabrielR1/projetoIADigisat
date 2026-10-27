"""Bounded, read-only investigation plans for natural-language report questions."""
from __future__ import annotations

import re
from datetime import date

from fastapi import HTTPException


def question_details(question: str) -> dict:
    from app import strip_accents
    q = strip_accents(question.casefold())
    discrepancy = bool(re.search(
        r"\b(diferenc\w*|divergenc\w*|inconsisten\w*|discrepanc\w*|concili\w*|"
        r"confer\w*|investig\w*|faltando|sobrando|nao (?:bate\w*|fecha\w*)|"
        r"a (?:mais|menos))\b", q))
    seller = None
    match = re.search(r'\bvendedor(?:a)?\s+(?:n[ºo°.]?\s*)?["“]([^"”]+)["”]', q)
    if not match:
        match = re.search(r"\b(?:vendedor(?:a)?\s+(?:numero\s+|n[ºo°.]?\s*)?|v)(\d+)\b", q)
    if match:
        seller = match.group(1).strip()
    else:
        match = re.search(r"\bvendedor(?:a)?\s+([a-z][a-z -]*)", q)
        if match:
            seller = re.split(r"\s+(?:no|na|do|da|em|de|entre|com|que|e|ontem|hoje|neste|nesta|nesse)\b",
                              match.group(1))[0].strip()
            if seller in {"todos", "todas"}:
                seller = None
    if re.search(r"\b(?:mesmo|aquele) vendedor\b|\b(?:mesma|aquela) vendedora\b", q):
        seller = None
    amount_match = re.search(r"r\$\s*(\d+(?:\.\d{3})*(?:,\d{1,2})?)", q)
    if not amount_match:
        amount_match = re.search(r"(\d+(?:\.\d{3})*(?:,\d{1,2})?)\s*reais\b", q)
    amount = float(amount_match.group(1).replace(".", "").replace(",", ".")) if amount_match else None
    return {"investigate": discrepancy or bool(seller), "seller_query": seller,
            "target_amount": amount if discrepancy else None}


def resolve_seller(db, query: str | None, scope: dict) -> dict | None:
    """Resolve against actual names and period participation; never pick an arbitrary tie."""
    if not query:
        return None
    from app import normalize_text
    from bson import ObjectId
    wanted = normalize_text(query)
    if re.fullmatch(r"[0-9a-f]{24}", wanted):
        candidates = list(db.Pessoas.find({"_id": ObjectId(wanted)}, {"Nome": 1}).max_time_ms(5000))
    else:
        accents = {"a": "[aáàâãä]", "e": "[eéèêë]", "i": "[iíìîï]",
                   "o": "[oóòôõö]", "u": "[uúùûü]", "c": "[cç]", " ": r"[\s-]+"}
        pattern = r"vendedor" if wanted.isdigit() else "".join(accents.get(c, re.escape(c)) for c in wanted)
        people = list(db.Pessoas.find({"Nome": {"$regex": pattern, "$options": "i"}}, {"Nome": 1})
                      .max_time_ms(5000).limit(101))
        if len(people) > 100:
            raise HTTPException(422, "Há muitos vendedores possíveis. Informe o nome completo entre aspas.")
        candidates = [p for p in people if wanted in normalize_text(p.get("Nome", "")).split()
                      or (not wanted.isdigit() and wanted in normalize_text(p.get("Nome", "")))]
    if not candidates:
        raise HTTPException(422, f"Vendedor '{query}' não encontrado. Informe o nome cadastrado entre aspas.")
    if len(candidates) > 1:
        active = [p for p in candidates if db.Movimentacoes.count_documents(
            {"$and": [scope, {"ItensBase": {"$elemMatch": {"VendedorReferencia": p["_id"],
                                                           "Cancelado": {"$ne": True}}}}]}, maxTimeMS=5000)]
        if len(active) == 1:
            candidates = active
        else:
            names = "; ".join(f'{p.get("Nome", "").strip()} (referência {p["_id"]})' for p in (active or candidates))
            error = HTTPException(422, f"Vendedor '{query}' é ambíguo no período. Especifique o nome completo entre aspas ou a referência: {names}.")
            error.choices = [{"label": p.get("Nome", "").strip(), "value": str(p["_id"])} for p in (active or candidates)]
            raise error
    return {"ref": candidates[0]["_id"], "name": candidates[0].get("Nome", "").strip()}


def investigate(req) -> dict:
    import app
    def money(value):
        rounded = round(float(value or 0), 2)
        return 0.0 if abs(rounded) < 0.005 else rounded
    if req.collection != "Movimentacoes" or req.date_field != "DataHoraEmissao":
        raise HTTPException(422, "A investigação usa Movimentacoes e DataHoraEmissao. Selecione essa coleção e esse campo de data.")
    if req.end < req.start:
        raise HTTPException(400, "A data final precisa ser igual ou posterior à inicial.")
    if (req.end - req.start).days > 366:
        raise HTTPException(422, "Para investigar documentos, escolha um período de até 367 dias.")
    if not app.MONGO_DATABASE:
        raise HTTPException(400, "Configure o banco MongoDB antes de investigar.")
    match, scope = app.period_match(req)
    if getattr(req, "status_filter", "valid") == "completed":
        scope["Situacao.Descricao"] = "Concluído"
    if getattr(req, "profile", "panel") == "native_nfce":
        scope.update({"_t": "NotaFiscalConsumidorEletronica", "Situacao.Descricao": "Concluído", "FinalidadeEmissaoNfe.Codigo": 1})
    elif getattr(req, "profile", "panel") == "fiscal_valid":
        scope["_t"] = {"$in": app.MOVEMENT_GROUPS["fiscal_sales"]}
        scope["Situacao.Descricao"] = "Concluído" if getattr(req, "status_filter", "valid") == "completed" else {"$nin": app.STATUS_EXCLUDED}
    if getattr(req, "search", "").strip():
        search = req.search.strip()
        choices = [{"Serie": {"$regex": re.escape(search), "$options": "i"}},
                   {"Empresa.Nome": {"$regex": re.escape(search), "$options": "i"}},
                   {"ItensBase.ProdutoServico.Descricao": {"$regex": re.escape(search), "$options": "i"}}]
        if search.isdigit():
            choices.extend([{"Numero": int(search)}, {"Numero": search}])
        scope["$or"] = choices
    active_items = {"$filter": {"input": {"$ifNull": ["$ItensBase", []]}, "as": "item",
                               "cond": {"$ne": ["$$item.Cancelado", True]}}}
    amount = app.rebase(app.COMPUTED_VALUES["@item_total"], "$$item.")
    with app.mongo_client() as client:
        db = client[app.MONGO_DATABASE]
        seller = resolve_seller(db, req.seller_query, scope)
        selected_scope = {"$and": [scope, {"ItensBase": {"$elemMatch": {
            "VendedorReferencia": seller["ref"], "Cancelado": {"$ne": True}}}}]} if seller else scope
        mine = {"$filter": {"input": "$ItemDetails", "as": "item",
                            "cond": {"$eq": ["$$item.VendedorReferencia", seller["ref"]]}}} if seller else "$ItemDetails"
        item_fields = {"Numero": "$$item.Numero", "Produto": "$$item.ProdutoServico.Descricao",
                       "Quantidade": "$$item.Quantidade", "VendedorReferencia": "$$item.VendedorReferencia",
                       "Valor": amount}
        unassigned = {"$gt": [{"$size": {"$filter": {"input": "$ItemDetails", "as": "i",
                            "cond": {"$eq": [{"$ifNull": ["$$i.VendedorReferencia", None]}, None]}}}}, 0]}
        mismatch = {"$gt": [{"$abs": "$Diferenca"}, 0.011]}
        unpaid = {"$and": [{"$lt": [{"$abs": "$Pagamentos"}, 0.01]}, {"$gt": [{"$abs": "$Itens"}, 0.01]}]}
        shared = {"$gt": [{"$size": {"$setUnion": ["$ItemDetails.VendedorReferencia", []]}}, 1]}
        negative = {"$lt": ["$Itens", -0.011]}
        flags = {"mismatch": mismatch, "unpaid": unpaid, "unassigned": unassigned, "shared": shared, "negative": negative}
        pipeline = [
            {"$match": selected_scope},
            {"$project": {"Data": "$DataHoraEmissao", "Numero": 1, "Serie": 1,
                          "Empresa": "$Empresa.Nome", "Situacao": "$Situacao.Descricao",
                          "Tipo": {"$arrayElemAt": [{"$ifNull": ["$_t", []]}, -1]},
                          "ActiveItems": active_items,
                          "Pagamentos": {"$sum": {"$map": {"input": app.payment_source_expression(),
                                                            "as": "p", "in": "$$p.Valor"}}}}},
            {"$project": {"Data": 1, "Numero": 1, "Serie": 1, "Empresa": 1, "Situacao": 1, "Tipo": 1,
                          "Pagamentos": 1,
                          "ItemDetails": {"$map": {"input": "$ActiveItems", "as": "item", "in": item_fields}}}},
            # Compute each item's formula once, then reuse it for the document,
            # seller attribution, quality checks and the displayed item detail.
            {"$addFields": {"Itens": {"$sum": "$ItemDetails.Valor"},
                            "Vendedor": {"$sum": {"$map": {"input": mine, "as": "item", "in": "$$item.Valor"}}}}},
            {"$addFields": {"Outros": {"$subtract": ["$Itens", "$Vendedor"]},
                            "Diferenca": {"$subtract": ["$Itens", "$Pagamentos"]}}},
            {"$addFields": {"Quality": {key: expr for key, expr in flags.items()}}},
            *([{"$match": {"Quality." + req.issue: True}}] if getattr(req, "issue", "all") != "all" else []),
            {"$facet": {
                "totals": [{"$group": {"_id": None, "documents": {"$sum": 1},
                            **{k: {"$sum": "$" + k} for k in ("Itens", "Vendedor", "Outros", "Pagamentos", "Diferenca")}}}],
                "rows": [{"$sort": {"Data": 1, "Serie": 1, "Numero": 1, "_id": 1}},
                         {"$skip": getattr(req, "offset", 0)},
                         {"$limit": req.limit}, {"$project": {"_id": 0}}],
                "by_type": [{"$group": {"_id": "$Tipo", "documents": {"$sum": 1},
                                        "items": {"$sum": "$Itens"}, "payments": {"$sum": "$Pagamentos"}}}],
                "quality": [{"$group": {"_id": None, **{k: {"$sum": {"$cond": ["$Quality." + k, 1, 0]}} for k in flags}}}],
                "duplicates": [{"$group": {"_id": {"company": "$Empresa", "type": "$Tipo", "serie": "$Serie", "number": "$Numero"},
                                             "count": {"$sum": 1}}}, {"$match": {"count": {"$gt": 1}, "_id.number": {"$ne": None}}},
                               {"$sort": {"count": -1}}, {"$limit": 100}],
            }},
        ]
        try:
            facets = list(db.Movimentacoes.aggregate(pipeline, maxTimeMS=20000, allowDiskUse=False))
        except app.PyMongoError as exc:
            raise HTTPException(422, f"Investigação não concluída: {exc.__class__.__name__}. Reduza o período.") from None
        facet = facets[0] if facets else {}
        total = (facet.get("totals") or [{}])[0]
        rows = facet.get("rows") or []
        refs = {i.get("VendedorReferencia") for row in rows for i in row.get("ItemDetails", []) if i.get("VendedorReferencia")}
        names = {str(p["_id"]): p.get("Nome", "").strip() for p in db.Pessoas.find(
            {"_id": {"$in": list(refs)}}, {"Nome": 1}).max_time_ms(5000)} if refs else {}
    totals = {k: money(total.get(k)) for k in ("Itens", "Vendedor", "Outros", "Pagamentos", "Diferenca")}
    details = []
    for row in rows:
        doc = f'{row.get("Serie", "")}/{row.get("Numero", "")}'
        items = row.pop("ItemDetails", [])
        row.pop("Quality", None)
        for item in items:
            ref = str(item.pop("VendedorReferencia", "") or "")
            item["Vendedor"] = names.get(ref) or (f"Referência {ref}" if ref else "Não informado")
            item["Valor"] = money(item.get("Valor"))
        details.append({"document": doc, "items": app.json_safe(items)})
        for key in totals:
            row[key] = money(row.get(key))
        row["Observacao"] = " · ".join(filter(None, [
            "Itens de outros vendedores" if seller and abs(row["Outros"]) > 0.01 else "",
            "Sem pagamento registrado" if abs(row["Pagamentos"]) < 0.01 and abs(row["Itens"]) > 0.01 else
            ("Itens e pagamentos divergem" if abs(row["Diferenca"]) > 0.01 else ""),
        ])) or "Itens e pagamentos conferem"
    documents = int(total.get("documents") or 0)
    findings = [f'{documents} vendas no período e nos filtros selecionados. Itens: {app.brl(totals["Itens"])}; '
                f'pagamentos registrados: {app.brl(totals["Pagamentos"])}; diferença itens − pagamentos: {app.brl(totals["Diferenca"])}.']
    if seller:
        findings.append(f'{seller["name"]}: itens próprios {app.brl(totals["Vendedor"])}. '
                        f'O total integral das vendas que contêm esse vendedor inclui {app.brl(totals["Outros"])} '
                        'em itens de outros vendedores ou sem vendedor. Essa diferença de atribuição não é falta de pagamento.')
    affected = [row for row in rows if (seller and abs(row["Outros"]) > 0.01) or abs(row["Diferenca"]) > 0.01]
    for row in affected[:10]:
        findings.append(f'Venda {row.get("Serie", "")}/{row.get("Numero", "")}: '
                        f'itens {app.brl(row["Itens"])}, vendedor {app.brl(row["Vendedor"])}, '
                        f'outros/sem vendedor {app.brl(row["Outros"])}, pagamentos {app.brl(row["Pagamentos"])}. '
                        f'{row["Observacao"]}.')
    if req.target_amount is not None:
        target = req.target_amount
        candidates = {"itens − pagamentos": abs(totals["Diferenca"])}
        if seller:
            candidates["itens de outros vendedores nas vendas compartilhadas"] = abs(totals["Outros"])
        label, closest = min(candidates.items(), key=lambda pair: abs(pair[1] - target))
        findings.append(f'Valor mencionado: aproximadamente {app.brl(target)}. Comparação mais próxima: {label}, '
                        f'{app.brl(closest)} (distância de {app.brl(abs(closest - target))}). '
                        'O valor mencionado é uma referência para comparação; nenhuma venda foi excluída por valor.')
    findings.append('Valores calculados a partir dos itens não cancelados. Os pagamentos pertencem ao documento inteiro; '
                    'não são rateados por vendedor. Este diagnóstico não comprova alterações históricas ou causas externas à base.')
    warnings = []
    if documents > len(rows):
        warnings.append(f'Exibindo {len(rows)} de {documents} vendas. Os totais consideram todas; reduza o período ou aumente o limite para listar as demais.')
    if not documents:
        warnings.append("Nenhuma venda corresponde ao vendedor, período e escopo selecionados.")
    return {"period": {"start": str(req.start), "end": str(req.end)}, "rows": app.json_safe(rows),
            "documents": documents, "count": len(rows), "read_only": True, "warnings": warnings,
            "value_field": "@item_total", "scope": {**app.scope_metadata(req), "excluded": 0},
            "quality": {"counts": (facet.get("quality") or [{}])[0], "duplicate_candidates": app.json_safe(facet.get("duplicates") or []),
                        "note": "Duplicidade é apenas uma candidata: mesmo tipo, empresa, série e número. Confira os documentos antes de concluir."},
            "pagination": {"offset": getattr(req, "offset", 0), "limit": req.limit, "total": documents,
                           "has_more": getattr(req, "offset", 0) + len(rows) < documents},
            "investigation": {"seller_query": req.seller_query, "seller": seller["name"] if seller else None,
                              "seller_reference": str(seller["ref"]) if seller else None,
                              "target_amount": req.target_amount, "totals": totals, "findings": findings,
                              "details": details, "by_type": app.json_safe(facet.get("by_type") or []),
                              "truncated": documents > len(rows)},
            "interpretation": {"kind": "documents", "kind_label": "Investigação de vendas",
                               "intent": "investigation", "operation_allowlisted": True,
                               "method": "plano de investigação validado", "period_source": "filtros",
                               "company": req.company_value,
                               "summary": ["Investigação de vendas", f'Vendedor: {seller["name"]}' if seller else "Vendedores: todos",
                                           f'Empresa: {req.company_value or "todas"}',
                                           f'Período: {req.start:%d/%m/%Y} a {req.end:%d/%m/%Y}']}}
