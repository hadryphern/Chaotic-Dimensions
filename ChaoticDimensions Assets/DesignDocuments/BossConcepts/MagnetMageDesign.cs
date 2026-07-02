// Documenta em codigo o conceito futuro de Magnet Mage Design, ainda sem spawn no jogo.

namespace ChaoticDimensions.Content.BossConcepts
{
	/// <summary>
	/// Pre-Lunatic Cultist design specification for the Magnet Mage.
	/// This class contains no runtime hooks and is intentionally non-functional.
	/// </summary>
	internal static class MagnetMageDesign
	{
		public const string DisplayName = "Mago Imã";
		public const int BaseLife = 980000;
		public const int SuggestedDefense = 72;
		public const string ProgressionSlot = "Pós-Golem e pré-Cultista Lunático";

		public static readonly BossAttackConcept[] Attacks = {
			new(
				"Entrada do Arquimago Polar",
				"Runas metálicas fecham-se como um compasso durante a cutscene.",
				"O mago materializa-se no centro de um círculo de fragmentos suspensos.",
				"O jogador identifica os quatro polos antes de recuperar o controlo.",
				210
			),
			new(
				"Inversão Cardinal",
				"Uma seta magnética aponta durante 45 ticks para a direção do próximo impulso.",
				"O boss mantém distância e altera o polo ativo.",
				"O jogador compensa um impulso forte para cima, baixo, esquerda ou direita.",
				150
			),
			new(
				"Rede Polarizada",
				"Quatro nós luminosos desenham previamente os limites da rede.",
				"A rede atravessa a arena e reduz mudanças bruscas de direção.",
				"O jogador passa pelo intervalo entre os nós antes do fecho.",
				135
			),
			new(
				"Núcleo de Atração",
				"Anéis concêntricos contraem-se em redor do mago.",
				"O jogador é puxado progressivamente para o centro enquanto lâminas orbitam.",
				"O jogador corre na tangente e usa impulsos apenas perto do anel interno.",
				180
			),
			new(
				"Repulsão de Massa",
				"O ecrã perde saturação e os objetos metálicos vibram durante 36 ticks.",
				"Uma onda radial lança jogador e projéteis para fora.",
				"O jogador procura um corredor sem minas magnéticas antes da onda.",
				105
			),
			new(
				"Hipnose Ferromagnética",
				"Um pêndulo metálico oscila três vezes sobre o boss.",
				"Os comandos horizontais são invertidos por intervalos curtos e anunciados.",
				"O jogador segue o pêndulo; cada inversão tem uma pausa segura.",
				210
			),
			new(
				"Órbita dos Quatro Polos",
				"Quatro símbolos N/S surgem nos cantos da arena.",
				"O boss teleporta entre polos e curva projéteis existentes.",
				"O jogador alterna entre centro e borda conforme o polo ativo.",
				240
			)
		};

		public static readonly BossDropConcept[] Drops = {
			new("Núcleo Magnetizado", "Componente obrigatório do futuro Crystaline Sigil definitivo.", true),
			new("Fio Polar", "Material para armas de controlo e acessórios de mobilidade.", false),
			new("Lente de Fluxo", "Acessório que reduz puxões e melhora aceleração aérea.", false),
			new("Grimório dos Polos", "Arma mágica que alterna atração e repulsão.", false),
			new("Lança Gauss", "Arma ranged de disparos carregados com perfuração.", false)
		};
	}
}
