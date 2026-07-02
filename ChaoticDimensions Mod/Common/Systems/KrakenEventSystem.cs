// Coordena a entrada, tempestade, camera e efeitos gerais da luta do Kraken.

using ChaoticDimensions.Content.NPCs.Kraken;
using ChaoticDimensions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Systems
{
	public class KrakenEventSystem : ModSystem
	{
		public const int IntroTitleFadeIn = 120;
		public const int IntroTitleHoldEnd = IntroTitleFadeIn + 110;
		public const int IntroTitleEnd = IntroTitleHoldEnd + 160;
		public const int FirstCameraStart = 9999 * 60;
		public const int FirstCameraEnd = FirstCameraStart;
		public const int SecondCameraStart = 9999 * 60;
		public const int BlackoutStart = 9999 * 60;
		public const int BlackoutEnd = BlackoutStart;
		public const int FirstSilhouetteStart = 9999 * 60;
		public const int FirstSilhouetteEnd = FirstSilhouetteStart;
		public const int SecondSilhouetteStart = 9999 * 60;
		public const int SecondSilhouetteEnd = SecondSilhouetteStart;
		public const int ThirdSilhouetteStart = 9999 * 60;
		public const int ThirdSilhouetteEnd = ThirdSilhouetteStart;
		public const int FourthSilhouetteStart = 9999 * 60;
		public const int FourthSilhouetteEnd = FourthSilhouetteStart;
		public const int BattleStart = IntroTitleEnd;
		public const int Phase2Start = 9999 * 60;
		public const int MusicLoop = IntroTitleEnd + 222 * 60;
		private const int SilhouetteFrameCount = KrakenBoss.LoopAnimationFrames;

		public bool Active;
		public bool SpawnedKraken;
		public int Timer;
		public int OwnerPlayer = -1;
		public int InkTimer;
		private const int MaxInkBlobs = 3;
		private readonly Vector2[] inkBlobCenters = new Vector2[MaxInkBlobs];
		private readonly float[] inkBlobRotations = new float[MaxInkBlobs];
		private readonly float[] inkBlobScales = new float[MaxInkBlobs];
		private readonly int[] inkBlobVariants = new int[MaxInkBlobs];
		private int inkBlobCount;
		private Vector2 cutsceneFocusWorld;
		private int shakeTimer;
		private float shakePower;
		private int focusTimer;
		private Vector2 focusWorld;
		private float focusZoom;
		private bool phase2Announced;
		private int phase2TransitionTimer;
		private float phase2Rage;

		public static KrakenEventSystem Instance => ModContent.GetInstance<KrakenEventSystem>();

		public bool BattleStarted => Active && Timer >= BattleStart;
		public bool Phase2Active => Active && IsKrakenInPhase2();
		public float Phase2Rage => phase2Rage;

		public bool CutsceneShaking => Active && IsCameraCutsceneActive();

		// Inicializa os dados temporarios ao abrir um mundo.
		public override void OnWorldLoad() {
			ResetEvent();
		}

		// Limpa os dados temporarios ao sair do mundo.
		public override void OnWorldUnload() {
			ResetEvent();
		}

		// Serializa o estado global para os clientes.
		public override void NetSend(BinaryWriter writer) {
			writer.Write(Active);
			writer.Write(SpawnedKraken);
			writer.Write(Timer);
			writer.Write((short)OwnerPlayer);
		}

		// Reconstrui o estado global recebido do servidor.
		public override void NetReceive(BinaryReader reader) {
			bool active = reader.ReadBoolean();
			bool spawnedKraken = reader.ReadBoolean();
			int timer = reader.ReadInt32();
			int ownerPlayer = reader.ReadInt16();

			ResetEvent();
			if (!active) {
				return;
			}

			Active = true;
			SpawnedKraken = spawnedKraken;
			Timer = timer;
			OwnerPlayer = ownerPlayer;
		}

		// Inicializa o encontro, escolhe o jogador responsavel e inicia a tempestade.
		public void StartEvent(Player player, bool sync = true) {
			if (Active) {
				return;
			}

			Active = true;
			SpawnedKraken = false;
			Timer = 0;
			OwnerPlayer = player.whoAmI;
			InkTimer = 0;
			cutsceneFocusWorld = player.Center + new Vector2(0f, -430f);
			phase2Announced = false;
			phase2TransitionTimer = 0;
			phase2Rage = 0f;

			Main.raining = true;
			Main.maxRaining = 1f;
			Main.rainTime = 60 * 60;

			if (sync && Main.netMode == NetmodeID.Server) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)global::ChaoticDimensions.ChaoticDimensions.MessageType.StartKrakenEvent);
				packet.Write((byte)player.whoAmI);
				packet.Send();
			}
		}

		// Encerra o encontro e devolve o clima ao estado normal.
		public void StopEvent(bool sync = true) {
			if (sync && Active && Main.netMode == NetmodeID.Server) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)global::ChaoticDimensions.ChaoticDimensions.MessageType.StopKrakenEvent);
				packet.Send();
			}

			bool wasActive = Active;
			ResetEvent();
			if (wasActive) {
				Main.raining = false;
				Main.maxRaining = 0f;
				Main.rainTime = 0;
				Main.windSpeedTarget = 0f;
			}
		}

		public void AddInk(int time) {
			if (InkTimer <= 45 && !Main.dedServ) {
				inkBlobCount = Main.rand.Next(1, MaxInkBlobs + 1);
				float screenScale = System.Math.Max(Main.screenWidth / 1920f, Main.screenHeight / 1080f);
				for (int i = 0; i < inkBlobCount; i++) {
					inkBlobCenters[i] = new Vector2(
						Main.rand.NextFloat(0.12f, 0.88f) * Main.screenWidth,
						Main.rand.NextFloat(0.14f, 0.86f) * Main.screenHeight
					);
					inkBlobRotations[i] = Main.rand.NextFloat(-0.48f, 0.48f);
					inkBlobScales[i] = Main.rand.NextFloat(2.8f, 4.1f) * screenScale;
					inkBlobVariants[i] = (Main.rand.Next(5) + i * 2) % 5;
				}
			}

			InkTimer = Utils.Clamp(InkTimer + time, 0, 430);
		}

		public void AddShake(int time, float power) {
			if (Main.dedServ) {
				return;
			}

			shakeTimer = System.Math.Max(shakeTimer, time);
			shakePower = System.Math.Max(shakePower, power);
		}

		public void FocusCamera(Vector2 worldPosition, int time, float zoom) {
			if (Main.dedServ) {
				return;
			}

			focusWorld = worldPosition;
			focusTimer = System.Math.Max(focusTimer, time);
			focusZoom = System.Math.Max(focusZoom, zoom);
		}

		public bool IsPointUnderFlood(Vector2 worldPosition) {
			return false;
		}

		private void ResetEvent() {
			Active = false;
			SpawnedKraken = false;
			Timer = 0;
			OwnerPlayer = -1;
			InkTimer = 0;
			inkBlobCount = 0;
			System.Array.Clear(inkBlobCenters, 0, inkBlobCenters.Length);
			System.Array.Clear(inkBlobRotations, 0, inkBlobRotations.Length);
			System.Array.Clear(inkBlobScales, 0, inkBlobScales.Length);
			System.Array.Clear(inkBlobVariants, 0, inkBlobVariants.Length);
			cutsceneFocusWorld = Vector2.Zero;
			shakeTimer = 0;
			shakePower = 0f;
			focusTimer = 0;
			focusWorld = Vector2.Zero;
			focusZoom = 1f;
			phase2Announced = false;
			phase2TransitionTimer = 0;
			phase2Rage = 0f;
		}

		// Avanca o evento e sincroniza chuva, fases e spawn do boss.
		public override void PostUpdateEverything() {
			if (!Active) {
				return;
			}

			Timer++;
			if (InkTimer > 0) {
				InkTimer--;
			}

			if (shakeTimer > 0) {
				shakeTimer--;
				if (shakeTimer <= 0) {
					shakePower = 0f;
				}
			}

			if (focusTimer > 0) {
				focusTimer--;
				if (focusTimer <= 0) {
					focusZoom = 1f;
				}
			}

			float rainIntensity = GetRainIntensity();
			Main.raining = rainIntensity > 0f;
			Main.maxRaining = rainIntensity;
			Main.rainTime = 60 * 60;
			if (BattleStarted) {
				Main.cloudAlpha = 1f;
				Main.windSpeedTarget = 0.82f;
			}

			if (Timer >= BattleStart && !SpawnedKraken && Main.netMode != NetmodeID.MultiplayerClient) {
				Player player = GetOwnerPlayer();
				if (player != null) {
					SpawnKraken(player);
				}
			}

			bool krakenExists = TryGetKraken(out NPC kraken);
			bool phase2Now = krakenExists && kraken.lifeMax > 0 && kraken.life <= kraken.lifeMax * 0.5f;
			float rageTarget = phase2Now ? 1f : 0f;
			phase2Rage = MathHelper.Lerp(phase2Rage, rageTarget, phase2Now ? 0.025f : 0.08f);

			if (phase2Now && !phase2Announced) {
				phase2Announced = true;
				phase2TransitionTimer = 180;
				AddShake(120, 18f);
				if (!Main.dedServ) {
					SoundEngine.PlaySound(SoundID.Roar, kraken.Center);
				}
			}

			if (phase2TransitionTimer > 0) {
				phase2TransitionTimer--;
			}

			if (BattleStarted) {
				Main.cloudAlpha = 1f;
				Main.windSpeedTarget = phase2Now ? -0.95f : 0.82f;
			}

			if (BattleStarted && !krakenExists && Main.netMode != NetmodeID.MultiplayerClient) {
				StopEvent(Main.netMode == NetmodeID.Server);
			}
		}

		// Mantem o enquadramento no Kraken sem bloquear o jogador.
		public override void ModifyScreenPosition() {
			if (!Active) {
				return;
			}

			float targetZoom = focusTimer > 0 ? focusZoom : GetCameraZoomTarget();
			Main.GameZoomTarget = MathHelper.Lerp(Main.GameZoomTarget, targetZoom, 0.08f);

			float power = 0f;
			// Gameplay stays framed around the boss; player movement remains completely free.
			if (BattleStarted && TryGetKraken(out NPC cameraKraken)) {
				Vector2 visualCenter = cameraKraken.Center + new Vector2(0f, KrakenBoss.VisualDrawOffsetY - 70f);
				Vector2 targetScreen = visualCenter - new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
				Main.screenPosition = Vector2.Lerp(Main.screenPosition, targetScreen, 0.32f);
			}
			else if (focusTimer > 0) {
				Vector2 targetScreen = focusWorld - new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
				Main.screenPosition = Vector2.Lerp(Main.screenPosition, targetScreen, 0.17f);
			}
			else if (IsCameraCutsceneActive()) {
				float cutsceneProgress = Utils.GetLerpValue(FirstCameraStart, BattleStart, Timer, true);
				Vector2 targetScreen = cutsceneFocusWorld - new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.56f);
				Main.screenPosition = Vector2.Lerp(Main.screenPosition, targetScreen, 0.14f);
				Main.screenPosition.Y -= MathHelper.Lerp(0f, 62f, cutsceneProgress);
				power = Timer >= SecondCameraStart ? MathHelper.Lerp(7f, 13f, cutsceneProgress) : 5.5f;
			}

			if (shakeTimer > 0) {
				float shakeFade = Utils.GetLerpValue(0f, 8f, shakeTimer, true);
				power = System.Math.Max(power, shakePower * shakeFade);
			}

			if (power > 0f) {
				Main.screenPosition += Main.rand.NextVector2Circular(power, power);
			}
		}

		// Desenha titulo, flash, tinta e vinheta sobre a interface.
		public override void PostDrawInterface(SpriteBatch spriteBatch) {
			if (!Active) {
				return;
			}

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			if (Timer < IntroTitleEnd) {
				DrawIntroTitle(spriteBatch, pixel);
				return;
			}

			DrawLightningFlash(spriteBatch, pixel);
			DrawInk(spriteBatch);
			DrawPlayerVignette(spriteBatch);
		}

		public override void PostDrawTiles() {
			if (!Active) {
				return;
			}

			if (Timer < IntroTitleEnd) {
				return;
			}

			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Identity);
			DrawMoonlitWorldGrade(spriteBatch, TextureAssets.MagicPixel.Value);
			spriteBatch.End();
		}

		private Player GetOwnerPlayer() {
			if (OwnerPlayer < 0 || OwnerPlayer >= Main.maxPlayers) {
				return null;
			}

			Player player = Main.player[OwnerPlayer];
			return player.active && !player.dead ? player : null;
		}

		// Cria o NPC no servidor e marca o evento como iniciado.
		private void SpawnKraken(Player player) {
			Vector2 spawnPosition = player.Center + new Vector2(0f, -80f);
			int index = NPC.NewNPC(new EntitySource_Misc("ChaoticDimensions_KrakenIntro"), (int)spawnPosition.X, (int)spawnPosition.Y, ModContent.NPCType<KrakenBoss>(), ai0: player.whoAmI);
			if (index >= 0 && index < Main.maxNPCs) {
				Main.npc[index].Center = spawnPosition;
				Main.npc[index].netUpdate = true;
			}

			Main.raining = true;
			Main.maxRaining = 1f;
			Main.rainTime = 60 * 60;
			Main.cloudAlpha = 1f;
			Main.windSpeedTarget = 0.82f;
			SpawnedKraken = true;
		}

		private bool TryGetKraken(out NPC kraken) {
			int krakenType = ModContent.NPCType<KrakenBoss>();
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == krakenType) {
					kraken = npc;
					return true;
				}
			}

			kraken = null;
			return false;
		}

		private bool IsKrakenInPhase2() {
			return TryGetKraken(out NPC kraken)
				&& kraken.lifeMax > 0
				&& kraken.life <= kraken.lifeMax * 0.5f;
		}

		private void DrawFlood(SpriteBatch spriteBatch, Texture2D pixel) {
			return;
		}

		private bool IsCameraCutsceneActive() {
			return (Timer >= FirstCameraStart && Timer < FirstCameraEnd)
				|| (Timer >= SecondCameraStart && Timer < BattleStart);
		}

		private float GetCameraZoomTarget() {
			if (Timer >= FirstCameraStart && Timer < FirstCameraEnd) {
				return 1.09f;
			}

			if (Timer >= SecondCameraStart && Timer < BattleStart) {
				float progress = Utils.GetLerpValue(SecondCameraStart, BattleStart, Timer, true);
				return MathHelper.Lerp(1.1f, 1.16f, progress);
			}

			return 1f;
		}

		private float GetRainIntensity() {
			return Active ? 1f : 0f;
		}

		private void DrawStormTint(SpriteBatch spriteBatch, Texture2D pixel) {
			if (Timer < FirstCameraStart) {
				return;
			}

			float stormProgress = Utils.GetLerpValue(FirstCameraStart, BattleStart, Timer, true);
			byte alpha = (byte)(Phase2Active ? 92 : MathHelper.Lerp(72, 118, stormProgress));
			spriteBatch.Draw(pixel, new Rectangle(-20, -20, Main.screenWidth + 40, Main.screenHeight + 40), new Color(8, 11, 19, alpha));
		}

		private void DrawDeepDarkFilter(SpriteBatch spriteBatch, Texture2D pixel) {
			if (Timer < IntroTitleEnd) {
				return;
			}

			float cutscene = Timer < BattleStart ? 1f : 0f;
			byte alpha = (byte)(Phase2Active ? 88 : MathHelper.Lerp(72f, 132f, cutscene));
			spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 4, 16, alpha));
			spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 26, 72, (byte)(Timer < BattleStart ? 42 : 24)));
		}

		private void DrawSilhouette(SpriteBatch spriteBatch, float scale, float opacity, float progress) {
			float sway = (float)System.Math.Sin(Timer * 0.004f) * 10f * scale;
			float sink = MathHelper.Lerp(-8f, 18f, progress);
			Vector2 center = new Vector2(Main.screenWidth * 0.5f + sway, Main.screenHeight * 0.43f + sink);
			Texture2D forwardTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Content/NPCs/Kraken/KrakenBoss").Value;
			Texture2D loopBackTexture = ModContent.Request<Texture2D>(KrakenBoss.LoopBackTexturePath).Value;
			int frame = (Timer / 18) % SilhouetteFrameCount;
			KrakenBoss.GetAnimationFrame(forwardTexture, loopBackTexture, frame, out Texture2D silhouette, out Rectangle source);
			Vector2 origin = source.Size() * 0.5f;
			Color dark = new Color(0, 0, 5, (byte)(142 * opacity));
			spriteBatch.Draw(silhouette, center, source, dark, sway * 0.00018f, origin, scale, SpriteEffects.None, 0f);
		}

		private void DrawDeepSeaCurtain(SpriteBatch spriteBatch, Texture2D pixel, float intensity) {
			byte alpha = (byte)MathHelper.Clamp(218f * intensity, 0f, 245f);
			spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 5, 18, alpha));
			spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 20, 58, (byte)(42 * intensity)));
		}

		private void DrawCutsceneFog(SpriteBatch spriteBatch, float intensity) {
			Texture2D fog = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenFogClouds").Value;
			Texture2D clouds = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenParallaxClouds").Value;
			float baseScale = System.Math.Max(Main.screenWidth / (float)fog.Width, Main.screenHeight / (float)fog.Height) * 1.34f;
			Vector2 origin = fog.Size() * 0.5f;
			Vector2 cloudOrigin = clouds.Size() * 0.5f;
			Vector2 screenCenter = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.48f);
			Vector2 cloudCenter = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.42f);
			float cloudScale = System.Math.Max(Main.screenWidth / (float)clouds.Width, Main.screenHeight / (float)clouds.Height) * 1.2f;
			float cloudSway = (float)System.Math.Sin(Timer * 0.006f) * 34f;
			float driftA = (Timer * 0.34f) % (fog.Width * baseScale);
			float driftB = (Timer * -0.22f) % (fog.Width * baseScale);
			Color cloudDense = new Color(130, 150, 172, (byte)(112 * intensity));
			Color cloudSoft = new Color(84, 104, 132, (byte)(74 * intensity));
			Color dense = new Color(126, 136, 148, (byte)(124 * intensity));
			Color soft = new Color(92, 104, 122, (byte)(76 * intensity));

			spriteBatch.Draw(clouds, cloudCenter + new Vector2(cloudSway, -34f), null, cloudDense, 0f, cloudOrigin, cloudScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(clouds, cloudCenter + new Vector2(-cloudSway * 0.55f, 42f), null, cloudSoft, 0f, cloudOrigin, cloudScale * 1.08f, SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(fog, screenCenter + new Vector2(driftA * 0.08f - 32f, -22f), null, dense, 0f, origin, baseScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(fog, screenCenter + new Vector2(driftB * 0.1f + 48f, 36f), null, soft, 0f, origin, baseScale * 1.1f, SpriteEffects.FlipHorizontally, 0f);
		}

		private bool GetSilhouetteWindow(out float scale, out float opacity, out float progress) {
			scale = 1f;
			opacity = 0f;
			progress = 0f;

			if (Timer >= FirstSilhouetteStart && Timer <= FirstSilhouetteEnd) {
				progress = Utils.GetLerpValue(FirstSilhouetteStart, FirstSilhouetteEnd, Timer, true);
				scale = MathHelper.Lerp(0.82f, 0.92f, progress);
				opacity = WindowOpacity(FirstSilhouetteStart, FirstSilhouetteEnd);
				return true;
			}

			if (Timer >= SecondSilhouetteStart && Timer <= SecondSilhouetteEnd) {
				progress = Utils.GetLerpValue(SecondSilhouetteStart, SecondSilhouetteEnd, Timer, true);
				scale = MathHelper.Lerp(1.16f, 1.36f, progress);
				opacity = WindowOpacity(SecondSilhouetteStart, SecondSilhouetteEnd);
				return true;
			}

			if (Timer >= ThirdSilhouetteStart && Timer <= ThirdSilhouetteEnd) {
				progress = Utils.GetLerpValue(ThirdSilhouetteStart, ThirdSilhouetteEnd, Timer, true);
				scale = MathHelper.Lerp(0.62f, 0.8f, progress);
				opacity = WindowOpacity(ThirdSilhouetteStart, ThirdSilhouetteEnd);
				return true;
			}

			if (Timer >= FourthSilhouetteStart && Timer <= FourthSilhouetteEnd) {
				progress = Utils.GetLerpValue(FourthSilhouetteStart, FourthSilhouetteEnd, Timer, true);
				scale = MathHelper.Lerp(0.88f, 1.14f, progress);
				opacity = WindowOpacity(FourthSilhouetteStart, FourthSilhouetteEnd);
				return true;
			}

			return false;
		}

		private float WindowOpacity(int start, int end) {
			float fadeIn = Utils.GetLerpValue(start, start + 34, Timer, true);
			float fadeOut = 1f - Utils.GetLerpValue(end - 72, end, Timer, true);
			return MathHelper.Clamp(MathHelper.Min(fadeIn, fadeOut), 0f, 1f);
		}

		private void DrawRain(SpriteBatch spriteBatch, Texture2D pixel) {
			if (Timer < FirstCameraStart) {
				return;
			}

			float stormProgress = Utils.GetLerpValue(FirstCameraStart, BattleStart, Timer, true);
			int count = Phase2Active ? 160 : Timer < FirstCameraEnd ? 38 : (int)MathHelper.Lerp(70, 145, stormProgress);
			Color color = new Color(130, 175, 230, (byte)(Timer < FirstCameraEnd ? 48 : 112));

			for (int i = 0; i < count; i++) {
				float x = (i * 137 + Timer * 15f) % (Main.screenWidth + 360) - 180;
				float y = (i * 83 + Timer * 34f) % (Main.screenHeight + 240) - 120;
				int width = i % 3 == 0 ? 2 : 1;
				int height = i % 4 == 0 ? 44 : 34;
				spriteBatch.Draw(pixel, new Rectangle((int)x, (int)y, width, height), color);
			}
		}

		private void DrawLightningFlash(SpriteBatch spriteBatch, Texture2D pixel) {
			float flash = 0f;
			flash = MathHelper.Max(flash, FlashAmount(FirstSilhouetteStart));
			flash = MathHelper.Max(flash, FlashAmount(SecondSilhouetteStart));
			flash = MathHelper.Max(flash, FlashAmount(BattleStart));

			if (flash <= 0f) {
				return;
			}

			spriteBatch.Draw(pixel, new Rectangle(-8, -8, Main.screenWidth + 16, Main.screenHeight + 16), new Color(130, 160, 210) * (flash * 0.02f));
		}

		private float FlashAmount(int flashTick) {
			int delta = Timer - flashTick;
			if (delta < 0 || delta > 18) {
				return 0f;
			}

			return 1f - delta / 18f;
		}

		private void DrawCinematicBars(SpriteBatch spriteBatch, Texture2D pixel) {
			if (!Active || Timer < FirstCameraStart) {
				return;
			}

			float amount = 0.52f;
			if (Timer < FirstCameraEnd) {
				float fadeIn = Utils.GetLerpValue(FirstCameraStart, FirstCameraStart + 18, Timer, true);
				float fadeOut = 1f - Utils.GetLerpValue(FirstCameraEnd - 18, FirstCameraEnd, Timer, true);
				amount = 0.78f * MathHelper.Clamp(MathHelper.Min(fadeIn, fadeOut), 0f, 1f);
			}
			else if (Timer >= SecondCameraStart && Timer < BattleStart) {
				amount = MathHelper.Lerp(0.62f, 1f, Utils.GetLerpValue(SecondCameraStart, BattleStart, Timer, true));
			}
			else if (Timer >= BattleStart) {
				amount = 0.55f;
			}

			amount = MathHelper.Clamp(amount, 0f, 1f);
			int barHeight = (int)(Main.screenHeight * 0.105f * amount);
			if (barHeight <= 0) {
				return;
			}

			Color black = new Color(0, 0, 0, 240);
			spriteBatch.Draw(pixel, new Rectangle(-8, -8, Main.screenWidth + 16, barHeight + 8), black);
			spriteBatch.Draw(pixel, new Rectangle(-8, Main.screenHeight - barHeight, Main.screenWidth + 16, barHeight + 8), black);
		}

		private void DrawBlackout(SpriteBatch spriteBatch, Texture2D pixel) {
			if (Timer < BlackoutStart || Timer >= BlackoutEnd) {
				return;
			}

			spriteBatch.Draw(pixel, new Rectangle(-96, -96, Main.screenWidth + 192, Main.screenHeight + 192), Color.Black);
		}

		private void DrawBossTitle(SpriteBatch spriteBatch) {
			if (Timer < BlackoutStart + 18 || Timer >= BlackoutEnd - 8) {
				return;
			}

			Texture2D title = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Effects/KrakenTitle").Value;
			float fadeIn = Utils.GetLerpValue(BlackoutStart + 18, BlackoutStart + 48, Timer, true);
			float fadeOut = 1f - Utils.GetLerpValue(BlackoutEnd - 34, BlackoutEnd - 8, Timer, true);
			float opacity = MathHelper.Clamp(MathHelper.Min(fadeIn, fadeOut), 0f, 1f);
			if (opacity <= 0f) {
				return;
			}

			Vector2 origin = title.Size() * 0.5f;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.49f);
			float scale = MathHelper.Min(Main.screenWidth / (title.Width * 3f), Main.screenHeight / (title.Height * 4.1f));
			Color main = Color.White * opacity;
			spriteBatch.Draw(title, center, null, main, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		private void DrawIntroTitle(SpriteBatch spriteBatch, Texture2D pixel) {
			float fadeIn = Utils.GetLerpValue(0f, IntroTitleFadeIn, Timer, true);
			float fadeOut = 1f - Utils.GetLerpValue(IntroTitleHoldEnd, IntroTitleEnd, Timer, true);
			float opacity = MathHelper.Clamp(MathHelper.Min(fadeIn, fadeOut), 0f, 1f);

			spriteBatch.Draw(pixel, new Rectangle(-96, -96, Main.screenWidth + 192, Main.screenHeight + 192), Color.Black);
			if (opacity <= 0f) {
				return;
			}

			Texture2D title = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Effects/KrakenTitle").Value;
			Vector2 origin = title.Size() * 0.5f;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.48f);
			float scale = MathHelper.Min(Main.screenWidth / (title.Width * 3f), Main.screenHeight / (title.Height * 4.1f));
			Color main = Color.White * opacity;
			spriteBatch.Draw(title, center, null, main, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		private void DrawAlienFog(SpriteBatch spriteBatch) {
			Texture2D fog = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Effects/KrakenAlienFog").Value;
			Vector2 origin = fog.Size() * 0.5f;
			float scale = System.Math.Max(Main.screenWidth / (float)fog.Width, Main.screenHeight / (float)fog.Height) * 1.18f;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
			float drift = Timer * 0.08f;
			Color near = Color.Lerp(new Color(92, 102, 112, 16), new Color(108, 104, 120, 18), phase2Rage);
			Color far = Color.Lerp(new Color(30, 34, 42, 18), new Color(42, 40, 52, 20), phase2Rage);

			spriteBatch.Draw(fog, center + new Vector2((float)System.Math.Sin(drift * 0.31f) * 72f, -22f), null, far, 0f, origin, scale * 1.18f, SpriteEffects.None, 0f);
			spriteBatch.Draw(fog, center + new Vector2((float)System.Math.Sin(drift * 0.45f + 2f) * -95f, 36f), null, near, 0f, origin, scale, SpriteEffects.FlipHorizontally, 0f);
		}

		private void DrawMoonlitWorldGrade(SpriteBatch spriteBatch, Texture2D pixel) {
			Color moonlitBlueGray = Color.Lerp(new Color(16, 20, 24), new Color(18, 20, 27), phase2Rage);
			float opacity = MathHelper.Lerp(0.14f, 0.17f, phase2Rage);
			spriteBatch.Draw(
				pixel,
				new Rectangle(-8, -8, Main.screenWidth + 16, Main.screenHeight + 16),
				moonlitBlueGray * opacity
			);
		}

		private void DrawAlienVisionMask(SpriteBatch spriteBatch) {
			Texture2D mask = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Effects/KrakenTelescopeMask").Value;
			Vector2 origin = mask.Size() * 0.5f;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
			if (TryGetKrakenScreenCenter(out Vector2 bossCenter)) {
				center = Vector2.Lerp(center, bossCenter, 0.28f);
				center.X = MathHelper.Clamp(center.X, Main.screenWidth * 0.34f, Main.screenWidth * 0.66f);
				center.Y = MathHelper.Clamp(center.Y, Main.screenHeight * 0.32f, Main.screenHeight * 0.68f);
			}

			float scale = System.Math.Max(Main.screenWidth, Main.screenHeight) / (float)mask.Width * 1.34f;
			spriteBatch.Draw(mask, center, null, new Color(185, 198, 208, 28), 0f, origin, scale, SpriteEffects.None, 0f);
		}

		private void DrawSpaceHaze(SpriteBatch spriteBatch) {
			Texture2D fog = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Effects/KrakenAlienFog").Value;
			Vector2 origin = fog.Size() * 0.5f;
			float scale = System.Math.Max(Main.screenWidth / (float)fog.Width, Main.screenHeight / (float)fog.Height) * 1.32f;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.48f);
			Color haze = Color.Lerp(new Color(50, 56, 64, 15), new Color(68, 64, 76, 17), phase2Rage);
			spriteBatch.Draw(fog, center + new Vector2((float)System.Math.Sin(Timer * 0.01f) * 58f, 0f), null, haze, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		private bool TryGetKrakenScreenCenter(out Vector2 screenCenter) {
			int krakenType = ModContent.NPCType<KrakenBoss>();
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == krakenType) {
					screenCenter = npc.Center - Main.screenPosition;
					return true;
				}
			}

			screenCenter = Vector2.Zero;
			return false;
		}

		private void DrawPlayerVignette(SpriteBatch spriteBatch) {
			Player player = Main.LocalPlayer;
			if (!player.active || player.dead) {
				return;
			}

			Texture2D vignette = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/KrakenPlayerVignette").Value;
			Vector2 center = player.Center - Main.screenPosition;
			float farthestCorner = 0f;
			Vector2[] corners = {
				Vector2.Zero,
				new Vector2(Main.screenWidth, 0f),
				new Vector2(0f, Main.screenHeight),
				new Vector2(Main.screenWidth, Main.screenHeight)
			};
			foreach (Vector2 corner in corners) {
				farthestCorner = System.Math.Max(farthestCorner, Vector2.Distance(center, corner));
			}

			float scale = farthestCorner / (vignette.Width * 0.5f) * 1.08f;
			float intensity = MathHelper.Lerp(0.68f, 0.9f, phase2Rage);
			spriteBatch.Draw(vignette, center, null, Color.White * intensity, 0f, vignette.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		private void DrawInk(SpriteBatch spriteBatch) {
			if (InkTimer <= 0 || inkBlobCount <= 0) {
				return;
			}

			float opacity = Utils.GetLerpValue(0, 70, InkTimer, true);
			for (int i = 0; i < inkBlobCount; i++) {
				Texture2D ink = ModContent.Request<Texture2D>(
					$"ChaoticDimensions/Content/Effects/KrakenInkSplash{inkBlobVariants[i]}"
				).Value;
				spriteBatch.Draw(
					ink,
					inkBlobCenters[i],
					null,
					Color.White * opacity,
					inkBlobRotations[i],
					ink.Size() * 0.5f,
					inkBlobScales[i],
					i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
					0f
				);
			}
		}
	}
}
