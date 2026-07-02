import { entries, languageOptions, siteConfig, uiCopy } from "./data.js";
import {
  backendState,
  initBackend,
  loadComments,
  loadPublishedEntries,
  loadAdminComments,
  postComment,
  registerEditorAccount,
  saveWikiEntry,
  setBackendListener,
  setCommentHidden,
  signInAdmin,
  signInGuest,
  signOutUser,
  uploadWikiAsset
} from "./supabase-bridge.js";

const defaultCategories = [
  "bosses",
  "superbosses",
  "minibosses",
  "mobs",
  "summons",
  "weapons",
  "armor",
  "accessories",
  "consumables",
  "materials",
  "buffs",
  "blocks",
  "tools",
  "ammo",
  "vanity",
  "pets",
  "mounts",
  "npcs",
  "biomes",
  "dimensions",
  "events",
  "lore",
  "systems"
];
const staticEntries = [...entries];
let allEntries = mergeEntries(staticEntries, []);
let orderedCategories = buildCategoryList(allEntries);
let craftableEntries = allEntries.filter((entry) => hasContentList(entry, "crafting"));

const entryTags = {
  "crystaline-devourer": "boss"
};

const progressionFlow = [
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
      intro: "Agora esta area tambem serve para autenticacao basica e comentarios reais por entrada quando o Supabase estiver configurado.",
      cardTitle: "Conta rapida para comentar",
      cardBody: "Quem visitar a wiki pode criar uma conta simples com nome para comentar nas paginas.",
      adminCardTitle: "Login de admin",
      adminCardBody: "Sua conta admin usa email e senha para publicar, editar e moderar.",
      name: "Nome",
      email: "Email",
      password: "Senha",
      message: "Comentario",
      button: "Entrar com conta rapida",
      adminButton: "Entrar como admin",
      signOut: "Sair",
      note: "Sem configurar o Supabase, os comentarios continuam desativados.",
      activeTitle: "Conta conectada",
      activeBody: "Com a conta conectada, o formulario de comentarios aparece dentro da pagina da entrada selecionada."
    },
    admin: {
      eyebrow: "Admin",
      title: "Painel para gestao da wiki",
      intro: "Aqui fica o editor real para publicar, substituir e organizar entradas da wiki usando o Supabase como backend.",
      uploadTitle: "Upload de imagens",
      uploadItems: ["login de admin", "upload de imagens", "cadastro de categorias", "editor de crafting", "moderacao de comentarios"],
      requirementTitle: "O que falta para ativar",
      requirementBody: "Para isso funcionar de verdade, preencha a configuracao publica do Supabase e rode o SQL do projeto.",
      editorTitle: "Publicar ou editar entrada",
      uploadButton: "Enviar imagem",
      saveButton: "Salvar entrada",
      moderationTitle: "Moderacao de comentarios",
      imagePath: "Pasta da imagem",
      imageFile: "Arquivo",
      entryId: "ID / slug",
      category: "Categoria",
      imageUrl: "URL da imagem",
      related: "Relacionados (separados por virgula)",
      titleLabel: "Titulo",
      subtitleLabel: "Subtitulo",
      summaryLabel: "Resumo",
      overviewLabel: "Overview",
      factsLabel: "Fatos (1 por linha)",
      obtainLabel: "Como obter (1 por linha)",
      craftingLabel: "Crafting (1 por linha)",
      dropsLabel: "Drops (1 por linha)",
      notesLabel: "Notas (1 por linha)",
      tacticsLabel: "Taticas (1 por linha)",
      publishedLabel: "Publicado",
      accessDenied: "Entre com uma conta admin para editar entradas e moderar comentarios.",
      backendMissing: "O Supabase ainda nao foi configurado nesta build."
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
      intro: "This area now also handles quick sign-in and real comments when Supabase is configured.",
      cardTitle: "Quick account for comments",
      cardBody: "Visitors can create a very basic account with only a display name to comment on wiki entries.",
      adminCardTitle: "Admin login",
      adminCardBody: "Your admin account uses email and password to publish, edit and moderate.",
      name: "Name",
      email: "Email",
      password: "Password",
      message: "Comment",
      button: "Create quick account",
      adminButton: "Sign in as admin",
      signOut: "Sign out",
      note: "Until Supabase is configured, comments stay disabled.",
      activeTitle: "Connected account",
      activeBody: "Once connected, the comment form appears inside the currently selected entry."
    },
    admin: {
      eyebrow: "Admin",
      title: "Wiki management panel",
      intro: "This panel publishes and edits real wiki entries through Supabase.",
      uploadTitle: "Image uploads",
      uploadItems: ["admin login", "image upload", "category editor", "crafting editor", "comment moderation"],
      requirementTitle: "What still needs to be connected",
      requirementBody: "To make this live, fill the public Supabase config and run the project SQL.",
      editorTitle: "Publish or edit entry",
      uploadButton: "Upload image",
      saveButton: "Save entry",
      moderationTitle: "Comment moderation",
      imagePath: "Image folder",
      imageFile: "File",
      entryId: "ID / slug",
      category: "Category",
      imageUrl: "Image URL",
      related: "Related IDs (comma separated)",
      titleLabel: "Title",
      subtitleLabel: "Subtitle",
      summaryLabel: "Summary",
      overviewLabel: "Overview",
      factsLabel: "Facts (1 per line)",
      obtainLabel: "How to obtain (1 per line)",
      craftingLabel: "Crafting (1 per line)",
      dropsLabel: "Drops (1 per line)",
      notesLabel: "Notes (1 per line)",
      tacticsLabel: "Tactics (1 per line)",
      publishedLabel: "Published",
      accessDenied: "Sign in with an admin account to edit entries and moderate comments.",
      backendMissing: "Supabase is not configured in this build yet."
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
  sidebarOpen: false,
  adminDraft: null,
  adminDraftSourceId: ""
};

bootstrap();

setBackendListener(async () => {
  refreshEntryCache();
  await ensureSelectedEntryComments();
  render();
});

async function bootstrap() {
  hydrateStateFromUrl();
  bindGlobalEvents();
  refreshEntryCache();
  render();
  await initBackend();
  refreshEntryCache();
  await ensureSelectedEntryComments();
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
  const runtime = getRuntimeCopy();
  const accountMarkup = backendState.enabled
    ? backendState.user
      ? `
        <span class="tag">${escapeHtml(backendState.profile?.display_name ?? "Signed in")}</span>
        <button class="link-button" type="button" id="sign-out-button">${copy.community.signOut}</button>
      `
      : `<a class="link-button" href="#community">${runtime.accountLabel}</a>`
    : `<span class="tag tag--subtle">Static</span>`;

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
      ${accountMarkup}
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

  elements.topbar.querySelector("#sign-out-button")?.addEventListener("click", async () => {
    await signOutUser();
  });
}

function renderSidebar() {
  const copy = getPageCopy();
  const runtime = getRuntimeCopy();
  const statusBody = backendState.enabled
    ? backendState.user
      ? runtime.sidebarSignedIn.replace("{name}", escapeHtml(backendState.profile?.display_name ?? runtime.connectedFallback))
      : runtime.sidebarAvailable
    : copy.sidebar.statusBody;

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
      <p>${statusBody}</p>
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
  const runtime = getRuntimeCopy();
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
      <a class="text-link" href="${item.target}">${runtime.openLabel}</a>
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
    if (!entry) {
      return "";
    }
    const content = getLocalizedEntry(entry);
    const preview = (content.facts ?? []).slice(0, 2).map((fact) => `<li>${escapeHtml(fact)}</li>`).join("");
    const title = escapeHtml(content.title ?? entry.id);
    const summary = escapeHtml(content.summary ?? "");

    return `
      <article class="timeline-card">
        <div class="timeline-card__head">
          <span class="tag">${copy.progression.stages[step.stage]}</span>
          <span class="tag tag--subtle">${getTagLabel(entry)}</span>
        </div>
        <div class="timeline-card__body">
          <img class="icon icon--large" src="${escapeHtml(entry.image)}" alt="${title}">
          <div>
            <h3>${title}</h3>
            <p>${summary}</p>
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
      ensureSelectedEntryComments();
      renderCatalog();
      syncUrl();
    });
  });

  elements.catalog.querySelector("#entry-comment-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const ok = await postComment(state.selectedEntryId, formData.get("commentBody"));
    if (ok) {
      event.currentTarget.reset();
    }
    renderCatalog();
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
  const runtime = getRuntimeCopy();

  if (!backendState.enabled) {
    elements.community.innerHTML = `
      <div class="section-heading">
        <p class="section-kicker">${copy.community.eyebrow}</p>
        <h2>${copy.community.title}</h2>
        <p>${copy.community.intro}</p>
      </div>

      <div class="placeholder-grid placeholder-grid--two">
        <article class="panel">
          <h3>${runtime.setupTitle}</h3>
          <p>${runtime.setupBody}</p>
          <div class="button-row">
            <a class="link-button" href="${getRepositoryFileUrl("docs/SUPABASE_SETUP.md")}" target="_blank" rel="noreferrer">${runtime.setupLinkLabel}</a>
            <a class="link-button" href="${getRepositoryFileUrl("supabase/wiki_schema.sql")}" target="_blank" rel="noreferrer">${runtime.sqlLinkLabel}</a>
          </div>
          ${backendState.error ? `<p class="message-bar message-bar--error">${escapeHtml(backendState.error)}</p>` : ""}
        </article>

        <article class="panel panel--muted">
          <h3>${copy.community.cardTitle}</h3>
          <p>${copy.community.cardBody}</p>
          <p class="helper-text">${copy.community.note}</p>
        </article>
      </div>
    `;
    return;
  }

  if (backendState.user) {
    const currentEntry = getEntryById(state.selectedEntryId);
    const currentTitle = escapeHtml(currentEntry ? (getLocalizedEntry(currentEntry)?.title ?? state.selectedEntryId) : state.selectedEntryId);
    elements.community.innerHTML = `
      <div class="section-heading">
        <p class="section-kicker">${copy.community.eyebrow}</p>
        <h2>${copy.community.title}</h2>
        <p>${copy.community.intro}</p>
      </div>

      <div class="placeholder-grid placeholder-grid--two">
        <article class="panel">
          <h3>${copy.community.activeTitle}</h3>
          <p>${copy.community.activeBody}</p>
          <div class="meta-row">
            <span class="tag">${escapeHtml(backendState.profile?.display_name ?? runtime.connectedFallback)}</span>
            <span class="tag tag--subtle">${backendState.isAdmin ? runtime.adminRoleLabel : runtime.memberRoleLabel}</span>
          </div>
          ${(backendState.authMessage || backendState.authError) ? `
            <p class="message-bar ${backendState.authError ? "message-bar--error" : "message-bar--success"}">
              ${escapeHtml(backendState.authError || backendState.authMessage)}
            </p>
          ` : ""}
          <div class="button-row">
            <a class="link-button" href="#catalog">${copy.nav.catalog}</a>
            ${backendState.isAdmin ? `<a class="link-button" href="#admin">${copy.nav.admin}</a>` : ""}
            <button class="button button--primary" type="button" id="community-sign-out">${copy.community.signOut}</button>
          </div>
        </article>

        <article class="panel panel--muted">
          <h3>${backendState.isAdmin ? copy.admin.title : copy.community.cardTitle}</h3>
          <p>${backendState.isAdmin ? runtime.adminSignedInBody : runtime.memberSignedInBody}</p>
          <p class="helper-text">${runtime.commentHint.replace("{title}", currentTitle)}</p>
        </article>
      </div>
    `;

    elements.community.querySelector("#community-sign-out")?.addEventListener("click", async () => {
      await signOutUser();
    });
    return;
  }

  elements.community.innerHTML = `
    <div class="section-heading">
      <p class="section-kicker">${copy.community.eyebrow}</p>
      <h2>${copy.community.title}</h2>
      <p>${copy.community.intro}</p>
    </div>

    <div class="placeholder-grid placeholder-grid--two">
      <article class="panel">
        <h3>${copy.community.cardTitle}</h3>
        <p>${copy.community.cardBody}</p>
        <form class="stub-form" id="guest-sign-in-form">
          <input type="text" name="displayName" placeholder="${copy.community.name}" required>
          <button class="button button--primary" type="submit">${copy.community.button}</button>
        </form>
        <p class="helper-text">${runtime.guestAccountHint}</p>
      </article>

      <article class="panel panel--muted">
        <h3>${copy.community.adminCardTitle}</h3>
        <p>${copy.community.adminCardBody}</p>
        <form class="stub-form" id="admin-auth-form">
          <input type="text" name="displayName" placeholder="${copy.community.name}">
          <input type="email" name="email" placeholder="${copy.community.email}" required>
          <input type="password" name="password" placeholder="${copy.community.password}" required>
          <div class="button-row">
            <button class="link-button" type="submit" name="mode" value="signup">${runtime.createEditorButton}</button>
            <button class="button button--primary" type="submit" name="mode" value="signin">${copy.community.adminButton}</button>
          </div>
        </form>
        <p class="helper-text">${runtime.editorCandidateNote}</p>
        ${(backendState.authMessage || backendState.authError) ? `
          <p class="message-bar ${backendState.authError ? "message-bar--error" : "message-bar--success"}">
            ${escapeHtml(backendState.authError || backendState.authMessage)}
          </p>
        ` : ""}
      </article>
    </div>
  `;

  elements.community.querySelector("#guest-sign-in-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    await signInGuest(formData.get("displayName"));
  });

  elements.community.querySelector("#admin-auth-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const submitter = event.submitter;
    const formData = new FormData(event.currentTarget);
    const email = formData.get("email");
    const password = formData.get("password");
    const displayName = formData.get("displayName");

    if (submitter?.value === "signup") {
      await registerEditorAccount(displayName, email, password);
    }
    else {
      await signInAdmin(email, password);
    }
  });
}

function renderAdmin() {
  const copy = getPageCopy();
  const runtime = getRuntimeCopy();
  const toolList = copy.admin.uploadItems.map((item) => `<li>${item}</li>`).join("");
  const draft = ensureAdminDraft();
  const selectedEntry = getEntryById(state.selectedEntryId);
  const selectedTitle = escapeHtml(selectedEntry ? (getLocalizedEntry(selectedEntry)?.title ?? state.selectedEntryId) : state.selectedEntryId);

  if (!backendState.enabled) {
    elements.admin.innerHTML = `
      <div class="section-heading">
        <p class="section-kicker">${copy.admin.eyebrow}</p>
        <h2>${copy.admin.title}</h2>
        <p>${copy.admin.intro}</p>
      </div>

      <div class="placeholder-grid placeholder-grid--two">
        <article class="panel">
          <h3>${copy.admin.requirementTitle}</h3>
          <p>${copy.admin.requirementBody}</p>
          <div class="button-row">
            <a class="link-button" href="${getRepositoryFileUrl("docs/SUPABASE_SETUP.md")}" target="_blank" rel="noreferrer">${runtime.setupLinkLabel}</a>
            <a class="link-button" href="${getRepositoryFileUrl("supabase/wiki_schema.sql")}" target="_blank" rel="noreferrer">${runtime.sqlLinkLabel}</a>
          </div>
          ${backendState.error ? `<p class="message-bar message-bar--error">${escapeHtml(backendState.error)}</p>` : ""}
        </article>

        <article class="panel panel--muted">
          <h3>${copy.admin.uploadTitle}</h3>
          <ul>${toolList}</ul>
          <p class="helper-text">${copy.admin.backendMissing}</p>
        </article>
      </div>
    `;
    return;
  }

  if (!backendState.isAdmin) {
    elements.admin.innerHTML = `
      <div class="section-heading">
        <p class="section-kicker">${copy.admin.eyebrow}</p>
        <h2>${copy.admin.title}</h2>
        <p>${copy.admin.intro}</p>
      </div>

      <div class="placeholder-grid placeholder-grid--two">
        <article class="panel">
          <h3>${copy.admin.accessDenied}</h3>
          <p>${runtime.editorCandidateNote}</p>
          <div class="button-row">
            <a class="link-button" href="#community">${copy.nav.community}</a>
            <a class="link-button" href="${getRepositoryFileUrl("docs/SUPABASE_SETUP.md")}" target="_blank" rel="noreferrer">${runtime.setupLinkLabel}</a>
          </div>
        </article>

        <article class="panel panel--muted">
          <h3>${runtime.promotionTitle}</h3>
          <p>${runtime.promotionBody}</p>
          <a class="link-button" href="${getRepositoryFileUrl("supabase/wiki_schema.sql")}" target="_blank" rel="noreferrer">${runtime.sqlLinkLabel}</a>
        </article>
      </div>
    `;
    return;
  }

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
        <form class="stub-form" id="asset-upload-form">
          <div class="form-grid form-grid--two">
            <label class="field-group">
              <span>${copy.admin.imagePath}</span>
              <input type="text" name="pathPrefix" value="${escapeHtml(draft.imagePath)}" placeholder="entries">
            </label>

            <label class="field-group">
              <span>${copy.admin.imageFile}</span>
              <input type="file" name="imageFile" accept="image/*" required>
            </label>
          </div>
          <button class="button button--primary" type="submit">${copy.admin.uploadButton}</button>
        </form>
        ${(backendState.uploadMessage || backendState.uploadError) ? `
          <p class="message-bar ${backendState.uploadError ? "message-bar--error" : "message-bar--success"}">
            ${escapeHtml(backendState.uploadError || backendState.uploadMessage)}
          </p>
        ` : ""}
      </article>

      <article class="panel panel--muted">
        <h3>${runtime.editorContextTitle}</h3>
        <p>${runtime.currentSelectionLabel.replace("{title}", selectedTitle)}</p>
        <div class="button-row">
          <button class="link-button" type="button" id="admin-load-selected">${runtime.loadSelectedButton}</button>
          <button class="link-button" type="button" id="admin-new-entry">${runtime.newEntryButton}</button>
        </div>
        <p class="helper-text">${runtime.saveHelp}</p>
      </article>
    </div>

    <article class="panel">
      <h3>${copy.admin.editorTitle}</h3>
      <form class="stub-form" id="admin-entry-form">
        <div class="form-grid form-grid--three">
          <label class="field-group">
            <span>${copy.admin.entryId}</span>
            <input type="text" name="entryId" value="${escapeHtml(draft.id)}" required>
          </label>

          <label class="field-group">
            <span>${copy.admin.category}</span>
            <input type="text" name="category" value="${escapeHtml(draft.category)}" required>
          </label>

          <label class="field-group">
            <span>${runtime.sortOrderLabel}</span>
            <input type="number" name="sortOrder" value="${escapeHtml(draft.sortOrder)}">
          </label>
        </div>

        <div class="form-grid form-grid--two">
          <label class="field-group">
            <span>${copy.admin.imageUrl}</span>
            <input type="url" id="entry-image-url" name="imageUrl" value="${escapeHtml(draft.imageUrl)}">
          </label>

          <label class="field-group">
            <span>${copy.admin.related}</span>
            <input type="text" name="related" value="${escapeHtml(draft.related)}">
          </label>
        </div>

        <div class="form-grid form-grid--two">
          <label class="field-group">
            <span>${copy.admin.titleLabel}</span>
            <input type="text" name="title" value="${escapeHtml(draft.title)}" required>
          </label>

          <label class="field-group">
            <span>${copy.admin.subtitleLabel}</span>
            <input type="text" name="subtitle" value="${escapeHtml(draft.subtitle)}">
          </label>
        </div>

        <label class="field-group">
          <span>${copy.admin.summaryLabel}</span>
          <textarea name="summary" rows="3">${escapeHtml(draft.summary)}</textarea>
        </label>

        <label class="field-group">
          <span>${copy.admin.overviewLabel}</span>
          <textarea name="overview" rows="5">${escapeHtml(draft.overview)}</textarea>
        </label>

        <div class="form-grid form-grid--two">
          <label class="field-group">
            <span>${copy.admin.factsLabel}</span>
            <textarea name="facts" rows="6">${escapeHtml(draft.facts)}</textarea>
          </label>

          <label class="field-group">
            <span>${copy.admin.obtainLabel}</span>
            <textarea name="obtain" rows="6">${escapeHtml(draft.obtain)}</textarea>
          </label>
        </div>

        <div class="form-grid form-grid--two">
          <label class="field-group">
            <span>${copy.admin.craftingLabel}</span>
            <textarea name="crafting" rows="6">${escapeHtml(draft.crafting)}</textarea>
          </label>

          <label class="field-group">
            <span>${copy.admin.dropsLabel}</span>
            <textarea name="drops" rows="6">${escapeHtml(draft.drops)}</textarea>
          </label>
        </div>

        <div class="form-grid form-grid--two">
          <label class="field-group">
            <span>${copy.catalog.pieces}</span>
            <textarea name="pieces" rows="5">${escapeHtml(draft.pieces)}</textarea>
          </label>

          <label class="field-group">
            <span>${copy.admin.notesLabel}</span>
            <textarea name="notes" rows="5">${escapeHtml(draft.notes)}</textarea>
          </label>
        </div>

        <label class="field-group">
          <span>${copy.admin.tacticsLabel}</span>
          <textarea name="tactics" rows="5">${escapeHtml(draft.tactics)}</textarea>
        </label>

        <label class="checkbox-row">
          <input type="checkbox" name="published" ${draft.published ? "checked" : ""}>
          <span>${copy.admin.publishedLabel}</span>
        </label>

        <div class="button-row">
          <button class="button button--primary" type="submit">${copy.admin.saveButton}</button>
        </div>
      </form>
      ${(backendState.entryMessage || backendState.entryError) ? `
        <p class="message-bar ${backendState.entryError ? "message-bar--error" : "message-bar--success"}">
          ${escapeHtml(backendState.entryError || backendState.entryMessage)}
        </p>
      ` : ""}
    </article>

    <article class="panel">
      <h3>${copy.admin.moderationTitle}</h3>
      ${backendState.adminComments.length > 0 ? `
        <div class="comment-stack">
          ${backendState.adminComments.map((comment) => `
            <article class="comment-card ${comment.is_hidden ? "is-hidden" : ""}">
              <div class="comment-card__head">
                <strong>${escapeHtml(comment.display_name)}</strong>
                <div class="meta-row">
                  <span class="tag tag--subtle">${escapeHtml(comment.entry_id)}</span>
                  <span class="tag ${comment.is_hidden ? "tag--subtle" : ""}">${comment.is_hidden ? runtime.hiddenLabel : runtime.visibleLabel}</span>
                </div>
              </div>
              <p class="comment-card__body">${formatMultilineText(comment.body)}</p>
              <div class="button-row">
                <span class="helper-text">${formatDateTime(comment.created_at)}</span>
                <button class="link-button" type="button" data-toggle-comment="${comment.id}" data-comment-hidden="${comment.is_hidden}">
                  ${comment.is_hidden ? runtime.showCommentButton : runtime.hideCommentButton}
                </button>
              </div>
            </article>
          `).join("")}
        </div>
      ` : `<div class="empty-state">${runtime.moderationEmpty}</div>`}
    </article>
  `;

  elements.admin.querySelector("#admin-load-selected")?.addEventListener("click", () => {
    loadSelectedEntryIntoAdminDraft();
    renderAdmin();
  });

  elements.admin.querySelector("#admin-new-entry")?.addEventListener("click", () => {
    clearAdminDraft();
    renderAdmin();
  });

  elements.admin.querySelector("#asset-upload-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const url = await uploadWikiAsset(formData.get("imageFile"), formData.get("pathPrefix"));
    if (url) {
      ensureAdminDraft();
      state.adminDraft.imageUrl = url;
      renderAdmin();
    }
  });

  const adminEntryForm = elements.admin.querySelector("#admin-entry-form");
  adminEntryForm?.addEventListener("input", () => {
    updateAdminDraftFromForm(adminEntryForm);
  });
  adminEntryForm?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const draftState = updateAdminDraftFromForm(event.currentTarget);
    if (!draftState.id || !draftState.category || !draftState.title) {
      backendState.entryError = runtime.requiredEntryFields;
      backendState.entryMessage = "";
      renderAdmin();
      return;
    }

    const baseEntry = getEntryById(draftState.id);
    const payload = {
      id: draftState.id,
      category: draftState.category,
      image_url: draftState.imageUrl || baseEntry?.image || "./assets/images/favicon.png",
      related_ids: parseCsvList(draftState.related),
      sort_order: Number(draftState.sortOrder || 0),
      is_published: draftState.published,
      content_json: buildLocalizedContentPayload(baseEntry, draftState)
    };

    const ok = await saveWikiEntry(payload);
    if (ok) {
      state.selectedEntryId = draftState.id;
      state.category = draftState.category || "all";
      refreshEntryCache();
      await ensureSelectedEntryComments(true);
      render();
    }
  });

  elements.admin.querySelectorAll("[data-toggle-comment]").forEach((button) => {
    button.addEventListener("click", async () => {
      const commentId = button.dataset.toggleComment;
      const isHidden = button.dataset.commentHidden === "true";
      await setCommentHidden(commentId, !isHidden);
      await ensureSelectedEntryComments(true);
      render();
    });
  });
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
  const preview = (content.facts ?? []).slice(0, 2).map((fact) => `<li>${escapeHtml(fact)}</li>`).join("");
  const title = escapeHtml(content.title ?? entry.id);
  const summary = escapeHtml(content.summary ?? "");

  return `
    <button class="entry-row ${state.selectedEntryId === entry.id ? "is-active" : ""}" type="button" data-entry-id="${entry.id}">
      <img class="icon" src="${escapeHtml(entry.image)}" alt="${title}">
      <div class="entry-row__body">
        <div class="entry-row__head">
          <strong>${title}</strong>
          <span class="tag tag--subtle">${getCategoryLabel(entry.category)}</span>
        </div>
        <p>${summary}</p>
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
  const title = escapeHtml(content.title ?? entry.id);
  const subtitle = escapeHtml(content.subtitle ?? "");
  const overview = escapeHtml(content.overview ?? content.summary ?? "");

  const related = (entry.related ?? []).map((entryId) => {
    const relatedEntry = getEntryById(entryId);
    if (!relatedEntry) {
      return "";
    }

    const relatedContent = getLocalizedEntry(relatedEntry);
    return `<button class="chip" type="button" data-entry-id="${entryId}">${escapeHtml(relatedContent.title ?? entryId)}</button>`;
  }).join("");

  return `
    <article class="detail-card">
      <div class="detail-card__head">
        <img class="icon icon--detail" src="${escapeHtml(entry.image)}" alt="${title}">
        <div>
          <div class="detail-meta">
            <span class="tag">${getCategoryLabel(entry.category)}</span>
            <span class="tag tag--subtle">${getTagLabel(entry)}</span>
          </div>
          <h3>${title}</h3>
          <p class="detail-subtitle">${subtitle}</p>
          <p>${overview}</p>
        </div>
      </div>

      ${sections}

      ${related ? `
        <div class="detail-section">
          <h4>${copy.catalog.related}</h4>
          <div class="related-row">${related}</div>
        </div>
      ` : ""}

      ${renderCommentsSection(entry)}
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
      <ul>${items.map((item) => `<li>${escapeHtml(item)}</li>`).join("")}</ul>
    </div>
  `;
}

function renderRecipeRow(entry) {
  const content = getLocalizedEntry(entry);
  const title = escapeHtml(content.title ?? entry.id);
  const subtitle = escapeHtml(content.subtitle ?? content.summary ?? "");
  return `
    <button class="entry-row ${state.selectedRecipeId === entry.id ? "is-active" : ""}" type="button" data-recipe-id="${entry.id}">
      <img class="icon" src="${escapeHtml(entry.image)}" alt="${title}">
      <div class="entry-row__body">
        <div class="entry-row__head">
          <strong>${title}</strong>
          <span class="tag tag--subtle">${getCategoryLabel(entry.category)}</span>
        </div>
        <p>${subtitle}</p>
      </div>
    </button>
  `;
}

function renderRecipeDetail(entry) {
  const copy = getPageCopy();
  const content = getLocalizedEntry(entry);
  const recipe = splitRecipeLines(content.crafting ?? []);
  const title = escapeHtml(content.title ?? entry.id);
  const summary = escapeHtml(content.summary ?? "");

  return `
    <article class="detail-card">
      <div class="detail-card__head">
        <img class="icon icon--detail" src="${escapeHtml(entry.image)}" alt="${title}">
        <div>
          <span class="tag">${copy.crafting.output}</span>
          <h3>${title}</h3>
          <p>${summary}</p>
        </div>
      </div>

      <div class="detail-section">
        <h4>${copy.crafting.ingredients}</h4>
        <ul>${recipe.ingredients.map((line) => `<li>${escapeHtml(line)}</li>`).join("")}</ul>
      </div>

      ${recipe.stations.length > 0 ? `
        <div class="detail-section">
          <h4>${copy.crafting.station}</h4>
          <ul>${recipe.stations.map((line) => `<li>${escapeHtml(line)}</li>`).join("")}</ul>
        </div>
      ` : ""}

      ${(content.notes ?? []).length > 0 ? `
        <div class="detail-section">
          <h4>${copy.crafting.notes}</h4>
          <ul>${content.notes.map((line) => `<li>${escapeHtml(line)}</li>`).join("")}</ul>
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
  return escapeHtml(baseCopy.categories?.[category] ?? humanizeCategory(category));
}

function getPageCopy() {
  return pageCopy[state.language] ?? pageCopy.en;
}

function getSiteName() {
  return (uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage]).siteName;
}

function getTagLabel(entry) {
  const copy = getPageCopy();
  const tagKey = entryTags[entry.id]
    ?? (entry.category === "bosses" || entry.category === "superbosses" ? "boss" : entry.category === "minibosses" ? "miniboss" : "mob");
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

function getRuntimeCopy() {
  if (state.language === "pt-BR") {
    return {
      openLabel: "Abrir",
      accountLabel: "Conta",
      connectedFallback: "conectado",
      sidebarSignedIn: "Supabase ativo. Usuario atual: {name}.",
      sidebarAvailable: "Supabase ativo. Visitantes ja podem criar conta rapida e comentar.",
      setupTitle: "Backend real via Supabase",
      setupBody: "A wiki continua no GitHub Pages, mas contas, comentarios, upload e edicao passam a funcionar pelo Supabase.",
      setupLinkLabel: "Abrir guia do Supabase",
      sqlLinkLabel: "Abrir SQL",
      guestAccountHint: "Esse fluxo usa conta anonima do Supabase, entao o visitante comenta so com nome.",
      createEditorButton: "Criar conta de editor",
      editorCandidateNote: "A conta criada por email e senha ainda precisa ser promovida para admin no SQL para liberar publicacao e edicao.",
      memberRoleLabel: "Membro",
      adminRoleLabel: "Admin",
      memberSignedInBody: "Sua conta ja pode comentar nas paginas da wiki. Abra qualquer entrada na Biblioteca para deixar feedback.",
      adminSignedInBody: "Sua conta admin ja pode publicar entradas, subir imagens e moderar comentarios na aba Admin.",
      commentHint: "Entrada atual para comentar: {title}",
      promotionTitle: "Promocao para admin",
      promotionBody: "Depois de criar ou entrar com sua conta de email, rode o SQL de promocao no projeto Supabase para marcar esse usuario como admin.",
      editorContextTitle: "Contexto do editor",
      currentSelectionLabel: "Entrada selecionada na Biblioteca: {title}",
      loadSelectedButton: "Carregar entrada selecionada",
      newEntryButton: "Nova entrada",
      saveHelp: "O editor salva o idioma atual e preserva as outras traducoes ja existentes quando houver.",
      sortOrderLabel: "Ordem",
      moderationEmpty: "Nenhum comentario recente para moderar.",
      hideCommentButton: "Ocultar",
      showCommentButton: "Mostrar",
      hiddenLabel: "Oculto",
      visibleLabel: "Visivel",
      requiredEntryFields: "Preencha pelo menos slug, categoria e titulo antes de salvar.",
      commentsTitle: "Comentarios",
      commentsEmpty: "Ainda nao ha comentarios nesta entrada.",
      commentsLoading: "Carregando comentarios...",
      commentsSignIn: "Entre pela aba Feedback para comentar nesta entrada.",
      commentButton: "Publicar comentario",
      commentSetupBody: "Os comentarios reais ficam disponiveis assim que o Supabase estiver configurado nesta build."
    };
  }

  return {
    openLabel: "Open",
    accountLabel: "Account",
    connectedFallback: "connected",
    sidebarSignedIn: "Supabase is live. Current user: {name}.",
    sidebarAvailable: "Supabase is live. Visitors can already create quick accounts and comment.",
    setupTitle: "Real backend with Supabase",
    setupBody: "The wiki stays on GitHub Pages, while accounts, comments, uploads and editing are powered by Supabase.",
    setupLinkLabel: "Open Supabase guide",
    sqlLinkLabel: "Open SQL",
    guestAccountHint: "This flow uses Supabase anonymous users, so visitors only need a display name to comment.",
    createEditorButton: "Create editor account",
    editorCandidateNote: "Email and password users still need the SQL promotion step before they gain admin publishing access.",
    memberRoleLabel: "Member",
    adminRoleLabel: "Admin",
    memberSignedInBody: "Your account can already comment on wiki pages. Open any entry in the Library to leave feedback.",
    adminSignedInBody: "Your admin account can already publish entries, upload images and moderate comments in the Admin tab.",
    commentHint: "Current entry to comment on: {title}",
    promotionTitle: "Admin promotion",
    promotionBody: "After creating or signing in with your email account, run the promotion SQL in Supabase to mark that user as admin.",
    editorContextTitle: "Editor context",
    currentSelectionLabel: "Selected Library entry: {title}",
    loadSelectedButton: "Load selected entry",
    newEntryButton: "New entry",
    saveHelp: "The editor saves the current language and preserves any other translations that already exist.",
    sortOrderLabel: "Order",
    moderationEmpty: "No recent comments to moderate.",
    hideCommentButton: "Hide",
    showCommentButton: "Show",
    hiddenLabel: "Hidden",
    visibleLabel: "Visible",
    requiredEntryFields: "Fill in at least the slug, category and title before saving.",
    commentsTitle: "Comments",
    commentsEmpty: "There are no comments on this entry yet.",
    commentsLoading: "Loading comments...",
    commentsSignIn: "Sign in through the Feedback section to comment on this entry.",
    commentButton: "Publish comment",
    commentSetupBody: "Real comments become available as soon as Supabase is configured in this build."
  };
}

function mergeEntries(baseEntries, publishedEntries) {
  const merged = new Map();

  baseEntries.forEach((entry) => {
    merged.set(entry.id, cloneEntry(entry));
  });

  publishedEntries.forEach((entry) => {
    const existing = merged.get(entry.id);
    merged.set(entry.id, {
      id: entry.id,
      category: entry.category ?? existing?.category ?? "materials",
      image: entry.image ?? existing?.image ?? "./assets/images/favicon.png",
      related: Array.isArray(entry.related) ? [...entry.related] : [...(existing?.related ?? [])],
      sortOrder: Number(entry.sortOrder ?? existing?.sortOrder ?? 0),
      isPublished: entry.isPublished ?? existing?.isPublished ?? true,
      content: mergeContentObjects(existing?.content ?? {}, entry.content ?? {})
    });
  });

  return [...merged.values()].sort(compareEntries);
}

function mergeContentObjects(baseContent, nextContent) {
  const merged = { ...baseContent };
  Object.entries(nextContent ?? {}).forEach(([languageCode, content]) => {
    merged[languageCode] = {
      ...(baseContent?.[languageCode] ?? {}),
      ...(content ?? {})
    };
  });
  return merged;
}

function cloneEntry(entry) {
  return {
    id: entry.id,
    category: entry.category,
    image: entry.image,
    related: [...(entry.related ?? [])],
    sortOrder: Number(entry.sortOrder ?? 0),
    isPublished: entry.isPublished ?? true,
    content: mergeContentObjects({}, entry.content ?? {})
  };
}

function compareEntries(left, right) {
  const sortDifference = Number(left.sortOrder ?? 0) - Number(right.sortOrder ?? 0);
  if (sortDifference !== 0) {
    return sortDifference;
  }

  const categoryDifference = normalize(left.category).localeCompare(normalize(right.category));
  if (categoryDifference !== 0) {
    return categoryDifference;
  }

  return normalize(getEntrySortTitle(left)).localeCompare(normalize(getEntrySortTitle(right)));
}

function buildCategoryList(entryList) {
  const found = new Set(entryList.map((entry) => entry.category).filter(Boolean));
  const ordered = defaultCategories.filter((category) => found.has(category));
  const extras = [...found]
    .filter((category) => !defaultCategories.includes(category))
    .sort((left, right) => normalize(humanizeCategory(left)).localeCompare(normalize(humanizeCategory(right))));

  return [...ordered, ...extras];
}

function refreshEntryCache() {
  allEntries = mergeEntries(staticEntries, backendState.publishedEntries ?? []);
  orderedCategories = buildCategoryList(allEntries);
  craftableEntries = allEntries.filter((entry) => hasContentList(entry, "crafting"));

  if (!allEntries.some((entry) => entry.id === state.selectedEntryId)) {
    state.selectedEntryId = allEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  if (!craftableEntries.some((entry) => entry.id === state.selectedRecipeId)) {
    state.selectedRecipeId = craftableEntries[0]?.id ?? allEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  if (state.category !== "all" && !orderedCategories.includes(state.category)) {
    state.category = "all";
  }
}

async function ensureSelectedEntryComments(force = false) {
  if (!backendState.enabled || !state.selectedEntryId) {
    return [];
  }

  const current = backendState.commentsByEntry[state.selectedEntryId];
  if (!force && (current?.loading || Array.isArray(current?.items) || current?.error)) {
    return current?.items ?? [];
  }

  return loadComments(state.selectedEntryId);
}

function renderCommentsSection(entry) {
  const runtime = getRuntimeCopy();
  const copy = getPageCopy();

  if (!backendState.enabled) {
    return `
      <div class="detail-section detail-section--comments">
        <h4>${runtime.commentsTitle}</h4>
        <p>${runtime.commentSetupBody}</p>
      </div>
    `;
  }

  const thread = backendState.commentsByEntry[entry.id];
  const itemsMarkup = thread?.loading
    ? `<div class="empty-state">${runtime.commentsLoading}</div>`
    : thread?.error
      ? `<div class="empty-state">${escapeHtml(thread.error)}</div>`
      : renderCommentItems(thread?.items ?? []);

  const formMarkup = backendState.user
    ? `
      <form class="stub-form" id="entry-comment-form">
        <textarea name="commentBody" rows="4" placeholder="${copy.community.message}" required></textarea>
        <button class="button button--primary" type="submit">${runtime.commentButton}</button>
      </form>
    `
    : `<p class="helper-text">${runtime.commentsSignIn}</p>`;

  const messageMarkup = (backendState.commentMessage || backendState.commentError)
    ? `
      <p class="message-bar ${backendState.commentError ? "message-bar--error" : "message-bar--success"}">
        ${escapeHtml(backendState.commentError || backendState.commentMessage)}
      </p>
    `
    : "";

  return `
    <div class="detail-section detail-section--comments">
      <h4>${runtime.commentsTitle}</h4>
      <div class="comment-stack">${itemsMarkup}</div>
      ${formMarkup}
      ${messageMarkup}
    </div>
  `;
}

function renderCommentItems(items) {
  const runtime = getRuntimeCopy();

  if (!items || items.length === 0) {
    return `<div class="empty-state">${runtime.commentsEmpty}</div>`;
  }

  return items.map((comment) => `
    <article class="comment-card ${comment.is_hidden ? "is-hidden" : ""}">
      <div class="comment-card__head">
        <strong>${escapeHtml(comment.display_name)}</strong>
        <div class="meta-row">
          <span class="tag tag--subtle">${formatDateTime(comment.created_at)}</span>
          ${comment.is_hidden ? `<span class="tag tag--subtle">${runtime.hiddenLabel}</span>` : ""}
        </div>
      </div>
      <p class="comment-card__body">${formatMultilineText(comment.body)}</p>
    </article>
  `).join("");
}

function formatDateTime(value) {
  if (!value) {
    return "";
  }

  return new Intl.DateTimeFormat(state.language, {
    dateStyle: "medium",
    timeStyle: "short"
  }).format(new Date(value));
}

function formatMultilineText(value) {
  return escapeHtml(value ?? "").replaceAll("\n", "<br>");
}

function getRepositoryFileUrl(repoPath) {
  return `${siteConfig.repoUrl}/blob/main/${repoPath}`;
}

function humanizeCategory(category) {
  return String(category ?? "")
    .replace(/[-_]+/g, " ")
    .replace(/\s+/g, " ")
    .trim()
    .replace(/\b\w/g, (character) => character.toUpperCase());
}

function getEntrySortTitle(entry) {
  return entry?.content?.[siteConfig.defaultLanguage]?.title
    ?? entry?.content?.en?.title
    ?? Object.values(entry?.content ?? {})[0]?.title
    ?? entry?.id
    ?? "";
}

function parseMultilineList(value) {
  return String(value ?? "")
    .split(/\r?\n/g)
    .map((line) => line.trim())
    .filter(Boolean);
}

function parseCsvList(value) {
  return String(value ?? "")
    .split(",")
    .map((item) => item.trim())
    .filter(Boolean);
}

function joinLines(value) {
  return Array.isArray(value) ? value.join("\n") : "";
}

function ensureAdminDraft() {
  if (!state.adminDraft) {
    loadSelectedEntryIntoAdminDraft();
  }

  return state.adminDraft;
}

function loadSelectedEntryIntoAdminDraft(entryId = state.selectedEntryId) {
  state.adminDraftSourceId = entryId;
  state.adminDraft = buildAdminDraft(getEntryById(entryId));
}

function clearAdminDraft() {
  state.adminDraftSourceId = "";
  state.adminDraft = buildAdminDraft(null);
}

function updateAdminDraftFromForm(form) {
  const formData = new FormData(form);
  state.adminDraft = {
    id: String(formData.get("entryId") ?? "").trim(),
    category: String(formData.get("category") ?? "").trim(),
    sortOrder: String(formData.get("sortOrder") ?? "0").trim(),
    imageUrl: String(formData.get("imageUrl") ?? "").trim(),
    imagePath: state.adminDraft?.imagePath ?? "entries",
    related: String(formData.get("related") ?? "").trim(),
    title: String(formData.get("title") ?? "").trim(),
    subtitle: String(formData.get("subtitle") ?? "").trim(),
    summary: String(formData.get("summary") ?? "").trim(),
    overview: String(formData.get("overview") ?? "").trim(),
    facts: String(formData.get("facts") ?? "").trim(),
    obtain: String(formData.get("obtain") ?? "").trim(),
    crafting: String(formData.get("crafting") ?? "").trim(),
    drops: String(formData.get("drops") ?? "").trim(),
    pieces: String(formData.get("pieces") ?? "").trim(),
    notes: String(formData.get("notes") ?? "").trim(),
    tactics: String(formData.get("tactics") ?? "").trim(),
    published: form.querySelector('[name="published"]')?.checked ?? false
  };

  return state.adminDraft;
}

function buildAdminDraft(entry) {
  const content = entry ? getLocalizedEntry(entry) : {};

  return {
    id: entry?.id ?? "",
    category: entry?.category ?? (state.category !== "all" ? state.category : "materials"),
    sortOrder: String(entry?.sortOrder ?? 0),
    imageUrl: entry?.image ?? "",
    imagePath: state.adminDraft?.imagePath ?? "entries",
    related: (entry?.related ?? []).join(", "),
    title: content?.title ?? "",
    subtitle: content?.subtitle ?? "",
    summary: content?.summary ?? "",
    overview: content?.overview ?? "",
    facts: joinLines(content?.facts),
    obtain: joinLines(content?.obtain),
    crafting: joinLines(content?.crafting),
    drops: joinLines(content?.drops),
    pieces: joinLines(content?.pieces),
    notes: joinLines(content?.notes),
    tactics: joinLines(content?.tactics),
    published: Boolean(entry?.isPublished ?? backendState.publishedEntries.some((publishedEntry) => publishedEntry.id === entry?.id))
  };
}

function buildLocalizedContentPayload(baseEntry, draft) {
  const existingContent = mergeContentObjects({}, baseEntry?.content ?? {});
  const currentLanguageContent = existingContent[state.language] ?? existingContent[siteConfig.defaultLanguage] ?? existingContent.en ?? {};
  const nextLanguageContent = {
    ...currentLanguageContent,
    title: draft.title,
    subtitle: draft.subtitle,
    summary: draft.summary,
    overview: draft.overview,
    facts: parseMultilineList(draft.facts),
    obtain: parseMultilineList(draft.obtain),
    crafting: parseMultilineList(draft.crafting),
    drops: parseMultilineList(draft.drops),
    pieces: parseMultilineList(draft.pieces),
    notes: parseMultilineList(draft.notes),
    tactics: parseMultilineList(draft.tactics)
  };

  const nextContent = {
    ...existingContent,
    [state.language]: nextLanguageContent
  };

  languageOptions.forEach((option) => {
    if (!nextContent[option.code]) {
      nextContent[option.code] = nextLanguageContent;
    }
  });

  return nextContent;
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
