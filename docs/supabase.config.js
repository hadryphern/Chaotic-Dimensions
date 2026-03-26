export const supabaseConfig = {
  url: "",
  anonKey: "",
  assetsBucket: "wiki-assets"
};

export function isSupabaseConfigured() {
  return Boolean(supabaseConfig.url && supabaseConfig.anonKey);
}
