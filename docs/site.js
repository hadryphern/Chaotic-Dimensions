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

const ASSET_VERSION = "20260327c";
const SITE_ICON_IMAGE = `./assets/images/icon.png?v=${ASSET_VERSION}`;
const SITE_FAVICON_IMAGE = `./assets/images/favicon.png?v=${ASSET_VERSION}`;
const DEFAULT_ENTRY_IMAGE = SITE_FAVICON_IMAGE;
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
      title: "A principal wiki do Chaotic Dimensions.",
      lead: "Um arquivo vivo para acompanhar itens, criaturas, receitas, progressao e sistemas do mod em um visual cosmico com leitura de wiki classica.",
      introTitle: "O que voce encontra aqui",
      introBody: "Cada area da wiki existe para documentar uma parte diferente do mod com clareza: consultar entradas, entender crafting, acompanhar a progressao e centralizar feedback da comunidade.",
      links: [
        { page: "library", title: "Biblioteca", body: "A Biblioteca reune as paginas principais da wiki em um catalogo pesquisavel, com filtros por categoria e acesso rapido para itens, mobs, bosses, materiais e sistemas." },
        { page: "crafting", title: "Crafting", body: "A area de Crafting foi separada para mostrar receitas com mais clareza, destacando estacoes de trabalho, ingredientes, sprites e relacoes entre os itens usados." },
        { page: "progression", title: "Progressao", body: "A pagina de Progressao organiza a rota do mod por etapas, encontros e marcos importantes, facilitando entender quando cada boss, mob ou sistema entra em cena." },
        { page: "feedback", title: "Feedback", body: "Feedback concentra comentarios, contas e retorno da comunidade sem poluir a parte enciclopedica da wiki, deixando o restante do site mais limpo e focado." }
      ],
      categoryTitle: "Categorias ativas",
      categoryBody: "As categorias agora funcionam como vitrines visuais da wiki, usando sprites e silhuetas do proprio conteudo para dar identidade a cada grupo.",
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
      title: "The main Chaotic Dimensions wiki.",
      lead: "A living archive for items, creatures, recipes, progression and systems, built with a cosmic atmosphere and a cleaner classic-wiki reading flow.",
      introTitle: "What lives here",
      introBody: "Each section of the wiki documents a different layer of the mod with more clarity: browse entries, inspect crafting, follow progression and centralize community feedback.",
      links: [
        { page: "library", title: "Library", body: "The Library gathers the main wiki pages in a searchable catalogue with category filters and cleaner access to items, mobs, bosses, materials and systems." },
        { page: "crafting", title: "Crafting", body: "Crafting has its own space so recipes can breathe, with workstations, ingredients, sprites and item relationships displayed more clearly." },
        { page: "progression", title: "Progression", body: "The Progression page arranges the mod by stages, encounters and milestones so it is easier to understand when each boss, mob or system enters the journey." },
        { page: "feedback", title: "Feedback", body: "Feedback centralizes comments, accounts and community notes without cluttering the encyclopedic side of the wiki." }
      ],
      categoryTitle: "Active categories",
      categoryBody: "Categories now act like visual showcases, using sprites and silhouettes from the content itself to give each group more identity.",
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
        <img src="${SITE_ICON_IMAGE}" alt="Chaotic Dimensions icon">
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
  const categoryMarkup = orderedCategories.map((category) => {
    const previewEntries = getHomeCategoryPreviewEntries(category);
    const previewMarkup = previewEntries.length > 0
      ? previewEntries.map((entry) => {
        const content = getLocalizedEntry(entry);
        const asset = getEntryDisplayAsset(entry, { ensure: false });
        return `<img class="summary-sprite" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}" loading="lazy">`;
      }).join("")
      : `<span class="summary-sprite summary-sprite--empty">?</span>`;

    return `
      <article class="summary-tile">
        <div class="summary-text">
          <strong>${getCategoryLabel(category)}</strong>
          <p>${getHomeCategoryBlurb(category)}</p>
        </div>
        <div class="summary-sprite-row" aria-hidden="true">${previewMarkup}</div>
      </article>
    `;
  }).join("");

  const pageLinks = copy.home.links.map((item) => `
    <article class="feature-tile">
      <h3>${item.title}</h3>
      <p>${item.body}</p>
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

function getHomeCategoryPreviewEntries(category, limit = 6) {
  return getEntriesByCategory(category)
    .filter((entry) => entry?.isPublished !== false)
    .slice(0, limit);
}

function getHomeCategoryBlurb(category) {
  const labels = {
    bosses: {
      "pt-BR": "Chefes, encontros centrais e lutas que definem o ritmo da progressao.",
      en: "Boss encounters and major fights that define the pace of progression."
    },
    superbosses: {
      "pt-BR": "Encontros de pico para o endgame e desafios de alto impacto.",
      en: "Peak endgame encounters built for the heaviest challenges."
    },
    minibosses: {
      "pt-BR": "Mini-bosses que reforcam biomas, rotas e recompensas intermediarias.",
      en: "Mini-bosses that reinforce biomes, routes and mid-tier rewards."
    },
    mobs: {
      "pt-BR": "Criaturas e spawns do mundo que ajudam a contar o ecossistema do mod.",
      en: "Creatures and world spawns that shape the mod's ecosystem."
    },
    summons: {
      "pt-BR": "Itens de invocacao, chamadas de encounter e gatilhos especiais.",
      en: "Summoning items, encounter triggers and ritual-like calls."
    },
    weapons: {
      "pt-BR": "Armas, variantes e poderes que formam o arsenal do Chaotic Dimensions.",
      en: "Weapons, variants and powers that build the Chaotic Dimensions arsenal."
    },
    armor: {
      "pt-BR": "Sets, pecas defensivas e armaduras voltadas para cada estilo de jogo.",
      en: "Armor sets, defensive pieces and class-focused equipment."
    },
    materials: {
      "pt-BR": "Materiais-base usados em crafts, invocacoes, upgrades e sistemas.",
      en: "Core materials used across crafts, summons, upgrades and systems."
    }
  };

  return labels[category]?.[state.language]
    ?? labels[category]?.en
    ?? ((state.language === "pt-BR")
      ? `Uma vitrine visual para acompanhar ${getCategoryLabel(category).toLowerCase()} dentro da wiki.`
      : `A visual showcase for tracking ${getCategoryLabel(category).toLowerCase()} across the wiki.`);
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

  const detailModel = buildEntryDetailModel(entry);
  const {
    content,
    entryAsset,
    bannerImage,
    recipeUrl,
    summonEntry,
    summonContent,
    usedInEntries,
    relatedEntries,
    narrativeParagraphs,
    factItems,
    contextItems,
    obtainItems,
    usageItems,
    progressionItems,
    dropItems,
    pieceItems,
    noteItems,
    tacticItems
  } = detailModel;
  const detailCopy = getExpandedEntryCopy();

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
          ${narrativeParagraphs.length > 0 ? `
            <div class="entry-narrative">
              ${narrativeParagraphs.map((paragraph) => `<p>${escapeHtml(paragraph)}</p>`).join("")}
            </div>
          ` : ""}
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
            ${factItems.length > 0 ? `
              <div class="content-block content-block--compact">
                <h3>${copy.entry.facts}</h3>
                <ul class="content-list">${factItems.map((item) => `<li>${escapeHtml(item)}</li>`).join("")}</ul>
              </div>
            ` : ""}
          </div>
        </aside>
      </div>
    </section>

    <section class="page-section">
      <div class="content-grid">
        ${renderContentBlock(detailCopy.context, contextItems)}
        ${renderContentBlock(copy.entry.obtain, obtainItems)}
        ${renderCraftingContentBlock(copy.entry.crafting, entry)}
        ${renderContentBlock(detailCopy.usage, usageItems)}
        ${renderContentBlock(detailCopy.progression, progressionItems)}
        ${renderContentBlock(copy.entry.drops, dropItems)}
        ${renderContentBlock(copy.entry.pieces, pieceItems)}
        ${renderContentBlock(copy.entry.notes, noteItems)}
        ${renderContentBlock(copy.entry.tactics, tacticItems)}
      </div>
      ${renderLinkedEntriesBlock(copy.entry.usedIn, usedInEntries)}
      ${renderLinkedEntriesBlock(copy.entry.related, relatedEntries)}
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

function buildEntryDetailModel(entry) {
  const content = getLocalizedEntry(entry);
  const entryAsset = getEntryDisplayAsset(entry);
  const bannerImage = entry.banner || "";
  const recipeUrl = hasContentList(entry, "crafting") ? buildPageUrl("crafting", { q: content.title }) : "";
  const summonEntry = findSummonEntry(entry);
  const summonContent = getLocalizedEntry(summonEntry);
  const recipe = parseRecipeModel(entry);
  const usedInEntries = getUsedInEntries(entry).slice(0, 12);
  const relatedEntries = (entry.related ?? [])
    .map((entryId) => getEntryById(entryId))
    .filter(Boolean);
  const relatedBossEntries = relatedEntries.filter((candidate) => isEncounterCategory(candidate?.category));
  const relatedBuffEntries = relatedEntries.filter((candidate) => candidate?.category === "buffs");
  const relatedArmorEntries = relatedEntries.filter((candidate) => candidate?.category === "armor");
  const progressionStage = getEntryProgressionStage(entry, relatedEntries, summonEntry);

  const detailContext = {
    entry,
    content,
    entryAsset,
    bannerImage,
    recipeUrl,
    summonEntry,
    summonContent,
    recipe,
    usedInEntries,
    relatedEntries,
    relatedBossEntries,
    relatedBuffEntries,
    relatedArmorEntries,
    progressionStage
  };

  return {
    ...detailContext,
    narrativeParagraphs: buildEntryNarrativeParagraphs(detailContext),
    factItems: buildExpandedFacts(detailContext),
    contextItems: buildEntryContextItems(detailContext),
    obtainItems: buildEntryObtainItems(detailContext),
    usageItems: buildEntryUsageItems(detailContext),
    progressionItems: buildEntryProgressionItems(detailContext),
    dropItems: buildEntryDropItems(detailContext),
    pieceItems: buildEntryPieceItems(detailContext),
    noteItems: buildEntryNoteItems(detailContext),
    tacticItems: buildEntryTacticItems(detailContext)
  };
}

function getExpandedEntryCopy() {
  const copy = {
    "pt-BR": {
      context: "Contexto e papel na progressao",
      usage: "Uso, valor e sinergias",
      progression: "Onde entra na progressao"
    },
    en: {
      context: "Context and progression role",
      usage: "Use cases, value and synergy",
      progression: "Where it fits in progression"
    }
  };

  return copy[state.language] ?? copy.en;
}

function buildEntryNarrativeParagraphs(detailContext) {
  const {
    entry,
    content,
    recipe,
    summonEntry,
    summonContent,
    usedInEntries,
    relatedEntries,
    relatedBossEntries,
    relatedBuffEntries,
    progressionStage
  } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [];

  if (state.language === "pt-BR") {
    switch (entry.category) {
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(
          `${title} funciona hoje como um encontro-chave da linha atual do mod, servindo como ponte entre a progressao vanilla avancada e os sistemas proprios do Chaotic Dimensions. ${progressionStage ? `Dentro da wiki, ele se encaixa em ${progressionStage}.` : ""}`.trim()
        );
        if (summonEntry) {
          lines.push(
            `A leitura desta pagina fica mais completa quando combinada com ${summonContent.title ?? summonEntry.id}, porque a invocacao, o preparo da receita e o ritmo das tentativas fazem parte do mesmo ciclo de progressao.`
          );
        }
        if (relatedEntries.length > 0) {
          lines.push(
            `As recompensas e paginas mais ligadas a esse encontro hoje incluem ${formatEntryTitleList(relatedEntries, 4)}, o que ajuda a mostrar por que derrotar o boss impacta diretamente no resto do arsenal cristalino.`
          );
        }
        break;
      case "summons":
        lines.push(
          `${title} existe para abrir um encontro ou uma etapa importante da wiki. Mesmo quando a pagina ja lista a receita, o valor real do item esta em controlar quando e como o jogador acessa o conteudo ligado a ele.`
        );
        if (relatedBossEntries.length > 0) {
          lines.push(
            `Hoje a funcao principal desta invocacao e levar o jogador ate ${formatEntryTitleList(relatedBossEntries, 3)}, entao a propria receita dela ajuda a ditar o ritmo das tentativas e do farm.`
          );
        }
        break;
      case "materials":
        lines.push(
          `${title} atua como material-base do arco cristalino atual. Em vez de ser apenas um drop numerico, ele funciona como a ponte pratica entre o loot de um encontro e a construcao de armas, armaduras, acessorios e outros upgrades.`
        );
        if (usedInEntries.length > 0) {
          lines.push(
            `No estado atual da wiki, esse material ja aparece em ${usedInEntries.length} receita(s) rastreada(s), incluindo ${formatEntryTitleList(usedInEntries, 4)}, o que transforma a pagina dele em uma referencia natural para planejar o proximo craft.`
          );
        }
        break;
      case "weapons":
        lines.push(
          `${title} ocupa um lugar direto no arsenal cristalino do mod. A pagina nao serve so para listar dano e recipe: ela tambem ajuda a mostrar de onde vem o item, em que etapa ele aparece e como ele conversa com outras pecas do mesmo tier.`
        );
        if (recipe.ingredients.length > 0) {
          lines.push(
            `A receita rastreada para essa arma passa por ${formatIngredientLabelList(recipe.ingredients, 4)}${recipe.stations.length > 0 ? ` em ${formatStationLabelList(recipe.stations, 2)}` : ""}, o que deixa claro como ela converte recursos de progressao em poder ofensivo imediato.`
          );
        }
        break;
      case "armor":
        lines.push(
          `${title} organiza a camada defensiva do tier atual e, ao mesmo tempo, abre variacoes reais por classe. Isso faz a pagina da armadura funcionar tanto como ficha de status quanto como guia de montagem do set.`
        );
        if (relatedBuffEntries.length > 0) {
          lines.push(
            `Como o set tambem se conecta a efeitos como ${formatEntryTitleList(relatedBuffEntries, 3)}, vale ler a armadura junto desses buffs para entender o pacote completo de defesa, mobilidade e resposta ao dano.`
          );
        }
        break;
      case "accessories":
        lines.push(
          `${title} mistura utilidade e sobrevivencia numa unica peca. Em paginas assim, o que importa nao e so o valor numerico do acessorio, mas o tipo de rota, luta ou reposicionamento que ele viabiliza dentro do mod.`
        );
        if (relatedBuffEntries.length > 0) {
          lines.push(
            `A leitura tambem fica melhor quando voce acompanha os efeitos ligados a ele, como ${formatEntryTitleList(relatedBuffEntries, 3)}, porque o acessorio nao termina no tooltip principal.`
          );
        }
        break;
      case "consumables":
        lines.push(
          `${title} entra como consumivel premium do pacote cristalino. O papel dessa pagina e mostrar nao so a cura ou o buff isolado, mas o quanto o item estende a consistencia de tentativas, farm e lutas longas no mesmo tier.`
        );
        if (relatedBuffEntries.length > 0) {
          lines.push(
            `Como a entrada ja se conecta a ${formatEntryTitleList(relatedBuffEntries, 3)}, ela tambem funciona como ponto central para entender todo o pacote de efeitos que o uso do consumivel entrega.`
          );
        }
        break;
      case "buffs":
        lines.push(
          `${title} representa um efeito temporario, entao esta pagina faz mais sentido quando lida como extensao da fonte que o aplica. Em outras palavras, o buff explica o resultado final de uma mecanica maior do mod.`
        );
        if (relatedEntries.length > 0) {
          lines.push(
            `Hoje ele aparece ligado a ${formatEntryTitleList(relatedEntries, 3)}, o que ajuda a entender quando esse efeito entra em cena, quanto ele reforca a build e por que vale documenta-lo separadamente.`
          );
        }
        break;
      default:
        if (relatedEntries.length > 0) {
          lines.push(
            `${title} faz parte de uma rede maior de paginas relacionadas, principalmente ${formatEntryTitleList(relatedEntries, 3)}, e a ideia desta entrada e ajudar a ligar tudo isso num fluxo mais legivel.`
          );
        }
        break;
    }
  }
  else {
    switch (entry.category) {
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(`${title} currently acts as a key encounter in the mod, bridging late vanilla progression and the mod's own reward loop.${progressionStage ? ` In the wiki it fits into ${progressionStage}.` : ""}`);
        if (summonEntry) {
          lines.push(`This page reads best together with ${summonContent.title ?? summonEntry.id}, because the summon item, its recipe and the attempt loop all belong to the same progression flow.`);
        }
        break;
      case "materials":
        lines.push(`${title} acts as a core material for the current crystal branch. Instead of being just another drop, it turns encounter rewards into weapons, armor pieces, accessories and follow-up upgrades.`);
        if (usedInEntries.length > 0) {
          lines.push(`The current wiki already tracks ${usedInEntries.length} recipe link(s) for this material, including ${formatEntryTitleList(usedInEntries, 4)}, which makes this page a natural reference when planning the next craft.`);
        }
        break;
      default:
        lines.push(`${title} is documented here not only as a stat sheet, but as part of a larger progression path that links obtain methods, connected entries, crafting and practical value.`);
        break;
    }
  }

  return dedupeLines(lines).slice(0, 3);
}

function buildExpandedFacts(detailContext) {
  const { recipe, summonEntry, summonContent, usedInEntries, relatedEntries, progressionStage, content } = detailContext;
  const lines = [...(content.facts ?? [])];

  if (progressionStage) {
    lines.push(state.language === "pt-BR"
      ? `Etapa atual: ${progressionStage}`
      : `Current stage: ${progressionStage}`);
  }

  if (usedInEntries.length > 0) {
    lines.push(state.language === "pt-BR"
      ? `Usado em ${usedInEntries.length} entrada(s) rastreada(s)`
      : `Used in ${usedInEntries.length} tracked entries`);
  }

  if (recipe.ingredients.length > 0) {
    lines.push(state.language === "pt-BR"
      ? `Receita rastreada com ${recipe.ingredients.length} componente(s)`
      : `Tracked recipe with ${recipe.ingredients.length} component(s)`);
  }

  if (recipe.stations.length > 0) {
    lines.push(state.language === "pt-BR"
      ? `Estacao principal: ${formatStationLabelList(recipe.stations, 1)}`
      : `Main station: ${formatStationLabelList(recipe.stations, 1)}`);
  }

  if (summonEntry) {
    lines.push(state.language === "pt-BR"
      ? `Invocacao ligada: ${summonContent.title ?? summonEntry.id}`
      : `Linked summon: ${summonContent.title ?? summonEntry.id}`);
  }

  if (relatedEntries.length > 0) {
    lines.push(state.language === "pt-BR"
      ? `${relatedEntries.length} pagina(s) relacionada(s) na wiki`
      : `${relatedEntries.length} related wiki page(s)`);
  }

  return dedupeLines(lines).slice(0, 8);
}

function buildEntryContextItems(detailContext) {
  const { entry, content, progressionStage, relatedEntries, usedInEntries } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [];

  if (state.language === "pt-BR") {
    switch (entry.category) {
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(`${title} e tratado pela wiki como um encontro central, entao a pagina dele precisa servir tanto para leitura rapida quanto para consulta de recompensas, invocacao e rota de progressao.`);
        break;
      case "summons":
        lines.push(`${title} funciona como gatilho de encontro ou chave de acesso para outra parte do mod, entao o valor dele vai alem do item em si.`);
        break;
      case "materials":
        lines.push(`${title} e um material de conversao: ele sai de uma etapa da progressao e reaparece em outra na forma de equipamentos, armas ou utilitarios.`);
        break;
      case "weapons":
        lines.push(`${title} entra como arma do tier cristalino atual e existe para transformar recursos de progressao em dano e pressao imediata.`);
        break;
      case "armor":
        lines.push(`${title} organiza a parte defensiva do tier atual e distribui o valor do set entre base fixa e variacoes por classe.`);
        break;
      case "accessories":
        lines.push(`${title} ocupa um papel de utilidade ativa, misturando reposicionamento, defesa e suporte para encontros mais exigentes.`);
        break;
      case "consumables":
        lines.push(`${title} foi pensado como consumivel de sustain e preparo, o tipo de item que melhora uma tentativa antes mesmo de a luta ficar caotica.`);
        break;
      case "buffs":
        lines.push(`${title} representa um efeito temporario, entao esta pagina existe para explicar a camada mecanica que fica escondida atras de um item, acessorio ou set.`);
        break;
      default:
        lines.push(`${title} faz parte da malha principal da wiki e serve para ligar nome, funcao, obtencao e relacoes com outras paginas do mod.`);
        break;
    }

    if (progressionStage) {
      lines.push(`Dentro da progressao atual da wiki, a etapa mais clara para esta entrada e ${progressionStage}.`);
    }

    if (relatedEntries.length > 0) {
      lines.push(`As paginas mais ligadas a esta entrada hoje sao ${formatEntryTitleList(relatedEntries, 4)}.`);
    }

    if (entry.category === "materials" && usedInEntries.length > 0) {
      lines.push(`${title} ja aparece em ${usedInEntries.length} uso(s) direto(s) rastreado(s), entao farmar esse material significa abrir espaco para uma parte relevante do conteudo atual.`);
    }
  }
  else {
    lines.push(`${title} is framed by the wiki as part of a bigger progression web, connecting identity, obtain methods, crafting and other related pages.`);
    if (progressionStage) {
      lines.push(`The clearest progression stage for this entry is ${progressionStage}.`);
    }
    if (relatedEntries.length > 0) {
      lines.push(`The closest linked pages right now are ${formatEntryTitleList(relatedEntries, 4)}.`);
    }
  }

  return dedupeLines(lines);
}

function buildEntryObtainItems(detailContext) {
  const { entry, content, recipe, summonEntry, summonContent, relatedEntries, progressionStage } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [...(content.obtain ?? [])];

  if (state.language === "pt-BR") {
    if (entry.category === "bosses" && summonEntry) {
      lines.push(`O jeito pratico de iniciar esse encontro e usando ${summonContent.title ?? summonEntry.id}.`);
      if (summonContent.crafting?.length) {
        lines.push(`A invocacao ligada ja tem receita propria, entao o preparo da luta comeca antes do spawn com ${summonContent.crafting[0]}.`);
      }
    }

    if (lines.length === 0 && recipe.ingredients.length > 0) {
      lines.push(`${title} e obtido principalmente por crafting, nao por drop direto.`);
      lines.push(`A receita rastreada usa ${formatIngredientLabelList(recipe.ingredients, 4)}${recipe.stations.length > 0 ? ` em ${formatStationLabelList(recipe.stations, 2)}` : ""}.`);
    }

    if (entry.category === "buffs" && relatedEntries.length > 0) {
      lines.push(`Esse buff nao e adquirido isoladamente: ele e aplicado por ${formatEntryTitleList(relatedEntries, 3)}.`);
    }

    if (progressionStage && entry.category !== "bosses") {
      lines.push(`A entrada esta amarrada ao momento de ${progressionStage}, entao costuma aparecer quando o jogador ja chegou nesse marco.`);
    }
  }
  else {
    if (lines.length === 0 && recipe.ingredients.length > 0) {
      lines.push(`${title} is primarily obtained through crafting rather than direct drops.`);
      lines.push(`The tracked recipe uses ${formatIngredientLabelList(recipe.ingredients, 4)}${recipe.stations.length > 0 ? ` at ${formatStationLabelList(recipe.stations, 2)}` : ""}.`);
    }
    if (entry.category === "buffs" && relatedEntries.length > 0) {
      lines.push(`This buff is not obtained directly: it is applied by ${formatEntryTitleList(relatedEntries, 3)}.`);
    }
  }

  return dedupeLines(lines);
}

function buildEntryUsageItems(detailContext) {
  const { entry, content, recipe, usedInEntries, relatedEntries, relatedBuffEntries, relatedArmorEntries, relatedBossEntries } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [];

  if (state.language === "pt-BR") {
    switch (entry.category) {
      case "materials":
        if (usedInEntries.length > 0) {
          lines.push(`O uso principal de ${title} hoje e alimentar crafts como ${formatEntryTitleList(usedInEntries, 4)}.`);
          lines.push(`Na pratica, esse material mede o quanto da linha cristalina voce ja consegue converter em upgrade real.`);
        }
        break;
      case "weapons":
        lines.push(`${title} serve como opcao ofensiva direta do arco cristalino atual, entao o valor dela aparece quando o jogador quer transformar farm em poder imediato.`);
        if (relatedArmorEntries.length > 0) {
          lines.push(`Ela conversa especialmente bem com ${formatEntryTitleList(relatedArmorEntries, 3)}, porque essas paginas pertencem ao mesmo pacote de progressao.`);
        }
        if (recipe.ingredients.length > 0) {
          lines.push(`A propria receita mostra quais recursos precisam ser guardados para priorizar essa arma sobre outras escolhas do mesmo tier.`);
        }
        break;
      case "armor":
        lines.push(`O set muda bastante de funcao conforme o helm escolhido, entao a pagina dele serve tanto para leitura de defesa quanto para comparacao entre estilos de jogo.`);
        if (relatedBuffEntries.length > 0) {
          lines.push(`Os buffs ligados, como ${formatEntryTitleList(relatedBuffEntries, 3)}, ajudam a explicar o valor total do conjunto depois que ele entra em combate.`);
        }
        break;
      case "accessories":
        lines.push(`${title} vale principalmente em lutas com muita pressao de reposicionamento, porque a pagina descreve um acessorio que resolve defesa e movimento ao mesmo tempo.`);
        if (relatedBuffEntries.length > 0) {
          lines.push(`O acessorio tambem estende seu valor atraves de ${formatEntryTitleList(relatedBuffEntries, 3)}.`);
        }
        break;
      case "consumables":
        lines.push(`${title} entra melhor em tentativas longas, repeticao de boss e janelas em que sobreviver por mais alguns segundos ja muda a consistencia da luta.`);
        if (relatedBuffEntries.length > 0) {
          lines.push(`Depois do uso, a entrada ainda se desdobra em ${formatEntryTitleList(relatedBuffEntries, 3)}, entao o efeito real do consumivel vai alem da cura imediata.`);
        }
        break;
      case "buffs":
        lines.push(`${title} deve ser lido como extensao da fonte que o ativa, nao como efeito isolado. Em termos praticos, ele ajuda a explicar por que um item ou set funciona tao bem em campo.`);
        if (relatedEntries.length > 0) {
          lines.push(`A melhor leitura de uso continua sendo junto de ${formatEntryTitleList(relatedEntries, 3)}.`);
        }
        break;
      case "summons":
        lines.push(`${title} e valioso porque controla o acesso ao encontro ligado a ele, permitindo repetir tentativas e planejar o farm em vez de depender de spawn aleatorio.`);
        if (relatedBossEntries.length > 0) {
          lines.push(`Hoje ele se conecta diretamente a ${formatEntryTitleList(relatedBossEntries, 3)}, entao a pagina tambem funciona como atalho para esse encontro.`);
        }
        break;
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(`Derrotar ${title} nao serve so para fechar uma luta: o encontro alimenta drops, crafts e paginas que sustentam o restante do tier atual.`);
        if (relatedEntries.length > 0) {
          lines.push(`As conexoes mais claras do loot e da progressao daqui hoje passam por ${formatEntryTitleList(relatedEntries, 4)}.`);
        }
        break;
      default:
        if (usedInEntries.length > 0) {
          lines.push(`${title} continua relevante porque ainda aparece em ${usedInEntries.length} receita(s) ou pagina(s) ligada(s) dentro da wiki.`);
        }
        break;
    }
  }
  else {
    if (usedInEntries.length > 0) {
      lines.push(`${title} stays relevant because it already appears in ${usedInEntries.length} tracked recipe or progression links across the wiki.`);
    }
    if (relatedEntries.length > 0) {
      lines.push(`Its closest synergy pages right now are ${formatEntryTitleList(relatedEntries, 3)}.`);
    }
  }

  return dedupeLines(lines);
}

function buildEntryProgressionItems(detailContext) {
  const { entry, content, summonEntry, summonContent, usedInEntries, relatedEntries, relatedBossEntries, progressionStage } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [];

  if (state.language === "pt-BR") {
    if (progressionStage) {
      lines.push(`A etapa mais natural para encaixar ${title} dentro do mod hoje e ${progressionStage}.`);
    }

    if (entry.category === "bosses" && relatedEntries.length > 0) {
      lines.push(`Uma vitoria aqui prepara o caminho para paginas como ${formatEntryTitleList(relatedEntries, 4)}.`);
    }

    if (entry.category === "summons" && relatedBossEntries.length > 0) {
      lines.push(`O fluxo mais direto e montar ${title}, abrir ${formatEntryTitleList(relatedBossEntries, 2)} e converter o loot desse encontro em upgrades.`);
    }

    if (entry.category === "materials" && relatedBossEntries.length > 0 && usedInEntries.length > 0) {
      lines.push(`A rota mais clara hoje e ${getLocalizedEntry(relatedBossEntries[0]).title ?? relatedBossEntries[0].id} -> ${title} -> ${formatEntryTitleList(usedInEntries, 3)}.`);
    }

    if (entry.category !== "bosses" && summonEntry) {
      lines.push(`A pagina da invocacao ${summonContent.title ?? summonEntry.id} ajuda a localizar onde esta entrada encosta nos encontros da wiki.`);
    }
  }
  else {
    if (progressionStage) {
      lines.push(`The clearest stage for ${title} in the current wiki is ${progressionStage}.`);
    }
    if (entry.category === "materials" && usedInEntries.length > 0) {
      lines.push(`The cleanest path currently looks like reward -> ${title} -> ${formatEntryTitleList(usedInEntries, 3)}.`);
    }
  }

  return dedupeLines(lines);
}

function buildEntryDropItems(detailContext) {
  const { entry, content, relatedEntries } = detailContext;
  const lines = [...(content.drops ?? [])];

  if (state.language === "pt-BR") {
    if (entry.category === "bosses" && relatedEntries.length > 0) {
      lines.push(`As paginas mais claramente alimentadas pelo loot atual deste encontro sao ${formatEntryTitleList(relatedEntries, 4)}.`);
    }
  }
  else if (entry.category === "bosses" && relatedEntries.length > 0) {
    lines.push(`The current reward loop of this encounter most clearly feeds pages like ${formatEntryTitleList(relatedEntries, 4)}.`);
  }

  return dedupeLines(lines);
}

function buildEntryPieceItems(detailContext) {
  const { entry, content } = detailContext;
  const lines = [...(content.pieces ?? [])];

  if (state.language === "pt-BR" && entry.category === "armor" && lines.length > 0) {
    lines.push(`As pecas listadas acima mostram como o set muda de funcao sem mudar a base de peito e pernas.`);
  }
  else if (entry.category === "armor" && lines.length > 0) {
    lines.push(`The listed pieces show how the set changes role while keeping the same chest and leg core.`);
  }

  return dedupeLines(lines);
}

function buildEntryNoteItems(detailContext) {
  const { entry, content, recipe, summonEntry, summonContent, usedInEntries, relatedEntries } = detailContext;
  const title = content.title ?? entry.id;
  const lines = [...(content.notes ?? [])];

  if (state.language === "pt-BR") {
    if (usedInEntries.length > 0) {
      lines.push(`A wiki ja rastreia ${usedInEntries.length} uso(s) direto(s) para ${title}, entao esta pagina pode funcionar como referencia central sempre que novos crafts forem entrando.`);
    }

    if (relatedEntries.length > 0) {
      lines.push(`Entre as conexoes mais importantes desta entrada hoje estao ${formatEntryTitleList(relatedEntries, 4)}.`);
    }

    if (recipe.stations.length > 0 && entry.category !== "summons") {
      lines.push(`Toda a montagem registrada para esta entrada passa por ${formatStationLabelList(recipe.stations, 2)}.`);
    }

    if (summonEntry && isEncounterCategory(entry.category)) {
      lines.push(`Ler ${summonContent.title ?? summonEntry.id} junto desta pagina ajuda a fechar o contexto completo do spawn e da preparacao da luta.`);
    }
  }
  else {
    if (usedInEntries.length > 0) {
      lines.push(`The wiki already tracks ${usedInEntries.length} direct follow-up use(s) for ${title}, so this page can act as a central reference as more crafts are added.`);
    }

    if (relatedEntries.length > 0) {
      lines.push(`Important linked pages currently include ${formatEntryTitleList(relatedEntries, 4)}.`);
    }
  }

  return dedupeLines(lines);
}

function buildEntryTacticItems(detailContext) {
  const { entry, content, relatedBuffEntries } = detailContext;
  const lines = [...(content.tactics ?? [])];

  if (state.language === "pt-BR") {
    switch (entry.category) {
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(`Vale tratar esta pagina como um guia de preparacao: invocacao, espaco de luta, recompensas e leitura de fases costumam importar tanto quanto os numeros do boss.`);
        break;
      case "weapons":
        lines.push(`Como arma de tier avancado, ela rende melhor quando o jogador ja consegue sustentar a janela de dano para explorar o padrao principal do disparo.`);
        break;
      case "armor":
        lines.push(`A decisao mais importante em combate costuma ser qual helm fecha a build, porque e isso que transforma o set em dano, sustain ou pressao por classe.`);
        break;
      case "accessories":
        lines.push(`O melhor valor desta entrada costuma aparecer quando o efeito ativo e guardado para reposicionamento ou para corrigir uma troca ruim de arena.`);
        break;
      case "consumables":
        lines.push(`Use esse tipo de consumivel em lutas longas ou em sequencias de tentativa, quando a cura imediata e os buffs extras realmente conseguem mudar a consistencia do encontro.`);
        break;
      case "buffs":
        lines.push(`Como buff, ele vale mais quando o jogador entende o timing da fonte que o aplica e aproveita a janela extra de defesa, regen ou mobilidade.`);
        if (relatedBuffEntries.length > 0) {
          lines.push(`Se houver outros buffs ligados a mesma linha, vale compara-los para entender que parte do pacote vem de cada efeito.`);
        }
        break;
      case "summons":
        lines.push(`A melhor forma de usar uma invocacao como esta e planejar o custo por tentativa antes do farm, para que o encontro possa ser repetido sem travar a progressao.`);
        break;
      default:
        break;
    }
  }
  else {
    switch (entry.category) {
      case "bosses":
      case "superbosses":
      case "minibosses":
        lines.push(`Treat this page as a prep guide as much as a stat sheet: summon timing, arena space, rewards and phase reading all matter.`);
        break;
      case "weapons":
        lines.push(`This kind of advanced-tier weapon performs best once the player can reliably hold its main damage window.`);
        break;
      case "buffs":
        lines.push(`As a buff, it matters most when the player understands the timing of the source that applies it.`);
        break;
      default:
        break;
    }
  }

  return dedupeLines(lines);
}

function renderLinkedEntriesBlock(label, entries) {
  if (!label || !entries || entries.length === 0) {
    return "";
  }

  return `
    <div class="content-block">
      <h3>${label}</h3>
      <div class="entry-inline-list">${entries.map((entry) => renderLinkedEntryRow(entry)).join("")}</div>
    </div>
  `;
}

function renderLinkedEntryRow(entry) {
  const content = getLocalizedEntry(entry);
  const asset = getEntryDisplayAsset(entry);
  const subtitle = String(content.subtitle ?? "").trim();
  const summary = String(content.summary ?? content.overview ?? "").trim();

  return `
    <a class="entry-inline-row entry-inline-row--detail" href="${buildPageUrl("entry", { entry: entry.id })}">
      <img class="entry-inline-row__image" src="${escapeHtml(asset.imageUrl)}" alt="${escapeHtml(content.title ?? entry.id)}">
      <div class="entry-inline-row__body">
        <strong>${escapeHtml(content.title ?? entry.id)}</strong>
        ${subtitle ? `<small class="entry-inline-row__subtitle">${escapeHtml(subtitle)}</small>` : ""}
        ${summary ? `<small class="entry-inline-row__summary">${escapeHtml(summary)}</small>` : ""}
      </div>
    </a>
  `;
}

function getEntryProgressionStage(entry, relatedEntries = [], summonEntry = null) {
  const groupKey = resolveProgressionGroupKey(entry)
    || resolveProgressionGroupKey(summonEntry)
    || relatedEntries.map((candidate) => resolveProgressionGroupKey(candidate)).find(Boolean);

  if (!groupKey) {
    return "";
  }

  const progressionCopy = getCopy().progression.groups[groupKey];
  return progressionCopy?.title ?? humanizeCategory(groupKey);
}

function resolveProgressionGroupKey(entry) {
  if (!entry) {
    return "";
  }

  const meta = getEntryMeta(entry);
  if (meta.progressionGroup) {
    return meta.progressionGroup;
  }

  if (entry.id === "crystaline-devourer") {
    return "post_moonlord";
  }

  if ((entry.related ?? []).includes("crystaline-devourer")) {
    return "post_moonlord";
  }

  return "";
}

function formatEntryTitleList(entries, limit = 3) {
  const titles = entries
    .slice(0, limit)
    .map((entry) => getLocalizedEntry(entry).title ?? entry.id)
    .filter(Boolean);

  return joinLocalizedList(titles);
}

function formatIngredientLabelList(ingredients, limit = 4) {
  return joinLocalizedList(
    ingredients
      .slice(0, limit)
      .map((ingredient) => ingredient.label)
      .filter(Boolean)
  );
}

function formatStationLabelList(stations, limit = 2) {
  return joinLocalizedList(
    stations
      .slice(0, limit)
      .map((station) => station.label)
      .filter(Boolean)
  );
}

function joinLocalizedList(values) {
  const clean = values.filter(Boolean);
  if (clean.length === 0) {
    return "";
  }

  if (clean.length === 1) {
    return clean[0];
  }

  const joiner = state.language === "pt-BR"
    ? " e "
    : state.language === "es"
      ? " y "
      : state.language === "ru"
        ? " и "
        : " and ";

  if (clean.length === 2) {
    return `${clean[0]}${joiner}${clean[1]}`;
  }

  return `${clean.slice(0, -1).join(", ")}${joiner}${clean.at(-1)}`;
}

function dedupeLines(lines) {
  const seen = new Set();
  const output = [];

  lines
    .flat()
    .map((item) => String(item ?? "").trim())
    .filter(Boolean)
    .forEach((item) => {
      const key = normalize(item);
      if (seen.has(key)) {
        return;
      }

      seen.add(key);
      output.push(item);
    });

  return output;
}

function isEncounterCategory(category) {
  return ["bosses", "superbosses", "minibosses", "mobs"].includes(category);
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
