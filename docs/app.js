import { entries, languageOptions, siteConfig, uiCopy } from "./data.js";
import { frontierEntries, frontierSection, frontierUiCopy } from "./orespawn-data.js";

const elements = {
  sidebar: document.querySelector("#sidebar"),
  hero: document.querySelector("#home"),
  download: document.querySelector("#download"),
  wiki: document.querySelector("#wiki"),
  frontier: document.querySelector("#frontier"),
  bosses: document.querySelector("#bosses"),
  roadmap: document.querySelector("#roadmap"),
  footer: document.querySelector("#site-footer"),
  mobileBrand: document.querySelector("#mobile-brand"),
  mobileNavToggle: document.querySelector("#mobile-nav-toggle"),
  sidebarOverlay: document.querySelector("#sidebar-overlay"),
  metaDescription: document.querySelector("#meta-description")
};

const allEntries = [...entries, ...frontierEntries];
const allCategories = Array.from(new Set(allEntries.map((entry) => entry.category)));

const state = {
  language: siteConfig.defaultLanguage,
  category: "all",
  search: "",
  selectedEntryId: siteConfig.defaultEntryId,
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
  const entryId = url.searchParams.get("entry");

  if (languageOptions.some((option) => option.code === language)) {
    state.language = language;
  }

  if (allEntries.some((entry) => entry.id === entryId)) {
    state.selectedEntryId = entryId;
  }
}

function bindGlobalEvents() {
  elements.mobileNavToggle.addEventListener("click", () => {
    toggleSidebar(!state.sidebarOpen);
  });

  elements.sidebarOverlay.addEventListener("click", () => {
    toggleSidebar(false);
  });

  window.addEventListener("resize", () => {
    if (window.innerWidth > 980 && state.sidebarOpen) {
      toggleSidebar(false);
    }
  });
}

function toggleSidebar(isOpen) {
  state.sidebarOpen = isOpen;
  document.body.classList.toggle("sidebar-open", isOpen);
  elements.sidebarOverlay.hidden = !isOpen;
}

function render() {
  syncMetadata();
  renderSidebar();
  renderHero();
  renderDownload();
  renderWiki();
  renderFrontier();
  renderBossSpotlight();
  renderRoadmap();
  renderFooter();
  syncUrl();
}

function syncMetadata() {
  const copy = getCopy();
  document.documentElement.lang = state.language;
  document.title = `${copy.siteName} | ${copy.metaTitle}`;
  elements.mobileBrand.textContent = copy.siteName;
  elements.metaDescription.setAttribute("content", copy.metaDescription);
}

function renderSidebar() {
  const copy = getCopy();
  const languageButtons = languageOptions.map((option) => `
    <button
      class="language-chip ${option.code === state.language ? "is-active" : ""}"
      type="button"
      data-language="${option.code}"
      aria-pressed="${option.code === state.language}"
    >
      ${option.label}
    </button>
  `).join("");

  const navItems = [
    { href: "#home", label: copy.nav.overview },
    { href: "#download", label: copy.nav.download },
    { href: "#wiki", label: copy.nav.explorer },
    { href: "#frontier", label: getFrontierCopy().nav.frontier },
    { href: "#bosses", label: copy.nav.boss },
    { href: "#roadmap", label: copy.nav.roadmap }
  ].map((item) => `<a class="sidebar-nav__link" href="${item.href}">${item.label}</a>`).join("");

  elements.sidebar.innerHTML = `
    <div class="sidebar__brand">
      <div class="brand-mark">
        <img src="./assets/images/icon.png" alt="Chaotic Dimensions icon">
      </div>
      <div>
        <p class="eyebrow">${copy.sidebar.eyebrow}</p>
        <h1>${copy.siteName}</h1>
        <p class="sidebar__summary">${copy.sidebar.summary}</p>
      </div>
    </div>

    <div class="sidebar__group">
      <p class="sidebar__label">${copy.sidebar.languages}</p>
      <div class="language-grid">${languageButtons}</div>
    </div>

    <nav class="sidebar-nav sidebar__group">
      <p class="sidebar__label">${copy.sidebar.navigation}</p>
      ${navItems}
    </nav>

    <div class="sidebar__group sidebar-actions">
      <a class="button button--ghost" href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">${copy.actions.github}</a>
      <a class="button button--ghost" href="${siteConfig.releasesUrl}" target="_blank" rel="noreferrer">${copy.actions.releases}</a>
      <a class="button button--ghost" href="${siteConfig.pagesUrl}" target="_blank" rel="noreferrer">${copy.actions.liveSite}</a>
    </div>

    <p class="sidebar__note">${copy.sidebar.note}</p>
  `;

  elements.sidebar.querySelectorAll("[data-language]").forEach((button) => {
    button.addEventListener("click", () => {
      state.language = button.dataset.language;
      render();
    });
  });

  elements.sidebar.querySelectorAll(".sidebar-nav__link").forEach((link) => {
    link.addEventListener("click", () => {
      if (window.innerWidth <= 980) {
        toggleSidebar(false);
      }
    });
  });
}

function renderHero() {
  const copy = getCopy();
  const metricCards = [
    { value: `${allEntries.length}`, label: copy.hero.metrics.entries },
    { value: `${allCategories.length}`, label: copy.hero.metrics.categories },
    { value: `${languageOptions.length}`, label: copy.hero.metrics.languages },
    { value: "24/7", label: copy.hero.metrics.hosting }
  ].map((metric) => `
    <article class="metric-card glass-card">
      <strong>${metric.value}</strong>
      <span>${metric.label}</span>
    </article>
  `).join("");

  elements.hero.innerHTML = `
    <div class="hero-card glass-card">
      <div class="hero-copy">
        <p class="eyebrow">${copy.hero.eyebrow}</p>
        <h2>${copy.hero.title}</h2>
        <p class="hero-lead">${copy.hero.lead}</p>
        <p class="hero-body">${copy.hero.body}</p>

        <div class="hero-actions">
          <a class="button button--primary" href="#wiki">${copy.actions.browseWiki}</a>
          <a class="button button--secondary" href="#download">${copy.actions.downloadGuide}</a>
          <a class="button button--ghost" href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">${copy.actions.github}</a>
        </div>

        <div class="metrics-grid">
          ${metricCards}
        </div>
      </div>

      <div class="hero-media">
        <div class="hero-banner">
          <img src="./assets/images/banner.png" alt="${copy.hero.bannerAlt}">
        </div>
        <div class="hero-gallery">
          <div class="hero-gallery__card glass-card">
            <p class="eyebrow">${copy.hero.galleryLabel}</p>
            <img src="./assets/images/gallery/crystaline-devour-title.png" alt="${copy.hero.galleryAltOne}">
          </div>
          <div class="hero-gallery__stack">
            <img class="glass-card" src="./assets/images/gallery/crystaline-devour-ring.png" alt="${copy.hero.galleryAltTwo}">
            <img class="glass-card" src="./assets/images/gallery/crystaline-devour-blaster.png" alt="${copy.hero.galleryAltThree}">
          </div>
        </div>
      </div>
    </div>
  `;
}

function renderDownload() {
  const copy = getCopy();

  const cards = [
    {
      title: copy.download.cards.pages.title,
      body: copy.download.cards.pages.body,
      href: siteConfig.pagesUrl,
      label: copy.download.cards.pages.action
    },
    {
      title: copy.download.cards.releases.title,
      body: copy.download.cards.releases.body,
      href: siteConfig.releasesUrl,
      label: copy.download.cards.releases.action
    },
    {
      title: copy.download.cards.source.title,
      body: copy.download.cards.source.body,
      href: siteConfig.repoUrl,
      label: copy.download.cards.source.action
    }
  ].map((card) => `
    <article class="download-card glass-card">
      <h3>${card.title}</h3>
      <p>${card.body}</p>
      <a class="button button--secondary" href="${card.href}" target="_blank" rel="noreferrer">${card.label}</a>
    </article>
  `).join("");

  const steps = copy.download.steps.map((step) => `<li>${step}</li>`).join("");
  const bullets = copy.download.notes.map((note) => `<li>${note}</li>`).join("");

  elements.download.innerHTML = `
    <div class="section-heading">
      <p class="eyebrow">${copy.download.eyebrow}</p>
      <h2>${copy.download.title}</h2>
      <p>${copy.download.intro}</p>
    </div>

    <div class="download-grid">
      ${cards}
    </div>

    <div class="guide-grid">
      <article class="glass-card guide-card">
        <h3>${copy.download.stepsTitle}</h3>
        <ol class="guide-list">${steps}</ol>
      </article>

      <article class="glass-card guide-card">
        <h3>${copy.download.sourceTitle}</h3>
        <p>${copy.download.sourceIntro}</p>
        <pre><code>git clone ${siteConfig.repoUrl}.git</code></pre>
        <pre><code>dotnet build ChaoticDimensions.csproj</code></pre>
        <ul class="note-list">${bullets}</ul>
      </article>
    </div>
  `;
}

function renderWiki() {
  const copy = getCopy();
  const visibleEntries = getVisibleEntries();

  if (!visibleEntries.some((entry) => entry.id === state.selectedEntryId)) {
    state.selectedEntryId = visibleEntries[0]?.id ?? siteConfig.defaultEntryId;
  }

  const categoryButtons = [
    {
      key: "all",
      label: copy.categories.all,
      count: allEntries.length
    },
    ...allCategories.map((category) => ({
      key: category,
      label: getCategoryLabel(category),
      count: allEntries.filter((entry) => entry.category === category).length
    }))
  ].map((category) => `
    <button
      type="button"
      class="filter-chip ${category.key === state.category ? "is-active" : ""}"
      data-category="${category.key}"
      aria-pressed="${category.key === state.category}"
    >
      <span>${category.label}</span>
      <small>${category.count}</small>
    </button>
  `).join("");

  const cards = visibleEntries.map((entry) => renderEntryCard(entry)).join("");
  const selectedEntry = getEntryById(state.selectedEntryId);
  const detailMarkup = selectedEntry ? renderEntryDetail(selectedEntry) : `<div class="empty-state glass-card">${copy.wiki.empty}</div>`;

  elements.wiki.innerHTML = `
    <div class="section-heading">
      <p class="eyebrow">${copy.wiki.eyebrow}</p>
      <h2>${copy.wiki.title}</h2>
      <p>${copy.wiki.intro}</p>
    </div>

    <div class="wiki-shell">
      <div class="wiki-browser">
        <div class="glass-card browser-controls">
          <label class="search-field">
            <span>${copy.wiki.searchLabel}</span>
            <input id="search-input" type="search" value="${escapeHtml(state.search)}" placeholder="${copy.wiki.searchPlaceholder}">
          </label>
          <div class="filter-row">
            ${categoryButtons}
          </div>
        </div>

        <div class="entry-grid">
          ${cards || `<div class="empty-state glass-card">${copy.wiki.empty}</div>`}
        </div>
      </div>

      <aside class="detail-panel">
        ${detailMarkup}
      </aside>
    </div>
  `;

  const searchInput = elements.wiki.querySelector("#search-input");
  searchInput?.addEventListener("input", (event) => {
    state.search = event.target.value;
    renderWiki();
    syncUrl();
  });

  elements.wiki.querySelectorAll("[data-category]").forEach((button) => {
    button.addEventListener("click", () => {
      state.category = button.dataset.category;
      renderWiki();
      syncUrl();
    });
  });

  elements.wiki.querySelectorAll("[data-entry-id]").forEach((button) => {
    button.addEventListener("click", () => {
      state.selectedEntryId = button.dataset.entryId;
      renderWiki();
      syncUrl();
    });
  });
}

function renderFrontier() {
  const frontierCopy = getFrontierCopy().frontier;
  const rosterEntries = frontierSection.rosterIds.map((entryId) => getEntryById(entryId)).filter(Boolean);
  const supportEntries = frontierSection.supportIds.map((entryId) => getEntryById(entryId)).filter(Boolean);
  const biomeCount = new Set(["beach", "forest-jungle", "desert", "snow"]).size;
  const gateCount = new Set(
    rosterEntries.map((entry) => getLocalizedEntry(entry).facts?.[0]).filter(Boolean)
  ).size;

  const metricCards = [
    { value: `${rosterEntries.length}`, label: frontierCopy.metrics.mobs },
    { value: `${supportEntries.filter((entry) => entry.category === "summons").length}`, label: frontierCopy.metrics.summons },
    { value: `${gateCount}`, label: frontierCopy.metrics.gates },
    { value: `${biomeCount}`, label: frontierCopy.metrics.biomes }
  ].map((metric) => `
    <article class="metric-card glass-card">
      <strong>${metric.value}</strong>
      <span>${metric.label}</span>
    </article>
  `).join("");

  const rosterCards = rosterEntries.map((entry) => {
    const content = getLocalizedEntry(entry);
    const previewFacts = (content.facts ?? []).slice(0, 3).map((fact) => `<li>${fact}</li>`).join("");

    return `
      <article class="frontier-card glass-card">
        <div class="frontier-card__media">
          <img class="sprite-art sprite-art--large" src="${entry.image}" alt="${content.title}">
        </div>
        <div class="frontier-card__body">
          <span class="entry-badge" data-category="${entry.category}">${getCategoryLabel(entry.category)}</span>
          <h3>${content.title}</h3>
          <p>${content.summary}</p>
          <ul class="fact-preview">${previewFacts}</ul>
          <button class="button button--secondary" type="button" data-frontier-entry="${entry.id}">${frontierCopy.openEntry}</button>
        </div>
      </article>
    `;
  }).join("");

  const supportCards = supportEntries.map((entry) => {
    const content = getLocalizedEntry(entry);
    const listSource = content.crafting ?? content.obtain ?? content.facts ?? [];
    const previewFacts = listSource.slice(0, 3).map((item) => `<li>${item}</li>`).join("");

    return `
      <article class="support-card glass-card">
        <div class="support-card__head">
          <div class="entry-card__media support-card__media">
            <img class="sprite-art" src="${entry.image}" alt="${content.title}">
          </div>
          <div>
            <span class="entry-badge" data-category="${entry.category}">${getCategoryLabel(entry.category)}</span>
            <h3>${content.title}</h3>
            <p>${content.summary}</p>
          </div>
        </div>
        <ul class="fact-preview">${previewFacts}</ul>
        <button class="button button--ghost" type="button" data-frontier-entry="${entry.id}">${frontierCopy.openEntry}</button>
      </article>
    `;
  }).join("");

  elements.frontier.innerHTML = `
    <div class="section-heading">
      <p class="eyebrow">${frontierCopy.eyebrow}</p>
      <h2>${frontierCopy.title}</h2>
      <p>${frontierCopy.intro}</p>
    </div>

    <div class="frontier-metrics">
      ${metricCards}
    </div>

    <div class="section-heading frontier-heading">
      <h3>${frontierCopy.rosterTitle}</h3>
      <p>${frontierCopy.rosterIntro}</p>
    </div>

    <div class="frontier-grid">
      ${rosterCards}
    </div>

    <div class="section-heading frontier-heading">
      <h3>${frontierCopy.supportTitle}</h3>
      <p>${frontierCopy.supportIntro}</p>
    </div>

    <div class="support-grid">
      ${supportCards}
    </div>
  `;

  elements.frontier.querySelectorAll("[data-frontier-entry]").forEach((button) => {
    button.addEventListener("click", () => {
      state.selectedEntryId = button.dataset.frontierEntry;
      renderWiki();
      syncUrl();
      document.querySelector("#wiki")?.scrollIntoView({ behavior: "smooth", block: "start" });
    });
  });
}

function renderBossSpotlight() {
  const copy = getCopy();
  const bossEntry = getEntryById("crystaline-devourer");
  const bossText = getLocalizedEntry(bossEntry);

  const phases = copy.spotlight.phases.map((phase) => `
    <article class="phase-card glass-card">
      <h3>${phase.title}</h3>
      <p>${phase.body}</p>
    </article>
  `).join("");

  const facts = bossText.facts.map((fact) => `<li>${fact}</li>`).join("");
  const drops = bossText.drops.map((drop) => `<li>${drop}</li>`).join("");

  elements.bosses.innerHTML = `
    <div class="section-heading">
      <p class="eyebrow">${copy.spotlight.eyebrow}</p>
      <h2>${copy.spotlight.title}</h2>
      <p>${copy.spotlight.intro}</p>
    </div>

    <div class="spotlight-layout">
      <article class="glass-card spotlight-gallery">
        <img class="spotlight-gallery__main" src="./assets/images/gallery/crystaline-devour-ring.png" alt="${copy.spotlight.galleryAltMain}">
        <div class="spotlight-gallery__thumbs">
          <img src="./assets/images/gallery/crystaline-devour-title.png" alt="${copy.spotlight.galleryAltOne}">
          <img src="./assets/images/gallery/crystaline-devour-blaster.png" alt="${copy.spotlight.galleryAltTwo}">
        </div>
      </article>

      <article class="glass-card spotlight-copy">
        <p class="eyebrow">${bossText.subtitle}</p>
        <h3>${bossText.title}</h3>
        <p>${bossText.overview}</p>

        <div class="detail-columns">
          <div>
            <h4>${copy.wiki.sections.facts}</h4>
            <ul>${facts}</ul>
          </div>
          <div>
            <h4>${copy.wiki.sections.drops}</h4>
            <ul>${drops}</ul>
          </div>
        </div>
      </article>
    </div>

    <div class="phase-grid">
      ${phases}
    </div>
  `;
}

function renderRoadmap() {
  const copy = getCopy();

  const cards = copy.roadmap.cards.map((card) => `
    <article class="roadmap-card glass-card">
      <p class="eyebrow">${card.eyebrow}</p>
      <h3>${card.title}</h3>
      <p>${card.body}</p>
    </article>
  `).join("");

  elements.roadmap.innerHTML = `
    <div class="section-heading">
      <p class="eyebrow">${copy.roadmap.eyebrow}</p>
      <h2>${copy.roadmap.title}</h2>
      <p>${copy.roadmap.intro}</p>
    </div>

    <div class="roadmap-grid">
      ${cards}
    </div>
  `;
}

function renderFooter() {
  const copy = getCopy();
  elements.footer.innerHTML = `
    <div>
      <p class="eyebrow">${copy.footer.eyebrow}</p>
      <h2>${copy.footer.title}</h2>
      <p>${copy.footer.body}</p>
    </div>

    <div class="footer-actions">
      <a class="button button--secondary" href="${siteConfig.repoUrl}" target="_blank" rel="noreferrer">${copy.actions.github}</a>
      <a class="button button--ghost" href="${siteConfig.releasesUrl}" target="_blank" rel="noreferrer">${copy.actions.releases}</a>
      <a class="button button--ghost" href="#wiki">${copy.actions.browseWiki}</a>
    </div>
  `;
}

function renderEntryCard(entry) {
  const copy = getCopy();
  const content = getLocalizedEntry(entry);
  const previewFacts = content.facts.slice(0, 2).map((fact) => `<li>${fact}</li>`).join("");

  return `
    <button
      type="button"
      class="entry-card glass-card ${entry.id === state.selectedEntryId ? "is-selected" : ""}"
      data-entry-id="${entry.id}"
    >
      <div class="entry-card__media">
        <img class="sprite-art" src="${entry.image}" alt="${content.title}">
      </div>
      <div class="entry-card__body">
        <span class="entry-badge" data-category="${entry.category}">${getCategoryLabel(entry.category)}</span>
        <h3>${content.title}</h3>
        <p>${content.summary}</p>
        <ul class="fact-preview">${previewFacts}</ul>
      </div>
    </button>
  `;
}

function renderEntryDetail(entry) {
  const copy = getCopy();
  const content = getLocalizedEntry(entry);
  const sections = ["facts", "obtain", "crafting", "drops", "pieces", "notes", "tactics"]
    .map((key) => renderListSection(copy.wiki.sections[key], content[key]))
    .join("");

  const relatedMarkup = (entry.related?.length ?? 0) > 0
    ? `
      <div class="detail-section">
        <h4>${copy.wiki.sections.related}</h4>
        <div class="related-chips">
          ${entry.related.map((relatedId) => {
            const relatedEntry = getEntryById(relatedId);
            const relatedContent = getLocalizedEntry(relatedEntry);
            return `<button type="button" class="related-chip" data-entry-id="${relatedId}">${relatedContent.title}</button>`;
          }).join("")}
        </div>
      </div>
    `
    : "";

  const galleryMarkup = entry.gallery?.length
    ? `
      <div class="gallery-strip">
        ${entry.gallery.map((image) => {
          const alt = image.alt?.[state.language] ?? image.alt?.[siteConfig.defaultLanguage] ?? content.title;
          return `<img src="${image.src}" alt="${alt}">`;
        }).join("")}
      </div>
    `
    : "";

  const bannerMedia = entry.banner
    ? `<img class="detail-banner" src="${entry.banner}" alt="${content.title}">`
    : `<img class="sprite-art sprite-art--large" src="${entry.image}" alt="${content.title}">`;

  return `
    <article class="glass-card detail-card">
      <div class="detail-hero">
        <div class="detail-copy">
          <span class="entry-badge" data-category="${entry.category}">${getCategoryLabel(entry.category)}</span>
          <h3>${content.title}</h3>
          <p class="detail-subtitle">${content.subtitle}</p>
          <p>${content.overview}</p>
        </div>
        <div class="detail-media">
          ${bannerMedia}
        </div>
      </div>

      ${galleryMarkup}
      ${sections}
      ${relatedMarkup}
    </article>
  `;
}

function renderListSection(label, items) {
  if (!items || items.length === 0) {
    return "";
  }

  const list = items.map((item) => `<li>${item}</li>`).join("");
  return `
    <div class="detail-section">
      <h4>${label}</h4>
      <ul>${list}</ul>
    </div>
  `;
}

function getVisibleEntries() {
  const term = state.search.trim().toLowerCase();

  return allEntries.filter((entry) => {
    if (state.category !== "all" && entry.category !== state.category) {
      return false;
    }

    if (!term) {
      return true;
    }

    const content = getLocalizedEntry(entry);
    const haystack = [
      entry.id,
      content.title,
      content.subtitle,
      content.summary,
      content.overview,
      ...(content.facts ?? []),
      ...(content.obtain ?? []),
      ...(content.crafting ?? []),
      ...(content.drops ?? []),
      ...(content.notes ?? []),
      ...(content.tactics ?? [])
    ].join(" ").toLowerCase();

    return haystack.includes(term);
  });
}

function getLocalizedEntry(entry) {
  return entry.content[state.language] ?? entry.content[siteConfig.defaultLanguage];
}

function getEntryById(entryId) {
  return allEntries.find((entry) => entry.id === entryId);
}

function getCopy() {
  return uiCopy[state.language] ?? uiCopy[siteConfig.defaultLanguage];
}

function getFrontierCopy() {
  return frontierUiCopy[state.language] ?? frontierUiCopy[siteConfig.defaultLanguage];
}

function getCategoryLabel(category) {
  const copy = getCopy();
  const frontierCopy = getFrontierCopy();
  return copy.categories[category] ?? frontierCopy.categories?.[category] ?? category;
}

function syncUrl() {
  const url = new URL(window.location.href);
  url.searchParams.set("lang", state.language);
  url.searchParams.set("entry", state.selectedEntryId);
  history.replaceState({}, "", `${url.pathname}${url.search}${url.hash}`);
}

function escapeHtml(value) {
  return value
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#39;");
}
