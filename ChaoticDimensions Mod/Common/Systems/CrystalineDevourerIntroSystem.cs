// Controla a duracao e o desenho da apresentacao do Crystaline Devourer.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using ChaoticDimensions.Content.Bosses.CrystalineDevourer;

namespace ChaoticDimensions.Common.Systems
{
	[Autoload(Side = ModSide.Client)]
	public sealed class CrystalineDevourerIntroSystem : ModSystem
	{
		private const int IntroDuration = 780;
		private const int FadeDuration = 210;
		private static readonly Asset<Texture2D> TitleCardTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/CrystalineDevourerTitleCard");

		private static int introTimer;
		private static int introPlayer = -1;

		public static bool IsActive => introTimer > 0;

		// Inicia a apresentacao e guarda o jogador que invocou.
		public static bool StartIntro(Player player) {
			if (IsActive || NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>())) {
				return false;
			}

			introTimer = IntroDuration;
			introPlayer = player.whoAmI;
			return true;
		}

		public static void CancelIntro() {
			introTimer = 0;
			introPlayer = -1;
		}

		// Liberta referencias para permitir recarregar o mod.
		public override void Unload() {
			CancelIntro();
		}

		// Avanca a apresentacao e cria o boss no final.
		public override void PostUpdateEverything() {
			if (!IsActive) {
				return;
			}

			if (introPlayer < 0 || introPlayer >= Main.maxPlayers || !Main.player[introPlayer].active || Main.player[introPlayer].dead) {
				CancelIntro();
				return;
			}

			introTimer--;
			if (introTimer <= 0) {
				Player player = Main.player[introPlayer];
				CancelIntro();
				if (Main.myPlayer == player.whoAmI && !NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>())) {
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CrystalineDevourerHead>());
				}
			}
		}

		// Insere o desenho personalizado na camada certa da interface.
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"ChaoticDimensions: Crystaline Devourer Intro",
					delegate {
						DrawIntro();
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		// Desenha o cartao com entrada, permanencia e fade.
		private static void DrawIntro() {
			if (!IsActive || Main.gameMenu) {
				return;
			}

			int elapsed = IntroDuration - introTimer;
			float fadeIn = Utils.GetLerpValue(0f, FadeDuration, elapsed, true);
			float fadeOut = Utils.GetLerpValue(0f, FadeDuration, introTimer, true);
			float alpha = fadeIn * fadeOut;

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Rectangle screen = new(-96, -96, Main.screenWidth + 192, Main.screenHeight + 192);
			Main.spriteBatch.Draw(pixel, screen, Color.Black * MathHelper.Clamp(alpha, 0f, 0.985f));

			Texture2D titleCard = TitleCardTexture.Value;
			float maxWidth = Main.screenWidth * 0.88f;
			float maxHeight = Main.screenHeight * 0.5f;
			float scale = Math.Min(maxWidth / titleCard.Width, maxHeight / titleCard.Height);
			Vector2 drawOrigin = titleCard.Size() * 0.5f;
			Vector2 drawPosition = new(Main.screenWidth * 0.5f, Main.screenHeight * 0.47f);
			Main.spriteBatch.Draw(titleCard, drawPosition, null, Color.White * alpha, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
		}
	}
}
