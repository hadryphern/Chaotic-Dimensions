# Progressão canónica do Chaotic Dimensions

Atualizado em 19 de junho de 2026. Este documento separa o que já funciona no build atual do que está planeado. Um conteúdo planeado não deve ser apresentado como jogável.

## Ordem principal

1. Terraria pré-Hardmode e Wall of Flesh.
2. Três bosses mecânicos.
3. Rosalita Ore e linha Rosalita.
4. Plantera e Golem.
5. Mago Imã (planeado, pré-Cultista).
6. Cultista Lunático, Pilares e Moon Lord.
7. Monthra (implementada, agora pós-Moon Lord).
8. Crystaline Devourer (implementado).
9. Alien Kraken (implementado).
10. Shadow Ascension e futuros bosses intermédios.
11. Palhaço Mímico (planeado, boss final).

## Mundo e materiais

### Shadow Biome

O Shadow Biome é criado com o mundo e substitui uma zona evil do lado esquerdo. Sem Shadow Totem, o jogador sofre trevas, drenagem de vida e perda de mobilidade. Phantasm, Shadow Eye, Shadow Slime e Shadow Worm fornecem Shadow Scrap e, no Hardmode, Soul of Shadow.

### Rosalita Ore

Rosalita Ore é gerado depois da derrota dos três bosses mecânicos. O minério é convertido em Rosalita Gem e combinado com Shadow Scrap e Hallowed Bar no Mythril/Orichalcum Anvil.

A linha inclui Rosalita Pickaxe, Axe, Hammer, Blade, Bow, Wand, Whip, Shield e armadura. É o equipamento indicado para Plantera, Golem e para preparar o futuro Mago Imã.

### Materiais legacy

Alexandrite, Chaos Crystal, Shadow Gem, Shadow Nugget, Vortex Gem, Glass Stick, Bedrock Stick, Iron Stick, Ratrix Stick e Shadow Bar permanecem no projeto. Alguns possuem receitas ou usos; outros ainda precisam de uma fonte natural definitiva. O relatório e a wiki devem marcar estes casos como incompletos.

## Mago Imã — planeado

- Slot: depois de Golem e antes do Cultista Lunático.
- Vida base prevista: 980 000.
- Invocação prevista: Selo Polarizado, produzido em Mythril/Orichalcum Anvil com Hallowed Bar, Ectoplasm, Martian Conduit Plating e componentes magnéticos.
- Arena: ampla, sem paredes letais; o desafio é o controlo de movimento.
- Mecânicas: impulsos cardinais, atração central, repulsão radial, redes magnetizadas, hipnose anunciada e órbita de polos.
- Drop obrigatório previsto: Núcleo Magnetizado.
- Função do Núcleo Magnetizado: componente da versão final do Crystaline Sigil.
- Estado técnico: classes de design compiláveis, sem ModNPC, sprite, summon ou spawn.

## Moon Lord

A derrota do Moon Lord abre a progressão própria de final de jogo. A partir deste ponto surgem as Monthra Butterflies e começa a rota Monthra → Crystaline → Kraken.

## Monthra — implementada

### Invocação

Depois do Moon Lord, Monthra Butterflies podem surgir na superfície e no céu. Matar uma inicia a apresentação da boss e, no fim da intro, invoca Monthra.

### Valores

- Vida base: 5 000 000.
- Dano base: 260.
- Defesa: 80.
- Hitbox: 460×350.
- Escala visual: 0,68.
- Música: Unholy Insurgency, ficheiro fornecido para o projeto.

### Combate

Monthra alterna cinco estados: Hover Volley, Dash Chain, Light Cage, Sweeping Burst e Butterfly Swarm. A luta privilegia proximidade, dashes e leitura rápida.

Light Cage cobre a zona exterior com quatro faixas luminosas. O centro livre mede aproximadamente 270×210 pixels; as faixas avisam durante 58 ticks e ficam perigosas durante 34. Butterfly Swarm cria seis a oito minions hostis com perseguição curva. Abaixo de 50% aumentam velocidade, projéteis e minions; abaixo de 20% os dashes e volleys tornam-se mais rápidos.

### Drops e função

Monthra deixa 55–75 Monthra Scales e Super Healing Potions. As escalas alimentam as armas Monthra, a linha Eclipsed Monthra e agora são exigidas pelo Crystaline Sigil. Assim a derrota de Monthra é obrigatória antes do Crystaline Devourer.

## Crystaline Devourer — implementado

### Invocação

Crystaline Sigil requer atualmente 250 Crystal Shards, 250 Lunar Bars, 40 Monthra Scales, Worm Food ou Bloody Spine e Lunar Crafting Station. Só funciona depois do Moon Lord e de Monthra.

Na progressão final, a receita também deverá receber um Núcleo Magnetizado do Mago Imã.

### Encontro

Dois vermes partilham a progressão da luta. Cada cabeça possui 2 500 000 de vida; o total combinado é 5 000 000. A arena tem semi-largura de 164 tiles e semi-altura de 104 tiles. Sair da arena é letal.

A intro dura 780 ticks, com 210 ticks de fade. A atmosfera usa o visual vanilla do Moon Lord com uma película rosa escura. A música é Trial of the Insane, ficheiro fornecido para o projeto.

### Recompensas

Crystaline Tear, Chaos Crystal Pickaxe e Heart of the God são os drops centrais; Crystaline Sword e Crystaline Eye têm chance adicional. A derrota gera Shadow Ore no Shadow Biome e desbloqueia a invocação do Kraken.

## Alien Kraken — implementado

### Invocação

O item de invocação exige 60 Crystaline Tears, 30 Souls of Shadow e 20 Lunar Bars na Godness Anvil. O evento só começa depois do Crystaline Devourer.

### Valores e apresentação

- Vida base: 10 000 000, exatamente o dobro da vida combinada do Crystaline Devourer.
- Defesa: 180 acima de 80% de vida; 0 entre 80% e 50%; 36 na fase 2; 54 na fase 3.
- Hitbox: 460×760.
- Frame final: 810×1050; atlas: 4860×6300.
- Escalas visuais: 1,28; 1,36; 1,44.
- Atmosfera: visual vanilla do Moon Lord com tom azul-cinzento discreto.
- Câmara: ancorada ao boss durante o combate.
- Vinheta: centrada no jogador, mais forte na segunda fase.

Os ataques incluem água, relâmpagos, sky beams, laser rotativo, laser guiado, vórtices, tentáculos, clones, minions e torres da Ruby. Os olhos vermelhos foram restaurados a partir do spritesheet original. As bordas foram limpas para eliminar completamente a aura cromática; a Ruby continua desenhada separadamente e visível nos estados de dash e invisibilidade.

## Shadow Ascension — implementado parcialmente

Depois do Crystaline, Shadow Ore é gerado. Shadow Ore, Shadow Scrap, Soul of Shadow e Rosalita Gem alimentam o tier Shadow na Godness Anvil.

O tier inclui Heart of Shadows, Glory Boots, Shadow Mana Potion, Shadow Melee Potion, Shadow Summon Staff, Shadow Whip, Shadow Bow, Shadow Arrow, Shadow Bullet, Shadow Staff, Shadow Zenith e armadura Shadow. Existem ainda versões Eclipsed Monthra de ferramentas e armas.

A rota natural da Godness Anvil, Shadow Totem e alguns materiais legacy ainda precisa de fecho definitivo. Até isso acontecer, o conteúdo deve ser descrito como implementado em código, mas parcialmente bloqueado em progressão.

## Palhaço Mímico — planeado

- Slot: último boss do mod.
- Vida base prevista: 179 000 000.
- Defesa prevista: 420, com janelas de vulnerabilidade associadas à música.
- Formas: palhaço, cópias de bosses vanilla e do mod, e forma verdadeira.
- Natureza: boss de ritmo, com telegraphs ligados a tempos, compassos e subdivisões.
- Foco: ataques corpo a corpo, sequências de desvio, corredores de facas, martelos, transformações e refrão final.
- Regra de justiça: a música reforça os sinais, mas cada ataque também deve possuir indicação visual para acessibilidade e para situações sem áudio.
- Estado técnico: classes de design compiláveis, sem ModNPC, sprite, summon ou spawn.

## Itens e linhas de equipamento

### Catálogo transversal de 245 itens

Foi acrescentado um catálogo funcional que atravessa 17 patamares, desde a superfície até ao pós-Kraken. A distribuição é de 50 itens melee, 45 ranged, 45 magic, 45 summon, 25 acessórios, 15 ferramentas, 10 consumíveis e 10 materiais.

Os itens são classes reais de `ModItem`, possuem receitas e reutilizam comportamentos comuns de projéteis, minions e chicotes. As sprites ainda são placeholders vanilla; por isso, a implementação está jogável, mas a direção artística continua pendente.

As linhas Krakenbane são desbloqueadas depois do Crystaline Devourer e têm 12 500 000 de dano base, suficiente para testar uma derrota do Kraken num golpe. Abyssal e Chaotic chegam a 25 000 000 e 50 000 000. Estes valores são deliberadamente extremos e devem ser revistos depois dos testes de progressão.

O catálogo individual encontra-se em `docs/Catalogo_245_Itens.md`. As pranchas em `assets_work/concept_sketches/items` registam todos os nomes e respetivos tiers.

### Monthra

Monthra Blade, Bow, Wand e Butterfly Staff usam Monthra Scale. Eclipsed Monthra Pickaxe, Axe, Hammer, Blade, Bow, Wand e Whip combinam Rosalita Gem, Monthra Scale e Shadow Scrap.

### Crystaline

Crystaline Eye oferece mobilidade; Crystaline Potion melhora recuperação; Crystaline Staff, Sword e Gun formam o arsenal; Crystaline Devour Armor representa o equipamento principal do boss.

### Rosalita

Ferramentas, armas, whip, shield e armadura formam a ponte entre os mecânicos e o conteúdo pós-Golem.

### Shadow

É uma das linhas artesanais mais avançadas do conteúdo anterior ao catálogo transversal. Os valores são muito superiores aos equipamentos vanilla e precisam de balanceamento contra a sequência pós-Kraken.

### Legacy

As linhas Alexandrite, Chaos Crystal, Shadow, Vortex e ferramentas herdadas continuam preservadas. Cada item deve receber uma das etiquetas: obtenção funcional, receita funcional sem drop natural, reservado ou placeholder visual.

## Pendências de progressão

- Implementar Mago Imã e o Núcleo Magnetizado.
- Fechar obtenção da Godness Anvil e Shadow Totem.
- Definir os bosses entre Kraken e Palhaço Mímico.
- Criar sprites próprias para os 245 itens e substituir os placeholders vanilla.
- Testar e balancear os tiers Krakenbane, Abyssal e Chaotic em jogo real.
- Rever valores excessivos do tier Shadow.
- Adicionar acessibilidade visual aos futuros ataques rítmicos.
- Verificar direitos e créditos das faixas musicais antes de qualquer distribuição pública.
