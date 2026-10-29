/* BI is rendered only from authenticated reporting APIs; no embedded sample figures. */
(() => {
  'use strict';
  const $ = id => document.getElementById(id);
  const esc = value => String(value ?? '').replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
  const money = value => value == null ? '—' : new Intl.NumberFormat('pt-BR', {style:'currency',currency:'BRL'}).format(value);
  const count = value => new Intl.NumberFormat('pt-BR',{maximumFractionDigits:0}).format(value);
  const shortDate = value => value ? String(value).slice(0,10).split('-').reverse().join('/') : '—';
  const dayLabel = value => shortDate(value).slice(0,5);
  const iso = d => `${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`;
  const state = {mapping:null,schema:null,models:[],overview:null,exploration:null,visibleRows:[],columns:[],tab:'overview',
    auth:null,authWait:null,drillCompany:null,drillDay:null,sort:null,sortDirection:1,request:0,exploreRequest:0,schemaRequest:0,source:null,
    views:[],activeView:null,viewDirty:false,restoring:false,viewLoad:0,viewPeriodPolicy:null,exploreSignature:null};
  const fieldLabels = {date_field:'mapDate',value_field:'mapValue',company_field:'mapCompany',product_field:'mapProduct'};

  async function api(path, options={}, retry=true) {
    const response = await fetch(path, {...options, headers:{'Content-Type':'application/json',...(state.auth?{Authorization:state.auth}:{}),...options.headers},signal:options.signal || AbortSignal.timeout(150000)});
    if (response.status===401 && retry) {await authenticate();return api(path,options,false);}
    let data;
    try {data=await response.json();} catch (_) {throw new Error('O serviço não retornou uma resposta válida. Confira se o Insight está em execução.');}
    if (!response.ok) {const detail=data.detail;throw new Error(Array.isArray(detail)?detail.map(e=>e.msg).join(' · '):String(detail||'Não foi possível concluir a consulta.'));}
    return data;
  }
  function authenticate() {
    if (state.authWait) return state.authWait;
    state.authWait = new Promise((resolve,reject)=>{
      $('biAuthError').textContent='';$('biAuth').showModal();
      const cancel = () => {cleanup();reject(new Error('Acesso cancelado. Atualize a análise para tentar novamente.'));};
      const submit = event => {event.preventDefault();const credentials=`${$('biAuthUser').value}:${$('biAuthPassword').value}`;const bytes=new TextEncoder().encode(credentials);state.auth='Basic '+btoa(String.fromCharCode(...bytes));cleanup();resolve();};
      const cleanup = () => {$('biAuthForm').removeEventListener('submit',submit);$('biAuthCancel').removeEventListener('click',cancel);$('biAuth').removeEventListener('cancel',cancel);$('biAuth').close();$('biAuthPassword').value='';state.authWait=null;};
      $('biAuthForm').addEventListener('submit',submit);$('biAuthCancel').addEventListener('click',cancel);$('biAuth').addEventListener('cancel',cancel,{once:true});
    });
    return state.authWait;
  }
  function error(message) {$('biError').hidden=!message;$('biError').textContent=message||'';}
  function notice(message, setup=false) {$('biNotice').hidden=!message;$('biNotice').replaceChildren(document.createTextNode(message||''));if(setup){const a=document.createElement('a');a.href='#model';a.textContent='Revisar modelo';$('biNotice').append(a);}}
  function busy(on) {document.body.setAttribute('aria-busy',String(on));$('biRefresh').disabled=on;$('biMobileRefresh').disabled=on;$('biRefresh').textContent=on?'Consultando a base…':'Atualizar análise';$('biMobileRefresh').textContent=on?'Consultando…':'Atualizar';}
  function rangeText() {return `${shortDate($('biStart').value)} a ${shortDate($('biEnd').value)}`;}
  function preset() {
    const now=new Date(), end=new Date(now), start=new Date(now), value=$('periodPreset').value;
    if(value==='month')start.setDate(1);
    else if(value==='previous'){start.setMonth(start.getMonth()-1,1);end.setDate(0);}
    else if(value!=='custom')start.setDate(start.getDate()-Number(value)+1);
    if(value!=='custom'){$('biStart').value=iso(start);$('biEnd').value=iso(end);}
    $('customDates').hidden=value!=='custom';updatePeriodLabel();
  }
  function updatePeriodLabel() {$('biPeriodLabel').textContent=rangeText();$('biMobilePeriod').textContent=`${dayLabel($('biStart').value)} a ${dayLabel($('biEnd').value)}`;}
  function updateFreshness() {
    const data=state.tab==='overview'?state.overview:state.tab==='explore'?state.exploration:null;
    $('biFreshness').textContent=data?`Atualizado ${new Date(data.generated_at).toLocaleTimeString('pt-BR',{hour:'2-digit',minute:'2-digit'})}`:state.tab==='model'?'Modelos locais':'Aguardando consulta';
  }
  function requestBody(drill=false) {
    if(!state.mapping)throw new Error('Mapeie os campos em Fontes e modelo antes de consultar.');
    const body={start:$('biStart').value,end:$('biEnd').value,company:$('biCompany').value||null,
      movement_group:$('biScope').value,only_valid:$('biValid').checked,compare:$('biCompare').checked};
    if($('modelSelect').value)body.model_id=$('modelSelect').value;else body.mapping=state.mapping;
    if(drill){body.company=state.drillCompany||body.company;if(state.drillDay)body.start=body.end=state.drillDay;}
    return body;
  }
  function dirty() {state.drillCompany=null;state.drillDay=null;state.exploreRequest++;state.request++;busy(false);$('runExplore').disabled=false;markViewDirty();if(state.overview||state.exploration)notice('Os filtros mudaram. Atualize a análise para consultar este novo recorte.');updatePeriodLabel();}
  function route() {
    const previousTab=state.tab;
    state.tab=['overview','explore','model'].includes(location.hash.slice(1))?location.hash.slice(1):'overview';
    if(previousTab!==state.tab&&state.tab!=='model')markViewDirty();
    document.querySelectorAll('[data-view]').forEach(el=>el.hidden=el.dataset.view!==state.tab);
    document.querySelectorAll('[data-tab]').forEach(el=>{if(el.dataset.tab===state.tab)el.setAttribute('aria-current','page');else el.removeAttribute('aria-current');});
    $('biFilterShell').hidden=state.tab==='model';
    $('exportView').disabled=state.tab==='model'||(state.tab==='overview'?!state.overview:!state.exploration);
    $('exportView').textContent=innerWidth<=760?'CSV':'Exportar CSV';
    $('exportView').setAttribute('aria-label','Exportar CSV');
    $('saveView').disabled=state.tab==='model'||!state.mapping;
    $('saveView').textContent=innerWidth<=760?'Salvar':'Salvar visão';
    $('shareView').textContent=innerWidth<=760?'Link':'Copiar link';
    updateFreshness();
    if(state.tab==='overview')requestAnimationFrame(drawTrend);
  }
  function setTab(tab) {if(location.hash===`#${tab}`)route();else location.hash=tab;}

  function readMapping() {
    const mapping={collection:$('sourceCollection').value};
    for(const [key,id] of Object.entries(fieldLabels))mapping[key]=$(id).value||null;
    if(!mapping.collection||!mapping.date_field||!mapping.value_field)throw new Error('Escolha a coleção, o campo de data e o campo de valor.');
    return mapping;
  }
  function populateMapping(schema, mapping=null) {
    const suggested=schema.suggested||{};
    for(const [key,id] of Object.entries(fieldLabels)){
      const select=$(id);select.replaceChildren();
      select.add(new Option(key==='company_field'||key==='product_field'?'Não mapeado':'Selecione um campo',''));
      if(key==='value_field')for(const field of schema.computed||[])select.add(new Option(field.label,field.name));
      for(const field of schema.fields){
        const types=field.types||[];
        const allowed=key==='date_field'?types.some(type=>['datetime','date'].includes(type)):key==='value_field'?types.some(type=>['int','float','Decimal128','Decimal','Int64'].includes(type)):true;
        if(allowed)select.add(new Option(field.name,field.name));
      }
      const suggestion={date_field:'date',value_field:'value',product_field:'product',company_field:'company'}[key];
      select.value=mapping?mapping[key]||'':suggested[suggestion]||'';
    }
    $('sampleNote').textContent=`Esquema observado em ${count(schema.sampled)} documentos. Campos raros podem não aparecer nesta amostra.`;
    $('lineageCollection').textContent=schema.collection;
    $('mappingGrain').textContent=$('mapValue').value==='@item_total'?'Total calculado do DigiSat: preço × quantidade arredondado − descontos + despesas + IPI/ST + frete + seguro.':'A data e a empresa pertencem ao documento. Produto e valor devem usar a mesma lista de itens.';
  }
  async function schemaFor(collection, mapping=null) {
    const token=++state.schemaRequest;
    $('sourceCollection').disabled=true;$('applyMapping').disabled=true;$('saveModel').disabled=true;
    try {const schema=await api(`/api/schema/${encodeURIComponent(collection)}`);if(token!==state.schemaRequest)return;state.schema=schema;populateMapping(schema,mapping);}
    finally {if(token===state.schemaRequest){$('sourceCollection').disabled=false;$('applyMapping').disabled=false;$('saveModel').disabled=false;}}
  }
  async function companies() {
    const mapping=state.mapping;if(!mapping)return;
    const current=$('biCompany').value;$('biCompany').replaceChildren(new Option('Todas as empresas',''));
    if(!mapping.company_field){$('biCompany').disabled=true;return;}
    $('biCompany').disabled=false;
    try {const result=await api(`/api/values/${encodeURIComponent(mapping.collection)}?field=${encodeURIComponent(mapping.company_field)}`);if(state.mapping!==mapping)return;for(const value of result.values)$('biCompany').add(new Option(String(value),String(value)));$('biCompany').value=current;if(!$('biCompany').value)$('biCompany').value='';if(result.truncated)$('biCompany').title='Lista limitada a 200 empresas.';}
    catch(_){$('biCompany').title='Não foi possível listar as empresas. Consulte o modelo e a conexão.';}
  }
  function mappingLabel() {$('biSourceLabel').textContent=state.mapping?`${state.source?.database||'Base atual'} · ${state.mapping.collection}`:'Base não mapeada';}
  async function applyMapping() {state.mapping=readMapping();$('modelSelect').value='';mappingLabel();await companies();dirty();route();notice('Mapeamento aplicado. Atualize a análise para calcular os resultados.');$('modelMessage').textContent='Mapeamento aplicado neste painel. Salve o modelo para reutilizá-lo.';}
  function renderModels() {
    const selected=$('modelSelect').value;
    $('modelSelect').replaceChildren(new Option('Mapeamento da conexão atual',''));
    for(const model of state.models){const option=new Option(model.name+(model.available?'':' · outra conexão'),model.id);option.disabled=!model.available;$('modelSelect').add(option);}
    $('modelSelect').value=selected;
    $('modelCount').textContent=`${state.models.filter(m=>m.available).length} nesta conexão`;
    $('savedModels').replaceChildren();
    if(!state.models.length){$('savedModels').innerHTML='<p class="bi-empty">Salve o primeiro modelo para reutilizar este mapeamento.</p>';return;}
    for(const model of state.models){
      const row=document.createElement('div');row.className='bi-saved-model';
      row.innerHTML=`<div><p>${esc(model.name)}</p><small>${esc(model.source.database||'Base não definida')} · ${esc(model.mapping.collection)}${model.available?'':' · conexão diferente'}</small></div><div><button class="secondary" type="button" data-load-model="${esc(model.id)}" ${model.available?'':'disabled'}>Usar modelo</button><button class="secondary" type="button" data-delete-model="${esc(model.id)}">Excluir</button></div>`;
      $('savedModels').append(row);
    }
  }
  async function loadModel(id) {
    const model=state.models.find(m=>m.id===id&&m.available);if(!model)throw new Error('Modelo indisponível nesta conexão.');
    state.mapping=model.mapping;$('sourceCollection').value=model.mapping.collection;$('modelSelect').value=model.id;$('modelName').value=model.name;
    await schemaFor(model.mapping.collection,model.mapping);await companies();mappingLabel();dirty();route();notice('Modelo selecionado. Atualize a análise para consultar os dados.');
  }

  function markViewDirty() {
    if(state.restoring)return;
    const canonical=value=>JSON.stringify(value,(_,item)=>item&&typeof item==='object'&&!Array.isArray(item)?Object.fromEntries(Object.entries(item).sort(([a],[b])=>a.localeCompare(b))):item);
    state.viewDirty=!state.activeView||canonical(currentViewConfig())!==canonical(state.activeView.config);
    $('biViewState').textContent=state.activeView?(state.viewDirty?'Alterações não salvas':'Visão salva'):'Filtros ainda não salvos';
    $('shareView').disabled=!state.activeView||state.viewDirty;
  }
  function currentViewConfig() {
    const tab=state.tab==='model'?(state.activeView?.config.tab||'overview'):state.tab,exploration=tab==='explore';
    const query={...requestBody(),mapping:state.mapping,model_id:null};
    let policy=state.viewPeriodPolicy||($('periodPreset').value==='custom'?'fixed':$('periodPreset').value);
    if(exploration&&state.drillDay)policy='fixed';
    return {query,period_preset:policy,tab,metric:$('trendMetric').value,dimension:$('exploreDimension').value,limit:Number($('exploreLimit').value),
      search:exploration?$('exploreSearch').value:'',sort:exploration?state.sort:null,sort_direction:state.sortDirection,
      drill_company:exploration?state.drillCompany:null,drill_day:exploration?state.drillDay:null};
  }
  function viewURL(id,tab) {
    const url=new URL(location.href);url.searchParams.set('view',id);url.hash=tab;return url.href;
  }
  function renderViews() {
    const select=$('savedViewSelect');select.replaceChildren(new Option('Visão de trabalho',''));
    for(const view of state.views){const option=new Option(view.name+(view.available?'':' · outra origem'),view.id);option.disabled=!view.available;select.add(option);}
    select.add(new Option('Gerenciar visões…','__manage__'));
    select.value=state.activeView?.id||'';
    $('savedViewsList').replaceChildren();
    if(!state.views.length){$('savedViewsList').innerHTML='<p class="bi-empty">Salve uma análise para voltar aos mesmos filtros e detalhamentos.</p>';return;}
    const periods={fixed:'Datas fixas',month:'Este mês',previous:'Mês passado','7':'Últimos 7 dias','30':'Últimos 30 dias'};
    for(const view of state.views){
      const row=document.createElement('div');row.className='bi-saved-view';
      row.innerHTML=`<div><strong>${esc(view.name)}</strong><small>${periods[view.period_preset]} · ${view.tab==='explore'?'Exploração':'Painel'}${view.available?'':' · origem diferente'}</small></div><div><button type="button" class="secondary" data-open-view="${esc(view.id)}" ${view.available?'':'disabled'}>Abrir</button><button type="button" class="secondary" data-remove-view="${esc(view.id)}">Excluir</button></div>`;
      $('savedViewsList').append(row);
    }
  }
  function openSaveView() {
    try{
      requestBody();$('saveViewError').textContent='';
      const preset=$('periodPreset').value,dynamic=preset!=='custom'&&!(state.tab==='explore'&&state.drillDay);
      $('viewName').value=state.activeView?.name||(state.tab==='explore'?'Exploração de vendas':'Acompanhamento de vendas');
      $('viewRolling').disabled=!dynamic;$('viewRolling').checked=dynamic;
      $('viewPeriodHint').textContent=dynamic?'O período escolhido será recalculado sempre que esta visão for aberta. Desmarque para manter as datas atuais.':'Este recorte será salvo com datas fixas, incluindo o dia detalhado, quando houver.';
      $('saveViewDialog').showModal();$('viewName').focus();
    }catch(exc){error(exc.message);}
  }
  async function saveView(event) {
    event.preventDefault();$('confirmSaveView').disabled=true;$('saveViewError').textContent='';
    try{
      const config={...currentViewConfig(),name:$('viewName').value,period_preset:$('viewRolling').checked&&!$('viewRolling').disabled?$('periodPreset').value:'fixed'};
      const record=await api('/api/bi/views',{method:'POST',body:JSON.stringify(config)});
      state.views.unshift({id:record.id,name:record.name,period_preset:record.config.period_preset,tab:record.config.tab,available:true,updated:record.updated});
      state.activeView=record;state.viewPeriodPolicy=record.config.period_preset;state.viewDirty=false;renderViews();$('biViewState').textContent='Visão salva';$('shareView').disabled=false;
      history.replaceState(null,'',viewURL(record.id,record.config.tab));$('saveViewDialog').close();
      notice('Visão salva no Insight. O link abre esta configuração e consulta a base atual.');
    }catch(exc){$('saveViewError').textContent=exc.message;}
    finally{$('confirmSaveView').disabled=false;}
  }
  async function loadView(id) {
    const token=++state.viewLoad;state.restoring=true;error('');
    state.request++;state.exploreRequest++;busy(false);
    try{
      const record=await api(`/api/bi/views/${encodeURIComponent(id)}`);if(token!==state.viewLoad)return;
      const config=record.config,query=config.query;
      state.mapping=query.mapping;$('modelSelect').value='';$('sourceCollection').value=query.mapping.collection;
      await schemaFor(query.mapping.collection,query.mapping);if(token!==state.viewLoad)return;
      await companies();if(token!==state.viewLoad)return;
      if(query.company&&!Array.from($('biCompany').options).some(option=>option.value===query.company))$('biCompany').add(new Option(query.company,query.company));
      $('biCompany').value=query.company||'';$('biStart').value=query.start;$('biEnd').value=query.end;
      $('periodPreset').value=config.period_preset==='fixed'?'custom':config.period_preset;
      $('customDates').hidden=$('periodPreset').value!=='custom';$('biScope').value=query.movement_group;$('biValid').checked=query.only_valid;$('biCompare').checked=query.compare;
      $('trendMetric').value=config.metric;$('exploreDimension').value=config.dimension;$('exploreLimit').value=String(config.limit);
      state.drillCompany=config.drill_company;state.drillDay=config.drill_day;updatePeriodLabel();mappingLabel();
      state.activeView=record;state.viewPeriodPolicy=config.period_preset;state.viewDirty=false;renderViews();setTab(config.tab);
      history.replaceState(null,'',viewURL(record.id,config.tab));
      if(config.tab==='explore'){
        await runExplore();if(token!==state.viewLoad)return;
        $('exploreSearch').value=config.search;state.sort=config.sort;state.sortDirection=config.sort_direction;renderTable();
      }else await runOverview();
      if(token===state.viewLoad){$('biViewState').textContent='Visão salva';$('shareView').disabled=false;}
    }catch(exc){if(token===state.viewLoad){error(exc.message);$('savedViewSelect').value=state.activeView?.id||'';}}
    finally{if(token===state.viewLoad)state.restoring=false;}
  }
  async function copyViewLink() {
    if(!state.activeView||state.viewDirty)return;
    const link=viewURL(state.activeView.id,state.activeView.config.tab);
    try{await navigator.clipboard.writeText(link);$('biViewState').textContent='Link copiado';}
    catch(_){$('viewLinkValue').value=link;$('viewLinkDialog').showModal();$('viewLinkValue').select();}
  }

  function drawTrend() {
    if(state.tab!=='overview')return;
    const rows=state.overview?.daily||[],metric=$('trendMetric').value;
    let labels=[''],series=[{name:metric==='total'?'Faturamento':'Documentos',values:[0]}];
    if(state.overview){
      const comparison=BIAnalysis.comparison(state.overview,metric);
      labels=comparison.current.dates.map(dayLabel);
      series=[{name:'Período atual',values:comparison.current.values,pointLabels:comparison.current.dates.map(shortDate)}];
      if(comparison.previous)series.push({name:'Período anterior',values:comparison.previous.values,pointLabels:comparison.previous.dates.map(shortDate),dash:true});
    }
    const values=series.flatMap(item=>item.values).filter(value=>value!=null);
    $('biTrendLegend').replaceChildren();
    $('trendComparisonNote').hidden=series.length<2;
    if(series.length>1)$('trendComparisonNote').textContent=`Anterior: ${shortDate(state.overview.previous_period.start)} a ${shortDate(state.overview.previous_period.end)}. Dias alinhados pela posição no período.`;
    const small=innerWidth<=760;
    const chartHeight=small?185:state.overview?150:180;
    $('biTrend').style.height=chartHeight+'px';
    Charts.line($('biTrend'),{labels,series,height:chartHeight,legend:$('biTrendLegend'),
      format:metric==='total'?money:count,tickFormat:metric==='total'?v=>new Intl.NumberFormat('pt-BR',{notation:'compact',maximumFractionDigits:1}).format(v):count,
      min:Math.min(0,...values),fill:false,yTicks:metric==='documents'&&Math.max(...values)<=4?[0,1,2,3,4]:null});
    if(!state.overview)$('biTrend').querySelector('svg')?.setAttribute('aria-label','Aguardando consulta. Nenhum valor foi carregado.');
    else if(!rows.length)$('biTrend').querySelector('svg')?.setAttribute('aria-label',series.length>1?'Sem movimento no período atual. O período anterior está desenhado para comparação.':'Nenhum dia com movimento no período consultado.');
  }
  function renderMetrics(data) {
    const labels=[['revenue','Faturamento'],['documents','Documentos'],['ticket','Ticket médio'],['payments','Pagamentos registrados']];
    $('biKpis').innerHTML=labels.map(([key,label])=>{
      const value=data.metrics[key],delta=data.deltas?.[key];let note='';
      if(delta){note=delta.percent==null?`${delta.absolute>0?'+':''}${key==='documents'?count(delta.absolute):money(delta.absolute)} · base anterior zero`:`${delta.percent>0?'+':''}${delta.percent.toLocaleString('pt-BR',{maximumFractionDigits:1})}% vs. período anterior`;}
      else note=value==null?'Não disponível na consulta':data.previous_period?'Comparação indisponível':'Sem comparação';
      const tone=key==='revenue'&&delta?.percent?delta.percent>0?'positive':'negative':'';
      return `<div><span>${label}</span><strong ${value==null?'class="is-missing"':''}>${key==='documents'?count(value):money(value)}</strong><small class="${tone}">${esc(note)}</small></div>`;
    }).join('');
  }
  function ranking(id, rows, type, denominator) {
    const el=$(id);el.replaceChildren();
    const issue=state.overview.errors[type==='company'?'companies':'products'];
    if(issue){el.innerHTML=`<p class="bi-empty">${esc(issue)}</p>`;return;}
    const available=type==='company'?state.overview.mapping.company_field:state.overview.mapping.product_field;
    if(!available){el.innerHTML='<p class="bi-empty">Esta dimensão não está mapeada. Configure-a em Fontes e modelo.</p>';return;}
    if(!rows.length){el.innerHTML='<p class="bi-empty">Nenhum movimento neste período e escopo.</p>';return;}
    const max=Math.max(...rows.map(row=>Math.abs(Number(row.total)||0)),1);
    for(const row of rows.slice(0,5)){
      const total=Number(row.total)||0,share=denominator>0?total/denominator*100:null;
      const button=document.createElement('button');button.type='button';button.className='bi-rank-row';
      const name=row._id==null?'Sem identificação':String(row._id);
      button.innerHTML=`<span class="rank-name" title="${esc(name)}">${esc(name)}</span><span class="rank-value">${money(total)}</span><span class="rank-bar" style="width:${Math.max(0,Math.min(100,Math.abs(total)/max*100))}%"></span><span class="rank-share">${share==null?'—':share.toLocaleString('pt-BR',{maximumFractionDigits:1})+'%'}</span>`;
      button.addEventListener('click',()=>{if(type==='company'&&row._id!=null){state.drillCompany=String(row._id);state.drillDay=null;$('exploreDimension').value='daily';setTab('explore');runExplore();}else showProduct(row);});
      if(type==='company'&&row._id==null)button.disabled=true;
      el.append(button);
    }
  }
  function showProduct(row) {
    $('productDetailTitle').textContent=row._id==null?'Sem identificação':String(row._id);
    $('productDetailBody').innerHTML=`<dl class="bi-measures"><div><dt>Valor agrupado</dt><dd>${money(Number(row.total)||0)}</dd></div><div><dt>Linhas de itens</dt><dd>${count(row.items||0)}</dd></div></dl><p>Linhas de itens são registros agregados, não a soma das quantidades vendidas. Este detalhe não aplica um filtro de produto aos documentos.</p>`;
    $('productDetail').showModal();
  }
  function renderOverview(data) {
    renderMetrics(data);drawTrend();
    const delta=data.deltas?.revenue;
    const peak=[...data.daily].sort((a,b)=>Number(b.total||0)-Number(a.total||0))[0];
    const concentration=peak&&data.metrics.revenue>0?Number(peak.total||0)/data.metrics.revenue:null;
    $('biFinding').textContent=data.metrics.documents===0?'Nenhum documento encontrado neste período e escopo.':concentration>.5?`${dayLabel(peak._id)} concentra ${(concentration*100).toLocaleString('pt-BR',{maximumFractionDigits:1})}% do faturamento. Abra os documentos desse dia para entender o resultado.`:`${money(data.metrics.revenue)} em ${count(data.metrics.documents)} documentos${delta?.percent!=null?` · ${delta.percent>0?'+':''}${delta.percent.toLocaleString('pt-BR',{maximumFractionDigits:1})}% em relação ao período anterior.`:'.'}`;
    const paid=data.metrics.payments,diff=paid==null?null:Math.round((data.metrics.revenue-paid)*100)/100;
    $('checkFinding').textContent=diff==null?'Pagamentos não disponíveis. O faturamento continua baseado no valor dos itens.':Math.abs(diff)<.01?'Itens e pagamentos fecham nesta consulta.':'Há uma diferença entre itens e pagamentos. Confira os documentos antes do fechamento.';
    $('biReconciliation').innerHTML=[['Valor dos itens',data.metrics.revenue],['Pagamentos',paid],['Diferença',diff]].map(([label,value])=>`<div><dt>${label}</dt><dd>${money(value)}</dd></div>`).join('');
    const dot=document.querySelector('.bi-status-dot');dot.className='bi-status-dot'+(diff==null?'':Math.abs(diff)<.01?' good':' warn');
    ranking('biCompanies',data.companies,'company',data.metrics.revenue);ranking('biProducts',data.products,'product',data.metrics.revenue);
    $('dayDrills').replaceChildren();
    for(const row of [...data.daily].sort((a,b)=>Number(b.total||0)-Number(a.total||0)).slice(0,4)){
      const button=document.createElement('button');button.type='button';button.textContent=`${dayLabel(row._id)} · ${money(Number(row.total)||0)}`;button.title=`Ver documentos de ${shortDate(row._id)}`;
      button.onclick=()=>{state.drillDay=row._id;state.drillCompany=null;$('exploreDimension').value='documents';setTab('explore');runExplore();};$('dayDrills').append(button);
    }
    $('trendHint').textContent=data.errors.daily||(!data.daily.length?'Nenhum dia com movimento neste período.':'Dias com maior faturamento · ver documentos:');
    $('biProvenance').innerHTML=`<p><strong>Origem:</strong> ${esc(data.source.database)} · ${esc(data.mapping.collection)} · ${esc(data.source.mode)}. Consulta em ${esc(new Date(data.generated_at).toLocaleString('pt-BR'))}.</p><p><strong>Período consultado:</strong> ${shortDate(data.period.start)} a ${shortDate(data.period.end)}.${data.previous_period?` Comparação: ${shortDate(data.previous_period.start)} a ${shortDate(data.previous_period.end)}.`:''}</p><p><strong>Escopo:</strong> ${esc(data.scope?.movement_label||'Conforme filtros')}. ${count(data.scope?.excluded||0)} documentos fora do escopo.</p><p>${esc(data.value_note||`Valor: ${data.mapping.value_field}. Revise a cobertura do campo antes de usar o total no fechamento.`)}</p>${data.coverage?`<p>Cobertura: ${esc(data.coverage.pct)}% · ${count(data.coverage.with_value)} de ${count(data.coverage.rows)} registros com valor.</p>`:''}<p>As consultas são independentes; alterações na base durante a execução podem produzir diferenças entre blocos. Rankings são limitados; o faturamento principal considera todo o escopo.</p>${data.warnings.length?'<ul>'+data.warnings.map(w=>`<li>${esc(w)}</li>`).join('')+'</ul>':''}`;
    updateFreshness();
    $('biSourceLabel').textContent=`${data.source.database} · ${data.mapping.collection}`;
  }
  async function runOverview() {
    const token=++state.request;error('');
    try{
      const body=requestBody();if(innerWidth<=760)$('biFilterDisclosure').open=false;busy(true);notice('Consultando os movimentos…');
      const data=await api('/api/bi/overview',{method:'POST',body:JSON.stringify(body)});if(token!==state.request)return;
      state.overview=data;renderOverview(data);$('exportView').disabled=state.tab!=='overview';
      const issues=[...Object.values(data.errors),...data.warnings];notice(issues.length?issues.join(' '):'');
    }catch(exc){if(token===state.request){error(exc.message);notice('');$('biFreshness').textContent=state.overview?'Falha ao atualizar · dados anteriores':'Consulta não concluída';}}
    finally{if(token===state.request)busy(false);}
  }
  function drillTrail() {
    $('drillTrail').replaceChildren();
    const root=document.createElement('button');root.type='button';root.textContent='Visão do período';root.onclick=()=>{state.drillCompany=null;state.drillDay=null;$('exploreDimension').value='companies';runExplore();};$('drillTrail').append(root);
    if(state.drillCompany){const span=document.createElement('span');span.textContent='›';const company=document.createElement('button');company.type='button';company.textContent=state.drillCompany;company.onclick=()=>{state.drillDay=null;$('exploreDimension').value='daily';runExplore();};$('drillTrail').append(span,company);}
    if(state.drillDay){const span=document.createElement('span');span.textContent=`› ${shortDate(state.drillDay)}`;$('drillTrail').append(span);}
  }
  async function runExplore() {
    markViewDirty();
    const token=++state.exploreRequest;error('');
    try{
      const body={...requestBody(true),dimension:$('exploreDimension').value,limit:Number($('exploreLimit').value)};
      const signature=JSON.stringify(body),preserve=state.exploreSignature===signature;
      $('runExplore').disabled=true;$('exploreStatus').textContent='Consultando os dados deste recorte…';drillTrail();
      const data=await api('/api/bi/explore',{method:'POST',body:JSON.stringify(body)});if(token!==state.exploreRequest)return;
      state.exploration=data;state.exploreSignature=signature;if(!preserve){state.sort=null;$('exploreSearch').value='';}renderTable();markViewDirty();updateFreshness();$('exportView').disabled=state.tab!=='explore';notice('');
    }catch(exc){if(token===state.exploreRequest){error(exc.message);$('exploreStatus').textContent=state.exploration?'Falha na atualização. A tabela mantém o resultado da última consulta.':'Não foi possível consultar este recorte.';}}
    finally{if(token===state.exploreRequest)$('runExplore').disabled=false;}
  }
  function renderTable() {
    const data=state.exploration;if(!data)return;
    const docs=data.dimension==='documents';
    const label={daily:'Dia de emissão',companies:'Empresa',top_products:'Produto'}[data.dimension];
    const columns=docs?[['Data','Emissão'],['Numero','Documento'],['Serie','Série'],['Empresa','Empresa'],['Produtos','Produtos'],['Produto','Produto'],['Valor','Valor'],['Situacao','Situação']].filter(([key])=>data.rows.some(row=>key in row)):[['_id',label],['total','Valor dos itens'],['documents','Documentos'],['items','Linhas de itens']].filter(([key])=>key==='_id'||data.rows.some(row=>key in row));
    state.columns=columns;
    const search=$('exploreSearch').value.trim().toLocaleLowerCase('pt-BR');
    let rows=data.rows.filter(row=>!search||Object.values(row).some(v=>String(v??'').toLocaleLowerCase('pt-BR').includes(search)));
    if(state.sort)rows=[...rows].sort((a,b)=>{const av=a[state.sort],bv=b[state.sort];return (typeof av==='number'&&typeof bv==='number'?av-bv:String(av??'').localeCompare(String(bv??''),'pt-BR',{numeric:true}))*state.sortDirection;});
    state.visibleRows=rows;
    const summary=BIAnalysis.visibleSummary(rows,data.dimension);
    $('exploreSummary').hidden=false;
    $('exploreSummary').innerHTML=`<span>Soma das linhas exibidas <small>${count(summary.rows)} linhas${summary.valuedRows<summary.rows?` · ${count(summary.valuedRows)} com valor`:''}</small></span><strong>${money(summary.amount)}</strong>`;
    const numeric=key=>['total','documents','items','Valor'].includes(key);
    const head=$('exploreTable').tHead;head.innerHTML='<tr>'+columns.map(([key,name])=>`<th scope="col" ${numeric(key)?'class="num"':''}><button type="button" data-sort="${key}">${esc(name)}${state.sort===key?(state.sortDirection===1?' ↑':' ↓'):''}</button></th>`).join('')+'</tr>';
    head.querySelectorAll('[data-sort]').forEach(button=>button.onclick=()=>{state.sortDirection=state.sort===button.dataset.sort?-state.sortDirection:1;state.sort=button.dataset.sort;markViewDirty();renderTable();});
    const tbody=$('exploreTable').tBodies[0];tbody.replaceChildren();
    if(!rows.length)tbody.innerHTML=`<tr><td colspan="${columns.length||1}" class="bi-empty">${search?'Nenhuma linha corresponde à pesquisa.':'Nenhum documento neste recorte.'}</td></tr>`;
    for(const row of rows){
      const tr=document.createElement('tr');
      for(const [key] of columns){const td=document.createElement('td');if(numeric(key))td.className='num';const value=row[key];let formatted;
        if(key==='Data'||(key==='_id'&&data.dimension==='daily'))formatted=shortDate(value);
        else if(key==='total'||key==='Valor')formatted=money(value);
        else if(numeric(key))formatted=value==null?'—':count(value);
        else formatted=Array.isArray(value)?value.join(', '):value==null?'Sem identificação':String(value);
        if(key==='_id'&&value!=null){const button=document.createElement('button');button.className='bi-row-action';button.type='button';button.textContent=formatted;button.onclick=()=>{if(data.dimension==='companies'){state.drillCompany=String(value);state.drillDay=null;$('exploreDimension').value='daily';runExplore();}else if(data.dimension==='daily'){state.drillDay=String(value);$('exploreDimension').value='documents';runExplore();}else showProduct(row);};td.append(button);}else td.textContent=formatted;tr.append(td);
      }tbody.append(tr);
    }
    $('exploreStatus').textContent=`${count(rows.length)} de ${count(data.rows.length)} linhas carregadas · ${shortDate(data.period.start)} a ${shortDate(data.period.end)} · ${count(data.documents)} documentos no escopo.${data.limit_reached?' Limite atingido; pode haver mais resultados. Refine os filtros para continuar.':''}${data.warnings?.length?' '+data.warnings.join(' '):''}`;
  }
  function csv() {
    const overview=state.tab==='overview';let columns,rows;
    if(overview){if(!state.overview)return;columns=[['indicador','Indicador'],['valor','Valor'],['inicio','Data inicial'],['fim','Data final']];rows=Object.entries(state.overview.metrics).map(([key,value])=>({indicador:{revenue:'Faturamento',documents:'Documentos',ticket:'Ticket médio',payments:'Pagamentos registrados'}[key],valor:value,inicio:state.overview.period.start,fim:state.overview.period.end}));}
    else {columns=state.columns;rows=state.visibleRows;}
    const result=overview?state.overview:state.exploration;
    if(!result)return;
    const metadata={base:result.source.database,empresa:result.applied_filters?.company||'Todas as empresas',escopo:result.applied_filters?.movement_group||'',validos:result.applied_filters?.only_valid?'Sim':'Não',inicio_consulta:result.period.start,fim_consulta:result.period.end};
    columns=[...columns,['base','Base'],['empresa','Empresa filtrada'],['escopo','Escopo'],['validos','Somente válidos'],['inicio_consulta','Início da consulta'],['fim_consulta','Fim da consulta']];
    rows=rows.map(row=>({...row,...metadata}));
    const cell=value=>{let text=Array.isArray(value)?value.join(', '):String(value??'');if(typeof value!=='number'&&/^[\s]*[=+\-@\t\r]/.test(text))text="'"+text;return '"'+text.replace(/"/g,'""')+'"';};
    const text='\ufeff'+[columns.map(([,name])=>cell(name)).join(';'),...rows.map(row=>columns.map(([key])=>cell(row[key])).join(';'))].join('\r\n');
    const url=URL.createObjectURL(new Blob([text],{type:'text/csv;charset=utf-8'}));const a=document.createElement('a');a.href=url;a.download=`insight-bi-${overview?'resumo':'exploracao'}.csv`;a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
  }
  async function boot() {
    preset();route();drawTrend();$('biFilterDisclosure').open=innerWidth>760;
    const results=await Promise.allSettled([api('/api/status'),api('/api/collections'),api('/api/bi/models'),api('/api/bi/views')]);
    const [status,collections,models,views]=results;
    if(status.status==='fulfilled'){
      const s=status.value;state.source=s;$('sourceState').textContent=s.connected?'Conectada':'Sem conexão';$('sourceState').classList.toggle('connected',!!s.connected);
      $('sourceDetails').innerHTML=`<div><dt>Banco de dados</dt><dd>${esc(s.database||'Não definido')}</dd></div><div><dt>Modo</dt><dd>${s.mode==='backup'?'Cópia de backup':'Base configurada'}</dd></div><div><dt>Acesso</dt><dd>Consultas de leitura</dd></div>`;
    }else{$('sourceState').textContent='Não foi possível verificar';}
    if(models.status==='fulfilled'){state.models=models.value.models;renderModels();}
    if(views.status==='fulfilled'){state.views=views.value.views;renderViews();}else{$('biViewState').textContent='Não foi possível listar as visões';}
    if(collections.status!=='fulfilled'){error(collections.reason.message);notice('Conecte uma base para começar. Configure a origem em Fontes e modelo.',true);return;}
    $('sourceCollection').replaceChildren();for(const item of collections.value)$('sourceCollection').add(new Option(`${item.name} · ${count(item.count)} documentos`,item.name));
    if(!collections.value.length){notice('Esta base não possui coleções disponíveis.',true);return;}
    try{
      await schemaFor($('sourceCollection').value);state.mapping=readMapping();$('modelName').value='Vendas por emissão';mappingLabel();await companies();route();
      notice(`Modelo sugerido a partir de ${state.mapping.collection}.`,true);
    }catch(exc){notice('Complete o mapeamento de data e valor para começar.',true);$('modelMessage').textContent=exc.message;}
    const linkedView=new URLSearchParams(location.search).get('view');
    if(linkedView)await loadView(linkedView);
  }
  function guarded(fn) {return async event=>{try{await fn(event);}catch(exc){error(exc.message);}};}
  $('biFilters').addEventListener('submit',event=>{event.preventDefault();state.tab==='explore'?runExplore():runOverview();});
  $('biMobileRefresh').onclick=()=>{if($('biFilters').reportValidity())state.tab==='explore'?runExplore():runOverview();};
  $('periodPreset').onchange=()=>{state.viewPeriodPolicy=$('periodPreset').value==='custom'?'fixed':$('periodPreset').value;preset();dirty();};for(const id of ['biStart','biEnd','biCompany','biScope','biValid','biCompare'])$(id).addEventListener('change',dirty);
  $('trendMetric').onchange=()=>{markViewDirty();drawTrend();};$('runExplore').onclick=runExplore;$('exploreSearch').oninput=()=>{markViewDirty();renderTable();};
  for(const id of ['exploreDimension','exploreLimit'])$(id).onchange=()=>{markViewDirty();notice('A configuração da exploração mudou. Consulte os dados para aplicar.');};
  $('resetDrill').onclick=()=>{state.drillCompany=null;state.drillDay=null;runExplore();};
  $('allCompanies').onclick=()=>{state.drillCompany=null;state.drillDay=null;$('exploreDimension').value='companies';setTab('explore');runExplore();};
  $('allProducts').onclick=()=>{state.drillCompany=null;state.drillDay=null;$('exploreDimension').value='top_products';setTab('explore');runExplore();};
  $('exportView').onclick=csv;$('closeProduct').onclick=()=>$('productDetail').close();
  $('saveView').onclick=openSaveView;$('saveViewForm').onsubmit=saveView;$('cancelSaveView').onclick=()=>$('saveViewDialog').close();
  $('shareView').onclick=guarded(copyViewLink);$('closeViewLink').onclick=()=>$('viewLinkDialog').close();$('closeViews').onclick=()=>$('viewsDialog').close();
  $('savedViewSelect').onchange=guarded(async()=>{
    const id=$('savedViewSelect').value;
    if(id==='__manage__'){$('savedViewSelect').value=state.activeView?.id||'';$('viewsDialog').showModal();return;}
    if(id)await loadView(id);
    else{state.activeView=null;state.viewPeriodPolicy=null;state.viewDirty=false;$('biViewState').textContent='Filtros ainda não salvos';$('shareView').disabled=true;const url=new URL(location.href);url.searchParams.delete('view');history.replaceState(null,'',url);}
  });
  $('savedViewsList').onclick=guarded(async event=>{
    const open=event.target.closest('[data-open-view]'),remove=event.target.closest('[data-remove-view]');
    if(open){$('viewsDialog').close();await loadView(open.dataset.openView);}
    if(remove){const id=remove.dataset.removeView,view=state.views.find(item=>item.id===id);if(!confirm(`Excluir a visão “${view.name}”? Os dados e modelos serão preservados.`))return;await api(`/api/bi/views/${encodeURIComponent(id)}`,{method:'DELETE'});state.views=state.views.filter(item=>item.id!==id);if(state.activeView?.id===id){state.activeView=null;state.viewDirty=false;$('biViewState').textContent='Visão excluída · filtros preservados';$('shareView').disabled=true;const url=new URL(location.href);url.searchParams.delete('view');history.replaceState(null,'',url);}renderViews();}
  });
  $('sourceCollection').onchange=guarded(async()=>{await schemaFor($('sourceCollection').value);$('modelMessage').textContent='Revise os campos e use o mapeamento ou salve um modelo.';});
  $('suggestMapping').onclick=()=>{if(state.schema)populateMapping(state.schema);};
  $('mapValue').onchange=()=>{$('mappingGrain').textContent=$('mapValue').value==='@item_total'?'O total calculado usa as linhas ItensBase e a fórmula do DigiSat.':'Produto e valor devem usar a mesma lista de itens.';};
  $('applyMapping').onclick=guarded(applyMapping);
  $('modelSelect').onchange=guarded(async()=>{if($('modelSelect').value)await loadModel($('modelSelect').value);else{state.mapping=readMapping();await companies();dirty();mappingLabel();}});
  $('mappingForm').onsubmit=guarded(async event=>{
    event.preventDefault();$('saveModel').disabled=true;try{const model=await api('/api/bi/models',{method:'POST',body:JSON.stringify({name:$('modelName').value,mapping:readMapping()})});state.models.unshift({...model,available:true});renderModels();await loadModel(model.id);$('modelMessage').textContent='Modelo salvo no Insight. Nenhum dado de origem foi alterado.';}finally{$('saveModel').disabled=false;}
  });
  $('savedModels').onclick=guarded(async event=>{const load=event.target.closest('[data-load-model]'),remove=event.target.closest('[data-delete-model]');if(load)await loadModel(load.dataset.loadModel);if(remove){const model=state.models.find(m=>m.id===remove.dataset.deleteModel);if(!confirm(`Excluir o modelo “${model.name}” do Insight? Os dados de origem serão preservados.`))return;await api(`/api/bi/models/${encodeURIComponent(model.id)}`,{method:'DELETE'});state.models=state.models.filter(m=>m.id!==model.id);renderModels();$('modelMessage').textContent='Modelo local excluído.';}});
  window.addEventListener('hashchange',route);let resize;window.addEventListener('resize',()=>{clearTimeout(resize);resize=setTimeout(drawTrend,150);});
  new MutationObserver(drawTrend).observe(document.documentElement,{attributes:true,attributeFilter:['data-theme']});
  matchMedia('(max-width:760px)').addEventListener('change',event=>{$('biFilterDisclosure').open=!event.matches;$('exportView').textContent=event.matches?'CSV':'Exportar CSV';$('saveView').textContent=event.matches?'Salvar':'Salvar visão';$('shareView').textContent=event.matches?'Link':'Copiar link';});
  document.fonts.ready.then(()=>boot()).catch(exc=>error(exc.message));
})();
