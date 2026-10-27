/* Stock queries own their filters and result snapshots, independently of sales. */
$('#stockOpen').onclick=guarded(async()=>{
  openWorkspaceDialog('Validade de lotes','<p>Carregando empresas…</p>');
  const data=await api('/api/workspace/stock/companies');
  if(!$('#workspaceDialog').open)return;
  if(!data.companies.length){$('#workspaceDialogBody').innerHTML='<p>Nenhuma empresa com produtos cadastrados nesta base.</p>';return;}
  const today=new Date();
  const localDate=`${today.getFullYear()}-${String(today.getMonth()+1).padStart(2,'0')}-${String(today.getDate()).padStart(2,'0')}`;
  $('#workspaceDialogBody').innerHTML=`<p>Quantidade física por lote, calculada pelos movimentos de estoque. Reservas não são descontadas.</p><form id="stockForm"><div class="workspace-form-grid"><label>Empresa<select name="company_id" required>${data.companies.length>1?'<option value="">Selecione a empresa</option>':''}${data.companies.map(c=>`<option value="${esc(c.id)}">${esc(c.name)}</option>`).join('')}</select></label><label>Data de referência<input name="reference_date" type="date" value="${localDate}" required></label><label>Horizonte em dias<input name="horizon_days" type="number" min="0" max="366" value="30" required></label><label>Produto, código ou lote<input name="search" type="search" maxlength="100"></label><label>Linhas por página<select name="page_size"><option>25</option><option selected>50</option><option>100</option><option>200</option></select></label></div><label class="check-label"><input name="include_expired" type="checkbox">Incluir vencidos e vencimentos da data de referência</label><label class="check-label"><input name="positive_only" type="checkbox" checked>Somente lotes com quantidade positiva</label><p>Sem incluir vencidos, a consulta considera apenas datas posteriores à referência, até o último dia do horizonte.</p><button class="primary" type="submit">Consultar lotes</button></form><div id="stockResult" aria-live="polite"></div>`;
  const form=$('#stockForm'),resultElement=$('#stockResult');
  let busy=false,lastRequest=null,currentRun=null;
  async function load(request){
    if(busy)return;
    busy=true;
    const controls=[...form.elements,...resultElement.querySelectorAll('button')];controls.forEach(c=>c.disabled=true);
    try{
      modalMessage('Consultando lotes…');
      const response=await postWorkspace('stock/expiry',request);
      if(!$('#workspaceDialog').open||$('#stockResult')!==resultElement)return;
      currentRun=response.run_id;lastRequest={...request};
      const r=response.result,p=r.pagination;
      const columns=[['Codigo','Código'],['Produto','Produto'],['Lote','Lote'],['Validade','Validade'],['Quantidade','Quantidade'],['Unidade','Unidade'],['SituacaoLote','Situação'],['DiasParaVencer','Dias até vencer']];
      resultElement.innerHTML=`<p><strong>${fmtNumber(r.total_lots)} lotes encontrados</strong> · ${r.rows.length?request.offset+1:0}–${request.offset+r.rows.length} exibidos</p><p>${r.interpretation.summary.map(esc).join('<br>')}</p><div class="stock-table-wrap" tabindex="0" role="region" aria-label="Lotes encontrados"><table><thead><tr>${columns.map(c=>`<th scope="col">${c[1]}</th>`).join('')}</tr></thead><tbody>${r.rows.map(row=>`<tr>${columns.map(([k])=>`<td>${esc(k==='Quantidade'?fmtNumber(row[k]):row[k])}</td>`).join('')}</tr>`).join('')}</tbody></table></div>${!r.rows.length?'<p>Nenhum lote encontrado com esses filtros.</p>':''}<div class="item-actions"><button type="button" class="secondary" id="stockPrevious" ${request.offset===0?'disabled':''}>Anterior</button><button type="button" class="secondary" id="stockNext" ${!p.has_more?'disabled':''}>Próxima</button><button type="button" class="secondary" data-stock-export="csv">CSV desta página</button><button type="button" class="secondary" data-stock-export="xlsx">Excel desta página</button></div><p>${r.warnings.map(esc).join('<br>')}</p><p id="stockExportStatus" role="status"></p>`;
      $('#stockPrevious').onclick=guarded(()=>load({...lastRequest,offset:Math.max(0,lastRequest.offset-lastRequest.page_size)}));
      $('#stockNext').onclick=guarded(()=>load({...lastRequest,offset:lastRequest.offset+lastRequest.page_size}));
      $$('[data-stock-export]').forEach(button=>button.onclick=guarded(async()=>{
        const format=button.dataset.stockExport,run=currentRun,note=$('#stockExportStatus');
        const buttons=[...resultElement.querySelectorAll('[data-stock-export]')];buttons.forEach(b=>b.disabled=true);
        try{
          note.textContent='Preparando a cópia da página exibida…';
          const job=await postWorkspace('exports',{run_id:run,format,complete:false});
          for(let i=0;i<180;i++){
            const progress=await api(`/api/workspace/exports/${job.id}`);
            if(progress.status==='failed'||progress.status==='cancelled')throw Error(progress.error);
            if(progress.status==='succeeded'){await downloadAuthenticated(progress.download,`DigiSat-lotes.${format}`);note.textContent=`${progress.records} lotes exportados da página original.`;return;}
            await new Promise(resolve=>setTimeout(resolve,500));
          }
          throw Error('A exportação ainda não terminou. Verifique o aplicativo.');
        }finally{buttons.forEach(b=>b.disabled=false);}
      }));
      modalMessage('Consulta concluída.');
    }catch(error){modalMessage(error.message,true);}
    finally{busy=false;controls.forEach(c=>{if(c.isConnected)c.disabled=false;});}
  }
  form.onsubmit=guarded(async()=>{
    const f=new FormData(form);
    await load({company_id:f.get('company_id'),reference_date:f.get('reference_date'),horizon_days:Number(f.get('horizon_days')),search:f.get('search'),page_size:Number(f.get('page_size')),include_expired:f.has('include_expired'),positive_only:f.has('positive_only'),offset:0});
  });
});
