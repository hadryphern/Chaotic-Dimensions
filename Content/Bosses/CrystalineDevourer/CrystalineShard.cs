using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineShard : ModNPC
	{
		public override string Texture => "Terraria/Images/Projectile_936";

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 125;
			NPC.defense = 0;
			NPC.lifeMax = 55;
			NPC.knockBackResist = 0.15f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontCountMe = true;
			NPC.value = 0f;
			NPC.scale = 1.15f;
		}

		public override bool CheckActive() => false;

		public override void AI() {
			if (NPC.ai[1]-- <= 0f) {
				NPC.active = false;
				return;
			}

			int targetIndex = (int)NPC.ai[0];
			if (targetIndex >= 0 && targetIndex < Main.maxPlayers && Main.player[targetIndex].active && !Main.player[targetIndex].dead) {
				Player target = Main.player[targetIndex];
				Vector2 offset = target.Center - NPC.Center;
				Vector2 desiredVelocity = (offset == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(offset)) * 15.5f;
				NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.05f);
			}

			NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(NPC.Center, 0.5f, 0.15f, 0.55f);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses;
			return true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Slow, 45);
			NPC.active = false;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (NPC.life <= 0) {
				for (int i = 0; i < 8; i++) {
					Dust.NewDustPerfect(NPC.Center, DustID.PinkCrystalShard, Main.rand.NextVector2Circular(2.4f, 2.4f));
				}
			}
		}
	}
}
