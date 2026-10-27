/* Pure, lossless serialization of an already displayed canonical BSON page. */
((root,factory)=>{const api=factory();if(typeof module==='object'&&module.exports)module.exports=api;else root.EditorPageExport=api;})(globalThis,()=>({
  encode(result,jsonl=false,exportedAt=new Date().toISOString()){
    if(jsonl)return result.documents.map(document=>document.json).join('\n')+(result.documents.length?'\n':'');
    return JSON.stringify({format:'DigiSat Insight / canonical Extended JSON',exported_at:exportedAt,
      database:result.database,collection:result.collection,offset:result.offset,limit:result.limit,
      has_more:result.has_more,query:result.translated,
      documents:result.documents.map(document=>JSON.parse(document.json))},null,2);
  }
}));
