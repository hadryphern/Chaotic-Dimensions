import { isSupabaseConfigured, supabaseConfig } from "./supabase.config.js";

export const backendState = {
  configured: false,
  enabled: false,
  ready: false,
  error: "",
  user: null,
  profile: null,
  isAdmin: false,
  publishedEntries: [],
  commentsByEntry: {},
  adminComments: [],
  authError: "",
  authMessage: "",
  commentError: "",
  commentMessage: "",
  entryError: "",
  entryMessage: "",
  uploadError: "",
  uploadMessage: ""
};

let supabaseClient = null;
let listener = () => {};

export function setBackendListener(callback) {
  listener = callback;
}

export function getSupabaseClient() {
  return supabaseClient;
}

export async function initBackend() {
  backendState.configured = isSupabaseConfigured();

  if (!backendState.configured) {
    backendState.ready = true;
    notify();
    return;
  }

  if (!window.supabase?.createClient) {
    backendState.error = "Supabase client could not be loaded in the browser.";
    backendState.ready = true;
    notify();
    return;
  }

  supabaseClient = window.supabase.createClient(supabaseConfig.url, supabaseConfig.anonKey, {
    auth: {
      persistSession: true,
      autoRefreshToken: true,
      detectSessionInUrl: true
    }
  });

  backendState.enabled = true;

  const sessionResult = await supabaseClient.auth.getSession();
  await applySession(sessionResult.data.session);

  supabaseClient.auth.onAuthStateChange((_event, session) => {
    applySession(session);
  });

  await loadPublishedEntries();

  backendState.ready = true;
  notify();
}

export async function signInGuest(displayName) {
  if (!supabaseClient) {
    return;
  }

  backendState.authError = "";
  backendState.authMessage = "";

  const cleanName = String(displayName ?? "").trim();
  if (cleanName.length < 2) {
    backendState.authError = "Choose a name with at least 2 characters.";
    notify();
    return;
  }

  localStorage.setItem("cd_pending_display_name", cleanName);

  const result = await supabaseClient.auth.signInAnonymously();
  if (result.error) {
    backendState.authError = result.error.message;
    notify();
    return;
  }

  await ensureProfile(cleanName);
  backendState.authMessage = "Quick account created. You can already comment.";
  notify();
}

export async function signInAdmin(email, password) {
  if (!supabaseClient) {
    return;
  }

  backendState.authError = "";
  backendState.authMessage = "";

  const result = await supabaseClient.auth.signInWithPassword({
    email: String(email ?? "").trim(),
    password: String(password ?? "")
  });

  if (result.error) {
    backendState.authError = result.error.message;
    notify();
    return;
  }

  backendState.authMessage = "Admin login successful.";
  notify();
}

export async function registerEditorAccount(displayName, email, password) {
  if (!supabaseClient) {
    return;
  }

  backendState.authError = "";
  backendState.authMessage = "";

  const cleanName = String(displayName ?? "").trim() || String(email ?? "").trim().split("@")[0] || "Editor";

  const result = await supabaseClient.auth.signUp({
    email: String(email ?? "").trim(),
    password: String(password ?? ""),
    options: {
      data: {
        display_name: cleanName.slice(0, 40)
      }
    }
  });

  if (result.error) {
    backendState.authError = result.error.message;
    notify();
    return;
  }

  if (result.data.session) {
    await ensureProfile(cleanName);
  }

  backendState.authMessage = result.data.session
    ? "Editor account created. Promote it to admin in the SQL step."
    : "Editor account created. Confirm the email if required, then promote it to admin in the SQL step.";
  notify();
}

export async function signOutUser() {
  if (!supabaseClient) {
    return;
  }

  backendState.authError = "";
  backendState.authMessage = "";

  const result = await supabaseClient.auth.signOut();
  if (result.error) {
    backendState.authError = result.error.message;
  }
  else {
    backendState.authMessage = "You have been signed out.";
  }

  notify();
}

export async function loadPublishedEntries() {
  if (!supabaseClient) {
    return [];
  }

  let query = supabaseClient
    .from("wiki_entries")
    .select("id, category, image_url, related_ids, sort_order, is_published, content_json")
    .order("sort_order", { ascending: true })
    .order("updated_at", { ascending: false });

  if (!backendState.isAdmin) {
    query = query.eq("is_published", true);
  }

  const result = await query;

  if (result.error) {
    backendState.error = result.error.message;
    notify();
    return [];
  }

  backendState.publishedEntries = (result.data ?? []).map((row) => ({
    id: row.id,
    category: row.category,
    image: row.image_url || "./assets/images/favicon.png",
    related: row.related_ids ?? [],
    sortOrder: row.sort_order ?? 0,
    isPublished: row.is_published ?? false,
    content: row.content_json ?? {}
  }));

  notify();
  return backendState.publishedEntries;
}

export async function loadComments(entryId) {
  if (!supabaseClient || !entryId) {
    return [];
  }

  backendState.commentsByEntry[entryId] = {
    items: backendState.commentsByEntry[entryId]?.items ?? [],
    loading: true,
    error: ""
  };
  notify();

  const result = await supabaseClient
    .from("wiki_comments")
    .select("id, entry_id, display_name, body, is_hidden, created_at")
    .eq("entry_id", entryId)
    .order("created_at", { ascending: false });

  if (result.error) {
    backendState.commentsByEntry[entryId] = {
      items: [],
      loading: false,
      error: result.error.message
    };
    notify();
    return [];
  }

  backendState.commentsByEntry[entryId] = {
    items: result.data ?? [],
    loading: false,
    error: ""
  };

  notify();
  return backendState.commentsByEntry[entryId].items;
}

export async function postComment(entryId, body) {
  if (!supabaseClient || !backendState.user || !backendState.profile) {
    backendState.commentError = "You need to sign in before commenting.";
    notify();
    return false;
  }

  const cleanBody = String(body ?? "").trim();
  if (cleanBody.length < 2) {
    backendState.commentError = "Write a longer comment before sending.";
    notify();
    return false;
  }

  backendState.commentError = "";
  backendState.commentMessage = "";

  const result = await supabaseClient
    .from("wiki_comments")
    .insert({
      entry_id: entryId,
      user_id: backendState.user.id,
      display_name: backendState.profile.display_name,
      body: cleanBody
    });

  if (result.error) {
    backendState.commentError = result.error.message;
    notify();
    return false;
  }

  backendState.commentMessage = "Comment published.";
  await loadComments(entryId);
  return true;
}

export async function saveWikiEntry(payload) {
  if (!supabaseClient || !backendState.isAdmin) {
    backendState.entryError = "Admin access is required to publish or edit entries.";
    notify();
    return false;
  }

  backendState.entryError = "";
  backendState.entryMessage = "";

  const result = await supabaseClient
    .from("wiki_entries")
    .upsert({
      ...payload,
      updated_by: backendState.user.id,
      created_by: payload.created_by ?? backendState.user.id
    }, { onConflict: "id" });

  if (result.error) {
    backendState.entryError = result.error.message;
    notify();
    return false;
  }

  backendState.entryMessage = "Entry saved to Supabase.";
  await loadPublishedEntries();
  notify();
  return true;
}

export async function uploadWikiAsset(file, pathPrefix = "entries") {
  if (!supabaseClient || !backendState.isAdmin) {
    backendState.uploadError = "Admin access is required to upload images.";
    notify();
    return null;
  }

  if (!file) {
    backendState.uploadError = "Choose an image before uploading.";
    notify();
    return null;
  }

  backendState.uploadError = "";
  backendState.uploadMessage = "";

  const safePrefix = String(pathPrefix ?? "entries").trim().replace(/^\/+|\/+$/g, "") || "entries";
  const safeName = file.name.replace(/[^a-zA-Z0-9._-]/g, "-");
  const objectPath = `${safePrefix}/${Date.now()}-${safeName}`;

  const uploadResult = await supabaseClient
    .storage
    .from(supabaseConfig.assetsBucket)
    .upload(objectPath, file, {
      cacheControl: "3600",
      upsert: true
    });

  if (uploadResult.error) {
    backendState.uploadError = uploadResult.error.message;
    notify();
    return null;
  }

  const publicUrl = supabaseClient
    .storage
    .from(supabaseConfig.assetsBucket)
    .getPublicUrl(objectPath)
    .data
    .publicUrl;

  backendState.uploadMessage = `Image uploaded: ${publicUrl}`;
  notify();
  return publicUrl;
}

export async function loadAdminComments() {
  if (!supabaseClient || !backendState.isAdmin) {
    backendState.adminComments = [];
    notify();
    return [];
  }

  const result = await supabaseClient
    .from("wiki_comments")
    .select("id, entry_id, display_name, body, is_hidden, created_at")
    .order("created_at", { ascending: false })
    .limit(20);

  if (result.error) {
    backendState.entryError = result.error.message;
    notify();
    return [];
  }

  backendState.adminComments = result.data ?? [];
  notify();
  return backendState.adminComments;
}

export async function setCommentHidden(commentId, isHidden) {
  if (!supabaseClient || !backendState.isAdmin) {
    return false;
  }

  const result = await supabaseClient
    .from("wiki_comments")
    .update({ is_hidden: isHidden })
    .eq("id", commentId);

  if (result.error) {
    backendState.entryError = result.error.message;
    notify();
    return false;
  }

  await loadAdminComments();
  return true;
}

async function applySession(session) {
  backendState.user = session?.user ?? null;
  backendState.profile = null;
  backendState.isAdmin = false;

  if (backendState.user) {
    await ensureProfile(localStorage.getItem("cd_pending_display_name"));
    await refreshProfile();
    if (backendState.isAdmin) {
      await loadAdminComments();
    }
  }
  else {
    backendState.adminComments = [];
  }

  notify();
}

async function ensureProfile(displayNameHint) {
  if (!supabaseClient || !backendState.user) {
    return;
  }

  const fallbackName = String(displayNameHint || backendState.user.user_metadata?.display_name || backendState.user.email?.split("@")[0] || "Traveler").trim();

  await supabaseClient
    .from("profiles")
    .upsert({
      id: backendState.user.id,
      display_name: fallbackName.slice(0, 40)
    }, { onConflict: "id" });

  localStorage.removeItem("cd_pending_display_name");
}

async function refreshProfile() {
  if (!supabaseClient || !backendState.user) {
    return;
  }

  const result = await supabaseClient
    .from("profiles")
    .select("id, display_name, role")
    .eq("id", backendState.user.id)
    .maybeSingle();

  if (!result.error) {
    backendState.profile = result.data;
    backendState.isAdmin = result.data?.role === "admin";
  }
}

function notify() {
  listener();
}
