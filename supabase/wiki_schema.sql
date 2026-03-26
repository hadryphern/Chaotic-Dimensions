create extension if not exists pgcrypto;

create or replace function public.touch_updated_at()
returns trigger
language plpgsql
as $$
begin
  new.updated_at = timezone('utc', now());
  return new;
end;
$$;

create table if not exists public.profiles (
  id uuid primary key references auth.users(id) on delete cascade,
  display_name text not null check (char_length(display_name) between 2 and 40),
  role text not null default 'member' check (role in ('member', 'admin')),
  created_at timestamptz not null default timezone('utc', now()),
  updated_at timestamptz not null default timezone('utc', now())
);

create table if not exists public.wiki_entries (
  id text primary key,
  category text not null,
  image_url text,
  related_ids text[] not null default '{}'::text[],
  sort_order integer not null default 0,
  is_published boolean not null default false,
  content_json jsonb not null default '{}'::jsonb,
  created_by uuid references public.profiles(id),
  updated_by uuid references public.profiles(id),
  created_at timestamptz not null default timezone('utc', now()),
  updated_at timestamptz not null default timezone('utc', now())
);

create table if not exists public.wiki_comments (
  id uuid primary key default gen_random_uuid(),
  entry_id text not null,
  user_id uuid not null references public.profiles(id) on delete cascade,
  display_name text not null check (char_length(display_name) between 2 and 40),
  body text not null check (char_length(body) between 2 and 2000),
  is_hidden boolean not null default false,
  created_at timestamptz not null default timezone('utc', now()),
  updated_at timestamptz not null default timezone('utc', now())
);

create index if not exists idx_wiki_entries_published on public.wiki_entries (is_published, sort_order, updated_at desc);
create index if not exists idx_wiki_comments_entry_id on public.wiki_comments (entry_id, created_at desc);
create index if not exists idx_wiki_comments_user_id on public.wiki_comments (user_id, created_at desc);

drop trigger if exists profiles_touch_updated_at on public.profiles;
create trigger profiles_touch_updated_at
before update on public.profiles
for each row execute function public.touch_updated_at();

drop trigger if exists wiki_entries_touch_updated_at on public.wiki_entries;
create trigger wiki_entries_touch_updated_at
before update on public.wiki_entries
for each row execute function public.touch_updated_at();

drop trigger if exists wiki_comments_touch_updated_at on public.wiki_comments;
create trigger wiki_comments_touch_updated_at
before update on public.wiki_comments
for each row execute function public.touch_updated_at();

create or replace function public.handle_new_user()
returns trigger
language plpgsql
security definer
set search_path = public
as $$
declare
  fallback_name text;
begin
  fallback_name := coalesce(
    nullif(trim(new.raw_user_meta_data ->> 'display_name'), ''),
    nullif(split_part(new.email, '@', 1), ''),
    'Traveler'
  );

  insert into public.profiles (id, display_name)
  values (new.id, left(fallback_name, 40))
  on conflict (id) do update
  set display_name = excluded.display_name,
      updated_at = timezone('utc', now());

  return new;
end;
$$;

drop trigger if exists on_auth_user_created on auth.users;
create trigger on_auth_user_created
after insert on auth.users
for each row execute function public.handle_new_user();

create or replace function public.is_admin()
returns boolean
language sql
stable
security definer
set search_path = public
as $$
  select exists (
    select 1
    from public.profiles profile
    where profile.id = auth.uid()
      and profile.role = 'admin'
  );
$$;

alter table public.profiles enable row level security;
alter table public.wiki_entries enable row level security;
alter table public.wiki_comments enable row level security;

drop policy if exists "profiles_select_own_or_admin" on public.profiles;
create policy "profiles_select_own_or_admin"
on public.profiles
for select
using (id = auth.uid() or public.is_admin());

drop policy if exists "profiles_insert_own" on public.profiles;
create policy "profiles_insert_own"
on public.profiles
for insert
with check (id = auth.uid());

drop policy if exists "profiles_update_own_or_admin" on public.profiles;
create policy "profiles_update_own_or_admin"
on public.profiles
for update
using (id = auth.uid() or public.is_admin())
with check (id = auth.uid() or public.is_admin());

drop policy if exists "wiki_entries_select_published_or_admin" on public.wiki_entries;
create policy "wiki_entries_select_published_or_admin"
on public.wiki_entries
for select
using (is_published or public.is_admin());

drop policy if exists "wiki_entries_insert_admin" on public.wiki_entries;
create policy "wiki_entries_insert_admin"
on public.wiki_entries
for insert
with check (public.is_admin());

drop policy if exists "wiki_entries_update_admin" on public.wiki_entries;
create policy "wiki_entries_update_admin"
on public.wiki_entries
for update
using (public.is_admin())
with check (public.is_admin());

drop policy if exists "wiki_entries_delete_admin" on public.wiki_entries;
create policy "wiki_entries_delete_admin"
on public.wiki_entries
for delete
using (public.is_admin());

drop policy if exists "wiki_comments_select_visible_or_admin" on public.wiki_comments;
create policy "wiki_comments_select_visible_or_admin"
on public.wiki_comments
for select
using (not is_hidden or public.is_admin());

drop policy if exists "wiki_comments_insert_own" on public.wiki_comments;
create policy "wiki_comments_insert_own"
on public.wiki_comments
for insert
to authenticated
with check (auth.uid() = user_id);

drop policy if exists "wiki_comments_update_own_or_admin" on public.wiki_comments;
create policy "wiki_comments_update_own_or_admin"
on public.wiki_comments
for update
to authenticated
using (auth.uid() = user_id or public.is_admin())
with check (auth.uid() = user_id or public.is_admin());

drop policy if exists "wiki_comments_delete_own_or_admin" on public.wiki_comments;
create policy "wiki_comments_delete_own_or_admin"
on public.wiki_comments
for delete
to authenticated
using (auth.uid() = user_id or public.is_admin());

insert into storage.buckets (id, name, public)
values ('wiki-assets', 'wiki-assets', true)
on conflict (id) do update set public = excluded.public;

drop policy if exists "wiki_assets_public_read" on storage.objects;
create policy "wiki_assets_public_read"
on storage.objects
for select
using (bucket_id = 'wiki-assets');

drop policy if exists "wiki_assets_admin_insert" on storage.objects;
create policy "wiki_assets_admin_insert"
on storage.objects
for insert
to authenticated
with check (bucket_id = 'wiki-assets' and public.is_admin());

drop policy if exists "wiki_assets_admin_update" on storage.objects;
create policy "wiki_assets_admin_update"
on storage.objects
for update
to authenticated
using (bucket_id = 'wiki-assets' and public.is_admin())
with check (bucket_id = 'wiki-assets' and public.is_admin());

drop policy if exists "wiki_assets_admin_delete" on storage.objects;
create policy "wiki_assets_admin_delete"
on storage.objects
for delete
to authenticated
using (bucket_id = 'wiki-assets' and public.is_admin());
