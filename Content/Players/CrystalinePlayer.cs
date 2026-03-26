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
	public enum CrystalineDevourSetType
	{
		None,
		Melee,
		Magic,
		Ranged,
		Summoner
	}

	public sealed class CrystalinePlayer : ModPlayer
	{
		private const int EyeTeleportCooldownFrames = 36;
		private const int AegisDurationFrames = 60 * 10;
		private const int AegisCooldownFrames = 60 * 180;
		private const float EyeTeleportBoostVelocity = 22f;
		private const int TeleportSearchRadiusTiles = 8;

		public bool crystalineEyeEquipped;
		public bool crystalineDevourRangedEconomy;
		public CrystalineDevourSetType crystalineDevourSet;

		private int crystalineEyeCooldown;
		private int crystalineAegisCooldown;

		public override void ResetEffects() {
			crystalineEyeEquipped = false;
			crystalineDevourRangedEconomy = false;
			crystalineDevourSet = CrystalineDevourSetType.None;
		}

		public override void UpdateDead() {
			crystalineEyeEquipped = false;
			crystalineDevourRangedEconomy = false;
			crystalineDevourSet = CrystalineDevourSetType.None;
			crystalineEyeCooldown = 0;
			crystalineAegisCooldown = 0;
		}

		public override void PreUpdateMovement() {
			if (crystalineEyeCooldown > 0) {
				crystalineEyeCooldown--;
			}

			if (crystalineAegisCooldown > 0) {
				crystalineAegisCooldown--;
			}

			if (Player.HasBuff(ModContent.BuffType<CrystalineDevourAegisBuff>())) {
				Player.immune = true;
				Player.immuneNoBlink = true;
				Player.noKnockback = true;
				Player.moveSpeed += 0.32f;
				Player.maxRunSpeed += 1.65f;
				Player.runAcceleration *= 1.35f;
				Player.SetImmuneTimeForAllTypes(2);
			}

			if (CanUseEyeTeleport() && crystalineEyeCooldown == 0 && Player.whoAmI == Main.myPlayer && Main.mouseRight && Main.mouseRightRelease) {
				TryUseCrystalineEyeTeleport();
			}
		}

		public override void PostHurt(Player.HurtInfo info) {
			if (crystalineDevourSet == CrystalineDevourSetType.None || crystalineAegisCooldown > 0 || Player.HasBuff(ModContent.BuffType<CrystalineDevourAegisBuff>())) {
				return;
			}

			crystalineAegisCooldown = AegisCooldownFrames;
			Player.AddBuff(ModContent.BuffType<CrystalineDevourAegisBuff>(), AegisDurationFrames);
		}

		public override bool CanConsumeAmmo(Item weapon, Item ammo) {
			if (crystalineDevourRangedEconomy && Main.rand.NextFloat() < 0.72f) {
				return false;
			}

			return base.CanConsumeAmmo(weapon, ammo);
		}

		private bool CanUseEyeTeleport() {
			return crystalineEyeEquipped && !Player.setSolar && !Player.mount.Active && !Player.noItems && !Player.CCed;
		}

		private void TryUseCrystalineEyeTeleport() {
			Vector2 startingCenter = Player.Center;
			if (!TryFindSafeTeleportCenter(Main.MouseWorld, out Vector2 safeCenter)) {
				return;
			}

			Vector2 destination = safeCenter - (Player.Size * 0.5f);
			Player.Teleport(destination, TeleportationStyleID.RodOfDiscord);
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendData(MessageID.TeleportEntity, number: Player.whoAmI, number2: destination.X, number3: destination.Y, number4: TeleportationStyleID.RodOfDiscord);
			}

			Vector2 boostDirection = (safeCenter - startingCenter).SafeNormalize(Vector2.UnitX * Player.direction);
			Player.velocity = boostDirection * EyeTeleportBoostVelocity;
			Player.fallStart = (int)(Player.position.Y / 16f);
			Player.AddBuff(ModContent.BuffType<CrystalineRushBuff>(), 60 * 4);
			crystalineEyeCooldown = EyeTeleportCooldownFrames;
		}

		private bool TryFindSafeTeleportCenter(Vector2 desiredCenter, out Vector2 safeCenter) {
			Vector2 clampedCenter = desiredCenter;
			clampedCenter.X = MathHelper.Clamp(clampedCenter.X, 24f + Player.width * 0.5f, (Main.maxTilesX * 16f) - 24f - (Player.width * 0.5f));
			clampedCenter.Y = MathHelper.Clamp(clampedCenter.Y, 24f + Player.height * 0.5f, (Main.maxTilesY * 16f) - 24f - (Player.height * 0.5f));

			for (int radius = 0; radius <= TeleportSearchRadiusTiles; radius++) {
				for (int x = -radius; x <= radius; x++) {
					for (int y = -radius; y <= radius; y++) {
						if (radius > 0 && Math.Abs(x) != radius && Math.Abs(y) != radius) {
							continue;
						}

						Vector2 candidateCenter = clampedCenter + new Vector2(x * 16f, y * 16f);
						Vector2 candidateTopLeft = candidateCenter - (Player.Size * 0.5f);
						if (Collision.SolidCollision(candidateTopLeft, Player.width, Player.height)) {
							continue;
						}

						safeCenter = candidateCenter;
						return true;
					}
				}
			}

			safeCenter = Player.Center;
			return false;
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
