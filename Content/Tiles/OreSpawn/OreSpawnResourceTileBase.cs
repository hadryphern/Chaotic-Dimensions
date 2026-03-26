using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Tiles.OreSpawn
{
	public abstract class OreSpawnResourceTileBase : ModTile
	{
		protected abstract Color MapColor { get; }
		protected abstract int DroppedItemType { get; }
		protected virtual int DustTypeValue => DustID.Stone;
		protected virtual bool IsOre => false;
		protected virtual bool IsLighted => false;
		protected virtual int MinimumPickaxePower => 0;
		protected virtual float MineResistance => 1f;
		protected virtual ushort FallbackTileType => TileID.Stone;

		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = IsLighted;
			Main.tileMergeDirt[Type] = true;

			if (IsOre) {
				TileID.Sets.Ore[Type] = true;
				TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
				Main.tileSpelunker[Type] = true;
				Main.tileOreFinderPriority[Type] = 450;
				Main.tileShine2[Type] = true;
				Main.tileShine[Type] = 975;
			}

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(MapColor, name);

			DustType = DustTypeValue;
			HitSound = IsOre ? SoundID.Tink : SoundID.Dig;
			RegisterItemDrop(DroppedItemType);
			MinPick = MinimumPickaxePower;
			MineResist = MineResistance;
			VanillaFallbackOnModDeletion = FallbackTileType;
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			if (!IsOre) {
				return false;
			}

			sightColor = MapColor;
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			if (!IsLighted) {
				return;
			}

			r = MapColor.R / 700f;
			g = MapColor.G / 700f;
			b = MapColor.B / 700f;
		}
	}
}
