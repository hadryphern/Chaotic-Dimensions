using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Players
{
	public sealed class MoonTalkIntroPlayer : ModPlayer
	{
		public override void SetControls() {
			if (!MoonTalkIntroSystem.IsActiveFor(Player)) {
				return;
			}

			Player.controlLeft = false;
			Player.controlRight = false;
			Player.controlUp = false;
			Player.controlDown = false;
			Player.controlJump = false;
			Player.controlUseItem = false;
			Player.controlUseTile = false;
			Player.controlHook = false;
			Player.controlMount = false;
			Player.controlQuickHeal = false;
			Player.controlQuickMana = false;
			Player.controlThrow = false;
		}

		public override void PreUpdateMovement() {
			if (!MoonTalkIntroSystem.IsActiveFor(Player)) {
				return;
			}

			Player.velocity = Vector2.Zero;
			Player.noItems = true;
			Player.channel = false;
			Player.itemAnimation = 0;
			Player.itemTime = 0;
			Player.immune = true;
			Player.immuneTime = 2;
		}

		public override bool CanUseItem(Item item) {
			return !MoonTalkIntroSystem.IsActiveFor(Player);
		}

		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
			return MoonTalkIntroSystem.IsActiveFor(Player);
		}
	}
}
