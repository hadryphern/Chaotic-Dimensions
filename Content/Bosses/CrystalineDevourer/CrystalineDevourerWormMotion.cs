using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	internal static class CrystalineDevourerWormMotion
	{
		public static Vector2 GetForwardDirection(NPC npc, Vector2 fallbackDirection) {
			Vector2 fallback = fallbackDirection.LengthSquared() > 0.01f ? fallbackDirection.SafeNormalize(Vector2.UnitY) : Vector2.UnitY;
			if (npc.velocity.LengthSquared() > 0.01f) {
				return npc.velocity.SafeNormalize(fallback);
			}

			Vector2 rotationDirection = (npc.rotation - MathHelper.PiOver2).ToRotationVector2();
			return rotationDirection.LengthSquared() > 0.01f ? rotationDirection : fallback;
		}

		public static void FollowSegment(NPC segment, NPC ahead, float followDistance, float curvatureBias, float rotationSmoothing) {
			Vector2 previousCenter = segment.Center;
			Vector2 fallbackDirection = GetForwardDirection(segment, Vector2.UnitY);
			Vector2 aheadForward = GetForwardDirection(ahead, fallbackDirection);
			Vector2 liveDirection = ahead.Center - segment.Center;
			if (liveDirection.LengthSquared() <= 0.01f) {
				liveDirection = aheadForward;
			}

			Vector2 followDirection = Vector2.Lerp(liveDirection.SafeNormalize(aheadForward), aheadForward, curvatureBias).SafeNormalize(aheadForward);
			segment.Center = ahead.Center - followDirection * followDistance;

			float desiredRotation = followDirection.ToRotation() + MathHelper.PiOver2;
			segment.rotation = SmoothAngle(segment.rotation, desiredRotation, rotationSmoothing);
			segment.velocity = segment.Center - previousCenter;
		}

		public static float SmoothAngle(float currentAngle, float targetAngle, float amount) {
			return currentAngle + MathHelper.WrapAngle(targetAngle - currentAngle) * MathHelper.Clamp(amount, 0f, 1f);
		}
	}
}
