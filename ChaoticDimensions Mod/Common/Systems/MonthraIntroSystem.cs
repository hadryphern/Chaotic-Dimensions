// Controla a apresentacao da Monthra antes de criar o NPC do boss.

using System;
using System.Collections.Generic;
using ChaoticDimensions.Content.Bosses.Monthra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChaoticDimensions.Common.Systems
{
	[Autoload(Side = ModSide.Client)]
	public sealed class MonthraIntroSystem : ModSystem
	{
		private const int IntroDuration = 390;
		private const int FadeDuration = 120;
		private static readonly Asset<Texture2D> TitleCardTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/MonthraTitleCard");

		private static int introTimer;
		private static int introPlayer = -1;

		public static bool IsActive => introTimer > 0;

		public static bool StartIntro(Player player) {
			if (IsActive || NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>())) {
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

		// Liberta referencias quando o mod e descarregado.
		public override void Unload() {
			CancelIntro();
		}

		// Atualiza o sistema depois das entidades do mundo.
		public override void PostUpdateEverything() {
			if (!IsActive) {
				return;
			}

			if (introPlayer < 0 || introPlayer >= Main.maxPlayers || !Main.player[introPlayer].active || Main.player[introPlayer].dead) {
				CancelIntro();
				return;
			}

			introTimer--;
			if (introTimer > 0) {
				return;
			}

			Player player = Main.player[introPlayer];
			CancelIntro();

			if (NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>())) {
				return;
			}

			if (Main.netMode == Terraria.ID.NetmodeID.SinglePlayer) {
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<MonthraBoss>());
				return;
			}

			if (Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient && Main.myPlayer == player.whoAmI) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)ChaoticDimensions.MessageType.SpawnMonthraAfterIntro);
				packet.Write((byte)player.whoAmI);
				packet.Send();
			}
		}

		// Insere o desenho personalizado na camada certa da interface.
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"ChaoticDimensions: Monthra Intro",
					delegate {
						DrawIntro();
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		private static void DrawIntro() {
			if (!IsActive || Main.gameMenu) {
				return;
			}

			int elapsed = IntroDuration - introTimer;
			float fadeIn = Utils.GetLerpValue(0f, FadeDuration, elapsed, true);
			float fadeOut = 1f - Utils.GetLerpValue(230f, IntroDuration, elapsed, true);
			float alpha = MathHelper.Clamp(MathHelper.Min(fadeIn, fadeOut), 0f, 1f);

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Rectangle screen = new(-96, -96, Main.screenWidth + 192, Main.screenHeight + 192);
			Main.spriteBatch.Draw(pixel, screen, Color.Black);
			Main.spriteBatch.Draw(pixel, screen, new Color(72, 38, 92) * (0.18f * alpha));

			Texture2D titleCard = TitleCardTexture.Value;
			float scale = Math.Min(Main.screenWidth / (titleCard.Width * 3f), Main.screenHeight / (titleCard.Height * 4.1f));
			Vector2 drawOrigin = titleCard.Size() * 0.5f;
			Vector2 drawPosition = new(Main.screenWidth * 0.5f, Main.screenHeight * 0.48f);
			Main.spriteBatch.Draw(titleCard, drawPosition, null, new Color(232, 202, 255) * alpha, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
		}
	}
}
