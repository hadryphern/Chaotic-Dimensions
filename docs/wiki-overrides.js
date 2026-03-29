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
  },
  {
    id: "crystal-creeper",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Crystal Creeper"
      }
    }
  },
  {
    id: "happy-creeper",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Happy Creeper"
      }
    }
  },
  {
    id: "kraken",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Kraken"
      }
    }
  },
  {
    id: "shadow-creeper",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Shadow Creeper"
      }
    }
  },
  {
    id: "snow-blaze",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Snow Blaze"
      }
    }
  },
  {
    id: "squid-kraken",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Squid Kraken"
      }
    }
  },
  {
    id: "white-creeper",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "White Creeper"
      }
    }
  },
  {
    id: "endernmon",
    isPublished: false,
    content: {
      "pt-BR": {
        title: "Endernmon"
      }
    }
  },
  {
    id: "monthra",
    category: "bosses",
    image: "./assets/images/content/Bosses/Monthra/MonthraBoss_Head_Boss.png",
    related: ["monthra-scale", "monthra-butterfly", "monthra-butterfly-staff"],
    sortOrder: 110,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Monthra",
        subtitle: "Boss pre-hardmode",
        summary: "Boss aereo do pre-hardmode que entra depois do conteudo evil vanilla e serve como ponte para a linha Monthra/Rosalita.",
        overview: "Monthra e o encontro principal do tier Monthra. Ela luta quase sempre no ar, alternando um estado de hover lateral com volleys de fireballs e um estado agressivo de sweep em que mistura homing shot com salvas mais densas. A ideia do encounter atual e pressionar mobilidade e leitura de espaco sem exigir gear de hardmode.",
        facts: [
          "22.000 de vida base",
          "30 de dano e 10 de defesa",
          "12 frames de animacao",
          "Entra na fase dois abaixo de 50% de vida",
          "Padroes principais: Hover Volley e Sweeping Burst",
          "Escala atual de desenho aumentada para parecer maior que King Slime"
        ],
        obtain: [
          "A rota natural atual e encontrar e matar uma Monthra Butterfly.",
          "A critter so aparece depois que o segundo boss vanilla caiu, em superficie ou ceu, e invocar a borboleta ja faz Monthra descer imediatamente.",
          "Uma arena com varias plataformas, espaco lateral e um pouco de altura facilita bastante a luta."
        ],
        drops: [
          "22 a 30 Monthra Scales garantidas",
          "5 a 10 Healing Potions"
        ],
        tactics: [
          "Durante o hover, observe em que lado ela estabiliza antes de cada volley para se mover na diagonal oposta.",
          "Na fase dois os disparos ficam mais densos e entra um homing shot extra no ciclo agressivo, entao reserve mobilidade para reagir depois da primeira leva.",
          "Como o boss nao e absurdamente rapido, o mais importante e nao panic-jump em linha reta para dentro do leque de fogo."
        ],
        notes: [
          "A luta atual usa MusicID.Boss3 e continua aberta a refinamentos de pattern, drops e efeitos visuais.",
          "Monthra funciona como base de craft para a linha Monthra e para o reforco Eclipsed Monthra."
        ]
      }
    }
  },
  {
    id: "monthra-butterfly",
    category: "mobs",
    image: "./assets/images/content/NPCs/Critters/MonthraButterfly.png",
    related: ["monthra", "monthra-butterfly-staff"],
    sortOrder: 111,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Monthra Butterfly",
        subtitle: "Critter de invocacao",
        summary: "Borboleta critter que vagueia pela superficie e chama Monthra quando morre.",
        overview: "Monthra Butterfly e o gatilho natural da luta contra Monthra. Ela e uma criatura passiva, sem dano, que voa em trajetorias leves de vaivem e reaproveita a fantasia visual de uma mariposa/borboleta cosmica do mod.",
        facts: [
          "20 de vida",
          "6 frames de animacao",
          "NPC passivo e contado como critter",
          "Nao causa dano nem usa ataque proprio",
          "Ao morrer, invoca Monthra se o boss ainda nao estiver ativo"
        ],
        obtain: [
          "So aparece depois que o Boss 2 vanilla foi derrotado.",
          "Pode spawnar na superficie e no ceu, com chance maior a noite.",
          "Chance atual: 0.012 de dia e 0.018 de noite em regioes abertas."
        ],
        drops: [
          "Nao possui drop padrao."
        ],
        notes: [
          "Se Monthra ja estiver viva, matar outra borboleta nao duplica o boss.",
          "O texto de invocacao atual exibido no jogo e 'Monthra desce do crepusculo...'."
        ]
      }
    }
  },
  {
    id: "monthra-butterfly-staff",
    category: "summons",
    image: "./assets/images/content/Items/Weapons/Summon/MonthraButterflyStaff.png",
    related: ["monthra", "monthra-scale", "monthra-butterfly"],
    sortOrder: 112,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Monthra Butterfly Staff",
        subtitle: "Summon pre-hardmode",
        summary: "Cajado de summon da linha Monthra que invoca uma borboleta ofensiva de orbita curta.",
        overview: "Monthra Butterfly Staff e a arma de summon basica do tier Monthra. Ao usar o cajado, o jogador invoca uma borboleta que orbita e persegue alvos de forma simples, servindo como primeiro passo da rota summoner ligada a Monthra.",
        facts: [
          "18 de dano summon",
          "10 de mana",
          "Raridade Orange",
          "Invoca MonthraButterflyMinion",
          "O minion ocupa 0.5 slot e orbita perto do jogador quando esta sem alvo"
        ],
        crafting: [
          "Monthra Scale x12 + Demonite Bar x10 + Shadow Scale x5 em Anvil",
          "ou Monthra Scale x12 + Crimtane Bar x10 + Tissue Sample x5 em Anvil"
        ],
        notes: [
          "A sprite do item agora usa a nova arte da Month enviada para o mod.",
          "Esta arma continua sendo a base do upgrade Shadow Summon Staff."
        ]
      }
    }
  },
  {
    id: "chaos-crystal",
    category: "materials",
    image: "./assets/images/content/Items/MinecraftLegacy/ChaosCrystal.png",
    related: ["ratrix-stick"],
    sortOrder: 210,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Chaos Crystal",
        subtitle: "Material de cristal",
        summary: "Cristal raro reservado para a linha mais cara de crafts e para encontros ligados ao futuro bioma de cristal.",
        overview: "Chaos Crystal e um material premium do mod que hoje ja existe em codigo, sprite e recipes, mas ainda depende de mais spawns cristalinos para ter uma rota natural mais rica. Mesmo assim, o item ja precisa estar catalogado porque aparece como ingrediente pesado em crafts de fim de cadeia como Ratrix Stick.",
        facts: [
          "Raridade Pink",
          "Valor de venda: 2 gold",
          "Usado como componente premium de craft",
          "Relacionado ao ecossistema futuro de cristal"
        ],
        obtain: [
          "Hoje ele aparece como material codado e reservado para drops de mobs cristalinos.",
          "Enquanto o bioma de cristal completo nao chega, trate o item como componente catalogado da progressao futura."
        ],
        notes: [
          "A pagina continua publicada mesmo sem rota natural final porque a wiki tambem documenta conteudo placeholder e progressao futura do mod."
        ]
      }
    }
  },
  {
    id: "chaos-crystal-pickaxe",
    category: "tools",
    image: "./assets/images/content/Items/ShadowBiome/ChaosCrystalPickaxe.png",
    related: ["crystaline-devourer", "shadow-ore", "shadow-biome"],
    sortOrder: 420,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Chaos Crystal Pickaxe",
        subtitle: "Ferramenta pos-Devour",
        summary: "Picareta vermelha do tier Crystaline/Shadow, criada para abrir o Shadow Ore e acelerar mineracao pesada.",
        overview: "Chaos Crystal Pickaxe e a ferramenta que marca a virada da progressao pos-Crystaline Devour. Ela nao e craftada no estado atual: entra diretamente no loot do boss e existe para garantir acesso ao Shadow Ore dentro do ShadowBiome.",
        facts: [
          "210 de dano melee",
          "300% de pick power",
          "useAnimation 10 e useTime 5",
          "Raridade Red",
          "Extremamente rapida para quebrar blocos"
        ],
        obtain: [
          "Dropa diretamente de Crystaline Devour.",
          "Ela e a chave de acesso pratica para minerar Shadow Ore sem ficar presa em pick tiers antigos."
        ],
        notes: [
          "No loop atual do mod, Shadow Ore so pode ser quebrado por picaretas com esse patamar de poder, e Chaos Crystal Pickaxe e a rota oficial."
        ]
      }
    }
  },
  {
    id: "heart-of-the-god",
    category: "consumables",
    image: "./assets/images/content/Items/ShadowBiome/HeartOfTheGod.png",
    related: ["crystaline-devourer"],
    sortOrder: 421,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Heart of the God",
        subtitle: "Consumivel permanente",
        summary: "Reliquia de vida permanente dropada por Crystaline Devour para estender o teto de vida depois de todo o progresso vanilla.",
        overview: "Heart of the God e o grande consumivel de vida do endgame atual. Ele so pode ser usado depois que o jogador ja consumiu todos os Life Crystals e todos os Life Fruits do Terraria base, e cada uso adiciona mais uma camada de vida permanente ao personagem.",
        facts: [
          "Dropa diretamente de Crystaline Devour",
          "2 usos maximos por personagem",
          "Cada uso concede +125 de vida maxima",
          "Total final adicional: +250 de vida"
        ],
        obtain: [
          "Derrote Crystaline Devour para conseguir o item.",
          "Depois, use apenas quando o personagem ja tiver todos os upgrades de vida vanilla consumidos."
        ],
        notes: [
          "O estado do item e salvo por personagem, nao por mundo.",
          "A ideia visual planejada e reforcar o coracao rosa em tiers mais altos da progressao."
        ]
      }
    }
  },
  {
    id: "heart-of-shadows",
    category: "accessories",
    image: "./assets/images/content/Items/ShadowBiome/HeartOfShadows.png",
    related: ["shadow-ascension", "shadow-ore", "godness-anvil", "shadow-melee-potion", "rosalita-shield", "crystaline-eye"],
    sortOrder: 422,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Heart of Shadows",
        subtitle: "Acessorio vermelho",
        summary: "Acessorio de pico do tier Shadow que comprime defesa, vida, mana, dano, mobilidade e suporte para todas as classes em uma unica peca.",
        overview: "Heart of Shadows funciona como o coracao do tier Shadow Ascension. Ele reaproveita o teleporte do Crystaline Eye e ao mesmo tempo empilha defesa extrema, velocidade, minions extras e escalonamento bruto de stats, posicionando a peca como acessorio de assinatura do late game atual.",
        facts: [
          "245 de defesa direta",
          "+260 de vida maxima e +280 de mana maxima",
          "+115% de dano generic e +22% de critico generic",
          "+8 minions maximos e +28% de alcance de chicote",
          "+42% move speed, +3.5 run speed e +18% endurance",
          "Remove knockback e reativa a logica de teleporte do Crystaline Eye"
        ],
        crafting: [
          "Hallowed Bar x35",
          "ShadowOre x50",
          "RosalitaGem x25",
          "MonthraScale x10",
          "ShadowScrap x23",
          "RosalitaShield x1",
          "CrystalineEye x1",
          "ShadowMeleePotion x1",
          "Godness Anvil"
        ],
        notes: [
          "Hoje o item ja existe em codigo e recipe, mesmo antes do boss Kraken King fechar totalmente a rota natural da bancada."
        ]
      }
    }
  },
  {
    id: "shadow-totem",
    category: "accessories",
    image: "./assets/images/content/Items/ShadowBiome/ShadowTotem.png",
    related: ["shadow-biome", "glory-boots"],
    sortOrder: 423,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Totem",
        subtitle: "Acessorio de bencao",
        summary: "Totem que anula a maldicao do ShadowBiome e permite atravessar o bioma sem cegueira, drenagem e lentidao extrema.",
        overview: "Shadow Totem existe para transformar o ShadowBiome em uma etapa de progressao real em vez de apenas uma parede de dano. Equipar o acessorio ativa a bencao de sombra no player, removendo a maldicao do biome e ainda entregando pequenos bonus de mobilidade e resistencia.",
        facts: [
          "12 de defesa",
          "+6% de velocidade de movimento",
          "+4% de endurance",
          "Ativa shadowBlessing no player",
          "Remove os debuffs ambientais do ShadowBiome"
        ],
        obtain: [
          "A fantasia final do mod liga o item ao drop do futuro Kraken Prince.",
          "Enquanto esse boss ainda nao entrou, o item segue catalogado e preparado para a rota natural futura."
        ],
        notes: [
          "Glory Boots herdam automaticamente essa protecao porque usam Shadow Totem no craft."
        ]
      }
    }
  },
  {
    id: "godness-anvil",
    category: "blocks",
    image: "./assets/images/content/Tiles/ShadowBiome/GodnessAnvilTile.png",
    related: ["shadow-ascension", "heart-of-shadows", "glory-boots"],
    sortOrder: 424,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Godness Anvil",
        subtitle: "Estacao de crafting Shadow",
        summary: "Bancada exclusiva do tier Shadow usada pelos crafts vermelhos do mod.",
        overview: "Godness Anvil e a estacao que amarra toda a camada Shadow Ascension. O tile ja existe, o item usa a mesma arte do tile e a bancada funciona como adjacencia de Mythril Anvil e Lunar Crafting Station, servindo como o grande ponto de convergencia dos crafts mais caros do mod.",
        facts: [
          "Tile custom 3x2",
          "AdjTiles: Mythril Anvil e Lunar Crafting Station",
          "Raridade Red como item",
          "Usada por Heart of Shadows, Glory Boots e todo o arsenal Shadow final"
        ],
        obtain: [
          "A fantasia final do mod liga a bancada ao drop do futuro Kraken King.",
          "Mesmo antes desse boss, a wiki ja documenta a bancada porque ela estrutura o tier Shadow no codigo atual."
        ],
        notes: [
          "A sprite do item e a mesma do tile, para manter identidade visual unica na wiki e no jogo."
        ]
      }
    }
  },
  {
    id: "shadow-eye",
    category: "mobs",
    image: "./assets/images/content/NPCs/ShadowBiome/ShadowEye.png",
    related: ["shadow-biome", "shadow-scrap", "soul-of-shadow"],
    sortOrder: 425,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Eye",
        subtitle: "Mob voador do ShadowBiome",
        summary: "Olho sombrio voador que patrulha superficie e cavernas do ShadowBiome com pressao constante e bom retorno de scrap.",
        overview: "Shadow Eye faz parte do pacote inicial de mobs do ShadowBiome. Ele usa o movimento voador hostil padrao da linha legacy e serve como ameaca de media mobilidade, perseguindo o jogador tanto na camada de superficie quanto nas cavernas do bioma.",
        facts: [
          "620 de vida",
          "82 de dano",
          "20 de defesa",
          "Hitbox 42x42",
          "Spawn chance: 0.05 na superficie e 0.06 no subterraneo"
        ],
        drops: [
          "Shadow Scrap x1-3",
          "1 a 2 Gold Coins",
          "Soul of Shadow x1-2 em hardmode"
        ],
        notes: [
          "Assim como os outros hostis do ShadowBiome, ele tenta sumir quando o alvo sai do bioma."
        ]
      }
    }
  },
  {
    id: "rosalita-armor",
    category: "armor",
    related: ["rosalita-helmet", "rosalita-breastplate", "rosalita-greaves"],
    sortOrder: 429,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Rosalita Armor",
        subtitle: "Set completo Rosalita",
        summary: "Set intermediario da linha Rosalita, organizado por pecas separadas para leitura clara de defesa, bonus por slot e bonus do conjunto."
      }
    }
  },
  {
    id: "rosalita-helmet",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Rosalita/RosalitaHelmet.png",
    related: ["rosalita-armor", "rosalita-breastplate", "rosalita-greaves"],
    sortOrder: 430,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Rosalita Helmet",
        subtitle: "Cabeca do set Rosalita",
        summary: "Capacete universal do set Rosalita, desenhado para reforcar todas as classes ao mesmo tempo.",
        facts: [
          "26 de defesa",
          "+8% de dano generic",
          "+6% de critico generic"
        ],
        crafting: [
          "Rosalita Gem x16 + Hallowed Bar x10 + Shadow Scrap x8 em Mythril Anvil"
        ],
        notes: [
          "Com peitoral e greaves fecha o set Rosalita e ativa o bonus hibrido do conjunto."
        ]
      }
    }
  },
  {
    id: "rosalita-breastplate",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Rosalita/RosalitaBreastplate.png",
    related: ["rosalita-armor", "rosalita-helmet", "rosalita-greaves"],
    sortOrder: 431,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Rosalita Breastplate",
        subtitle: "Peitoral do set Rosalita",
        summary: "Peca central do set Rosalita, combinando dano generic, minion extra e mana bonus.",
        facts: [
          "34 de defesa",
          "+10% de dano generic",
          "+1 minion",
          "+30 de mana maxima"
        ],
        crafting: [
          "Rosalita Gem x24 + Hallowed Bar x14 + Shadow Scrap x12 em Mythril Anvil"
        ]
      }
    }
  },
  {
    id: "rosalita-greaves",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Rosalita/RosalitaGreaves.png",
    related: ["rosalita-armor", "rosalita-helmet", "rosalita-breastplate"],
    sortOrder: 432,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Rosalita Greaves",
        subtitle: "Greaves do set Rosalita",
        summary: "Perneiras do set Rosalita focadas em velocidade de movimento e sustain leve.",
        facts: [
          "24 de defesa",
          "+12% de move speed",
          "+0.8 de max run speed",
          "+2 de life regen"
        ],
        crafting: [
          "Rosalita Gem x18 + Hallowed Bar x12 + Shadow Scrap x10 em Mythril Anvil"
        ]
      }
    }
  },
  {
    id: "shadow-armor",
    category: "armor",
    related: ["shadow-helmet", "shadow-breastplate", "shadow-greaves"],
    sortOrder: 432.5,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Armor",
        subtitle: "Set completo Shadow",
        summary: "Set avancado da linha Shadow, exibido por pecas para separar defesa, bonus individuais e bonus do conjunto com mais clareza."
      }
    }
  },
  {
    id: "shadow-helmet",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Shadow/ShadowHelmet.png",
    related: ["shadow-armor", "shadow-breastplate", "shadow-greaves"],
    sortOrder: 433,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Helmet",
        subtitle: "Cabeca do set Shadow",
        summary: "Capacete do set Shadow, feito para o tier pos-Crystaline Devour e com ganho brutal de ofensiva universal.",
        facts: [
          "52 de defesa",
          "+14% de dano generic",
          "+8% de critico generic"
        ],
        crafting: [
          "ShadowOre x48 + ShadowScrap x18 + SoulOfShadow x8 + RosalitaGem x14 em Godness Anvil"
        ],
        notes: [
          "Com peito e pernas fecha o set Shadow e libera o bonus global do conjunto."
        ]
      }
    }
  },
  {
    id: "shadow-breastplate",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Shadow/ShadowBreastplate.png",
    related: ["shadow-armor", "shadow-helmet", "shadow-greaves"],
    sortOrder: 434,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Breastplate",
        subtitle: "Peitoral do set Shadow",
        summary: "Peitoral do set Shadow, empilhando vida, mana, minions e endurance no mesmo slot.",
        facts: [
          "70 de defesa",
          "+160 de vida maxima",
          "+80 de mana maxima",
          "+2 minions",
          "+8% de endurance"
        ],
        crafting: [
          "ShadowOre x72 + ShadowScrap x24 + SoulOfShadow x12 + RosalitaGem x20 em Godness Anvil"
        ]
      }
    }
  },
  {
    id: "shadow-greaves",
    category: "armor",
    image: "./assets/images/content/Items/Armor/Shadow/ShadowGreaves.png",
    related: ["shadow-armor", "shadow-helmet", "shadow-breastplate"],
    sortOrder: 435,
    isPublished: true,
    content: {
      "pt-BR": {
        title: "Shadow Greaves",
        subtitle: "Greaves do set Shadow",
        summary: "Perneiras do set Shadow focadas em velocidade, aceleracao e sustain enquanto o jogador atravessa o ShadowBiome e o endgame do mod.",
        facts: [
          "46 de defesa",
          "+18% de move speed",
          "+1.8 de max run speed",
          "runAcceleration x1.24",
          "+4 de life regen"
        ],
        crafting: [
          "ShadowOre x56 + ShadowScrap x18 + SoulOfShadow x8 + RosalitaGem x16 em Godness Anvil"
        ]
      }
    }
  },
  {
    id: "crystaline-sword",
    category: "weapons",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-axe",
    category: "tools",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-gem",
    category: "materials",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-nugget",
    category: "materials",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-ore",
    category: "materials",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-pickaxe",
    category: "tools",
    isPublished: false,
    content: {}
  },
  {
    id: "ruby-sword",
    category: "weapons",
    isPublished: false,
    content: {}
  },
  {
    id: "shadow-dirt-block-shadow-dirt-tile",
    category: "blocks",
    isPublished: false,
    content: {}
  },
  {
    id: "shadow-stone-block-shadow-stone-tile",
    category: "blocks",
    isPublished: false,
    content: {}
  },
  {
    id: "shadow-wood-shadow-wood-tile",
    category: "blocks",
    isPublished: false,
    content: {}
  },
  {
    id: "shadow-grass-tile",
    category: "blocks",
    isPublished: false,
    content: {}
  }
];
