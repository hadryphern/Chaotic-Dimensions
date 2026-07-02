# Chaotic Dimensions

Repositório oficial do mod **Chaotic Dimensions** para **Terraria / tModLoader**.

A organização principal agora está separada em três áreas:
 
## [ChaoticDImensions PAP](ChaoticDImensions%20PAP)

Documentos da PAP:

- relatório principal
- modelo do relatório
- normas oficiais
- relatórios antigos/base

## [ChaoticDimensions Assets](ChaoticDimensions%20Assets)

Arquivos de apoio do projeto:

- sprites antigas, testes e versões alternativas
- concepts, ideias planejadas e bosses ainda arquivados
- wiki local, imagens de referência e materiais de apresentação
- backups e arquivos temporários preservados por histórico do projeto

## [ChaoticDimensions Mod](ChaoticDimensions%20Mod)

Projeto real usado pelo **tModLoader**:

- `ChaoticDimensions.csproj`
- `build.txt`
- `Common/`
- `Content/`
- `Localization/`
- `Sounds/`
- `Assets/` usados diretamente em jogo
- ícones, descrições e metadados do mod

Esta pasta deve manter a estrutura normal de um mod dentro de `tModLoader/ModSources`.

## Build Local

```sh
dotnet build "ChaoticDimensions Mod/ChaoticDimensions.csproj"
```

No tModLoader, a pasta de ModSources deve apontar para:

```text
ChaoticDimensions Mod/
```

## Wiki

A wiki pública continua preservada em:

```text
ChaoticDimensions Assets/Documentation/WikiSite/
```
