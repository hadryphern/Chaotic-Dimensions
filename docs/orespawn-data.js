export const frontierUiCopy = {
  "pt-BR": {
    categories: {
      mobs: "Mobs"
    },
    nav: {
      frontier: "Fronteira"
    },
    frontier: {
      eyebrow: "OreSpawn Frontier",
      title: "Wave 1 ja virou parte viva da wiki.",
      intro: "Os prototipos OreSpawn ja tem progressao, spawn, drops e invocadores no codigo. Esta secao transforma isso em leitura rapida enquanto a expansao cresce.",
      metrics: {
        mobs: "Mobs prototipo",
        summons: "Invocadores",
        gates: "Gates reais",
        biomes: "Biomas ativos"
      },
      rosterTitle: "Bestiario em producao",
      rosterIntro: "Clique em qualquer criatura para abrir a ficha completa no catalogo abaixo.",
      supportTitle: "Invocadores e recompensa",
      supportIntro: "A wave ja tem itens utilitarios para chamar minibosses e uma arma de recompensa garantida.",
      openEntry: "Abrir no catalogo"
    }
  },
  en: {
    categories: {
      mobs: "Mobs"
    },
    nav: {
      frontier: "Frontier"
    },
    frontier: {
      eyebrow: "OreSpawn Frontier",
      title: "Wave 1 is now a living part of the wiki.",
      intro: "The OreSpawn prototypes already have progression gates, spawn rules, drops and summon items in code. This section turns that into fast reading while the expansion grows.",
      metrics: {
        mobs: "Prototype mobs",
        summons: "Summon items",
        gates: "Real gates",
        biomes: "Active biomes"
      },
      rosterTitle: "Bestiary in production",
      rosterIntro: "Click any creature to open its full sheet inside the explorer below.",
      supportTitle: "Summons and reward",
      supportIntro: "The wave already has utility items for calling minibosses and one guaranteed reward weapon.",
      openEntry: "Open in explorer"
    }
  },
  es: {
    categories: {
      mobs: "Mobs"
    },
    nav: {
      frontier: "Frontera"
    },
    frontier: {
      eyebrow: "OreSpawn Frontier",
      title: "La Wave 1 ya forma parte viva de la wiki.",
      intro: "Los prototipos OreSpawn ya tienen progresion, spawns, drops e invocadores en el codigo. Esta seccion lo convierte en lectura rapida mientras la expansion crece.",
      metrics: {
        mobs: "Mobs prototipo",
        summons: "Invocadores",
        gates: "Gates reales",
        biomes: "Biomas activos"
      },
      rosterTitle: "Bestiario en produccion",
      rosterIntro: "Haz clic en cualquier criatura para abrir su ficha completa dentro del explorador.",
      supportTitle: "Invocadores y recompensa",
      supportIntro: "La wave ya tiene objetos para llamar minibosses y un arma de recompensa garantizada.",
      openEntry: "Abrir en catalogo"
    }
  },
  ru: {
    categories: {
      mobs: "Mobs"
    },
    nav: {
      frontier: "Frontier"
    },
    frontier: {
      eyebrow: "OreSpawn Frontier",
      title: "Wave 1 already lives inside the wiki.",
      intro: "The OreSpawn prototypes already ship with progression gates, spawn rules, drops and summon items in code. This section turns that into readable documentation while the expansion grows.",
      metrics: {
        mobs: "Prototype mobs",
        summons: "Summon items",
        gates: "Real gates",
        biomes: "Active biomes"
      },
      rosterTitle: "Bestiary in production",
      rosterIntro: "Click a creature to open its full sheet in the explorer below.",
      supportTitle: "Summons and reward",
      supportIntro: "This wave already has repeatable summon tools and one guaranteed reward weapon.",
      openEntry: "Open in explorer"
    }
  }
};

export const frontierSection = {
  rosterIds: [
    "water-dragon",
    "mantis",
    "caterkiller",
    "emperor-scorpion",
    "hercules",
    "cephadrome"
  ],
  supportIds: [
    "caterkiller-bait",
    "emperor-scorpion-idol",
    "hercules-totem",
    "cephadrome-caller",
    "big-hammer"
  ]
};

export const frontierEntries = [
  {
    id: "water-dragon",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/WaterDragon.png",
    related: ["mantis"],
    content: {
      "pt-BR": {
        title: "Water Dragon",
        subtitle: "Predador costeiro voador",
        summary: "Mob de entrada da linha OreSpawn que patrulha praia e oceano logo apos Eye of Cthulhu.",
        overview: "Water Dragon e uma das primeiras criaturas da frente OreSpawn. Ele usa AI aerea, aparece na praia e ja entrega material proprio para empurrar a identidade da expansao cedo no progresso.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: praia e oceano",
          "24 damage, 8 defense, 260 life",
          "Criatura voadora com IA de bat"
        ],
        drops: [
          "Water Dragon Scale x1-2",
          "Iron Bar x3-5",
          "Lead Bar x3-5"
        ],
        notes: [
          "Boa primeira vitrine do tema OreSpawn fora da linha cristalina."
        ],
        tactics: [
          "Luta melhor em espaco vertical, porque ele ignora gravidade e colisao com tiles."
        ]
      },
      en: {
        title: "Water Dragon",
        subtitle: "Flying coastal predator",
        summary: "An early OreSpawn creature that patrols the beach and ocean right after Eye of Cthulhu.",
        overview: "Water Dragon is one of the first creatures in the OreSpawn front. It uses flying AI, appears on the coast and already drops its own material to establish the expansion early in progression.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: beach and ocean",
          "24 damage, 8 defense, 260 life",
          "Flying creature using bat-style AI"
        ],
        drops: [
          "Water Dragon Scale x1-2",
          "Iron Bar x3-5",
          "Lead Bar x3-5"
        ],
        notes: [
          "A strong first showcase for the OreSpawn theme outside the crystal line."
        ],
        tactics: [
          "Fight it with vertical space, since it ignores gravity and tile collision."
        ]
      },
      es: {
        title: "Water Dragon",
        subtitle: "Depredador costero volador",
        summary: "Mob temprano de la linea OreSpawn que patrulla playa y oceano justo despues de Eye of Cthulhu.",
        overview: "Water Dragon es una de las primeras criaturas del frente OreSpawn. Usa IA aerea, aparece en la costa y ya entrega su propio material para marcar la expansion desde temprano.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: playa y oceano",
          "24 damage, 8 defense, 260 life",
          "Criatura voladora con IA tipo bat"
        ],
        drops: [
          "Water Dragon Scale x1-2",
          "Iron Bar x3-5",
          "Lead Bar x3-5"
        ],
        notes: [
          "Es una buena primera muestra del tema OreSpawn fuera de la linea cristalina."
        ],
        tactics: [
          "Conviene pelearlo con espacio vertical porque ignora gravedad y colision."
        ]
      },
      ru: {
        title: "Water Dragon",
        subtitle: "Flying coastal predator",
        summary: "Early OreSpawn mob that patrols beach and ocean right after Eye of Cthulhu.",
        overview: "Water Dragon is one of the first creatures in the OreSpawn front. It uses flying AI, appears on the coast and already drops its own material so the expansion shows up early in progression.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: beach and ocean",
          "24 damage, 8 defense, 260 life",
          "Flying creature using bat-style AI"
        ],
        drops: [
          "Water Dragon Scale x1-2",
          "Iron Bar x3-5",
          "Lead Bar x3-5"
        ],
        notes: [
          "Strong first showcase for the OreSpawn theme outside the crystal line."
        ],
        tactics: [
          "Fight it with vertical space, since it ignores gravity and tile collision."
        ]
      }
    }
  },
  {
    id: "mantis",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/Mantis.png",
    related: ["water-dragon"],
    content: {
      "pt-BR": {
        title: "Mantis",
        subtitle: "Cacador terrestre de floresta e jungle",
        summary: "Mob pos-Eye of Cthulhu com presenca rapida em superficie de floresta ou jungle.",
        overview: "Mantis empurra o lado terrestre da wave OreSpawn inicial. E mais fragil que os minibosses, mas aparece com frequencia alta e ja cria uma rota de farm dedicada para material proprio.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: floresta ou jungle de superficie",
          "28 damage, 10 defense, 220 life",
          "Chance de spawn alta para a wave"
        ],
        drops: [
          "Mantis Claw x1-2"
        ],
        notes: [
          "Serve como criatura base para apresentar a fauna OreSpawn antes dos minibosses."
        ],
        tactics: [
          "Como usa AI terrestre, terreno plano ajuda a controlar o avanco."
        ]
      },
      en: {
        title: "Mantis",
        subtitle: "Forest and jungle ground hunter",
        summary: "A post-Eye of Cthulhu mob with a fast presence on forest or jungle surface.",
        overview: "Mantis pushes the terrestrial side of the early OreSpawn wave. It is lighter than the minibosses, but spawns often and creates an immediate farm route for its own material.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: forest or jungle surface",
          "28 damage, 10 defense, 220 life",
          "High spawn weight for the wave"
        ],
        drops: [
          "Mantis Claw x1-2"
        ],
        notes: [
          "It works as the base creature that introduces OreSpawn fauna before the minibosses."
        ],
        tactics: [
          "Since it uses ground AI, flatter terrain makes it easier to control."
        ]
      },
      es: {
        title: "Mantis",
        subtitle: "Cazador terrestre de bosque y jungle",
        summary: "Mob post-Eye of Cthulhu con presencia rapida en superficie de bosque o jungle.",
        overview: "Mantis empuja el lado terrestre de la wave inicial de OreSpawn. Es mas fragil que los minibosses, pero aparece mucho y crea una ruta temprana de farmeo para su propio material.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: bosque o jungle de superficie",
          "28 damage, 10 defense, 220 life",
          "Alta tasa de spawn para la wave"
        ],
        drops: [
          "Mantis Claw x1-2"
        ],
        notes: [
          "Funciona como criatura base para presentar la fauna OreSpawn antes de los minibosses."
        ],
        tactics: [
          "Como usa IA terrestre, el terreno plano ayuda a controlar su avance."
        ]
      },
      ru: {
        title: "Mantis",
        subtitle: "Forest and jungle ground hunter",
        summary: "Post-Eye of Cthulhu mob with a fast presence on forest or jungle surface.",
        overview: "Mantis pushes the ground side of the early OreSpawn wave. It is lighter than the minibosses, but spawns often and creates an immediate farm route for its own material.",
        facts: [
          "Gate: Post Eye of Cthulhu",
          "Spawn: forest or jungle surface",
          "28 damage, 10 defense, 220 life",
          "High spawn weight for the wave"
        ],
        drops: [
          "Mantis Claw x1-2"
        ],
        notes: [
          "Base creature that introduces OreSpawn fauna before the minibosses."
        ],
        tactics: [
          "Because it uses ground AI, flatter terrain makes it easier to control."
        ]
      }
    }
  },
  {
    id: "caterkiller",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/Caterkiller.png",
    related: ["caterkiller-bait"],
    content: {
      "pt-BR": {
        title: "Caterkiller",
        subtitle: "Miniboss de floresta / jungle",
        summary: "Miniboss pos-Evil Boss que usa AI agressiva terrestre e dropa um jaw garantido.",
        overview: "Caterkiller abre a camada de minibosses OreSpawn na superficie verde do mundo. Ele ocupa bastante espaco, tem vida alta para o estagio e pode ser chamado com item proprio.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: floresta ou jungle de superficie",
          "44 damage, 18 defense, 1425 life",
          "Miniboss com slot alto e spawn unico"
        ],
        drops: [
          "Caterkiller Jaw garantido",
          "Ruby x3-6"
        ],
        notes: [
          "Usa Black Recluse como referencia de AI.",
          "Tem summon dedicado: Caterkiller Bait."
        ],
        tactics: [
          "Segurar distancia horizontal ajuda a ler as investidas com menos caos."
        ]
      },
      en: {
        title: "Caterkiller",
        subtitle: "Forest / jungle miniboss",
        summary: "A post-Evil Boss miniboss with aggressive ground AI and a guaranteed jaw drop.",
        overview: "Caterkiller opens the OreSpawn miniboss layer on the green side of the surface world. It takes up real space, has high life for its stage and can be called with a dedicated summon item.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: forest or jungle surface",
          "44 damage, 18 defense, 1425 life",
          "Miniboss with high slot cost and unique spawn"
        ],
        drops: [
          "Guaranteed Caterkiller Jaw",
          "Ruby x3-6"
        ],
        notes: [
          "Uses Black Recluse as its AI reference.",
          "Has a dedicated summon: Caterkiller Bait."
        ],
        tactics: [
          "Keeping horizontal distance helps read its rushes with less chaos."
        ]
      },
      es: {
        title: "Caterkiller",
        subtitle: "Miniboss de bosque / jungle",
        summary: "Miniboss post-Evil Boss con IA terrestre agresiva y jaw garantizada como drop.",
        overview: "Caterkiller abre la capa de minibosses OreSpawn en la superficie verde del mundo. Ocupa espacio real, tiene mucha vida para su etapa y puede invocarse con item propio.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: bosque o jungle de superficie",
          "44 damage, 18 defense, 1425 life",
          "Miniboss con alto costo de slot y spawn unico"
        ],
        drops: [
          "Caterkiller Jaw garantizado",
          "Ruby x3-6"
        ],
        notes: [
          "Usa Black Recluse como referencia de IA.",
          "Tiene summon dedicado: Caterkiller Bait."
        ],
        tactics: [
          "Mantener distancia horizontal ayuda a leer sus cargas con menos caos."
        ]
      },
      ru: {
        title: "Caterkiller",
        subtitle: "Forest / jungle miniboss",
        summary: "Post-Evil Boss miniboss with aggressive ground AI and a guaranteed jaw drop.",
        overview: "Caterkiller opens the OreSpawn miniboss layer on the green side of the surface world. It has high life for its stage and can be called with a dedicated summon item.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: forest or jungle surface",
          "44 damage, 18 defense, 1425 life",
          "Miniboss with high slot cost and unique spawn"
        ],
        drops: [
          "Guaranteed Caterkiller Jaw",
          "Ruby x3-6"
        ],
        notes: [
          "Uses Black Recluse as its AI reference.",
          "Has a dedicated summon: Caterkiller Bait."
        ],
        tactics: [
          "Keeping horizontal distance helps read its rushes with less chaos."
        ]
      }
    }
  },
  {
    id: "emperor-scorpion",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/EmperorScorpion.png",
    related: ["emperor-scorpion-idol"],
    content: {
      "pt-BR": {
        title: "Emperor Scorpion",
        subtitle: "Miniboss de deserto",
        summary: "Miniboss pos-Evil Boss que segura o deserto com veneno e muito drop de scale.",
        overview: "Emperor Scorpion e o braco desertico da wave. Ele entra cedo no pos-boss maligno, aplica Poisoned no contato e entrega um bloco forte de materiais para futuros crafts.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: deserto de superficie",
          "42 damage, 16 defense, 1200 life",
          "Imune a Poisoned"
        ],
        drops: [
          "Emperor Scorpion Scale x6-10",
          "Diamond x4-8"
        ],
        notes: [
          "Aplica Poisoned por 240 ticks ao atingir o jogador.",
          "Tem summon dedicado: Emperor Scorpion Idol."
        ],
        tactics: [
          "Levar antidoto ou mobilidade ajuda a estabilizar o veneno prolongado."
        ]
      },
      en: {
        title: "Emperor Scorpion",
        subtitle: "Desert miniboss",
        summary: "A post-Evil Boss desert miniboss that controls its biome with poison and heavy scale drops.",
        overview: "Emperor Scorpion is the desert arm of the wave. It enters right after the evil boss stage, inflicts Poisoned on hit and drops a strong stack of materials for future crafts.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: desert surface",
          "42 damage, 16 defense, 1200 life",
          "Immune to Poisoned"
        ],
        drops: [
          "Emperor Scorpion Scale x6-10",
          "Diamond x4-8"
        ],
        notes: [
          "Applies Poisoned for 240 ticks on hit.",
          "Has a dedicated summon: Emperor Scorpion Idol."
        ],
        tactics: [
          "Extra mobility or poison cleanup helps stabilize the fight."
        ]
      },
      es: {
        title: "Emperor Scorpion",
        subtitle: "Miniboss de desierto",
        summary: "Miniboss post-Evil Boss que domina el desierto con veneno y mucho drop de scale.",
        overview: "Emperor Scorpion es el brazo desertico de la wave. Entra temprano tras el boss maligno, aplica Poisoned al golpear y suelta una buena pila de materiales para futuros crafts.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: desierto de superficie",
          "42 damage, 16 defense, 1200 life",
          "Inmune a Poisoned"
        ],
        drops: [
          "Emperor Scorpion Scale x6-10",
          "Diamond x4-8"
        ],
        notes: [
          "Aplica Poisoned durante 240 ticks al golpear.",
          "Tiene summon dedicado: Emperor Scorpion Idol."
        ],
        tactics: [
          "Mas movilidad o limpieza de veneno ayuda a estabilizar el combate."
        ]
      },
      ru: {
        title: "Emperor Scorpion",
        subtitle: "Desert miniboss",
        summary: "Post-Evil Boss desert miniboss with poison pressure and heavy scale drops.",
        overview: "Emperor Scorpion is the desert arm of the wave. It appears early, inflicts Poisoned on hit and drops a strong stack of materials for future crafts.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: desert surface",
          "42 damage, 16 defense, 1200 life",
          "Immune to Poisoned"
        ],
        drops: [
          "Emperor Scorpion Scale x6-10",
          "Diamond x4-8"
        ],
        notes: [
          "Applies Poisoned for 240 ticks on hit.",
          "Has a dedicated summon: Emperor Scorpion Idol."
        ],
        tactics: [
          "Extra mobility or poison cleanup helps stabilize the fight."
        ]
      }
    }
  },
  {
    id: "hercules",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/Hercules.png",
    related: ["hercules-totem", "big-hammer"],
    content: {
      "pt-BR": {
        title: "Hercules",
        subtitle: "Miniboss de impacto bruto",
        summary: "Miniboss pos-Evil Boss de floresta / jungle com drop garantido da Big Hammer.",
        overview: "Hercules e um miniboss simples e eficiente para o catalogo: pressao de contato alta, vida razoavel e uma recompensa fixa que ja conversa com progressao de ferramenta e melee.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: floresta ou jungle de superficie",
          "38 damage, 14 defense, 960 life",
          "Usa Mimic como referencia de AI"
        ],
        drops: [
          "Big Hammer garantida"
        ],
        notes: [
          "Tem summon dedicado: Hercules Totem."
        ],
        tactics: [
          "Como o premio e garantido, repetir o summon e uma forma limpa de testar balanceamento."
        ]
      },
      en: {
        title: "Hercules",
        subtitle: "Raw impact miniboss",
        summary: "A post-Evil Boss forest / jungle miniboss with a guaranteed Big Hammer drop.",
        overview: "Hercules is a clean and efficient miniboss for the catalog: high contact pressure, respectable life and a fixed reward that already connects tool progression with melee identity.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: forest or jungle surface",
          "38 damage, 14 defense, 960 life",
          "Uses Mimic as its AI reference"
        ],
        drops: [
          "Guaranteed Big Hammer"
        ],
        notes: [
          "Has a dedicated summon: Hercules Totem."
        ],
        tactics: [
          "Because the reward is guaranteed, repeated summons are a clean way to test balancing."
        ]
      },
      es: {
        title: "Hercules",
        subtitle: "Miniboss de impacto bruto",
        summary: "Miniboss post-Evil Boss de bosque / jungle con drop garantizado de Big Hammer.",
        overview: "Hercules es un miniboss limpio y eficiente para el catalogo: presion de contacto alta, vida solida y una recompensa fija que ya conecta progresion de herramienta con identidad melee.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: bosque o jungle de superficie",
          "38 damage, 14 defense, 960 life",
          "Usa Mimic como referencia de IA"
        ],
        drops: [
          "Big Hammer garantizada"
        ],
        notes: [
          "Tiene summon dedicado: Hercules Totem."
        ],
        tactics: [
          "Como la recompensa es garantizada, repetir el summon sirve para probar balance."
        ]
      },
      ru: {
        title: "Hercules",
        subtitle: "Raw impact miniboss",
        summary: "Post-Evil Boss forest / jungle miniboss with a guaranteed Big Hammer drop.",
        overview: "Hercules is a clean and efficient miniboss for the catalog: high contact pressure, respectable life and a fixed reward that already connects tool progression with melee identity.",
        facts: [
          "Gate: Post Evil Boss",
          "Spawn: forest or jungle surface",
          "38 damage, 14 defense, 960 life",
          "Uses Mimic as its AI reference"
        ],
        drops: [
          "Guaranteed Big Hammer"
        ],
        notes: [
          "Has a dedicated summon: Hercules Totem."
        ],
        tactics: [
          "Because the reward is guaranteed, repeated summons are a clean way to test balancing."
        ]
      }
    }
  },
  {
    id: "cephadrome",
    category: "mobs",
    image: "./assets/images/content/NPCs/OreSpawn/Cephadrome.png",
    related: ["cephadrome-caller"],
    content: {
      "pt-BR": {
        title: "Cephadrome",
        subtitle: "Miniboss voador de neve",
        summary: "Miniboss pos-Wall of Flesh com dano forte, Frostburn e horn garantido.",
        overview: "Cephadrome eleva a wave para o hardmode. E voador, aparece na superficie gelada, tem o maior dano bruto da leva atual e funciona como alvo claro para a transicao do bioma de neve.",
        facts: [
          "Gate: Post Wall of Flesh",
          "Spawn: neve de superficie",
          "60 damage, 20 defense, 2400 life",
          "Miniboss voador com Frostburn ao contato"
        ],
        drops: [
          "Cephadrome Horn garantido",
          "Ruby x6-12"
        ],
        notes: [
          "Tem summon dedicado: Cephadrome Caller."
        ],
        tactics: [
          "Arena com teto aberto ajuda a reagir melhor a mobilidade aerea."
        ]
      },
      en: {
        title: "Cephadrome",
        subtitle: "Flying snow miniboss",
        summary: "A post-Wall of Flesh miniboss with strong damage, Frostburn and a guaranteed horn drop.",
        overview: "Cephadrome pushes the wave into hardmode. It flies, appears on the snow surface, has the highest raw damage of the current roster and acts as a clear target for snow biome progression.",
        facts: [
          "Gate: Post Wall of Flesh",
          "Spawn: snow surface",
          "60 damage, 20 defense, 2400 life",
          "Flying miniboss that inflicts Frostburn on hit"
        ],
        drops: [
          "Guaranteed Cephadrome Horn",
          "Ruby x6-12"
        ],
        notes: [
          "Has a dedicated summon: Cephadrome Caller."
        ],
        tactics: [
          "An arena with open ceiling helps answer its aerial movement."
        ]
      },
      es: {
        title: "Cephadrome",
        subtitle: "Miniboss volador de nieve",
        summary: "Miniboss post-Wall of Flesh con dano alto, Frostburn y horn garantizado.",
        overview: "Cephadrome empuja la wave hacia hardmode. Vuela, aparece en la nieve de superficie, tiene el mayor dano bruto del roster actual y sirve como objetivo claro para progresion en ese bioma.",
        facts: [
          "Gate: Post Wall of Flesh",
          "Spawn: nieve de superficie",
          "60 damage, 20 defense, 2400 life",
          "Miniboss volador que aplica Frostburn"
        ],
        drops: [
          "Cephadrome Horn garantizado",
          "Ruby x6-12"
        ],
        notes: [
          "Tiene summon dedicado: Cephadrome Caller."
        ],
        tactics: [
          "Una arena con techo abierto ayuda a responder mejor a su movilidad aerea."
        ]
      },
      ru: {
        title: "Cephadrome",
        subtitle: "Flying snow miniboss",
        summary: "Post-Wall of Flesh miniboss with strong damage, Frostburn and a guaranteed horn drop.",
        overview: "Cephadrome pushes the wave into hardmode. It flies, appears on the snow surface and carries the highest raw damage in the current roster.",
        facts: [
          "Gate: Post Wall of Flesh",
          "Spawn: snow surface",
          "60 damage, 20 defense, 2400 life",
          "Flying miniboss that inflicts Frostburn on hit"
        ],
        drops: [
          "Guaranteed Cephadrome Horn",
          "Ruby x6-12"
        ],
        notes: [
          "Has a dedicated summon: Cephadrome Caller."
        ],
        tactics: [
          "An arena with open ceiling helps answer its aerial movement."
        ]
      }
    }
  },
  {
    id: "caterkiller-bait",
    category: "summons",
    image: "./assets/images/content/Items/Summons/OreSpawn/CaterkillerBait.png",
    related: ["caterkiller"],
    content: {
      "pt-BR": {
        title: "Caterkiller Bait",
        subtitle: "Invocador de miniboss",
        summary: "Item permanente que chama Caterkiller assim que o gate pos-Evil Boss esta liberado.",
        overview: "Caterkiller Bait transforma o miniboss de floresta em encounter repetivel para teste, farm e apresentacao da wave na wiki.",
        facts: [
          "Nao consumivel",
          "Use style: HoldUp",
          "So funciona quando o gate Post Evil Boss esta ativo"
        ],
        crafting: [
          "25 Cobweb",
          "4 Stinger",
          "Anvil"
        ],
        notes: [
          "Nao pode ser usado se o Caterkiller ja estiver vivo."
        ]
      },
      en: {
        title: "Caterkiller Bait",
        subtitle: "Miniboss summon",
        summary: "A permanent item that summons Caterkiller once the Post Evil Boss gate is open.",
        overview: "Caterkiller Bait turns the forest miniboss into a repeatable encounter for testing, farming and showcasing the wave inside the wiki.",
        facts: [
          "Not consumable",
          "Use style: HoldUp",
          "Only works once the Post Evil Boss gate is active"
        ],
        crafting: [
          "25 Cobweb",
          "4 Stinger",
          "Anvil"
        ],
        notes: [
          "Cannot be used while Caterkiller is already alive."
        ]
      },
      es: {
        title: "Caterkiller Bait",
        subtitle: "Invocador de miniboss",
        summary: "Objeto permanente que llama a Caterkiller cuando el gate Post Evil Boss ya esta abierto.",
        overview: "Caterkiller Bait convierte al miniboss de bosque en un encounter repetible para pruebas, farmeo y presentacion dentro de la wiki.",
        facts: [
          "No consumible",
          "Use style: HoldUp",
          "Solo funciona con el gate Post Evil Boss activo"
        ],
        crafting: [
          "25 Cobweb",
          "4 Stinger",
          "Anvil"
        ],
        notes: [
          "No puede usarse si Caterkiller ya esta vivo."
        ]
      },
      ru: {
        title: "Caterkiller Bait",
        subtitle: "Miniboss summon",
        summary: "Permanent item that summons Caterkiller once the Post Evil Boss gate is open.",
        overview: "Caterkiller Bait turns the forest miniboss into a repeatable encounter for testing, farming and showcase work inside the wiki.",
        facts: [
          "Not consumable",
          "Use style: HoldUp",
          "Only works once the Post Evil Boss gate is active"
        ],
        crafting: [
          "25 Cobweb",
          "4 Stinger",
          "Anvil"
        ],
        notes: [
          "Cannot be used while Caterkiller is already alive."
        ]
      }
    }
  },
  {
    id: "emperor-scorpion-idol",
    category: "summons",
    image: "./assets/images/content/Items/Summons/OreSpawn/EmperorScorpionIdol.png",
    related: ["emperor-scorpion"],
    content: {
      "pt-BR": {
        title: "Emperor Scorpion Idol",
        subtitle: "Invocador de deserto",
        summary: "Chama Emperor Scorpion sob demanda para farm e testes do ramo desertico.",
        overview: "O idol consolida o encontro do scorpion como parte repetivel da wave, otimo para balancear drops de scale e dificuldade do bioma.",
        facts: [
          "Nao consumivel",
          "Gate: Post Evil Boss",
          "Bloqueado enquanto o miniboss esta vivo"
        ],
        crafting: [
          "8 Antlion Mandible",
          "5 Stinger",
          "Anvil"
        ]
      },
      en: {
        title: "Emperor Scorpion Idol",
        subtitle: "Desert summon",
        summary: "Summons Emperor Scorpion on demand for farming and testing the desert branch.",
        overview: "The idol makes the scorpion encounter fully repeatable, which is useful for tuning scale drops and biome difficulty.",
        facts: [
          "Not consumable",
          "Gate: Post Evil Boss",
          "Blocked while the miniboss is alive"
        ],
        crafting: [
          "8 Antlion Mandible",
          "5 Stinger",
          "Anvil"
        ]
      },
      es: {
        title: "Emperor Scorpion Idol",
        subtitle: "Invocador de desierto",
        summary: "Llama a Emperor Scorpion bajo demanda para farmeo y pruebas de la rama desertica.",
        overview: "El idol vuelve repetible el encuentro del scorpion, util para ajustar drops de scale y dificultad del bioma.",
        facts: [
          "No consumible",
          "Gate: Post Evil Boss",
          "Bloqueado mientras el miniboss esta vivo"
        ],
        crafting: [
          "8 Antlion Mandible",
          "5 Stinger",
          "Anvil"
        ]
      },
      ru: {
        title: "Emperor Scorpion Idol",
        subtitle: "Desert summon",
        summary: "Summons Emperor Scorpion on demand for farming and testing the desert branch.",
        overview: "The idol makes the scorpion encounter repeatable, which is useful for tuning scale drops and biome difficulty.",
        facts: [
          "Not consumable",
          "Gate: Post Evil Boss",
          "Blocked while the miniboss is alive"
        ],
        crafting: [
          "8 Antlion Mandible",
          "5 Stinger",
          "Anvil"
        ]
      }
    }
  },
  {
    id: "hercules-totem",
    category: "summons",
    image: "./assets/images/content/Items/Summons/OreSpawn/HerculesTotem.png",
    related: ["hercules", "big-hammer"],
    content: {
      "pt-BR": {
        title: "Hercules Totem",
        subtitle: "Invocador de floresta / jungle",
        summary: "Ferramenta permanente para repetir o encontro do Hercules ate sair a Big Hammer.",
        overview: "Como o drop principal do Hercules e garantido, o totem funciona muito bem como peca de showcase: craft simples, loop curto e recompensa clara.",
        facts: [
          "Nao consumivel",
          "Gate: Post Evil Boss",
          "Use style: HoldUp"
        ],
        crafting: [
          "2 Vine",
          "6 Jungle Spores",
          "Anvil"
        ]
      },
      en: {
        title: "Hercules Totem",
        subtitle: "Forest / jungle summon",
        summary: "A permanent tool for repeating the Hercules encounter until the Big Hammer is yours.",
        overview: "Because Hercules drops a guaranteed main reward, the totem works very well as a showcase piece: simple recipe, short loop and clear payoff.",
        facts: [
          "Not consumable",
          "Gate: Post Evil Boss",
          "Use style: HoldUp"
        ],
        crafting: [
          "2 Vine",
          "6 Jungle Spores",
          "Anvil"
        ]
      },
      es: {
        title: "Hercules Totem",
        subtitle: "Invocador de bosque / jungle",
        summary: "Herramienta permanente para repetir el encuentro con Hercules hasta conseguir Big Hammer.",
        overview: "Como Hercules deja una recompensa principal garantizada, el totem funciona muy bien como pieza de showcase: receta simple, loop corto y premio claro.",
        facts: [
          "No consumible",
          "Gate: Post Evil Boss",
          "Use style: HoldUp"
        ],
        crafting: [
          "2 Vine",
          "6 Jungle Spores",
          "Anvil"
        ]
      },
      ru: {
        title: "Hercules Totem",
        subtitle: "Forest / jungle summon",
        summary: "Permanent tool for repeating the Hercules encounter until the Big Hammer drops.",
        overview: "Because Hercules has a guaranteed reward, the totem works well as a showcase piece with a short loop and a clear payoff.",
        facts: [
          "Not consumable",
          "Gate: Post Evil Boss",
          "Use style: HoldUp"
        ],
        crafting: [
          "2 Vine",
          "6 Jungle Spores",
          "Anvil"
        ]
      }
    }
  },
  {
    id: "cephadrome-caller",
    category: "summons",
    image: "./assets/images/content/Items/Summons/OreSpawn/CephadromeCaller.png",
    related: ["cephadrome"],
    content: {
      "pt-BR": {
        title: "Cephadrome Caller",
        subtitle: "Invocador hardmode de neve",
        summary: "Atalho direto para o miniboss gelado da wave assim que o hardmode abre.",
        overview: "Cephadrome Caller transforma o pico de dificuldade do bioma nevado em uma encounter controlada e repetivel, ideal para testar numeros de hardmode.",
        facts: [
          "Nao consumivel",
          "Gate: Post Wall of Flesh",
          "Use style: HoldUp"
        ],
        crafting: [
          "8 Ruby",
          "5 Soul of Light",
          "1 Ice Feather",
          "Mythril Anvil"
        ]
      },
      en: {
        title: "Cephadrome Caller",
        subtitle: "Hardmode snow summon",
        summary: "A direct shortcut to the icy miniboss as soon as hardmode opens.",
        overview: "Cephadrome Caller turns the snow biome spike into a controlled and repeatable encounter, which is ideal for tuning hardmode numbers.",
        facts: [
          "Not consumable",
          "Gate: Post Wall of Flesh",
          "Use style: HoldUp"
        ],
        crafting: [
          "8 Ruby",
          "5 Soul of Light",
          "1 Ice Feather",
          "Mythril Anvil"
        ]
      },
      es: {
        title: "Cephadrome Caller",
        subtitle: "Invocador hardmode de nieve",
        summary: "Atajo directo al miniboss helado de la wave cuando se abre hardmode.",
        overview: "Cephadrome Caller convierte el pico de dificultad del bioma nevado en un encounter controlado y repetible, ideal para ajustar numeros de hardmode.",
        facts: [
          "No consumible",
          "Gate: Post Wall of Flesh",
          "Use style: HoldUp"
        ],
        crafting: [
          "8 Ruby",
          "5 Soul of Light",
          "1 Ice Feather",
          "Mythril Anvil"
        ]
      },
      ru: {
        title: "Cephadrome Caller",
        subtitle: "Hardmode snow summon",
        summary: "Direct shortcut to the icy miniboss as soon as hardmode opens.",
        overview: "Cephadrome Caller turns the snow biome spike into a controlled and repeatable encounter, ideal for tuning hardmode numbers.",
        facts: [
          "Not consumable",
          "Gate: Post Wall of Flesh",
          "Use style: HoldUp"
        ],
        crafting: [
          "8 Ruby",
          "5 Soul of Light",
          "1 Ice Feather",
          "Mythril Anvil"
        ]
      }
    }
  },
  {
    id: "big-hammer",
    category: "weapons",
    image: "./assets/images/content/Items/Weapons/Melee/BigHammer.png",
    related: ["hercules", "hercules-totem"],
    content: {
      "pt-BR": {
        title: "Big Hammer",
        subtitle: "Reward weapon / hammer",
        summary: "Martelo melee garantido do Hercules com poder de ferramenta e loop de farm direto.",
        overview: "Big Hammer ja nasce como peca otima para a wiki: reward clara, identidade visual forte e funcao hibrida entre arma e ferramenta.",
        facts: [
          "48 melee damage",
          "30 use time / 30 animation",
          "9 knockback",
          "80 hammer power",
          "Auto reuse"
        ],
        obtain: [
          "Drop garantido do Hercules"
        ],
        notes: [
          "E um otimo exemplo de recompensa que comunica valor imediato para o jogador."
        ]
      },
      en: {
        title: "Big Hammer",
        subtitle: "Reward weapon / hammer",
        summary: "A guaranteed Hercules melee hammer with tool power and a very direct farm loop.",
        overview: "Big Hammer is already a strong wiki piece: clear reward, strong visual identity and a hybrid role between weapon and tool.",
        facts: [
          "48 melee damage",
          "30 use time / 30 animation",
          "9 knockback",
          "80 hammer power",
          "Auto reuse"
        ],
        obtain: [
          "Guaranteed drop from Hercules"
        ],
        notes: [
          "It is a great example of a reward that communicates immediate value to the player."
        ]
      },
      es: {
        title: "Big Hammer",
        subtitle: "Reward weapon / hammer",
        summary: "Martillo melee garantizado de Hercules con poder de herramienta y loop de farmeo directo.",
        overview: "Big Hammer ya funciona muy bien en la wiki: recompensa clara, identidad visual fuerte y rol hibrido entre arma y herramienta.",
        facts: [
          "48 melee damage",
          "30 use time / 30 animation",
          "9 knockback",
          "80 hammer power",
          "Auto reuse"
        ],
        obtain: [
          "Drop garantizado de Hercules"
        ],
        notes: [
          "Es un gran ejemplo de recompensa con valor inmediato para el jugador."
        ]
      },
      ru: {
        title: "Big Hammer",
        subtitle: "Reward weapon / hammer",
        summary: "Guaranteed Hercules melee hammer with tool power and a direct farm loop.",
        overview: "Big Hammer is already a strong wiki piece with a clear reward, strong visual identity and a hybrid role between weapon and tool.",
        facts: [
          "48 melee damage",
          "30 use time / 30 animation",
          "9 knockback",
          "80 hammer power",
          "Auto reuse"
        ],
        obtain: [
          "Guaranteed drop from Hercules"
        ],
        notes: [
          "It clearly communicates immediate value to the player."
        ]
      }
    }
  }
];
