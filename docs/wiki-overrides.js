export const entryOverrides = [
  {
    id: "chaos-crystal",
    related: ["crystal-creeper", "vortex-blade", "ratrix-stick"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "drop"
      },
      "pt-BR": {
        summary: "Catalisador raro dropado por Crystal Creeper e usado nos crafts mais caros da linha MinecraftLegacy.",
        overview: "Chaos Crystal marca a virada do material comum para o componente premium dentro da linha MinecraftLegacy. No estado atual do codigo ele ja aparece na Vortex Blade e tambem no Ratrix Stick, entao cada unidade dropada vira estoque importante para os crafts mais caros desse pacote.",
        facts: [
          "Raridade Pink",
          "Valor de venda: 2 gold",
          "Drop identificado em Crystal Creeper",
          "Material usado por Vortex Blade e Ratrix Stick"
        ],
        obtain: [
          "Dropado por Crystal Creeper no loop atual de MinecraftLegacy.",
          "Hoje a rota mais clara e farmar o mob e guardar o cristal para os crafts de tier mais alto."
        ],
        notes: [
          "Como nao existe receita publica para ele, vale separar esse estoque dos materiais comuns para nao travar os crafts finais."
        ]
      },
      en: {
        summary: "A rare catalyst dropped by Crystal Creeper and used by the most expensive MinecraftLegacy crafts.",
        overview: "Chaos Crystal marks the shift from common material to premium component inside the MinecraftLegacy line. In the current code it already feeds Vortex Blade and also enters Ratrix Stick, so every dropped unit matters once the branch reaches its more expensive crafts.",
        facts: [
          "Pink rarity",
          "Sell value: 2 gold",
          "Tracked drop from Crystal Creeper",
          "Used by Vortex Blade and Ratrix Stick"
        ],
        obtain: [
          "Dropped by Crystal Creeper in the current MinecraftLegacy loop.",
          "The cleanest route right now is to farm the mob and bank the crystals for higher-tier crafts."
        ],
        notes: [
          "Because there is no public recipe for it, it is worth stockpiling separately from common materials so final crafts do not stall."
        ]
      }
    }
  },
  {
    id: "glass-stick",
    related: ["ruby-sword", "ruby-pickaxe", "ruby-axe", "vortex-blade"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        summary: "Haste basica de vidro que sustenta os primeiros crafts de arma e ferramenta da linha MinecraftLegacy.",
        overview: "Glass Stick funciona como cabo e estrutura leve para varias armas e ferramentas do pacote. Agora que a receita existe no codigo, a wiki pode tratar o item como componente fabricavel de verdade e nao so como placeholder de material.",
        facts: [
          "Raridade White",
          "Criado com 50 Glass",
          "Usa Furnace como estacao principal",
          "Aparece em Ruby Sword, Ruby Pickaxe, Ruby Axe e Vortex Blade"
        ],
        obtain: [
          "Criado com 50 Glass em Furnace.",
          "Por ser barato e fabricavel cedo, costuma virar componente de estoque para varios crafts seguidos."
        ],
        crafting: [
          "Glass x50 em Furnace"
        ],
        notes: [
          "Mesmo sendo simples, ele conecta a linha Ruby ao salto posterior da Vortex Blade."
        ]
      },
      en: {
        summary: "A basic glass rod that supports the first MinecraftLegacy weapon and tool crafts.",
        overview: "Glass Stick works as the handle and light frame for multiple weapons and tools in the package. Now that the recipe exists in code, the wiki can treat it as a real craftable component instead of a placeholder material.",
        facts: [
          "White rarity",
          "Crafted from 50 Glass",
          "Uses Furnace as its main station",
          "Appears in Ruby Sword, Ruby Pickaxe, Ruby Axe and Vortex Blade"
        ],
        obtain: [
          "Crafted from 50 Glass at a Furnace.",
          "Because it is cheap and available early, it naturally becomes a stock component for several consecutive recipes."
        ],
        crafting: [
          "Glass x50 at Furnace"
        ],
        notes: [
          "Even as a simple ingredient, it connects the Ruby line to the later Vortex Blade jump."
        ]
      }
    }
  },
  {
    id: "iron-stick",
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        summary: "Haste metalica craftavel que prepara a linha MinecraftLegacy para receitas mais industriais.",
        overview: "Iron Stick funciona como componente de metal refinado dentro da linha MinecraftLegacy. O codigo atual ja abre duas receitas alternativas, usando Iron Bar ou Lead Bar, o que torna a entrada util mesmo antes de uma lista maior de follow-ups aparecer publicamente.",
        facts: [
          "Raridade Blue",
          "Tem receita com Iron Bar ou Lead Bar",
          "Ambas as receitas usam Anvil",
          "Age como material de estoque para a linha metalica do pacote"
        ],
        obtain: [
          "Criado com 50 Iron Bar em Anvil.",
          "Tambem pode ser criado com 50 Lead Bar em Anvil."
        ],
        crafting: [
          "Iron Bar x50 em Anvil",
          "Lead Bar x50 em Anvil"
        ],
        notes: [
          "Hoje ele funciona mais como componente preparado para expansoes do que como um hub ja cheio de crafts publicos."
        ]
      },
      en: {
        summary: "A craftable metal rod that prepares the MinecraftLegacy line for heavier industrial recipes.",
        overview: "Iron Stick acts as a refined metal component inside the MinecraftLegacy branch. The current code already exposes two alternative recipes, using Iron Bar or Lead Bar, which makes the page useful even before a larger list of public follow-up items is added.",
        facts: [
          "Blue rarity",
          "Has both Iron Bar and Lead Bar recipes",
          "Both recipes use an Anvil",
          "Acts as a stocked component for the branch's metal line"
        ],
        obtain: [
          "Crafted with 50 Iron Bar at an Anvil.",
          "It can also be crafted with 50 Lead Bar at an Anvil."
        ],
        crafting: [
          "Iron Bar x50 at Anvil",
          "Lead Bar x50 at Anvil"
        ],
        notes: [
          "Right now it behaves more like a prepared component for future expansion than a fully connected recipe hub."
        ]
      }
    }
  },
  {
    id: "rosalita-gem",
    related: ["rosalita-ore", "rosalita-shield", "shadow-bar", "ratrix-stick"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        summary: "Refinamento central da Rosalita e pivô dos crafts mais importantes da fase hardmode da linha MinecraftLegacy.",
        overview: "Rosalita Gem e o ponto em que o minerio de Rosalita deixa de ser apenas worldgen e passa a virar progressao real. A gema ja alimenta o Rosalita Shield, sobe para Shadow Bar e tambem participa do Ratrix Stick, entao ela funciona como uma ponte entre defesa, alloy e tier alto.",
        facts: [
          "Raridade Light Red",
          "Valor de venda: 45 silver",
          "Criada com 4 Rosalita Ore",
          "Material usado por Rosalita Shield, Shadow Bar e Ratrix Stick"
        ],
        obtain: [
          "Refine 4 Rosalita Ore em Furnace para gerar 1 Rosalita Gem.",
          "Na pratica, ela entra em cena depois que o mundo ja liberou a geracao de Rosalita no hardmode."
        ],
        crafting: [
          "Rosalita Ore x4 em Furnace"
        ],
        notes: [
          "A gema e o verdadeiro checkpoint da linha Rosalita: minerar o ore importa, mas converter em gemas e o que libera os crafts relevantes."
        ]
      },
      en: {
        summary: "The refined core of Rosalita and the main pivot for the important hardmode crafts in the MinecraftLegacy line.",
        overview: "Rosalita Gem is the point where Rosalita stops being mere worldgen ore and turns into real progression. The gem already feeds Rosalita Shield, upgrades into Shadow Bar and also participates in Ratrix Stick, so it bridges defense, alloy crafting and higher-tier material flow.",
        facts: [
          "Light Red rarity",
          "Sell value: 45 silver",
          "Crafted from 4 Rosalita Ore",
          "Used by Rosalita Shield, Shadow Bar and Ratrix Stick"
        ],
        obtain: [
          "Refine 4 Rosalita Ore at a Furnace to create 1 Rosalita Gem.",
          "In practice it comes online only after the world has unlocked Rosalita generation in hardmode."
        ],
        crafting: [
          "Rosalita Ore x4 at Furnace"
        ],
        notes: [
          "The gem is the real checkpoint of the Rosalita line: mining the ore matters, but refining it is what unlocks the relevant crafts."
        ]
      }
    }
  },
  {
    id: "rosalita-ore",
    related: ["rosalita-gem"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "worldgen"
      },
      "pt-BR": {
        summary: "Minerio de progressao do hardmode que passa a ser gerado depois dos tres chefes mecanicos.",
        overview: "Rosalita Ore e o novo degrau de mineracao da linha MinecraftLegacy no hardmode. O sistema de worldgen injeta esse minerio depois que o mundo entra em hardmode e os tres mech bosses ja cairam, fazendo dele a ponte entre o hardmode padrao e os crafts mais avancados da linha Rosalita.",
        facts: [
          "Raridade Pink",
          "Gerado depois dos tres chefes mecanicos",
          "Faixa de geracao entre 44% e 88% da profundidade do mundo",
          "Principal refinamento: Rosalita Gem em Furnace"
        ],
        obtain: [
          "Depois que o mundo esta em hardmode e os tres mech bosses foram derrotados, novos veios de Rosalita Ore podem ser gerados.",
          "A rota pratica e minerar em camadas subterraneas profundas e refinar o minerio em Rosalita Gem."
        ],
        notes: [
          "O sistema marca a geracao por mundo para evitar repeticao, entao esse ore funciona como desbloqueio de etapa e nao como loot isolado."
        ]
      },
      en: {
        summary: "A hardmode progression ore that starts generating after all three mechanical bosses are defeated.",
        overview: "Rosalita Ore is the new mining step for the MinecraftLegacy line in hardmode. The worldgen system injects this ore once the world is in hardmode and all three mech bosses are down, turning it into the bridge between standard hardmode and the more advanced Rosalita crafts.",
        facts: [
          "Pink rarity",
          "Generated after all three mechanical bosses",
          "Ore band spans roughly 44% to 88% world depth",
          "Primary refinement target: Rosalita Gem at a Furnace"
        ],
        obtain: [
          "Once the world is in hardmode and all three mech bosses are defeated, new Rosalita Ore veins can be generated.",
          "The practical route is to mine it in deeper underground layers and refine it into Rosalita Gem."
        ],
        notes: [
          "The system flags the generation per world to avoid repetition, so this ore acts like a stage unlock rather than a random drop source."
        ]
      }
    }
  },
  {
    id: "rosalita-shield",
    related: ["rosalita-gem"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        summary: "Escudo defensivo que transforma Rosalita Gem em estabilidade real para lutas mais longas.",
        overview: "Rosalita Shield nao e so um acessorio tematico: ele pega a linha Rosalita e converte em defesa, resistencia percentual e imunidade a knockback. Isso faz a pagina dele funcionar como checkpoint defensivo importante dentro do bloco MinecraftLegacy.",
        facts: [
          "Raridade Yellow",
          "+8 de defesa direta",
          "+5% de endurance",
          "Remove knockback",
          "Criado com 18 Rosalita Gem em Mythril Anvil"
        ],
        obtain: [
          "Criado com 18 Rosalita Gem em Mythril Anvil.",
          "Ele entra melhor no mid-to-late hardmode, quando o jogador ja consegue sustentar o custo alto de Rosalita."
        ],
        crafting: [
          "Rosalita Gem x18 em Mythril Anvil"
        ],
        notes: [
          "O item ficou mais forte no update atual e agora merece ser lido como acessorio defensivo serio, nao como enfeite de colecao."
        ]
      },
      en: {
        summary: "A defensive shield that turns Rosalita Gem into real stability for longer fights.",
        overview: "Rosalita Shield is not just a themed accessory: it converts the Rosalita branch into flat defense, percentage endurance and knockback immunity. That makes it a meaningful defensive checkpoint inside the MinecraftLegacy block.",
        facts: [
          "Yellow rarity",
          "+8 flat defense",
          "+5% endurance",
          "Removes knockback",
          "Crafted with 18 Rosalita Gem at a Mythril Anvil"
        ],
        obtain: [
          "Crafted with 18 Rosalita Gem at a Mythril Anvil.",
          "It fits best into mid-to-late hardmode once the player can sustain the higher Rosalita cost."
        ],
        crafting: [
          "Rosalita Gem x18 at Mythril Anvil"
        ],
        notes: [
          "The item was strengthened in the current update and now reads as a real defensive accessory instead of a collection piece."
        ]
      }
    }
  },
  {
    id: "shadow-ore",
    related: ["crystaline-devourer"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "worldgen"
      },
      "pt-BR": {
        summary: "Minerio de escalada pos-Crystaline Devour gerado no mundo depois do boss cosmico atual.",
        overview: "Shadow Ore e a camada de world escalation que a linha MinecraftLegacy recebe depois da queda do Crystaline Devour. O sistema injeta esse minerio quando o downed do boss fica verdadeiro, transformando o loot cosmico do mod em nova liberacao de recurso no mundo.",
        facts: [
          "Raridade Red",
          "Gerado depois de derrotar Crystaline Devour",
          "Faixa de geracao entre 38% e 92% da profundidade do mundo",
          "Funciona como desbloqueio de minerio para a fase cosmica atual"
        ],
        obtain: [
          "Depois da derrota de Crystaline Devour, o sistema pode injetar veios de Shadow Ore no mundo.",
          "A rota pratica e minerar o novo ore nas camadas profundas assim que o boss ja tiver sido abatido."
        ],
        notes: [
          "Mesmo antes de uma malha completa de receitas publicas aparecer, o ore ja documenta a escalada de worldgen ligada ao boss cosmico."
        ]
      },
      en: {
        summary: "A post-Crystaline Devour escalation ore generated in the world after the current cosmic boss falls.",
        overview: "Shadow Ore is the world-escalation layer that the MinecraftLegacy line receives after Crystaline Devour is defeated. The system injects this ore once the boss downed flag is true, turning cosmic progression into a new world resource unlock.",
        facts: [
          "Red rarity",
          "Generated after defeating Crystaline Devour",
          "Ore band spans roughly 38% to 92% world depth",
          "Acts as the mining unlock for the current cosmic stage"
        ],
        obtain: [
          "After Crystaline Devour is defeated, the system can inject Shadow Ore veins into the world.",
          "The practical route is to mine the new ore in deeper layers once the boss kill is already registered."
        ],
        notes: [
          "Even before a full public recipe mesh appears, the ore already documents the worldgen escalation tied to the cosmic boss."
        ]
      }
    }
  },
  {
    id: "vortex-gem",
    related: ["vortex-blade"],
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "static"
      },
      "pt-BR": {
        summary: "Gema vermelha de tier alto usada para fechar a receita pesada da Vortex Blade.",
        overview: "Vortex Gem ocupa o topo atual da sublinha de gemas do MinecraftLegacy. Mesmo sem uma rota publica de obtencao totalmente exposta na wiki, a propria receita da Vortex Blade ja mostra que a gema existe como material premium e de alto custo por unidade.",
        facts: [
          "Raridade Red",
          "Valor de venda: 1 gold",
          "Ingrediente principal da Vortex Blade",
          "Cada craft rastreado pede 12 unidades"
        ],
        obtain: [
          "A rota publica de obtencao ainda nao esta fechada no codigo rastreado pela wiki.",
          "Mesmo assim, a Vortex Blade ja deixa claro que cada unidade tem peso alto no tier."
        ],
        notes: [
          "Vale tratar a gema como material premium de estoque, porque o primeiro craft conhecido ja pede uma quantidade grande."
        ]
      },
      en: {
        summary: "A high-tier red gem used to close out the heavy Vortex Blade recipe.",
        overview: "Vortex Gem currently sits at the top of the MinecraftLegacy gem subline. Even without a fully public obtain route exposed in the wiki yet, the Vortex Blade recipe already shows that the gem exists as a premium material with significant unit cost.",
        facts: [
          "Red rarity",
          "Sell value: 1 gold",
          "Primary ingredient for Vortex Blade",
          "The tracked craft already asks for 12 units"
        ],
        obtain: [
          "The public obtain route is not fully exposed in the code currently tracked by the wiki.",
          "Even so, the Vortex Blade recipe already makes it clear that each unit carries high tier weight."
        ],
        notes: [
          "It is best treated like a premium stock material because the first known craft already demands a large amount."
        ]
      }
    }
  },
  {
    id: "shadow-bar",
    category: "materials",
    image: "./assets/images/content/Items/MinecraftLegacy/ShadowBar.png",
    related: ["rosalita-gem", "ratrix-stick"],
    sortOrder: 364,
    isPublished: true,
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        title: "Shadow Bar",
        subtitle: "Material MinecraftLegacy",
        summary: "Barra refinada de tier alto que mistura Rosalita, Hallowed e Lunar para empurrar a linha MinecraftLegacy ao endgame.",
        overview: "Shadow Bar funciona como liga de transicao entre o hardmode avancado e a camada mais cara do MinecraftLegacy. A receita combina Rosalita Gem, Lunar Bar e Hallowed Bar em Mythril Anvil, deixando claro que o item ja nasce pensado para crafts de peso e nao para uso descartavel.",
        facts: [
          "Raridade Red",
          "Valor de venda: 3 gold",
          "Criado com Rosalita Gem, Lunar Bar e Hallowed Bar",
          "Serve como passo direto para Ratrix Stick"
        ],
        obtain: [
          "Criado com 1 Rosalita Gem + 1 Lunar Bar + 1 Hallowed Bar em Mythril Anvil.",
          "Na pratica, a receita pede que o jogador junte progresso de hardmode classico com recursos mais tardios."
        ],
        crafting: [
          "Rosalita Gem x1 + Lunar Bar x1 + Hallowed Bar x1 em Mythril Anvil"
        ],
        notes: [
          "Hoje ele ja funciona como alloy importante por si so, mesmo antes de uma lista maior de follow-ups ser publicada."
        ]
      },
      en: {
        title: "Shadow Bar",
        subtitle: "Material from MinecraftLegacy",
        summary: "A high-tier refined bar that mixes Rosalita, Hallowed and Lunar resources to push MinecraftLegacy into endgame crafting.",
        overview: "Shadow Bar works as a transition alloy between advanced hardmode and the most expensive MinecraftLegacy layer. Its recipe combines Rosalita Gem, Lunar Bar and Hallowed Bar at a Mythril Anvil, which clearly positions it as a heavy craft component rather than a disposable filler.",
        facts: [
          "Red rarity",
          "Sell value: 3 gold",
          "Crafted from Rosalita Gem, Lunar Bar and Hallowed Bar",
          "Acts as a direct step toward Ratrix Stick"
        ],
        obtain: [
          "Crafted with 1 Rosalita Gem + 1 Lunar Bar + 1 Hallowed Bar at a Mythril Anvil.",
          "In practice the recipe asks the player to combine classic hardmode progress with later-tier materials."
        ],
        crafting: [
          "Rosalita Gem x1 + Lunar Bar x1 + Hallowed Bar x1 at Mythril Anvil"
        ],
        notes: [
          "It already works as a meaningful alloy on its own even before a larger list of follow-up items is published."
        ]
      }
    }
  },
  {
    id: "ratrix-stick",
    category: "materials",
    image: "./assets/images/content/Items/MinecraftLegacy/RatrixStick.png",
    related: ["rosalita-gem", "shadow-bar", "chaos-crystal"],
    sortOrder: 365,
    isPublished: true,
    content: {
      _meta: {
        eventKey: "minecraftlegacy",
        spawnKind: "craft"
      },
      "pt-BR": {
        title: "Ratrix Stick",
        subtitle: "Material MinecraftLegacy",
        summary: "Haste premium montada na Lunar Crafting Station para concentrar os materiais mais caros da linha MinecraftLegacy.",
        overview: "Ratrix Stick e um componente de fim de cadeia dentro do bloco MinecraftLegacy atual. A receita junta Rosalita Gem, Shadow Bar, Lunar Bar e Chaos Crystal em quantidade alta, entao a pagina dele funciona como marcador claro de investimento pesado e preparacao para futuros crafts de elite.",
        facts: [
          "Raridade Cyan",
          "Valor de venda: 15 gold",
          "Pede 10 Rosalita Gem, 10 Shadow Bar, 10 Lunar Bar e 10 Chaos Crystal",
          "Usa Lunar Crafting Station como estacao"
        ],
        obtain: [
          "Criado com Rosalita Gem x10 + Shadow Bar x10 + Lunar Bar x10 + Chaos Crystal x10 em Lunar Crafting Station.",
          "A receita concentra materiais de varias etapas e deixa claro que o item foi pensado como capstone material da linha."
        ],
        crafting: [
          "Rosalita Gem x10 + Shadow Bar x10 + Lunar Bar x10 + Chaos Crystal x10 em Lunar Crafting Station"
        ],
        notes: [
          "Mesmo antes de ganhar mais follow-ups publicos, ele ja merece pagina propria porque cristaliza o custo maximo atual da linha MinecraftLegacy."
        ]
      },
      en: {
        title: "Ratrix Stick",
        subtitle: "Material from MinecraftLegacy",
        summary: "A premium rod assembled at the Lunar Crafting Station to concentrate the most expensive materials in the MinecraftLegacy line.",
        overview: "Ratrix Stick is an end-of-chain component inside the current MinecraftLegacy block. The recipe pulls together Rosalita Gem, Shadow Bar, Lunar Bar and Chaos Crystal in high quantities, so the page acts as a clear marker of heavy investment and preparation for future elite crafts.",
        facts: [
          "Cyan rarity",
          "Sell value: 15 gold",
          "Requires 10 Rosalita Gem, 10 Shadow Bar, 10 Lunar Bar and 10 Chaos Crystal",
          "Uses Lunar Crafting Station"
        ],
        obtain: [
          "Crafted with 10 Rosalita Gem + 10 Shadow Bar + 10 Lunar Bar + 10 Chaos Crystal at a Lunar Crafting Station.",
          "The recipe concentrates materials from several stages, making it a clear capstone material for the branch."
        ],
        crafting: [
          "Rosalita Gem x10 + Shadow Bar x10 + Lunar Bar x10 + Chaos Crystal x10 at Lunar Crafting Station"
        ],
        notes: [
          "Even before it gains more public follow-up crafts, it already deserves its own page because it captures the current maximum cost of the MinecraftLegacy line."
        ]
      }
    }
  }
];
