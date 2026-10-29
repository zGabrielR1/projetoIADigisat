/* Pure BI presentation math, shared by the browser and offline Node tests. */
(function(root) {
  'use strict';
  const DAY = 86400000;
  function days(start, end) {
    const parse = value => {
      if (typeof value !== 'string' || !/^\d{4}-\d{2}-\d{2}$/.test(value)) throw new Error('Data inválida.');
      const stamp = Date.parse(value+'T00:00:00Z');
      if (!Number.isFinite(stamp) || new Date(stamp).toISOString().slice(0,10)!==value) throw new Error('Data inválida.');
      return stamp;
    };
    const first=parse(start), last=parse(end), size=(last-first)/DAY+1;
    if(size<1||size>366)throw new Error('Período de análise inválido.');
    return Array.from({length:size},(_,i)=>new Date(first+i*DAY).toISOString().slice(0,10));
  }
  function daily(rows, period, metric, complete) {
    const dates=days(period.start,period.end), valuesByDay=new Map(rows.map(row=>[row._id,row[metric]]));
    return {dates,values:dates.map(day=>{
      if(!valuesByDay.has(day))return complete?0:null;
      const value=valuesByDay.get(day);
      return typeof value==='number'&&Number.isFinite(value)?value:null;
    })};
  }
  function comparison(data,metric) {
    const current=daily(data.daily||[],data.period,metric,!!data.daily_complete);
    const previous=data.previous_period&&!data.errors?.previous_daily
      ?daily(data.previous_daily||[],data.previous_period,metric,!!data.previous_daily_complete):null;
    if(previous&&previous.dates.length!==current.dates.length)throw new Error('Os períodos de comparação têm tamanhos diferentes.');
    return {current,previous};
  }
  const decimalFormat=new Intl.NumberFormat('en-US',{useGrouping:false,minimumFractionDigits:2,maximumFractionDigits:2});
  function displayedCents(value) {
    const text=decimalFormat.format(value),negative=text.startsWith('-'),[whole,fraction]=text.replace('-','').split('.');
    return (Number(whole)*100+Number(fraction))*(negative?-1:1);
  }
  function visibleSummary(rows,dimension) {
    const field=dimension==='documents'?'Valor':'total';
    const amounts=rows.map(row=>row[field]).filter(value=>value!==null&&value!==undefined&&value!==''&&Number.isFinite(Number(value)));
    return {rows:rows.length,amount:amounts.length?amounts.reduce((sum,value)=>sum+displayedCents(Number(value)),0)/100:null,valuedRows:amounts.length};
  }
  const api={days,daily,comparison,displayedCents,visibleSummary};
  if(typeof module!=='undefined'&&module.exports)module.exports=api;
  root.BIAnalysis=api;
})(globalThis);
