using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Common.Systems;

namespace ChaoticDimensions.Content.BossBars
{
	public sealed class CrystalineDevourerBossBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
			if (bossHeadIndex >= 0 && bossHeadIndex < TextureAssets.NpcHeadBoss.Length) {
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}

			return null;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
			if (!CrystalineDevourerArenaSystem.HasAnyLivingPlayers() || !Main.LocalPlayer.active || Main.LocalPlayer.dead) {
				return false;
			}

			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt >= Main.maxNPCs) {
				return false;
			}

			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active || npc.ModNPC is not CrystalineDevourerHead) {
				return false;
			}

			bossHeadIndex = npc.GetBossHeadTextureIndex();
			int totalLife = 0;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC activeNpc = Main.npc[i];
				if (activeNpc.active && activeNpc.type == npc.type) {
					totalLife += System.Math.Max(0, activeNpc.life);
				}
			}

			life = totalLife;
			lifeMax = CrystalineDevourerHead.SharedLifeMax;
			return true;
		}
	}
}
