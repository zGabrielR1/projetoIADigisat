/* Shared suite navigation. Page-specific forms and handlers stay in their modules. */
document.addEventListener('DOMContentLoaded', () => {
  const routes = [
    ['/', 'Visão geral', 'M3 3h7v7H3zM14 3h7v7h-7zM3 14h7v7H3zM14 14h7v7h-7z'],
    ['/bi', 'Business intelligence', 'M3 20h18M5 16v-4m7 4V5m7 11V9M5 7l7-4 7 2'],
    ['/native-reports', 'Central de relatórios', 'M4 20V10m6 10V4m6 16v-7M2 20h20'],
    ['/reports', 'Catálogo SuiteG6', 'M4 4h16v16H4zM8 4v16M12 8h5m-5 4h5m-5 4h5'],
    ['/finance-reports', 'Financeiro', 'M12 2v20m5-16H9a4 4 0 0 0 0 8h6a4 4 0 0 1 0 8H6'],
    ['/database-editor', 'Banco de dados', 'M20 6c0 2-4 4-8 4S4 8 4 6s4-4 8-4 8 2 8 4Zm0 0v12c0 2-4 4-8 4s-8-2-8-4V6m0 6c0 2 4 4 8 4s8-2 8-4']
  ];
  const sidebar = document.createElement('aside');
  sidebar.className = 'pebble-sidebar';
  sidebar.setAttribute('aria-label', 'Navegação principal');
  const staticRoutes={index:'/',bi:'/bi',native:'/native-reports',reports:'/reports',finance:'/finance-reports',editor:'/database-editor'};
  const current = routes.find(([url]) => location.pathname === url || staticRoutes[location.pathname.split('/').pop().replace('.html','')]===url);
  sidebar.innerHTML = `<a class="pebble-brand" href="/"><span>D</span><div>DigiSat <b>Insight</b></div></a><nav>${routes.map(([url, label, path]) => `<a href="${url}" ${current?.[0] === url ? 'aria-current="page"' : ''}><svg viewBox="0 0 24 24" aria-hidden="true"><path d="${path}"/></svg><span>${label}</span></a>`).join('')}</nav><div class="pebble-sidebar-bottom"><span class="pebble-live-dot"></span> Seu espaço de dados<small>DigiSat Insight · SuiteG6</small></div>`;
  document.body.prepend(sidebar);
  document.body.classList.add('pebble-suite');
  // Four readable mobile destinations; the remaining tools live in an accessible menu.
  sidebar.querySelectorAll('nav>a').forEach((link,index)=>{if(index>2)link.classList.add('pebble-mobile-extra');link.querySelector('span').classList.add('nav-full');if(index<=2){const short=document.createElement('span');short.className='nav-short';short.textContent=['Visão geral','BI','Relatórios'][index];link.append(short);}});
  const more=document.createElement('button');more.className='pebble-more secondary';more.type='button';more.setAttribute('aria-haspopup','dialog');more.setAttribute('aria-controls','suiteNavigation');more.innerHTML='<svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 6h16M4 12h16M4 18h16"/></svg><span>Mais</span>';
  if(current&&routes.indexOf(current)>2)more.setAttribute('data-active','true');
  const menu=document.createElement('dialog');menu.id='suiteNavigation';menu.className='pebble-menu';menu.setAttribute('aria-labelledby','suiteNavigationTitle');
  menu.innerHTML='<div class="pebble-menu-head"><h2 id="suiteNavigationTitle">Ferramentas do Insight</h2><button type="button" class="secondary">Fechar</button></div><nav>'+routes.map(([url,label])=>`<a href="${url}" ${current?.[0]===url?'aria-current="page"':''}>${label}</a>`).join('')+'</nav>';
  document.body.append(menu);sidebar.querySelector('nav').append(more);more.onclick=()=>menu.showModal();menu.querySelector('button').onclick=()=>menu.close();
  if(new URLSearchParams(location.search).get('tool')==='connection')document.getElementById('connectionOpen')?.click();
  const home = document.getElementById('assistente');
  if (!home) return;
  document.body.classList.add('pebble-dashboard');
  const main = document.querySelector('main');
  const top = main.querySelector('.top');
  top.querySelector('h1').textContent = 'Uma nova perspectiva do seu negócio.';
  top.querySelector('.eyebrow').textContent = 'DIGISAT INSIGHT';
  top.querySelector('.tagline').textContent = 'Suas vendas, relatórios e investigações em um só lugar.';
  const layout = document.createElement('div');
  layout.className = 'dashboard-layout';
  top.after(layout);
  const center = document.createElement('div');
  center.className = 'dashboard-center';
  const tools = document.createElement('aside');
  tools.className = 'dashboard-tools';
  tools.setAttribute('aria-label','Ferramentas do espaço de trabalho');
  layout.append(center, tools);
  center.append(home);
  const heroCopy = home.querySelector('.ask-copy');
  heroCopy.querySelector('.pill').textContent = 'INTELIGÊNCIA PARA DECIDIR';
  heroCopy.querySelector('h2').innerHTML = 'Menos buscas.<br>Mais respostas.';
  heroCopy.querySelector('p').textContent = 'Transforme uma pergunta em uma visão clara do seu negócio.';
  const orbit = document.createElement('div');
  orbit.className = 'dashboard-orbit';
  orbit.innerHTML = '<div><strong>7</strong><span>visões de negócio</span></div>';
  orbit.setAttribute('aria-label','Sete tipos de relatório disponíveis');
  home.append(orbit);
  const examples = document.createElement('details');
  examples.className = 'dashboard-examples';
  examples.innerHTML = '<summary>Experimente uma pergunta</summary>';
  home.after(examples);
  examples.append(home.querySelector('.suggestions'));
  const stats = document.createElement('section');
  stats.className = 'dashboard-stats';
  stats.setAttribute('aria-label','Resumo da consulta atual');
  center.append(stats);
  function syncMetrics() {
    const metrics = [...document.querySelectorAll('#summary .metric')].slice(0,3);
    stats.replaceChildren();
    if (!metrics.length) {
      for (const [label,value,note] of [['Faturamento','—','Execute uma consulta para ver o total'],['Documentos','—','Documentos no período selecionado'],['Itens vendidos','—','Quantidade calculada na sua base']]) {
        const card = document.createElement('div');
        card.className = 'dashboard-stat';
        const small = document.createElement('small');small.textContent=label;
        const strong = document.createElement('strong');strong.textContent=value;
        const p = document.createElement('p');p.textContent=note;
        card.append(small,strong,p);stats.append(card);
      }
    } else metrics.forEach(metric => {const card=metric.cloneNode(true);card.className='dashboard-stat has-value';stats.append(card);});
  }
  new MutationObserver(syncMetrics).observe(document.getElementById('summary'),{childList:true,subtree:true});
  syncMetrics();
  center.append(main.querySelector('.report-section'));
  tools.innerHTML = '<div class="dashboard-tools-heading"><h2>Seu espaço de trabalho</h2><span>↗</span></div><p class="dashboard-tools-intro">Continue de onde precisa.</p><div class="dashboard-destinations"><a href="/bi"><span class="destination-icon">↗</span><div><strong>Business intelligence</strong><small>Painéis e exploração de vendas</small></div><span>›</span></a><a href="/native-reports"><span class="destination-icon">↗</span><div><strong>Central de relatórios</strong><small>Encontre e consulte no SuiteG6</small></div><span>›</span></a><a href="/finance-reports"><span class="destination-icon">$</span><div><strong>Financeiro</strong><small>Contas a pagar e receber</small></div><span>›</span></a></div><h3>Ferramentas</h3>';
  tools.append(main.querySelector('.workspace-nav'));
  const status = document.getElementById('status');
  tools.append(status);
  const settings = document.createElement('details');
  settings.className = 'dashboard-settings';
  settings.innerHTML = '<summary>Período, perfil e filtros</summary>';
  layout.after(settings);
  settings.append(main.querySelector('.toolbar'),main.querySelector('.workspace-profile'));
  const filterPanel=document.getElementById('filtersPanel');
  settings.append(filterPanel);
  // Keep errors visible alongside the connection controls, without displacing the dashboard.
  tools.append(document.getElementById('setupBanner'));
});
