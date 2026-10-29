/* 1.0 workspace: server-owned conversation context and explicit evidence provenance. */
const workspace = { conversationId:null, plan:null, runId:null, pending:null, requestedKind:null, extra:{}, exportBusy:false, restoring:false };
try { workspace.conversationId = sessionStorage.getItem("insight-conversation"); } catch (_) {}
const postWorkspace = (path, body) => api(`/api/workspace/${path}`, {method:"POST",body:JSON.stringify(body)});

function rememberConversation(id) {
  workspace.conversationId = id;
  try { if(id) sessionStorage.setItem("insight-conversation",id); else sessionStorage.removeItem("insight-conversation"); } catch (_) {}
}

function renderConversation(messages, prompt=null) {
  const log=$("#conversationLog");
  log.innerHTML=(messages||[]).map(m=>`<div class="chat-message ${m.role==='user'?'user':'assistant'}"><strong>${m.role==='user'?'Você':'Insight'}</strong>${esc(m.text)}</div>`).join("");
  log.classList.toggle("hidden",!messages?.length);
  for(const [i,m] of (messages||[]).entries()){
    if(!m.native_reports)continue;
    const results=document.createElement('div');results.className='native-suggestions';
    for(const report of m.native_reports.matches||[]){
      const link=document.createElement('a');link.className='native-suggestion';
      link.href=report.url.startsWith('/native-reports?')?report.url:'/native-reports';
      const title=document.createElement('strong');title.textContent=report.title;
      const detail=document.createElement('span');detail.textContent=`${report.module} · ${report.available?'Abrir filtros':'Ausente na instalação'}`;
      link.append(title,detail);results.append(link);
    }
    const note=document.createElement('p');note.textContent=m.native_reports.period_error||(m.native_reports.period?`Período sugerido: ${m.native_reports.period.start} a ${m.native_reports.period.end}. Confirme os filtros na tela nativa.`:'A busca identifica relatórios. Consulte os dados após login e confirmação dos filtros.');results.append(note);
    log.children[i].append(results);
  }
  log.scrollTop=log.scrollHeight;
  const box=$("#clarificationBox");
  workspace.pending=prompt;
  box.classList.toggle("hidden",!prompt);
  box.innerHTML=prompt?`<b>Preciso esclarecer antes de consultar</b><p>${esc(prompt.question)}</p><div class="clarification-options">${(prompt.options||[]).map((o,i)=>`<button type="button" class="secondary" data-answer-index="${i}">${esc(o.label)}</button>`).join("")}</div><p>Escolha uma opção ou escreva a resposta no campo da pergunta.</p>`:"";
  $$('[data-answer-index]',box).forEach(b=>b.addEventListener('click',()=>sendAnswer(prompt.options[Number(b.dataset.answerIndex)].value)));
  $("#question").placeholder=prompt?"Responda à pergunta de esclarecimento":"Ex.: e o vendedor 3? Compare com o dia anterior";
}

function applyPlan(plan) {
  workspace.plan=plan;
  setRange(plan.start,plan.end,null);
  for(const [key,id] of Object.entries({collection:"collection",date_field:"dateField",value_field:"valueField",product_field:"productField",company_field:"companyField",company_value:"companyValue",movement_group:"movementGroup"})) {
    if(plan[key]!==undefined && $("#"+id)) $("#"+id).value=plan[key]??"";
  }
  $("#onlyValid").checked=plan.only_valid;
  $("#profileSelect").value=plan.profile;
  $("#pageSize").value=String(plan.page_size);
  $("#documentSearch").value=plan.search||"";
  $("#issueFilter").value=plan.issue||"all";
}

const renderBeforeWorkspace=render;
render=function(data) {
  renderBeforeWorkspace(data);
  $("#comparisonBox").classList.add("hidden");
  $("#qualityBox").classList.add("hidden");
  if(data.comparison) {
    const c=data.comparison;
    const names={Itens:"Itens dos documentos",Vendedor:"Itens do vendedor",Outros:"Outros/sem vendedor",Pagamentos:"Pagamentos",Diferenca:"Itens − pagamentos"};
    $("#comparisonBox").innerHTML=`<h3>Comparação</h3><p>${esc(c.plan.start)} a ${esc(c.plan.end)} · ${esc(c.result.investigation?.seller||'todos os vendedores')} · ${esc(c.plan.company_value||'todas as empresas')}</p><p>${esc(c.direction)}</p><div class="comparison-grid">${Object.entries(c.metrics).map(([k,v])=>`<div class="comparison-stat"><small>${esc(names[k])}</small><strong>${fmtMoney(v.difference)}</strong><small>Base ${fmtMoney(v.base)} · comparação ${fmtMoney(v.comparison)}</small><small>${v.percent===null?'Percentual indefinido: base igual a zero':`${fmtNumber(v.percent)}%`}</small></div>`).join("")}</div><button class="secondary" type="button" id="removeComparison">Remover comparação</button><details><summary>Documentos da comparação — ${c.result.rows.length} listados de ${c.result.documents}</summary><div class="table-wrap"><table><thead><tr><th>Série/número</th><th>Itens</th><th>Vendedor</th><th>Outros</th><th>Pagamentos</th></tr></thead><tbody>${c.result.rows.map(r=>`<tr><td>${esc(r.Serie)}/${esc(r.Numero)}</td><td class="num">${fmtMoney(r.Itens)}</td><td class="num">${fmtMoney(r.Vendedor)}</td><td class="num">${fmtMoney(r.Outros)}</td><td class="num">${fmtMoney(r.Pagamentos)}</td></tr>`).join("")}</tbody></table></div></details>`;
    $("#comparisonBox").classList.remove("hidden");
    $("#removeComparison").onclick=()=>refine({comparison:null});
  }
  if(data.plan?.kind==='quality') {
    const counts=data.quality?.counts||{};
    const labels={mismatch:"Itens e pagamentos divergentes",unpaid:"Sem pagamento",unassigned:"Itens sem vendedor",shared:"Vendas compartilhadas",negative:"Total negativo"};
    const candidates=data.quality?.duplicate_candidates||[];
    $("#qualityBox").innerHTML=`<h3>Qualidade dos dados</h3><div class="comparison-grid">${Object.entries(labels).map(([k,v])=>`<div class="comparison-stat"><small>${esc(v)}</small><strong>${fmtNumber(counts[k]||0)}</strong></div>`).join("")}</div><p>${esc(data.quality?.note||'')}</p><p>${candidates.length} grupos candidatos à duplicidade (até 100 exibidos).</p>${candidates.length?`<details><summary>Conferir candidatos</summary>${candidates.map(r=>`<p>${esc(r._id.company)} · ${esc(r._id.type)} · ${esc(r._id.serie)}/${esc(r._id.number)} · ${r.count} registros</p>`).join("")}</details>`:''}`;
    $("#qualityBox").classList.remove("hidden");
  }
  const p=data.pagination;
  $("#documentControls").classList.toggle("hidden",!p);
  $("#pageControls").classList.toggle("hidden",!p);
  if(p) {
    $("#previousPage").disabled=p.offset===0;
    $("#nextPage").disabled=!p.has_more;
    $("#pageNote").textContent=`${p.total? p.offset+1:0}–${p.offset+data.rows.length} de ${fmtNumber(p.total)} vendas`;
    $("#tableFoot").textContent=`${data.rows.length} vendas nesta página · totais sobre todas as ${fmtNumber(p.total)} vendas encontradas`;
    $("#tableFoot").classList.remove('hidden');
  }
  $("#evidenceToolbar").classList.toggle("hidden",!workspace.runId);
  const provenance=data.provenance;
  if(provenance) {
    const saved=provenance.source==='saved_snapshot';
    $("#provenanceNote").textContent=`${saved?'CÓPIA SALVA — valores não recalculados':'Consulta à base atual'} · ${new Date(provenance.generated_at).toLocaleString('pt-BR')} · ${fmtNumber(provenance.duration_ms)} ms · ${provenance.version}`;
    $("#profileDescription").textContent=provenance.profile.description;
  }
};

async function receiveWorkspace(response) {
  rememberConversation(response.conversation_id);
  if(response.status==='native_reports'){
    workspace.pending=null;renderConversation(response.messages);$('#question').value='';
    if(state.data)render(state.data);else{$('#resultTitle').textContent='Escolha um relatório nas sugestões';$('#resultNote').textContent='A busca encontrou opções no catálogo. Nenhuma consulta de dados foi executada.';$('#summary').replaceChildren();$('#table thead').replaceChildren();$('#table tbody').replaceChildren();$('#evidenceToolbar').classList.add('hidden');}
    $('#resultado').setAttribute('aria-busy','false');$('#table').setAttribute('aria-busy','false');return;
  }
  applyPlan(response.plan);
  renderConversation(response.messages,response.clarification||null);
  if(response.status==='clarification') {
    if(state.data) render(state.data);
    else { $("#resultTitle").textContent="Aguardando esclarecimento"; $("#summary").innerHTML=""; $("#table tbody").innerHTML=""; }
    $("#resultado").setAttribute('aria-busy','false');
    $("#table").setAttribute('aria-busy','false');
    $("#question").value="";
    $("#question").focus();
    return;
  }
  workspace.runId=response.run_id;
  state.data=response.result;
  state.rows=response.result.rows||[];
  state.kind=response.result.interpretation.kind;
  state.sort=null;
  state.investigation=null;
  selectKind(state.kind);
  render(response.result);
  $("#savedTitle").value=$("#savedTitle").value||response.result.interpretation.kind_label;
}

const workspaceReady=(async()=>{
  if(!workspace.conversationId) return;
  try {
    const session=await api(`/api/workspace/conversations/${encodeURIComponent(workspace.conversationId)}`);
    workspace.plan=session.pending?.plan||session.plan;
    renderConversation(session.messages,session.pending?.clarification||null);
  } catch (_) {rememberConversation(null);}
})();

execute=async function(question=null,options={}) {
  await workspaceReady;
  if(options.auto && workspace.pending){if(workspace.plan)applyPlan(workspace.plan);$('#resultTitle').textContent='Aguardando esclarecimento';return;}
  if(state.busy) {state.pending=true; state.pendingCall={question,options};return;}
  const request=payload();
  if(!question&&(!request.collection||!request.date_field||!request.start||!request.end)){
    if(!options.auto) toast("Aguarde a conexão e o mapeamento, ou configure a conexão.");
    return;
  }
  let filters={...request,status_filter:request.only_valid?(workspace.plan?.status_filter==='completed'?'completed':'valid'):'all',page_size:Number($("#pageSize").value),profile:$("#profileSelect").value,search:$("#documentSearch").value,issue:$("#issueFilter").value,offset:0};
  delete filters.limit;
  if(options.auto && workspace.plan) filters={...workspace.plan};
  if(workspace.requestedKind) {
    if(workspace.pending){rememberConversation(null);workspace.pending=null;}
    filters={...filters,kind:workspace.requestedKind,seller_query:null,target_amount:null,comparison:null,issue:'all',search:'',profile:'panel'};
    workspace.requestedKind=null;
  } else if(!workspace.plan) filters.kind=state.kind;
  filters={...filters,...workspace.extra}; workspace.extra={};
  state.busy=true;setBusy(true);showLoading();
  try {
    const response=await postWorkspace('conversation',{conversation_id:workspace.conversationId,message:question||'',filters,action:question?'message':'refine'});
    await receiveWorkspace(response);
    hideSetup();
    if(question&&response.status!=='native_reports') scrollToResults();
  } catch(error) {showError(error.message); $("#evidenceToolbar").classList.add('hidden');}
  finally {state.busy=false;setBusy(false);runPending();}
};

async function sendAnswer(answer) {
  if(state.busy) return;
  state.busy=true;setBusy(true);
  try {await receiveWorkspace(await postWorkspace('conversation',{conversation_id:workspace.conversationId,answer}));}
  catch(error){toast(error.message);}
  finally{state.busy=false;setBusy(false);}
}

async function refine(values) {workspace.extra=values;await execute();}
$$('.report-card').forEach(card=>card.addEventListener('click',()=>{workspace.requestedKind=card.dataset.kind;},true));
$('#newConversation').onclick=()=>{rememberConversation(null);workspace.plan=null;workspace.runId=null;workspace.pending=null;state.investigation=null;state.kind='revenue';workspace.extra={kind:'revenue',seller_query:null,comparison:null};renderConversation([]);$('#question').value='';$('#question').focus();toast('Nova conversa iniciada.');};
$('#profileSelect').onchange=()=>refine({profile:$('#profileSelect').value});
$('#qualityRun').onclick=()=>refine({kind:'quality',comparison:null,offset:0});
$('#previousPage').onclick=()=>refine({offset:Math.max(0,workspace.plan.offset-workspace.plan.page_size)});
$('#nextPage').onclick=()=>refine({offset:workspace.plan.offset+workspace.plan.page_size});
$('#pageSize').onchange=()=>refine({page_size:Number($('#pageSize').value),offset:0});
$('#issueFilter').onchange=()=>refine({issue:$('#issueFilter').value,offset:0,kind:'quality'});
let searchTimer;
$('#documentSearch').oninput=()=>{clearTimeout(searchTimer);searchTimer=setTimeout(()=>refine({search:$('#documentSearch').value,offset:0}),500);};

function openWorkspaceDialog(title,html) {
  $('#workspaceDialogTitle').textContent=title;
  $('#workspaceDialogBody').innerHTML=html;
  $('#workspaceDialogMessage').textContent='';
  if(!$('#workspaceDialog').open)$('#workspaceDialog').showModal();
}
$('#workspaceDialogClose').onclick=()=>$('#workspaceDialog').close();
const modalMessage=(text,error=false)=>{const p=$('#workspaceDialogMessage');p.textContent=text;p.className=error?'status-failure':'status-success';};
const guarded=handler=>async(event)=>{event?.preventDefault();try{await handler(event);}catch(error){if($('#workspaceDialog').open)modalMessage(error.message,true);else toast(error.message);}};

$('#saveInvestigation').onclick=guarded(async()=>{
  if(!workspace.runId)throw Error('Execute uma consulta primeiro.');
  await postWorkspace('saved',{run_id:workspace.runId,title:$('#savedTitle').value.trim()||'Investigação'});
  toast('Investigação salva com os valores desta consulta.');
});

async function openRun(key) {
  await receiveWorkspace(await postWorkspace('conversation/resume',{run_id:key}));
  $('#workspaceDialog').close();scrollToResults();
}

$('#savedOpen').onclick=guarded(async()=>{
  const data=await api('/api/workspace/saved');
  openWorkspaceDialog('Investigações salvas',`<p>Abra uma cópia com os números originais. Executar ou continuar a conversa consulta a base atual.</p><button type="button" class="secondary" id="backupState">Baixar backup do espaço de trabalho</button>${data.saved.length?data.saved.map(s=>`<article class="workspace-item"><h3>${esc(s.title)}</h3><p>${esc(new Date(s.created).toLocaleString('pt-BR'))}</p><div class="item-actions"><button type="button" class="secondary" data-open-run="${esc(s.run_id)}">Abrir cópia salva</button><button type="button" class="ghost" data-remove-saved="${esc(s.id)}">Remover da lista</button></div></article>`).join(''):'<p>Nenhuma investigação salva ainda.</p>'}`);
  const searchLabel=document.createElement('label');
  searchLabel.textContent='Buscar nas últimas 200 investigações salvas';
  const search=document.createElement('input');search.type='search';search.maxLength=120;search.placeholder='Digite parte do título';
  searchLabel.append(search);$('#workspaceDialogBody').prepend(searchLabel);
  const empty=document.createElement('p');empty.textContent='Nenhuma investigação corresponde à busca.';empty.hidden=true;$('#workspaceDialogBody').append(empty);
  search.oninput=()=>{
    let matches=0;
    $$('#workspaceDialogBody .workspace-item').forEach(article=>{const show=article.querySelector('h3').textContent.toLocaleLowerCase('pt-BR').includes(search.value.trim().toLocaleLowerCase('pt-BR'));article.hidden=!show;if(show)matches++;});
    empty.hidden=matches>0||!data.saved.length;
  };
  $$('[data-open-run]').forEach(b=>{
    b.onclick=guarded(()=>openRun(b.dataset.openRun));
    const saved=data.saved.find(s=>s.run_id===b.dataset.openRun&&s.id===b.closest('article').querySelector('[data-remove-saved]').dataset.removeSaved);
    const form=document.createElement('form');form.className='item-actions';
    const title=document.createElement('input');title.value=saved.title;title.required=true;title.maxLength=120;title.setAttribute('aria-label','Novo título da investigação');
    const rename=document.createElement('button');rename.type='submit';rename.className='secondary';rename.textContent='Renomear';form.append(title,rename);b.closest('article').append(form);
    form.onsubmit=guarded(async()=>{
      if(!title.value.trim())throw Error('Informe um título.');
      rename.disabled=true;
      try{const updated=await api(`/api/workspace/saved/${saved.id}`,{method:'PATCH',body:JSON.stringify({title:title.value})});saved.title=updated.title;title.value=updated.title;b.closest('article').querySelector('h3').textContent=updated.title;search.oninput();toast('Título atualizado.');}
      finally{rename.disabled=false;}
    });
  });
  $$('[data-remove-saved]').forEach(b=>b.onclick=guarded(async()=>{await api(`/api/workspace/saved/${b.dataset.removeSaved}`,{method:'DELETE'});b.closest('article').remove();}));
  $('#backupState').onclick=guarded(()=>downloadAuthenticated('/api/workspace/state/backup','DigiSat-workspace.sqlite3',{method:'POST'}));
});

$('#compareOpen').onclick=()=>{
  const plan=workspace.plan||{start:$('#start').value,end:$('#end').value};
  openWorkspaceDialog('Comparar períodos, vendedores ou empresas',`<p>A base continua com os filtros atuais. Defina abaixo o segundo conjunto; a diferença será comparação menos base.</p><form id="comparisonForm"><div class="workspace-form-grid"><label>Início<input name="start" type="date" value="${esc(plan.start)}" required></label><label>Fim<input name="end" type="date" value="${esc(plan.end)}" required></label><label>Vendedor (vazio = todos)<input name="seller" value="${esc(plan.seller_query||'')}" maxlength="120"></label><label>Empresa (nome exato; vazio = todas)<input name="company" value="${esc(plan.company_value||'')}" maxlength="120"></label></div><div class="dialog-actions"><button class="primary">Aplicar comparação</button></div></form>`);
  $('#comparisonForm').onsubmit=guarded(async e=>{const f=new FormData(e.target);const c={start:f.get('start'),end:f.get('end'),seller_query:f.get('seller')||null,company_value:f.get('company')||null,label:'Comparação selecionada'};$('#workspaceDialog').close();await refine({comparison:c,kind:'investigation',offset:0});});
};

async function downloadAuthenticated(url,name,options={}) {
  const r=await fetch(url,{...options,headers:state.auth?{Authorization:state.auth}:{}});
  if(!r.ok) {const data=await r.json().catch(()=>({}));throw Error(data.detail||'Não foi possível baixar o arquivo.');}
  const blob=await r.blob();const object=URL.createObjectURL(blob);const link=document.createElement('a');link.href=object;link.download=name;link.click();setTimeout(()=>URL.revokeObjectURL(object),2000);
}

async function exportEvidence(format) {
  if(workspace.exportBusy||!workspace.runId)return;
  workspace.exportBusy=true;const note=$('#exportProgress');
  try {
    const job=await postWorkspace('exports',{run_id:workspace.runId,format,complete:$('#exportScope').value==='complete'});
    $('#cancelExport').classList.remove('hidden');$('#cancelExport').disabled=false;
    $('#cancelExport').onclick=async()=>{await postWorkspace(`exports/${job.id}/cancel`,{});$('#cancelExport').disabled=true;note.textContent='Cancelamento solicitado…';};
    note.textContent='Exportação em preparação…';
    let finished=false;
    for(let attempt=0;attempt<3600;attempt++) {
      const current=await api(`/api/workspace/exports/${job.id}`);
      if(current.status==='failed'||current.status==='cancelled')throw Error(current.error);
      if(current.status==='succeeded'){await downloadAuthenticated(current.download,`DigiSat-evidencias.${format}`);note.textContent=`Arquivo pronto · ${current.records} registros exportados.`;finished=true;break;}
      note.textContent=`Preparando ${format.toUpperCase()} · ${current.records} registros processados. Mantenha o aplicativo aberto.`;
      await new Promise(resolve=>setTimeout(resolve,1000));
    }
    if(!finished)throw Error('A exportação ainda não terminou. Verifique o aplicativo antes de tentar novamente.');
  } catch(error){note.textContent=error.message;}
  finally{workspace.exportBusy=false;$('#cancelExport').classList.add('hidden');}
}
$$('[data-evidence-format]').forEach(b=>b.onclick=()=>exportEvidence(b.dataset.evidenceFormat));

$('#connectionOpen').onclick=guarded(async()=>{
  const c=await api('/api/workspace/connection');
  openWorkspaceDialog('Configurar e testar conexão',`<p>O aplicativo consulta a base existente. Nenhum comando para criar usuários ou alterar dados será executado. Peça ao administrador uma conta dedicada com permissão read.</p><form id="connectionForm"><div class="workspace-form-grid">${[['host','Servidor',c.host],['port','Porta',c.port],['database','Banco',c.database],['username','Usuário (opcional)',c.username],['auth_source','Banco de autenticação',c.auth_source]].map(([name,label,value])=>`<label>${label}<input name="${name}" value="${esc(value||'')}" ${name==='port'?'type="number" min="1" max="65535"':'maxlength="120"'} ${name==='username'?'':'required'}></label>`).join('')}<label>Senha (vazio = manter a atual)<input type="password" name="password" autocomplete="new-password" maxlength="1000"></label></div><label class="check-label"><input type="checkbox" name="readonly" ${c.require_read_only?'checked':''}>Exigir que o MongoDB comprove permissões somente de leitura</label><p>Salvar testa a conexão, preserva a configuração anterior e aplica os novos dados.</p><div class="dialog-actions"><button type="button" class="secondary" id="testConnection">Testar conexão</button><button class="primary">Testar e salvar</button></div></form>`);
  const settings=()=>{const f=new FormData($('#connectionForm'));return {host:f.get('host'),port:Number(f.get('port')),database:f.get('database'),username:f.get('username'),auth_source:f.get('auth_source'),password:f.get('password')||null,require_read_only:f.has('readonly')};};
  const configuration=document.createElement('section');configuration.className='connection-context';
  configuration.innerHTML=`<h3>Configuração desta instalação</h3><dl><div><dt>Arquivo de configuração</dt><dd>${esc(c.configuration_file||'Não identificado')}</dd></div><div><dt>Arquivo de senha</dt><dd>${c.password_configured?(c.password_file_exists?'Encontrado · será usado se a senha ficar vazia':'Não encontrado · informe uma senha ou revise MONGO_PASSWORD_FILE'):'Não configurado · informe a senha se o usuário exigir autenticação'}</dd></div></dl><p>Os valores do formulário só serão aplicados ao salvar. Variáveis de ambiente definidas antes de iniciar o aplicativo têm prioridade sobre o arquivo.</p>`;
  $('#connectionForm').before(configuration);
  const result=document.createElement('section');result.id='connectionCheck';result.className='connection-check';result.hidden=true;result.setAttribute('aria-live','polite');$('#connectionForm').after(result);
  let busy=false,lastSummary='';
  const form=$('#connectionForm');
  const setBusy=value=>{busy=value;form.setAttribute('aria-busy',String(value));form.querySelectorAll('input,button').forEach(el=>el.disabled=value);};
  $('#connectionForm').addEventListener('input',()=>{if(!busy){result.hidden=true;lastSummary='';modalMessage('Filtros de conexão alterados. Teste novamente antes de salvar.');}});
  $('#testConnection').onclick=guarded(async()=>{
    if(busy||!$('#connectionForm').reportValidity())return;
    const scope=settings();setBusy(true);result.hidden=true;modalMessage('Verificando conexão…');
    try{
      const r=await postWorkspace('connection/inspect',scope);
      if(!form.isConnected||!$('#workspaceDialog').open)return;
      const labels={ok:'Concluído',attention:'Revisar',failed:'Falhou',skipped:'Não executado'};
      result.innerHTML=`<h3>${r.ready?'Conexão testada':'Conexão precisa de ajuste'}</h3><ol>${r.checks.map(check=>`<li><span>${esc(check.label)}</span><strong data-check-status="${esc(check.status)}">${esc(labels[check.status]||'Aguardando')}</strong></li>`).join('')}</ol><p>${esc(r.message)}</p>${r.security&&!r.security.verified_read_only?`<p>${esc(r.security.message)}</p>`:''}<p>O teste não salva a configuração.</p><button type="button" class="secondary" id="copyConnectionCheck">Copiar diagnóstico sem senha</button>`;
      result.hidden=false;result.scrollIntoView({block:'nearest',behavior:'smooth'});modalMessage(r.ready?'Teste concluído. Salve para aplicar.':'Revise a etapa indicada e teste novamente.',!r.ready);
      lastSummary=[`DigiSat Insight · diagnóstico de conexão`, `Servidor: ${scope.host}:${scope.port}`, `Banco: ${scope.database}`, `Usuário: ${scope.username||'(sem usuário)'}`, `Banco de autenticação: ${scope.auth_source}`, `Exigir leitura: ${scope.require_read_only?'sim':'não'}`,...r.checks.map(x=>`${x.label}: ${labels[x.status]}`),r.message,r.security?.message||''].filter(Boolean).join('\n');
      $('#copyConnectionCheck').onclick=guarded(async()=>{await navigator.clipboard.writeText(lastSummary);modalMessage('Diagnóstico copiado sem senha.');});
    }catch(error){if(form.isConnected&&$('#workspaceDialog').open)modalMessage(error.message,true);}
    finally{setBusy(false);}
  });
  $('#connectionForm').onsubmit=guarded(async()=>{
    if(busy)return;const scope=settings();setBusy(true);result.hidden=true;modalMessage('Testando e salvando…');
    try{const r=await postWorkspace('connection/save',scope);if(form.isConnected)modalMessage(r.message);rememberConversation(null);workspace.plan=null;await init();}
    finally{setBusy(false);}
  });
});

$('#diagnosticsOpen').onclick=guarded(async()=>{
  openWorkspaceDialog('Diagnóstico da instalação','<p>Verificando conexão, permissões, índices e consultas recentes…</p>');
  const d=await api('/api/workspace/diagnostics');
  $('#workspaceDialogBody').innerHTML=`<p>Versão ${esc(d.version)} · ${d.connected?'MongoDB conectado':'MongoDB indisponível'} · ${fmtNumber(d.latency_ms)} ms</p><p>${esc(d.security?.message||d.message||'')}</p><p>Primeiro documento: ${esc(d.first_document||'não disponível')}<br>Último documento: ${esc(d.last_document||'não disponível')}</p><p>Dados locais graváveis: ${d.data_directory_writable?'sim':'não'}.</p><p>Metas de desempenho: consultas usuais até 5 s; investigações até 20 s. Valide com seu volume real. O aplicativo não cria índices.</p><details><summary>Índices disponíveis</summary><pre class="workspace-diagnostic">${esc((d.indexes||[]).join('\n'))}</pre></details><h3>Consultas e eventos recentes</h3>${(d.recent_queries||[]).map(r=>`<p>${esc(r.created)} · ${esc(r.category)} · ${r.duration_ms==null?'':`${fmtNumber(r.duration_ms)} ms`} · ${esc(r.detail)}</p>`).join('')}`;
  const c=d.configuration;
  if(c){const context=document.createElement('section');context.className='connection-context';context.innerHTML=`<h3>Configuração desta instalação</h3><dl><div><dt>Arquivo</dt><dd>${esc(c.configuration_file)}</dd></div><div><dt>Senha em arquivo</dt><dd>${c.password_configured?(c.password_file_exists?'Encontrado':'Não encontrado'):'Não configurada'}</dd></div><div><dt>Verificação somente leitura</dt><dd>${c.require_read_only?'Exigida':'Opcional'}</dd></div></dl>`;$('#workspaceDialogBody').prepend(context);}
});

function openScheduleForm() {
  if(!workspace.runId){toast('Execute uma investigação primeiro.');return;}
  openWorkspaceDialog('Agendar conferência',`<p>A conferência executa enquanto o DigiSat Insight está aberto. Alertas ficam neste aplicativo. O resultado precisa mudar e ultrapassar o limite para criar um novo alerta.</p><form id="scheduleForm"><div class="workspace-form-grid"><label>Título<input name="title" required maxlength="120" value="${esc($('#savedTitle').value||'Conferência')}"/></label><label>Intervalo em minutos<input name="interval" type="number" min="1" max="10080" value="60" required></label><label>Período<select name="period"><option value="yesterday">Sempre ontem</option><option value="today">Sempre hoje</option><option value="fixed">Manter as datas desta investigação</option></select></label><label>O que acompanhar<select name="metric"><option value="Diferenca">Diferença entre itens e pagamentos</option><option value="Outros">Itens de outros/sem vendedor</option><option value="quality_issues">Quantidade de ocorrências de qualidade</option></select></label><label>Alertar acima de<input name="threshold" type="number" min="0" step="0.01" value="0.01" required></label></div><label class="check-label"><input type="checkbox" name="enabled">Ativar execuções automáticas</label><div class="dialog-actions"><button class="primary">Criar agendamento</button></div></form>`);
  $('#scheduleForm').onsubmit=guarded(async e=>{const f=new FormData(e.target);await postWorkspace('schedules',{run_id:workspace.runId,title:f.get('title'),interval_minutes:Number(f.get('interval')),period_policy:f.get('period'),metric:f.get('metric'),threshold:Number(f.get('threshold')),enabled:f.has('enabled')});modalMessage('Agendamento criado. Você pode executar agora ou ativá-lo na lista.');await refreshAlerts();});
}
$('#scheduleCurrent').onclick=openScheduleForm;

async function refreshAlerts() {
  try {const a=await api('/api/workspace/alerts');const n=a.alerts.filter(r=>!r.seen).length;$('#alertCount').textContent=n?`(${n})`:'';}catch(_){}
}
$('#schedulesOpen').onclick=guarded(async()=>{
  const [s,a]=await Promise.all([api('/api/workspace/schedules'),api('/api/workspace/alerts')]);
  openWorkspaceDialog('Agendamentos e alertas',`<p>${esc(s.note)}</p><h3>Agendamentos</h3>${s.schedules.length?s.schedules.map(r=>`<article class="workspace-item"><h3>${esc(r.title)}</h3><p>${r.enabled?'Ativo':'Pausado'} · a cada ${r.interval_minutes} min · último resultado: ${esc(r.last_status)}${r.last_error?' · '+esc(r.last_error):''}</p><div class="item-actions"><button type="button" class="secondary" data-run-schedule="${r.id}">Executar agora</button><button type="button" class="secondary" data-toggle-schedule="${r.id}" data-enabled="${!r.enabled}">${r.enabled?'Pausar':'Ativar'}</button><button type="button" class="ghost" data-delete-schedule="${r.id}">Remover</button></div></article>`).join(''):'<p>Nenhum agendamento. Execute uma investigação e use Agendar conferência.</p>'}<h3>Alertas</h3>${a.alerts.length?a.alerts.map(r=>`<article class="workspace-item"><h3>${esc(r.body.title)}</h3><p>${esc(r.created)} · ${esc(r.body.metric)}: ${fmtNumber(r.body.value)} · limite ${fmtNumber(r.body.threshold)}</p><p>${esc(r.body.message)}</p><button class="secondary" type="button" data-alert-run="${r.body.run_id}" data-alert-id="${r.id}">Abrir evidência</button></article>`).join(''):'<p>Nenhum alerta.</p>'}`);
  $$('[data-run-schedule]').forEach(b=>b.onclick=guarded(async()=>{b.disabled=true;modalMessage('Executando…');const r=await postWorkspace(`schedules/${b.dataset.runSchedule}/run`,{});modalMessage(r.last_status==='succeeded'?'Conferência concluída. Abra a lista novamente para ver os alertas.':r.last_error,r.last_status!=='succeeded');b.disabled=false;await refreshAlerts();}));
  $$('[data-toggle-schedule]').forEach(b=>b.onclick=guarded(async()=>{await api(`/api/workspace/schedules/${b.dataset.toggleSchedule}`,{method:'PATCH',body:JSON.stringify({enabled:b.dataset.enabled==='true'})});modalMessage('Agendamento atualizado.');b.disabled=true;}));
  $$('[data-delete-schedule]').forEach(b=>b.onclick=guarded(async()=>{await api(`/api/workspace/schedules/${b.dataset.deleteSchedule}`,{method:'DELETE'});b.closest('article').remove();}));
  $$('[data-alert-run]').forEach(b=>b.onclick=guarded(async()=>{await postWorkspace(`alerts/${b.dataset.alertId}/seen`,{});await openRun(b.dataset.alertRun);await refreshAlerts();}));
});
setTimeout(refreshAlerts,1000);
setInterval(refreshAlerts,60000);
