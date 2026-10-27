const $ = (selector, root = document) => root.querySelector(selector);
const $$ = (selector, root = document) => [...root.querySelectorAll(selector)];

const KIND_LABELS = {
  revenue: "Faturamento",
  top_products: "Produtos mais vendidos",
  daily: "Vendas por dia",
  companies: "Por empresa",
  documents: "Documentos",
  payments: "Formas de pagamento",
  ticket: "Ticket médio por dia",
};
const KIND_SHORT = { revenue: "Faturamento", top_products: "Produtos", daily: "Por dia", companies: "Empresas", documents: "Documentos", payments: "Pagamentos", ticket: "Ticket médio" };
const MOVEMENT_LABELS = {
  all_sales: "Vendas (fiscal + registros)",
  fiscal_sales: "Vendas fiscais",
  nfce: "NFC-e",
  nfe: "NF-e de saída",
  manual: "Notas manuais",
  registro_venda: "Registros de venda",
  purchases: "Compras e entradas",
  all: "Tudo",
};
const TYPE_LABELS = {
  NotaFiscalConsumidorEletronica: "NFC-e",
  NotaFiscalEletronicaSaida: "NF-e de saída",
  NotaFiscalEletronicaEntrada: "NF-e de entrada",
  NotaFiscalManual: "Nota manual",
  NotaFiscalCompra: "Nota de compra",
  RegistroVenda: "Registro de venda",
  PreVenda: "Pré-venda",
  DocumentoAuxiliarVenda: "Documento auxiliar de venda",
  PedidoCompra: "Pedido de compra",
};
const MOVEMENT_HINTS = {
  all_sales: "Inclui NFC-e, NF-e de saída, notas manuais e registros de venda; exclui compras, pré-vendas e pedidos.",
  fiscal_sales: "Somente documentos fiscais de saída: NFC-e, NF-e de saída e notas manuais.",
  nfce: "Somente cupons NFC-e.",
  nfe: "Somente NF-e de saída.",
  manual: "Somente notas fiscais manuais.",
  registro_venda: "Somente registros de venda do PDV (modelo antigo).",
  purchases: "Somente notas de entrada e compras.",
  all: "Sem filtro de tipo: inclui vendas, compras, pré-vendas e pedidos.",
};
const RECON_MONEY = new Set(["items", "payments", "difference", "Itens", "Pagamentos", "Diferenca"]);
const RECON_NUMBER = new Set(["documents", "without_payment"]);
const RECON_TYPE_LABELS = { Tipo: "Tipo", documents: "Documentos", items: "Itens", payments: "Pagamentos", difference: "Diferença", without_payment: "Sem pagamento" };
const RECON_DOC_LABELS = { Data: "Emissão", Numero: "Número", Serie: "Série", Empresa: "Empresa", Tipo: "Tipo", Situacao: "Situação", Itens: "Itens", Pagamentos: "Pagamentos", Diferenca: "Diferença", SemPagamento: "Sem pagamento" };
const LABELS = {
  _id: { revenue: "Grupo", top_products: "Produto", daily: "Dia", companies: "Empresa", documents: "Registro", payments: "Espécie", ticket: "Dia" },
  total: "Total",
  items: "Itens",
  documents: "Documentos",
  participacao: "Participação",
  codigo: "Código",
  ticket: "Ticket médio",
};
const TOTAL_COLUMNS = new Set(["total", "Valor"]);
const NUMBER_COLUMNS = new Set(["items", "documents"]);
const INVESTIGATION_MONEY = new Set(["Itens", "Vendedor", "Outros", "Pagamentos", "Diferenca"]);

const state = {
  kind: "revenue",
  fields: [],
  computed: [],
  collections: [],
  rows: [],
  data: null,
  recon: null,
  busy: false,
  pending: false,
  pendingCall: null,
  sort: null,
  auth: null,
  maxRows: 500,
  autoTimer: null,
  investigation: null,
};

const today = () => {
  const now = new Date();
  return new Date(now.getFullYear(), now.getMonth(), now.getDate());
};
const pad = (value) => String(value).padStart(2, "0");
const iso = (date) => `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
const parseISO = (value) => new Date(`${value}T12:00:00`);
const fmtDate = (value) => (value ? parseISO(value).toLocaleDateString("pt-BR") : "—");
const fmtDateTime = (value) => {
  if (!value) return "";
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? String(value) : date.toLocaleString("pt-BR", { dateStyle: "short", timeStyle: "short" });
};
const fmtMoney = (value) => Number(value || 0).toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
const fmtNumber = (value) => Number(value || 0).toLocaleString("pt-BR");
const esc = (value) => String(value ?? "").replace(/[&<>"']/g, (char) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[char]));
const isNumeric = (value) => typeof value === "number" && Number.isFinite(value);
const plural = (value, singular, pluralForm) => `${fmtNumber(value)} ${Number(value) === 1 ? singular : pluralForm}`;
const EMPTY_ICON = '<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 6h16M4 12h10M4 18h7"/></svg>';
const emptyState = (title, hint) => `<div class="empty-state">${EMPTY_ICON}<strong>${esc(title)}</strong><span>${esc(hint)}</span></div>`;

function toast(message) {
  const element = $("#toast");
  element.textContent = message;
  element.classList.add("show");
  clearTimeout(toast.timer);
  toast.timer = setTimeout(() => element.classList.remove("show"), 3600);
}

/* ---------- authentication ---------- */
function requestAuth() {
  return new Promise((resolve, reject) => {
    const dialog = $("#authDialog");
    const form = $("#authForm");
    form.onsubmit = (event) => {
      event.preventDefault();
      const raw = `${$("#authUser").value}:${$("#authPassword").value}`;
      state.auth = `Basic ${btoa(unescape(encodeURIComponent(raw)))}`;
      dialog.close();
      resolve(state.auth);
    };
    $("#authCancel").onclick = () => {
      dialog.close();
      reject(new Error("Autenticação cancelada."));
    };
    dialog.showModal();
    $("#authUser").focus();
  });
}

async function api(path, options = {}) {
  const send = () => fetch(path, {
    ...options,
    headers: {
      ...(options.body ? { "Content-Type": "application/json" } : {}),
      ...(state.auth ? { Authorization: state.auth } : {}),
      ...(options.headers || {}),
    },
  });
  let response = await send();
  let unauthorized = false;
  if (response.status === 401) {
    state.auth = null;
    if (options._authRetry) {
      unauthorized = true;
    } else {
      await requestAuth();
      response = await send();
      unauthorized = response.status === 401;
    }
  }
  let data = {};
  try { data = await response.json(); } catch { /* non-JSON error */ }
  if (!response.ok) {
    if (unauthorized) throw new Error("Usuário ou senha inválidos.");
    let detail = data.detail;
    if (Array.isArray(detail)) {
      detail = detail.map((item) => `${(item.loc || []).slice(1).join(".")}: ${item.msg}`).join("; ");
    }
    throw new Error(detail || `Erro ${response.status} ao consultar o servidor.`);
  }
  return data;
}

/* ---------- date ranges ---------- */
function presetRange(name) {
  const end = today();
  if (name === "hoje") return { start: iso(end), end: iso(end) };
  if (name === "ontem") {
    const value = new Date(end); value.setDate(value.getDate() - 1);
    return { start: iso(value), end: iso(value) };
  }
  if (name === "7d") {
    const value = new Date(end); value.setDate(value.getDate() - 6);
    return { start: iso(value), end: iso(end) };
  }
  if (name === "mes") return { start: iso(new Date(end.getFullYear(), end.getMonth(), 1)), end: iso(end) };
  if (name === "mespassado") {
    const first = new Date(end.getFullYear(), end.getMonth() - 1, 1);
    const last = new Date(end.getFullYear(), end.getMonth(), 0);
    return { start: iso(first), end: iso(last) };
  }
  if (name === "ano") return { start: iso(new Date(end.getFullYear(), 0, 1)), end: iso(end) };
  return { start: iso(end), end: iso(end) };
}

function setRange(start, end, preset) {
  $("#start").value = start;
  $("#end").value = end;
  $$("#presets button").forEach((button) => button.classList.toggle("active", button.dataset.range === preset));
  $("#rangeLabel").textContent = `${fmtDate(start)} — ${fmtDate(end)}`;
}

function markCustomRange() {
  $$("#presets button").forEach((button) => button.classList.remove("active"));
  $("#rangeLabel").textContent = `${fmtDate($("#start").value)} — ${fmtDate($("#end").value)}`;
}

/* ---------- setup / status ---------- */
function setStatus(mode, title, subtitle) {
  $("#status").className = `status ${mode}`;
  $("#statusText").textContent = title;
  $("#statusSub").textContent = subtitle;
}

function showSetup(html) {
  const banner = $("#setupBanner");
  banner.innerHTML = html;
  banner.classList.remove("hidden");
}

function hideSetup() {
  $("#setupBanner").classList.add("hidden");
}

function renderStatus(data) {
  state.maxRows = Number(data.max_rows) || 500;
  $("#maxRows").textContent = fmtNumber(state.maxRows);
  const backup = $("#backupNote");
  if (data.backup) {
    backup.textContent = `Backup mais recente: ${data.backup.name} · ${new Date(data.backup.modified).toLocaleString("pt-BR")}. O painel não abre o ZIP; restaure em uma instância isolada para consultar.`;
    backup.classList.remove("hidden");
  }
  if (data.connected) {
    setStatus("online", "MongoDB conectado", `${data.database || "selecione o banco"} · somente leitura`);
    hideSetup();
  } else {
    setStatus("error", "Sem conexão com o MongoDB", "verifique o serviço e o .env");
    showSetup(`<b>Não foi possível conectar ao MongoDB</b>
      <p>${esc(data.connection_message || "O servidor não respondeu.")}</p>
      <p>Confira <code>MONGO_URI</code> e as credenciais no <code>.env</code>, inicie o serviço do MongoDB e use ↻ para tentar de novo.</p>`);
  }
}

/* ---------- schema mapping ---------- */
function fillSelect(select, items, placeholder) {
  select.innerHTML = `<option value="">${placeholder}</option>` +
    items.map((item) => `<option value="${esc(item.value)}">${esc(item.label)}</option>`).join("");
}

function loadSchema() {
  return api(`/api/schema/${encodeURIComponent($("#collection").value)}`).then((data) => {
    state.fields = data.fields || [];
    state.computed = data.computed || [];
    const real = state.fields.map((field) => ({ value: field.name, label: field.name }));
    const computed = state.computed.map((field) => ({ value: field.name, label: field.label }));
    fillSelect($("#dateField"), real, "Selecione o campo de data");
    fillSelect($("#valueField"), [...computed, ...real], "Não mapeado");
    fillSelect($("#productField"), real, "Não mapeado");
    fillSelect($("#companyField"), real, "Não mapeado");

    const suggested = data.suggested || {};
    ["date", "value", "product", "company"].forEach((key) => {
      const target = { date: "#dateField", value: "#valueField", product: "#productField", company: "#companyField" }[key];
      if (suggested[key]) $(target).value = suggested[key];
    });
    resetScopeDefaults();
    $("#mapNote").textContent = `${plural(data.sampled, "documento amostrado", "documentos amostrados")} · ${plural(state.fields.length, "campo identificado", "campos identificados")}. Revise o mapeamento antes de usar os números como oficiais.`;
    loadCompanyValues();
  });
}

function resetScopeDefaults() {
  const names = new Set(state.fields.map((field) => field.name));
  const hasTypes = names.has("_t");
  const hasStatus = names.has("Situacao.Descricao");
  const select = $("#movementGroup");
  const previous = select.value;
  const fallback = $("#collection").value === "Movimentacoes" ? "all_sales" : "all";
  select.disabled = !hasTypes;
  select.value = hasTypes ? (MOVEMENT_LABELS[previous] && previous !== "all_sales" ? previous : fallback) : "all";
  $("#onlyValid").disabled = !hasStatus;
  $("#onlyValid").checked = hasStatus;
  $("#movementHint").textContent = hasTypes
    ? (MOVEMENT_HINTS[select.value] || "")
    : "Campo de tipo (_t) não encontrado nesta coleção; o escopo de movimentação fica desativado.";
  $("#validHint").textContent = hasStatus ? "Exclui cancelados, rejeitados e inutilizados." : "Campo de situação não encontrado nesta coleção.";
}

async function loadCompanyValues() {
  const field = $("#companyField").value;
  const list = $("#companyOptions");
  if (!field || !$("#collection").value) { list.innerHTML = ""; return; }
  try {
    const data = await api(`/api/values/${encodeURIComponent($("#collection").value)}?field=${encodeURIComponent(field)}`);
    list.innerHTML = (data.values || []).map((value) => `<option value="${esc(value)}"></option>`).join("");
  } catch {
    list.innerHTML = "";
  }
}

/* ---------- payload + execution ---------- */
function payload() {
  return {
    collection: $("#collection").value,
    start: $("#start").value,
    end: $("#end").value,
    date_field: $("#dateField").value,
    value_field: $("#valueField").value || null,
    product_field: $("#productField").value || null,
    company_field: $("#companyField").value || null,
    company_value: $("#companyValue").value.trim() || null,
    only_valid: $("#onlyValid").checked,
    only_sales: false,
    movement_group: $("#movementGroup").value || null,
    limit: state.maxRows,
  };
}

function validate(request, kind) {
  if (!request.collection) return "Selecione uma coleção.";
  if (!request.date_field) return "Selecione o campo de data.";
  if (!request.start || !request.end) return "Informe o período inicial e final.";
  if (request.end < request.start) return "A data final precisa ser igual ou posterior à inicial.";
  if (kind === "revenue" && !request.value_field) return "Mapeie o campo de valor para o faturamento.";
  if (kind === "top_products" && !request.product_field) return "Mapeie o campo de produto para o ranking.";
  if (kind === "companies" && !request.company_field) return "Mapeie o campo de empresa.";
  if (kind === "ticket" && !request.value_field) return "Mapeie o campo de valor para o ticket médio.";
  return null;
}

function openFilters() {
  $("#filtersPanel").classList.remove("hidden");
  $("#filtersToggle").setAttribute("aria-expanded", "true");
}

function setBusy(busy) {
  $("#askBtn").classList.toggle("running", busy);
  $("#askBtn").disabled = busy;
  $("#runSelectedBtn").disabled = busy;
  $("#applyBtn").disabled = busy;
  if (busy) $("#reconBtn").disabled = true;
  $("#askForm").setAttribute("aria-busy", String(busy));
}

function runPending() {
  if (!state.pending) return;
  const call = state.pendingCall;
  state.pending = false;
  state.pendingCall = null;
  execute(call?.question ?? null, call?.options || {});
}

function clearBoxes() {
  ["#warnings", "#infoBox", "#errorBox"].forEach((selector) => {
    $(selector).classList.add("hidden");
    $(selector).innerHTML = "";
  });
  $("#interpretation").classList.add("hidden");
  $("#interpretation").innerHTML = "";
}

function showLoading() {
  $("#investigationDetails").classList.add("hidden");
  $("#investigationDetails").innerHTML = "";
  clearBoxes();
  clearRecon();
  $("#resultado").setAttribute("aria-busy", "true");
  $("#table").setAttribute("aria-busy", "true");
  $("#resultTitle").textContent = "Consultando…";
  $("#resultNote").textContent = "Executando agregação somente leitura no MongoDB.";
  $("#summary").innerHTML = '<div class="skeleton" style="width:100%"></div>';
  $("#chart").classList.add("hidden");
  $("#table thead").innerHTML = "";
  $("#table tbody").innerHTML = [92, 76, 84, 61].map((width) =>
    `<tr class="skeleton-row" aria-hidden="true"><td><span class="skeleton-line" style="width:${width}%"></span></td></tr>`).join("");
  $("#tableFoot").classList.add("hidden");
  $("#exportBtn").disabled = true;
}

function showError(message) {
  clearBoxes();
  clearRecon();
  $("#resultado").setAttribute("aria-busy", "false");
  $("#table").setAttribute("aria-busy", "false");
  $("#resultTitle").textContent = "Não foi possível gerar";
  $("#resultNote").textContent = "A consulta não foi concluída.";
  const box = $("#errorBox");
  box.innerHTML = `<b>Erro na consulta</b><p>${esc(message)}</p><p>Confira o período, o mapeamento dos campos e a conexão com o banco.</p>`;
  box.classList.remove("hidden");
  $("#summary").innerHTML = "";
  $("#chart").classList.add("hidden");
  $("#table thead").innerHTML = "";
  $("#table tbody").innerHTML = `<tr><td class="empty">${emptyState("Nenhum resultado para mostrar", "Corrija os filtros e execute a consulta novamente.")}</td></tr>`;
  $("#tableFoot").classList.add("hidden");
  $("#exportBtn").disabled = true;
}

async function execute(question = null, options = {}) {
  if (state.busy) {
    state.pending = true;
    state.pendingCall = { question, options };
    return;
  }
  const request = payload();
  const kind = question ? (state.kind || "revenue") : state.kind;
  const problem = validate(request, question ? null : kind);
  if (problem) {
    toast(problem);
    if (!options.auto) openFilters();
    return;
  }
  state.busy = true;
  setBusy(true);
  showLoading();
  try {
    const data = question
      ? await api("/api/ask", { method: "POST", body: JSON.stringify({ ...request, question }) })
      : state.investigation
        ? await api("/api/investigation", { method: "POST", body: JSON.stringify({ ...request, ...state.investigation }) })
        : await api("/api/report", { method: "POST", body: JSON.stringify({ ...request, kind }) });
    state.data = data;
    state.investigation = data.investigation ? {
      seller_query: data.investigation.seller_reference || data.investigation.seller_query,
      target_amount: data.investigation.target_amount,
    } : null;
    state.rows = data.rows || [];
    if (data.interpretation) state.kind = data.interpretation.kind;
    selectKind(state.kind);
    if (data.interpretation?.period_source === "pergunta") {
      setRange(data.period.start, data.period.end, null);
    }
    if (data.interpretation?.company) $("#companyValue").value = data.interpretation.company;
    if (data.interpretation && !data.interpretation.company && /todas?\s+as\s+empresas/i.test(question || "")) {
      $("#companyValue").value = "";
    }
    state.sort = null;
    render(data);
    if (!options.auto) hideSetup();
    if (question) scrollToResults();
  } catch (error) {
    showError(error.message);
    if (options.auto) showSetup(`<b>A consulta inicial falhou</b><p>${esc(error.message)}</p><p>Revise os filtros em <strong>Filtros avançados</strong>.</p>`);
  } finally {
    state.busy = false;
    setBusy(false);
    runPending();
  }
}

function scrollToResults() {
  const target = $("#resultado");
  if (target.getBoundingClientRect().top > window.innerHeight - 160) {
    target.scrollIntoView({ behavior: "smooth", block: "start" });
  }
}

/* ---------- rendering ---------- */
function selectKind(kind) {
  $$(".report-card").forEach((card) => {
    const selected = card.dataset.kind === kind;
    card.classList.toggle("selected", selected);
    card.setAttribute("aria-pressed", String(selected));
  });
}

function metric(label, value, emphasis = false, tone = "") {
  return `<div class="metric ${emphasis ? "emphasis" : ""} ${tone}"><small>${esc(label)}</small><strong>${esc(value)}</strong></div>`;
}

function renderInterpretation(data) {
  const parts = data.interpretation?.summary || [];
  const chips = [];
  parts.forEach((part) => chips.push(`<span class="chip accent">${esc(part)}</span>`));
  if (!parts.length) {
    chips.push(`<span class="chip accent">${esc(KIND_LABELS[state.kind])}</span>`);
    chips.push(`<span class="chip">${fmtDate(data.period.start)} — ${fmtDate(data.period.end)}</span>`);
    if ($("#companyValue").value.trim()) chips.push(`<span class="chip">Empresa: ${esc($("#companyValue").value.trim())}</span>`);
  }
  const scopeAlreadyShown = parts.some((part) => part.startsWith("Escopo:"));
  if (!scopeAlreadyShown && data.scope?.movement_label) {
    chips.push(`<span class="chip">Escopo: ${esc(data.scope.movement_label)}</span>`);
  }
  if (data.interpretation) {
    chips.push(`<span class="chip" title="${esc(data.interpretation.method)}">Classificação: ${/modelo/i.test(data.interpretation.method) ? "IA" : "regras locais"}</span>`);
  }
  if (data.scope && data.scope.excluded > 0) {
    chips.push(`<span class="chip">${plural(data.scope.excluded, "documento", "documentos")} fora do escopo</span>`);
  }
  const box = $("#interpretation");
  box.innerHTML = chips.join("");
  box.classList.remove("hidden");
}

function render(data) {
  const rows = data.rows || [];
  const kind = state.kind;
  clearBoxes();
  $("#resultado").setAttribute("aria-busy", "false");
  $("#table").setAttribute("aria-busy", "false");
  $("#resultTitle").textContent = KIND_LABELS[kind];
  $("#resultNote").textContent = `${fmtDate(data.period.start)} a ${fmtDate(data.period.end)} · ${plural(data.documents, "documento", "documentos")} no escopo · ${plural(rows.length, "linha", "linhas")}`;
  renderInterpretation(data);
  if (data.investigation) {
    renderInvestigation(data);
    return;
  }

  const warnings = data.warnings || [];
  if (warnings.length) {
    $("#warnings").innerHTML = `<b>Atenção ao resultado</b>${warnings.map((warning) => `<p>${esc(warning)}</p>`).join("")}`;
    $("#warnings").classList.remove("hidden");
  }

  const notes = [];
  if (data.value_note) notes.push(data.value_note);
  if (data.payments_note) notes.push(data.payments_note);
  if (data.ticket_note) notes.push(data.ticket_note);
  if (data.scope && data.scope.excluded > 0) {
    const scopeLabel = data.scope.movement_label || "escopo selecionado";
    const validText = data.scope.only_valid ? "; somente documentos válidos" : "";
    notes.push(`${plural(data.scope.excluded, "documento", "documentos")} do período ficaram fora do escopo selecionado e não entram nos totais. Escopo: ${scopeLabel}${validText}.`);
  }
  const total = rows.reduce((sum, row) => sum + (isNumeric(row.total) ? row.total : 0), 0);
  if (kind === "revenue" && isNumeric(data.payments_total) && rows.length) {
    const difference = total - data.payments_total;
    const base = Math.max(Math.abs(data.payments_total), 1);
    if (Math.abs(difference) / base > 0.01) {
      notes.push(`Conferência: os pagamentos registrados somam ${fmtMoney(data.payments_total)}, diferença de ${fmtMoney(difference)} em relação aos itens. Verifique documentos sem pagamento registrado, devoluções, descontos ou acréscimos.`);
    }
  }
  if (notes.length) {
    $("#infoBox").innerHTML = `<b>Como o número foi calculado</b>${notes.map((note) => `<p>${esc(note)}</p>`).join("")}`;
    $("#infoBox").classList.remove("hidden");
  }

  const metrics = [];
  const itemSum = rows.reduce((sum, row) => sum + (isNumeric(row.items) ? row.items : 0), 0);
  if (kind === "revenue") {
    metrics.push(metric("Faturamento", fmtMoney(rows[0]?.total || 0), true));
    if (isNumeric(data.payments_total)) metrics.push(metric("Recebido (pagamentos)", fmtMoney(data.payments_total)));
    metrics.push(metric("Itens", fmtNumber(rows[0]?.items || 0)));
    metrics.push(metric("Documentos", fmtNumber(data.documents)));
  } else if (kind === "top_products") {
    metrics.push(metric("Produtos", fmtNumber(rows.length), true));
    metrics.push(metric("Itens vendidos", fmtNumber(itemSum)));
    if (data.value_field) metrics.push(metric("Total dos itens", fmtMoney(total)));
    metrics.push(metric("Documentos", fmtNumber(data.documents)));
  } else if (kind === "daily") {
    metrics.push(metric("Total do período", data.value_field ? fmtMoney(total) : fmtNumber(itemSum), true));
    const days = rows.length || 1;
    metrics.push(metric("Média por dia com venda", data.value_field ? fmtMoney(total / days) : fmtNumber(itemSum / days)));
    metrics.push(metric("Documentos", fmtNumber(data.documents)));
    metrics.push(metric("Dias com movimento", fmtNumber(rows.length)));
  } else if (kind === "companies") {
    metrics.push(metric("Empresas", fmtNumber(rows.length), true));
    if (data.value_field) metrics.push(metric("Total dos itens", fmtMoney(total)));
    metrics.push(metric("Itens", fmtNumber(itemSum)));
    metrics.push(metric("Documentos", fmtNumber(data.documents)));
  } else if (kind === "payments") {
    metrics.push(metric("Total recebido", fmtMoney(data.payments_total), true));
    metrics.push(metric("Espécies", fmtNumber(rows.length)));
    metrics.push(metric("Lançamentos", fmtNumber(data.payment_entries)));
    metrics.push(metric("Documentos com pagamento", fmtNumber(data.documents_with_payment)));
  } else if (kind === "ticket") {
    metrics.push(metric("Ticket médio", fmtMoney(data.ticket_total), true));
    metrics.push(metric("Total dos itens", fmtMoney(total)));
    metrics.push(metric("Documentos", fmtNumber(data.documents)));
    metrics.push(metric("Dias com movimento", fmtNumber(rows.length)));
  } else {
    metrics.push(metric("Documentos listados", fmtNumber(rows.length), true));
    metrics.push(metric("No período / escopo", fmtNumber(data.documents)));
    const listed = rows.reduce((sum, row) => sum + (isNumeric(row.Valor) ? row.Valor : 0), 0);
    if (data.value_field) metrics.push(metric("Soma dos listados", fmtMoney(listed)));
  }
  $("#summary").innerHTML = metrics.join("");
  renderChart(rows);
  renderTable(rows);
  $("#exportBtn").disabled = !rows.length;
  prepareRecon(data);
}

function renderInvestigation(data) {
  const report = data.investigation;
  const totals = report.totals;
  $("#resultTitle").textContent = "Investigação de vendas";
  $("#summary").innerHTML = [
    metric("Vendas encontradas", fmtNumber(data.documents), true),
    metric("Itens dos documentos", fmtMoney(totals.Itens)),
    ...(report.seller ? [metric("Itens do vendedor", fmtMoney(totals.Vendedor)),
      metric("Outros / sem vendedor", fmtMoney(totals.Outros), false, Math.abs(totals.Outros) > 0.01 ? "warn" : "")] : []),
    metric("Pagamentos registrados", fmtMoney(totals.Pagamentos)),
    metric("Itens − pagamentos", fmtMoney(totals.Diferenca), false, Math.abs(totals.Diferenca) > 0.01 ? "warn" : ""),
  ].join("");
  $("#infoBox").innerHTML = `<b>O que a conferência encontrou</b>${report.findings.map(text => `<p>${esc(text)}</p>`).join("")}`;
  $("#infoBox").classList.remove("hidden");
  if (data.warnings?.length) {
    $("#warnings").innerHTML = `<b>Atenção ao resultado</b>${data.warnings.map(text => `<p>${esc(text)}</p>`).join("")}`;
    $("#warnings").classList.remove("hidden");
  }
  $("#chart").classList.add("hidden");
  clearRecon();
  renderTable(data.rows);
  $("#tableFoot").textContent = `${data.rows.length} de ${data.documents} vendas listadas · totais calculados sobre todas as vendas encontradas`;
  const box = $("#investigationDetails");
  box.innerHTML = `<h3>Itens de cada venda</h3><p class="muted">Abra uma venda para conferir produtos, quantidades, vendedores e valores. Itens cancelados ficam fora dos cálculos.</p>` + report.details.map(doc =>
    `<details class="sale-detail"><summary>Venda ${esc(doc.document)} · ${doc.items.length} itens</summary><div class="table-wrap"><table><thead><tr><th>Item</th><th>Produto</th><th>Quantidade</th><th>Vendedor</th><th>Valor</th></tr></thead><tbody>${doc.items.map(item =>
      `<tr><td>${esc(item.Numero ?? "")}</td><td>${esc(item.Produto ?? "")}</td><td class="num">${fmtNumber(item.Quantidade || 0)}</td><td>${esc(item.Vendedor)}</td><td class="num">${fmtMoney(item.Valor)}</td></tr>`).join("")}</tbody></table></div></details>`).join("");
  box.classList.remove("hidden");
  $("#exportBtn").disabled = !data.rows.length;
}

function renderChart(rows) {
  const chart = $("#chart");
  chart.innerHTML = "";
  if (!rows.length || state.kind === "revenue" || state.kind === "documents") {
    chart.classList.add("hidden");
    return;
  }
  const valueField = state.data?.value_field;
  const metricKey = state.kind === "payments" ? "total"
    : state.kind === "ticket" ? "ticket"
    : (valueField && rows.some((row) => isNumeric(row.total)) ? "total" : "items");
  const format = (metricKey === "total" || metricKey === "ticket") ? fmtMoney : fmtNumber;

  if (state.kind === "daily" || state.kind === "ticket") {
    const max = Math.max(...rows.map((row) => Number(row[metricKey]) || 0), 1);
    const step = Math.ceil(rows.length / 12);
    const bars = rows.map((row, index) => {
      const value = Number(row[metricKey]) || 0;
      const height = Math.max((value / max) * 100, 2);
      const label = index % step === 0 || index === rows.length - 1 ? fmtDate(row._id).slice(0, 5) : "";
      return `<div class="bar-col" title="${esc(fmtDate(row._id))}: ${esc(format(value))}">
        <span class="tip">${esc(format(value))}</span>
        <div class="bar" style="height:${height}%"></div>
        <span>${esc(label)}</span>
      </div>`;
    }).join("");
    const title = state.kind === "ticket" ? "Ticket médio por dia"
      : (metricKey === "total" ? "Total por dia" : "Itens por dia");
    chart.innerHTML = `<p class="chart-title">${title}</p><div class="bars">${bars}</div>`;
  } else {
    const top = [...rows].sort((a, b) => (Number(b[metricKey]) || 0) - (Number(a[metricKey]) || 0)).slice(0, 12);
    const max = Math.max(...top.map((row) => Number(row[metricKey]) || 0), 1);
    const bars = top.map((row) => {
      const value = Number(row[metricKey]) || 0;
      return `<div class="hbar" title="${esc(row._id)}: ${esc(format(value))}">
        <span class="label">${esc(row._id ?? "—")}</span>
        <span class="track"><span class="fill" style="width:${Math.max((value / max) * 100, 2)}%"></span></span>
        <span class="value">${esc(format(value))}</span>
      </div>`;
    }).join("");
    chart.innerHTML = `<p class="chart-title">${state.kind === "companies" ? "Empresas com maior movimento" : state.kind === "payments" ? "Formas de pagamento" : "Produtos com maior movimento"}</p><div class="hbars">${bars}</div>`;
  }
  chart.classList.remove("hidden");
}

/* ---------- reconciliation (items vs payments) ---------- */
function clearRecon() {
  state.recon = null;
  $("#reconBox").classList.add("hidden");
  $("#reconBox").setAttribute("aria-busy", "false");
  $("#reconNote").textContent = "";
  $("#reconSummary").innerHTML = "";
  $("#reconWarnings").classList.add("hidden");
  $("#reconWarnings").innerHTML = "";
  ["#reconByType", "#reconDetail"].forEach((selector) => {
    $(`${selector} thead`).innerHTML = "";
    $(`${selector} tbody`).innerHTML = "";
  });
  $("#reconByTypeWrap").classList.add("hidden");
  $("#reconDetailWrap").classList.add("hidden");
  $("#reconFoot").classList.add("hidden");
  $("#reconFoot").textContent = "";
  $("#reconBtn").disabled = false;
  $("#reconBtn").textContent = "Detalhar diferenças";
}

function prepareRecon(data) {
  clearRecon();
  if (state.kind !== "revenue") return;
  const total = (data.rows || []).reduce((sum, row) => sum + (isNumeric(row.total) ? row.total : 0), 0);
  const note = isNumeric(data.payments_total)
    ? `Itens ${fmtMoney(total)} · pagamentos ${fmtMoney(data.payments_total)} · diferença ${fmtMoney(total - data.payments_total)}.`
    : "Não há total de pagamentos para este escopo; a conferência mostra os documentos sem pagamento registrado.";
  $("#reconNote").textContent = note;
  $("#reconBtn").disabled = !data.documents;
  $("#reconBox").classList.remove("hidden");
}

async function loadReconciliation() {
  if (state.busy || state.kind !== "revenue" || !state.data) return;
  const request = payload();
  const problem = validate(request, "revenue");
  if (problem) {
    toast(problem);
    return;
  }
  state.busy = true;
  setBusy(true);
  $("#reconBox").setAttribute("aria-busy", "true");
  $("#reconBtn").disabled = true;
  $("#reconBtn").textContent = "Conferindo…";
  try {
    const data = await api("/api/reconciliation", {
      method: "POST",
      body: JSON.stringify({ ...request, limit: state.maxRows }),
    });
    state.recon = data;
    renderReconciliation(data);
  } catch (error) {
    $("#reconWarnings").innerHTML = `<b>Não foi possível conferir</b><p>${esc(error.message)}</p>`;
    $("#reconWarnings").classList.remove("hidden");
  } finally {
    state.busy = false;
    setBusy(false);
    $("#reconBox").setAttribute("aria-busy", "false");
    $("#reconBtn").disabled = false;
    $("#reconBtn").textContent = state.recon ? "Atualizar conferência" : "Detalhar diferenças";
    runPending();
  }
}

function reconCell(key, value) {
  if (key === "Data") return fmtDateTime(value);
  if (key === "Tipo") return TYPE_LABELS[value] || value || "Sem tipo";
  if (RECON_MONEY.has(key)) return fmtMoney(value);
  if (RECON_NUMBER.has(key)) return fmtNumber(value);
  if (key === "SemPagamento") return value ? "Sim" : "Não";
  return value == null ? "" : String(value);
}

function renderReconTable(table, rows, columns, labels, emptyTitle, emptyHint = "") {
  table.querySelector("thead").innerHTML = `<tr>${columns.map((key) =>
    `<th scope="col">${esc(labels[key] || fieldLabel(key))}</th>`).join("")}</tr>`;
  if (!rows.length) {
    table.querySelector("tbody").innerHTML = `<tr><td class="empty" colspan="${columns.length}">${emptyState(emptyTitle, emptyHint)}</td></tr>`;
    return;
  }
  table.querySelector("tbody").innerHTML = rows.map((row) => `<tr>${columns.map((key) => {
    const numeric = RECON_MONEY.has(key) || RECON_NUMBER.has(key);
    const text = reconCell(key, row[key]);
    const title = text.length > 40 ? ` title="${esc(text)}"` : "";
    return `<td class="${numeric ? "num" : ""}"${title}>${esc(text)}</td>`;
  }).join("")}</tr>`).join("");
}

function renderReconciliation(data) {
  const metrics = [
    metric("Itens", fmtMoney(data.items_total), true),
    metric("Pagamentos", fmtMoney(data.payments_total)),
    metric("Diferença", fmtMoney(data.difference), false, Math.abs(data.difference) > 0.01 ? "warn" : ""),
    metric("Sem pagamento", fmtNumber(data.documents_without_payment), false, data.documents_without_payment > 0 ? "warn" : ""),
  ];
  $("#reconSummary").innerHTML = metrics.join("");

  const warnings = data.warnings || [];
  if (warnings.length) {
    $("#reconWarnings").innerHTML = `<b>O que explica a diferença</b>${warnings.map((warning) => `<p>${esc(warning)}</p>`).join("")}`;
    $("#reconWarnings").classList.remove("hidden");
  } else {
    $("#reconWarnings").classList.add("hidden");
  }

  const byType = data.by_type || [];
  $("#reconByTypeWrap").classList.toggle("hidden", !byType.length);
  if (byType.length) {
    renderReconTable($("#reconByType"), byType,
      ["Tipo", "documents", "items", "payments", "difference", "without_payment"],
      RECON_TYPE_LABELS, "Nenhum tipo de movimentação no escopo.",
      "Ajuste o período ou o escopo para ampliar a conferência.");
  }

  const detail = data.rows || [];
  const columns = ["Data", "Numero", "Serie", "Empresa", "Tipo", "Situacao", "Itens", "Pagamentos", "Diferenca", "SemPagamento"]
    .filter((key) => detail.some((row) => key in row));
  $("#reconDetailWrap").classList.remove("hidden");
  renderReconTable($("#reconDetail"), detail, columns.length ? columns : ["Data", "Itens", "Pagamentos", "Diferenca"],
    RECON_DOC_LABELS, "Nenhuma diferença acima de R$ 0,01",
    "Itens e pagamentos estão conciliados neste recorte.");

  $("#reconFoot").textContent = data.truncated
    ? `Mostrando os ${fmtNumber(detail.length)} maiores desvios de ${fmtNumber(data.difference_count)}. Refine o período para ver todos.`
    : `${plural(data.difference_count, "documento", "documentos")} com diferença acima de R$ 0,01.`;
  $("#reconFoot").classList.remove("hidden");
}

function columnsFor(rows) {
  const columns = [];
  rows.forEach((row) => Object.keys(row).forEach((key) => {
    if (!columns.includes(key)) columns.push(key);
  }));
  return columns;
}

function fieldLabel(key) {
  const known = {
    Itens: "Itens do documento",
    Vendedor: "Itens do vendedor",
    Outros: "Itens de outros / sem vendedor",
    Pagamentos: "Pagamentos do documento",
    Diferenca: "Itens − pagamentos",
    Observacao: "Conferência",
    Data: "Emissão",
    DataHoraEmissao: "Emissão",
    DataEmissao: "Emissão",
    Numero: "Número",
    Serie: "Série",
    Situacao: "Situação",
    Empresa: "Empresa",
    Produto: "Produto",
    Produtos: "Produtos",
    Valor: "Valor",
    "ItensBase.TotalLiquido": "Total líquido (nativo)",
    "ItensBase.TotalBruto": "Total bruto",
    "Empresa.Nome": "Empresa",
    "ItensBase.ProdutoServico.Descricao": "Produto",
    ChaveAcesso: "Chave de acesso",
    "Situacao.Descricao": "Situação",
  };
  if (known[key]) return known[key];
  const last = key.split(".").pop();
  return last.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ");
}

function columnLabel(key) {
  if (key === "_id") return LABELS._id[state.kind] || "Grupo";
  if (LABELS[key]) return LABELS[key];
  return fieldLabel(key);
}

function listText(value) {
  const items = value.map((item) => (item && typeof item === "object" ? (item.Descricao ?? JSON.stringify(item)) : item))
    .filter((item) => item != null && item !== "");
  return [...new Set(items)].join(", ");
}

function formatCell(key, value) {
  if (value == null) return "";
  if (state.data?.investigation && INVESTIGATION_MONEY.has(key)) return fmtMoney(value);
  if (key === "Tipo") return TYPE_LABELS[value] || value;
  if (key === "total" || key === "Valor") return (state.data?.value_field || state.kind === "payments") ? fmtMoney(value) : fmtNumber(value);
  if (key === "ticket") return fmtMoney(value);
  if (key === "participacao") return `${Number(value || 0).toFixed(2).replace(".", ",")}%`;
  if (NUMBER_COLUMNS.has(key)) return fmtNumber(value);
  if (key === "_id" && (state.kind === "daily" || state.kind === "ticket")) return fmtDate(value);
  if (key === "Data") return fmtDateTime(value);
  if (Array.isArray(value)) return listText(value);
  if (typeof value === "object") return JSON.stringify(value);
  return String(value);
}

function compareValues(left, right, key) {
  if (key === "_id" && (state.kind === "daily" || state.kind === "ticket")) return String(left || "").localeCompare(String(right || ""));
  if (isNumeric(left) && isNumeric(right)) return left - right;
  return String(left ?? "").localeCompare(String(right ?? ""), "pt-BR", { numeric: true });
}

function renderTable(rows) {
  const table = $("#table");
  const foot = $("#tableFoot");
  if (!rows.length) {
    table.querySelector("thead").innerHTML = "";
    table.querySelector("tbody").innerHTML = `<tr><td class="empty">${emptyState(
      "Nenhum registro para esses filtros",
      "Ajuste o período, a empresa ou o escopo e execute novamente.",
    )}</td></tr>`;
    foot.classList.add("hidden");
    return;
  }
  const columns = columnsFor(rows);
  let sorted = rows;
  if (state.sort) {
    sorted = [...rows].sort((a, b) => compareValues(a[state.sort.key], b[state.sort.key], state.sort.key) * state.sort.dir);
  }
  const head = columns.map((key) => {
    const active = state.sort?.key === key;
    const arrow = active ? (state.sort.dir > 0 ? " ↑" : " ↓") : "";
    return `<th data-key="${esc(key)}" scope="col" tabindex="0" aria-sort="${active ? (state.sort.dir > 0 ? "ascending" : "descending") : "none"}" title="Ordenar por ${esc(columnLabel(key))}">${esc(columnLabel(key))}${arrow}</th>`;
  }).join("");
  const body = sorted.map((row) => `<tr>${columns.map((key) => {
    const raw = row[key];
    const numeric = key === "total" || key === "Valor" || key === "ticket" || NUMBER_COLUMNS.has(key) || (state.data?.investigation && INVESTIGATION_MONEY.has(key));
    const title = raw != null && String(formatCell(key, raw)).length > 40 ? ` title="${esc(formatCell(key, raw))}"` : "";
    return `<td class="${numeric ? "num" : ""}"${title}>${esc(formatCell(key, raw))}</td>`;
  }).join("")}</tr>`).join("");
  table.querySelector("thead").innerHTML = `<tr>${head}</tr>`;
  table.querySelector("tbody").innerHTML = body;
  const sortTableBy = (key) => {
    state.sort = state.sort?.key === key ? { key, dir: state.sort.dir * -1 } : { key, dir: 1 };
    renderTable(state.rows);
    const header = $$("#table thead th").find((item) => item.dataset.key === key);
    header?.focus({ preventScroll: true });
  };
  $$("#table thead th").forEach((th) => {
    th.addEventListener("click", () => sortTableBy(th.dataset.key));
    th.addEventListener("keydown", (event) => {
      if (event.key !== "Enter" && event.key !== " ") return;
      event.preventDefault();
      sortTableBy(th.dataset.key);
    });
  });
  foot.textContent = `${plural(sorted.length, "linha", "linhas")} · até ${fmtNumber(state.maxRows)} linhas por consulta${sorted.length >= state.maxRows ? " · refine o período para ver menos" : ""}`;
  foot.classList.remove("hidden");
}

/* ---------- CSV ---------- */
function csvCell(key, value) {
  if (value == null) return "";
  if (key === "Data") return fmtDateTime(value);
  if (Array.isArray(value)) return listText(value);
  if (isNumeric(value) && key === "ticket") return value.toFixed(2).replace(".", ",");
  if (isNumeric(value) && (key === "total" || key === "Valor") && (state.data?.value_field || state.kind === "payments")) return value.toFixed(2).replace(".", ",");
  if (isNumeric(value) && (key === "total" || key === "Valor")) return String(value).replace(".", ",");
  if (isNumeric(value)) return String(value).replace(".", ",");
  if (typeof value === "object") return JSON.stringify(value);
  return String(value);
}

function csvQuote(value) {
  const text = String(value ?? "").replaceAll('"', '""');
  return /[;"\r\n]/.test(text) ? `"${text}"` : text;
}

function buildCsv() {
  const rows = state.rows;
  if (!rows.length) return "";
  const columns = columnsFor(rows);
  const lines = [columns.map(columnLabel), ...rows.map((row) => columns.map((key) => csvCell(key, row[key])))];
  return "\ufeff" + lines.map((line) => line.map(csvQuote).join(";")).join("\r\n");
}

function exportCsv() {
  const rows = state.rows;
  if (!rows.length) return;
  const csv = buildCsv();
  const blob = new Blob([csv], { type: "text/csv;charset=utf-8" });
  const link = document.createElement("a");
  link.href = URL.createObjectURL(blob);
  link.download = `digisat-${state.kind}-${$("#start").value}-a-${$("#end").value}.csv`;
  document.body.appendChild(link);
  link.click();
  link.remove();
  setTimeout(() => URL.revokeObjectURL(link.href), 1500);
  toast("CSV exportado.");
}

/* ---------- init and events ---------- */
async function loadWorkspace(data) {
  try {
    const collections = await api("/api/collections");
    state.collections = collections;
    const collection = $("#collection");
    collection.innerHTML = collections.map((item) => `<option value="${esc(item.name)}">${esc(item.name)} · ${fmtNumber(item.count)}</option>`).join("");
    if (!collections.length) {
      setStatus("error", "Base conectada, sem coleções", data.database || "");
      showSetup(`<b>Nenhuma coleção legível</b><p>O banco <code>${esc(data.database || "")}</code> não expôs coleções para esta conta. Confira as permissões de leitura.</p>`);
      return;
    }
    const preferred = collections.find((item) => item.name === "Movimentacoes") || collections[0];
    collection.value = preferred.name;
    setStatus("online", "MongoDB conectado", `${data.database} · ${preferred.name} (${fmtNumber(preferred.count)})`);
    await loadSchema();
    $("#filtersToggle").setAttribute("aria-expanded", "false");
    execute(null, { auto: true });
  } catch (error) {
    showSetup(`<b>Não foi possível carregar a base</b><p>${esc(error.message)}</p>`);
    setStatus("error", "Falha ao carregar", "veja a mensagem acima");
  }
}

async function init() {
  setRange(presetRange("mes").start, presetRange("mes").end, "mes");
  $("#table tbody").innerHTML = `<tr><td class="empty">${emptyState(
    "Pronto para consultar",
    "Escolha um período e um relatório para ver os dados.",
  )}</td></tr>`;
  setStatus("", "Verificando conexão", "conectando ao MongoDB");
  hideSetup();
  try {
    const data = await api("/api/status");
    renderStatus(data);
    if (!data.connected) {
      if (data.connection_message) {
        showSetup(`<b>MongoDB indisponível</b><p>${esc(data.connection_message)}</p><p>Confira <code>MONGO_URI</code> no <code>.env</code> e se o serviço está em execução. Depois use ↻.</p>`);
      }
      $("#table tbody").innerHTML = '<tr><td class="empty">Sem conexão com a base.</td></tr>';
      return;
    }
    if (!data.configured) {
      const databases = (data.databases || []).join(", ") || data.database_list_message || "nenhum banco visível";
      showSetup(`<b>Banco de dados não selecionado</b><p>Bancos visíveis: <code>${esc(databases)}</code></p><p>Defina <code>MONGO_DATABASE</code> no arquivo <code>.env</code> e reinicie o painel.</p>`);
      $("#table tbody").innerHTML = '<tr><td class="empty">Escolha o banco no .env para consultar.</td></tr>';
      return;
    }
    await loadWorkspace(data);
  } catch (error) {
    setStatus("error", "Serviço indisponível", "não foi possível falar com a API");
    showSetup(`<b>Não foi possível falar com o serviço</b><p>${esc(error.message)}</p><p>Confirme se o painel está em execução em <code>http://127.0.0.1:8088</code>.</p>`);
    $("#table tbody").innerHTML = '<tr><td class="empty">Serviço indisponível.</td></tr>';
  }
}

$$("#presets button").forEach((button) => {
  button.addEventListener("click", () => {
    const range = presetRange(button.dataset.range);
    setRange(range.start, range.end, button.dataset.range);
    execute();
  });
});

$("#start").addEventListener("change", () => {
  markCustomRange();
  clearTimeout(state.autoTimer);
  state.autoTimer = setTimeout(() => execute(), 450);
});
$("#end").addEventListener("change", () => {
  markCustomRange();
  clearTimeout(state.autoTimer);
  state.autoTimer = setTimeout(() => execute(), 450);
});

$("#filtersToggle").addEventListener("click", () => {
  const panel = $("#filtersPanel");
  const open = panel.classList.toggle("hidden") === false;
  $("#filtersToggle").setAttribute("aria-expanded", String(open));
});

$("#collection").addEventListener("change", async () => {
  if (!$("#collection").value) return;
  $("#companyValue").value = "";
  await loadSchema();
  execute();
});
$("#companyField").addEventListener("change", () => {
  $("#companyValue").value = "";
  loadCompanyValues();
});
$("#schemaBtn").addEventListener("click", async () => {
  await loadSchema();
  toast("Campos atualizados.");
});
$("#applyBtn").addEventListener("click", () => execute());
$("#runSelectedBtn").addEventListener("click", () => execute());
$("#exportBtn").addEventListener("click", exportCsv);
$("#reconBtn").addEventListener("click", loadReconciliation);
$("#refreshBtn").addEventListener("click", init);

$("#movementGroup").addEventListener("change", () => {
  $("#movementHint").textContent = MOVEMENT_HINTS[$("#movementGroup").value] || "";
  execute();
});
$("#onlyValid").addEventListener("change", () => execute());

$$(".report-card").forEach((card) => {
  card.addEventListener("click", () => {
    state.investigation = null;
    state.kind = card.dataset.kind;
    selectKind(state.kind);
    execute();
  });
});

$("#askForm").addEventListener("submit", (event) => {
  event.preventDefault();
  const question = $("#question").value.trim();
  if (!question) {
    toast("Digite o que deseja consultar.");
    $("#question").focus();
    return;
  }
  execute(question);
});

$$(".suggestions button").forEach((button) => {
  button.addEventListener("click", () => {
    $("#question").value = button.dataset.q;
    execute(button.dataset.q);
  });
});

$$(".rail-btn").forEach((button) => {
  button.addEventListener("click", () => {
    $$(".rail-btn").forEach((item) => item.classList.remove("active"));
    button.classList.add("active");
  });
});

init();
