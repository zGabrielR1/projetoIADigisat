'use strict';
(() => {
  const $=id=>document.getElementById(id);
  const normal=t=>String(t).normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLowerCase();
  const el=(tag,text,parent)=>{const node=document.createElement(tag);if(text!==undefined)node.textContent=text;if(parent)parent.append(node);return node;};
  const option=(target,value,label)=>{const node=el('option',label,target);node.value=value;return node;};
  const friendly=t=>String(t).replace(/Referencia$/,'').replace(/([a-záéíóú])([A-Z])/g,'$1 $2').replace(/^./,s=>s.toUpperCase());
  let reports=[],session=null,httpAuth=null,authWait=null,readers=[],result=null,snapshot=null,columns=[],busy=false;
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
  function clear(){result=null;snapshot=null;$('result').hidden=true;$('error').hidden=true;}
  function selected(){return reports.find(r=>r.id===$('report').value);}
  function filterReports(preferred){
    const text=normal($('search').value);const module=$('module').value;
    const found=reports.filter(r=>(!module||r.module===module)&&normal(`${r.title} ${r.module}`).includes(text));
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
      if(p.kind==='datetime'){input.step='.001';input.required=true;input.value=localDate(/final|fim/i.test(p.name),/inicial|inicio/i.test(p.name));}
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
    for(const parameter of call.parameters||[])readers.push([parameter.name,makeField(parameter,$('parameters'))]);
  }
  function configure(){
    clear();const r=selected();$('call').replaceChildren();
    if(r)for(let i=0;i<r.calls.length;i++){const c=r.calls[i];if(c.available)option($('call'),String(i),friendly(c.method.replace(/^Obter/,'')));}
    $('callLabel').hidden=(r?.calls.filter(c=>c.available).length||0)<=1;
    $('unavailable').hidden=!!r?.calls.some(c=>c.available);$('unavailable').textContent=r?'Este relatório não foi localizado nos assemblies instalados. Nenhuma regra ou valor foi inventado.':'Nenhum relatório corresponde à pesquisa.';
    configureCall();
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
    try{const data=await api('/api/native/query',payload);snapshot=payload;render(data);$('statusText').textContent='Consulta nativa concluída.';}catch(error){result=null;fail(error);$('statusText').textContent='Consulta não concluída.';}finally{busy=false;$('query').disabled=!session;}
  }
  function csv(){
    if(!result)return;const quote=value=>{let text=value===null||value===undefined?'':typeof value==='object'?JSON.stringify(value):String(value);if(/^[\s]*[=+\-@]/.test(text)&&!/^[-+]?\d+(\.\d+)?$/.test(text))text="'"+text;return '"'+text.replace(/"/g,'""')+'"';};
    const lines=[columns.map(quote).join(';'),...result.rows.map(row=>{const flat={...row.context,...primitive(row.record)};return columns.map(c=>quote(flat[c])).join(';');})];const url=URL.createObjectURL(new Blob(['\ufeff'+lines.join('\r\n')],{type:'text/csv;charset=utf-8'}));const a=el('a');a.href=url;a.download=snapshot.report_id+'-pagina-'+snapshot.offset+'.csv';a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
  }
  async function load(){
    try{
      const data=await api('/api/native/catalog');reports=data.reports;const modules=[...new Set(reports.map(r=>r.module))].sort((a,b)=>a.localeCompare(b,'pt-BR'));for(const m of modules)option($('module'),m,m);
      const opts=await api('/api/finance/options');option($('company'),'','Selecione');for(const c of opts.companies)option($('company'),c.id,c.label);if(opts.companies.length===1)$('company').value=opts.companies[0].id;
      filterReports(new URLSearchParams(location.search).get('report'));$('statusText').textContent=`${data.available} entradas com serviços nativos; ${reports.length-data.available} não localizadas.`;
      $('search').oninput=()=>filterReports($('report').value);$('module').onchange=()=>filterReports($('report').value);$('report').onchange=configure;$('call').onchange=configureCall;
      $('connectForm').onsubmit=async e=>{e.preventDefault();$('connect').disabled=true;$('error').hidden=true;try{const value=await api('/api/native/connect',{username:$('username').value,password:$('password').value,company_id:$('company').value});session=value.session;$('password').value='';$('connectForm').hidden=true;$('connected').hidden=false;$('connectionInfo').textContent=`Conectado como ${value.username} · empresa ${$('company').selectedOptions[0].textContent} · conexão válida por 20 minutos.`;$('query').disabled=false;}catch(error){fail(error);}finally{$('connect').disabled=false;}};
      $('disconnect').onclick=async()=>{try{await api('/api/native/disconnect',{});}finally{session=null;clear();$('connectForm').hidden=false;$('connected').hidden=true;$('query').disabled=true;}};
      $('queryForm').onsubmit=e=>{e.preventDefault();const args=Object.fromEntries(readers.map(([name,read])=>[name,read()]));run({report_id:$('report').value,call_index:Number($('call').value),arguments:args,page_size:Number($('pageSize').value),offset:0,dataset:'$'});};
      $('parameters').oninput=clear;$('previous').onclick=()=>run({...snapshot,offset:Math.max(0,snapshot.offset-snapshot.page_size)});$('next').onclick=()=>run({...snapshot,offset:snapshot.offset+snapshot.page_size});$('dataset').onchange=()=>run({...snapshot,offset:0,dataset:$('dataset').value});$('csv').onclick=csv;
    }catch(error){fail(error);$('statusText').textContent='Não foi possível carregar a conexão nativa.';}
  }
  load();
})();
