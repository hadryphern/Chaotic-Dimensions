using ChaoticDimensions.Common.Progression;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public abstract class OreSpawnPrototypeNpc : ModNPC
	{
		protected virtual bool IsFlying => false;
		protected virtual bool IsMiniBoss => false;
		protected virtual int FrameCount => 4;
		protected virtual int PrototypeWidth => 48;
		protected virtual int PrototypeHeight => 48;
		protected virtual int PrototypeDamage => 20;
		protected virtual int PrototypeDefense => 8;
		protected virtual int PrototypeLifeMax => 180;
		protected virtual float PrototypeKnockBackResist => 0.35f;
		protected virtual float PrototypeValue => Item.buyPrice(silver: 75);
		protected virtual float PrototypeSpawnChance => 0f;
		protected virtual ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.Anytime;
		protected virtual int VanillaAIType => IsFlying ? NPCID.GiantBat : NPCID.Zombie;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = FrameCount;
		}

		public override void SetDefaults() {
			NPC.width = PrototypeWidth;
			NPC.height = PrototypeHeight;
			NPC.damage = PrototypeDamage;
			NPC.defense = PrototypeDefense;
			NPC.lifeMax = PrototypeLifeMax;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = PrototypeValue;
			NPC.knockBackResist = PrototypeKnockBackResist;
			NPC.aiStyle = IsFlying ? NPCAIStyleID.Bat : NPCAIStyleID.Fighter;
			NPC.noGravity = IsFlying;
			NPC.noTileCollide = IsFlying;
			NPC.npcSlots = IsMiniBoss ? 5f : 1.25f;
			NPC.rarity = IsMiniBoss ? 2 : 0;

			AIType = VanillaAIType;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (!ChaoticProgressionHelper.IsMet(ProgressionGate) || spawnInfo.PlayerSafe || !CanSpawnHere(spawnInfo)) {
				return 0f;
			}

			if (IsMiniBoss && NPC.AnyNPCs(Type)) {
				return 0f;
			}

			return PrototypeSpawnChance;
		}

		public override void FindFrame(int frameHeight) {
			if (FrameCount <= 1) {
				NPC.frame.Y = 0;
				return;
			}

			if (IsFlying) {
				NPC.frameCounter += 0.18 + (NPC.velocity.Length() * 0.03);
			}
			else if (System.Math.Abs(NPC.velocity.X) > 0.2f) {
				NPC.frameCounter += 0.15 + (System.Math.Abs(NPC.velocity.X) * 0.02);
			}
			else {
				NPC.frameCounter = 0;
				NPC.frame.Y = 0;
				return;
			}

			int frame = (int)NPC.frameCounter % FrameCount;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI() {
			if (NPC.HasValidTarget) {
				NPC.direction = NPC.Center.X < Main.player[NPC.target].Center.X ? 1 : -1;
				NPC.spriteDirection = NPC.direction;
			}

			if (!IsFlying && NPC.collideX && NPC.velocity.Y == 0f) {
				NPC.velocity.Y = -5.4f;
			}
		}

		protected abstract bool CanSpawnHere(NPCSpawnInfo spawnInfo);

		protected static bool IsForestSurface(Player player) {
			return player.ZoneOverworldHeight &&
				!player.ZoneBeach &&
				!player.ZoneDesert &&
				!player.ZoneSnow &&
				!player.ZoneJungle &&
				!player.ZoneDungeon &&
				!player.ZoneCorrupt &&
				!player.ZoneCrimson;
		}

		protected static bool IsForestOrJungleSurface(Player player) {
			return player.ZoneOverworldHeight && (player.ZoneJungle || IsForestSurface(player));
		}

		protected static float DaySurfaceChance() => SpawnCondition.OverworldDaySlime.Chance;
	}
}
