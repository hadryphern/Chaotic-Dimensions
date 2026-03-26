using Microsoft.Xna.Framework;
using Terraria.ID;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;

namespace ChaoticDimensions.Content.Tiles.OreSpawn
{
	public sealed class AmethystOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(169, 118, 220);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<Amethyst>();
		protected override int DustTypeValue => DustID.GemAmethyst;
		protected override bool IsOre => true;
		protected override float MineResistance => 1.8f;
	}

	public sealed class RubyOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(205, 64, 84);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<Ruby>();
		protected override int DustTypeValue => DustID.GemRuby;
		protected override bool IsOre => true;
		protected override float MineResistance => 2f;
	}

	public sealed class PinkTourmalineOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(244, 132, 189);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<PinkTourmaline>();
		protected override int DustTypeValue => DustID.PinkTorch;
		protected override bool IsOre => true;
		protected override float MineResistance => 2.1f;
	}

	public sealed class TigersEyeOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(196, 132, 67);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TigersEye>();
		protected override int DustTypeValue => DustID.GoldFlame;
		protected override bool IsOre => true;
		protected override float MineResistance => 2.25f;
	}

	public sealed class KyaniteOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(79, 171, 255);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<Kyanite>();
		protected override int DustTypeValue => DustID.BlueCrystalShard;
		protected override bool IsOre => true;
		protected override float MineResistance => 2.4f;
	}

	public sealed class SaltOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(232, 240, 255);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<Salt>();
		protected override int DustTypeValue => DustID.SilverCoin;
		protected override bool IsOre => true;
		protected override float MineResistance => 1.7f;
	}

	public sealed class TitaniumOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(160, 174, 208);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TitaniumOre>();
		protected override int DustTypeValue => DustID.Titanium;
		protected override bool IsOre => true;
		protected override int MinimumPickaxePower => 120;
		protected override float MineResistance => 3.8f;
	}

	public sealed class UraniumOreTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(136, 226, 90);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<UraniumOre>();
		protected override int DustTypeValue => DustID.GreenTorch;
		protected override bool IsOre => true;
		protected override bool IsLighted => true;
		protected override int MinimumPickaxePower => 135;
		protected override float MineResistance => 4.2f;
	}

	public sealed class AmethystBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(133, 87, 190);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<AmethystBlock>();
		protected override int DustTypeValue => DustID.GemAmethyst;
		protected override float MineResistance => 2.2f;
	}

	public sealed class RubyBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(151, 41, 57);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<RubyBlock>();
		protected override int DustTypeValue => DustID.GemRuby;
		protected override float MineResistance => 2.4f;
	}

	public sealed class PinkTourmalineBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(211, 103, 163);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<PinkTourmalineBlock>();
		protected override int DustTypeValue => DustID.PinkTorch;
		protected override float MineResistance => 2.4f;
	}

	public sealed class TigersEyeBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(154, 101, 53);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TigersEyeBlock>();
		protected override int DustTypeValue => DustID.GoldFlame;
		protected override float MineResistance => 2.5f;
	}

	public sealed class KyaniteBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(62, 124, 222);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<KyaniteBlock>();
		protected override int DustTypeValue => DustID.BlueCrystalShard;
		protected override bool IsLighted => true;
		protected override float MineResistance => 2.8f;
	}

	public sealed class SaltBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(209, 218, 233);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<SaltBlock>();
		protected override int DustTypeValue => DustID.SilverCoin;
		protected override float MineResistance => 1.6f;
	}

	public sealed class TitaniumBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(126, 145, 186);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TitaniumBlock>();
		protected override int DustTypeValue => DustID.Titanium;
		protected override float MineResistance => 3.2f;
	}

	public sealed class UraniumBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(74, 170, 70);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<UraniumBlock>();
		protected override int DustTypeValue => DustID.GreenTorch;
		protected override bool IsLighted => true;
		protected override float MineResistance => 3.5f;
	}

	public sealed class MolenoidDirtTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(95, 65, 43);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<MolenoidDirt>();
		protected override int DustTypeValue => DustID.Dirt;
		protected override ushort FallbackTileType => TileID.Dirt;
	}

	public sealed class RedAntNestTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(169, 59, 52);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<RedAntNest>();
		protected override int DustTypeValue => DustID.t_LivingWood;
		protected override ushort FallbackTileType => TileID.Mud;
	}

	public sealed class TermiteNestTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(201, 155, 86);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TermiteNest>();
		protected override int DustTypeValue => DustID.t_Slime;
		protected override ushort FallbackTileType => TileID.Mud;
	}

	public sealed class CrystalTreeLogTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(136, 215, 248);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<CrystalTreeLog>();
		protected override int DustTypeValue => DustID.BlueCrystalShard;
		protected override bool IsLighted => true;
		protected override ushort FallbackTileType => TileID.WoodBlock;
	}

	public sealed class SkyTreeLogTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(116, 177, 223);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<SkyTreeLog>();
		protected override int DustTypeValue => DustID.t_LivingWood;
		protected override ushort FallbackTileType => TileID.WoodBlock;
	}

	public sealed class DuplicatorLogTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(212, 187, 115);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<DuplicatorLog>();
		protected override int DustTypeValue => DustID.t_LivingWood;
		protected override ushort FallbackTileType => TileID.WoodBlock;
	}

	public sealed class TeleportBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(98, 92, 218);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<TeleportBlock>();
		protected override int DustTypeValue => DustID.PurpleTorch;
		protected override bool IsLighted => true;
		protected override float MineResistance => 3f;
		protected override ushort FallbackTileType => TileID.BlueDungeonBrick;
	}

	public sealed class EnderPearlBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(91, 58, 141);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<EnderPearlBlock>();
		protected override int DustTypeValue => DustID.Shadowflame;
		protected override bool IsLighted => true;
		protected override ushort FallbackTileType => TileID.BlueDungeonBrick;
	}

	public sealed class EyeOfEnderBlockTile : OreSpawnResourceTileBase
	{
		protected override Color MapColor => new(78, 150, 76);
		protected override int DroppedItemType => Terraria.ModLoader.ModContent.ItemType<EyeOfEnderBlock>();
		protected override int DustTypeValue => DustID.GemEmerald;
		protected override bool IsLighted => true;
		protected override ushort FallbackTileType => TileID.GreenDungeonBrick;
	}
}
