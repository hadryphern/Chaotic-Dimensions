# Supabase Setup

1. Create a Supabase project.
2. In `Authentication > Providers`, enable:
   - `Anonymous`
   - `Email`
3. Open the SQL Editor and run `supabase/wiki_schema.sql`.
4. Open `docs/supabase.config.js` and fill:
   - `url`
   - `anonKey`
5. Deploy the updated `docs/` files to GitHub Pages.
6. Create your editor user from the wiki login screen or from Supabase Auth, then run:

```sql
update public.profiles
set role = 'admin'
where id = (
  select id
  from auth.users
  where email = 'your-admin-email@example.com'
);
```

7. Public visitors can use the quick account flow to comment with only a display name.

Notes:
- `anonKey` is public and can live in the frontend.
- Images are uploaded to the `wiki-assets` storage bucket.
- Published entries from Supabase override static entries when the `id` matches.
