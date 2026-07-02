# Wiki Maintenance Workflow

Esta wiki vive no proprio projeto e deve acompanhar o estado real do mod, mesmo quando um item, mob ou boss ainda usa sprite placeholder.

## Fonte de verdade

- Estrutura base do site: [docs/data.js](C:/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/data.js)
- Entradas geradas automaticamente: [docs/generated-reference-overrides.js](C:/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/generated-reference-overrides.js) e [docs/generated-minecraft-legacy-data.js](C:/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/generated-minecraft-legacy-data.js)
- Override manual final: [docs/wiki-overrides.js](C:/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/wiki-overrides.js)

`wiki-overrides.js` deve ser tratado como a camada final manual da wiki. Sempre que um texto gerado ficar incompleto, desatualizado, placeholder demais ou simplesmente errado, a correção deve entrar ali.

## Regra editorial

- Se o conteúdo já existe no mod, ele pode e deve existir na wiki.
- Placeholder visual não bloqueia publicação da página.
- Se a rota natural de obtenção ainda não estiver pronta, a página deve explicar isso claramente.
- Se um conteúdo sair do mod, a entrada deve ser escondida com `isPublished: false` em `wiki-overrides.js`.

## Fluxo recomendado a cada update

1. Adicionar ou atualizar o conteúdo no mod.
2. Sincronizar as imagens da wiki:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\wiki\Sync-WikiAssets.ps1
```

3. Criar ou ajustar a entrada em `wiki-overrides.js`.
4. Se o conteúdo precisar de documentação longa, atualizar também:

- [docs/ChaoticDimensionsWikiReference.md](C:/Users/happi/Documents/My%20Games/Terraria/tModLoader/ModSources/ChaoticDimensions/docs/ChaoticDimensionsWikiReference.md)

## O que cada entrada deve ter

- `id`: slug estavel da página
- `category`: categoria da wiki
- `image`: caminho em `./assets/images/content/...`
- `related`: ids conectados
- `content["pt-BR"]`
  - `title`
  - `subtitle`
  - `summary`
  - `overview`
  - `facts`
  - `obtain`
  - `crafting`
  - `drops`
  - `tactics`
  - `notes`

Nem toda página precisa de todos os campos, mas itens, mobs e bosses principais devem ficar o mais completos possível.

## Filosofia da wiki

- Documentar o que existe agora.
- Marcar claramente o que ainda depende de boss, biome, evento ou sprite final.
- Nunca esconder progresso real do mod só porque a arte ainda está provisória.
