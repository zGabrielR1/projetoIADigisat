/* MongoDB editor. Credentials/tokens stay in memory; document values are inert text. */
(() => {
  'use strict';
  const $=s=>document.querySelector(s);
  const state={auth:null,database:null,collection:null,token:null,expires:0,query:null,document:null,preview:null,generation:0,querying:false,applying:false};
  const status=(text,error=false)=>{$('#editorStatus').textContent=text;$('#editorStatus').classList.toggle('error',error);};
  const pretty=text=>JSON.stringify(JSON.parse(text),null,2);
  function formatDraft(text){
    // Parsing ordinary JSON in JavaScript would round untagged Int64 values.
    // Server-returned documents use canonical BSON tags; protect new/pasted drafts.
    const tokens=/"(?:\\.|[^"\\])*"|(-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?)/g;
    for(const match of text.matchAll(tokens)){if(match[1]&&Number.isInteger(Number(match[1]))&&!Number.isSafeInteger(Number(match[1])))throw Error('Use $numberLong ou $numberDouble em Extended JSON para formatar números grandes sem perder precisão.');}
    return pretty(text);
  }
  const guard=fn=>async event=>{event?.preventDefault();try{await fn(event);}catch(error){status(error.message,true);const dialog=document.querySelector('dialog[open]');if(dialog){let note=dialog.querySelector('.operation-error');if(!note){note=node('p',undefined,'operation-error');note.setAttribute('role','alert');dialog.append(note);}note.textContent=error.message;}}};
  const node=(tag,text,cls)=>{const n=document.createElement(tag);if(text!==undefined)n.textContent=text;if(cls)n.className=cls;return n;};
  let authPromise=null;
  function authenticate(){
    if(authPromise)return authPromise;
    authPromise=new Promise((resolve,reject)=>{
      const dialog=$('#editorAuth');dialog.showModal();$('#editorUser').focus();
      const finish=(ok)=>{
        if(ok){const bytes=new TextEncoder().encode($('#editorUser').value+':'+$('#editorPassword').value);state.auth='Basic '+btoa(String.fromCharCode(...bytes));}
        $('#editorPassword').value='';dialog.close();authPromise=null;
        if(ok)resolve();else reject(Error('Autenticação cancelada.'));
      };
      $('#editorAuthForm').onsubmit=e=>{e.preventDefault();finish(true);};
      $('#cancelAuth').onclick=()=>finish(false);
      dialog.oncancel=e=>{e.preventDefault();finish(false);};
    });
    return authPromise;
  }
  async function api(path,body,options={}){
    const send=()=>fetch('/api/editor/'+path,{method:body===undefined?'GET':'POST',headers:{...(body===undefined?{}:{'Content-Type':'application/json'}),...(state.auth?{Authorization:state.auth}:{}),...(state.token?{'X-Insight-Editor-Session':state.token}:{}),...(options.headers||{})},...(body===undefined?{}:{body:JSON.stringify(body)})});
    let response=await send();
    if(response.status===401){state.auth=null;await authenticate();response=await send();}
    const data=await response.json().catch(()=>({}));
    if(!response.ok){const detail=Array.isArray(data.detail)?data.detail.map(x=>x.msg).join('; '):data.detail;throw Error(detail||'Não foi possível concluir a operação.');}
    return data;
  }
  function dirty(){const d=state.document;return d&&$('#documentJson').value!==d.display;}
  function discard(){return !dirty()||window.confirm('Descartar as alterações ainda não aplicadas neste documento?');}
  function enabled(){return Boolean(state.token&&state.expires>Date.now());}
  function controls(){
    const active=enabled(),doc=state.document;
    $('#writeState').textContent=active?`Edição habilitada · ${Math.max(0,Math.ceil((state.expires-Date.now())/60000))} min`:'Edição bloqueada';
    $('#writeState').classList.toggle('unlocked',active);
    $('#unlockWrites').hidden=active;$('#unlockWrites').disabled=!state.database||state.applying;
    $('#lockWrites').hidden=!active;$('#lockWrites').disabled=state.applying;
    $('#writeHistory').disabled=!active||state.applying;
    $('#newDocument').disabled=!active||!state.collection||state.applying;
    $('#documentJson').readOnly=!active||!doc||state.applying;
    $('#previewDocument').disabled=!active||!doc||state.applying;
    $('#deleteDocument').disabled=!active||!doc||doc.operation==='insert'||state.applying;
    $('#downloadOriginal').disabled=!doc||doc.operation==='insert';
  }
  async function lock(){
    const token=state.token;state.token=null;state.expires=0;state.preview=null;controls();
    if(token)await api('lock',{}, {headers:{'X-Insight-Editor-Session':token}});
  }
  function clearDocument(){state.document=null;state.preview=null;$('#documentJson').value='';$('#documentHeading').textContent='Editor de documento';$('#documentContext').textContent='Abra um documento completo para editar. Tipos BSON são preservados em Extended JSON.';controls();}
  function namespace(){return state.database&&state.collection?`${state.database} / ${state.collection}`:state.database||'Nenhuma coleção selecionada';}
  async function selectCollection(database,collection){
    if(state.applying||!discard())return;
    state.generation++;
    if(state.database!==database)await lock();
    state.database=database;state.collection=collection;state.query=null;
    $('#selectedNamespace').textContent=namespace();$('#queryOffset').value=0;
    $('#sqlText').value=`SELECT * FROM \`${collection.replace(/`/g,'``')}\` LIMIT 50`;
    $('#documentResults').replaceChildren(node('p','Coleção selecionada. Execute uma consulta para carregar documentos.','editor-empty'));
    $('#previousResults').disabled=true;$('#nextResults').disabled=true;$('#resultCount').textContent='';
    document.querySelectorAll('.collection-item').forEach(b=>b.classList.toggle('selected',b.dataset.database===database&&b.dataset.collection===collection));
    clearDocument();status('Coleção selecionada: '+namespace());
  }
  function filterCollections(){const text=$('#collectionSearch').value.trim().toLocaleLowerCase();document.querySelectorAll('.collection-item').forEach(b=>b.hidden=!b.textContent.toLocaleLowerCase().includes(text));}
  async function tree(){
    status('Carregando bancos…');
    const result=await api('databases');
    $('#connectionLabel').textContent=`${result.host}:${result.port} · ${result.username||'sem usuário configurado'} · conexão atual do Insight`;
    const container=$('#databaseTree');container.replaceChildren();
    for(const name of result.databases){
      const details=node('details'),summary=node('summary',name),list=node('div');details.append(summary,list);container.append(details);
      let loaded=false,loading=false;
      details.addEventListener('toggle',guard(async()=>{
        if(!details.open||loaded||loading)return;
        loading=true;list.textContent='Carregando coleções…';
        try{
          const data=await api('collections?database='+encodeURIComponent(name));list.replaceChildren();
          for(const collection of data.collections){const button=node('button',collection,'collection-item');button.type='button';button.dataset.database=name;button.dataset.collection=collection;button.onclick=guard(()=>selectCollection(name,collection));list.append(button);}
          if(!data.collections.length)list.append(node('p','Nenhuma coleção disponível.','editor-note'));
          loaded=true;filterCollections();
        }catch(error){list.textContent=error.message;throw error;}finally{loading=false;}
      }));
      if(name===result.selected)details.open=true;
    }
    status('Selecione uma coleção na árvore.');
  }
  function addCondition(){
    if($('#conditionRows').children.length>=30){status('Use até 30 condições.',true);return;}
    const row=node('div',undefined,'condition-row');
    const field=node('input');field.placeholder='Campo, ex.: Descricao';field.maxLength=120;field.setAttribute('aria-label','Campo da condição');
    const operator=node('select');operator.setAttribute('aria-label','Operador');
    for(const [value,label] of [['eq','Igual'],['ne','Diferente'],['gt','Maior'],['gte','Maior ou igual'],['lt','Menor'],['lte','Menor ou igual'],['contains','Contém'],['in','Em lista'],['nin','Fora da lista'],['exists','Existe']]){const option=node('option',label);option.value=value;operator.append(option);}
    const type=node('select');type.setAttribute('aria-label','Tipo do valor');
    for(const [value,label] of [['string','Texto'],['number','Número'],['boolean','Booleano'],['null','Nulo'],['objectid','ObjectId'],['date','Data'],['json','JSON']]){const option=node('option',label);option.value=value;type.append(option);}
    const value=node('input');value.placeholder='Valor';value.maxLength=1000;value.setAttribute('aria-label','Valor da condição');
    const remove=node('button','Remover','ghost');remove.type='button';remove.onclick=()=>row.remove();
    row.append(field,operator,type,value,remove);$('#conditionRows').append(row);
  }
  function valueJson(type,value){
    if(type==='number'){const n=Number(value);if(!value.trim()||!Number.isFinite(n))throw Error('Informe um número válido.');if(Number.isInteger(n)&&!Number.isSafeInteger(n))throw Error('Para inteiros grandes use JSON com $numberLong.');return JSON.stringify(n);}
    if(type==='boolean'){if(!['true','false'].includes(value.toLowerCase()))throw Error('Use true ou false para booleanos.');return value.toLowerCase();}
    if(type==='null')return 'null';
    if(type==='objectid')return JSON.stringify({$oid:value});
    if(type==='date'){if(!/^\d{4}-\d{2}-\d{2}$/.test(value))throw Error('Data deve ser AAAA-MM-DD.');return JSON.stringify({$date:value+'T00:00:00Z'});}
    if(type==='json'){JSON.parse(value);return value;}
    return JSON.stringify(value);
  }
  function request(){
    if(!state.database||!state.collection)throw Error('Selecione uma coleção na árvore.');
    const conditions=$('#queryMode').value==='builder'?[...$('#conditionRows').children].map(row=>{const [field,op,type,value]=row.children;return {field:field.value,operator:op.value,value_json:valueJson(type.value,value.value)};}):[];
    return {database:state.database,collection:state.collection,mode:$('#queryMode').value,conditions,conjunction:$('#builderConjunction').value,filter_json:$('#filterJson').value,projection_json:$('#projectionJson').value,sort_json:$('#sortJson').value,sql:$('#sqlText').value,limit:Number($('#queryLimit').value),offset:Number($('#queryOffset').value)};
  }
  function treeValue(key,value){
    if(value===null||typeof value!=='object')return node('div',`${key}: ${typeof value==='string'?JSON.stringify(value):String(value)}`,'tree-value');
    const keys=Object.keys(value);
    if(keys.length===1&&['$oid','$numberInt','$numberLong','$numberDouble','$numberDecimal','$date'].includes(keys[0]))return node('div',`${key}: ${keys[0]} ${typeof value[keys[0]]==='object'?JSON.stringify(value[keys[0]]):value[keys[0]]}`,'tree-value');
    const details=node('details'),summary=node('summary',`${key}: ${Array.isArray(value)?'Array':'Object'} (${keys.length})`);details.append(summary);
    let rendered=false;
    details.addEventListener('toggle',()=>{if(!details.open||rendered)return;rendered=true;for(const child of keys.slice(0,200))details.append(treeValue(child,value[child]));if(keys.length>200)details.append(node('p','Árvore limitada a 200 itens por nível. Abra o JSON completo para ver todos.','editor-note'));});
    return details;
  }
  async function openDocument(context,id){
    if(state.applying||!discard())return;
    const generation=state.generation;
    const result=await api('document',{database:context.database,collection:context.collection,id_json:id});
    if(generation!==state.generation)return;
    const display=pretty(result.json);
    state.document={...context,id_json:result.id_json,original:result.json,display,operation:'update'};
    $('#documentJson').value=display;$('#documentHeading').textContent='Editar documento';$('#documentContext').textContent=`${context.database} / ${context.collection} · _id: ${result.id_json}`;
    $('#draftStatus').textContent='Documento completo carregado. Nenhuma escrita enviada.';controls();$('#documentJson').scrollIntoView({block:'nearest',behavior:'smooth'});
  }
  async function execute(payload){
    if(state.querying||state.applying)return;
    state.querying=true;$('#runQuery').disabled=true;const generation=state.generation;
    try{
      status('Consultando…');const result=await api('query',payload);
      if(generation!==state.generation)return;
      state.query={payload,result};
      $('#translatedQuery').textContent=JSON.stringify(result.translated,null,2);
      $('#resultCount').textContent=`· ${result.documents.length} nesta página`;
      $('#queryOffset').value=result.offset;$('#queryLimit').value=result.limit;
      $('#previousResults').disabled=result.offset===0;$('#nextResults').disabled=!result.has_more;
      const container=$('#documentResults');container.replaceChildren();
      for(const item of result.documents){
        const article=node('article',undefined,'document-result'),head=node('div',undefined,'document-result-head');
        head.append(node('code',item.id_json===null?'_id não incluído na projeção':'_id: '+item.id_json));
        const open=node('button','Abrir documento completo','secondary');open.type='button';open.disabled=item.id_json===null;open.onclick=guard(()=>openDocument({database:result.database,collection:result.collection},item.id_json));head.append(open);
        const values=node('div',undefined,'document-tree');for(const [key,value] of Object.entries(JSON.parse(item.json)))values.append(treeValue(key,value));article.append(head,values);container.append(article);
      }
      if(!result.documents.length)container.append(node('p','Nenhum documento encontrado.','editor-empty'));
      status(`Consulta concluída: ${result.database} / ${result.collection} · ${result.documents.length} documentos exibidos${result.has_more?' · há mais resultados':''}.`);
    }finally{state.querying=false;$('#runQuery').disabled=false;}
  }
  function download(text,name){const url=URL.createObjectURL(new Blob([pretty(text)],{type:'application/json'}));const a=node('a');a.href=url;a.download=name;a.click();setTimeout(()=>URL.revokeObjectURL(url),2000);}
  async function prepare(operation){
    const doc=state.document;if(!doc||!enabled())throw Error('Abra um documento e habilite a edição.');
    const payload={database:doc.database,collection:doc.collection,operation,id_json:doc.id_json||null,original_json:doc.original||null,document_json:operation==='delete'?null:$('#documentJson').value};
    const result=await api('preview',payload);
    state.preview=result;
    $('#previewHeading').textContent={insert:'Inserir documento',update:'Aplicar alterações',delete:'Excluir documento permanentemente'}[operation];
    $('#previewNamespace').textContent=`${result.database} / ${result.collection} · uma operação em um documento`;
    $('#previewBefore').value=pretty(result.before_json);$('#previewAfter').value=pretty(result.after_json);
    $('#previewNotice').textContent=operation==='delete'?'A exclusão é permanente no MongoDB. O original foi copiado para o diário local antes da aplicação. Em MongoDB 3.4, um campo novo adicionado por outro processo no último instante pode não bloquear a exclusão.':'O editor gravou uma prévia e a cópia original no diário local. Campos alterados serão conferidos antes da escrita. Campos novos independentes adicionados por outro processo são preservados nas edições.';
    $('#deleteConfirmLabel').hidden=operation!=='delete';$('#deleteConfirm').value='';$('#previewError').textContent='';$('#applyChange').disabled=false;
    $('#previewDialog').showModal();
  }
  $('#queryForm').onsubmit=guard(async()=>execute(request()));
  $('#queryMode').onchange=()=>{const mode=$('#queryMode').value;$('#builderPanel').hidden=mode!=='builder';$('#jsonPanel').hidden=mode!=='json';$('#sqlPanel').hidden=mode!=='sql';$('#queryOptions').hidden=mode==='sql';};
  $('#addCondition').onclick=addCondition;$('#collectionSearch').oninput=filterCollections;
  $('#refreshTree').onclick=guard(tree);
  $('#previousResults').onclick=guard(async()=>{const q=state.query;if(!q)return;const offset=Math.max(0,q.result.offset-q.result.limit);await execute(pageRequest(q,offset));});
  $('#nextResults').onclick=guard(async()=>{const q=state.query;if(q)await execute(pageRequest(q,q.result.offset+q.result.documents.length));});
  function pageRequest(q,offset){
    // SQL pagination uses the compiled fixed query, so LIMIT/OFFSET in the text
    // cannot accidentally re-run the first page.
    return {...q.payload,mode:'json',collection:q.result.collection,conditions:[],filter_json:q.result.translated.filter,projection_json:q.result.translated.projection,sort_json:JSON.stringify(Object.fromEntries(q.result.translated.sort)),limit:q.result.limit,offset};
  }
  $('#unlockWrites').onclick=()=>{if(!state.database)return;$('#unlockDescription').textContent='Banco: '+state.database;$('#unlockPhraseLabel').textContent='Digite EDITAR '+state.database;$('#unlockPhrase').value='';$('#unlockDialog').showModal();$('#unlockPhrase').focus();};
  $('#cancelUnlock').onclick=()=>$('#unlockDialog').close();
  $('#unlockForm').onsubmit=guard(async()=>{const database=state.database;const access=await api('unlock',{database,confirmation:$('#unlockPhrase').value});if(state.database!==database){await api('lock',{}, {headers:{'X-Insight-Editor-Session':access.token}});throw Error('O banco mudou. Habilite a edição novamente.');}state.token=access.token;state.expires=Date.now()+access.expires_in*1000;$('#unlockDialog').close();controls();status('Edição habilitada para '+database+'. Cada gravação exige revisão e aplicação explícita.');});
  $('#lockWrites').onclick=guard(async()=>{await lock();status('Sessão de escrita bloqueada. O rascunho local foi preservado.');});
  $('#closeHistory').onclick=()=>$('#historyDialog').close();
  $('#writeHistory').onclick=guard(async()=>{
    const result=await api('history?database='+encodeURIComponent(state.database));
    const list=$('#historyList');list.replaceChildren();$('#historyDetail').replaceChildren();
    for(const item of result.changes){
      const labels={insert:'Criação',update:'Edição',delete:'Exclusão',applied:'Confirmado',failed:'Recusado',unknown:'Incerto: confira o documento',applying:'Enviado: confira o documento'};
      const button=node('button',`${item.created} · ${item.collection} · ${labels[item.operation]||item.operation} · ${labels[item.status]||item.status}`,'secondary');button.type='button';
      button.onclick=guard(async()=>{
        const change=await api('history/'+encodeURIComponent(item.id));
        const detail=$('#historyDetail');detail.replaceChildren(node('h3',change.database+' / '+change.collection));
        const columns=node('div',undefined,'editor-two-columns');
        for(const [label,key] of [['Antes','before_json'],['Depois','after_json']]){const wrapper=node('label',label);const text=node('textarea');text.rows=12;text.readOnly=true;text.value=pretty(change[key]);wrapper.append(text);columns.append(wrapper);}
        detail.append(columns);
        const actions=node('div',undefined,'editor-buttons');
        for(const [label,key] of [['Baixar anterior','before_json'],['Baixar posterior','after_json']]){const button=node('button',label,'secondary');button.type='button';button.disabled=change[key]==='null';button.onclick=()=>download(change[key],`DigiSat-${change.id}-${key}.json`);actions.append(button);}
        const restore=node('button','Carregar anterior como rascunho','primary');restore.type='button';restore.disabled=change.before_json==='null'||change.status!=='applied';
        restore.onclick=guard(async()=>{
          if(!discard())return;
          let current=null;
          try{current=await api('document',{database:change.database,collection:change.collection,id_json:change.id_json});}
          catch(error){if(error.message!=='Documento não encontrado.')throw error;}
          state.document={database:change.database,collection:change.collection,id_json:current?.id_json||null,original:current?.json||null,display:current?pretty(current.json):'{}',operation:current?'update':'insert'};
          $('#documentJson').value=pretty(change.before_json);$('#documentHeading').textContent='Rascunho a partir da cópia anterior';$('#documentContext').textContent=`${change.database} / ${change.collection} · cópia de ${change.created}`;
          $('#draftStatus').textContent='Cópia carregada como rascunho. Revise antes/depois e aplique para gravar.';$('#historyDialog').close();controls();$('#documentJson').focus();
        });
        actions.append(restore);detail.append(actions);
      });
      list.append(button);
    }
    if(!result.changes.length)list.append(node('p','Nenhuma gravação enviada nesta conexão.','editor-note'));
    $('#historyDialog').showModal();
  });
  $('#newDocument').onclick=()=>{if(!enabled()||!state.collection||!discard())return;const display='{}';state.document={database:state.database,collection:state.collection,operation:'insert',display,original:null,id_json:null};$('#documentJson').value=display;$('#documentHeading').textContent='Novo documento';$('#documentContext').textContent=namespace()+' · o _id será gerado na prévia se estiver ausente.';controls();$('#documentJson').focus();};
  $('#documentJson').oninput=()=>{$('#draftStatus').textContent=dirty()?'Rascunho alterado. Revise e aplique para gravar no MongoDB.':'Nenhuma alteração pendente.';state.preview=null;};
  $('#formatDocument').onclick=guard(async()=>{if($('#documentJson').value.trim())$('#documentJson').value=formatDraft($('#documentJson').value);});
  $('#downloadOriginal').onclick=()=>{if(state.document?.original)download(state.document.original,'DigiSat-documento-original.json');};
  $('#previewDocument').onclick=guard(()=>prepare(state.document.operation));
  $('#deleteDocument').onclick=guard(()=>prepare('delete'));
  $('#cancelPreview').onclick=()=>{if(!state.applying){state.preview=null;$('#previewDialog').close();}};
  $('#previewDialog').oncancel=event=>{if(state.applying)event.preventDefault();else state.preview=null;};
  $('#applyChange').onclick=async()=>{
    if(state.applying||!state.preview)return;
    const preview=state.preview;
    if(preview.operation==='delete'&&$('#deleteConfirm').value!==preview.collection){$('#previewError').textContent='Digite o nome exato da coleção para excluir.';return;}
    state.applying=true;$('#applyChange').disabled=true;$('#cancelPreview').disabled=true;controls();
    try{
      const result=await api('apply/'+encodeURIComponent(preview.id),{confirmation:preview.operation==='delete'?$('#deleteConfirm').value:preview.collection});
      $('#previewDialog').close();state.preview=null;
      if(preview.operation==='delete')clearDocument();
      else{const display=pretty(result.after_json);state.document={database:preview.database,collection:preview.collection,id_json:preview.id_json,original:result.after_json,display,operation:'update'};$('#documentJson').value=display;$('#documentHeading').textContent='Documento gravado';$('#documentContext').textContent=`${preview.database} / ${preview.collection} · alteração ${result.id}`;}
      status(`Gravação confirmada no MongoDB · ${preview.operation} · ${preview.database} / ${preview.collection}. Execute a consulta novamente para atualizar a lista.`);
      $('#draftStatus').textContent='Escrita confirmada. O original foi preservado no diário local.';
    }catch(error){$('#previewError').textContent=error.message;status(error.message,true);state.preview=null;}
    finally{state.applying=false;$('#cancelPreview').disabled=false;controls();}
  };
  window.addEventListener('beforeunload',event=>{if(dirty()||state.applying){event.preventDefault();event.returnValue='';}});
  setInterval(()=>{if(state.token&&state.expires<=Date.now()){state.token=null;state.expires=0;status('A sessão de escrita expirou. O rascunho foi preservado.');}controls();},1000);
  controls();tree().catch(error=>status(error.message,true));
})();
