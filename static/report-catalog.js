'use strict';
(() => {
  const $ = id => document.getElementById(id);
  let catalog;
  const normal = text => String(text).normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();
  function node(tag, text, parent) {
    const el = document.createElement(tag);
    if (text !== undefined) el.textContent = text;
    if (parent) parent.append(el);
    return el;
  }
  function field(list, label, values, fallback = 'Não recuperado por extração literal.') {
    node('dt', label, list);
    node('dd', values.length ? values.join('; ') : fallback, list);
  }
  function render() {
    const query = normal($('search').value.trim());
    const module = $('module').value;
    const reports = catalog.reports.filter(r => (!module || r.module === module) &&
      (!query || normal([r.title, r.module_label, r.module, r.class, ...r.parameters, ...r.bindings].join(' ')).includes(query)));
    $('count').textContent = `${reports.length} de ${catalog.summary.entries} entradas`;
    const fragment = document.createDocumentFragment();
    for (const r of reports) {
      const details = node('details', undefined, fragment);
      const summary = node('summary', r.title, details);
      node('span', `${r.module_label} · ${r.class}`, summary);
      let populated = false;
      details.addEventListener('toggle', () => {
        if (!details.open || populated) return;
        populated = true;
        node('p', r.execution.reason, details).className = 'pending';
        if(r.execution.status === 'implemented_data_view') {
          const link=node('a','Abrir consulta financeira',details);
          link.href='/finance-reports?report='+encodeURIComponent(r.id);
        }
        if(r.native_url){const link=node('a','Consultar no servidor SuiteG6',details);link.href='/native-reports?report='+encodeURIComponent(r.id);link.style.display='block';}
        const list = node('dl', undefined, details);
        field(list, 'Parâmetros nativos', r.parameters);
        field(list, 'Campos apresentados (DTO)', r.bindings);
        field(list, 'Agrupamentos do layout', r.groups);
        field(list, 'Resumos do layout', r.summaries);
        field(list, 'Expressões calculadas', r.expressions);
        field(list, 'Contratos de serviço', r.services);
        field(list, 'Chamadas catalogadas', r.service_calls);
        field(list, 'Pesquisa da regra', [r.coverage]);
        if (r.limitations.length) field(list, 'Limites da pesquisa', r.limitations);
        node('h3', 'Evidências da apresentação', details);
        const sources = node('ul', undefined, details);
        for (const source of r.sources) {
          const li = node('li', source.path, sources);
          node('br', undefined, li);
          node('code', `SHA-256 ${source.sha256}`, li);
        }
        if (!r.sources.length) node('li', 'Classe não localizada nas fontes retidas.', sources);
        if (r.references.length) {
          node('h3', 'Documentação de pesquisa', details);
          const refs = node('ul', undefined, details);
          for (const ref of r.references) node('li', ref, refs);
        }
      });
    }
    $('reports').replaceChildren(fragment);
  }
  async function load() {
    try {
      const response = await fetch('/static/report-catalog.json', {cache: 'no-store'});
      if (!response.ok) throw new Error('Não foi possível carregar o catálogo.');
      catalog = await response.json();
      const s = catalog.summary;
      $('summary').textContent = `${s.entries} entradas · ${s.modules} módulos · ${s.native_service_entries || 0} com serviços nativos · ${s.unavailable_entries || 0} ausentes da instalação`;
      $('limits').textContent = catalog.limitations;
      for (const m of [...catalog.modules].sort((a,b) => a.label.localeCompare(b.label, 'pt-BR'))) {
        const option = node('option', `${m.label} (${m.count})`, $('module'));
        option.value = m.id;
      }
      $('search').addEventListener('input', render);
      $('module').addEventListener('change', render);
      render();
    } catch (err) {
      $('summary').textContent = 'Catálogo indisponível';
      $('error').textContent = err.message;
      $('error').hidden = false;
    }
  }
  load();
})();
