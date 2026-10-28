'use strict';
(() => {
  const $ = id => document.getElementById(id);
  const labels = {inclusion:'Inclusão', due:'Vencimento', settled:'Quitação', issue:'Emissão'};
  const moneyLabels = {value:'Valor', corrected:'Valor corrigido / líquido', settled:'Valor quitado', interest:'Juros', penalty:'Multa', discount:'Desconto', fee:'Taxa'};
  const columns = [['company','Empresa'],['person','Pessoa'],['document','Documento'],['order','Parcela'],['issued_at','Emissão'],['due_at','Vencimento'],['settled_at','Quitação'],['species','Espécie'],['status','Situação'],['network','Rede'],['operation','Operação'],...Object.entries(moneyLabels),['account','Conta']];
  let specs = new Map(), titles = new Map(), result, snapshot, auth, pendingAuth, controller;
  const element = (tag, text, parent) => {const node=document.createElement(tag);if(text!==undefined)node.textContent=text;if(parent)parent.append(node);return node;};
  const option = (select, value, label) => {const node=element('option',label,select);node.value=value;};
  const localDate = d => `${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`;
  function money(value) {const text=String(value ?? '0.00');const [whole, fraction='00']=text.split('.');const negative=whole.startsWith('-');return `${negative?'-':''}R$ ${BigInt(whole.replace('-','')).toLocaleString('pt-BR')},${fraction.padEnd(2,'0')}`;}
  function display(key,value) {if(key in moneyLabels)return money(value);if(key.endsWith('_at'))return value?value.slice(0,10).split('-').reverse().join('/'):'—';return value??'—';}
  function error(err) {if(err.name==='AbortError')return;$('error').textContent=err.message;$('error').hidden=false;}
  function authenticate() {
    if(pendingAuth)return pendingAuth;
    pendingAuth=new Promise((resolve,reject)=>{
      $('authDialog').showModal();
      $('authForm').onsubmit=e=>{e.preventDefault();const bytes=new TextEncoder().encode(`${$('authUser').value}:${$('authPassword').value}`);auth='Basic '+btoa(String.fromCharCode(...bytes));$('authPassword').value='';$('authDialog').close();resolve();};
      $('authDialog').oncancel=()=>reject(new Error('Acesso cancelado.'));
      $('authCancel').onclick=()=>{$('authDialog').close();reject(new Error('Acesso cancelado.'));};
    }).finally(()=>{pendingAuth=null;});
    return pendingAuth;
  }
  async function api(path, options={}) {
    const send=()=>fetch(path,{...options,headers:{...(options.body?{'Content-Type':'application/json'}:{}),...(auth?{Authorization:auth}:{})}});
    let response=await send();
    if(response.status===401){await authenticate();response=await send();}
    const data=await response.json();
    if(!response.ok){let message=data.detail;if(Array.isArray(message))message=message.map(v=>v.msg).join('; ');throw new Error(typeof message==='string'?message:'Não foi possível consultar.');}
    return data;
  }
  function clear() {controller?.abort();result=null;snapshot=null;$('result').hidden=true;$('error').hidden=true;}
  function configure() {
    clear();const s=specs.get($('report').value);if(!s)return;
    $('basis').replaceChildren();for(const basis of s.dates)option($('basis'),basis,labels[basis]);$('basis').value='due';$('status').value='all';
    for(const id of ['basisLabel','statusLabel'])$(id).hidden=s.mode!=='select';
    $('statusLabel').hidden=s.mode!=='select'||!s.status_filter;
    for(const id of ['companyLabel','consolidatedLabel'])$(id).hidden=s.global_scope;
    $('consolidatedLabel').hidden=s.global_scope||s.seller_filter;
    $('consolidated').checked=false;$('company').disabled=false;
    $('fixedScope').hidden=s.mode==='select';
    $('fixedScope').textContent=s.global_scope?'Todas as empresas · parcelas quitadas com qualquer histórico no período de competência.':s.mode==='open'?'Parcelas em aberto · período pelo vencimento.':'Parcelas quitadas · período pela quitação.';
    $('accountLabel').hidden=!s.account_filter||s.global_scope;$('speciesLabel').hidden=s.mode!=='open';$('personFilters').hidden=!(s.person_filter||s.seller_filter||s.detail_filters);
    $('personLookup').hidden=!(s.person_filter||s.seller_filter);$('personTitle').textContent=s.seller_filter?'Vendedor obrigatório':s.card_filter?'Administradora de cartão':'Pessoa';$('accountCodes').hidden=!s.detail_filters;
    $('segmentLabel').hidden=!s.segment_filter;$('segment').value='';
    $('operationLabel').hidden=!s.card_filter;$('operation').value='all';$('orderLabel').hidden=!s.card_filter;
    $('zeroFeeLabel').hidden=!s.fee_filter;$('zeroFee').checked=true;
    $('orderBy').replaceChildren();for(const [value,label] of [['default','Padrão'],['issue','Emissão'],['due','Vencimento'],...(s.fee_filter?[['fee_asc','Taxa crescente'],['fee_desc','Taxa decrescente']]:[['network','Rede']])])option($('orderBy'),value,label);
    $('account').value='';$('species').value='99';$('person').replaceChildren();option($('person'),'','Todas as pessoas');
    for(const id of ['plan','costCenter','personSearch'])$(id).value='';$('personStatus').textContent='';
  }
  function request() {
    const s=specs.get($('report').value), consolidated=$('consolidated').checked;
    const data={report_id:s.id,start:$('start').value,end:$('end').value,date_basis:s.mode==='select'?$('basis').value:'due',status:s.mode==='select'?$('status').value:'all',consolidated:s.global_scope?false:consolidated,page_size:Number($('pageSize').value),offset:0};
    if(!s.global_scope&&!consolidated)data.company_id=$('company').value||null;
    if(s.account_filter&&!s.global_scope)data.account_id=$('account').value||null;
    if(s.mode==='open')data.species=Number($('species').value);
    if(s.person_filter)data.person_id=$('person').value||null;
    if(s.seller_filter)data.seller_id=$('person').value||null;
    if(s.detail_filters){data.plan_code=$('plan').value.trim()||null;data.cost_center_code=$('costCenter').value.trim()||null;}
    if(s.segment_filter)data.segment_id=$('segment').value||null;
    if(s.card_filter){data.operation=$('operation').value;data.order_by=$('orderBy').value;}
    if(s.fee_filter)data.include_zero_fees=$('zeroFee').checked;
    return data;
  }
  function table(target,rows,cols) {
    target.replaceChildren();const header=element('tr',undefined,element('thead',undefined,target));
    for(const [,label] of cols)element('th',label,header).scope='col';
    const body=element('tbody',undefined,target);
    for(const row of rows){const tr=element('tr',undefined,body);for(const [key] of cols){const td=element('td',display(key,row[key]),tr);if(key in moneyLabels)td.className='money';}}
    if(!rows.length)element('td','Nenhuma parcela encontrada para os filtros informados.',element('tr',undefined,body)).colSpan=cols.length;
  }
  function render(data) {
    result=data;$('resultTitle').textContent=titles.get(data.report_id)||data.report_id;
    $('applied').textContent=`${snapshot.start.split('-').reverse().join('/')} a ${snapshot.end.split('-').reverse().join('/')} · ${data.record_count} parcelas · totais de todo o resultado`;
    $('totals').replaceChildren();for(const [key,label] of Object.entries(moneyLabels)){const card=element('div',label,$('totals'));element('strong',money(data.totals[key]),card);}
    table($('rows'),data.rows,columns);table($('groups'),data.groups,[['company','Empresa'],['label','Agrupamento'],['count','Parcelas'],...Object.entries(moneyLabels)]);
    const p=data.pagination;$('pageLabel').textContent=`${data.record_count?p.offset+1:0}–${p.offset+data.rows.length} de ${data.record_count}`;$('previous').disabled=p.offset===0;$('next').disabled=!p.has_more;$('csv').disabled=!data.rows.length;
    $('warnings').replaceChildren();for(const warning of data.warnings)element('li',warning,$('warnings'));
    $('provenance').textContent=`Base: ${data.provenance.database} · coleção: ${data.provenance.collection} · referência dos encargos: ${data.provenance.reference_date} · ${data.provenance.duration_ms} ms`;
    $('result').hidden=false;
  }
  async function run(data) {
    controller?.abort();controller=new AbortController();const current=controller;
    $('error').hidden=true;$('run').disabled=true;$('loadStatus').textContent='Consultando parcelas…';$('result').hidden=true;result=null;
    try {const response=await api('/api/finance/query',{method:'POST',body:JSON.stringify(data),signal:current.signal});if(current!==controller)return;snapshot=data;render(response);$('loadStatus').textContent='Consulta concluída.';}
    catch(err){if(current===controller){error(err);$('loadStatus').textContent=err.name==='AbortError'?'Consulta cancelada.':'Consulta não concluída.';}}
    finally{if(current===controller)$('run').disabled=false;}
  }
  function csv() {
    if(!result)return;
    const escaped=(value,numeric)=>{let text=String(value??'');if(!numeric&&/^[\s]*[=+\-@]/.test(text))text="'"+text;return '"'+text.replace(/"/g,'""')+'"';};
    const rows=[columns.map(([,label])=>escaped(label,false)).join(';'),...result.rows.map(row=>columns.map(([key])=>escaped(key in moneyLabels?String(row[key]).replace('.',','):row[key],key in moneyLabels)).join(';'))];
    const blob=new Blob(['\ufeff'+rows.join('\r\n')],{type:'text/csv;charset=utf-8'});const url=URL.createObjectURL(blob);const a=element('a');a.href=url;a.download=`financeiro-${snapshot.start}-${snapshot.end}-pagina-${snapshot.offset}.csv`;a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
  }
  async function load() {
    try {
      const [metadata,catalog]=await Promise.all([api('/api/finance/reports'),fetch('/static/report-catalog.json').then(r=>{if(!r.ok)throw new Error('Catálogo indisponível.');return r.json();})]);
      for(const r of catalog.reports)titles.set(r.id,`${r.module_label} · ${r.title}`);
      specs=new Map(metadata.reports.map(s=>[s.id,s]));for(const s of metadata.reports)option($('report'),s.id,titles.get(s.id)||s.id);
      const requested=new URLSearchParams(location.search).get('report');if(specs.has(requested))$('report').value=requested;
      const opts=await api('/api/finance/options');option($('company'),'','Selecione uma empresa');for(const c of opts.companies)option($('company'),c.id,c.label);if(opts.companies.length===1)$('company').value=opts.companies[0].id;
      option($('account'),'','Todas as contas');for(const c of opts.accounts)option($('account'),c.id,c.label);
      option($('segment'),'','Todos os tipos');for(const c of opts.segments)option($('segment'),c.id,c.label);
      const today=new Date();$('start').value=localDate(new Date(today.getFullYear(),today.getMonth(),1));$('end').value=localDate(today);
      configure();$('financeForm').hidden=false;$('loadStatus').textContent=`${specs.size} visões financeiras disponíveis.`;
      $('report').onchange=configure;$('financeForm').addEventListener('input',clear);
      $('consolidated').onchange=()=>{$('company').disabled=$('consolidated').checked;};
      $('financeForm').onsubmit=e=>{e.preventDefault();run(request());};
      $('previous').onclick=()=>run({...snapshot,offset:Math.max(0,snapshot.offset-snapshot.page_size)});$('next').onclick=()=>run({...snapshot,offset:snapshot.offset+snapshot.page_size});$('csv').onclick=csv;
      $('findPerson').onclick=async()=>{try{const data=await api('/api/finance/people?q='+encodeURIComponent($('personSearch').value));$('person').replaceChildren();option($('person'),'','Todas as pessoas');for(const p of data.people)option($('person'),p.id,p.label);$('personStatus').textContent=`${data.people.length} pessoas encontradas.${data.has_more?' Refine a pesquisa para encontrar outras.':''}`;clear();}catch(err){error(err);}};
    } catch(err){error(err);$('loadStatus').textContent='Não foi possível carregar os relatórios financeiros.';}
  }
  load();
})();
