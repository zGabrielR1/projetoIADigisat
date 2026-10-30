'use strict';
(() => {
  const $=id=>document.getElementById(id);
  const normal=t=>String(t).normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLowerCase();
  const el=(tag,text,parent)=>{const node=document.createElement(tag);if(text!==undefined)node.textContent=text;if(parent)parent.append(node);return node;};
  const option=(target,value,label)=>{const node=el('option',label,target);node.value=value;return node;};
  const friendly=t=>String(t).replace(/Referencia$/,'').replace(/([a-záéíóú])([A-Z])/g,'$1 $2').replace(/^./,s=>s.toUpperCase());
  let reports=[],session=null,httpAuth=null,authWait=null,readers=[],result=null,snapshot=null,columns=[],busy=false,revision=0,onlyFavorites=false,searchRevision=0,presetState=null,pendingQuery=null;
  let favorites=[];try{const saved=JSON.parse(localStorage.getItem('insight-report-favorites')||'[]');if(Array.isArray(saved))favorites=saved.filter(v=>typeof v==='string').slice(0,200);}catch(_){}
  let recentReports=[];try{const saved=JSON.parse(localStorage.getItem('insight-report-recents')||'[]');if(Array.isArray(saved))recentReports=[...new Set(saved.filter(v=>typeof v==='string'))].slice(0,5);}catch(_){}
  const linkParams=new URLSearchParams(location.search);
  const validDate=value=>{try{return /^\d{4}-\d{2}-\d{2}$/.test(value)&&new Date(value).toISOString().slice(0,10)===value;}catch(_){return false;}};
  let suggestedPeriod=validDate(linkParams.get('start'))&&validDate(linkParams.get('end'))?{start:linkParams.get('start'),end:linkParams.get('end')}:null;
  const sameChoice=(a,b)=>JSON.stringify(a,Object.keys(a||{}).sort())===JSON.stringify(b,Object.keys(b||{}).sort());
  async function applySuggestions(reportId,callIndex,values,warnings=[]){
    const preview=await api('/api/native/preview',{report_id:reportId,call_index:callIndex,arguments:values});
    if(selected()?.id!==reportId)return;
    presetState=preview;$('call').value=String(callIndex);configureCall();
    if(!suggestedPeriod&&!warnings.length&&selected().calls[callIndex].parameters.some(p=>p.kind==='datetime'))warnings=['O link não informa período. Confira as datas antes de consultar.'];
    $('suggestionList').replaceChildren();for(const item of preview.summary)el('li',`${item.label}: ${item.display}`,$('suggestionList'));
    $('suggestionWarning').textContent=warnings.join(' ');$('suggestionReview').hidden=!preview.summary.length&&!warnings.length;
  }
  function favoriteState(){const r=selected();const active=!!r&&favorites.includes(r.id);$('favorite').textContent=active?'★ Favoritado':'☆ Favoritar';$('favorite').setAttribute('aria-pressed',String(active));$('favorite').disabled=!r;$('copyLink').disabled=!r;}
  function fail(error){$('error').textContent=error.message;$('error').hidden=false;}
  function authenticate(){
    if(authWait)return authWait;
    authWait=new Promise((resolve,reject)=>{
      $('authDialog').showModal();$('authForm').onsubmit=e=>{e.preventDefault();const bytes=new TextEncoder().encode(`${$('authUser').value}:${$('authPassword').value}`);httpAuth='Basic '+btoa(String.fromCharCode(...bytes));$('authPassword').value='';$('authDialog').close();resolve();};
      $('authCancel').onclick=()=>{$('authDialog').close();reject(new Error('Acesso cancelado.'));};$('authDialog').oncancel=()=>reject(new Error('Acesso cancelado.'));
    }).finally(()=>authWait=null);return authWait;
  }
  async function api(path,body){
    const send=()=>fetch(path,{method:body?'POST':'GET',headers:{...(body?{'Content-Type':'application/json'}:{}),...(httpAuth?{Authorization:httpAuth}:{}),...(session?{'X-Native-Session':session}:{})},...(body?{body:JSON.stringify(body)}:{})});
    let response=await send();if(response.status===401&&(!session||!path.endsWith('/query'))){await authenticate();response=await send();}
    const data=await response.json();if(!response.ok){const detail=Array.isArray(data.detail)?data.detail.map(d=>d.msg).join('; '):data.detail;throw new Error(typeof detail==='string'?detail:'Consulta indisponível.');}return data;
  }
  function clear(){revision++;result=null;snapshot=null;$('result').hidden=true;$('error').hidden=true;}
  function selected(){return reports.find(r=>r.id===$('report').value);}
  function renderRecent(){const section=$('recentReports'),list=$('recentReportList');if(!section||!list)return;list.replaceChildren();const items=recentReports.map(id=>reports.find(r=>r.id===id)).filter(Boolean);section.hidden=!items.length;for(const report of items){const button=el('button',undefined,list);button.type='button';button.className='recent-report';button.title=report.title+' / '+report.module;el('span',report.title,button);el('small',report.module,button);button.onclick=()=>{presetState=null;suggestedPeriod=null;$('suggestionReview').hidden=true;onlyFavorites=false;$('favoritesOnly').setAttribute('aria-pressed','false');$('module').value='';$('search').value='';filterReports(report.id);$('report').focus();};}}
  function filterReports(preferred){
    const text=normal($('search').value);const module=$('module').value;
    const found=reports.filter(r=>(!onlyFavorites||favorites.includes(r.id))&&(!module||r.module===module)&&normal(`${r.title} ${r.module}`).includes(text));
    $('report').replaceChildren();for(const r of found)option($('report'),r.id,`${r.module} · ${r.title}`);
    if(found.some(r=>r.id===preferred))$('report').value=preferred;
    configure();
  }
  function localDate(end=false,start=false){const d=new Date();const date=[d.getFullYear(),String(d.getMonth()+1).padStart(2,'0'),String(start?1:d.getDate()).padStart(2,'0')].join('-');return date+(end?'T23:59:59.999':'T00:00:00');}
  function makeField(p,parent){
    if(p.fixed)return ()=>p.default;
    const label=p.label||friendly(p.name);
    if(p.kind==='object'&&p.choices?.length){
      const box=el('fieldset',undefined,parent);el('legend',label,box);const select=el('select',undefined,box);select.required=true;option(select,'','Selecione');
      for(let i=0;i<p.choices.length;i++)option(select,String(i),p.choices[i].label);
      if(p.default!==undefined){const i=p.choices.findIndex(c=>sameChoice(c.value,p.default));if(i>=0)select.value=String(i);}
      option(select,'manual','Informar código e descrição');const manual=el('div',undefined,box);manual.hidden=true;
      const nested=(p.properties||[]).map(child=>[child.name,makeField(child,manual)]);
      select.onchange=()=>{manual.hidden=select.value!=='manual';for(const control of manual.querySelectorAll('input,select,textarea'))control.disabled=manual.hidden;};select.onchange();
      return ()=>select.value==='manual'?Object.fromEntries(nested.map(([name,read])=>[name,read()])):p.choices[Number(select.value)].value;
    }
    if(p.kind==='object'){
      const box=el('fieldset',undefined,parent);el('legend',label,box);
      const nested=(p.properties||[]).map(child=>[child.name,makeField(child,box)]);
      return ()=>Object.fromEntries(nested.map(([name,read])=>[name,read()]));
    }
    const wrapper=el('label',label,parent);let input;
    if(p.kind==='enum'||(p.kind==='number'&&p.choices?.length)){
      input=el('select',undefined,wrapper);input.required=true;option(input,'','Selecione');for(const choice of p.choices||[])option(input,String(choice.value),friendly(choice.label));
    }else if(p.kind==='array'){
      input=el('textarea',undefined,wrapper);input.placeholder='Uma referência ou código por linha; deixe vazio para lista vazia.';
    }else{
      input=el('input',undefined,wrapper);input.type=p.kind==='boolean'?'checkbox':p.kind==='datetime'?'datetime-local':'text';
      if(p.kind==='number')input.inputMode='decimal';
      if(p.kind==='datetime'){input.step='.001';input.required=true;const end=/final|fim/i.test(p.name),start=/inicial|inicio/i.test(p.name);input.value=typeof p.default==='string'?p.default:suggestedPeriod&&(end||start)?suggestedPeriod[end?'end':'start']+(end?'T23:59:59.999':'T00:00:00'):localDate(end,start);}
      if(p.kind==='boolean'){wrapper.className='check';input.checked=p.default===true;}
      if(p.kind==='text')input.maxLength=2000;
      if(p.kind==='number'){input.required=true;input.value=p.default!==undefined?String(p.default):'';}
    }
    if(p.kind==='text'&&(p.lookup||/referencia/i.test(p.name))){
      const hint=normal(`${p.name} ${p.label||''}`);
      const kind=p.lookup||(/pessoa|cliente|vendedor|tecnico|fornecedor|prescritor|colaborador/.test(hint)?'people':/produto/.test(hint)?'products':/conta/.test(hint)?'accounts':/ticket/.test(hint)?'tickets':/contrato/.test(hint)?'contracts':null);
      if(kind){
        input.placeholder='Referência ou nome para pesquisar';const button=el('button','Pesquisar cadastro',wrapper);button.type='button';const found=el('select',undefined,wrapper);found.hidden=true;
        button.onclick=async()=>{try{const data=await api('/api/native/lookup?kind='+encodeURIComponent(kind)+'&q='+encodeURIComponent(input.value));found.replaceChildren();option(found,'',data.items.length?'Selecione o cadastro':'Nenhum cadastro encontrado');for(const item of data.items)option(found,item.id,item.label);found.hidden=false;}catch(error){fail(error);}};
        found.onchange=()=>{if(found.value){input.value=found.value;clear();}};
      }
    }
    if(p.default!==undefined&&!['object','boolean','datetime'].includes(p.kind))input.value=String(p.default);
    return ()=>p.kind==='boolean'?input.checked:p.kind==='array'?input.value.split(/[\n,]+/).map(v=>v.trim()).filter(Boolean):input.value;
  }
  function configureCall(){
    clear();const r=selected();const call=r?.calls[Number($('call').value)];$('parameters').replaceChildren();readers=[];
    $('queryForm').hidden=!call?.available;$('query').disabled=!session||busy;
    if(!call)return;
    const active=presetState?.report_id===r.id&&presetState.call_index===Number($('call').value);
    if(!active)$('suggestionReview').hidden=true;
    for(const parameter of call.parameters||[]){const p=active&&Object.hasOwn(presetState.arguments,parameter.name)?{...parameter,default:presetState.arguments[parameter.name]}:parameter;readers.push([p.name,makeField(p,$('parameters'))]);}
  }
  function configure(){
    clear();const r=selected();$('call').replaceChildren();
    if(r){let best=0,weight=-1;for(let i=0;i<r.calls.length;i++){const c=r.calls[i];if(c.available){option($('call'),String(i),friendly(c.method.replace(/^Obter/,'')));const w=(c.parameters.some(p=>p.kind==='datetime')?100:0)+c.parameters.length;if(w>weight){best=i;weight=w;}}}$('call').value=String(best);}
    $('callLabel').hidden=(r?.calls.filter(c=>c.available).length||0)<=1;
    $('unavailable').hidden=!!r?.calls.some(c=>c.available);$('unavailable').textContent=r?'Este relatório não foi localizado nos assemblies instalados. Nenhuma regra ou valor foi inventado.':'Nenhum relatório corresponde à pesquisa.';
    $('reportSummary').textContent=r?`${r.module} · ${r.calls.filter(c=>c.available).length} conjunto(s) disponível(is)`:'Escolha um relatório. Seus favoritos ficam guardados neste navegador.';
    favoriteState();configureCall();
  }
  function primitive(row,prefix='',out={}){
    if(row&&typeof row==='object'&&!Array.isArray(row)){for(const [key,value]of Object.entries(row)){const path=prefix?prefix+'.'+key:key;if(value&&typeof value==='object'&&!Array.isArray(value))primitive(value,path,out);else out[path]=value;}}
    else out[prefix||'Resultado']=row;
    return out;
  }
  function render(data){
    result=data;$('resultTitle').textContent=data.title;$('provenance').textContent=`SuiteG6 · usuário ${data.provenance.username} · empresa ${data.provenance.company_id} · ${data.provenance.queried_at.replace('T',' ')} · ${data.provenance.duration_ms} ms`;
    $('dataset').replaceChildren();for(const d of data.datasets)option($('dataset'),d.path,`${d.path==='$'?'Principal':d.path.replace(/^\$\[?\]?\.?/,'')} (${d.count})`);$('dataset').value=snapshot.dataset;
    const rows=data.rows.map(row=>({...row.context,...primitive(row.record)}));columns=[...new Set(rows.flatMap(row=>Object.keys(row)))];
    $('rows').replaceChildren();const head=el('tr',undefined,el('thead',undefined,$('rows')));for(const name of columns)el('th',friendly(name),head).scope='col';const body=el('tbody',undefined,$('rows'));
    for(const row of rows){const tr=el('tr',undefined,body);for(const name of columns){const value=row[name];const td=el('td',undefined,tr);if(value&&typeof value==='object'){td.className='cell-object';const details=el('details',undefined,td);el('summary',Array.isArray(value)?`${value.length} itens`:'Detalhes',details);el('pre',JSON.stringify(value,null,2),details);}else td.textContent=value===null||value===undefined?'—':String(value);}}
    if(!rows.length)el('td','Nenhum registro retornado para os parâmetros informados.',el('tr',undefined,body)).colSpan=Math.max(1,columns.length);
    const p=data.pagination;$('pageLabel').textContent=`${p.total?p.offset+1:0}–${p.offset+rows.length} de ${p.total}`;$('previous').disabled=p.offset===0;$('next').disabled=!p.has_more;$('csv').disabled=!rows.length;
    $('warnings').replaceChildren();for(const warning of data.warnings)el('li',warning,$('warnings'));$('result').hidden=false;
  }
  async function run(payload){
    if(busy)return;busy=true;$('query').disabled=true;$('error').hidden=true;$('result').hidden=true;$('statusText').textContent='Consultando o serviço nativo…';
    const generation=revision;
    try{const data=await api('/api/native/query',payload);if(generation!==revision){$('statusText').textContent='A seleção mudou durante a consulta. Execute novamente com os filtros atuais.';return;}snapshot=payload;render(data);recentReports=[payload.report_id,...recentReports.filter(id=>id!==payload.report_id)].slice(0,5);try{localStorage.setItem('insight-report-recents',JSON.stringify(recentReports));}catch(_){}renderRecent();$('statusText').textContent='Consulta nativa concluída.';}catch(error){result=null;fail(error);$('statusText').textContent='Consulta não concluída. Se a conexão expirou, desconecte e entre novamente.';}finally{busy=false;$('query').disabled=!session;}
  }
  async function discoverReports(question){
    const request=++searchRevision;$('discover').disabled=true;$('discoveryStatus').textContent='Procurando no catálogo…';
    try{const data=await api('/api/native/search',{question,limit:6});if(request!==searchRevision)return;$('discoveryResults').replaceChildren();
      $('discoveryStatus').textContent=(data.period_error||data.message)+(data.period?` Período sugerido: ${data.period.start} a ${data.period.end}.`:'');
      if(data.needs_clarification)$('discoveryStatus').textContent+=' O pedido contém mais de uma opção. Escolha o relatório e confira a situação.';
      for(const match of data.matches){const button=el('button',undefined,$('discoveryResults'));button.type='button';button.className='discovery-match';el('span',match.title,button);el('small',`${match.module} · ${match.available?'Abrir filtros':'Ausente na instalação'}`,button);if(match.proposed_filters?.suggestions.length)el('small',`${match.proposed_filters.suggestions.length} filtro(s) sugerido(s) para revisão`,button);button.onclick=async()=>{suggestedPeriod=data.period;onlyFavorites=false;$('favoritesOnly').setAttribute('aria-pressed','false');$('module').value='';$('search').value='';presetState=null;filterReports(match.id);if(match.available){const p=match.proposed_filters;try{await applySuggestions(match.id,p.call_index,Object.fromEntries(p.suggestions.map(s=>[s.name,s.value])),p.warnings);}catch(error){fail(error);}}$('report').focus();};}
    }catch(error){fail(error);$('discoveryStatus').textContent='Não foi possível pesquisar. Tente novamente.';}finally{if(request===searchRevision)$('discover').disabled=false;}
  }
  function jsonPage(){if(!result)return;const blob=new Blob([JSON.stringify({report_id:result.report_id,provenance:result.provenance,dataset:snapshot.dataset,pagination:result.pagination,rows:result.rows},null,2)],{type:'application/json'});const url=URL.createObjectURL(blob);const link=el('a');link.href=url;link.download=result.report_id+'-pagina-'+snapshot.offset+'.json';link.click();setTimeout(()=>URL.revokeObjectURL(url),1000);}
  function csv(){
    if(!result)return;const quote=value=>{let text=value===null||value===undefined?'':typeof value==='object'?JSON.stringify(value):String(value);if(/^[\s]*[=+\-@]/.test(text)&&!/^[-+]?\d+(\.\d+)?$/.test(text))text="'"+text;return '"'+text.replace(/"/g,'""')+'"';};
    const lines=[columns.map(quote).join(';'),...result.rows.map(row=>{const flat={...row.context,...primitive(row.record)};return columns.map(c=>quote(flat[c])).join(';');})];const url=URL.createObjectURL(new Blob(['\ufeff'+lines.join('\r\n')],{type:'text/csv;charset=utf-8'}));const a=el('a');a.href=url;a.download=snapshot.report_id+'-pagina-'+snapshot.offset+'.csv';a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
  }
  async function load(){
    try{
      const data=await api('/api/native/catalog');reports=data.reports;const modules=[...new Set(reports.map(r=>r.module))].sort((a,b)=>a.localeCompare(b,'pt-BR'));for(const m of modules)option($('module'),m,m);
      const opts=await api('/api/finance/options');option($('company'),'','Selecione');for(const c of opts.companies)option($('company'),c.id,c.label);if(opts.companies.length===1)$('company').value=opts.companies[0].id;
      filterReports(new URLSearchParams(location.search).get('report'));renderRecent();$('statusText').textContent=`${data.available} entradas com serviços nativos; ${reports.length-data.available} não localizadas.`;
      $('search').oninput=()=>filterReports($('report').value);$('module').onchange=()=>filterReports($('report').value);$('report').onchange=configure;$('call').onchange=configureCall;
      $('discoveryForm').onsubmit=e=>{e.preventDefault();discoverReports($('intent').value.trim());};for(const button of document.querySelectorAll('[data-intent]'))button.onclick=()=>{$('intent').value=button.dataset.intent;discoverReports(button.dataset.intent);};
      $('favoritesOnly').onclick=()=>{onlyFavorites=!onlyFavorites;$('favoritesOnly').setAttribute('aria-pressed',String(onlyFavorites));filterReports($('report').value);};
      $('favorite').onclick=()=>{const id=selected()?.id;if(!id)return;favorites=favorites.includes(id)?favorites.filter(v=>v!==id):[id,...favorites].slice(0,200);try{localStorage.setItem('insight-report-favorites',JSON.stringify(favorites));}catch(_){}if(onlyFavorites)filterReports(id);else favoriteState();};
      $('copyLink').onclick=async()=>{const url=new URL('/native-reports',location.origin);url.searchParams.set('report',selected().id);if(suggestedPeriod){url.searchParams.set('start',suggestedPeriod.start);url.searchParams.set('end',suggestedPeriod.end);}try{await navigator.clipboard.writeText(url.href);$('statusText').textContent='Link copiado. Ele inclui somente o relatório e o período sugerido.';}catch(_){$('statusText').textContent='Copie este link: '+url.href;}};
      $('jsonPage').onclick=jsonPage;
      $('clearSuggestions').onclick=()=>{presetState=null;suggestedPeriod=null;$('suggestionReview').hidden=true;configureCall();};
      if(linkParams.has('preset')||linkParams.has('call')){try{const text=linkParams.get('preset')||'{}';if(text.length>8192)throw new Error('As sugestões do link excedem o limite permitido.');const values=JSON.parse(text);if(!values||Array.isArray(values)||typeof values!=='object')throw new Error('Sugestões do link inválidas.');await applySuggestions(selected().id,Number(linkParams.get('call')||$('call').value),values);}catch(error){fail(error);}}
      $('connectForm').onsubmit=async e=>{e.preventDefault();$('connect').disabled=true;$('error').hidden=true;try{const value=await api('/api/native/connect',{username:$('username').value,password:$('password').value,company_id:$('company').value});session=value.session;$('password').value='';$('connectForm').hidden=true;$('connected').hidden=false;$('connectionInfo').textContent=`Conectado como ${value.username} · empresa ${$('company').selectedOptions[0].textContent} · conexão válida por 20 minutos.`;$('query').disabled=false;}catch(error){fail(error);}finally{$('connect').disabled=false;}};
      $('disconnect').onclick=async()=>{try{await api('/api/native/disconnect',{});}finally{session=null;clear();$('connectForm').hidden=false;$('connected').hidden=true;$('query').disabled=true;}};
      $('queryForm').onsubmit=e=>{e.preventDefault();if(!session||busy)return;const args=Object.fromEntries(readers.map(([name,read])=>[name,read()]));pendingQuery={report_id:$('report').value,call_index:Number($('call').value),arguments:args,page_size:Number($('pageSize').value),offset:0,dataset:'$'};$('reviewTitle').textContent=selected().title;$('reviewConnection').textContent=$('connectionInfo').textContent;$('reviewFilters').replaceChildren();const call=selected().calls[pendingQuery.call_index];for(const p of call.parameters){el('dt',p.label||friendly(p.name),$('reviewFilters'));const value=args[p.name];const choice=(p.choices||[]).find(c=>sameChoice(c.value,value)||(typeof c.value!=='object'&&typeof value!=='object'&&String(c.value)===String(value)));el('dd',choice?choice.label:typeof value==='boolean'?(value?'Sim':'Não'):value&&typeof value==='object'?JSON.stringify(value):String(value===null||value===undefined||value===''?'Sem filtro':value),$('reviewFilters'));}$('queryReview').showModal();};
      $('confirmQuery').onclick=()=>{const query=pendingQuery;pendingQuery=null;$('queryReview').close();if(query)run(query);};$('cancelQuery').onclick=()=>{pendingQuery=null;$('queryReview').close();};$('queryReview').oncancel=()=>pendingQuery=null;
      $('parameters').oninput=clear;$('previous').onclick=()=>run({...snapshot,offset:Math.max(0,snapshot.offset-snapshot.page_size)});$('next').onclick=()=>run({...snapshot,offset:snapshot.offset+snapshot.page_size});$('dataset').onchange=()=>run({...snapshot,offset:0,dataset:$('dataset').value});$('csv').onclick=csv;
    }catch(error){fail(error);$('statusText').textContent='Não foi possível carregar a conexão nativa.';}
  }
  load();
})();
