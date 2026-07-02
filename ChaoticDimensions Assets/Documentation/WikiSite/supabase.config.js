export const supabaseConfig = {
  url: "https://zgxcqkutrcmlqbxyzxwj.supabase.co",
  anonKey: "sb_publishable_qV3i08A_pDOwQQ4EDU44Fg_uqaWj7at",
  assetsBucket: "wiki-assets"
};

export function isSupabaseConfigured() {
  return Boolean(supabaseConfig.url && supabaseConfig.anonKey);
}
