const test=require('node:test');
const assert=require('node:assert/strict');
const analysis=require('../static/bi-analysis.js');

test('day alignment crosses months and leap days without local-time shifts',()=>{
  assert.deepEqual(analysis.days('2024-02-28','2024-03-01'),['2024-02-28','2024-02-29','2024-03-01']);
  assert.throws(()=>analysis.days('2026-02-30','2026-03-01'));
  assert.throws(()=>analysis.days('2026-01-01','2028-01-01'));
});
test('complete series infer zeros, while truncated series and missing measures retain gaps',()=>{
  const period={start:'2026-10-01',end:'2026-10-03'},rows=[{_id:'2026-10-01',total:0},{_id:'2026-10-03',total:null}];
  assert.deepEqual(analysis.daily(rows,period,'total',true).values,[0,0,null]);
  assert.deepEqual(analysis.daily(rows,period,'total',false).values,[0,null,null]);
});
test('comparison aligns equal positions and never maps prior dates onto current labels',()=>{
  const result=analysis.comparison({period:{start:'2026-10-01',end:'2026-10-02'},previous_period:{start:'2026-09-29',end:'2026-09-30'},daily:[{_id:'2026-10-02',total:12}],previous_daily:[{_id:'2026-09-29',total:5}],daily_complete:true,previous_daily_complete:true,errors:{}},'total');
  assert.deepEqual(result.current.values,[0,12]);
  assert.deepEqual(result.previous.values,[5,0]);
  assert.equal(result.previous.dates[0],'2026-09-29');
});
test('failed previous series is absent rather than a fabricated zero baseline',()=>{
  const data={period:{start:'2026-10-01',end:'2026-10-01'},previous_period:{start:'2026-09-30',end:'2026-09-30'},daily:[],daily_complete:true,errors:{previous_daily:'timeout'}};
  assert.equal(analysis.comparison(data,'total').previous,null);
});
test('visible totals sum the cents actually displayed, including negative half cents',()=>{
  assert.equal(analysis.displayedCents(1.005),101);
  assert.equal(analysis.displayedCents(-1.005),-101);
  assert.deepEqual(analysis.visibleSummary([{total:1.005},{total:2.005},{total:-1.005},{total:null}],'daily'),{rows:4,amount:2.01,valuedRows:3});
  assert.deepEqual(analysis.visibleSummary([{Valor:0},{Valor:'10.50'}],'documents'),{rows:2,amount:10.5,valuedRows:2});
  assert.equal(analysis.visibleSummary([],'documents').amount,null);
});
