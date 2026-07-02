// Documenta em codigo o conceito futuro de Boss Design Models, ainda sem spawn no jogo.

namespace ChaoticDimensions.Content.BossConcepts
{
	/// <summary>
	/// Design-only data. These records deliberately do not inherit from any
	/// tModLoader content type, so they cannot register or spawn in game.
	/// </summary>
	internal readonly record struct BossAttackConcept(
		string Name,
		string Telegraph,
		string Movement,
		string PlayerResponse,
		int SuggestedDurationTicks
	);

	internal readonly record struct BossDropConcept(
		string Name,
		string Purpose,
		bool RequiredForProgression
	);
}
