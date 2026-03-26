# Supabase Setup

1. Create a Supabase project.
2. Open the SQL Editor and run [supabase/wiki_schema.sql](/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/supabase/wiki_schema.sql).
3. Open [supabase.config.js](/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/supabase.config.js) and fill:
   - `url`
   - `anonKey`
4. Deploy the updated `docs/` files to GitHub Pages.
5. Create your admin user by signing in with email and password, then run:

```sql
update public.profiles
set role = 'admin'
where id = (
  select id
  from auth.users
  where email = 'your-admin-email@example.com'
);
```

6. Public visitors can use the quick account flow to comment with only a display name.

Notes:
- `anonKey` is public and can live in the frontend.
- Images are uploaded to the `wiki-assets` storage bucket.
- Published entries from Supabase override static entries when the `id` matches.
