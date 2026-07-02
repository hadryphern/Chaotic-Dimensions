// Implementa a IA, animacao e regras dos NPCs de Monthra Moth Minion.

using ChaoticDimensions.Content.Bosses.Monthra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Monthra
{
	/// <summary>
	/// Hostile butterfly summoned by Monthra. It pursues the target in curved
	/// approaches and disappears as soon as the boss encounter ends.
	/// </summary>
	public sealed class MonthraMothMinion : ModNPC
	{
		private int OwnerIndex => (int)NPC.ai[0];

		public override string Texture => "ChaoticDimensions/Content/NPCs/Critters/MonthraButterfly";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 6;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			NPC.width = 34;
			NPC.height = 28;
			NPC.damage = 170;
			NPC.defense = 28;
			NPC.lifeMax = 24000;
			NPC.knockBackResist = 0.2f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.value = 0f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			if (!OwnerAlive()) {
				NPC.active = false;
				return;
			}

			NPC.TargetClosest(false);
			Player target = Main.player[NPC.target];
			Vector2 predicted = target.Center + target.velocity * 10f;
			Vector2 desired = (predicted - NPC.Center).SafeNormalize(Vector2.UnitY) * 15f;
			float curve = (float)System.Math.Sin(Main.GameUpdateCount * 0.08f + NPC.whoAmI) * 0.09f;
			desired = desired.RotatedBy(curve);
			NPC.velocity = Vector2.Lerp(NPC.velocity, desired, 0.075f);
			NPC.rotation = NPC.velocity.X * 0.045f;
			NPC.spriteDirection = NPC.velocity.X >= 0f ? 1 : -1;
		}

		// Escolhe o frame de animacao apresentado neste tick.
		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5) {
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (frameHeight * Main.npcFrameCount[Type]);
			}
		}

		private bool OwnerAlive() {
			return OwnerIndex >= 0 &&
				OwnerIndex < Main.maxNPCs &&
				Main.npc[OwnerIndex].active &&
				Main.npc[OwnerIndex].type == ModContent.NPCType<MonthraBoss>();
		}

		public override bool CheckActive() => false;
	}
}
