from collections import defaultdict
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / "docs" / "Guiao_Apresentacao_Codigo.md"
files = sorted(
    [p for p in ROOT.rglob("*.cs") if not any(x in {".git", "bin", "obj", "tmp"} for x in p.relative_to(ROOT).parts)],
    key=lambda p: p.relative_to(ROOT).as_posix().lower()
)

def purpose(path):
    first = path.read_text(encoding="utf-8").splitlines()[0]
    return first.removeprefix("//").strip()

def types(path):
    text = path.read_text(encoding="utf-8")
    return list(dict.fromkeys(re.findall(r"\b(?:class|struct|enum|record)\s+(\w+)", text)))

def method_line(relative, method):
    path = ROOT / relative
    for number, line in enumerate(path.read_text(encoding="utf-8").splitlines(), 1):
        if re.search(rf"^\s*(?:public|private|protected|internal)\s+.*\b{method}\s*\(", line):
            return number
    return "?"

lines = [
    "# Guião de apresentação do código",
    "",
    "Este ficheiro é um apoio oral. Os comentários no código explicam intenção e decisões importantes sem repetir literalmente cada instrução.",
    "",
    "## Visão geral",
    "",
    "- **ChaoticDimensions.cs** liga o mod ao tModLoader, regista efeitos gráficos e encaminha mensagens de rede.",
    "- **Common/Systems** guarda apresentações, arenas, clima, eventos e bosses derrotados.",
    "- **Content/Bosses e Content/NPCs** contêm as máquinas de estados e a inteligência dos inimigos.",
    "- **Content/Projectiles** separa o comportamento dos ataques da classe principal de cada boss.",
    "- **Content/Scenes** decide quando ativar música, filtros e céus especiais.",
    "- O servidor cria NPCs e projéteis; o cliente trata principalmente de desenho, interface e áudio.",
    "",
    "## Fluxo do Alien Kraken",
    "",
    "1. KrakenSummonItem verifica a progressão e pede o início do evento.",
    "2. KrakenEventSystem.StartEvent inicia a tempestade e sincroniza o encontro.",
    "3. KrakenEventSystem.SpawnKraken cria o NPC apenas no servidor.",
    "4. KrakenBoss.AI escolhe movimento, fase e ataque especial.",
    "5. RunAttackPattern abre janelas de ataque; SpawnCoordinatedCombo escolhe apenas uma ameaça principal.",
    "6. PreDraw seleciona os atlas de animação e desenha a Ruby separadamente.",
    "",
    "## Fluxo da Monthra",
    "",
    "1. A Monthra Butterfly pós-Moon Lord inicia MonthraIntroSystem.",
    "2. MonthraBoss.AI calcula a raiva pela percentagem de vida perdida.",
    "3. A máquina de estados alterna volley, dash, raios, grelha, espiral e enxame.",
    "4. MonthraScene ativa a música, o filtro e MonthraGalaxySky.",
    "",
    "## Fluxo do Crystaline Devourer",
    "",
    "1. CrystalineSigil inicia a apresentação prolongada.",
    "2. CrystalineDevourerIntroSystem cria o boss no final da apresentação.",
    "3. CrystalineDevourerArenaSystem guarda os tiles e cria uma barreira temporária.",
    "4. O verme líder cria o gémeo, sincroniza a vida e escolhe ataques.",
    "5. Quando o encontro termina, os tiles originais são restaurados.",
    "",
    "## Vocabulário rápido",
    "",
    "- **SetStaticDefaults:** metadados registados uma vez.",
    "- **SetDefaults:** tamanho, dano, defesa, raridade e valores iniciais.",
    "- **AI:** lógica executada a cada tick, normalmente 60 vezes por segundo.",
    "- **PreDraw:** desenho manual antes do Terraria desenhar a entidade.",
    "- **ModSystem:** sistema global ligado ao mundo, interface ou eventos.",
    "- **ModSceneEffect:** música e efeitos ativados por uma condição.",
    "- **Main.netMode:** distingue cliente, servidor dedicado e jogo local.",
    "- **NPC.ai e Projectile.ai:** campos pequenos e sincronizados usados por máquinas de estados.",
    "",
    "## Pontos para abrir durante a apresentação",
    "",
]
for relative, method, explanation in [
    ("ChaoticDimensions.cs", "HandlePacket", "entrada e distribuição dos pacotes de rede"),
    ("Common/Systems/KrakenEventSystem.cs", "StartEvent", "início da tempestade e do evento"),
    ("Content/NPCs/Kraken/KrakenBoss.cs", "RunAttackPattern", "agenda dos ataques"),
    ("Content/NPCs/Kraken/KrakenBoss.cs", "SpawnCoordinatedCombo", "redução do spam"),
    ("Content/Bosses/Monthra/MonthraBoss.cs", "AI", "máquina de estados e raiva"),
    ("Content/Bosses/Monthra/MonthraBoss.cs", "RunLightLattice", "zona segura dos lasers"),
    ("Common/Systems/CrystalineDevourerArenaSystem.cs", "CreateBarrier", "construção temporária da arena"),
    ("Content/Bosses/CrystalineDevourer/CrystalineDevourerHead.cs", "SyncSharedLife", "vida partilhada entre vermes"),
]:
    lines.append(f"- **{relative}:{method_line(relative, method)}** — {method}: {explanation}.")

lines += [
    "",
    "## Roteiro oral de oito minutos",
    "",
    "1. **Objetivo:** desenvolver um mod jogável de Terraria com C# e tModLoader.",
    "2. **Arquitetura:** separar sistemas, bosses, itens, projéteis e recursos visuais.",
    "3. **Kraken:** evento, máquina de estados, animação por atlas e redução de spam.",
    "4. **Monthra:** progressão de raiva e ataques com aviso visual.",
    "5. **Crystaline:** dois NPCs com vida partilhada e arena restaurável.",
    "6. **Multijogador:** criação no servidor e sincronização da AI.",
    "7. **Testes:** compilação, carregamento e correção do filtro nulo da Monthra.",
    "8. **Continuidade:** balanceamento, novos bosses e substituição de placeholders.",
    "",
    "## Respostas curtas para perguntas prováveis",
    "",
    "- **Porque existem vários sistemas?** Para não misturar clima, interface, arena e progressão dentro da classe de um boss.",
    "- **Porque não comentar cada linha?** Comentários úteis explicam intenção; repetir o código piora a leitura e a manutenção.",
    "- **Como se evita spam no Kraken?** Cada janela escolhe uma ameaça principal e as fases alteram precisão e tempo.",
    "- **Como a Monthra fica mais difícil?** A vida perdida reduz durações e aumenta velocidade gradualmente.",
    "- **Como a arena não destrói o mundo?** Cada tile é guardado antes da barreira e restaurado no final.",
    "- **O que corre no servidor?** Spawn, dano, NPCs e projéteis. Céus, filtros e interface ficam no cliente.",
    "",
    "## Mapa de todos os ficheiros C#",
    "",
]

groups = defaultdict(list)
for path in files:
    rel = path.relative_to(ROOT).as_posix()
    parts = rel.split("/")
    group = "/".join(parts[:2]) if len(parts) > 1 else "Raiz"
    groups[group].append(path)

for group in sorted(groups):
    lines += [f"### {group}", ""]
    for path in groups[group]:
        rel = path.relative_to(ROOT).as_posix()
        content = path.read_text(encoding="utf-8")
        type_names = types(path)
        suffix = f" Tipos: {', '.join(type_names[:6])}." if type_names else ""
        lines.append(f"- **{rel}** ({len(content.splitlines())} linhas): {purpose(path)}{suffix}")
    lines.append("")

OUT.parent.mkdir(parents=True, exist_ok=True)
OUT.write_text("\n".join(lines), encoding="utf-8")
print(f"Guide written with {len(files)} files: {OUT}")
