using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerTail : ModNPC
	{
		public override string Texture => "Terraria/Images/NPC_136";

		public override void SetStaticDefaults() {
			NPCID.Sets.MustAlwaysDraw[Type] = true;
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<CrystalineDevourerHead>();
		}

		public override void SetDefaults() {
			NPC.width = 80;
			NPC.height = 80;
			NPC.damage = 175;
			NPC.defense = 5;
			NPC.lifeMax = CrystalineDevourerHead.SharedLifeMax;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.dontCountMe = true;
		}

		public override bool PreAI() {
			if (NPC.ai[1] < 0 || NPC.ai[1] >= Main.maxNPCs) {
				NPC.active = false;
				return false;
			}

			NPC ahead = Main.npc[(int)NPC.ai[1]];
			NPC head = NPC.realLife >= 0 && NPC.realLife < Main.maxNPCs ? Main.npc[NPC.realLife] : null;
			if (!ahead.active || head is null || !head.active) {
				NPC.active = false;
				return false;
			}

			Vector2 direction = ahead.Center - NPC.Center;
			NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
			float length = direction.Length();
			if (length > 0f) {
				NPC.Center = ahead.Center - direction / length * NPC.width;
			}

			NPC.velocity = Vector2.Zero;
			NPC.damage = head.damage;
			NPC.life = head.life;
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses;
			return true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Slow, 90);
		}
	}
}
