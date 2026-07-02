// Documenta em codigo o conceito futuro de Mimic Clown Design, ainda sem spawn no jogo.

namespace ChaoticDimensions.Content.BossConcepts
{
	/// <summary>
	/// Final-boss design specification. Rhythm timings are expressed as musical
	/// subdivisions so a future implementation can bind them to the active track.
	/// </summary>
	internal static class MimicClownDesign
	{
		public const string DisplayName = "Palhaço Mímico";
		public const int BaseLife = 179000000;
		public const int SuggestedDefense = 420;
		public const string ProgressionSlot = "Último boss do Chaotic Dimensions";

		public static readonly string[] Forms = {
			"Palhaço de Circo",
			"Olho do Caos",
			"Monthra Mímico",
			"Crystaline Mímico",
			"Kraken Mímico",
			"Moon Lord Mímico",
			"Forma Verdadeira"
		};

		public static readonly BossAttackConcept[] Attacks = {
			new(
				"Passos de Abertura",
				"Holofotes marcam quatro batidas antes de cada salto.",
				"O palhaço aproxima-se com saltos corpo a corpo sincronizados ao tempo.",
				"O jogador desvia na batida forte e evita reagir antecipadamente.",
				240
			),
			new(
				"Espelho de Boss",
				"A silhueta do boss copiado surge durante um compasso completo.",
				"A forma muda e usa uma versão curta de um padrão conhecido.",
				"O jogador reconhece a silhueta e muda imediatamente o tipo de movimento.",
				300
			),
			new(
				"Corredor de Facas",
				"Linhas de palco acendem em colcheias alternadas.",
				"Facas fecham corredores sucessivos sem perseguir diretamente.",
				"O jogador executa uma sequência fixa de desvios no ritmo.",
				180
			),
			new(
				"Valsa dos Martelos",
				"Três pancadas sonoras mostram a ordem esquerda-centro-direita.",
				"Martelos gigantes atingem a arena no primeiro tempo de cada compasso.",
				"O jogador memoriza a ordem e ocupa o setor acabado de atingir.",
				210
			),
			new(
				"Silêncio Falso",
				"A música e os elementos de interface diminuem durante dois tempos.",
				"O boss prepara um ataque corpo a corpo atrasado, fora da expectativa visual.",
				"O jogador segue a animação da mão, não apenas a música.",
				120
			),
			new(
				"Refrão Caótico",
				"Todas as marcas anteriores reaparecem com uma cor por família de ataque.",
				"A forma verdadeira combina dash, faixas, projéteis e golpes no chão.",
				"O jogador prioriza a cor da ameaça que resolve primeiro.",
				360
			),
			new(
				"Última Gargalhada",
				"A barra de vida bloqueia em 1% e oito batidas são contadas no cenário.",
				"O boss executa uma sequência final sem receber dano.",
				"O jogador sobrevive às oito batidas; a janela final permite concluir a luta.",
				300
			)
		};

		public static readonly BossDropConcept[] Drops = {
			new("Coração da Dimensão Caótica", "Material final para o equipamento de conclusão.", true),
			new("Máscara do Último Ato", "Acessório de ritmo que recompensa ações no tempo da música.", false),
			new("Arsenal Mímico", "Escolha entre arma melee, ranged, magic ou summon final.", false),
			new("Caixa de Música do Último Ato", "Registo da música e da vitória final.", false)
		};
	}
}
