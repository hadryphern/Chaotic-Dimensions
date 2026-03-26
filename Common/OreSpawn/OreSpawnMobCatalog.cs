using System.Collections.Generic;
using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using ChaoticDimensions.Content.Items.OreSpawn;
using ChaoticDimensions.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnMobCatalog
	{
		private static OreSpawnMobDefinition Ambient(string key, string displayName, OreSpawnSpawnKind spawnKind, int width, int height, int damage, int defense, int life, int aiType, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Walker, spawnKind, ChaoticProgressionGate.Anytime, width, height, width, height, 4, damage, defense, life, 0.4f, 0.06f, NPCAIStyleID.Fighter, aiType, false, false, Notes: notes);

		private static OreSpawnMobDefinition Flyer(string key, string displayName, OreSpawnSpawnKind spawnKind, ChaoticProgressionGate gate, int width, int height, int damage, int defense, int life, float spawnChance, bool miniBoss = false, int onHitBuffType = 0, int onHitBuffTime = 0, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Flyer, spawnKind, gate, width, height, width, height, 4, damage, defense, life, miniBoss ? 0.08f : 0.2f, spawnChance, NPCAIStyleID.Bat, NPCID.CaveBat, miniBoss, true, onHitBuffType, onHitBuffTime, miniBoss ? 2 : 0, PlaceholderTexturePath: "Terraria/Images/NPC_49", Notes: notes);

		private static OreSpawnMobDefinition Walker(string key, string displayName, OreSpawnSpawnKind spawnKind, ChaoticProgressionGate gate, int width, int height, int damage, int defense, int life, float spawnChance, bool miniBoss = false, int onHitBuffType = 0, int onHitBuffTime = 0, int aiType = NPCID.Zombie, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Walker, spawnKind, gate, width, height, width, height, 4, damage, defense, life, miniBoss ? 0.08f : 0.32f, spawnChance, NPCAIStyleID.Fighter, aiType, miniBoss, false, onHitBuffType, onHitBuffTime, miniBoss ? 2 : 0, PlaceholderTexturePath: "Terraria/Images/NPC_3", Notes: notes);

		private static OreSpawnMobDefinition Hopper(string key, string displayName, OreSpawnSpawnKind spawnKind, ChaoticProgressionGate gate, int width, int height, int damage, int defense, int life, float spawnChance, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Hopper, spawnKind, gate, width, height, width, height, 4, damage, defense, life, 0.28f, spawnChance, NPCAIStyleID.Slime, NPCID.BlueSlime, false, false, Notes: notes);

		private static OreSpawnMobDefinition Burrower(string key, string displayName, OreSpawnSpawnKind spawnKind, ChaoticProgressionGate gate, int width, int height, int damage, int defense, int life, float spawnChance, bool miniBoss = false, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Burrower, spawnKind, gate, width, height, width, height, 4, damage, defense, life, miniBoss ? 0.05f : 0.15f, spawnChance, NPCAIStyleID.Worm, NPCID.DiggerHead, miniBoss, false, Notes: notes);

		private static OreSpawnMobDefinition Companion(string key, string displayName, int width, int height, int damage, int defense, int life, string notes = "") =>
			new(key, displayName, OreSpawnNpcArchetype.Companion, OreSpawnSpawnKind.None, ChaoticProgressionGate.PostChaoticApexTrio, width, height, width, height, 4, damage, defense, life, 0f, 0f, NPCAIStyleID.Fighter, NPCID.Unicorn, false, false, Rarity: 3, PlaceholderTexturePath: "Terraria/Images/NPC_17", Notes: notes);

		private static OreSpawnBossDefinition Boss(string key, string displayName, OreSpawnBossArchetype archetype, int width, int height, int lifeMax, int damage, int defense, int musicId, float value, string notes = "") =>
			new(key, displayName, archetype, ChaoticProgressionGate.PostChaoticApexTrio, width, height, width, height, 4, damage, defense, lifeMax, musicId, value, PlaceholderTexturePath: "Terraria/Images/NPC_222", Notes: notes);

		public static readonly OreSpawnMobDefinition WaterDragon = Flyer("WaterDragon", "Water Dragon", OreSpawnSpawnKind.Ocean, ChaoticProgressionGate.PostEyeOfCthulhu, 56, 56, 24, 8, 260, 0.08f, notes: "Shoots waterballs and fireballs.");
		public static readonly OreSpawnMobDefinition Mantis = Walker("Mantis", "Mantis", OreSpawnSpawnKind.ForestOrJungleDay, ChaoticProgressionGate.PostEyeOfCthulhu, 48, 48, 28, 10, 220, 0.09f, aiType: NPCID.Hornet, notes: "Fast predatory insect.");
		public static readonly OreSpawnMobDefinition EmperorScorpion = Walker("EmperorScorpion", "Emperor Scorpion", OreSpawnSpawnKind.DesertDay, ChaoticProgressionGate.PostEvilBoss, 64, 64, 42, 16, 1200, 0.03f, true, BuffID.Poisoned, 240, NPCID.DesertGhoulCorruption, "Desert miniboss.");
		public static readonly OreSpawnMobDefinition Hercules = Walker("Hercules", "Hercules", OreSpawnSpawnKind.DesertDay, ChaoticProgressionGate.PostQueenBee, 56, 56, 48, 18, 1500, 0.02f, true, aiType: NPCID.GiantTortoise, notes: "Launches players high.");
		public static readonly OreSpawnMobDefinition Caterkiller = Walker("Caterkiller", "Caterkiller", OreSpawnSpawnKind.ForestNight, ChaoticProgressionGate.PostQueenBee, 64, 48, 40, 14, 1100, 0.025f, true, aiType: NPCID.DevourerHead, notes: "Segmented bug miniboss.");
		public static readonly OreSpawnMobDefinition Cephadrome = Flyer("Cephadrome", "Cephadrome", OreSpawnSpawnKind.SnowSurface, ChaoticProgressionGate.PostAnyMech, 56, 56, 58, 20, 2200, 0.015f, true, notes: "Ice flyer and Kraken counter.");
		public static readonly OreSpawnMobDefinition Alien = Walker("Alien", "Alien", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostAnyMech, 54, 60, 62, 22, 2600, 0.01f, true, aiType: NPCID.MartianWalker, notes: "High-tech invader.");
		public static readonly OreSpawnMobDefinition AttackSquid = Flyer("AttackSquid", "Attack Squid", OreSpawnSpawnKind.Ocean, ChaoticProgressionGate.PostEvilBoss, 46, 46, 28, 8, 300, 0.05f, notes: "Aggressive ocean squid.");
		public static readonly OreSpawnMobDefinition Basilisc = Walker("Basilisc", "Basilisc", OreSpawnSpawnKind.DesertNight, ChaoticProgressionGate.PostQueenBee, 52, 40, 36, 12, 800, 0.03f, true, aiType: NPCID.Antlion, notes: "Heavy desert predator.");
		public static readonly OreSpawnMobDefinition BombOmb = Hopper("BombOmb", "Bomb-omb", OreSpawnSpawnKind.SurfaceNight, ChaoticProgressionGate.PostSkeletron, 32, 32, 34, 6, 180, 0.03f, "Explosive hopper.");
		public static readonly OreSpawnMobDefinition Brutalfly = Flyer("Brutalfly", "Brutalfly", OreSpawnSpawnKind.JungleDay, ChaoticProgressionGate.PostWallOfFlesh, 44, 44, 34, 10, 420, 0.04f, notes: "Aggressive late insect.");
		public static readonly OreSpawnMobDefinition CaveFisher = Walker("CaveFisher", "Cave Fisher", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostSkeletron, 40, 36, 30, 10, 260, 0.025f, aiType: NPCID.CaveBat, notes: "Underground ambusher.");
		public static readonly OreSpawnMobDefinition CreepingHorror = Walker("CreepingHorror", "Creeping Horror", OreSpawnSpawnKind.CorruptNight, ChaoticProgressionGate.PostAnyMech, 48, 40, 46, 14, 900, 0.02f, true, aiType: NPCID.Corruptor, notes: "Corruption horror.");
		public static readonly OreSpawnMobDefinition CrystalUrchin = Walker("CrystalUrchin", "Crystal Urchin", OreSpawnSpawnKind.HallowNight, ChaoticProgressionGate.PostPlantera, 36, 34, 38, 18, 700, 0.02f, aiType: NPCID.Gastropod, notes: "Spiked crystal creature.");
		public static readonly OreSpawnMobDefinition Dragon = Flyer("Dragon", "Dragon", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostWallOfFlesh, 72, 64, 54, 16, 1800, 0.02f, true, notes: "Rideable dragon archetype.");
		public static readonly OreSpawnMobDefinition DungeonBeast = Walker("DungeonBeast", "Dungeon Beast", OreSpawnSpawnKind.Dungeon, ChaoticProgressionGate.PostPlantera, 60, 52, 56, 20, 2200, 0.015f, true, aiType: NPCID.Paladin, notes: "Dungeon heavy.");
		public static readonly OreSpawnMobDefinition EnderKnight = Walker("EnderKnight", "Ender Knight", OreSpawnSpawnKind.CorruptNight, ChaoticProgressionGate.PostPlantera, 44, 52, 48, 18, 1200, 0.02f, aiType: NPCID.ArmoredSkeleton, notes: "End-style knight.");
		public static readonly OreSpawnMobDefinition EnderReaper = Flyer("EnderReaper", "Ender Reaper", OreSpawnSpawnKind.CrimsonNight, ChaoticProgressionGate.PostPlantera, 46, 54, 50, 18, 1400, 0.02f, true, notes: "Late flying reaper.");
		public static readonly OreSpawnMobDefinition Ghost = Flyer("Ghost", "Ghost", OreSpawnSpawnKind.SurfaceNight, ChaoticProgressionGate.PostKingSlime, 34, 42, 16, 4, 90, 0.03f, notes: "Simple night ghost.");
		public static readonly OreSpawnMobDefinition GhostSkeleton = Walker("GhostSkeleton", "Ghost Skeleton", OreSpawnSpawnKind.SurfaceNight, ChaoticProgressionGate.PostKingSlime, 38, 48, 20, 6, 120, 0.03f, aiType: NPCID.Skeleton, notes: "Night skeleton variant.");
		public static readonly OreSpawnMobDefinition Hydrolisc = Walker("Hydrolisc", "Hydrolisc", OreSpawnSpawnKind.JungleDay, ChaoticProgressionGate.PostAnyMech, 54, 42, 56, 18, 1600, 0.02f, true, aiType: NPCID.AngryTrapper, notes: "Strong jungle threat.");
		public static readonly OreSpawnMobDefinition JumpyBug = Hopper("JumpyBug", "Jumpy Bug", OreSpawnSpawnKind.ForestDay, ChaoticProgressionGate.PostEyeOfCthulhu, 30, 28, 18, 5, 80, 0.06f, "Fast early hopper.");
		public static readonly OreSpawnMobDefinition Kyuubi = Flyer("Kyuubi", "Kyuubi", OreSpawnSpawnKind.ForestNight, ChaoticProgressionGate.PostPlantera, 64, 56, 68, 22, 2800, 0.012f, true, notes: "Late mythical fox.");
		public static readonly OreSpawnMobDefinition LeafMonster = Walker("LeafMonster", "Leaf Monster", OreSpawnSpawnKind.ForestDay, ChaoticProgressionGate.PostEvilBoss, 40, 44, 24, 8, 180, 0.04f, aiType: NPCID.MourningWood, notes: "Forest ambusher.");
		public static readonly OreSpawnMobDefinition LurkingTerror = Burrower("LurkingTerror", "Lurking Terror", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostAnyMech, 48, 40, 46, 16, 1200, 0.02f, true, "Burrowing terror.");
		public static readonly OreSpawnMobDefinition Molenoid = Burrower("Molenoid", "Molenoid", OreSpawnSpawnKind.Underground, ChaoticProgressionGate.PostEvilBoss, 36, 32, 22, 6, 180, 0.04f, false, "Digging mole-creature.");
		public static readonly OreSpawnMobDefinition Mosquito = Flyer("Mosquito", "Mosquito", OreSpawnSpawnKind.JungleDay, ChaoticProgressionGate.PostEyeOfCthulhu, 28, 28, 14, 3, 60, 0.06f, notes: "Tiny flying pest.");
		public static readonly OreSpawnMobDefinition RedAntRobot = Walker("RedAntRobot", "Red Ant Robot", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 50, 56, 52, 20, 1800, 0f, true, aiType: NPCID.MartianOfficer, notes: "Reserved for event-based spawns.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition RoboGunner = Walker("RoboGunner", "Robo Gunner", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 44, 52, 46, 16, 900, 0f, aiType: NPCID.MartianOfficer, notes: "Reserved for village siege style events.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition RoboJeffery = Walker("RoboJeffery", "Robo Jeffery", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 44, 52, 42, 14, 860, 0f, aiType: NPCID.MartianEngineer, notes: "Reserved for village siege style events.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition RoboPounder = Walker("RoboPounder", "Robo Pounder", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 54, 58, 54, 20, 1600, 0f, true, aiType: NPCID.MartianWalker, notes: "Heavy robot event unit.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition RoboSniper = Walker("RoboSniper", "Robo Sniper", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 42, 50, 48, 15, 820, 0f, aiType: NPCID.MartianOfficer, notes: "Long-range robot event unit.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition RoboWarrior = Walker("RoboWarrior", "Robo Warrior", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 56, 60, 60, 22, 2200, 0f, true, aiType: NPCID.MartianWalker, notes: "Village dimension flagship robot.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition Rotator = Flyer("Rotator", "Rotator", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostAnyMech, 42, 42, 40, 12, 820, 0.025f, notes: "Spinning underground flyer.");
		public static readonly OreSpawnMobDefinition Scorpion = Walker("Scorpion", "Scorpion", OreSpawnSpawnKind.DesertDay, ChaoticProgressionGate.PostEyeOfCthulhu, 38, 30, 18, 5, 100, 0.05f, aiType: NPCID.Scorpion, notes: "Base desert scorpion.");
		public static readonly OreSpawnMobDefinition SeaMonster = Flyer("SeaMonster", "Sea Monster", OreSpawnSpawnKind.Ocean, ChaoticProgressionGate.PostWallOfFlesh, 60, 52, 52, 18, 1800, 0.02f, true, notes: "Dangerous sea predator.");
		public static readonly OreSpawnMobDefinition SeaViper = Flyer("SeaViper", "Sea Viper", OreSpawnSpawnKind.Ocean, ChaoticProgressionGate.PostWallOfFlesh, 54, 48, 44, 14, 1200, 0.03f, true, notes: "Ocean serpent.");
		public static readonly OreSpawnMobDefinition SpiderDriver = Walker("SpiderDriver", "Spider Driver", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 44, 48, 44, 16, 900, 0f, aiType: NPCID.MartianOfficer, notes: "Event-only robot rider.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition SpiderRobot = Walker("SpiderRobot", "Spider Robot", OreSpawnSpawnKind.None, ChaoticProgressionGate.PostAnyMech, 50, 44, 48, 18, 1200, 0f, true, aiType: NPCID.MartianWalker, notes: "Event-only spider mech.") with { RequiredEventKey = OreSpawnWorldCatalog.VillageNightSiegeKey };
		public static readonly OreSpawnMobDefinition SpitBug = Hopper("SpitBug", "Spit Bug", OreSpawnSpawnKind.ForestDay, ChaoticProgressionGate.PostEyeOfCthulhu, 30, 28, 16, 4, 70, 0.05f, "Bug that harasses early players.");
		public static readonly OreSpawnMobDefinition StinkBug = Hopper("StinkBug", "Stink Bug", OreSpawnSpawnKind.ForestDay, ChaoticProgressionGate.PostEyeOfCthulhu, 30, 28, 18, 4, 80, 0.05f, "Smelly early pest.");
		public static readonly OreSpawnMobDefinition TerribleTerror = Flyer("TerribleTerror", "Terrible Terror", OreSpawnSpawnKind.Underworld, ChaoticProgressionGate.PostAnyMech, 48, 44, 54, 18, 1500, 0.02f, true, notes: "Late fiery terror.");
		public static readonly OreSpawnMobDefinition Triffid = Walker("Triffid", "Triffid", OreSpawnSpawnKind.JungleDay, ChaoticProgressionGate.PostSkeletron, 46, 54, 38, 14, 700, 0.03f, aiType: NPCID.ManEater, notes: "Walking killer plant.");
		public static readonly OreSpawnMobDefinition Vortex = Flyer("Vortex", "Vortex", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostCultist, 52, 52, 74, 24, 3200, 0.015f, true, notes: "Late celestial flyer.");

		public static readonly OreSpawnMobDefinition Alosaurus = Walker("Alosaurus", "Alosaurus", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostWallOfFlesh, 60, 42, 42, 12, 900, 0.02f, true, aiType: NPCID.Wolf, notes: "Mining-dimension dino stand-in.");
		public static readonly OreSpawnMobDefinition BabyDragon = Flyer("BabyDragon", "Baby Dragon", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostWallOfFlesh, 40, 40, 28, 8, 280, 0.03f, notes: "Small dragon.");
		public static readonly OreSpawnMobDefinition Baryonyx = Walker("Baryonyx", "Baryonyx", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostWallOfFlesh, 56, 40, 40, 12, 820, 0.02f, aiType: NPCID.Wolf, notes: "Predatory dino.");
		public static readonly OreSpawnMobDefinition Camarasaurus = Walker("Camarasaurus", "Camarasaurus", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostWallOfFlesh, 70, 54, 34, 14, 1600, 0.01f, true, aiType: NPCID.GiantTortoise, notes: "Large dino.");
		public static readonly OreSpawnMobDefinition Cryolophosaurus = Walker("Cryolophosaurus", "Cryolophosaurus", OreSpawnSpawnKind.SnowSurface, ChaoticProgressionGate.PostWallOfFlesh, 60, 44, 44, 14, 900, 0.02f, aiType: NPCID.Wolf, notes: "Cold-climate dino.");
		public static readonly OreSpawnMobDefinition Leonopteryx = Flyer("Leonopteryx", "Leonopteryx", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostPlantera, 64, 56, 64, 20, 2200, 0.015f, true, notes: "Huge predator flyer.");
		public static readonly OreSpawnMobDefinition Nastysaurus = Walker("Nastysaurus", "Nastysaurus", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostWallOfFlesh, 64, 48, 48, 16, 1100, 0.018f, true, aiType: NPCID.Wolf, notes: "Aggressive dino.");
		public static readonly OreSpawnMobDefinition Pointysaurus = Walker("Pointysaurus", "Pointysaurus", OreSpawnSpawnKind.SnowSurface, ChaoticProgressionGate.PostWallOfFlesh, 60, 44, 46, 14, 980, 0.02f, true, aiType: NPCID.Wolf, notes: "Spiked dino.");
		public static readonly OreSpawnMobDefinition TRex = Walker("TRex", "T-Rex", OreSpawnSpawnKind.Cavern, ChaoticProgressionGate.PostAllMechs, 74, 58, 72, 22, 2600, 0.01f, true, aiType: NPCID.GiantTortoise, notes: "Top dinosaur predator.");

		public static readonly OreSpawnMobDefinition Beaver = Ambient("Beaver", "Beaver", OreSpawnSpawnKind.ForestDay, 28, 24, 6, 2, 40, NPCID.Bunny, "Small ambient critter.");
		public static readonly OreSpawnMobDefinition Bee = Ambient("Bee", "Bee", OreSpawnSpawnKind.ForestOrJungleDay, 24, 22, 8, 2, 30, NPCID.BeeSmall, "Ambient bee.");
		public static readonly OreSpawnMobDefinition Cassowary = Ambient("Cassowary", "Cassowary", OreSpawnSpawnKind.ForestDay, 30, 34, 10, 3, 60, NPCID.Bunny, "Ground bird.");
		public static readonly OreSpawnMobDefinition Chipmunk = Companion("Chipmunk", "Chipmunk", 22, 20, 18, 4, 120, "Battle mobs team-play creature.");
		public static readonly OreSpawnMobDefinition CliffRacer = Flyer("CliffRacer", "Cliff Racer", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.Anytime, 30, 26, 12, 4, 70, 0.04f, notes: "Ambient sky bird.");
		public static readonly OreSpawnMobDefinition CloudShark = Flyer("CloudShark", "Cloud Shark", OreSpawnSpawnKind.Sky, ChaoticProgressionGate.PostWallOfFlesh, 44, 32, 32, 10, 420, 0.03f, notes: "Sky predator.");
		public static readonly OreSpawnMobDefinition Crab = Ambient("Crab", "Crab", OreSpawnSpawnKind.Ocean, 24, 18, 8, 3, 40, NPCID.Crab, "Beach critter.");
		public static readonly OreSpawnMobDefinition Cricket = Ambient("Cricket", "Cricket", OreSpawnSpawnKind.ForestNight, 22, 18, 6, 1, 20, NPCID.Grasshopper, "Night critter.");
		public static readonly OreSpawnMobDefinition EasterBunny = Ambient("EasterBunny", "Easter Bunny", OreSpawnSpawnKind.ForestDay, 24, 24, 6, 2, 40, NPCID.Bunny, "Special bunny.");
		public static readonly OreSpawnMobDefinition Fairy = Flyer("Fairy", "Fairy", OreSpawnSpawnKind.HallowNight, ChaoticProgressionGate.PostEvilBoss, 28, 30, 12, 4, 80, 0.03f, notes: "Small magic flyer.");
		public static readonly OreSpawnMobDefinition Firefly = Flyer("Firefly", "Firefly", OreSpawnSpawnKind.ForestNight, ChaoticProgressionGate.Anytime, 20, 20, 6, 1, 18, 0.05f, notes: "Ambient night light.");
		public static readonly OreSpawnMobDefinition Flounder = Ambient("Flounder", "Flounder", OreSpawnSpawnKind.Ocean, 28, 18, 6, 2, 28, NPCID.Goldfish, "Fish variant.");
		public static readonly OreSpawnMobDefinition Frog = Ambient("Frog", "Frog", OreSpawnSpawnKind.JungleDay, 22, 18, 6, 2, 24, NPCID.Frog, "Ambient frog.");
		public static readonly OreSpawnMobDefinition Gazelle = Ambient("Gazelle", "Gazelle", OreSpawnSpawnKind.ForestDay, 30, 24, 8, 2, 50, NPCID.Bunny, "Fast harmless animal.");
		public static readonly OreSpawnMobDefinition GoldFish = Ambient("GoldFish", "Gold Fish", OreSpawnSpawnKind.Ocean, 20, 16, 4, 1, 18, NPCID.Goldfish, "Ambient fish.");
		public static readonly OreSpawnMobDefinition Hammerhead = Ambient("Hammerhead", "Hammerhead", OreSpawnSpawnKind.Ocean, 40, 24, 12, 4, 80, NPCID.Shark, "Ambient predator fish.");
		public static readonly OreSpawnMobDefinition Irukandji = Flyer("Irukandji", "Irukandji", OreSpawnSpawnKind.Ocean, ChaoticProgressionGate.PostEvilBoss, 22, 22, 14, 3, 70, 0.05f, notes: "Tiny dangerous jellyfish.");
		public static readonly OreSpawnMobDefinition Lizard = Companion("Lizard", "Lizard", 26, 18, 20, 4, 150, "Battle mobs team-play creature.");
		public static readonly OreSpawnMobDefinition Ostrich = Companion("Ostrich", "Ostrich", 34, 42, 24, 5, 180, "Battle mobs team-play creature.");
		public static readonly OreSpawnMobDefinition Peacock = Ambient("Peacock", "Peacock", OreSpawnSpawnKind.ForestDay, 28, 34, 8, 2, 42, NPCID.BirdBlue, "Colorful ambient bird.");
		public static readonly OreSpawnMobDefinition Rat = Ambient("Rat", "Rat", OreSpawnSpawnKind.ForestNight, 20, 14, 8, 1, 24, NPCID.Mouse, "Small rat.");
		public static readonly OreSpawnMobDefinition RubberDucky = Ambient("RubberDucky", "Rubber Ducky", OreSpawnSpawnKind.Ocean, 20, 18, 0, 0, 20, NPCID.Duck2, "Novelty duck.");
		public static readonly OreSpawnMobDefinition Skate = Ambient("Skate", "Skate", OreSpawnSpawnKind.Ocean, 28, 16, 8, 2, 28, NPCID.Goldfish, "Sea skate.");
		public static readonly OreSpawnMobDefinition Stinky = Ambient("Stinky", "Stinky", OreSpawnSpawnKind.ForestDay, 20, 20, 6, 2, 26, NPCID.Bunny, "Passive skunk-like mob.");
		public static readonly OreSpawnMobDefinition Termite = Ambient("Termite", "Termite", OreSpawnSpawnKind.ForestDay, 16, 16, 6, 0, 12, NPCID.Buggy, "Dimension transport utility pest.");
		public static readonly OreSpawnMobDefinition VelocityRaptor = Companion("VelocityRaptor", "Velocity Raptor", 34, 30, 28, 6, 240, "Battle mobs team-play creature.");
		public static readonly OreSpawnMobDefinition Whale = Ambient("Whale", "Whale", OreSpawnSpawnKind.Ocean, 72, 34, 14, 4, 220, NPCID.Dolphin, "Massive ambient sea life.");

		public static readonly OreSpawnMobDefinition Girlfriend = Companion("Girlfriend", "Girlfriend", 28, 48, 18, 8, 320, "Social companion NPC.");
		public static readonly OreSpawnMobDefinition Boyfriend = Companion("Boyfriend", "Boyfriend", 28, 48, 18, 8, 320, "Social companion NPC.");
		public static readonly OreSpawnMobDefinition ThePrince = Companion("ThePrince", "The Prince", 64, 56, 88, 24, 3000, "Royal pet line.");
		public static readonly OreSpawnMobDefinition YoungPrince = Companion("YoungPrince", "Young Prince", 72, 64, 104, 28, 4500, "Royal growth stage.");
		public static readonly OreSpawnMobDefinition YoungAdultPrince = Companion("YoungAdultPrince", "Young Adult Prince", 96, 80, 128, 34, 8500, "Late royal growth stage.");
		public static readonly OreSpawnMobDefinition ThePrincess = Companion("ThePrincess", "The Princess", 72, 64, 112, 28, 5000, "Royal companion stage.");

		public static IReadOnlyList<OreSpawnMobDefinition> AllMobs { get; } = new[] {
			WaterDragon, Mantis, EmperorScorpion, Hercules, Caterkiller, Cephadrome,
			Alien, AttackSquid, Basilisc, BombOmb, Brutalfly, CaveFisher, CreepingHorror, CrystalUrchin,
			Dragon, DungeonBeast, EnderKnight, EnderReaper, Ghost, GhostSkeleton, Hydrolisc,
			JumpyBug, Kyuubi, LeafMonster, LurkingTerror, Molenoid, Mosquito, RedAntRobot,
			RoboGunner, RoboJeffery, RoboPounder, RoboSniper, RoboWarrior, Rotator, Scorpion,
			SeaMonster, SeaViper, SpiderDriver, SpiderRobot, SpitBug, StinkBug, TerribleTerror,
			Triffid, Vortex,
			Alosaurus, BabyDragon, Baryonyx, Camarasaurus, Cryolophosaurus, Leonopteryx, Nastysaurus, Pointysaurus, TRex,
			Beaver, Bee, Cassowary, Chipmunk, CliffRacer, CloudShark, Crab, Cricket, EasterBunny, Fairy, Firefly,
			Flounder, Frog, Gazelle, GoldFish, Hammerhead, Irukandji, Lizard, Ostrich, Peacock, Rat, RubberDucky,
			Skate, Stinky, Termite, VelocityRaptor, Whale,
			Girlfriend, Boyfriend, ThePrince, YoungPrince, YoungAdultPrince, ThePrincess
		};

		public static IReadOnlyList<OreSpawnFamilyDefinition> GroupPages { get; } = new[] {
			new OreSpawnFamilyDefinition("Ants", "Ants", "Brown, Rainbow, Red, and Unstable ant family page."),
			new OreSpawnFamilyDefinition("Birds", "Birds", "Ambient bird family page."),
			new OreSpawnFamilyDefinition("Butterflies", "Butterflies", "Butterfly family page."),
			new OreSpawnFamilyDefinition("Cows", "Cows", "Apple cow family page."),
			new OreSpawnFamilyDefinition("Criminals", "Criminals", "Hostile human family page."),
			new OreSpawnFamilyDefinition("Dragonflies", "Dragonflies", "Dragonfly family page."),
			new OreSpawnFamilyDefinition("Moths", "Moths", "Moth family page."),
			new OreSpawnFamilyDefinition("Nightmares", "Nightmares", "Escalating nightmare family page."),
			new OreSpawnFamilyDefinition("Worms", "Worms", "Three-stage worm family page.")
		};

		public static IReadOnlyList<OreSpawnBossDefinition> AllBosses { get; } = new[] {
			Boss("Kraken", "Kraken", OreSpawnBossArchetype.TentacleHover, 120, 120, 520000, 180, 40, MusicID.Boss4, Item.buyPrice(platinum: 2), "Post-chaotic ocean apex."),
			Boss("Mobzilla", "Mobzilla", OreSpawnBossArchetype.Bruiser, 154, 154, 850000, 220, 52, MusicID.Boss2, Item.buyPrice(platinum: 3), "Massive kaiju boss."),
			Boss("Mothra", "Mothra", OreSpawnBossArchetype.AerialSweep, 144, 104, 610000, 190, 38, MusicID.Boss5, Item.buyPrice(platinum: 2, gold: 50), "Sky terror boss."),
			Boss("TheKing", "The King", OreSpawnBossArchetype.RoyalCaster, 160, 128, 1200000, 250, 60, MusicID.Boss5, Item.buyPrice(platinum: 4), "Royal apex beta."),
			Boss("TheQueen", "The Queen", OreSpawnBossArchetype.RoyalCaster, 170, 140, 1350000, 260, 64, MusicID.Boss5, Item.buyPrice(platinum: 4), "Royal apex beta."),
			Boss("Wtf", "WTF?", OreSpawnBossArchetype.HoverCharge, 132, 132, 480000, 170, 36, MusicID.Boss4, Item.buyPrice(platinum: 2), "Special weird boss beta.")
		};

		private static readonly Dictionary<string, OreSpawnMobDefinition> MobMap = new();
		private static readonly Dictionary<string, OreSpawnBossDefinition> BossMap = new();

		static OreSpawnMobCatalog() {
			foreach (OreSpawnMobDefinition definition in AllMobs) {
				MobMap[definition.Key] = definition;
			}

			foreach (OreSpawnBossDefinition definition in AllBosses) {
				BossMap[definition.Key] = definition;
			}
		}

		public static OreSpawnMobDefinition GetMob(string key) => MobMap[key];

		public static OreSpawnBossDefinition GetBoss(string key) => BossMap[key];
	}
}
