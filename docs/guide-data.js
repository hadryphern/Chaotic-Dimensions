const t = (ptBR, en) => ({
  "pt-BR": ptBR,
  en
});

const notes = (ptBR, en) => ({
  "pt-BR": ptBR,
  en
});

const preset = (armor, weapons, accessories, buffs, ptNotes, enNotes) => ({
  armor,
  weapons,
  accessories,
  buffs,
  notes: notes(ptNotes, enNotes)
});

export const guidePhaseOrder = [
  "pre_hardmode",
  "early_hardmode",
  "late_hardmode",
  "celestial",
  "post_moonlord"
];

export const guideSetupPresets = {
  pre_early: {
    melee: preset(["Platinum Armor", "Gold Armor"], ["Ice Blade", "Platinum Broadsword", "Mace"], ["Hermes Boots", "Cloud in a Bottle", "Shark Tooth Necklace"], ["Ironskin Potion", "Swiftness Potion", "Well Fed"], ["Jogue por mobilidade e consistencia antes de tentar burst.", "Armas com projecao curta ajudam muito nessa etapa."], ["Play for mobility and consistency before forcing burst.", "Short projectile weapons help a lot at this stage."]),
    ranged: preset(["Fossil Armor", "Platinum Armor"], ["Demon Bow", "Tendon Bow", "Boomstick"], ["Hermes Boots", "Cloud in a Bottle", "Shark Tooth Necklace"], ["Archery Potion", "Ironskin Potion", "Swiftness Potion"], ["Frostburn Arrows continuam valendo quase tudo aqui.", "Boomstick entra muito bem se a arena for curta."], ["Frostburn Arrows carry most early fights.", "Boomstick is great if the arena stays compact."]),
    magic: preset(["Gem Robe", "Platinum Armor"], ["Diamond Staff", "Amber Staff", "Wand of Sparking"], ["Band of Starpower", "Hermes Boots", "Cloud in a Bottle"], ["Magic Power Potion", "Mana Regeneration Potion", "Ironskin Potion"], ["Gem staves sao a base mais limpa para magic no comeco.", "Nao estoure toda a mana em panic cast."], ["Gem staves are the cleanest early magic baseline.", "Do not dump your whole mana bar into panic casts."]),
    summoner: preset(["Flinx Fur Coat", "Platinum Armor"], ["Flinx Staff", "Finch Staff", "Snapthorn"], ["Pygmy Necklace", "Hermes Boots", "Cloud in a Bottle"], ["Summoning Potion", "Bewitching Table", "Swiftness Potion"], ["Summoner ainda joga como hibrido aqui; use o whip o tempo todo.", "Flinx Staff e a rota mais estavel se voce ja tiver o set."], ["Summoner still plays like a hybrid here; keep the whip active at all times.", "Flinx Staff is the steadiest route if you already built the set."])
  },
  pre_evil: {
    melee: preset(["Shadow Armor", "Crimson Armor", "Platinum Armor"], ["Ball O' Hurt", "Blade of Grass", "Amazon"], ["Feral Claws", "Shield of Cthulhu", "Hermes Boots"], ["Ironskin Potion", "Swiftness Potion", "Regeneration Potion"], ["Priorize armas que acertem varios segmentos ou creepers.", "O dash do Shield of Cthulhu limpa muitos erros de posicionamento."], ["Prioritize weapons that hit multiple segments or creepers.", "Shield of Cthulhu cleans up a lot of positioning mistakes."]),
    ranged: preset(["Fossil Armor", "Shadow Armor", "Crimson Armor"], ["Demon Bow", "Tendon Bow", "Boomstick"], ["Shark Tooth Necklace", "Shield of Cthulhu", "Hermes Boots"], ["Archery Potion", "Ironskin Potion", "Swiftness Potion"], ["Jester's Arrows e Boomstick simplificam muito as lutas evil.", "Nao lute preso ao terreno natural do bioma."], ["Jester's Arrows and Boomstick simplify the evil fights a lot.", "Do not fight while trapped in the natural biome terrain."]),
    magic: preset(["Jungle Armor", "Gem Robe"], ["Vilethorn", "Crimson Rod", "Diamond Staff"], ["Band of Starpower", "Mana Flower", "Hermes Boots"], ["Magic Power Potion", "Mana Regeneration Potion", "Ironskin Potion"], ["Vilethorn e uma das melhores respostas para Eater.", "Use a arma de area para fechar a fase de adds rapido."], ["Vilethorn is one of the best answers for Eater.", "Use area control tools to close the add phase quickly."]),
    summoner: preset(["Obsidian Armor", "Flinx Fur Coat"], ["Flinx Staff", "Vampire Frog Staff", "Snapthorn"], ["Pygmy Necklace", "Feral Claws", "Shield of Cthulhu"], ["Summoning Potion", "Bewitching Table", "Ironskin Potion"], ["Whip focus vale mais que perseguir com minions soltos.", "Obsidian Armor ja comeca a parecer um setup de verdade aqui."], ["Whip focus matters more than chasing with loose minions.", "Obsidian Armor starts feeling like a real setup here."])
  },
  pre_mid: {
    melee: preset(["Molten Armor", "Shadow Armor", "Crimson Armor"], ["Volcano", "Blade of Grass", "Sunfury"], ["Shield of Cthulhu", "Feral Claws", "Lightning Boots"], ["Ironskin Potion", "Swiftness Potion", "Regeneration Potion"], ["Molten Armor deixa esse bloco do pre-hardmode muito mais confortavel.", "Prefira dano limpo a contato forcado."], ["Molten Armor makes this slice of pre-Hardmode far more comfortable.", "Favor clean damage over forced contact."]),
    ranged: preset(["Fossil Armor", "Molten Armor"], ["Molten Fury", "Boomstick", "Minishark"], ["Shark Tooth Necklace", "Shield of Cthulhu", "Lightning Boots"], ["Archery Potion", "Ironskin Potion", "Swiftness Potion"], ["Molten Fury e a rota segura para chefes mais aereos.", "Boomstick ainda resolve bem encontros apertados."], ["Molten Fury is the safe route for more aerial fights.", "Boomstick still solves tighter encounters well."]),
    magic: preset(["Jungle Armor", "Meteor Armor"], ["Water Bolt", "Demon Scythe", "Space Gun"], ["Mana Flower", "Celestial Cuffs", "Lightning Boots"], ["Magic Power Potion", "Mana Regeneration Potion", "Ironskin Potion"], ["Water Bolt e a arma mais redonda desse trecho.", "Meteor Armor com Space Gun continua absurdamente confortavel."], ["Water Bolt is the most rounded weapon for this stretch.", "Meteor Armor with Space Gun remains absurdly comfortable."]),
    summoner: preset(["Obsidian Armor", "Bee Armor"], ["Imp Staff", "Hornet Staff", "Spinal Tap"], ["Pygmy Necklace", "Feral Claws", "Lightning Boots"], ["Summoning Potion", "Bewitching Table", "Ironskin Potion"], ["Obsidian Armor e o kit mais confiavel para esse trecho.", "Hornet Staff e Bee Armor entram como rota mais passiva."], ["Obsidian Armor is the most reliable kit for this stretch.", "Hornet Staff and Bee Armor offer the more passive route."])
  },
  pre_dungeon: {
    melee: preset(["Molten Armor"], ["Night's Edge", "Volcano", "Cascade"], ["Shield of Cthulhu", "Feral Claws", "Lightning Boots"], ["Ironskin Potion", "Swiftness Potion", "Regeneration Potion"], ["Chegue com um setup realmente fechado; essa e a ultima grande prova antes do Dungeon.", "Nao force hits ruins enquanto ainda ha alvos secundarios perigosos."], ["Arrive with a truly finished setup; this is the last big test before the Dungeon.", "Do not force bad hits while dangerous secondary targets are still alive."]),
    ranged: preset(["Necro Armor"], ["Hellwing Bow", "Phoenix Blaster", "Minishark"], ["Shark Tooth Necklace", "Shield of Cthulhu", "Lightning Boots"], ["Archery Potion", "Ironskin Potion", "Swiftness Potion"], ["Necro Armor com Hellwing Bow e uma das rotas mais estaveis do pre-hardmode.", "Phoenix Blaster ajuda quando voce quer foco mais previsivel."], ["Necro Armor with Hellwing Bow is one of the steadiest pre-Hardmode routes.", "Phoenix Blaster helps when you want more predictable focus damage."]),
    magic: preset(["Jungle Armor", "Meteor Armor"], ["Water Bolt", "Demon Scythe", "Space Gun"], ["Mana Flower", "Celestial Cuffs", "Lightning Boots"], ["Magic Power Potion", "Mana Regeneration Potion", "Ironskin Potion"], ["Water Bolt continua soberano no pre-Dungeon.", "Use Demon Scythe so em janelas em que a arena estiver realmente limpa."], ["Water Bolt remains king before the Dungeon.", "Use Demon Scythe only in windows where the arena is truly clean."]),
    summoner: preset(["Obsidian Armor"], ["Imp Staff", "Spinal Tap", "Hornet Staff"], ["Pygmy Necklace", "Feral Claws", "Lightning Boots"], ["Summoning Potion", "Bewitching Table", "Ironskin Potion"], ["Obsidian Armor continua sendo o kit que fecha a rota com mais seguranca.", "Whip focus ainda e o que decide a luta."], ["Obsidian Armor remains the setup that closes this route with the most safety.", "Whip focus is still what decides the fight."])
  },
  pre_wof: {
    melee: preset(["Molten Armor"], ["Night's Edge", "Sunfury", "Volcano"], ["Terraspark Boots", "Obsidian Shield", "Feral Claws"], ["Ironskin Potion", "Swiftness Potion", "Regeneration Potion"], ["O que mata tentativa aqui e perder runway, nao falta de dano.", "Leve uma resposta para os Hungries."], ["What kills runs here is losing the runway, not lacking damage.", "Bring a clean answer for the Hungries."]),
    ranged: preset(["Necro Armor"], ["Hellwing Bow", "Phoenix Blaster", "Beenades"], ["Terraspark Boots", "Shark Tooth Necklace", "Obsidian Shield"], ["Archery Potion", "Ironskin Potion", "Swiftness Potion"], ["Beenades continuam premium se voce quiser uma kill rapida.", "Hellwing Bow e o caminho mais estavel para um clear limpo."], ["Beenades remain premium if you want a fast kill.", "Hellwing Bow is the most stable route for a clean clear."]),
    magic: preset(["Meteor Armor", "Jungle Armor"], ["Water Bolt", "Demon Scythe", "Space Gun"], ["Mana Flower", "Celestial Cuffs", "Terraspark Boots"], ["Magic Power Potion", "Mana Regeneration Potion", "Ironskin Potion"], ["Meteor Armor com Space Gun ainda vale muito nessa luta.", "Water Bolt escala muito bem em runway limpo."], ["Meteor Armor plus Space Gun is still excellent here.", "Water Bolt scales very well on a clean runway."]),
    summoner: preset(["Obsidian Armor"], ["Imp Staff", "Hornet Staff", "Spinal Tap"], ["Pygmy Necklace", "Feral Claws", "Terraspark Boots"], ["Summoning Potion", "Bewitching Table", "Ironskin Potion"], ["Obsidian Armor segue como a resposta summon do pre-hardmode.", "Mantenha o foco do whip nos alvos grandes para nao quebrar a corrida."], ["Obsidian Armor remains the summon answer for pre-Hardmode.", "Keep whip focus on the large targets so you do not break your run."])
  },
  hardmode_open: {
    melee: preset(["Titanium Armor", "Frost Armor"], ["Amarok", "Shadowflame Knife", "Ice Sickle"], ["Wings", "Lightning Boots", "Warrior Emblem"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion"], ["Procure armas que continuem batendo sem contato constante.", "Esse trecho premia arena aerea limpa."], ["Look for weapons that keep hitting without constant contact.", "This stretch rewards a clean aerial arena."]),
    ranged: preset(["Titanium Armor", "Frost Armor"], ["Onyx Blaster", "Daedalus Stormbow", "Clockwork Assault Rifle"], ["Wings", "Lightning Boots", "Ranger Emblem"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion"], ["Daedalus e a rota de burst; Onyx, a rota de controle.", "Se sua arena for ruim, prefira armas de cadencia previsivel."], ["Daedalus is the burst route; Onyx is the control route.", "If your arena is weak, prefer weapons with predictable cadence."]),
    magic: preset(["Titanium Armor", "Forbidden Armor"], ["Sky Fracture", "Crystal Serpent", "Meteor Staff"], ["Wings", "Mana Flower", "Sorcerer Emblem"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion"], ["Sky Fracture e Crystal Serpent carregam muito bem essa etapa.", "Meteor Staff entra quando multi-hit vale o risco."], ["Sky Fracture and Crystal Serpent carry this phase very well.", "Meteor Staff comes in when multi-hit is worth the risk."]),
    summoner: preset(["Spider Armor"], ["Blade Staff", "Sanguine Staff", "Cool Whip"], ["Wings", "Pygmy Necklace", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion"], ["Blade Staff e Sanguine Staff sao os melhores alicerces do comeco do hardmode.", "Cool Whip ajuda a segurar foco quando a arena abre demais."], ["Blade Staff and Sanguine Staff are the best early Hardmode foundations.", "Cool Whip helps maintain focus once the arena opens up."])
  },
  mechs: {
    melee: preset(["Titanium Armor", "Frost Armor"], ["Shadowflame Knife", "Amarok", "Ice Sickle"], ["Wings", "Warrior Emblem", "Charm of Myths"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion"], ["Melee joga melhor com projecao nesse trio de bosses.", "Nao lute baixo demais contra worms e muita pressao de projeteis."], ["Melee performs better with ranged options against the mech trio.", "Do not fight too low against worms and heavy projectile pressure."]),
    ranged: preset(["Titanium Armor", "Frost Armor"], ["Daedalus Stormbow", "Onyx Blaster", "Clockwork Assault Rifle"], ["Wings", "Ranger Emblem", "Charm of Myths"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion"], ["Onyx e Daedalus continuam sendo os melhores pilares desse trecho.", "Escolha burst ou controle de acordo com o boss, nao por habito."], ["Onyx and Daedalus remain the two pillars of this stretch.", "Pick burst or control based on the boss, not just habit."]),
    magic: preset(["Forbidden Armor", "Titanium Armor"], ["Meteor Staff", "Sky Fracture", "Crystal Serpent"], ["Wings", "Mana Flower", "Sorcerer Emblem"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion"], ["Meteor Staff vence encontros de multi-hit; Sky Fracture segura alvo unico.", "Crystal Serpent entra como arma de ritmo e controle."], ["Meteor Staff wins multi-hit encounters; Sky Fracture handles single-target fights.", "Crystal Serpent steps in as the rhythm-and-control option."]),
    summoner: preset(["Spider Armor", "Forbidden Armor"], ["Blade Staff", "Sanguine Staff", "Cool Whip"], ["Wings", "Pygmy Necklace", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion"], ["Blade Staff e brutal no Destroyer; Sanguine Staff brilha nos alvos aereos.", "Use o whip para escolher o alvo principal da tentativa."], ["Blade Staff is brutal on the Destroyer; Sanguine Staff shines on aerial targets.", "Use the whip to choose the main target for each attempt."])
  },
  post_mech: {
    melee: preset(["Chlorophyte Armor", "Hallowed Armor"], ["True Night's Edge", "Death Sickle", "True Excalibur"], ["Wings", "Warrior Emblem", "Charm of Myths"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion"], ["Depois dos mechs, dano sustentado e arena boa valem mais que burst bruto.", "Death Sickle fica especialmente forte em janelas apertadas."], ["After the mechs, sustained damage and a good arena matter more than raw burst.", "Death Sickle becomes especially strong in tight windows."]),
    ranged: preset(["Chlorophyte Armor", "Hallowed Armor"], ["Megashark", "Chlorophyte Shotbow", "Daedalus Stormbow"], ["Wings", "Ranger Emblem", "Charm of Myths"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion"], ["Megashark e o kit mais generico e mais confiavel da fase.", "Shotbow cresce quando voce consegue manter o boss em angulos previsiveis."], ["Megashark is the most generic and reliable kit of the phase.", "Shotbow scales once you can hold the boss in predictable angles."]),
    magic: preset(["Chlorophyte Armor", "Forbidden Armor"], ["Sky Fracture", "Crystal Serpent", "Golden Shower"], ["Wings", "Mana Flower", "Sorcerer Emblem"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion"], ["Golden Shower entra em quase toda luta por causa do debuff.", "Sky Fracture e Crystal Serpent cobrem tanto fase 1 quanto fase 2."], ["Golden Shower enters almost every fight because of the debuff.", "Sky Fracture and Crystal Serpent cover both phase 1 and phase 2 cleanly."]),
    summoner: preset(["Spider Armor"], ["Sanguine Staff", "Blade Staff", "Durendal"], ["Wings", "Pygmy Necklace", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion"], ["Summoner ainda joga melhor com controle de whip do que com greed de minion.", "Durendal melhora muito o foco em target unico."], ["Summoner still performs better through whip control than minion greed.", "Durendal improves single-target focus a lot."])
  },
  post_plantera: {
    melee: preset(["Turtle Armor", "Beetle Armor"], ["Terra Blade", "Death Sickle", "Possessed Hatchet"], ["Wings", "Master Ninja Gear", "Warrior Emblem"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion"], ["Terra Blade vira a arma mais redonda desse trecho.", "Possessed Hatchet e a rota segura em fights mais tecnicas."], ["Terra Blade becomes the most rounded weapon in this stretch.", "Possessed Hatchet is the safe route in more technical fights."]),
    ranged: preset(["Shroomite Armor", "Chlorophyte Armor"], ["Megashark", "Tsunami", "Chlorophyte Shotbow"], ["Wings", "Master Ninja Gear", "Ranger Emblem"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion"], ["Shroomite e a subida natural para lutas mais longas.", "Tsunami e premium assim que voce o tiver."], ["Shroomite is the natural upgrade for longer fights.", "Tsunami becomes premium the moment you get it."]),
    magic: preset(["Spectre Armor"], ["Razorblade Typhoon", "Magnet Sphere", "Golden Shower"], ["Wings", "Mana Flower", "Master Ninja Gear"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion"], ["Spectre Armor muda completamente o conforto da classe.", "Golden Shower segue otimo mesmo quando suas armas ja sao fortes."], ["Spectre Armor completely changes how comfortable the class feels.", "Golden Shower stays great even after your main weapons get stronger."]),
    summoner: preset(["Tiki Armor", "Spooky Armor"], ["Raven Staff", "Xeno Staff", "Dark Harvest"], ["Wings", "Pygmy Necklace", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion"], ["Raven e Xeno Staff sao os pilares mais seguros desse trecho.", "Whips melhores comecam a ditar muito seu desempenho real."], ["Raven and Xeno Staff are the safest pillars of this stretch.", "Stronger whips start defining your real performance here."])
  },
  lunar: {
    melee: preset(["Beetle Armor"], ["Terra Blade", "Influx Waver", "Possessed Hatchet"], ["Wings", "Master Ninja Gear", "Warrior Emblem"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion"], ["Lute por consistencia para chegar inteiro ao trecho dos pilares.", "Nao desperdice recursos antes da reta final."], ["Fight for consistency so you reach the pillars healthy.", "Do not burn resources before the final stretch."]),
    ranged: preset(["Shroomite Armor"], ["Tsunami", "Chain Gun", "Xenopopper"], ["Wings", "Master Ninja Gear", "Ranger Emblem"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion"], ["A classe ja tem dano demais; o foco passa a ser uptime limpo.", "Prefira armas que te deixem ler o clone e o padrao com clareza."], ["The class already has enough damage; the focus shifts to clean uptime.", "Prefer weapons that let you read the clone and pattern clearly."]),
    magic: preset(["Spectre Armor"], ["Razorblade Typhoon", "Nightglow", "Laser Machinegun"], ["Wings", "Mana Flower", "Master Ninja Gear"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion"], ["A luta e curta: consistencia vale mais que greed de burst.", "Nightglow e Razorblade Typhoon simplificam bastante a leitura."], ["The fight is short: consistency matters more than greedy burst.", "Nightglow and Razorblade Typhoon simplify the read a lot."]),
    summoner: preset(["Spooky Armor", "Tiki Armor"], ["Xeno Staff", "Raven Staff", "Kaleidoscope"], ["Wings", "Pygmy Necklace", "Master Ninja Gear"], ["Summoning Potion", "Bewitching Table", "Endurance Potion"], ["Xeno Staff e especialmente forte contra lutas de alvo pequeno e rapido.", "Use whip para forcar foco limpo e evitar DPS perdido."], ["Xeno Staff is especially strong against small, fast targets.", "Use the whip to force clean focus and avoid wasted DPS."])
  },
  moon_lord: {
    melee: preset(["Beetle Armor"], ["Solar Eruption", "Daybreak", "Terra Blade"], ["Celestial Shell", "Fire Gauntlet", "Warrior Emblem"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion", "Well Fed"], ["Chegue com pelo menos uma arma lunar pronta antes de insistir na kill.", "Uptime inteligente vale mais do que correr atras de dano perfeito."], ["Arrive with at least one lunar weapon before you hard commit to the kill.", "Intelligent uptime matters more than chasing perfect damage."]),
    ranged: preset(["Shroomite Armor"], ["Phantasm", "Vortex Beater", "SDMG"], ["Celestial Shell", "Ranger Emblem", "Sniper Scope"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion", "Well Fed"], ["Phantasm e Vortex Beater formam a rota mais natural do fight.", "S.D.M.G. vira o teto quando voce ja tem a primeira kill."], ["Phantasm and Vortex Beater are the natural route for the fight.", "S.D.M.G. becomes the ceiling once you already have the first kill."]),
    magic: preset(["Spectre Armor"], ["Nebula Blaze", "Nebula Arcanum", "Lunar Flare"], ["Celestial Cuffs", "Mana Flower", "Sorcerer Emblem"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion", "Well Fed"], ["Nebula Blaze e Lunar Flare resolvem bem o fight inteiro.", "Controle de mana ainda importa porque a luta e longa."], ["Nebula Blaze and Lunar Flare handle the whole fight well.", "Mana control still matters because the fight is long."]),
    summoner: preset(["Spooky Armor", "Tiki Armor"], ["Stardust Dragon Staff", "Stardust Cell Staff", "Kaleidoscope"], ["Papyrus Scarab", "Pygmy Necklace", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion", "Well Fed"], ["Chegue com setup stardust fechado para nao alongar demais a tentativa.", "Kaleidoscope empurra muito o dano se voce consegue manter uptime."], ["Arrive with a finished stardust setup so the attempt does not drag on.", "Kaleidoscope pushes damage extremely hard if you can maintain uptime."])
  },
  post_moonlord: {
    melee: preset(["Solar Flare Armor"], ["Zenith", "Solar Eruption", "Daybreak"], ["Celestial Shell", "Fire Gauntlet", "Destroyer Emblem"], ["Ironskin Potion", "Endurance Potion", "Lifeforce Potion", "Well Fed"], ["Agora a luta testa sua arena e sua leitura, nao se o seu dano e suficiente.", "Nao troque mobilidade por um acessorio ganancioso."], ["Now the fight tests your arena and your reads, not whether your damage is high enough.", "Do not trade mobility away for a greedy accessory."]),
    ranged: preset(["Vortex Armor"], ["SDMG", "Phantasm", "Celebration Mk2"], ["Celestial Shell", "Ranger Emblem", "Sniper Scope"], ["Archery Potion", "Ammo Reservation Potion", "Endurance Potion", "Well Fed"], ["S.D.M.G. e Phantasm mantem pressao forte sem te travar.", "Celebration Mk2 cresce muito em arenas grandes."], ["S.D.M.G. and Phantasm keep up strong pressure without locking you down.", "Celebration Mk2 scales extremely well in large arenas."]),
    magic: preset(["Nebula Armor"], ["Last Prism", "Nebula Blaze", "Lunar Flare"], ["Celestial Cuffs", "Mana Flower", "Sorcerer Emblem"], ["Magic Power Potion", "Mana Regeneration Potion", "Endurance Potion", "Well Fed"], ["Last Prism e o teto; Nebula Blaze e a rota mais confortavel em movimento.", "Leve sustain suficiente para nao perder janelas longas."], ["Last Prism is the ceiling; Nebula Blaze is the more comfortable mobile route.", "Bring enough sustain so you do not lose long punish windows."]),
    summoner: preset(["Stardust Armor"], ["Terraprisma", "Stardust Dragon Staff", "Kaleidoscope"], ["Papyrus Scarab", "Necromantic Scroll", "Summoner Emblem"], ["Summoning Potion", "Bewitching Table", "Endurance Potion", "Well Fed"], ["Terraprisma entrega o melhor controle de alvo do trecho.", "Stardust Dragon continua fortissimo se voce ja domina o reposicionamento."], ["Terraprisma gives you the strongest target control of the phase.", "Stardust Dragon remains extremely strong if you already master repositioning."])
  }
};

export const bossGuideEntries = [
  {
    id: "king-slime",
    phase: "pre_hardmode",
    setupKey: "pre_early",
    source: "vanilla",
    optional: true,
    wikiTitle: "King Slime",
    title: t("King Slime", "King Slime"),
    summary: t("Boss opcional de abertura do vanilla.", "Optional opening boss."),
    when: t("Pode entrar antes do Eye of Cthulhu.", "Can be fought before Eye of Cthulhu."),
    arena: t("Use uma arena curta de dois andares.", "Use a short two-layer arena."),
    focus: t("Treine mobilidade simples e uptime limpo.", "Practice simple movement and clean uptime.")
  },
  {
    id: "eye-of-cthulhu",
    phase: "pre_hardmode",
    setupKey: "pre_early",
    source: "vanilla",
    optional: false,
    wikiTitle: "Eye of Cthulhu",
    title: t("Eye of Cthulhu", "Eye of Cthulhu"),
    summary: t("Primeiro boss central do vanilla.", "The first core vanilla boss."),
    when: t("Entre quando ja tiver mobilidade e armadura de minerio.", "Fight once you have movement gear and a proper ore armor set."),
    arena: t("Dois ou tres andares longos ja bastam.", "Two or three long layers are enough."),
    focus: t("Na fase 2, corra reto e troque de andar so quando necessario.", "In phase two, run straight and only change layers when needed.")
  },
  {
    id: "eater-of-worlds",
    phase: "pre_hardmode",
    setupKey: "pre_evil",
    source: "vanilla",
    optional: false,
    wikiTitle: "Eater of Worlds",
    title: t("Eater of Worlds", "Eater of Worlds"),
    summary: t("Boss evil da rota Corruption.", "The Corruption evil boss."),
    when: t("Encaixe logo depois do Eye of Cthulhu.", "Slot it in right after Eye of Cthulhu."),
    arena: t("Abra o bioma e nao lute preso aos tuneis.", "Open up the biome and avoid fighting in tunnels."),
    focus: t("Leve perfuracao ou multi-hit e a luta cai muito de nivel.", "Bring piercing or multi-hit and the fight becomes much easier.")
  },
  {
    id: "brain-of-cthulhu",
    phase: "pre_hardmode",
    setupKey: "pre_evil",
    source: "vanilla",
    optional: false,
    wikiTitle: "Brain of Cthulhu",
    title: t("Brain of Cthulhu", "Brain of Cthulhu"),
    summary: t("Boss evil da rota Crimson.", "The Crimson evil boss."),
    when: t("Entre quando ja conseguir limpar creepers rapidamente.", "Fight it once you can clear the creepers quickly."),
    arena: t("Abra uma arena acima do crimson para escapar da fase final.", "Open an arena above the Crimson for the last phase."),
    focus: t("Feche a fase 1 rapido e nao desperdice recursos nos clones.", "Close phase one quickly and do not waste resources on the clones.")
  },
  {
    id: "queen-bee",
    phase: "pre_hardmode",
    setupKey: "pre_mid",
    source: "vanilla",
    optional: true,
    wikiTitle: "Queen Bee",
    title: t("Queen Bee", "Queen Bee"),
    summary: t("Boss opcional que fortalece muito ranged, magic e summon.", "Optional boss that heavily upgrades ranged, magic, and summon."),
    when: t("Encaixe depois do boss evil e antes de Skeletron.", "Fight it after the evil boss and before Skeletron."),
    arena: t("Monte espaco lateral dentro ou acima da hive.", "Build lateral room inside or above the hive."),
    focus: t("Limpe o minimo de adds e volte a pressionar a rainha.", "Clear only the adds you must and go back to the queen.")
  },
  {
    id: "deerclops",
    phase: "pre_hardmode",
    setupKey: "pre_mid",
    source: "vanilla",
    optional: true,
    wikiTitle: "Deerclops",
    title: t("Deerclops", "Deerclops"),
    summary: t("Boss opcional de espacamento e leitura de padrao.", "Optional boss focused on spacing and pattern reading."),
    when: t("Cabe na mesma janela de Queen Bee e Monthra.", "It fits in the same window as Queen Bee and Monthra."),
    arena: t("Use uma arena larga no snow biome.", "Use a wide arena in the Snow biome."),
    focus: t("Jogue para consistencia, nao para greed.", "Play for consistency rather than greed.")
  },
  {
    id: "monthra",
    phase: "pre_hardmode",
    setupKey: "pre_mid",
    source: "mod",
    optional: false,
    entryId: "monthra",
    title: t("Monthra", "Monthra"),
    summary: t("Primeiro boss oficial do Chaotic Dimensions.", "The first official Chaotic Dimensions boss."),
    when: t("Entre depois de Eater/Brain e antes de Skeletron.", "Fight it after Eater/Brain and before Skeletron."),
    arena: t("Arena horizontal aberta com dois andares e muita rota lateral.", "Use an open horizontal arena with two layers and lots of lateral routing."),
    focus: t("A fase 2 cobra leitura dos homings e disciplina de movimento.", "Phase two checks how well you read the homings and keep movement discipline.")
  },
  {
    id: "skeletron",
    phase: "pre_hardmode",
    setupKey: "pre_dungeon",
    source: "vanilla",
    optional: false,
    wikiTitle: "Skeletron",
    title: t("Skeletron", "Skeletron"),
    summary: t("Gate do Dungeon e da reta final do pre-hardmode.", "The Dungeon gate and late pre-Hardmode checkpoint."),
    when: t("Encare depois de fechar Queen Bee e Monthra se quiser uma luta mais limpa.", "Take it on after Queen Bee and Monthra if you want a cleaner fight."),
    arena: t("Use varios andares e bastante espaco horizontal.", "Use several layers and plenty of horizontal space."),
    focus: t("Derrube as maos com consistencia antes de acelerar a cabeca.", "Remove the hands consistently before speeding up the head phase.")
  },
  {
    id: "wall-of-flesh",
    phase: "pre_hardmode",
    setupKey: "pre_wof",
    source: "vanilla",
    optional: false,
    wikiTitle: "Wall of Flesh",
    title: t("Wall of Flesh", "Wall of Flesh"),
    summary: t("Fechamento do pre-hardmode e porta de entrada do hardmode.", "The pre-Hardmode closer and the gate into Hardmode."),
    when: t("Entre so quando seu melhor kit do pre-hardmode estiver pronto.", "Go in only when your best pre-Hardmode kit is ready."),
    arena: t("Monte um runway longo no Underworld.", "Build a long Underworld runway."),
    focus: t("O perigo real e perder o ritmo do kite ou bater em obstaculos.", "The real danger is losing your kite rhythm or clipping into terrain.")
  },
  {
    id: "queen-slime",
    phase: "early_hardmode",
    setupKey: "hardmode_open",
    source: "vanilla",
    optional: true,
    wikiTitle: "Queen Slime",
    title: t("Queen Slime", "Queen Slime"),
    summary: t("Boss opcional de abertura do hardmode.", "An optional early-Hardmode opener."),
    when: t("Encaixe depois de asas e um kit inicial de hardmode.", "Fight it after getting wings and a real first Hardmode kit."),
    arena: t("Use uma arena alta com varios andares longos.", "Use a tall arena with several long layers."),
    focus: t("A fase voadora exige rota limpa e calma.", "The flying phase demands clean routing and patience.")
  },
  {
    id: "the-destroyer",
    phase: "early_hardmode",
    setupKey: "mechs",
    source: "vanilla",
    optional: false,
    wikiTitle: "The Destroyer",
    title: t("Destroyer", "The Destroyer"),
    summary: t("Mech mais facil de abusar com multi-hit.", "The easiest mech to abuse with multi-hit damage."),
    when: t("Normalmente e o primeiro mech ideal se seu setup perfura bem.", "It is usually the ideal first mech if your setup pierces well."),
    arena: t("Use plataformas altas e evite lutar perto do chao.", "Use high platforms and avoid fighting too close to the ground."),
    focus: t("O encontro gira em torno de bater em varios segmentos e controlar probes.", "The encounter revolves around hitting multiple segments and controlling probes.")
  },
  {
    id: "the-twins",
    phase: "early_hardmode",
    setupKey: "mechs",
    source: "vanilla",
    optional: false,
    wikiTitle: "The Twins",
    title: t("Twins", "The Twins"),
    summary: t("Mech de foco em alvo e leitura aerea.", "The aerial target-focus mech."),
    when: t("Enfrente quando seu dano single target ja estiver confiavel.", "Fight them once your single-target damage is reliable."),
    arena: t("Use varios andares e muito espaco vertical.", "Use several layers and plenty of vertical space."),
    focus: t("Escolha uma kill order e nao persiga os dois olhos ao mesmo tempo.", "Pick a kill order and do not chase both eyes at once.")
  },
  {
    id: "skeletron-prime",
    phase: "early_hardmode",
    setupKey: "mechs",
    source: "vanilla",
    optional: false,
    wikiTitle: "Skeletron Prime",
    title: t("Skeletron Prime", "Skeletron Prime"),
    summary: t("Mech mais tecnico na leitura dos bracos.", "The most technical mech in terms of arm reads."),
    when: t("Encaixe quando ja aguentar um fight longo sem arena curta.", "Fight it when you can survive a long encounter without a cramped arena."),
    arena: t("Abra a arena para cima e para os lados.", "Open the arena upward and sideways."),
    focus: t("Derrube serra e canhao cedo para limpar a luta.", "Delete the saw and cannon early to clean the fight up.")
  },
  {
    id: "plantera",
    phase: "late_hardmode",
    setupKey: "post_mech",
    source: "vanilla",
    optional: false,
    wikiTitle: "Plantera",
    title: t("Plantera", "Plantera"),
    summary: t("Gate do post-mech e da segunda metade do hardmode.", "The post-mech gate into late Hardmode."),
    when: t("Encare depois dos tres mechs e de uma arena dedicada.", "Fight it after all three mechs and after building a dedicated arena."),
    arena: t("Faça uma arena grande na jungle subterranea.", "Build a large arena in the Underground Jungle."),
    focus: t("A primeira fase pede ritmo; a segunda, rota e paciencia.", "Phase one asks for rhythm; phase two asks for route planning and patience.")
  },
  {
    id: "golem",
    phase: "late_hardmode",
    setupKey: "post_plantera",
    source: "vanilla",
    optional: false,
    wikiTitle: "Golem",
    title: t("Golem", "Golem"),
    summary: t("Checkpoint que abre o resto do endgame.", "A checkpoint that opens the rest of the endgame."),
    when: t("Entre assim que Plantera cair se quiser acelerar o endgame.", "Fight it right after Plantera if you want to accelerate the endgame."),
    arena: t("Se preciso, alargue a sala do templo antes da luta.", "If needed, widen the temple room before the fight."),
    focus: t("A luta premia pressao constante e controle do espaco no chao.", "The fight rewards constant pressure and floor-space control.")
  },
  {
    id: "betsy",
    phase: "late_hardmode",
    setupKey: "post_plantera",
    source: "vanilla",
    optional: true,
    wikiTitle: "Betsy",
    title: t("Betsy", "Betsy"),
    summary: t("Boss opcional do Old One's Army com loot excelente para varias classes.", "An optional Old One's Army boss with excellent loot for several classes."),
    when: t("Cabe depois de Golem ou no mesmo bloco dos bosses opcionais de late hardmode.", "It fits after Golem or in the same late-Hardmode optional-boss window."),
    arena: t("Prepare a defesa do evento com lanes claras e espaco para voo lateral.", "Prepare the event defense with clear lanes and room for lateral flight."),
    focus: t("A luta mistura controle de evento com boss fight, entao trate posicionamento como prioridade.", "The fight mixes event control with a boss encounter, so treat positioning as a priority.")
  },
  {
    id: "duke-fishron",
    phase: "late_hardmode",
    setupKey: "post_plantera",
    source: "vanilla",
    optional: true,
    wikiTitle: "Duke Fishron",
    title: t("Duke Fishron", "Duke Fishron"),
    summary: t("Boss opcional de alta recompensa e alta execucao.", "A high-reward optional boss with high execution demands."),
    when: t("Pode entrar depois de Plantera; muitos deixam para depois de Golem.", "You can fight it after Plantera, though many players wait until after Golem."),
    arena: t("Monte uma arena longa sobre o oceano.", "Build a long arena over the ocean."),
    focus: t("A luta melhora quando voce para de reagir tarde e comeca a antecipar os ciclos.", "The fight improves once you stop reacting late and start anticipating the cycle.")
  },
  {
    id: "empress-of-light",
    phase: "late_hardmode",
    setupKey: "post_plantera",
    source: "vanilla",
    optional: true,
    wikiTitle: "Empress of Light",
    title: t("Empress of Light", "Empress of Light"),
    summary: t("Boss opcional de execucao pura e padrao visual.", "An optional boss built around pure execution and visual pattern reads."),
    when: t("Cabe depois de Plantera; de noite ja rende loot muito forte.", "It fits after Plantera, and even the nighttime version gives very strong loot."),
    arena: t("Use uma arena muito aberta e com visibilidade total.", "Use a very open arena with total visibility."),
    focus: t("Nao troque dano. Leia o padrao e castigue so as janelas boas.", "Do not trade damage. Read the pattern and punish only the clean windows.")
  },
  {
    id: "lunatic-cultist",
    phase: "celestial",
    setupKey: "lunar",
    source: "vanilla",
    optional: false,
    wikiTitle: "Lunatic Cultist",
    title: t("Lunatic Cultist", "Lunatic Cultist"),
    summary: t("Boss de transicao para os pilares e Moon Lord.", "The transition boss into the pillars and Moon Lord."),
    when: t("Entre quando o kit pos-Golem ja estiver consolidado.", "Fight it once your post-Golem kit is fully settled."),
    arena: t("Uma arena simples ao redor do Dungeon ja resolve.", "A simple arena around the Dungeon is enough."),
    focus: t("A luta fica muito mais facil quando voce nao erra o clone.", "The fight becomes much easier once you stop hitting the wrong clone.")
  },
  {
    id: "moon-lord",
    phase: "celestial",
    setupKey: "moon_lord",
    source: "vanilla",
    optional: false,
    wikiTitle: "Moon Lord",
    title: t("Moon Lord", "Moon Lord"),
    summary: t("Final boss do vanilla e ultima prova antes do trecho cosmico do mod.", "The vanilla final boss and the last check before the mod's cosmic stretch."),
    when: t("Entre com setup pos-Cultist afiado e pelo menos uma arma lunar boa.", "Enter with a polished post-Cultist setup and at least one strong lunar weapon."),
    arena: t("Use uma arena longa com nurse acessivel e suporte completo.", "Use a long arena with Nurse access and full support stations."),
    focus: t("A luta e sobre uptime inteligente e disciplina, nao so burst.", "The fight is about intelligent uptime and discipline, not just burst.")
  },
  {
    id: "crystaline-devourer",
    phase: "post_moonlord",
    setupKey: "post_moonlord",
    source: "mod",
    optional: false,
    entryId: "crystaline-devourer",
    title: t("Crystaline Devour", "Crystaline Devour"),
    summary: t("Primeiro boss de endgame proprio do Chaotic Dimensions.", "The first true Chaotic Dimensions endgame boss."),
    when: t("Entre logo depois do Moon Lord com setup vanilla completo de endgame.", "Fight it right after Moon Lord with a full vanilla endgame setup."),
    arena: t("Use uma arena muito larga e com leitura visual limpa.", "Use a very wide arena with clean visual readability."),
    focus: t("Sobreviva aos ciclos longos enquanto mantem dano alto e constante.", "Survive the long cycles while maintaining high, steady damage.")
  }
];
