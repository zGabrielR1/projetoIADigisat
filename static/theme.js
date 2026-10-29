/* Apply before styles load to avoid a flash of the wrong theme. */
(() => {
  const key = "digisat-insight-theme";
  const system = window.matchMedia("(prefers-color-scheme: dark)");
  let preference;
  try { preference = localStorage.getItem(key); } catch (_) { /* Storage may be blocked. */ }
  if (preference !== "light" && preference !== "dark") preference = null;

  function apply(theme) {
    document.documentElement.dataset.theme = theme;
    const button = document.getElementById("themeToggle");
    if (!button) return;
    const dark = theme === "dark";
    button.setAttribute("aria-pressed", String(dark));
    button.title = dark ? "Ativar modo claro" : "Ativar modo escuro";
    document.getElementById("themeLabel").textContent = dark ? "Modo claro" : "Modo escuro";
  }

  apply(preference || "dark");
  document.addEventListener("DOMContentLoaded", () => {
    apply(document.documentElement.dataset.theme);
    document.getElementById("themeToggle")?.addEventListener("click", () => {
      preference = document.documentElement.dataset.theme === "dark" ? "light" : "dark";
      try { localStorage.setItem(key, preference); } catch (_) { /* Keep working without persistence. */ }
      apply(preference);
    });
  });
  system.addEventListener("change", (event) => {
    if (!preference) apply("dark");
  });
  window.addEventListener("storage", (event) => {
    if (event.key !== key && event.key !== null) return;
    preference = event.newValue === "light" || event.newValue === "dark" ? event.newValue : null;
    apply(preference || "dark");
  });
})();
