using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	internal static class CrystalineDevourerSegmentVisuals
	{
		public static bool TryGetLinkedSegment(NPC owner, float aiValue, out NPC linked) {
			linked = null;
			int index = (int)aiValue;
			if (index < 0 || index >= Main.maxNPCs) {
				return false;
			}

			NPC candidate = Main.npc[index];
			if (!candidate.active || candidate.whoAmI == owner.whoAmI) {
				return false;
			}

			if (owner.realLife >= 0 && candidate.whoAmI != owner.realLife && candidate.realLife != owner.realLife) {
				return false;
			}

			linked = candidate;
			return true;
		}

		public static Vector2 GetFallbackDirection(NPC npc) {
			if (npc.velocity.LengthSquared() > 0.01f) {
				return npc.velocity.SafeNormalize(Vector2.UnitY);
			}

			Vector2 rotationDirection = (npc.rotation - MathHelper.PiOver2).ToRotationVector2();
			return rotationDirection.LengthSquared() > 0.01f ? rotationDirection : Vector2.UnitY;
		}

		public static Vector2 GetSegmentAxis(NPC npc, out float curvature) {
			Vector2 fallback = GetFallbackDirection(npc);
			bool hasAhead = TryGetLinkedSegment(npc, npc.ai[1], out NPC ahead);
			bool hasBehind = TryGetLinkedSegment(npc, npc.ai[0], out NPC behind);

			Vector2 axis;
			if (hasAhead && hasBehind) {
				axis = ahead.Center - behind.Center;
			}
			else if (hasAhead) {
				axis = ahead.Center - npc.Center;
			}
			else if (hasBehind) {
				axis = npc.Center - behind.Center;
			}
			else {
				axis = fallback;
			}

			axis = axis.LengthSquared() > 0.01f ? axis.SafeNormalize(fallback) : fallback;
			curvature = 0f;
			if (hasAhead && hasBehind) {
				Vector2 front = (ahead.Center - npc.Center).SafeNormalize(axis);
				Vector2 back = (npc.Center - behind.Center).SafeNormalize(axis);
				curvature = MathHelper.Clamp((1f - Vector2.Dot(front, back)) * 0.5f, 0f, 1f);
			}

			return axis;
		}

		public static Vector2 GetSegmentDrawCenter(NPC npc, float blendAmount) {
			Vector2 center = npc.Center;
			blendAmount = MathHelper.Clamp(blendAmount, 0f, 1f);
			bool hasAhead = TryGetLinkedSegment(npc, npc.ai[1], out NPC ahead);
			bool hasBehind = TryGetLinkedSegment(npc, npc.ai[0], out NPC behind);

			if (hasAhead && hasBehind) {
				Vector2 midpoint = (ahead.Center + behind.Center) * 0.5f;
				center = Vector2.Lerp(center, midpoint, blendAmount);
			}
			else if (hasAhead) {
				center = Vector2.Lerp(center, (center + ahead.Center) * 0.5f, blendAmount * 0.35f);
			}

			return center;
		}

		public static Vector2 GetHeadForward(NPC npc) {
			Vector2 forward = GetFallbackDirection(npc);
			if (TryGetLinkedSegment(npc, npc.ai[0], out NPC nextSegment)) {
				Vector2 awayFromBody = npc.Center - nextSegment.Center;
				if (awayFromBody.LengthSquared() > 0.01f) {
					forward = awayFromBody.SafeNormalize(forward);
				}
			}

			if (npc.velocity.LengthSquared() > 0.01f) {
				Vector2 travelDirection = npc.velocity.SafeNormalize(forward);
				forward = Vector2.Lerp(forward, travelDirection, 0.38f).SafeNormalize(forward);
			}

			return forward;
		}
	}
}
