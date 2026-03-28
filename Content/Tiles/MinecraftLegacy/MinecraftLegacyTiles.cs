using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChaoticDimensions.Content.Tiles.MinecraftLegacy
{
	public abstract class MinecraftLegacyTileBase : ModTile
	{
		protected virtual bool IsOre => false;
		protected virtual int RequiredPickaxePower => 0;
		protected virtual float TileMineResist => 1f;
		protected virtual Color MapColor => new(180, 180, 180);

		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileSpelunker[Type] = IsOre;
			Main.tileOreFinderPriority[Type] = (short)(IsOre ? 400 : 0);
			TileID.Sets.Ore[Type] = IsOre;

			MinPick = RequiredPickaxePower;
			MineResist = TileMineResist;
			HitSound = SoundID.Tink;
			DustType = DustID.Stone;

			AddMapEntry(MapColor);
		}
	}

	public sealed class RawAlexandriteBlockTile : MinecraftLegacyTileBase
	{
		protected override Color MapColor => new(120, 235, 196);
	}

	public sealed class GreystedWoodTile : MinecraftLegacyTileBase
	{
		protected override Color MapColor => new(116, 116, 132);

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			Main.tileSolidTop[Type] = false;
			HitSound = SoundID.Dig;
			DustType = DustID.WoodFurniture;
		}
	}

	public sealed class ShadowBlockTile : MinecraftLegacyTileBase
	{
		protected override Color MapColor => new(70, 64, 96);
	}

	public sealed class ShadowOreTile : MinecraftLegacyTileBase
	{
		protected override bool IsOre => true;
		protected override int RequiredPickaxePower => 225;
		protected override float TileMineResist => 4.6f;
		protected override Color MapColor => new(96, 84, 144);
	}

	public sealed class RubyOreTile : MinecraftLegacyTileBase
	{
		protected override bool IsOre => true;
		protected override int RequiredPickaxePower => 55;
		protected override float TileMineResist => 2.1f;
		protected override Color MapColor => new(226, 55, 75);
	}

	public sealed class RosalitaOreTile : MinecraftLegacyTileBase
	{
		protected override bool IsOre => true;
		protected override int RequiredPickaxePower => 180;
		protected override float TileMineResist => 3.4f;
		protected override Color MapColor => new(238, 119, 176);
	}

	public sealed class BlueBerryPlantTile : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = [16, 16];
			TileObjectData.addTile(Type);

			HitSound = SoundID.Grass;
			DustType = DustID.Grass;
			AddMapEntry(new Color(76, 118, 255));
		}
	}
}
