using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChaoticDimensions.Common.Systems
{
	[Autoload(Side = ModSide.Client)]
	public sealed class MoonTalkIntroSystem : ModSystem
	{
		private const int StartupDelay = 120;
		private const int BlackScreenDuration = 31 * 60;
		private const int BossFadeInDuration = 8 * 60;
		private const int SceneFadeOutDuration = 5 * 60;
		private const int SubtitleFadeDuration = 45;
		private const int TypewriterTicksPerCharacter = 2;

		private static readonly int[] DialogueDurations = { 300, 311, 405, 423, 300, 300, 300, 300, 300, 300 };
		private static readonly string[] DialogueKeys = {
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line01",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line02",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line03",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line04",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line05",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line06",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line07",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line08",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line09",
			"Mods.ChaoticDimensions.MoonTalk.Dialogue.Line10"
		};

		private static readonly SoundStyle[] VoiceSounds = {
			new("ChaoticDimensions/Sounds/MoonTalk/Voice01_OlaJogador") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice02_BemVindoAoSeuNovoMundo") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice03_CoisasJamaisVistas") { Volume = 0.68f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice04_ConhecimentoDestruido") { Volume = 0.68f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice05_EuSouTudo") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice06_SeuUnicoAliado") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice07_TentareiAjudar") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice08_CustaraCaro") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice09_BoaSorte") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music },
			new("ChaoticDimensions/Sounds/MoonTalk/Voice10_VocePrecisara") { Volume = 0.72f, MaxInstances = 1, Type = SoundType.Music }
		};

		private static readonly SoundStyle BackgroundMusic = new("ChaoticDimensions/Sounds/MoonTalk/MoonTalkSong") {
			Volume = 0.58f,
			MaxInstances = 1,
			Type = SoundType.Music
		};

		private const int SoulOrbFrameCount = 64;
		private const int SoulOrbFrameSize = 256;
		private const int SoulOrbAtlasColumns = 8;
		private static readonly Asset<Texture2D> SoulOrbTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/SoulOrb/SoulOrb_Atlas");

		private static bool active;
		private static int sceneTimer;
		private static int currentDialogueIndex = -1;
		private static bool audioMixCaptured;
		private static float savedSoundVolume;
		private static float savedAmbientVolume;
		private int startupTimer;

		public static bool IsActive => active;

		public static bool IsActiveFor(Player player) {
			return active && player.whoAmI == Main.myPlayer;
		}

		private static int DialogueTotalDuration {
			get {
				int total = 0;
				foreach (int duration in DialogueDurations) {
					total += duration;
				}

				return total;
			}
		}

		private static int DialogueEnd => BlackScreenDuration + DialogueTotalDuration;
		private static int SceneEnd => DialogueEnd + SceneFadeOutDuration;

		public override void OnWorldLoad() {
			CancelIntro();
			startupTimer = StartupDelay;
		}

		public override void OnWorldUnload() {
			CancelIntro();
		}

		public override void Unload() {
			CancelIntro();
		}

		public override void PostUpdateEverything() {
			if (Main.gameMenu) {
				CancelIntro();
				return;
			}

			if (!active) {
				TryStartIntro();
				return;
			}

			SilenceGameAudio();
			UpdateDialogue();
			sceneTimer++;

			if (sceneTimer >= SceneEnd) {
				CompleteIntro();
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			if (!active) {
				return;
			}

			layers.Add(new LegacyGameInterfaceLayer(
				"ChaoticDimensions: MoonTalk Introduction",
				delegate {
					DrawIntro();
					return true;
				},
				InterfaceScaleType.UI));
		}

		private void TryStartIntro() {
			if (!MoonTalkWorldSystem.IntroPending || MoonTalkWorldSystem.IntroCompleted || Main.netMode == NetmodeID.Server) {
				return;
			}

			if (startupTimer > 0) {
				startupTimer--;
				return;
			}

			Player player = Main.LocalPlayer;
			if (!player.active || player.dead) {
				return;
			}

			active = true;
			sceneTimer = 0;
			currentDialogueIndex = -1;
			CaptureAndMuteGameAudio();
			SoundEngine.PlaySound(BackgroundMusic, updateCallback: sound => {
				sound.Volume = 1f - Utils.GetLerpValue(DialogueEnd, SceneEnd, sceneTimer, true);
				return active;
			});
		}

		private static void UpdateDialogue() {
			if (sceneTimer < BlackScreenDuration || sceneTimer >= DialogueEnd) {
				currentDialogueIndex = -1;
				return;
			}

			GetDialoguePosition(sceneTimer - BlackScreenDuration, out int index, out _);
			if (index == currentDialogueIndex) {
				return;
			}

			currentDialogueIndex = index;
			int playingIndex = index;
			SoundEngine.PlaySound(VoiceSounds[index], updateCallback: _ => active && currentDialogueIndex == playingIndex);
		}

		private static void GetDialoguePosition(int dialogueTime, out int index, out int lineTime) {
			int cursor = 0;
			for (int i = 0; i < DialogueDurations.Length; i++) {
				int next = cursor + DialogueDurations[i];
				if (dialogueTime < next) {
					index = i;
					lineTime = dialogueTime - cursor;
					return;
				}

				cursor = next;
			}

			index = DialogueDurations.Length - 1;
			lineTime = DialogueDurations[^1];
		}

		private static void CompleteIntro() {
			active = false;
			currentDialogueIndex = -1;
			RestoreGameAudio();
			MoonTalkWorldSystem.MarkIntroCompleted();

			if (Main.netMode == NetmodeID.MultiplayerClient) {
				ModPacket packet = ModContent.GetInstance<ChaoticDimensions>().GetPacket();
				packet.Write((byte)ChaoticDimensions.MessageType.CompleteMoonTalkIntro);
				packet.Send();
			}
		}

		private static void CancelIntro() {
			active = false;
			sceneTimer = 0;
			currentDialogueIndex = -1;
			RestoreGameAudio();
		}

		private static void CaptureAndMuteGameAudio() {
			if (!audioMixCaptured) {
				savedSoundVolume = Main.soundVolume;
				savedAmbientVolume = Main.ambientVolume;
				audioMixCaptured = true;
			}

			SoundEngine.StopTrackedSounds();
			SilenceGameAudio();
		}

		private static void SilenceGameAudio() {
			Main.soundVolume = 0f;
			Main.ambientVolume = 0f;
			SoundEngine.StopAmbientSounds();

			for (int i = 0; i < Main.musicFade.Length; i++) {
				Main.musicFade[i] = 0f;
			}

			Main.newMusic = 0;
		}

		private static void RestoreGameAudio() {
			if (!audioMixCaptured) {
				return;
			}

			SoundEngine.StopTrackedSounds();
			Main.soundVolume = savedSoundVolume;
			Main.ambientVolume = savedAmbientVolume;
			audioMixCaptured = false;
		}

		private static void DrawIntro() {
			if (!active || Main.gameMenu) {
				return;
			}

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Rectangle fullScreen = new(-128, -128, Main.screenWidth + 256, Main.screenHeight + 256);
			float sceneAlpha = 1f - Utils.GetLerpValue(DialogueEnd, SceneEnd, sceneTimer, true);
			Main.spriteBatch.Draw(pixel, fullScreen, Color.Black * sceneAlpha);

			if (sceneTimer < BlackScreenDuration) {
				return;
			}

			float bossAlpha = Utils.GetLerpValue(BlackScreenDuration, BlackScreenDuration + BossFadeInDuration, sceneTimer, true);
			bossAlpha *= sceneAlpha;
			DrawSoulOrb(bossAlpha);
			DrawSubtitle();
		}

		private static void DrawSoulOrb(float alpha) {
			Texture2D texture = SoulOrbTexture.Value;
			int frameIndex = (sceneTimer / 3) % SoulOrbFrameCount;
			int frameColumn = frameIndex % SoulOrbAtlasColumns;
			int frameRow = frameIndex / SoulOrbAtlasColumns;
			Rectangle frame = new(frameColumn * SoulOrbFrameSize, frameRow * SoulOrbFrameSize, SoulOrbFrameSize, SoulOrbFrameSize);
			Vector2 origin = new(SoulOrbFrameSize * 0.5f);
			float scale = MathHelper.Clamp(Main.screenHeight * 0.26f / SoulOrbFrameSize, 0.85f, 1.8f);
			Vector2 drift = new(
				MathF.Sin(sceneTimer * 0.018f) * 4f,
				MathF.Sin(sceneTimer * 0.022f) * 18f);
			Vector2 position = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.45f) + drift;

			Main.spriteBatch.Draw(texture, position, frame, Color.White * alpha, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		private static void DrawSubtitle() {
			if (sceneTimer < BlackScreenDuration || sceneTimer >= DialogueEnd) {
				return;
			}

			GetDialoguePosition(sceneTimer - BlackScreenDuration, out int index, out int lineTime);
			string completeText = Language.GetTextValue(DialogueKeys[index]);
			int visibleCharacters = Math.Min(completeText.Length, (lineTime / TypewriterTicksPerCharacter) + 1);
			string visibleText = completeText[..visibleCharacters];

			float alpha = 1f - Utils.GetLerpValue(DialogueDurations[index] - SubtitleFadeDuration, DialogueDurations[index], lineTime, true);
			DynamicSpriteFont font = FontAssets.MouseText.Value;
			Vector2 textSize = font.MeasureString(visibleText);
			float scale = Math.Min(1.18f, Main.screenWidth * 0.86f / Math.Max(textSize.X, 1f));
			scale = Math.Max(scale, 0.78f);
			Vector2 position = new(Main.screenWidth * 0.5f, Main.screenHeight * 0.88f);
			Vector2 origin = textSize * 0.5f;

			Utils.DrawBorderString(Main.spriteBatch, visibleText, position + new Vector2(2f, 2f), Color.Black * (0.85f * alpha), scale, 0.5f, 0.5f);
			Utils.DrawBorderString(Main.spriteBatch, visibleText, position, new Color(224, 225, 230) * alpha, scale, 0.5f, 0.5f);
		}
	}
}
