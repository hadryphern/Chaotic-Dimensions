using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public abstract class OreSpawnCatalogNpcBase : OreSpawnPrototypeNpc
	{
		protected abstract string DefinitionKey { get; }
		protected OreSpawnMobDefinition Definition => OreSpawnMobCatalog.GetMob(DefinitionKey);

		protected override bool IsFlying => Definition.IsFlying;
		protected override bool IsMiniBoss => Definition.IsMiniBoss;
		protected override int FrameCount => Definition.RecommendedFrameCount;
		protected override int PrototypeWidth => Definition.Width;
		protected override int PrototypeHeight => Definition.Height;
		protected override int PrototypeDamage => Definition.Damage;
		protected override int PrototypeDefense => Definition.Defense;
		protected override int PrototypeLifeMax => Definition.LifeMax;
		protected override float PrototypeKnockBackResist => Definition.KnockBackResist;
		protected override float PrototypeValue => Item.buyPrice(silver: Definition.IsMiniBoss ? 90 : 25);
		protected override float PrototypeSpawnChance => Definition.SpawnChance;
		protected override ChaoticProgressionGate ProgressionGate => Definition.Gate;
		protected override int VanillaAIType => Definition.VanillaAiType;

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.aiStyle = Definition.AiStyle;
			NPC.HitSound = GetSound(Definition.HitSoundId, SoundID.NPCHit1);
			NPC.DeathSound = GetSound(Definition.DeathSoundId, SoundID.NPCDeath1);
			NPC.rarity = Definition.Rarity;

			if (Definition.Archetype == OreSpawnNpcArchetype.Companion) {
				NPC.friendly = true;
				NPC.damage = 0;
				NPC.knockBackResist = 0f;
			}
		}

		public override void AI() {
			base.AI();

			switch (Definition.Archetype) {
				case OreSpawnNpcArchetype.Hopper:
					if (NPC.velocity.Y == 0f && Main.rand.NextBool(70)) {
						NPC.velocity.Y = -6.5f;
					}
					break;
				case OreSpawnNpcArchetype.Burrower:
					if (NPC.collideY && Main.rand.NextBool(55)) {
						NPC.velocity.Y = -8f;
					}
					break;
				case OreSpawnNpcArchetype.Companion:
					Player player = Main.LocalPlayer;
					Vector2 desired = player.Center + new Vector2(player.direction * -48f, -32f);
					Vector2 offset = desired - NPC.Center;
					NPC.velocity = Vector2.Lerp(NPC.velocity, offset.SafeNormalize(Vector2.Zero) * 6f, 0.08f);
					NPC.direction = NPC.velocity.X >= 0f ? 1 : -1;
					NPC.spriteDirection = NPC.direction;
					break;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			base.ModifyNPCLoot(npcLoot);
			if (Definition.Drops is null) {
				return;
			}

			foreach (OreSpawnDropDefinition drop in Definition.Drops) {
				npcLoot.Add(ItemDropRule.Common(drop.ItemTypeFactory(), drop.ChanceDenominator, drop.Minimum, drop.Maximum));
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			if (Definition.OnHitBuffType > 0 && Definition.OnHitBuffTime > 0) {
				target.AddBuff(Definition.OnHitBuffType, Definition.OnHitBuffTime);
			}
		}

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			Player player = spawnInfo.Player;
			if (Definition.RequiredEventKey is not null) {
				return OreSpawnEventStateSystem.IsEventActive(Definition.RequiredEventKey) &&
					OreSpawnEventStateSystem.CanSpawnEventMob(Definition.RequiredEventKey, player);
			}

			return Definition.SpawnKind switch {
				OreSpawnSpawnKind.None => false,
				OreSpawnSpawnKind.ForestDay => IsForestSurface(player) && Main.dayTime,
				OreSpawnSpawnKind.ForestNight => IsForestSurface(player) && !Main.dayTime,
				OreSpawnSpawnKind.ForestOrJungleDay => IsForestOrJungleSurface(player) && Main.dayTime,
				OreSpawnSpawnKind.JungleDay => player.ZoneJungle && player.ZoneOverworldHeight && Main.dayTime,
				OreSpawnSpawnKind.DesertDay => player.ZoneDesert && player.ZoneOverworldHeight && Main.dayTime,
				OreSpawnSpawnKind.DesertNight => player.ZoneDesert && player.ZoneOverworldHeight && !Main.dayTime,
				OreSpawnSpawnKind.Ocean => player.ZoneBeach,
				OreSpawnSpawnKind.BeachNight => player.ZoneBeach && !Main.dayTime,
				OreSpawnSpawnKind.SnowSurface => player.ZoneSnow && player.ZoneOverworldHeight,
				OreSpawnSpawnKind.Underground => player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight,
				OreSpawnSpawnKind.Cavern => player.ZoneRockLayerHeight || player.ZoneUnderworldHeight,
				OreSpawnSpawnKind.Dungeon => player.ZoneDungeon,
				OreSpawnSpawnKind.Sky => player.ZoneSkyHeight,
				OreSpawnSpawnKind.Underworld => player.ZoneUnderworldHeight,
				OreSpawnSpawnKind.SurfaceNight => player.ZoneOverworldHeight && !Main.dayTime,
				OreSpawnSpawnKind.HallowNight => player.ZoneHallow && player.ZoneOverworldHeight && !Main.dayTime,
				OreSpawnSpawnKind.CorruptNight => player.ZoneCorrupt && player.ZoneOverworldHeight && !Main.dayTime,
				OreSpawnSpawnKind.CrimsonNight => player.ZoneCrimson && player.ZoneOverworldHeight && !Main.dayTime,
				_ => false
			};
		}

		private static Terraria.Audio.SoundStyle GetSound(int soundId, Terraria.Audio.SoundStyle fallback) {
			return soundId switch {
				11 => SoundID.NPCHit11,
				13 => SoundID.NPCHit13,
				19 => SoundID.NPCDeath19,
				22 => SoundID.NPCDeath22,
				31 => SoundID.NPCHit31,
				_ => fallback
			};
		}
	}
}
