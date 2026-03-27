import { entries, languageOptions, siteConfig, uiCopy } from "./data.js";
import { generatedTerrariaAssets } from "./generated-terraria-assets.js";
import {
  backendState,
  initBackend,
  loadAdminComments,
  loadComments,
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

const PAGE_FILES = {
  home: "index.html",
  library: "library.html",
  entry: "entry.html",
  crafting: "crafting.html",
  progression: "progression.html",
  feedback: "feedback.html",
  admin: "admin.html"
};

const CATEGORY_ORDER = [
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

const ENTRY_TAGS = {
  "crystaline-devourer": "boss"
};

const PROGRESSION_GROUPS = [
  { key: "pre_hardmode" },
  { key: "pre_moonlord" },
  { key: "post_moonlord" }
];

const WORKSTATIONS = [
  { id: "lunar-crafting-station", label: "Lunar Crafting Station", short: "LC", keywords: ["lunar crafting station"] },
  { id: "ancient-manipulator", label: "Ancient Manipulator", short: "AM", keywords: ["ancient manipulator"] },
  { id: "mythril-anvil", label: "Mythril Anvil", short: "MY", keywords: ["mythril anvil", "orichalcum anvil"] },
  { id: "adamantite-forge", label: "Adamantite Forge", short: "AF", keywords: ["adamantite forge"] },
  { id: "anvil", label: "Anvil", short: "AN", keywords: [" anvil", "anvil "] },
  { id: "furnace", label: "Furnace", short: "FU", keywords: ["hellforge", "forge", "furnace"] }
];

const HOME_FEATURED_IDS = [
  "crystaline-devourer"
];

const DEFAULT_ENTRY_IMAGE = "./assets/images/favicon.png";
const TERRARIA_WIKI = {
  apiUrl: "https://terraria.wiki.gg/api.php",
  pageBaseUrl: "https://terraria.wiki.gg/wiki/"
};

const RUNTIME_TERRARIA_LOOKUP_ENABLED = false;

const staticEntries = mergeStaticSources(entries);
const pageId = document.body.dataset.page ?? "home";

const elements = {
  header: document.querySelector("#site-header"),
  main: document.querySelector("#page-root"),
  footer: document.querySelector("#site-footer"),
  metaDescription: document.querySelector("#meta-description")
};

const pageCopy = {
  "pt-BR": {
    siteLabel: "Wiki Oficial",
    nav: {
      home: "Inicio",
      library: "Biblioteca",
      crafting: "Crafting",
      progression: "Progressao",
      feedback: "Feedback",
      admin: "Admin"
    },
    header: {
      account: "Entrar",
      signOut: "Sair",
      static: "Static"
    },
    home: {
      title: "Uma wiki mais profissional, organizada por paginas e feita para crescer.",
      lead: "A base agora trabalha como uma wiki de referencia: cada area tem sua propria pagina, a navegacao fica limpa no topo e as informacoes deixam de competir no mesmo lugar.",
      introTitle: "Comece pelo que voce precisa",
      introBody: "Use a Biblioteca para descobrir entradas, abra paginas dedicadas para cada item ou boss, consulte receitas em Crafting e acompanhe a Progressao sem a poluicao de uma pagina unica.",
      links: [
        { page: "library", title: "Biblioteca", body: "Lista limpa de itens, mobs, bosses e sistemas." },
        { page: "crafting", title: "Crafting", body: "Receitas separadas da listagem principal." },
        { page: "progression", title: "Progressao", body: "Ordem sugerida de encounters e gates." },
        { page: "feedback", title: "Feedback", body: "Login, comentarios e retorno da comunidade." }
      ],
      categoryTitle: "Categorias ativas",
      categoryBody: "Cada categoria vira um filtro real na Biblioteca e pode continuar crescendo sem quebrar o layout.",
      featuredTitle: "Destaques atuais",
      featuredBody: "Entradas importantes para iniciar a documentacao do mod."
    },
    library: {
      title: "Biblioteca",
      body: "Uma listagem limpa para navegar por entradas e abrir paginas dedicadas.",
      search: "Buscar por item, mob, boss ou sistema",
      category: "Categoria",
      all: "Tudo",
      empty: "Nenhuma entrada combinou com o filtro atual.",
      open: "Abrir pagina",
      results: "Resultados"
    },
    entry: {
      back: "Voltar para a biblioteca",
      openCrafting: "Ver em Crafting",
      facts: "Fatos rapidos",
      summon: "Invocacao",
      obtain: "Como obter",
      crafting: "Crafting",
      drops: "Drops",
      pieces: "Pecas",
      notes: "Notas",
      tactics: "Taticas",
      usedIn: "Usado em",
      related: "Relacionados",
      comments: "Comentarios",
      commentsHint: "Entre pela pagina de Feedback para comentar nesta entrada.",
      commentsEmpty: "Ainda nao ha comentarios nesta entrada.",
      commentButton: "Publicar comentario",
      commentPlaceholder: "Escreva um comentario util para esta pagina",
      notFoundTitle: "Entrada nao encontrada",
      notFoundBody: "Essa pagina ainda nao tem uma entrada valida. Volte para a Biblioteca e abra outra ficha."
    },
    crafting: {
      title: "Crafting",
      body: "Receitas em uma pagina propria, com consulta mais clara e menos ruido visual.",
      search: "Buscar receita ou ingrediente",
      empty: "Nenhuma receita encontrada para o filtro atual.",
      ingredients: "Ingredientes",
      station: "Estacao",
      output: "Resultado",
      open: "Abrir entrada",
      allTags: "Todas as tags"
    },
    progression: {
      title: "Progressao",
      body: "Uma rota simples para ligar spawn, drops, crafting e checkpoints do mod.",
      open: "Abrir pagina",
      summon: "Invocacao",
      crafting: "Como chamar",
      curiosities: "Curiosidades",
      groups: {
        pre_hardmode: {
          title: "Pre-Hardmode",
          body: "Criaturas, minibosses e loops iniciais antes da quebra do mundo."
        },
        pre_moonlord: {
          title: "Pre-Moon Lord",
          body: "Hardmode avancado com encontros que preparam o salto final da progressao."
        },
        post_moonlord: {
          title: "Post-Moon Lord",
          body: "Conteudo de pico, pensado para encounters e rewards do endgame atual."
        }
      }
    },
    feedback: {
      title: "Feedback e contas",
      body: "Separado da wiki principal para manter a navegacao limpa. Aqui voce cria conta, faz login e entende como comentar nas paginas das entradas.",
      guestTitle: "Conta rapida para comentarios",
      guestBody: "Visitantes podem entrar com um nome e comentar sem precisar criar um perfil complexo.",
      guestButton: "Entrar com conta rapida",
      adminTitle: "Login de editor e admin",
      adminBody: "Use email e senha para criar sua conta de editor ou entrar na conta que vai virar admin.",
      createEditor: "Criar conta de editor",
      loginAdmin: "Entrar como admin",
      activeTitle: "Conta conectada",
      activeBody: "Sua sessao esta pronta. Va ate a pagina de uma entrada para comentar ou abra o painel Admin se essa conta ja tiver permissao.",
      name: "Nome",
      email: "Email",
      password: "Senha",
      signOut: "Sair",
      memberRole: "Membro",
      adminRole: "Admin"
    },
    admin: {
      title: "Painel Admin",
      body: "Um painel mais profissional para editar conteudo, publicar entradas, subir imagens e moderar comentarios.",
      accessTitle: "Acesso restrito",
      accessBody: "Entre com uma conta admin para liberar o workspace de edicao.",
      setupTitle: "Fluxo de publicacao",
      setupBody: "Crie sua conta no Feedback, promova para admin no SQL e depois volte aqui para publicar e editar.",
      browserTitle: "Browser de entradas",
      browserSearch: "Buscar entrada por nome ou slug",
      browserFilter: "Filtrar categoria",
      browserAll: "Todas as categorias",
      browserNew: "Nova entrada",
      browserResults: "resultados",
      browserPublished: "publicadas",
      browserDrafts: "rascunhos",
      browserEmpty: "Nenhuma entrada bateu com a busca atual.",
      browserLoad: "Carregar",
      editorTitle: "Editor de entrada",
      workspaceTitle: "Workspace de publicacao",
      workspaceBody: "Edite metadados, conteudo principal, imagem e relacoes sem misturar tudo em uma pagina publica.",
      identityTitle: "Identidade e publicacao",
      identityBody: "Defina slug, categoria, ordem, relacoes e status de publicacao da entrada.",
      mediaTitle: "Midia principal",
      mediaBody: "Escolha a imagem principal da pagina e faça upload sem sair do painel.",
      discoveryTitle: "Sprite fallback e wiki vanilla",
      discoveryBody: "Use alias e fonte wiki quando o nome nao bater sozinho. A fallback image entra se nao houver sprite local.",
      metadataTitle: "Metadados",
      contentTitle: "Conteudo da pagina",
      previewTitle: "Preview rapido",
      previewEmpty: "Preencha titulo, resumo e imagem para visualizar melhor a ficha antes de publicar.",
      openEntry: "Abrir pagina",
      openLibrary: "Abrir biblioteca",
      uploadTitle: "Assets",
      moderationTitle: "Moderacao de comentarios",
      save: "Salvar entrada",
      upload: "Enviar imagem",
      noComments: "Nenhum comentario recente para moderar.",
      fields: {
        slug: "Slug",
        category: "Categoria",
        order: "Ordem",
        imageUrl: "Imagem principal",
        fallbackImage: "Fallback image URL",
        vanillaAlias: "Alias vanilla",
        wikiSource: "Wiki source",
        imageFolder: "Pasta da imagem",
        imageFile: "Arquivo",
        related: "Relacionados (slug, slug)",
        title: "Titulo",
        subtitle: "Subtitulo",
        summary: "Resumo",
        overview: "Overview",
        facts: "Fatos (1 por linha)",
        obtain: "Como obter (1 por linha)",
        crafting: "Crafting (1 por linha)",
        drops: "Drops (1 por linha)",
        pieces: "Pecas (1 por linha)",
        notes: "Notas (1 por linha)",
        tactics: "Taticas (1 por linha)",
        published: "Publicado"
      },
      hints: {
        imageUrl: "URL principal da imagem usada pela pagina e pelo card da biblioteca.",
        fallbackImage: "Imagem reserva se a entrada ainda nao tiver sprite propria.",
        vanillaAlias: "Nome vanilla exato para buscar sprite automaticamente na Terraria Wiki.",
        wikiSource: "Titulo da pagina vanilla ou URL completa da Terraria Wiki para forcar a origem.",
        related: "Use slugs separados por virgula para ligar summon, set, drops ou paginas irmas.",
        crafting: "Uma linha por receita. O parser detecta ingredientes, quantidades e bancada automaticamente."
      },
      moderationHide: "Ocultar",
      moderationShow: "Mostrar"
    },
    common: {
      openPage: "Abrir pagina",
      liveSite: "Pages",
      releases: "Releases",
      github: "GitHub",
      noData: "Nada para mostrar aqui ainda.",
      notConfigured: "Supabase ainda nao esta configurado nesta build.",
      connectedAs: "Conectado como"
    }
  },
  en: {
    siteLabel: "Official Wiki",
    nav: {
      home: "Home",
      library: "Library",
      crafting: "Crafting",
      progression: "Progression",
      feedback: "Feedback",
      admin: "Admin"
    },
    header: {
      account: "Sign in",
      signOut: "Sign out",
      static: "Static"
    },
    home: {
      title: "A cleaner, more professional wiki with dedicated pages.",
      lead: "The new structure behaves like a real reference wiki: focused pages, top navigation and content that no longer competes inside one oversized layout.",
      introTitle: "Start from the page you need",
      introBody: "Use the Library to browse entries, open dedicated pages for items and bosses, check recipes in Crafting and follow Progression without a noisy all-in-one screen.",
      links: [
        { page: "library", title: "Library", body: "Clean list of items, mobs, bosses and systems." },
        { page: "crafting", title: "Crafting", body: "Recipes separated from the main listing." },
        { page: "progression", title: "Progression", body: "Suggested encounters and gates." },
        { page: "feedback", title: "Feedback", body: "Login, comments and community input." }
      ],
      categoryTitle: "Active categories",
      categoryBody: "Each category becomes a real library filter and can keep expanding without breaking the layout.",
      featuredTitle: "Current highlights",
      featuredBody: "Important entries to anchor the mod documentation."
    },
    library: {
      title: "Library",
      body: "A clean listing built to open dedicated pages for every entry.",
      search: "Search item, mob, boss or system",
      category: "Category",
      all: "All",
      empty: "No entry matched the current filter.",
      open: "Open page",
      results: "Results"
    },
    entry: {
      back: "Back to library",
      openCrafting: "View in Crafting",
      facts: "Quick facts",
      summon: "Summon",
      obtain: "How to obtain",
      crafting: "Crafting",
      drops: "Drops",
      pieces: "Pieces",
      notes: "Notes",
      tactics: "Tactics",
      usedIn: "Used in",
      related: "Related",
      comments: "Comments",
      commentsHint: "Sign in through the Feedback page to comment on this entry.",
      commentsEmpty: "There are no comments on this entry yet.",
      commentButton: "Publish comment",
      commentPlaceholder: "Write a useful comment for this page",
      notFoundTitle: "Entry not found",
      notFoundBody: "This page does not have a valid entry yet. Go back to the Library and open another sheet."
    },
    crafting: {
      title: "Crafting",
      body: "Recipes now live on their own page for clearer lookup.",
      search: "Search recipe or ingredient",
      empty: "No recipe matched the current filter.",
      ingredients: "Ingredients",
      station: "Station",
      output: "Result",
      open: "Open entry",
      allTags: "All tags"
    },
    progression: {
      title: "Progression",
      body: "A simple route that links spawns, drops, crafting and progression checkpoints.",
      open: "Open page",
      summon: "Summon",
      crafting: "How to call it",
      curiosities: "Notes",
      groups: {
        pre_hardmode: {
          title: "Pre-Hardmode",
          body: "Creatures, minibosses and early loops before the world break."
        },
        pre_moonlord: {
          title: "Pre-Moon Lord",
          body: "Late hardmode encounters that prepare the final jump in progression."
        },
        post_moonlord: {
          title: "Post-Moon Lord",
          body: "Peak content built around current endgame encounters and rewards."
        }
      }
    },
    feedback: {
      title: "Feedback and accounts",
      body: "Separated from the main wiki so navigation stays focused. Create an account, sign in and learn how to comment on entry pages.",
      guestTitle: "Quick comment account",
      guestBody: "Visitors can sign in with just a name and leave feedback without a complex profile flow.",
      guestButton: "Use quick account",
      adminTitle: "Editor and admin login",
      adminBody: "Use email and password to create your editor account or sign into the account that will become admin.",
      createEditor: "Create editor account",
      loginAdmin: "Sign in as admin",
      activeTitle: "Connected account",
      activeBody: "Your session is ready. Go to an entry page to comment or open Admin if this account already has permission.",
      name: "Name",
      email: "Email",
      password: "Password",
      signOut: "Sign out",
      memberRole: "Member",
      adminRole: "Admin"
    },
    admin: {
      title: "Admin Workspace",
      body: "A more professional workspace for editing content, publishing entries, uploading images and moderating comments.",
      accessTitle: "Restricted access",
      accessBody: "Sign in with an admin account to unlock the editing workspace.",
      setupTitle: "Publishing flow",
      setupBody: "Create your account in Feedback, promote it to admin in SQL and then come back here to publish and edit.",
      browserTitle: "Entry browser",
      browserSearch: "Search entry by title or slug",
      browserFilter: "Filter category",
      browserAll: "All categories",
      browserNew: "New entry",
      browserResults: "results",
      browserPublished: "published",
      browserDrafts: "drafts",
      browserEmpty: "No entries matched the current filters.",
      browserLoad: "Load",
      editorTitle: "Entry editor",
      workspaceTitle: "Publishing workspace",
      workspaceBody: "Edit metadata, main content, image and relationships without mixing everything into a public-facing page.",
      identityTitle: "Identity and publishing",
      identityBody: "Set slug, category, order, related entries and the publication state.",
      mediaTitle: "Primary media",
      mediaBody: "Choose the main page image and upload assets without leaving the workspace.",
      discoveryTitle: "Vanilla wiki and sprite fallback",
      discoveryBody: "Use alias and wiki source when the name does not match by itself. Fallback image is used when there is no local sprite yet.",
      metadataTitle: "Metadata",
      contentTitle: "Page content",
      previewTitle: "Quick preview",
      previewEmpty: "Fill title, summary and image to get a better preview before publishing.",
      openEntry: "Open page",
      openLibrary: "Open library",
      uploadTitle: "Assets",
      moderationTitle: "Comment moderation",
      save: "Save entry",
      upload: "Upload image",
      noComments: "No recent comments to moderate.",
      fields: {
        slug: "Slug",
        category: "Category",
        order: "Order",
        imageUrl: "Primary image",
        fallbackImage: "Fallback image URL",
        vanillaAlias: "Vanilla alias",
        wikiSource: "Wiki source",
        imageFolder: "Image folder",
        imageFile: "File",
        related: "Related (slug, slug)",
        title: "Title",
        subtitle: "Subtitle",
        summary: "Summary",
        overview: "Overview",
        facts: "Facts (1 per line)",
        obtain: "How to obtain (1 per line)",
        crafting: "Crafting (1 per line)",
        drops: "Drops (1 per line)",
        pieces: "Pieces (1 per line)",
        notes: "Notes (1 per line)",
        tactics: "Tactics (1 per line)",
        published: "Published"
      },
      hints: {
        imageUrl: "Primary image used by the entry page and library card.",
        fallbackImage: "Reserve image when the entry does not have its own sprite yet.",
        vanillaAlias: "Exact vanilla Terraria item name used for automatic sprite lookup.",
        wikiSource: "Terraria Wiki page title or full URL to force a specific source.",
        related: "Use comma-separated slugs to connect summons, sets, drops or sibling pages.",
        crafting: "One recipe per line. The parser detects ingredients, amounts and workstation automatically."
      },
      moderationHide: "Hide",
      moderationShow: "Show"
    },
    common: {
      openPage: "Open page",
      liveSite: "Pages",
      releases: "Releases",
      github: "GitHub",
      noData: "Nothing to show here yet.",
      notConfigured: "Supabase is not configured in this build yet.",
      connectedAs: "Connected as"
    }
  }
};

pageCopy.es = pageCopy.en;
pageCopy.ru = pageCopy.en;

let allEntries = [];
let orderedCategories = [];
let craftableEntries = [];
const externalAssetState = {
  cache: buildInitialExternalAssetCache()
};

const state = {
  language: siteConfig.defaultLanguage,
  search: "",
  category: "all",
  entryId: siteConfig.defaultEntryId,
  adminSearch: "",
  adminCategory: "all",
  adminDraft: null
};

hydrateStateFromUrl();

setBackendListener(() => {
  refreshEntryCache();
  render();
});

bootstrap();

async function bootstrap() {
  refreshEntryCache();
  await initBackend();
  refreshEntryCache();

  if (pageId === "entry") {
    await ensureCurrentEntryComments();
  }

  if (pageId === "admin" && backendState.isAdmin) {
    await loadAdminComments();
  }

  render();
}

function hydrateStateFromUrl() {
  const url = new URL(window.location.href);
  const language = url.searchParams.get("lang");
  const search = url.searchParams.get("q");
  const category = url.searchParams.get("category");
  const entryId = url.searchParams.get("entry");
  const editId = url.searchParams.get("edit");

  if (languageOptions.some((option) => option.code === language)) {
    state.language = language;
  }

  if (search) {
    state.search = search;
  }

  if (category) {
    state.category = category;
  }

  if (entryId) {
    state.entryId = entryId;
  }

  if (editId) {
    state.entryId = editId;
  }
}

function render() {
  renderHeader();
  renderPage();
  renderFooter();
  syncMetadata();
  syncUrl();
}

function renderHeader() {
  const copy = getCopy();
  const accountMarkup = backendState.enabled
    ? backendState.user
      ? `
        <div class="account-group">
          <span class="account-chip">${escapeHtml(backendState.profile?.display_name ?? "User")}</span>
          <button class="header-link" type="button" id="header-sign-out">${copy.header.signOut}</button>
        </div>
      `
      : `<a class="header-link header-link--button" href="${buildPageUrl("feedback")}">${copy.header.account}</a>`
    : `<span class="account-chip account-chip--subtle">${copy.header.static}</span>`;

  const navMarkup = Object.entries(copy.nav).map(([key, label]) => `
    <a class="header-link ${pageId === key ? "is-active" : ""}" href="${buildPageUrl(key)}">${label}</a>
  `).join("");

  const languageMarkup = languageOptions.map((option) => `
    <button class="language-pill ${option.code === state.language ? "is-active" : ""}" type="button" data-language="${option.code}">
      ${escapeHtml(option.label)}
    </button>
  `).join("");

  elements.header.innerHTML = `
    <div class="header-brand">
      <a class="brand-link" href="${buildPageUrl("home")}">
        <img src="./assets/images/icon.png" alt="Chaotic Dimensions icon">
        <div>
          <strong>${escapeHtml(getSiteName())}</strong>
          <span>${copy.siteLabel}</span>
        </div>
      </a>
    </div>

    <div class="header-right">
      <nav class="main-nav">${navMarkup}</nav>
      <div class="header-controls">
        <div class="language-group">${languageMarkup}</div>
        ${accountMarkup}
      </div>
    </div>
  `;

  elements.header.querySelectorAll("[data-language]").forEach((button) => {
    button.addEventListener("click", () => {
      state.language = button.dataset.language;
      render();
    });
  });

  elements.header.querySelector("#header-sign-out")?.addEventListener("click", async () => {
    await signOutUser();
    render();
  });
}

function renderPage() {
  switch (pageId) {
    case "library":
      renderLibraryPage();
      return;
    case "entry":
      renderEntryPage();
      return;
    case "crafting":
      renderCraftingPage();
      return;
    case "progression":
      renderProgressionPage();
      return;
    case "feedback":
      renderFeedbackPage();
      return;
    case "admin":
      renderAdminPage();
      return;
    default:
      renderHomePage();
  }
}

function renderHomePage() {
  const copy = getCopy();
  const featuredEntries = getHomeFeaturedEntries().map((entry) => renderEntryCard(entry, true)).join("");
  const categoryMarkup = orderedCategories.map((category) => `
    <article class="summary-tile">
      <div>
        <strong>${getCategoryLabel(category)}</strong>
        <p>${getEntriesByCategory(category).length}</p>
      </div>
      <a class="inline-link" href="${buildPageUrl("library", { category })}">${copy.common.openPage}</a>
    </article>
  `).join("");

  const pageLinks = copy.home.links.map((item) => `
    <article class="feature-tile">
      <h3>${item.title}</h3>
      <p>${item.body}</p>
      <a class="inline-link" href="${buildPageUrl(item.page)}">${copy.common.openPage}</a>
    </article>
  `).join("");

  elements.main.innerHTML = `
    <section class="page-section page-section--portal">
      <div class="section-head">
        <div>
          <p class="eyebrow">${copy.siteLabel}</p>
          <h1>${copy.home.title}</h1>
          <p>${copy.home.lead}</p>
        </div>
        <div class="section-actions">
          <a class="header-link header-link--button" href="${siteConfig.pagesUrl}" target="_blank" rel="noreferrer">${copy.common.liveSite}</a>
          <a class="header-link header-link--button" href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">${copy.common.github}</a>
        </div>
      </div>
      <div class="section-head">
        <div>
          <h2>${copy.home.introTitle}</h2>
          <p>${copy.home.introBody}</p>
        </div>
      </div>
      <div class="feature-grid">${pageLinks}</div>
    </section>

    <section class="page-section">
      <div class="section-head">
        <div>
          <h2>${copy.home.categoryTitle}</h2>
          <p>${copy.home.categoryBody}</p>
        </div>
      </div>
      <div class="summary-grid">${categoryMarkup}</div>
    </section>

    <section class="page-section">
      <div class="section-head">
        <div>
          <h2>${copy.home.featuredTitle}</h2>
          <p>${copy.home.featuredBody}</p>
        </div>
      </div>
      <div class="entry-grid">${featuredEntries}</div>
    </section>
  `;
}

function renderLibraryPage() {
  const copy = getCopy();
  const visibleEntries = getVisibleEntries();
  const categoryButtons = [
    {
      key: "all",
      label: copy.library.all,
      count: allEntries.length
    },
    ...orderedCategories.map((category) => ({
      key: category,
      label: getCategoryLabel(category),
      count: getEntriesByCategory(category).length
    }))
  ].map((item) => `
    <button class="catalog-chip ${state.category === item.key || (item.key === "all" && state.category === "all") ? "is-active" : ""}" type="button" data-library-quick-category="${item.key}">
      <span>${item.label}</span>
      <strong>${item.count}</strong>
    </button>
  `).join("");

  elements.main.innerHTML = `
    <section class="page-hero page-hero--compact">
      <p class="eyebrow">${copy.nav.library}</p>
      <h1>${copy.library.title}</h1>
      <p class="hero-lead">${copy.library.body}</p>
    </section>

    <section class="page-section">
      <div class="catalog-layout">
        <aside class="catalog-sidebar">
          <div class="content-block content-block--compact">
            <h3>${copy.library.title}</h3>
            <p>${copy.library.body}</p>
          </div>
          <div class="content-block content-block--compact">
            <label class="field-group">
              <span>${copy.library.search}</span>
              <input class="field-input" id="library-search" type="search" value="${escapeHtml(state.search)}" placeholder="${copy.library.search}">
            </label>
            <label class="field-group">
              <span>${copy.library.category}</span>
              <select class="field-input" id="library-category">
                <option value="all">${copy.library.all}</option>
                ${orderedCategories.map((category) => `
                  <option value="${category}" ${state.category === category ? "selected" : ""}>${getCategoryLabel(category)}</option>
                `).join("")}
              </select>
            </label>
          </div>
          <div class="content-block content-block--compact">
            <h3>${copy.library.category}</h3>
            <div class="catalog-chip-list">${categoryButtons}</div>
          </div>
        </aside>
        <div class="catalog-results">
          <div class="section-head section-head--inline">
            <h2>${copy.library.results}</h2>
            <span class="subtle-label">${visibleEntries.length}</span>
          </div>
          <div class="entry-grid">
            ${visibleEntries.length > 0 ? visibleEntries.map((entry) => renderEntryCard(entry, true)).join("") : `<div class="empty-card">${copy.library.empty}</div>`}
          </div>
        </div>
      </div>
    </section>
  `;

  elements.main.querySelector("#library-search")?.addEventListener("input", (event) => {
    state.search = event.target.value;
    renderLibraryPage();
    syncUrl();
  });

  elements.main.querySelector("#library-category")?.addEventListener("change", (event) => {
    state.category = event.target.value;
    renderLibraryPage();
    syncUrl();
  });

  elements.main.querySelectorAll("[data-library-quick-category]")?.forEach((button) => {
    button.addEventListener("click", () => {
      state.category = button.dataset.libraryQuickCategory ?? "all";
      renderLibraryPage();
      syncUrl();
    });
  });
}

function renderEntryPage() {
  const copy = getCopy();
  const entry = getEntryById(state.entryId);

  if (!entry) {
    elements.main.innerHTML = renderNotFound(copy.entry.notFoundTitle, copy.entry.notFoundBody, buildPageUrl("library"), copy.entry.back);
    return;
  }

  const content = getLocalizedEntry(entry);
  const entryAsset = getEntryDisplayAsset(entry);
  const bannerImage = entry.banner || "";
  const recipeUrl = hasContentList(entry, "crafting") ? buildPageUrl("crafting", { q: content.title }) : "";
  const summonEntry = findSummonEntry(entry);
  const summonContent = getLocalizedEntry(summonEntry);
  const usedInEntries = getUsedInEntries(entry).slice(0, 12);
  const relatedMarkup = (entry.related ?? []).map((entryId) => {
    const relatedEntry = getEntryById(entryId);
    if (!relatedEntry) {
      return "";
    }
    const relatedContent = getLocalizedEntry(relatedEntry);
    return `<a class="inline-tag" href="${buildPageUrl("entry", { entry: relatedEntry.id })}">${escapeHtml(relatedContent.title ?? relatedEntry.id)}</a>`;
  }).join("");
  const usedInMarkup = usedInEntries.map((usedInEntry) => {
    const usedInContent = getLocalizedEntry(usedInEntry);
    const usedInAsset = getEntryDisplayAsset(usedInEntry);
    return `
      <a class="entry-inline-row" href="${buildPageUrl("entry", { entry: usedInEntry.id })}">
        <img class="entry-inline-row__image" src="${escapeHtml(usedInAsset.imageUrl)}" alt="${escapeHtml(usedInContent.title ?? usedInEntry.id)}">
        <span>${escapeHtml(usedInContent.title ?? usedInEntry.id)}</span>
      </a>
    `;
  }).join("");

  const thread = backendState.commentsByEntry[entry.id];
  const commentsMarkup = thread?.loading
    ? `<div class="empty-card">Loading comments...</div>`
    : thread?.items?.length
      ? thread.items.map((comment) => `
        <article class="comment-card">
          <div class="comment-head">
            <strong>${escapeHtml(comment.display_name)}</strong>
            <span>${escapeHtml(formatDateTime(comment.created_at))}</span>
          </div>
          <p>${formatMultilineText(comment.body)}</p>
        </article>
      `).join("")
      : `<div class="empty-card">${copy.entry.commentsEmpty}</div>`;

  elements.main.innerHTML = `
    <section class="page-section page-section--entry-top">
      <a class="inline-link" href="${buildPageUrl("library")}">${copy.entry.back}</a>
      <div class="entry-hero">
        <div class="entry-main">
          ${bannerImage ? `
            <div class="entry-banner-art">
              <img src="${escapeHtml(bannerImage)}" alt="${escapeHtml(content.title ?? entry.id)}">
            </div>
          ` : ""}
          <p class="eyebrow">${getCategoryLabel(entry.category)}</p>
          <h1>${escapeHtml(content.title ?? entry.id)}</h1>
          <p class="entry-subtitle">${escapeHtml(content.subtitle ?? "")}</p>
          <p class="hero-lead">${escapeHtml(content.overview ?? content.summary ?? "")}</p>
          <div class="entry-inline-links">
            ${recipeUrl ? `<a class="inline-link" href="${recipeUrl}">${copy.entry.openCrafting}</a>` : ""}
            <a class="inline-link" href="${buildPageUrl("feedback")}">${copy.entry.comments}</a>
          </div>
        </div>
        <aside class="entry-aside">
          <div class="aside-card">
            <img class="entry-image entry-image--showcase" src="${escapeHtml(entryAsset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
            <div class="meta-stack">
              <span class="inline-tag">${getCategoryLabel(entry.category)}</span>
              <span class="inline-tag inline-tag--subtle">${escapeHtml(getTagLabel(entry))}</span>
            </div>
            ${summonEntry ? `
              <div class="content-block content-block--compact">
                <h3>${copy.entry.summon}</h3>
                <a class="entry-title-link" href="${buildPageUrl("entry", { entry: summonEntry.id })}">${escapeHtml(summonContent.title ?? summonEntry.id)}</a>
                ${summonContent.crafting?.length ? `<ul class="content-list">${summonContent.crafting.slice(0, 4).map((item) => `<li>${escapeHtml(item)}</li>`).join("")}</ul>` : ""}
              </div>
            ` : ""}
            ${(content.facts ?? []).length > 0 ? `
              <div class="content-block content-block--compact">
                <h3>${copy.entry.facts}</h3>
                <ul class="content-list">${(content.facts ?? []).map((item) => `<li>${escapeHtml(item)}</li>`).join("")}</ul>
              </div>
            ` : ""}
          </div>
        </aside>
      </div>
    </section>

    <section class="page-section">
      <div class="content-grid">
        ${renderContentBlock(copy.entry.obtain, content.obtain)}
        ${renderCraftingContentBlock(copy.entry.crafting, entry)}
        ${renderContentBlock(copy.entry.drops, content.drops)}
        ${renderContentBlock(copy.entry.pieces, content.pieces)}
        ${renderContentBlock(copy.entry.notes, content.notes)}
        ${renderContentBlock(copy.entry.tactics, content.tactics)}
      </div>
      ${usedInMarkup ? `
        <div class="content-block">
          <h3>${copy.entry.usedIn}</h3>
          <div class="entry-inline-list">${usedInMarkup}</div>
        </div>
      ` : ""}
      ${relatedMarkup ? `
        <div class="content-block">
          <h3>${copy.entry.related}</h3>
          <div class="tag-row">${relatedMarkup}</div>
        </div>
      ` : ""}
    </section>

    <section class="page-section">
      <div class="section-head">
        <div>
          <h2>${copy.entry.comments}</h2>
          <p>${backendState.user ? `${getCopy().common.connectedAs} ${escapeHtml(backendState.profile?.display_name ?? "user")}.` : copy.entry.commentsHint}</p>
        </div>
      </div>

      <div class="comment-stack">${commentsMarkup}</div>

      ${backendState.enabled && backendState.user ? `
        <form class="editor-form" id="comment-form">
          <label class="field-group">
            <span>${copy.entry.comments}</span>
            <textarea class="field-input field-input--textarea" name="body" rows="4" placeholder="${copy.entry.commentPlaceholder}" required></textarea>
          </label>
          <button class="action-button" type="submit">${copy.entry.commentButton}</button>
        </form>
      ` : ""}

      ${renderStatusMessage(backendState.commentError || backendState.commentMessage, Boolean(backendState.commentError))}
    </section>
  `;

  elements.main.querySelector("#comment-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const ok = await postComment(entry.id, formData.get("body"));
    if (ok) {
      event.currentTarget.reset();
    }
    await ensureCurrentEntryComments(true);
    renderEntryPage();
  });
}

function renderCraftingPage() {
  const copy = getCopy();
  const visibleRecipes = getVisibleRecipes();
  const recipeCategories = [...new Set(craftableEntries.map((entry) => entry.category))];
  const categoryButtons = [
    {
      key: "all",
      label: copy.crafting.allTags,
      count: craftableEntries.length
    },
    ...recipeCategories.map((category) => ({
      key: category,
      label: getCategoryLabel(category),
      count: craftableEntries.filter((entry) => entry.category === category).length
    }))
  ].map((item) => `
    <button class="catalog-chip ${state.category === item.key || (item.key === "all" && state.category === "all") ? "is-active" : ""}" type="button" data-crafting-quick-category="${item.key}">
      <span>${item.label}</span>
      <strong>${item.count}</strong>
    </button>
  `).join("");

  elements.main.innerHTML = `
    <section class="page-hero page-hero--compact">
      <p class="eyebrow">${copy.nav.crafting}</p>
      <h1>${copy.crafting.title}</h1>
      <p class="hero-lead">${copy.crafting.body}</p>
    </section>

    <section class="page-section">
      <div class="catalog-layout">
        <aside class="catalog-sidebar">
          <div class="content-block content-block--compact">
            <h3>${copy.crafting.title}</h3>
            <p>${copy.crafting.body}</p>
          </div>
          <div class="content-block content-block--compact">
            <label class="field-group">
              <span>${copy.crafting.search}</span>
              <input class="field-input" id="crafting-search" type="search" value="${escapeHtml(state.search)}" placeholder="${copy.crafting.search}">
            </label>
            <label class="field-group">
              <span>${copy.library.category}</span>
              <select class="field-input" id="crafting-category">
                <option value="all">${copy.crafting.allTags}</option>
                ${recipeCategories.map((category) => `
                  <option value="${category}" ${state.category === category ? "selected" : ""}>${getCategoryLabel(category)}</option>
                `).join("")}
              </select>
            </label>
          </div>
          <div class="content-block content-block--compact">
            <h3>${copy.crafting.allTags}</h3>
            <div class="catalog-chip-list">${categoryButtons}</div>
          </div>
        </aside>
        <div class="catalog-results">
          <div class="section-head section-head--inline">
            <h2>${copy.library.results}</h2>
            <span class="subtle-label">${visibleRecipes.length}</span>
          </div>
          <div class="recipe-list">
            ${visibleRecipes.length > 0 ? visibleRecipes.map((entry) => renderRecipeCard(entry)).join("") : `<div class="empty-card">${copy.crafting.empty}</div>`}
          </div>
        </div>
      </div>
    </section>
  `;

  elements.main.querySelector("#crafting-search")?.addEventListener("input", (event) => {
    state.search = event.target.value;
    renderCraftingPage();
    syncUrl();
  });

  elements.main.querySelector("#crafting-category")?.addEventListener("change", (event) => {
    state.category = event.target.value;
    renderCraftingPage();
    syncUrl();
  });

  elements.main.querySelectorAll("[data-crafting-quick-category]")?.forEach((button) => {
    button.addEventListener("click", () => {
      state.category = button.dataset.craftingQuickCategory ?? "all";
      renderCraftingPage();
      syncUrl();
    });
  });
}

function renderProgressionPage() {
  const copy = getCopy();
  const groupMarkup = PROGRESSION_GROUPS.map((group) => {
    const entriesForGroup = getProgressionEntries(group.key);
    if (entriesForGroup.length === 0) {
      return "";
    }

    return `
      <section class="page-section progression-group">
        <div class="section-head">
          <div>
            <h2>${copy.progression.groups[group.key].title}</h2>
            <p>${copy.progression.groups[group.key].body}</p>
          </div>
        </div>
        <div class="progression-stack progression-stack--grouped">
          ${entriesForGroup.map((entry) => renderProgressionCard(entry)).join("")}
        </div>
      </section>
    `;
  }).join("");

  elements.main.innerHTML = `
    <section class="page-hero page-hero--compact">
      <p class="eyebrow">${copy.nav.progression}</p>
      <h1>${copy.progression.title}</h1>
      <p class="hero-lead">${copy.progression.body}</p>
    </section>

    ${groupMarkup}
  `;
}

function renderFeedbackPage() {
  const copy = getCopy();

  if (!backendState.enabled) {
    elements.main.innerHTML = `
      <section class="page-hero page-hero--compact">
        <p class="eyebrow">${copy.nav.feedback}</p>
        <h1>${copy.feedback.title}</h1>
        <p class="hero-lead">${copy.feedback.body}</p>
      </section>
      <section class="page-section">
        <div class="empty-card">${copy.common.notConfigured}</div>
      </section>
    `;
    return;
  }

  if (backendState.user) {
    elements.main.innerHTML = `
      <section class="page-hero page-hero--compact">
        <p class="eyebrow">${copy.nav.feedback}</p>
        <h1>${copy.feedback.title}</h1>
        <p class="hero-lead">${copy.feedback.body}</p>
      </section>

      <section class="page-section">
        <div class="feedback-grid">
          <article class="panel-card">
            <h2>${copy.feedback.activeTitle}</h2>
            <p>${copy.feedback.activeBody}</p>
            <div class="tag-row">
              <span class="inline-tag">${escapeHtml(backendState.profile?.display_name ?? "User")}</span>
              <span class="inline-tag inline-tag--subtle">${backendState.isAdmin ? copy.feedback.adminRole : copy.feedback.memberRole}</span>
            </div>
            ${renderStatusMessage(backendState.authError || backendState.authMessage, Boolean(backendState.authError))}
            <div class="button-row">
              <a class="header-link header-link--button" href="${buildPageUrl("library")}">${copy.nav.library}</a>
              ${backendState.isAdmin ? `<a class="header-link header-link--button" href="${buildPageUrl("admin")}">${copy.nav.admin}</a>` : ""}
              <button class="action-button action-button--secondary" type="button" id="feedback-sign-out">${copy.feedback.signOut}</button>
            </div>
          </article>
        </div>
      </section>
    `;

    elements.main.querySelector("#feedback-sign-out")?.addEventListener("click", async () => {
      await signOutUser();
      renderFeedbackPage();
    });
    return;
  }

  elements.main.innerHTML = `
    <section class="page-hero page-hero--compact">
      <p class="eyebrow">${copy.nav.feedback}</p>
      <h1>${copy.feedback.title}</h1>
      <p class="hero-lead">${copy.feedback.body}</p>
    </section>

    <section class="page-section">
      <div class="feedback-grid">
        <article class="panel-card">
          <h2>${copy.feedback.guestTitle}</h2>
          <p>${copy.feedback.guestBody}</p>
          <form class="editor-form" id="guest-form">
            <label class="field-group">
              <span>${copy.feedback.name}</span>
              <input class="field-input" type="text" name="displayName" required>
            </label>
            <button class="action-button" type="submit">${copy.feedback.guestButton}</button>
          </form>
        </article>

        <article class="panel-card">
          <h2>${copy.feedback.adminTitle}</h2>
          <p>${copy.feedback.adminBody}</p>
          <form class="editor-form" id="editor-form">
            <label class="field-group">
              <span>${copy.feedback.name}</span>
              <input class="field-input" type="text" name="displayName">
            </label>
            <label class="field-group">
              <span>${copy.feedback.email}</span>
              <input class="field-input" type="email" name="email" required>
            </label>
            <label class="field-group">
              <span>${copy.feedback.password}</span>
              <input class="field-input" type="password" name="password" required>
            </label>
            <div class="button-row">
              <button class="action-button action-button--secondary" type="submit" name="intent" value="create">${copy.feedback.createEditor}</button>
              <button class="action-button" type="submit" name="intent" value="login">${copy.feedback.loginAdmin}</button>
            </div>
          </form>
        </article>
      </div>

      ${renderStatusMessage(backendState.authError || backendState.authMessage, Boolean(backendState.authError))}
    </section>
  `;

  elements.main.querySelector("#guest-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    await signInGuest(formData.get("displayName"));
    renderFeedbackPage();
  });

  elements.main.querySelector("#editor-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const submitter = event.submitter;
    const email = formData.get("email");
    const password = formData.get("password");
    const displayName = formData.get("displayName");

    if (submitter?.value === "create") {
      await registerEditorAccount(displayName, email, password);
    }
    else {
      await signInAdmin(email, password);
    }

    renderFeedbackPage();
  });
}

function getDraftPreviewAsset(draft) {
  const baseEntry = draft.id ? getEntryById(draft.id) : null;
  const baseContent = baseEntry?.content ?? {};
  const previewContent = {
    ...baseContent,
    _meta: {
      ...(baseContent._meta ?? {}),
      vanillaAlias: draft.vanillaAlias,
      wikiSource: draft.wikiSource,
      fallbackImage: draft.fallbackImage
    },
    [state.language]: {
      ...(getLocalizedEntry(baseEntry) ?? {}),
      title: draft.title || getLocalizedEntry(baseEntry)?.title || "",
      subtitle: draft.subtitle || getLocalizedEntry(baseEntry)?.subtitle || "",
      summary: draft.summary || getLocalizedEntry(baseEntry)?.summary || "",
      overview: draft.overview || getLocalizedEntry(baseEntry)?.overview || ""
    }
  };

  return getEntryDisplayAsset({
    id: draft.id || baseEntry?.id || "preview",
    image: draft.imageUrl || baseEntry?.image || DEFAULT_ENTRY_IMAGE,
    content: previewContent
  });
}

function renderAdminBrowserEntry(entry, activeEntryId, copy) {
  const content = getLocalizedEntry(entry);
  const asset = getEntryDisplayAsset(entry, { ensure: false });

  return `
    <button class="admin-entry-button ${activeEntryId === entry.id ? "is-active" : ""}" type="button" data-admin-entry="${entry.id}">
      <img class="admin-entry-button__thumb" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
      <span class="admin-entry-button__copy">
        <strong>${escapeHtml(content.title ?? entry.id)}</strong>
        <small>${escapeHtml(entry.id)}</small>
      </span>
      <span class="admin-entry-button__meta">
        <span class="inline-tag inline-tag--subtle">${getCategoryLabel(entry.category)}</span>
        <span class="inline-tag inline-tag--subtle">${entry.isPublished ? copy.admin.fields.published : copy.admin.browserDrafts}</span>
      </span>
    </button>
  `;
}

function renderAdminPage() {
  const copy = getCopy();

  if (!backendState.enabled) {
    elements.main.innerHTML = `
      <section class="page-hero page-hero--compact">
        <p class="eyebrow">${copy.nav.admin}</p>
        <h1>${copy.admin.title}</h1>
        <p class="hero-lead">${copy.admin.body}</p>
      </section>
      <section class="page-section">
        <div class="empty-card">${copy.common.notConfigured}</div>
      </section>
    `;
    return;
  }

  if (!backendState.isAdmin) {
    elements.main.innerHTML = `
      <section class="page-hero page-hero--compact">
        <p class="eyebrow">${copy.nav.admin}</p>
        <h1>${copy.admin.title}</h1>
        <p class="hero-lead">${copy.admin.body}</p>
      </section>
      <section class="page-section">
        <div class="feedback-grid">
          <article class="panel-card">
            <h2>${copy.admin.accessTitle}</h2>
            <p>${copy.admin.accessBody}</p>
            <a class="header-link header-link--button" href="${buildPageUrl("feedback")}">${copy.nav.feedback}</a>
          </article>
          <article class="panel-card">
            <h2>${copy.admin.setupTitle}</h2>
            <p>${copy.admin.setupBody}</p>
          </article>
        </div>
      </section>
    `;
    return;
  }

  const adminEntries = getAdminBrowserEntries();
  const draft = ensureAdminDraft();
  const previewSummary = draft.summary || draft.overview || copy.admin.previewEmpty;
  const previewHref = draft.id ? buildPageUrl("entry", { entry: draft.id }) : "";
  const categoryOptions = [...new Set([...CATEGORY_ORDER, ...orderedCategories, draft.category].filter(Boolean))];
  const browserCategoryOptions = ["all", ...new Set([...CATEGORY_ORDER, ...orderedCategories].filter(Boolean))];
  const previewAsset = getDraftPreviewAsset(draft);
  const totalEntries = allEntries.length;
  const publishedEntries = allEntries.filter((entry) => entry.isPublished).length;
  const draftEntries = Math.max(totalEntries - publishedEntries, 0);

  elements.main.innerHTML = `
    <section class="page-hero page-hero--compact">
      <p class="eyebrow">${copy.nav.admin}</p>
      <h1>${copy.admin.title}</h1>
      <p class="hero-lead">${copy.admin.body}</p>
    </section>

    <section class="page-section">
      <div class="admin-layout">
        <aside class="admin-sidebar">
          <article class="panel-card admin-browser-card">
            <div class="section-head section-head--inline">
              <div>
                <h2>${copy.admin.browserTitle}</h2>
                <p>${adminEntries.length} ${copy.admin.browserResults}</p>
              </div>
              <button class="action-button action-button--secondary" type="button" id="admin-new-entry">${copy.admin.browserNew}</button>
            </div>
            <div class="admin-browser-stats">
              <article class="summary-tile admin-stat-tile">
                <strong>${totalEntries}</strong>
                <span>${copy.admin.browserResults}</span>
              </article>
              <article class="summary-tile admin-stat-tile">
                <strong>${publishedEntries}</strong>
                <span>${copy.admin.browserPublished}</span>
              </article>
              <article class="summary-tile admin-stat-tile">
                <strong>${draftEntries}</strong>
                <span>${copy.admin.browserDrafts}</span>
              </article>
            </div>
            <div class="admin-filter-grid">
              <input class="field-input" id="admin-search" type="search" value="${escapeHtml(state.adminSearch)}" placeholder="${copy.admin.browserSearch}">
              <label class="field-group">
                <span>${copy.admin.browserFilter}</span>
                <select class="field-input" id="admin-category-filter">
                  ${browserCategoryOptions.map((option) => `
                    <option value="${escapeHtml(option)}" ${state.adminCategory === option ? "selected" : ""}>${escapeHtml(option === "all" ? copy.admin.browserAll : getCategoryLabel(option))}</option>
                  `).join("")}
                </select>
              </label>
            </div>
            <div class="admin-entry-list">
              ${adminEntries.length > 0 ? adminEntries.map((entry) => renderAdminBrowserEntry(entry, draft.id, copy)).join("") : `<div class="empty-card">${copy.admin.browserEmpty}</div>`}
            </div>
          </article>
        </aside>

        <div class="admin-main">
          <article class="panel-card">
            <div class="section-head">
              <div>
                <h2>${copy.admin.workspaceTitle}</h2>
                <p>${copy.admin.workspaceBody}</p>
              </div>
              <div class="button-row">
                ${previewHref ? `<a class="header-link header-link--button" href="${previewHref}">${copy.admin.openEntry}</a>` : ""}
                <a class="header-link header-link--button" href="${buildPageUrl("library")}">${copy.admin.openLibrary}</a>
              </div>
            </div>

            <div class="admin-workspace-grid">
              <div class="admin-rail">
                <article class="panel-card panel-card--nested admin-preview">
                  <h3>${copy.admin.previewTitle}</h3>
                  <img class="entry-image entry-image--large" src="${escapeHtml(previewAsset.imageUrl)}" alt="${escapeHtml(draft.title || draft.id || "Preview")}">
                  <div class="tag-row">
                    <span class="inline-tag">${getCategoryLabel(draft.category || "materials")}</span>
                    ${draft.id ? `<span class="inline-tag inline-tag--subtle">${escapeHtml(draft.id)}</span>` : ""}
                    <span class="inline-tag inline-tag--subtle">${draft.published ? copy.admin.fields.published : copy.admin.browserDrafts}</span>
                  </div>
                  <strong>${escapeHtml(draft.title || copy.admin.editorTitle)}</strong>
                  <p>${escapeHtml(previewSummary)}</p>
                </article>

                <article class="panel-card panel-card--nested">
                  <div class="section-head section-head--inline">
                    <div>
                      <h3>${copy.admin.mediaTitle}</h3>
                      <p>${copy.admin.mediaBody}</p>
                    </div>
                  </div>
                  <form class="editor-form" id="asset-form">
                    ${renderField(copy.admin.fields.imageFolder, "pathPrefix", draft.imagePath)}
                    <label class="field-group">
                      <span>${copy.admin.fields.imageFile}</span>
                      <input class="field-input" type="file" name="imageFile" accept="image/*" required>
                    </label>
                    <button class="action-button" type="submit">${copy.admin.upload}</button>
                  </form>
                  ${renderStatusMessage(backendState.uploadError || backendState.uploadMessage, Boolean(backendState.uploadError))}
                </article>
              </div>

              <form class="editor-form admin-editor-form" id="admin-editor-form">
                <div class="form-section">
                  <div class="section-head section-head--inline">
                    <div>
                      <h3>${copy.admin.identityTitle}</h3>
                      <p>${copy.admin.identityBody}</p>
                    </div>
                  </div>
                  <div class="field-grid field-grid--triple">
                    ${renderField(copy.admin.fields.slug, "id", draft.id)}
                    ${renderSelectField(copy.admin.fields.category, "category", draft.category, categoryOptions)}
                    ${renderField(copy.admin.fields.order, "sortOrder", draft.sortOrder, "number")}
                  </div>
                  <div class="field-grid field-grid--double">
                    ${renderField(copy.admin.fields.imageUrl, "imageUrl", draft.imageUrl, "url", { hint: copy.admin.hints.imageUrl })}
                    ${renderField(copy.admin.fields.related, "related", draft.related, "text", { hint: copy.admin.hints.related })}
                  </div>
                  <div class="checkbox-row">
                    <input type="checkbox" name="published" ${draft.published ? "checked" : ""}>
                    <span>${copy.admin.fields.published}</span>
                  </div>
                </div>

                <div class="form-section">
                  <div class="section-head section-head--inline">
                    <div>
                      <h3>${copy.admin.discoveryTitle}</h3>
                      <p>${copy.admin.discoveryBody}</p>
                    </div>
                  </div>
                  <div class="field-grid field-grid--double">
                    ${renderField(copy.admin.fields.fallbackImage, "fallbackImage", draft.fallbackImage, "url", { hint: copy.admin.hints.fallbackImage })}
                    ${renderField(copy.admin.fields.vanillaAlias, "vanillaAlias", draft.vanillaAlias, "text", { hint: copy.admin.hints.vanillaAlias })}
                  </div>
                  ${renderField(copy.admin.fields.wikiSource, "wikiSource", draft.wikiSource, "text", { hint: copy.admin.hints.wikiSource })}
                </div>

                <div class="form-section">
                  <div class="section-head section-head--inline">
                    <div>
                      <h3>${copy.admin.contentTitle}</h3>
                      <p>${copy.admin.workspaceBody}</p>
                    </div>
                  </div>
                  <div class="field-grid field-grid--double">
                    ${renderField(copy.admin.fields.title, "title", draft.title)}
                    ${renderField(copy.admin.fields.subtitle, "subtitle", draft.subtitle)}
                  </div>
                  ${renderTextarea(copy.admin.fields.summary, "summary", draft.summary, 3)}
                  ${renderTextarea(copy.admin.fields.overview, "overview", draft.overview, 5)}
                  <div class="field-grid field-grid--double">
                    ${renderTextarea(copy.admin.fields.facts, "facts", draft.facts, 6)}
                    ${renderTextarea(copy.admin.fields.obtain, "obtain", draft.obtain, 6)}
                  </div>
                  <div class="field-grid field-grid--double">
                    ${renderTextarea(copy.admin.fields.crafting, "crafting", draft.crafting, 6, { hint: copy.admin.hints.crafting })}
                    ${renderTextarea(copy.admin.fields.drops, "drops", draft.drops, 6)}
                  </div>
                  <div class="field-grid field-grid--double">
                    ${renderTextarea(copy.admin.fields.pieces, "pieces", draft.pieces, 5)}
                    ${renderTextarea(copy.admin.fields.notes, "notes", draft.notes, 5)}
                  </div>
                  ${renderTextarea(copy.admin.fields.tactics, "tactics", draft.tactics, 5)}
                </div>

                <div class="form-section">
                  <div class="button-row">
                    <button class="action-button" type="submit">${copy.admin.save}</button>
                    ${previewHref ? `<a class="header-link header-link--button" href="${previewHref}">${copy.admin.openEntry}</a>` : ""}
                  </div>
                </div>
              </form>
            </div>
            ${renderStatusMessage(backendState.entryError || backendState.entryMessage, Boolean(backendState.entryError))}
          </article>

          <div class="admin-subgrid admin-subgrid--single">
            <article class="panel-card">
              <h2>${copy.admin.moderationTitle}</h2>
              <div class="comment-stack">
                ${backendState.adminComments.length > 0 ? backendState.adminComments.map((comment) => `
                  <article class="comment-card">
                    <div class="comment-head">
                      <strong>${escapeHtml(comment.display_name)}</strong>
                      <span>${escapeHtml(comment.entry_id)}</span>
                    </div>
                    <p>${formatMultilineText(comment.body)}</p>
                    <div class="button-row">
                      <span class="subtle-label">${escapeHtml(formatDateTime(comment.created_at))}</span>
                      <button class="action-button action-button--secondary" type="button" data-toggle-comment="${comment.id}" data-comment-hidden="${comment.is_hidden}">
                        ${comment.is_hidden ? copy.admin.moderationShow : copy.admin.moderationHide}
                      </button>
                    </div>
                  </article>
                `).join("") : `<div class="empty-card">${copy.admin.noComments}</div>`}
              </div>
            </article>
          </div>
        </div>
      </div>
    </section>
  `;

  elements.main.querySelector("#admin-search")?.addEventListener("input", (event) => {
    state.adminSearch = event.target.value;
    renderAdminPage();
  });

  elements.main.querySelector("#admin-category-filter")?.addEventListener("change", (event) => {
    state.adminCategory = event.target.value;
    renderAdminPage();
  });

  elements.main.querySelector("#admin-new-entry")?.addEventListener("click", () => {
    clearAdminDraft();
    renderAdminPage();
  });

  elements.main.querySelectorAll("[data-admin-entry]").forEach((button) => {
    button.addEventListener("click", () => {
      loadEntryIntoDraft(button.dataset.adminEntry);
      renderAdminPage();
    });
  });

  elements.main.querySelector("#asset-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const pathPrefix = formData.get("pathPrefix");
    const url = await uploadWikiAsset(formData.get("imageFile"), pathPrefix);
    if (url) {
      ensureAdminDraft();
      state.adminDraft.imagePath = String(pathPrefix ?? "entries").trim() || "entries";
      state.adminDraft.imageUrl = url;
      renderAdminPage();
    }
  });

  elements.main.querySelector("#admin-editor-form")?.addEventListener("submit", async (event) => {
    event.preventDefault();
    const draftState = updateDraftFromForm(event.currentTarget);

    if (!draftState.id || !draftState.category || !draftState.title) {
      backendState.entryError = "Fill in slug, category and title before saving.";
      backendState.entryMessage = "";
      renderAdminPage();
      return;
    }

    const baseEntry = getEntryById(draftState.id);
    const payload = {
      id: draftState.id,
      category: draftState.category,
      image_url: draftState.imageUrl || baseEntry?.image || DEFAULT_ENTRY_IMAGE,
      related_ids: parseCsvList(draftState.related),
      sort_order: Number(draftState.sortOrder || 0),
      is_published: draftState.published,
      content_json: buildLocalizedContentPayload(baseEntry, draftState)
    };

    const ok = await saveWikiEntry(payload);
    if (ok) {
      state.entryId = draftState.id;
      refreshEntryCache();
      loadEntryIntoDraft(draftState.id);
      renderAdminPage();
    }
  });

  elements.main.querySelectorAll("[data-toggle-comment]").forEach((button) => {
    button.addEventListener("click", async () => {
      const commentId = button.dataset.toggleComment;
      const isHidden = button.dataset.commentHidden === "true";
      await setCommentHidden(commentId, !isHidden);
      renderAdminPage();
    });
  });
}

function renderFooter() {
  const copy = getCopy();

  elements.footer.innerHTML = `
    <div class="footer-row">
      <span>${escapeHtml(getSiteName())}</span>
      <div class="footer-links">
        <a href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">${copy.common.github}</a>
        <a href="${siteConfig.releasesUrl}" target="_blank" rel="noreferrer">${copy.common.releases}</a>
        <a href="${siteConfig.pagesUrl}" target="_blank" rel="noreferrer">${copy.common.liveSite}</a>
      </div>
    </div>
  `;
}

function renderEntryCard(entry, includeFacts = false) {
  const content = getLocalizedEntry(entry);
  const asset = getEntryDisplayAsset(entry);
  const facts = includeFacts ? (content.facts ?? []).slice(0, 2) : [];

  return `
    <article class="entry-card">
      <div class="entry-card-head">
        <a class="entry-card-thumb" href="${buildPageUrl("entry", { entry: entry.id })}">
          <img class="entry-card-image" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
        </a>
        <div class="entry-card-copy">
          <div class="tag-row">
            <span class="inline-tag">${getCategoryLabel(entry.category)}</span>
            <span class="inline-tag inline-tag--subtle">${escapeHtml(getTagLabel(entry))}</span>
          </div>
          <a class="entry-title-link" href="${buildPageUrl("entry", { entry: entry.id })}">
            <h3>${escapeHtml(content.title ?? entry.id)}</h3>
          </a>
        </div>
      </div>
      <p>${escapeHtml(content.summary ?? "")}</p>
      ${facts.length > 0 ? `<ul class="content-list">${facts.map((fact) => `<li>${escapeHtml(fact)}</li>`).join("")}</ul>` : ""}
    </article>
  `;
}

function renderRecipeCard(entry) {
  const copy = getCopy();
  const content = getLocalizedEntry(entry);
  const asset = getEntryDisplayAsset(entry);
  const recipe = parseRecipeModel(entry);

  return `
    <article class="recipe-card">
      <div class="entry-card-head">
        <a class="entry-card-thumb" href="${buildPageUrl("entry", { entry: entry.id })}">
          <img class="entry-card-image" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
        </a>
        <div class="entry-card-copy">
          <div class="tag-row">
            <span class="inline-tag">${getCategoryLabel(entry.category)}</span>
            <span class="inline-tag inline-tag--subtle">${escapeHtml(getTagLabel(entry))}</span>
          </div>
          <a class="entry-title-link" href="${buildPageUrl("entry", { entry: entry.id })}">
            <h3>${escapeHtml(content.title ?? entry.id)}</h3>
          </a>
        </div>
      </div>
      ${recipe.stations.length > 0 ? `
        <div class="recipe-stations">
          <strong>${copy.crafting.station}</strong>
          <div class="workstation-list">
            ${recipe.stations.map((station) => renderWorkstationPill(station)).join("")}
          </div>
        </div>
      ` : ""}
      <div class="recipe-meta">
        <div>
          <strong>${copy.crafting.ingredients}</strong>
          <div class="recipe-entry-list">
            ${recipe.ingredients.slice(0, 6).map((ingredient) => renderRecipeIngredient(ingredient)).join("")}
          </div>
        </div>
      </div>
    </article>
  `;
}

function renderCraftingContentBlock(label, entry) {
  const recipe = parseRecipeModel(entry);

  if (recipe.lines.length === 0) {
    return "";
  }

  return `
    <article class="content-block">
      <h3>${label}</h3>
      ${recipe.stations.length > 0 ? `
        <div class="recipe-stations">
          <strong>${getCopy().crafting.station}</strong>
          <div class="workstation-list">
            ${recipe.stations.map((station) => renderWorkstationPill(station)).join("")}
          </div>
        </div>
      ` : ""}
      <div class="recipe-entry-list recipe-entry-list--stacked">
        ${recipe.ingredients.length > 0
          ? recipe.ingredients.map((ingredient) => renderRecipeIngredient(ingredient)).join("")
          : recipe.lines.map((line) => `<div class="recipe-entry recipe-entry--plain">${escapeHtml(line)}</div>`).join("")}
      </div>
    </article>
  `;
}

function renderWorkstationPill(station) {
  const stationEntry = findEntryByMention(station.label);
  const stationAsset = stationEntry ? getEntryDisplayAsset(stationEntry) : null;
  const vanillaAsset = !stationEntry ? getExternalAsset(station.label) : null;
  const stationImage = stationAsset?.sourceType && stationAsset.sourceType !== "default"
    ? stationAsset.imageUrl
    : "";

  if (!stationEntry && !vanillaAsset) {
    ensureExternalAsset(station.label);
  }

  return `
    <${stationEntry || vanillaAsset ? "a" : "div"} class="workstation-pill" title="${escapeHtml(station.label)}" ${stationEntry
        ? `href="${buildPageUrl("entry", { entry: stationEntry.id })}"`
        : vanillaAsset
          ? `href="${escapeHtml(vanillaAsset.pageUrl)}" target="_blank" rel="noreferrer"`
          : ""}>
      ${stationEntry && stationImage
        ? `<img class="workstation-pill__image" src="${escapeHtml(stationImage)}" alt="${escapeHtml(station.label)}">`
        : vanillaAsset?.imageUrl
          ? `<img class="workstation-pill__image" src="${escapeHtml(vanillaAsset.imageUrl)}" alt="${escapeHtml(station.label)}">`
        : `<span class="workstation-pill__icon">${escapeHtml(station.short)}</span>`}
      <span>${escapeHtml(station.label)}</span>
    </${stationEntry || vanillaAsset ? "a" : "div"}>
  `;
}

function renderRecipeIngredient(ingredient) {
  const entryAsset = ingredient.entry ? getEntryDisplayAsset(ingredient.entry) : null;
  const vanillaAsset = !ingredient.entry ? getExternalAsset(ingredient.label) : null;
  if (!ingredient.entry && !vanillaAsset) {
    ensureExternalAsset(ingredient.label);
  }

  const image = entryAsset?.sourceType && entryAsset.sourceType !== "default"
    ? entryAsset.imageUrl
    : vanillaAsset?.imageUrl;
  const labelMarkup = ingredient.entry
    ? `<a class="entry-title-link" href="${buildPageUrl("entry", { entry: ingredient.entry.id })}">${escapeHtml(ingredient.label)}</a>`
    : vanillaAsset
      ? `<a class="entry-title-link" href="${escapeHtml(vanillaAsset.pageUrl)}" target="_blank" rel="noreferrer">${escapeHtml(ingredient.label)}</a>`
    : `<span>${escapeHtml(ingredient.label)}</span>`;

  return `
    <div class="recipe-entry">
      ${image ? `<img class="recipe-entry__image" src="${escapeHtml(image)}" alt="${escapeHtml(ingredient.label)}">` : `<span class="recipe-entry__fallback">${escapeHtml((ingredient.label || "?").slice(0, 2).toUpperCase())}</span>`}
      <div class="recipe-entry__body">
        ${labelMarkup}
        ${ingredient.amount ? `<small>x${escapeHtml(ingredient.amount)}</small>` : ""}
      </div>
    </div>
  `;
}

function renderProgressionCard(entry) {
  const copy = getCopy();
  const content = getLocalizedEntry(entry);
  const asset = getEntryDisplayAsset(entry);
  const summonEntry = findSummonEntry(entry);
  const summonContent = getLocalizedEntry(summonEntry);
  const summonLines = summonContent.crafting?.length ? summonContent.crafting.slice(0, 4) : (summonContent.obtain ?? []).slice(0, 3);
  const noteLines = (content.notes ?? []).slice(0, 3);

  return `
    <article class="progress-card progress-card--detailed">
      <img class="progress-image progress-image--large" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
      <div class="progress-copy">
        <div class="tag-row">
          <span class="inline-tag">${getCategoryLabel(entry.category)}</span>
          <span class="inline-tag inline-tag--subtle">${escapeHtml(getTagLabel(entry))}</span>
        </div>
        <a class="entry-title-link" href="${buildPageUrl("entry", { entry: entry.id })}">
          <h3>${escapeHtml(content.title ?? entry.id)}</h3>
        </a>
        <p>${escapeHtml(content.summary ?? content.overview ?? "")}</p>
        <div class="progress-detail-grid">
          ${summonEntry ? `
            <div class="progress-detail-block">
              <strong>${copy.progression.summon}</strong>
              <a class="inline-link" href="${buildPageUrl("entry", { entry: summonEntry.id })}">${escapeHtml(summonContent.title ?? summonEntry.id)}</a>
            </div>
          ` : ""}
          ${summonLines.length > 0 ? `
            <div class="progress-detail-block">
              <strong>${copy.progression.crafting}</strong>
              <ul class="content-list">${summonLines.map((line) => `<li>${escapeHtml(line)}</li>`).join("")}</ul>
            </div>
          ` : ""}
          ${noteLines.length > 0 ? `
            <div class="progress-detail-block">
              <strong>${copy.progression.curiosities}</strong>
              <ul class="content-list">${noteLines.map((line) => `<li>${escapeHtml(line)}</li>`).join("")}</ul>
            </div>
          ` : ""}
        </div>
      </div>
    </article>
  `;
}

function renderContentBlock(label, items) {
  if (!label || !items || items.length === 0) {
    return "";
  }

  return `
    <article class="content-block">
      <h3>${label}</h3>
      <ul class="content-list">${items.map((item) => `<li>${escapeHtml(item)}</li>`).join("")}</ul>
    </article>
  `;
}

function renderStatusMessage(message, isError) {
  if (!message) {
    return "";
  }

  return `
    <div class="status-message ${isError ? "is-error" : "is-success"}">
      ${escapeHtml(message)}
    </div>
  `;
}

function renderNotFound(title, body, href, label) {
  return `
    <section class="page-section">
      <div class="empty-card">
        <h1>${escapeHtml(title)}</h1>
        <p>${escapeHtml(body)}</p>
        <a class="inline-link" href="${href}">${escapeHtml(label)}</a>
      </div>
    </section>
  `;
}

function renderField(label, name, value, type = "text", options = {}) {
  const { placeholder = "", hint = "" } = options;
  return `
    <label class="field-group">
      <span>${label}</span>
      <input class="field-input" type="${type}" name="${name}" value="${escapeHtml(value ?? "")}" placeholder="${escapeHtml(placeholder)}">
      ${hint ? `<small class="field-hint">${escapeHtml(hint)}</small>` : ""}
    </label>
  `;
}

function renderSelectField(label, name, value, options, config = {}) {
  const { hint = "" } = config;
  return `
    <label class="field-group">
      <span>${label}</span>
      <select class="field-input" name="${name}">
        ${options.map((option) => `
          <option value="${escapeHtml(option)}" ${option === value ? "selected" : ""}>${escapeHtml(getCategoryLabel(option))}</option>
        `).join("")}
      </select>
      ${hint ? `<small class="field-hint">${escapeHtml(hint)}</small>` : ""}
    </label>
  `;
}

function renderTextarea(label, name, value, rows, options = {}) {
  const { placeholder = "", hint = "" } = options;
  return `
    <label class="field-group">
      <span>${label}</span>
      <textarea class="field-input field-input--textarea" name="${name}" rows="${rows}" placeholder="${escapeHtml(placeholder)}">${escapeHtml(value ?? "")}</textarea>
      ${hint ? `<small class="field-hint">${escapeHtml(hint)}</small>` : ""}
    </label>
  `;
}

function syncMetadata() {
  const copy = getCopy();
  document.documentElement.lang = state.language;

  const titleMap = {
    home: getSiteName(),
    library: copy.nav.library,
    entry: getLocalizedEntry(getEntryById(state.entryId))?.title ?? copy.nav.library,
    crafting: copy.nav.crafting,
    progression: copy.nav.progression,
    feedback: copy.nav.feedback,
    admin: copy.nav.admin
  };

  document.title = `${getSiteName()} | ${titleMap[pageId] ?? getSiteName()}`;
  elements.metaDescription?.setAttribute("content", `Chaotic Dimensions wiki - ${titleMap[pageId] ?? getSiteName()}.`);
}

function syncUrl() {
  const params = new URLSearchParams();
  params.set("lang", state.language);

  if (pageId === "library" || pageId === "crafting") {
    if (state.search.trim()) {
      params.set("q", state.search.trim());
    }
  }

  if ((pageId === "library" || pageId === "crafting") && state.category !== "all") {
    params.set("category", state.category);
  }

  if (pageId === "entry") {
    params.set("entry", state.entryId);
  }

  if (pageId === "admin" && state.adminDraft?.id) {
    params.set("edit", state.adminDraft.id);
  }

  const next = `${window.location.pathname}${params.toString() ? `?${params}` : ""}`;
  history.replaceState({}, "", next);
}

function buildPageUrl(page, extraParams = {}) {
  const params = new URLSearchParams();
  params.set("lang", state.language);

  Object.entries(extraParams).forEach(([key, value]) => {
    if (value !== undefined && value !== null && String(value).trim() !== "") {
      params.set(key, String(value));
    }
  });

  return `${PAGE_FILES[page]}${params.toString() ? `?${params}` : ""}`;
}

function refreshEntryCache() {
  allEntries = mergeEntries(staticEntries, backendState.publishedEntries ?? []);
  orderedCategories = buildCategoryList(allEntries);
  craftableEntries = allEntries.filter((entry) => hasContentList(entry, "crafting"));

  if (!getEntryById(state.entryId)) {
    state.entryId = allEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  if (state.category !== "all" && !orderedCategories.includes(state.category)) {
    state.category = "all";
  }

  if (!state.adminDraft && allEntries.length > 0) {
    loadEntryIntoDraft(state.entryId);
  }
}

async function ensureCurrentEntryComments(force = false) {
  if (!backendState.enabled) {
    return [];
  }

  const current = backendState.commentsByEntry[state.entryId];
  if (!force && (current?.loading || Array.isArray(current?.items))) {
    return current.items ?? [];
  }

  return loadComments(state.entryId);
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
    if (state.category !== "all" && entry.category !== state.category) {
      return false;
    }

    if (!term) {
      return true;
    }

    return normalize(buildSearchText(entry)).includes(term);
  });
}

function getHomeFeaturedEntries() {
  const curated = HOME_FEATURED_IDS
    .map((entryId) => getEntryById(entryId))
    .filter(Boolean);

  if (curated.length >= 4) {
    return curated;
  }

  return allEntries
    .filter((entry) => entry.category !== "blocks" && entry.category !== "events" && entry.category !== "systems")
    .slice(0, 6);
}

function getAdminBrowserEntries() {
  const term = normalize(state.adminSearch);
  const category = state.adminCategory;
  return allEntries.filter((entry) => {
    if (category !== "all" && entry.category !== category) {
      return false;
    }

    if (!term) {
      return true;
    }
    return normalize(buildSearchText(entry)).includes(term);
  }).slice(0, 120);
}

function getEntriesByCategory(category) {
  return allEntries.filter((entry) => entry.category === category);
}

function getProgressionEntries(groupKey) {
  return allEntries.filter((entry) => {
    const meta = getEntryMeta(entry);
    if (entry.id === "crystaline-devourer") {
      return groupKey === "post_moonlord";
    }

    return Boolean(meta.featureInProgression) && meta.progressionGroup === groupKey;
  }).sort(compareEntries);
}

function getEntryById(entryId) {
  return allEntries.find((entry) => entry.id === entryId);
}

function getUsedInEntries(entry) {
  const content = getLocalizedEntry(entry);
  const needles = [entry.id, content.title]
    .filter(Boolean)
    .map((value) => normalize(value));

  return allEntries.filter((candidate) => {
    if (candidate.id === entry.id) {
      return false;
    }

    return getEntryLanguageContents(candidate).some((candidateContent) => {
      return (candidateContent?.crafting ?? []).some((line) => {
        const normalizedLine = normalize(line);
        return needles.some((needle) => needle && normalizedLine.includes(needle));
      });
    });
  }).sort(compareEntries);
}

function findSummonEntry(entry) {
  const directMatch = (entry.related ?? [])
    .map((entryId) => getEntryById(entryId))
    .find((relatedEntry) => relatedEntry?.category === "summons");

  if (directMatch) {
    return directMatch;
  }

  return allEntries.find((candidate) => candidate.category === "summons" && (candidate.related ?? []).includes(entry.id));
}

function getEntryMeta(entry) {
  return entry?.content?._meta ?? {};
}

function getEntryLanguageContents(entry) {
  return Object.entries(entry?.content ?? {})
    .filter(([languageCode]) => !languageCode.startsWith("_"))
    .map(([, content]) => content);
}

function getLocalizedEntry(entry) {
  if (!entry) {
    return {};
  }

  return entry.content?.[state.language]
    ?? entry.content?.[siteConfig.defaultLanguage]
    ?? entry.content?.en
    ?? getEntryLanguageContents(entry)[0]
    ?? {};
}

function getCategoryLabel(category) {
  const baseCopy = uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage] ?? uiCopy.en;
  return baseCopy.categories?.[category] ?? humanizeCategory(category);
}

function getSiteName() {
  return (uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage] ?? uiCopy.en).siteName;
}

function getCopy() {
  return pageCopy[state.language] ?? pageCopy.en;
}

function isDefaultEntryImage(imageUrl) {
  const clean = String(imageUrl ?? "").trim();
  return !clean || clean === DEFAULT_ENTRY_IMAGE;
}

function getEntryExternalLookup(entry) {
  const meta = getEntryMeta(entry);
  const content = getLocalizedEntry(entry);
  return parseTerrariaWikiReference(meta.wikiSource)
    || String(meta.vanillaAlias ?? "").trim()
    || String(content.title ?? "").trim()
    || String(entry?.id ?? "").trim();
}

function getEntryDisplayAsset(entry, options = {}) {
  if (!entry) {
    return {
      imageUrl: DEFAULT_ENTRY_IMAGE,
      externalPageUrl: "",
      sourceType: "default"
    };
  }

  const meta = getEntryMeta(entry);
  const primaryImage = String(entry.image ?? "").trim();
  if (!isDefaultEntryImage(primaryImage)) {
    return {
      imageUrl: primaryImage,
      externalPageUrl: "",
      sourceType: "primary"
    };
  }

  const fallbackImage = String(meta.fallbackImage ?? "").trim();
  if (fallbackImage) {
    return {
      imageUrl: fallbackImage,
      externalPageUrl: buildTerrariaWikiPageUrl(meta.wikiSource),
      sourceType: "fallback"
    };
  }

  const lookupLabel = getEntryExternalLookup(entry);
  const externalAsset = lookupLabel ? getExternalAsset(lookupLabel) : null;
  if (!externalAsset && options.ensure !== false && lookupLabel) {
    ensureExternalAsset(lookupLabel);
  }

  if (externalAsset?.imageUrl) {
    return {
      imageUrl: externalAsset.imageUrl,
      externalPageUrl: externalAsset.pageUrl,
      sourceType: "wiki"
    };
  }

  return {
    imageUrl: primaryImage || DEFAULT_ENTRY_IMAGE,
    externalPageUrl: buildTerrariaWikiPageUrl(meta.wikiSource),
    sourceType: "default"
  };
}

function getTagLabel(entry) {
  const categoryTagMap = {
    bosses: "boss",
    superbosses: "superboss",
    minibosses: "miniboss",
    mobs: "mob",
    summons: "summon",
    weapons: "weapon",
    armor: "armor",
    accessories: "accessory",
    consumables: "consumable",
    materials: "material",
    blocks: "block",
    tools: "tool",
    npcs: "npc",
    dimensions: "dimension",
    events: "event",
    systems: "system"
  };
  const tagKey = ENTRY_TAGS[entry.id]
    ?? categoryTagMap[entry.category]
    ?? "entry";
  const labels = {
    "pt-BR": {
      boss: "Boss",
      superboss: "Superboss",
      miniboss: "Mini-Boss",
      mob: "Mob",
      summon: "Invocacao",
      weapon: "Arma",
      armor: "Armadura",
      accessory: "Acessorio",
      consumable: "Consumivel",
      material: "Material",
      block: "Bloco",
      tool: "Ferramenta",
      npc: "NPC",
      dimension: "Dimensao",
      event: "Evento",
      system: "Sistema",
      entry: "Entrada"
    },
    en: {
      boss: "Boss",
      superboss: "Superboss",
      miniboss: "Mini-Boss",
      mob: "Mob",
      summon: "Summon",
      weapon: "Weapon",
      armor: "Armor",
      accessory: "Accessory",
      consumable: "Consumable",
      material: "Material",
      block: "Block",
      tool: "Tool",
      npc: "NPC",
      dimension: "Dimension",
      event: "Event",
      system: "System",
      entry: "Entry"
    }
  };
  return (labels[state.language] ?? labels.en)[tagKey] ?? tagKey;
}

function buildSearchText(entry) {
  const content = getLocalizedEntry(entry);
  const meta = getEntryMeta(entry);
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
      ...(content.pieces ?? []),
      meta.vanillaAlias,
      meta.wikiSource,
      meta.progressionGate,
      meta.spawnKind,
      meta.eventKey
    ].filter(Boolean).join(" ");
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
      image: entry.image ?? existing?.image ?? DEFAULT_ENTRY_IMAGE,
      banner: entry.banner ?? existing?.banner ?? "",
      related: Array.isArray(entry.related) ? [...entry.related] : [...(existing?.related ?? [])],
      sortOrder: Number(entry.sortOrder ?? existing?.sortOrder ?? 0),
      isPublished: entry.isPublished ?? existing?.isPublished ?? true,
      content: mergeContentObjects(existing?.content ?? {}, entry.content ?? {})
    });
  });

  return [...merged.values()].sort(compareEntries);
}

function mergeStaticSources(...sourceLists) {
  const merged = new Map();

  sourceLists.flat().forEach((entry) => {
    if (!entry) {
      return;
    }

    const existing = merged.get(entry.id);
    if (!existing) {
      merged.set(entry.id, cloneEntry(entry));
      return;
    }

    const nextImage = !isDefaultEntryImage(entry.image) ? entry.image : existing.image;
    const nextSortOrder = Number(entry.sortOrder ?? 0) !== 0 || Number(existing.sortOrder ?? 0) === 0
      ? Number(entry.sortOrder ?? existing.sortOrder ?? 0)
      : Number(existing.sortOrder ?? 0);

    merged.set(entry.id, {
      id: entry.id,
      category: entry.category ?? existing.category,
      image: nextImage,
      banner: entry.banner || existing.banner || "",
      related: [...new Set([...(existing.related ?? []), ...(entry.related ?? [])])],
      sortOrder: nextSortOrder,
      isPublished: entry.isPublished ?? existing.isPublished ?? true,
      content: mergeContentObjects(existing.content, entry.content ?? {})
    });
  });

  return [...merged.values()].sort(compareEntries);
}

function cloneEntry(entry) {
  return {
    id: entry.id,
    category: entry.category,
    image: entry.image,
    banner: entry.banner ?? "",
    related: [...(entry.related ?? [])],
    sortOrder: Number(entry.sortOrder ?? 0),
    isPublished: entry.isPublished ?? true,
    content: mergeContentObjects({}, entry.content ?? {})
  };
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
  const ordered = CATEGORY_ORDER.filter((category) => found.has(category));
  const extras = [...found]
    .filter((category) => !CATEGORY_ORDER.includes(category))
    .sort((left, right) => normalize(humanizeCategory(left)).localeCompare(normalize(humanizeCategory(right))));

  return [...ordered, ...extras];
}

function ensureAdminDraft() {
  if (!state.adminDraft) {
    loadEntryIntoDraft(state.entryId);
  }

  return state.adminDraft;
}

function loadEntryIntoDraft(entryId) {
  const entry = getEntryById(entryId);
  const content = getLocalizedEntry(entry);
  const meta = getEntryMeta(entry);

  state.adminDraft = {
    id: entry?.id ?? "",
    category: entry?.category ?? "materials",
    sortOrder: String(entry?.sortOrder ?? 0),
    imageUrl: !isDefaultEntryImage(entry?.image) ? entry?.image ?? "" : "",
    fallbackImage: String(meta?.fallbackImage ?? ""),
    vanillaAlias: String(meta?.vanillaAlias ?? ""),
    wikiSource: String(meta?.wikiSource ?? ""),
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
    published: Boolean(entry?.isPublished)
  };
}

function clearAdminDraft() {
  state.adminDraft = {
    id: "",
    category: state.adminCategory !== "all"
      ? state.adminCategory
      : state.category !== "all"
        ? state.category
        : "materials",
    sortOrder: "0",
    imageUrl: "",
    fallbackImage: "",
    vanillaAlias: "",
    wikiSource: "",
    imagePath: "entries",
    related: "",
    title: "",
    subtitle: "",
    summary: "",
    overview: "",
    facts: "",
    obtain: "",
    crafting: "",
    drops: "",
    pieces: "",
    notes: "",
    tactics: "",
    published: false
  };
}

function updateDraftFromForm(form) {
  const formData = new FormData(form);
  state.adminDraft = {
    id: String(formData.get("id") ?? "").trim(),
    category: String(formData.get("category") ?? "").trim(),
    sortOrder: String(formData.get("sortOrder") ?? "0").trim(),
    imageUrl: String(formData.get("imageUrl") ?? "").trim(),
    fallbackImage: String(formData.get("fallbackImage") ?? "").trim(),
    vanillaAlias: String(formData.get("vanillaAlias") ?? "").trim(),
    wikiSource: String(formData.get("wikiSource") ?? "").trim(),
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

function buildLocalizedContentPayload(baseEntry, draft) {
  const existingContent = mergeContentObjects({}, baseEntry?.content ?? {});
  const currentLanguageContent = existingContent[state.language]
    ?? existingContent[siteConfig.defaultLanguage]
    ?? existingContent.en
    ?? {};

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
    _meta: {
      ...(existingContent._meta ?? {}),
      vanillaAlias: draft.vanillaAlias,
      wikiSource: draft.wikiSource,
      fallbackImage: draft.fallbackImage
    },
    [state.language]: nextLanguageContent
  };

  languageOptions.forEach((option) => {
    if (!nextContent[option.code]) {
      nextContent[option.code] = nextLanguageContent;
    }
  });

  return nextContent;
}

function parseRecipeModel(entry) {
  const content = getLocalizedEntry(entry);
  const lines = content.crafting ?? [];
  const stations = extractWorkstations(lines);
  const ingredients = [];

  lines.forEach((line) => {
    const cleanedLine = stripStationClause(line);
    if (!cleanedLine) {
      return;
    }

    cleanedLine
      .split("+")
      .map((segment) => segment.trim())
      .filter(Boolean)
      .forEach((segment) => {
        ingredients.push(parseRecipeIngredientLine(segment));
      });
  });

  return {
    lines,
    stations,
    ingredients
  };
}

function parseRecipeIngredientLine(segment) {
  const normalizedSegment = segment.includes(":")
    ? segment.split(":").pop().trim()
    : segment.trim();
  const suffixAmount = normalizedSegment.match(/^(.+?)\s+x(\d+(?:-\d+)?)$/i);
  const prefixAmount = normalizedSegment.match(/^(\d+(?:-\d+)?)\s+(.+)$/i);
  const amount = prefixAmount?.[1] ?? suffixAmount?.[2] ?? "";
  const label = (prefixAmount?.[2] ?? suffixAmount?.[1] ?? normalizedSegment).trim();
  const entry = findEntryByMention(label);

  return {
    raw: segment,
    amount,
    label,
    entry
  };
}

function extractWorkstations(lines) {
  const stations = [];
  const seen = new Set();

  lines.forEach((line) => {
    const normalizedLine = ` ${normalize(line)} `;
    WORKSTATIONS.forEach((station) => {
      if (station.id === "anvil" && (normalizedLine.includes(" mythril anvil ") || normalizedLine.includes(" orichalcum anvil "))) {
        return;
      }

      if (!seen.has(station.id) && station.keywords.some((keyword) => normalizedLine.includes(keyword))) {
        seen.add(station.id);
        stations.push(station);
      }
    });
  });

  return stations;
}

function stripStationClause(line) {
  const trimmed = String(line ?? "").trim();
  if (!trimmed) {
    return "";
  }

  const stationOnly = extractWorkstations([trimmed]);
  if (stationOnly.length > 0 && !trimmed.includes("+") && !/\d/.test(trimmed)) {
    return "";
  }

  const matcher = trimmed.match(/^(.*?)(?:\s+(?:na|no|em|at)\s+.+)$/i);
  if (matcher && extractWorkstations([trimmed]).length > 0) {
    return matcher[1].trim();
  }

  return trimmed;
}

function findEntryByMention(label) {
  const normalizedLabel = normalize(label).replace(/^the\s+/, "").trim();
  if (!normalizedLabel) {
    return null;
  }

  const candidateEntries = [...allEntries].sort((left, right) => {
    const leftTitle = getLocalizedEntry(left).title ?? left.id;
    const rightTitle = getLocalizedEntry(right).title ?? right.id;
    return normalize(rightTitle).length - normalize(leftTitle).length;
  });

  const exact = candidateEntries.find((entry) => {
    const content = getLocalizedEntry(entry);
    const meta = getEntryMeta(entry);
    const alias = normalize(meta.vanillaAlias).trim();
    return normalize(entry.id) === normalizedLabel
      || normalize(content.title) === normalizedLabel
      || (alias && alias === normalizedLabel);
  });

  if (exact) {
    return exact;
  }

  return candidateEntries.find((entry) => {
    const content = getLocalizedEntry(entry);
    const meta = getEntryMeta(entry);
    const alias = normalize(meta.vanillaAlias).trim();
    return normalizedLabel.includes(normalize(content.title))
      || normalizedLabel.includes(normalize(entry.id))
      || (alias && normalizedLabel.includes(alias));
  }) ?? null;
}

function loadExternalAssetCache() {
  try {
    const raw = window.localStorage?.getItem("cd_external_asset_cache_v1");
    return raw ? JSON.parse(raw) : {};
  }
  catch {
    return {};
  }
}

function buildInitialExternalAssetCache() {
  const generatedCache = Object.fromEntries(
    Object.entries(generatedTerrariaAssets ?? {}).map(([key, data]) => [key, { status: "ready", data }])
  );

  return {
    ...loadExternalAssetCache(),
    ...generatedCache
  };
}

function saveExternalAssetCache() {
  try {
    window.localStorage?.setItem("cd_external_asset_cache_v1", JSON.stringify(externalAssetState.cache));
  }
  catch {
    // Ignore storage quota or privacy mode issues.
  }
}

function getExternalAsset(label) {
  const record = externalAssetState.cache[getExternalAssetKey(label)];
  return record?.status === "ready" ? record.data : null;
}

function ensureExternalAsset(label) {
  const key = getExternalAssetKey(label);
  if (!key || !shouldResolveExternalAsset(label) || !RUNTIME_TERRARIA_LOOKUP_ENABLED) {
    return;
  }

  const current = externalAssetState.cache[key];
  if (current?.status === "loading" || current?.status === "ready" || current?.status === "missing") {
    return;
  }

  externalAssetState.cache[key] = { status: "loading" };

  fetchTerrariaWikiAsset(label)
    .then((asset) => {
      externalAssetState.cache[key] = asset
        ? { status: "ready", data: asset }
        : { status: "missing" };
      saveExternalAssetCache();
      render();
    })
    .catch(() => {
      externalAssetState.cache[key] = { status: "missing" };
      saveExternalAssetCache();
      render();
    });
}

function getExternalAssetKey(label) {
  return normalize(parseTerrariaWikiReference(label) || label)
    .replace(/\s+/g, " ")
    .trim();
}

function shouldResolveExternalAsset(label) {
  const clean = parseTerrariaWikiReference(label) || String(label ?? "").trim();
  return Boolean(clean && clean.length <= 48 && !clean.includes("/") && !clean.includes("\\"));
}

async function fetchTerrariaWikiAsset(label) {
  const candidates = buildTerrariaWikiCandidates(label);

  for (const candidate of candidates) {
    const resolvedTitle = await resolveTerrariaWikiTitle(candidate);
    const data = await fetchTerrariaWikiParse(resolvedTitle || candidate);
    if (!data?.parse?.text?.["*"]) {
      continue;
    }

    const imageUrl = extractTerrariaWikiImage(data.parse.text["*"]);
    if (!imageUrl) {
      continue;
    }

    const title = data.parse.title ?? candidate;
    return {
      title,
      imageUrl,
      pageUrl: `${TERRARIA_WIKI.pageBaseUrl}${encodeURIComponent(title.replaceAll(" ", "_"))}`
    };
  }

  return null;
}

async function resolveTerrariaWikiTitle(candidate) {
  const url = new URL(TERRARIA_WIKI.apiUrl);
  url.searchParams.set("action", "query");
  url.searchParams.set("titles", candidate);
  url.searchParams.set("redirects", "1");
  url.searchParams.set("format", "json");
  url.searchParams.set("origin", "*");

  const response = await fetch(url.toString());
  if (!response.ok) {
    return "";
  }

  const data = await response.json();
  const page = Object.values(data?.query?.pages ?? {})[0];
  return page?.missing === "" ? "" : page?.title ?? "";
}

async function fetchTerrariaWikiParse(title) {
  const url = new URL(TERRARIA_WIKI.apiUrl);
  url.searchParams.set("action", "parse");
  url.searchParams.set("page", title);
  url.searchParams.set("prop", "text");
  url.searchParams.set("format", "json");
  url.searchParams.set("origin", "*");

  const response = await fetch(url.toString());
  if (!response.ok) {
    return null;
  }

  return response.json();
}

function buildTerrariaWikiCandidates(label) {
  const clean = (parseTerrariaWikiReference(label) || String(label ?? ""))
    .replace(/\s+x\d+(?:-\d+)?$/i, "")
    .replace(/^\d+(?:-\d+)?\s+/, "")
    .replace(/\s+/g, " ")
    .trim();

  const candidates = new Set([clean]);
  if (clean.endsWith("s")) {
    candidates.add(clean.slice(0, -1));
  }

  return [...candidates].filter(Boolean);
}

function parseTerrariaWikiReference(reference) {
  const raw = String(reference ?? "").trim();
  if (!raw) {
    return "";
  }

  if (/^https?:\/\//i.test(raw)) {
    try {
      const url = new URL(raw);
      if (!/terraria\.wiki\.gg$/i.test(url.hostname)) {
        return "";
      }

      const pagePath = url.pathname.replace(/^\/wiki\//, "");
      return decodeURIComponent(pagePath).replaceAll("_", " ").trim();
    }
    catch {
      return "";
    }
  }

  return raw.replaceAll("_", " ").trim();
}

function buildTerrariaWikiPageUrl(reference) {
  const title = parseTerrariaWikiReference(reference);
  return title ? `${TERRARIA_WIKI.pageBaseUrl}${encodeURIComponent(title.replaceAll(" ", "_"))}` : "";
}

function extractTerrariaWikiImage(html) {
  const parser = new DOMParser();
  const doc = parser.parseFromString(html, "text/html");
  const preferredImage = doc.querySelector('.infobox.item .section.images img[alt*="sprite"], .infobox.item .section.images img');
  const src = preferredImage?.getAttribute("src");

  if (!src) {
    return "";
  }

  if (src.startsWith("//")) {
    return `https:${src}`;
  }

  if (src.startsWith("/")) {
    return `https://terraria.wiki.gg${src}`;
  }

  return src;
}

function hasContentList(entry, key) {
  return getEntryLanguageContents(entry).some((content) => Array.isArray(content?.[key]) && content[key].length > 0);
}

function getEntrySortTitle(entry) {
  return entry?.content?.[siteConfig.defaultLanguage]?.title
    ?? entry?.content?.en?.title
    ?? getEntryLanguageContents(entry)[0]?.title
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

function normalize(value) {
  return String(value ?? "").toLowerCase();
}

function humanizeCategory(category) {
  return String(category ?? "")
    .replace(/[-_]+/g, " ")
    .replace(/\s+/g, " ")
    .trim()
    .replace(/\b\w/g, (character) => character.toUpperCase());
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

function escapeHtml(value) {
  return String(value ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#39;");
}
