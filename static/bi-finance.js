(() => {
'use strict';
const $=id=>document.getElementById(id);
const state={auth:null,authWait:null,scope:null,side:null,person:null,personLabel:'',band:'all',due:null,reference:null,offset:0,detailRequest:0,detail:null,trigger:null};
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
const esc=v=>String(v??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const money=v=>v==null?'—':new Intl.NumberFormat('pt-BR',{style:'currency',currency:'BRL'}).format(v);
const day=v=>v?.slice(0,10).split('-').reverse().join('/')||'—';
const iso=d=>`${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`;
const post=(url,data)=>api(url,{method:'POST',body:JSON.stringify(data)});
function fail(message){$('error').hidden=!message;$('error').textContent=message||'';}
const bandLabels=['A vencer','Vence hoje','1–7 dias','8–30 dias','31–60 dias','61–90 dias','Mais de 90 dias'];
const bandKeys=['upcoming','today','1_7','8_30','31_60','61_90','90_plus'];
function drillAmount(source,side,{band=null,date=null,label=''}){
 if(!source)return date?`<span>—<small>${side==='receivable'?'receber':'pagar'}</small></span>`:'—';
 if(!source.count)return `<span>${money(source.amount)}${date?`<small>${side==='receivable'?'receber':'pagar'}</small>`:''}</span>`;
 return `<button type="button" class="finance-drill" data-side="${side}" ${band?`data-band="${band}"`:''} ${date?`data-due="${date}"`:''} aria-label="${esc(`${side==='receivable'?'A receber':'A pagar'} · ${label} · ${source.count} parcelas · ${money(source.amount)}`)}">${money(source.amount)}${date?`<small>${side==='receivable'?'receber':'pagar'}</small>`:''}</button>`;
}
function render(data){
 const sides=data?.sides||{},a=sides.receivable?.analysis,b=sides.payable?.analysis;
 $('kpis').innerHTML=[['A receber',a,'total','count'],['A pagar',b,'total','count'],['Recebimentos vencidos',a,'overdue','overdue_count'],['Pagamentos vencidos',b,'overdue','overdue_count']].map(([label,s,key,count])=>`<div><span>${label}</span><strong class="${s?'':'is-missing'}">${money(s?.[key])}</strong><small>${s?`${s[count]} parcelas no recorte`:'Fonte indisponível'}</small></div>`).join('');
 $('aging').innerHTML='<table><thead><tr><th>Vencimento</th><th class="num">A receber</th><th class="num">A pagar</th></tr></thead><tbody>'+bandLabels.map((label,i)=>`<tr><td>${label}</td><td class="num">${drillAmount(a?.aging[i],'receivable',{band:bandKeys[i],label})}${a?`<small>${a.aging[i].count} parcelas</small>`:''}</td><td class="num">${drillAmount(b?.aging[i],'payable',{band:bandKeys[i],label})}${b?`<small>${b.aging[i].count} parcelas</small>`:''}</td></tr>`).join('')+'</tbody></table>';
 for(const side of ['receivable','payable']){
  const source=sides[side];
  $(side+'People').innerHTML=source?.error?`<p class="bi-empty">${esc(source.error)}</p>`:source?.analysis.people.map(p=>`<div class="finance-row"><span>${p.id?`<button data-side="${side}" data-person="${esc(p.id)}" data-person-label="${esc(p.label)}">${esc(p.label)}</button>`:esc(p.label)}<small>${p.count} parcelas · vencido ${money(p.overdue)}</small></span><strong>${money(p.amount)}</strong></div>`).join('')||'<p class="bi-empty">Nenhuma parcela pendente neste recorte.</p>';
  document.querySelector(`.bi-panel-head button[data-side="${side}"]`).disabled=!source?.analysis;
 }
 const dates=[...new Set([...(a?.daily||[]),...(b?.daily||[])].map(x=>x.date))].filter(d=>d>=data?.reference_date).sort().slice(0,7);
 $('agenda').innerHTML=dates.map(date=>`<div class="finance-row"><span>${day(date)}<small>Vencimento</small></span><strong>${drillAmount(a?(a.daily.find(x=>x.date===date)||{amount:'0',count:0}):null,'receivable',{date,label:day(date)})}${drillAmount(b?(b.daily.find(x=>x.date===date)||{amount:'0',count:0}):null,'payable',{date,label:day(date)})}</strong></div>`).join('')||'<p class="bi-empty">'+(a&&b?'Nenhum vencimento futuro neste recorte.':'Consulte as fontes para exibir a agenda.')+'</p>';
}
async function refresh(event){
 event?.preventDefault();fail('');$('refresh').disabled=true;$('refresh').textContent='Consultando…';$('details').hidden=true;state.scope=null;state.detail=null;state.detailRequest++;$('exportParcels').disabled=true;render(null);$('provenance').textContent='Consulta em andamento.';
 const scope={start:$('start').value,end:$('end').value,company_id:$('company').value||null,account_id:$('account').value||null};
 try{const data=await post('/api/bi/finance/overview',scope);state.scope=scope;state.reference=data.reference_date;render(data);
 $('scope').textContent=`Vencimentos de ${day(data.start)} a ${day(data.end)} · pendentes · correção em ${day(data.reference_date)}.`;
 $('freshness').textContent=`Consultado às ${new Date().toLocaleTimeString('pt-BR')} · ${Object.values(data.sides).some(s=>s.error)?'Cobertura parcial: confira as fontes abaixo':'Recebimentos e Pagamentos disponíveis'}`;
 $('provenance').textContent=Object.entries(data.sides).map(([side,s])=>`${side==='receivable'?'Recebimentos':'Pagamentos'}: ${s.error||`${s.provenance.database} · ${s.provenance.collection} · leitura ao vivo`}`).join(' | ');
 }catch(error){fail(error.message);$('freshness').textContent='Consulta não concluída; nenhum total atual disponível.';}
 finally{$('refresh').disabled=false;$('refresh').textContent='Atualizar análise';}
}
async function parcels(){
 if(!state.scope)return;
 const request=++state.detailRequest;
 state.detail=null;$('exportParcels').disabled=true;$('detailTotals').hidden=true;
 $('details').hidden=false;$('detailTitle').textContent=state.side==='receivable'?'Parcelas a receber':'Parcelas a pagar';$('parcelTable').replaceChildren();$('detailStatus').textContent='Consultando parcelas…';$('previous').disabled=$('next').disabled=true;
 $('detailContext').textContent=[state.personLabel||'Todas as pessoas',state.band==='all'?'':bandLabels[bandKeys.indexOf(state.band)],state.due?`Vencimento em ${day(state.due)}`:'',`Recorte: ${day(state.scope.start)} a ${day(state.scope.end)}`].filter(Boolean).join(' · ');
 try{const data=await post('/api/bi/finance/parcels',{...state.scope,side:state.side,person_id:state.person,offset:state.offset,band:state.band,due_date:state.due,reference_date:state.reference});
 if(request!==state.detailRequest)return;
 $('detailContext').textContent=[state.personLabel||'Todas as pessoas',state.band==='all'?'':bandLabels[bandKeys.indexOf(state.band)],`Vencimentos: ${day(data.drill_scope.start)} a ${day(data.drill_scope.end)}`,`Referência: ${day(data.drill_scope.reference_date)}`].filter(Boolean).join(' · ');
 $('parcelTable').innerHTML='<table><thead><tr><th>Pessoa / empresa</th><th>Documento</th><th>Vencimento</th><th>Conta</th><th class="num">Corrigido</th></tr></thead><tbody>'+data.rows.map(r=>`<tr><td>${esc(r.person)}<small class="bi-fine"> · ${esc(r.company)}</small></td><td data-label="Documento">${esc(r.document)||'—'}</td><td data-label="Vencimento">${day(r.due_at)}</td><td data-label="Conta">${esc(r.account)||'—'}</td><td class="num" data-label="Corrigido">${money(r.corrected)}</td></tr>`).join('')+'</tbody></table>';
 $('detailStatus').textContent=data.rows.length?`Página ${Math.floor(state.offset/50)+1} · ${state.offset+1}–${state.offset+data.rows.length} de ${data.record_count} parcelas`:data.record_count?'Esta página ficou vazia após mudanças na base. Volte à página anterior ou atualize a análise.':'Nenhuma parcela encontrada.';$('previous').disabled=!state.offset;$('next').disabled=!data.pagination.has_more;
 state.detail={data,scope:{...state.scope},side:state.side,person:state.person,band:state.band,due:state.due,offset:state.offset};
 $('detailTotals').innerHTML=`<div><span>Total do detalhamento · ${data.record_count} parcelas</span><strong>${money(data.analysis.total)}</strong></div><div><span>Subtotal desta página · ${data.rows.length} parcelas</span><strong>${money(data.page_total)}</strong></div>`;$('detailTotals').hidden=false;$('exportParcels').disabled=!data.rows.length;$('exportParcels').textContent=`CSV · ${data.rows.length} parcelas`;$('exportParcels').setAttribute('aria-label',`Exportar ${data.rows.length} parcelas desta página em CSV`);
 }catch(error){if(request===state.detailRequest)$('detailStatus').textContent=error.message;}
}
function exportParcels(){
 const detail=state.detail;if(!detail?.data.rows.length)return;
 const {data,scope,side,band,offset}=detail;
 const headers=['Tipo','Pessoa','Empresa','Documento','Descrição','Vencimento','Emissão','Situação','Conta','Valor original','Corrigido','Juros','Multa','Desconto','Taxa','ID da parcela','Banco de dados','Coleção','Referência do cálculo','Vencimentos desde','Vencimentos até','Faixa de atraso','Empresa filtrada (ID)','Conta filtrada (ID)','Posição no detalhamento'];
 const numeric=new Set([9,10,11,12,13,14]);
 const quote=(value,isNumber=false)=>{let text=String(value??'');if(isNumber&&/^-?\d+\.\d{2}$/.test(text))text=text.replace('.',',');else if(/^[\s\u0000-\u0020]*[=+\-@]/.test(text)||/^[\t\r\n]/.test(text))text="'"+text;return '"'+text.replace(/"/g,'""')+'"';};
 const lines=[headers.map(v=>quote(v)).join(';'),...data.rows.map((r,i)=>[
 side==='receivable'?'A receber':'A pagar',r.person,r.company,r.document,r.description,r.due_at,r.issued_at,r.status,r.account,r.value,r.corrected,r.interest,r.penalty,r.discount,r.fee,r.id,data.provenance.database,data.provenance.collection,data.provenance.reference_date,data.drill_scope.start,data.drill_scope.end,band==='all'?'Todas':bandLabels[bandKeys.indexOf(band)],scope.company_id||'Todas',scope.account_id||'Todas',offset+i+1
 ].map((value,index)=>quote(value,numeric.has(index))).join(';'))];
 const blob=new Blob(['\uFEFF'+lines.join('\r\n')+'\r\n'],{type:'text/csv;charset=utf-8'}),url=URL.createObjectURL(blob),link=document.createElement('a');link.href=url;link.download=`parcelas-${side}-${data.drill_scope.start}-${data.drill_scope.end}-pagina-${Math.floor(offset/50)+1}.csv`;link.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
}
$('exportParcels').onclick=exportParcels;
document.addEventListener('click',event=>{const button=event.target.closest('[data-side]');if(!button||!state.scope)return;state.trigger=button;state.side=button.dataset.side;state.person=button.dataset.person||null;state.personLabel=button.dataset.personLabel||'';state.band=button.dataset.band||'all';state.due=button.dataset.due||null;state.offset=0;parcels();$('detailTitle').focus({preventScroll:true});$('details').scrollIntoView({behavior:'smooth',block:'start'});});
$('filters').addEventListener('submit',refresh);$('closeDetails').onclick=()=>{state.detailRequest++;state.detail=null;$('exportParcels').disabled=true;$('details').hidden=true;if(state.trigger?.isConnected)state.trigger.focus();};$('previous').onclick=()=>{state.offset=Math.max(0,state.offset-50);parcels();};$('next').onclick=()=>{state.offset+=50;parcels();};
const now=new Date(),start=new Date(now),end=new Date(now);start.setDate(start.getDate()-365);end.setDate(end.getDate()+30);$('start').value=iso(start);$('end').value=iso(end);if(matchMedia('(max-width:640px)').matches)$('filtersDisclosure').open=false;
render(null);
api('/api/finance/options').then(data=>{for(const [key,id] of [['companies','company'],['accounts','account']])for(const row of data[key]){const option=document.createElement('option');option.value=row.id;option.textContent=row.label;$(id).append(option);}return refresh();}).catch(error=>fail(error.message));
})();
