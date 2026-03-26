import { entries, languageOptions, siteConfig, uiCopy } from "./data.js";
import { frontierEntries, frontierUiCopy } from "./orespawn-data.js";

const allEntries = [...entries, ...frontierEntries];
const orderedCategories = ["bosses", "mobs", "summons", "weapons", "armor", "accessories", "consumables", "materials", "buffs"];
const craftableEntries = allEntries.filter((entry) => hasContentList(entry, "crafting"));

const entryTags = {
  "crystaline-devourer": "boss",
  "water-dragon": "mob",
  mantis: "mob",
  caterkiller: "miniboss",
  "emperor-scorpion": "miniboss",
  hercules: "miniboss",
  cephadrome: "miniboss"
};

const progressionFlow = [
  { id: "water-dragon", stage: "early" },
  { id: "mantis", stage: "early" },
  { id: "caterkiller", stage: "post_evil" },
  { id: "emperor-scorpion", stage: "post_evil" },
  { id: "hercules", stage: "post_evil" },
  { id: "cephadrome", stage: "hardmode" },
  { id: "crystaline-devourer", stage: "endgame" }
];

const pageCopy = {
  "pt-BR": {
    nav: {
      overview: "Inicio",
      progression: "Progressao",
      catalog: "Biblioteca",
      crafting: "Crafting",
      community: "Feedback",
      admin: "Admin"
    },
    topbar: {
      search: "Buscar item, boss, mob ou material",
      menu: "Abrir menu"
    },
    overview: {
      eyebrow: "Wiki Oficial",
      title: "Uma wiki mais limpa, direta e facil de expandir.",
      lead: "Base pensada como wiki real: navegacao simples, paginas legiveis, crafting claro e uma progressao que voce pode crescer por update.",
      body: "Em vez de uma landing page carregada, esta versao prioriza texto, referencia, categorias e leitura. O foco agora e documentar exatamente como pegar itens, onde cada coisa aparece e como a progressao se encaixa.",
      stats: {
        entries: "Entradas",
        recipes: "Receitas",
        categories: "Categorias",
        languages: "Idiomas"
      },
      sectionsTitle: "Estrutura da wiki",
      sectionsIntro: "As areas abaixo foram organizadas para funcionar como uma base de documentacao, nao como uma homepage promocional.",
      quickTitle: "Atalhos",
      quickCards: [
        { title: "Itens e equipamentos", body: "Biblioteca central com busca, filtros e pagina detalhada de cada entrada.", target: "#catalog" },
        { title: "Crafting", body: "Guia separado para receitas, ingredientes, estacoes e itens relacionados.", target: "#crafting" },
        { title: "Progressao", body: "Linha de bosses, minibosses e encounters para orientar o jogador.", target: "#progression" }
      ]
    },
    sidebar: {
      title: "Navegacao",
      categories: "Categorias",
      links: "Projeto",
      status: "Status",
      statusBody: "Layout minimalista entregue. Login, upload e comentarios reais aguardam o backend escolhido."
    },
    progression: {
      eyebrow: "Guia de Progressao",
      title: "Ordem sugerida de encounters",
      intro: "Esta linha serve como espinha dorsal da wiki. Ela ajuda a ligar spawn, drops, crafting e upgrades no mesmo fluxo.",
      stages: {
        early: "Abertura",
        post_evil: "Pos Evil Boss",
        hardmode: "Hardmode",
        endgame: "Pico atual"
      }
    },
    catalog: {
      eyebrow: "Biblioteca",
      title: "Itens, bosses, mobs e sistemas",
      intro: "Selecione uma categoria, pesquise por nome e abra a ficha completa ao lado.",
      all: "Tudo",
      empty: "Nenhuma entrada combinou com o filtro atual.",
      facts: "Fatos rapidos",
      obtain: "Como obter",
      crafting: "Crafting",
      drops: "Drops",
      pieces: "Pecas",
      notes: "Notas",
      tactics: "Taticas",
      related: "Relacionados"
    },
    crafting: {
      eyebrow: "Crafting",
      title: "Receitas e ingredientes",
      intro: "Area separada para consultar receitas com mais clareza, no estilo de uma wiki de mod focada em progressao.",
      empty: "Nenhuma receita encontrada para o filtro atual.",
      ingredients: "Ingredientes",
      station: "Estacao",
      output: "Resultado",
      notes: "Notas"
    },
    community: {
      eyebrow: "Feedback",
      title: "Comentarios da comunidade",
      intro: "A estrutura visual ja esta pronta, mas comentarios publicos reais precisam de backend para salvar nome, mensagem e moderacao.",
      cardTitle: "Base preparada para comentarios",
      cardBody: "Posso ligar aqui um sistema simples de feedback com nome e comentario, mas isso precisa de um servico para gravar os dados de verdade.",
      name: "Nome",
      message: "Comentario",
      button: "Enviar feedback",
      note: "No estado atual do GitHub Pages, este formulario e apenas estrutural."
    },
    admin: {
      eyebrow: "Admin",
      title: "Painel para gestao da wiki",
      intro: "Tambem da para deixar uma area admin para upload de imagens, escolha de categoria, criacao de receita e organizacao do conteudo.",
      uploadTitle: "Ferramentas previstas",
      uploadItems: ["login de admin", "upload de imagens", "cadastro de categorias", "editor de crafting", "moderacao de comentarios"],
      requirementTitle: "O que falta para ativar",
      requirementBody: "Para isso funcionar de verdade, precisamos conectar a wiki a um backend. Minha recomendacao e GitHub Pages para o site e Supabase para auth, storage e comentarios."
    },
    footer: { text: "Chaotic Dimensions Wiki" },
    tags: { boss: "Boss", miniboss: "Mini-Boss", mob: "Mob" }
  },
  en: {
    nav: {
      overview: "Overview",
      progression: "Progression",
      catalog: "Library",
      crafting: "Crafting",
      community: "Feedback",
      admin: "Admin"
    },
    topbar: {
      search: "Search item, boss, mob or material",
      menu: "Open menu"
    },
    overview: {
      eyebrow: "Official Wiki",
      title: "A cleaner and more expandable wiki foundation.",
      lead: "Built like a real wiki: simple navigation, readable pages, clear crafting and a progression flow you can keep expanding.",
      body: "Instead of a crowded landing page, this version focuses on reference, categories and readable documentation.",
      stats: {
        entries: "Entries",
        recipes: "Recipes",
        categories: "Categories",
        languages: "Languages"
      },
      sectionsTitle: "Wiki structure",
      sectionsIntro: "The sections below are organized to behave like documentation instead of a promo homepage.",
      quickTitle: "Shortcuts",
      quickCards: [
        { title: "Items and equipment", body: "Central library with search, filters and detailed pages.", target: "#catalog" },
        { title: "Crafting", body: "Separate guide for recipes, ingredients, stations and related items.", target: "#crafting" },
        { title: "Progression", body: "Boss and miniboss route to guide players.", target: "#progression" }
      ]
    },
    sidebar: {
      title: "Navigation",
      categories: "Categories",
      links: "Project",
      status: "Status",
      statusBody: "Minimal layout is live. Real login, upload and comments still need the backend choice."
    },
    progression: {
      eyebrow: "Progression Guide",
      title: "Suggested encounter order",
      intro: "This acts as the backbone of the wiki, connecting spawn rules, drops and upgrades.",
      stages: {
        early: "Opening",
        post_evil: "Post Evil Boss",
        hardmode: "Hardmode",
        endgame: "Current peak"
      }
    },
    catalog: {
      eyebrow: "Library",
      title: "Items, bosses, mobs and systems",
      intro: "Pick a category, search by name and open the full sheet on the side.",
      all: "All",
      empty: "No entry matches the current filter.",
      facts: "Quick facts",
      obtain: "How to obtain",
      crafting: "Crafting",
      drops: "Drops",
      pieces: "Pieces",
      notes: "Notes",
      tactics: "Tactics",
      related: "Related"
    },
    crafting: {
      eyebrow: "Crafting",
      title: "Recipes and ingredients",
      intro: "A separate area for recipe lookup with clearer progression references.",
      empty: "No recipe matched the current filter.",
      ingredients: "Ingredients",
      station: "Station",
      output: "Result",
      notes: "Notes"
    },
    community: {
      eyebrow: "Feedback",
      title: "Community comments",
      intro: "The UI is ready, but real public comments need a backend to store names, messages and moderation.",
      cardTitle: "Comment area prepared",
      cardBody: "I can wire a simple name-and-comment system here, but it needs a service that actually stores data.",
      name: "Name",
      message: "Comment",
      button: "Send feedback",
      note: "Right now on GitHub Pages this form is only structural."
    },
    admin: {
      eyebrow: "Admin",
      title: "Wiki management panel",
      intro: "This can become the area for image uploads, category selection, recipe editing and content organization.",
      uploadTitle: "Planned tools",
      uploadItems: ["admin login", "image upload", "category editor", "crafting editor", "comment moderation"],
      requirementTitle: "What still needs to be connected",
      requirementBody: "To make this real, the wiki needs a backend. My recommendation is GitHub Pages for the site and Supabase for auth, storage and comments."
    },
    footer: { text: "Chaotic Dimensions Wiki" },
    tags: { boss: "Boss", miniboss: "Mini-Boss", mob: "Mob" }
  }
};

const elements = {
  topbar: document.querySelector("#topbar"),
  sidebar: document.querySelector("#sidebar"),
  overview: document.querySelector("#overview"),
  progression: document.querySelector("#progression"),
  catalog: document.querySelector("#catalog"),
  crafting: document.querySelector("#crafting"),
  community: document.querySelector("#community"),
  admin: document.querySelector("#admin"),
  footer: document.querySelector("#site-footer"),
  overlay: document.querySelector("#sidebar-overlay"),
  metaDescription: document.querySelector("#meta-description")
};

const state = {
  language: siteConfig.defaultLanguage,
  search: "",
  category: "all",
  selectedEntryId: siteConfig.defaultEntryId,
  selectedRecipeId: craftableEntries[0]?.id ?? siteConfig.defaultEntryId,
  sidebarOpen: false
};

bootstrap();

function bootstrap() {
  hydrateStateFromUrl();
  bindGlobalEvents();
  render();
}

function hydrateStateFromUrl() {
  const url = new URL(window.location.href);
  const language = url.searchParams.get("lang");
  const search = url.searchParams.get("q");
  const category = url.searchParams.get("category");
  const entryId = url.searchParams.get("entry");
  const recipeId = url.searchParams.get("recipe");

  if (languageOptions.some((option) => option.code === language)) {
    state.language = language;
  }

  if (search) {
    state.search = search;
  }

  if (category === "all" || orderedCategories.includes(category)) {
    state.category = category;
  }

  if (allEntries.some((entry) => entry.id === entryId)) {
    state.selectedEntryId = entryId;
  }

  if (craftableEntries.some((entry) => entry.id === recipeId)) {
    state.selectedRecipeId = recipeId;
  }
}

function bindGlobalEvents() {
  elements.overlay.addEventListener("click", () => toggleSidebar(false));

  window.addEventListener("resize", () => {
    if (window.innerWidth > 1040 && state.sidebarOpen) {
      toggleSidebar(false);
    }
  });
}

function render() {
  syncMetadata();
  renderTopbar();
  renderSidebar();
  renderOverview();
  renderProgression();
  renderCatalog();
  renderCrafting();
  renderCommunity();
  renderAdmin();
  renderFooter();
  syncUrl();
}

function renderTopbar() {
  const copy = getPageCopy();
  const languageMarkup = languageOptions.map((option) => `
    <button class="chip ${option.code === state.language ? "is-active" : ""}" type="button" data-language="${option.code}">
      ${option.label}
    </button>
  `).join("");

  elements.topbar.innerHTML = `
    <div class="topbar__left">
      <button class="menu-button" type="button" id="menu-button" aria-label="${copy.topbar.menu}">
        <span></span>
        <span></span>
        <span></span>
      </button>
      <a class="brand" href="#overview">
        <img src="./assets/images/icon.png" alt="Chaotic Dimensions icon">
        <div>
          <strong>${getSiteName()}</strong>
          <span>Wiki</span>
        </div>
      </a>
    </div>

    <label class="search-box">
      <input id="global-search" type="search" value="${escapeHtml(state.search)}" placeholder="${copy.topbar.search}">
    </label>

    <div class="topbar__right">
      <div class="language-row">${languageMarkup}</div>
      <a class="link-button" href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">GitHub</a>
    </div>
  `;

  elements.topbar.querySelector("#menu-button")?.addEventListener("click", () => toggleSidebar(!state.sidebarOpen));
  elements.topbar.querySelector("#global-search")?.addEventListener("input", (event) => {
    state.search = event.target.value;
    renderCatalog();
    renderCrafting();
    syncUrl();
  });

  elements.topbar.querySelectorAll("[data-language]").forEach((button) => {
    button.addEventListener("click", () => {
      state.language = button.dataset.language;
      render();
    });
  });
}

function renderSidebar() {
  const copy = getPageCopy();
  const navItems = [
    { href: "#overview", label: copy.nav.overview },
    { href: "#progression", label: copy.nav.progression },
    { href: "#catalog", label: copy.nav.catalog },
    { href: "#crafting", label: copy.nav.crafting },
    { href: "#community", label: copy.nav.community },
    { href: "#admin", label: copy.nav.admin }
  ].map((item) => `<a class="sidebar-link" href="${item.href}">${item.label}</a>`).join("");

  const categoryButtons = orderedCategories.map((category) => `
    <button class="category-button ${state.category === category ? "is-active" : ""}" type="button" data-category-jump="${category}">
      <span>${getCategoryLabel(category)}</span>
      <small>${getEntriesByCategory(category).length}</small>
    </button>
  `).join("");

  elements.sidebar.innerHTML = `
    <div class="sidebar-card">
      <p class="section-kicker">${copy.sidebar.title}</p>
      <nav class="sidebar-nav">${navItems}</nav>
    </div>

    <div class="sidebar-card">
      <p class="section-kicker">${copy.sidebar.categories}</p>
      <div class="category-stack">${categoryButtons}</div>
    </div>

    <div class="sidebar-card">
      <p class="section-kicker">${copy.sidebar.links}</p>
      <div class="sidebar-actions">
        <a class="link-button" href="${siteConfig.pagesUrl}" target="_blank" rel="noreferrer">Pages</a>
        <a class="link-button" href="${siteConfig.releasesUrl}" target="_blank" rel="noreferrer">Releases</a>
      </div>
    </div>

    <div class="sidebar-card sidebar-card--muted">
      <p class="section-kicker">${copy.sidebar.status}</p>
      <p>${copy.sidebar.statusBody}</p>
    </div>
  `;

  elements.sidebar.querySelectorAll("[data-category-jump]").forEach((button) => {
    button.addEventListener("click", () => {
      state.category = button.dataset.categoryJump;
      renderSidebar();
      renderCatalog();
      syncUrl();
      document.querySelector("#catalog")?.scrollIntoView({ behavior: "smooth", block: "start" });
      if (window.innerWidth <= 1040) {
        toggleSidebar(false);
      }
    });
  });
}

function renderOverview() {
  const copy = getPageCopy();
  const stats = [
    { value: allEntries.length, label: copy.overview.stats.entries },
    { value: craftableEntries.length, label: copy.overview.stats.recipes },
    { value: orderedCategories.length, label: copy.overview.stats.categories },
    { value: languageOptions.length, label: copy.overview.stats.languages }
  ].map((item) => `
    <article class="stat-card">
      <strong>${item.value}</strong>
      <span>${item.label}</span>
    </article>
  `).join("");

  const quickCards = copy.overview.quickCards.map((item) => `
    <article class="mini-card">
      <h3>${item.title}</h3>
      <p>${item.body}</p>
      <a class="text-link" href="${item.target}">Abrir</a>
    </article>
  `).join("");

  const categorySummary = orderedCategories.map((category) => `
    <article class="taxonomy-card">
      <h3>${getCategoryLabel(category)}</h3>
      <p>${getEntriesByCategory(category).length}</p>
    </article>
  `).join("");

  elements.overview.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.overview.eyebrow}</p>
      <h1>${copy.overview.title}</h1>
      <p class="section-lead">${copy.overview.lead}</p>
      <p class="section-copy">${copy.overview.body}</p>
    </div>

    <div class="overview-grid">
      <div class="panel">
        <div class="stats-grid">${stats}</div>
      </div>

      <div class="panel">
        <p class="panel-kicker">${copy.overview.quickTitle}</p>
        <div class="mini-grid">${quickCards}</div>
      </div>
    </div>

    <div class="section-heading section-heading--compact">
      <h2>${copy.overview.sectionsTitle}</h2>
      <p>${copy.overview.sectionsIntro}</p>
    </div>

    <div class="taxonomy-grid">${categorySummary}</div>
  `;
}

function renderProgression() {
  const copy = getPageCopy();
  const cards = progressionFlow.map((step) => {
    const entry = getEntryById(step.id);
    const content = getLocalizedEntry(entry);
    const preview = (content.facts ?? []).slice(0, 2).map((fact) => `<li>${fact}</li>`).join("");

    return `
      <article class="timeline-card">
        <div class="timeline-card__head">
          <span class="tag">${copy.progression.stages[step.stage]}</span>
          <span class="tag tag--subtle">${getTagLabel(entry)}</span>
        </div>
        <div class="timeline-card__body">
          <img class="icon icon--large" src="${entry.image}" alt="${content.title}">
          <div>
            <h3>${content.title}</h3>
            <p>${content.summary}</p>
            <ul class="compact-list">${preview}</ul>
          </div>
        </div>
      </article>
    `;
  }).join("");

  elements.progression.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.progression.eyebrow}</p>
      <h2>${copy.progression.title}</h2>
      <p>${copy.progression.intro}</p>
    </div>

    <div class="timeline-grid">${cards}</div>
  `;
}

function renderCatalog() {
  const copy = getPageCopy();
  const visibleEntries = getVisibleEntries();

  if (!visibleEntries.some((entry) => entry.id === state.selectedEntryId)) {
    state.selectedEntryId = visibleEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  const filterButtons = [
    { key: "all", label: copy.catalog.all, count: allEntries.length },
    ...orderedCategories.map((category) => ({
      key: category,
      label: getCategoryLabel(category),
      count: getEntriesByCategory(category).length
    }))
  ].map((item) => `
    <button class="chip ${state.category === item.key ? "is-active" : ""}" type="button" data-filter="${item.key}">
      ${item.label} <small>${item.count}</small>
    </button>
  `).join("");

  const rows = visibleEntries.length > 0
    ? visibleEntries.map((entry) => renderCatalogRow(entry)).join("")
    : `<div class="empty-state">${copy.catalog.empty}</div>`;

  const selectedEntry = getEntryById(state.selectedEntryId);
  const detailMarkup = selectedEntry ? renderEntryDetail(selectedEntry) : "";

  elements.catalog.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.catalog.eyebrow}</p>
      <h2>${copy.catalog.title}</h2>
      <p>${copy.catalog.intro}</p>
    </div>

    <div class="filter-row">${filterButtons}</div>

    <div class="browser-layout">
      <div class="list-panel">${rows}</div>
      <aside class="detail-panel">${detailMarkup}</aside>
    </div>
  `;

  elements.catalog.querySelectorAll("[data-filter]").forEach((button) => {
    button.addEventListener("click", () => {
      state.category = button.dataset.filter;
      renderCatalog();
      renderSidebar();
      syncUrl();
    });
  });

  elements.catalog.querySelectorAll("[data-entry-id]").forEach((button) => {
    button.addEventListener("click", () => {
      state.selectedEntryId = button.dataset.entryId;
      renderCatalog();
      syncUrl();
    });
  });
}

function renderCrafting() {
  const copy = getPageCopy();
  const visibleRecipes = getVisibleRecipes();

  if (!visibleRecipes.some((entry) => entry.id === state.selectedRecipeId)) {
    state.selectedRecipeId = visibleRecipes[0]?.id ?? craftableEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  const recipeList = visibleRecipes.length > 0
    ? visibleRecipes.map((entry) => renderRecipeRow(entry)).join("")
    : `<div class="empty-state">${copy.crafting.empty}</div>`;

  const selectedRecipe = getEntryById(state.selectedRecipeId);
  const recipeDetail = selectedRecipe ? renderRecipeDetail(selectedRecipe) : "";

  elements.crafting.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.crafting.eyebrow}</p>
      <h2>${copy.crafting.title}</h2>
      <p>${copy.crafting.intro}</p>
    </div>

    <div class="browser-layout">
      <div class="list-panel">${recipeList}</div>
      <aside class="detail-panel">${recipeDetail}</aside>
    </div>
  `;

  elements.crafting.querySelectorAll("[data-recipe-id]").forEach((button) => {
    button.addEventListener("click", () => {
      state.selectedRecipeId = button.dataset.recipeId;
      renderCrafting();
      syncUrl();
    });
  });
}

function renderCommunity() {
  const copy = getPageCopy();
  elements.community.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.community.eyebrow}</p>
      <h2>${copy.community.title}</h2>
      <p>${copy.community.intro}</p>
    </div>

    <div class="placeholder-grid">
      <article class="panel">
        <h3>${copy.community.cardTitle}</h3>
        <p>${copy.community.cardBody}</p>
        <div class="stub-form">
          <input type="text" placeholder="${copy.community.name}" disabled>
          <textarea rows="5" placeholder="${copy.community.message}" disabled></textarea>
          <button class="button button--primary" type="button" disabled>${copy.community.button}</button>
        </div>
        <p class="helper-text">${copy.community.note}</p>
      </article>
    </div>
  `;
}

function renderAdmin() {
  const copy = getPageCopy();
  const toolList = copy.admin.uploadItems.map((item) => `<li>${item}</li>`).join("");

  elements.admin.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.admin.eyebrow}</p>
      <h2>${copy.admin.title}</h2>
      <p>${copy.admin.intro}</p>
    </div>

    <div class="placeholder-grid placeholder-grid--two">
      <article class="panel">
        <h3>${copy.admin.uploadTitle}</h3>
        <ul>${toolList}</ul>
      </article>

      <article class="panel panel--muted">
        <h3>${copy.admin.requirementTitle}</h3>
        <p>${copy.admin.requirementBody}</p>
      </article>
    </div>
  `;
}

function renderFooter() {
  const copy = getPageCopy();
  elements.footer.innerHTML = `
    <div class="footer-inner">
      <strong>${copy.footer.text}</strong>
      <span>${siteConfig.pagesUrl}</span>
    </div>
  `;
}

function renderCatalogRow(entry) {
  const content = getLocalizedEntry(entry);
  const preview = (content.facts ?? []).slice(0, 2).map((fact) => `<li>${fact}</li>`).join("");

  return `
    <button class="entry-row ${state.selectedEntryId === entry.id ? "is-active" : ""}" type="button" data-entry-id="${entry.id}">
      <img class="icon" src="${entry.image}" alt="${content.title}">
      <div class="entry-row__body">
        <div class="entry-row__head">
          <strong>${content.title}</strong>
          <span class="tag tag--subtle">${getCategoryLabel(entry.category)}</span>
        </div>
        <p>${content.summary}</p>
        <ul class="compact-list">${preview}</ul>
      </div>
    </button>
  `;
}

function renderEntryDetail(entry) {
  const copy = getPageCopy();
  const content = getLocalizedEntry(entry);
  const sections = ["facts", "obtain", "crafting", "drops", "pieces", "notes", "tactics"]
    .map((key) => renderDetailSection(copy.catalog[key], content[key]))
    .join("");

  const related = (entry.related ?? []).map((entryId) => {
    const relatedEntry = getEntryById(entryId);
    if (!relatedEntry) {
      return "";
    }

    const relatedContent = getLocalizedEntry(relatedEntry);
    return `<button class="chip" type="button" data-entry-id="${entryId}">${relatedContent.title}</button>`;
  }).join("");

  return `
    <article class="detail-card">
      <div class="detail-card__head">
        <img class="icon icon--detail" src="${entry.image}" alt="${content.title}">
        <div>
          <div class="detail-meta">
            <span class="tag">${getCategoryLabel(entry.category)}</span>
            <span class="tag tag--subtle">${getTagLabel(entry)}</span>
          </div>
          <h3>${content.title}</h3>
          <p class="detail-subtitle">${content.subtitle ?? ""}</p>
          <p>${content.overview ?? content.summary}</p>
        </div>
      </div>

      ${sections}

      ${related ? `
        <div class="detail-section">
          <h4>${copy.catalog.related}</h4>
          <div class="related-row">${related}</div>
        </div>
      ` : ""}
    </article>
  `;
}

function renderDetailSection(label, items) {
  if (!label || !items || items.length === 0) {
    return "";
  }

  return `
    <div class="detail-section">
      <h4>${label}</h4>
      <ul>${items.map((item) => `<li>${item}</li>`).join("")}</ul>
    </div>
  `;
}

function renderRecipeRow(entry) {
  const content = getLocalizedEntry(entry);
  return `
    <button class="entry-row ${state.selectedRecipeId === entry.id ? "is-active" : ""}" type="button" data-recipe-id="${entry.id}">
      <img class="icon" src="${entry.image}" alt="${content.title}">
      <div class="entry-row__body">
        <div class="entry-row__head">
          <strong>${content.title}</strong>
          <span class="tag tag--subtle">${getCategoryLabel(entry.category)}</span>
        </div>
        <p>${content.subtitle ?? content.summary}</p>
      </div>
    </button>
  `;
}

function renderRecipeDetail(entry) {
  const copy = getPageCopy();
  const content = getLocalizedEntry(entry);
  const recipe = splitRecipeLines(content.crafting ?? []);

  return `
    <article class="detail-card">
      <div class="detail-card__head">
        <img class="icon icon--detail" src="${entry.image}" alt="${content.title}">
        <div>
          <span class="tag">${copy.crafting.output}</span>
          <h3>${content.title}</h3>
          <p>${content.summary}</p>
        </div>
      </div>

      <div class="detail-section">
        <h4>${copy.crafting.ingredients}</h4>
        <ul>${recipe.ingredients.map((line) => `<li>${line}</li>`).join("")}</ul>
      </div>

      ${recipe.stations.length > 0 ? `
        <div class="detail-section">
          <h4>${copy.crafting.station}</h4>
          <ul>${recipe.stations.map((line) => `<li>${line}</li>`).join("")}</ul>
        </div>
      ` : ""}

      ${(content.notes ?? []).length > 0 ? `
        <div class="detail-section">
          <h4>${copy.crafting.notes}</h4>
          <ul>${content.notes.map((line) => `<li>${line}</li>`).join("")}</ul>
        </div>
      ` : ""}
    </article>
  `;
}

function syncMetadata() {
  const copy = getPageCopy();
  document.documentElement.lang = state.language;
  document.title = `${getSiteName()} | ${copy.nav.catalog}`;
  elements.metaDescription.setAttribute("content", "Chaotic Dimensions wiki with library, crafting and progression.");
  document.body.classList.toggle("sidebar-open", state.sidebarOpen);
  elements.overlay.hidden = !state.sidebarOpen;
}

function syncUrl() {
  const url = new URL(window.location.href);
  url.searchParams.set("lang", state.language);
  url.searchParams.set("entry", state.selectedEntryId);
  url.searchParams.set("recipe", state.selectedRecipeId);
  url.searchParams.set("category", state.category);

  if (state.search.trim()) {
    url.searchParams.set("q", state.search.trim());
  }
  else {
    url.searchParams.delete("q");
  }

  history.replaceState({}, "", `${url.pathname}${url.search}${url.hash}`);
}

function toggleSidebar(isOpen) {
  state.sidebarOpen = isOpen;
  document.body.classList.toggle("sidebar-open", isOpen);
  elements.overlay.hidden = !isOpen;
}

function getVisibleEntries() {
  const term = normalize(state.search);
  return allEntries.filter((entry) => {
    if (state.category !== "all" && entry.category !== state.category) {
      return false;
    }

    if (!term) {
      return true;
    }

    return normalize(buildSearchText(entry)).includes(term);
  });
}

function getVisibleRecipes() {
  const term = normalize(state.search);
  return craftableEntries.filter((entry) => {
    if (!term) {
      return true;
    }

    return normalize(buildSearchText(entry)).includes(term);
  });
}

function getEntriesByCategory(category) {
  return allEntries.filter((entry) => entry.category === category);
}

function getLocalizedEntry(entry) {
  return entry.content[state.language] ?? entry.content[siteConfig.defaultLanguage] ?? entry.content.en;
}

function getEntryById(entryId) {
  return allEntries.find((entry) => entry.id === entryId);
}

function getCategoryLabel(category) {
  const baseCopy = uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage];
  const frontierCopy = frontierUiCopy[state.language] ?? frontierUiCopy[siteConfig.defaultLanguage] ?? frontierUiCopy.en;
  return baseCopy.categories?.[category] ?? frontierCopy.categories?.[category] ?? category;
}

function getPageCopy() {
  return pageCopy[state.language] ?? pageCopy.en;
}

function getSiteName() {
  return (uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage]).siteName;
}

function getTagLabel(entry) {
  const copy = getPageCopy();
  const tagKey = entryTags[entry.id] ?? (entry.category === "bosses" ? "boss" : "mob");
  return copy.tags[tagKey] ?? tagKey;
}

function buildSearchText(entry) {
  const content = getLocalizedEntry(entry);
  return [
    entry.id,
    entry.category,
    content.title,
    content.subtitle,
    content.summary,
    content.overview,
    ...(content.facts ?? []),
    ...(content.obtain ?? []),
    ...(content.crafting ?? []),
    ...(content.drops ?? []),
    ...(content.notes ?? []),
    ...(content.tactics ?? []),
    ...(content.pieces ?? [])
  ].filter(Boolean).join(" ");
}

function splitRecipeLines(lines) {
  const stationMatchers = ["anvil", "station", "crafting station"];
  const stations = [];
  const ingredients = [];

  lines.forEach((line) => {
    const normalized = normalize(line);
    const looksLikeIngredientRow = normalized.includes("+") || normalized.includes(":") || /\d/.test(normalized);

    if (!looksLikeIngredientRow && stationMatchers.some((matcher) => normalized.includes(matcher))) {
      stations.push(line);
    }
    else {
      ingredients.push(line);
    }
  });

  return {
    ingredients: ingredients.length > 0 ? ingredients : lines,
    stations
  };
}

function hasContentList(entry, key) {
  return Object.values(entry.content ?? {}).some((content) => Array.isArray(content?.[key]) && content[key].length > 0);
}

function normalize(value) {
  return String(value ?? "").toLowerCase();
}

function escapeHtml(value) {
  return String(value ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#39;");
}
