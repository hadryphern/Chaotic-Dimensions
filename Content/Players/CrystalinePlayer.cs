using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Items.Weapons.Melee;
using ChaoticDimensions.Content.Projectiles.Melee;

namespace ChaoticDimensions.Content.Players
{
	public sealed class CrystalinePlayer : ModPlayer
	{
		private const int DashCooldownFrames = 120;
		private const int DashDurationFrames = 22;
		private const float DashVelocity = 42f;

		public bool crystalineEyeEquipped;
		public int dashDir = 0;
		public int dashDelay;
		public int dashTimer;

		public override void ResetEffects() {
			crystalineEyeEquipped = false;
			dashDir = 0;
		}

		public override void UpdateDead() {
			crystalineEyeEquipped = false;
			dashDir = 0;
			dashDelay = 0;
			dashTimer = 0;
		}

		public override void PreUpdateMovement() {
			if (CanUseDash() && dashDelay == 0 && Player.whoAmI == Main.myPlayer && Main.mouseRight && Main.mouseRightRelease) {
				float direction = Main.MouseWorld.X >= Player.Center.X ? 1f : -1f;
				dashDir = direction > 0f ? 1 : -1;
				Player.velocity.X = direction * DashVelocity;
				Player.position.X += direction * 160f;
				Player.immune = true;
				Player.immuneNoBlink = true;
				Player.immuneTime = Math.Max(Player.immuneTime, 120);
				Player.noKnockback = true;
				Player.AddBuff(ModContent.BuffType<CrystalineRushBuff>(), 60 * 60 * 5);
				dashDelay = DashCooldownFrames;
				dashTimer = DashDurationFrames;
			}

			if (dashDelay > 0) {
				dashDelay--;
			}

			if (dashTimer > 0) {
				Player.eocDash = dashTimer;
				Player.armorEffectDrawShadowEOCShield = true;
				Player.noKnockback = true;
				Player.immune = true;
				Player.immuneNoBlink = true;
				Player.immuneTime = Math.Max(Player.immuneTime, 120);
				Player.velocity.X = dashDir * DashVelocity;
				dashTimer--;
			}
		}

		private bool CanUseDash() {
			return crystalineEyeEquipped && Player.dashType == DashID.None && !Player.setSolar && !Player.mount.Active;
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (item.type == ModContent.ItemType<CrystalineSword>()) {
				HealFromCrystalineSword();
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.type == ModContent.ProjectileType<CrystalineSwordProjectile>()) {
				HealFromCrystalineSword();
			}
		}

		private void HealFromCrystalineSword() {
			const int healAmount = 10;
			Player.statLife = Utils.Clamp(Player.statLife + healAmount, 0, Player.statLifeMax2);
			Player.HealEffect(healAmount);
		}
	}
}
