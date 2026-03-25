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
			Vector2 aheadForward = GetForwardDirection(ahead, Vector2.UnitY);
			Vector2 followDirection = ahead.Center - segment.Center;
			if (followDirection.LengthSquared() <= 0.01f) {
				followDirection = aheadForward;
			}

			followDirection = followDirection.SafeNormalize(aheadForward);
			segment.Center = ahead.Center - followDirection * followDistance;

			Vector2 visualDirection = Vector2.Lerp(followDirection, aheadForward, curvatureBias).SafeNormalize(followDirection);
			float desiredRotation = visualDirection.ToRotation() + MathHelper.PiOver2;
			segment.rotation = SmoothAngle(segment.rotation, desiredRotation, rotationSmoothing);
			segment.velocity = Vector2.Zero;
		}

		public static float SmoothAngle(float currentAngle, float targetAngle, float amount) {
			return currentAngle + MathHelper.WrapAngle(targetAngle - currentAngle) * MathHelper.Clamp(amount, 0f, 1f);
		}
	}
}
