#nullable enable
using System;
using ChaoticDimensions.Common.Progression;

namespace ChaoticDimensions.Common.OreSpawn
{
	public enum OreSpawnNpcArchetype
	{
		Walker,
		Flyer,
		Hopper,
		Burrower,
		Companion
	}

	public enum OreSpawnBossArchetype
	{
		HoverCharge,
		AerialSweep,
		Bruiser,
		RoyalCaster,
		TentacleHover
	}

	public enum OreSpawnSpawnKind
	{
		None,
		ForestDay,
		ForestNight,
		ForestOrJungleDay,
		JungleDay,
		DesertDay,
		DesertNight,
		Ocean,
		BeachNight,
		SnowSurface,
		Underground,
		Cavern,
		Dungeon,
		Sky,
		Underworld,
		SurfaceNight,
		HallowNight,
		CorruptNight,
		CrimsonNight
	}

	public enum OreSpawnItemKind
	{
		Melee,
		Magic,
		Ranged,
		Tool,
		Material,
		Utility
	}

	public sealed record OreSpawnDropDefinition(
		Func<int> ItemTypeFactory,
		int ChanceDenominator = 1,
		int Minimum = 1,
		int Maximum = 1);

	public sealed record OreSpawnMobDefinition(
		string Key,
		string DisplayName,
		OreSpawnNpcArchetype Archetype,
		OreSpawnSpawnKind SpawnKind,
		ChaoticProgressionGate Gate,
		int Width,
		int Height,
		int RecommendedFrameWidth,
		int RecommendedFrameHeight,
		int RecommendedFrameCount,
		int Damage,
		int Defense,
		int LifeMax,
		float KnockBackResist,
		float SpawnChance,
		int AiStyle,
		int VanillaAiType,
		bool IsMiniBoss = false,
		bool IsFlying = false,
		int OnHitBuffType = 0,
		int OnHitBuffTime = 0,
		int Rarity = 0,
		int HitSoundId = 1,
		int DeathSoundId = 1,
		string PlaceholderTexturePath = "Terraria/Images/NPC_1",
		string? RequiredEventKey = null,
		OreSpawnDropDefinition[]? Drops = null,
		string Notes = "")
	{
		public int RecommendedSheetWidth => RecommendedFrameWidth;
		public int RecommendedSheetHeight => RecommendedFrameHeight * Math.Max(1, RecommendedFrameCount);
	}

	public sealed record OreSpawnBossDefinition(
		string Key,
		string DisplayName,
		OreSpawnBossArchetype Archetype,
		ChaoticProgressionGate Gate,
		int Width,
		int Height,
		int RecommendedFrameWidth,
		int RecommendedFrameHeight,
		int RecommendedFrameCount,
		int Damage,
		int Defense,
		int LifeMax,
		int MusicId,
		float Value,
		int Rarity = 8,
		string PlaceholderTexturePath = "Terraria/Images/NPC_222",
		OreSpawnDropDefinition[]? Drops = null,
		string Notes = "")
	{
		public int RecommendedSheetWidth => RecommendedFrameWidth;
		public int RecommendedSheetHeight => RecommendedFrameHeight * Math.Max(1, RecommendedFrameCount);
	}

	public sealed record OreSpawnItemDefinition(
		string Key,
		string DisplayName,
		OreSpawnItemKind Kind,
		int RecommendedWidth,
		int RecommendedHeight,
		int Damage,
		int UseTime,
		int UseAnimation,
		int Rarity,
		int Value,
		int UseStyle,
		float KnockBack = 0f,
		int Mana = 0,
		int Hammer = 0,
		int Axe = 0,
		int Pick = 0,
		int Shoot = 0,
		float ShootSpeed = 0f,
		int UseAmmo = 0,
		bool AutoReuse = true,
		int MaxStack = 1,
		string PlaceholderTexturePath = "Terraria/Images/Item_0",
		int AppliedBuffType = 0,
		int AppliedBuffTime = 0,
		string Notes = "");

	public sealed record OreSpawnFamilyDefinition(
		string Key,
		string DisplayName,
		string Notes);

	public sealed record OreSpawnEventDefinition(
		string Key,
		string DisplayName,
		ChaoticProgressionGate Gate,
		string Notes,
		bool HasActiveState = false,
		bool IsPlayerTriggered = false);

	public sealed record OreSpawnDimensionDefinition(
		string Key,
		string DisplayName,
		ChaoticProgressionGate Gate,
		string Notes);
}
