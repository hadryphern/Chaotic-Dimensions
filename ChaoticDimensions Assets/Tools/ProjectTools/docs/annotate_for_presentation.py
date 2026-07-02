from pathlib import Path
from datetime import datetime
import re
import shutil

ROOT = Path(__file__).resolve().parents[2]
BACKUP = ROOT / "tmp" / "comment_backups" / datetime.now().strftime("%Y%m%d_%H%M%S")
files = sorted(
    [p for p in ROOT.rglob("*.cs") if not any(x in {".git", "bin", "obj", "tmp", "tools"} for x in p.relative_to(ROOT).parts)],
    key=lambda p: p.relative_to(ROOT).as_posix().lower()
)

exact = {
    "ChaoticDimensions.cs": "Liga o mod ao ciclo de vida do tModLoader e trata as mensagens de rede.",
    "Common/Systems/KrakenEventSystem.cs": "Coordena a entrada, tempestade, camera e efeitos gerais da luta do Kraken.",
    "Content/NPCs/Kraken/KrakenBoss.cs": "Implementa as fases, movimentos, ataques e desenho do Alien Kraken.",
    "Content/Bosses/Monthra/MonthraBoss.cs": "Implementa os seis estados de combate e a progressao de raiva da Monthra.",
    "Content/Bosses/CrystalineDevourer/CrystalineDevourerHead.cs": "Controla os dois vermes, a vida partilhada e os ataques do Crystaline Devourer.",
    "Common/Systems/CrystalineDevourerArenaSystem.cs": "Cria a arena temporaria do Crystaline e restaura os tiles no final.",
    "Common/Systems/CrystalineDevourerIntroSystem.cs": "Controla a duracao e o desenho da apresentacao do Crystaline Devourer.",
    "Common/Systems/MonthraIntroSystem.cs": "Controla a apresentacao da Monthra antes de criar o NPC do boss.",
    "Common/Graphics/CrystalineDevourerSky.cs": "Desenha o ceu cosmico proprio da luta do Crystaline Devourer.",
    "Common/Graphics/MonthraGalaxySky.cs": "Desenha a galaxia rosa usada durante a luta da Monthra.",
    "Content/Scenes/CrystalineDevourerScene.cs": "Ativa a musica e o ceu do Crystaline apenas durante o encontro.",
    "Content/Scenes/MonthraScene.cs": "Ativa a musica, o filtro e o ceu rosa durante a luta da Monthra.",
    "Content/SceneEffects/KrakenSceneEffect.cs": "Liga a musica e o filtro espectral Moon Lord ao evento do Kraken.",
    "Common/Progression/ChaoticProgressionHelper.cs": "Centraliza as verificacoes de progressao usadas por itens, NPCs e receitas.",
    "Common/Systems/ChaoticDownedBossSystem.cs": "Guarda no mundo quais bosses do mod ja foram derrotados.",
}

def humanize(stem):
    return re.sub(r"([a-z0-9])([A-Z])", r"\1 \2", stem).replace("_", " ").strip()

def purpose(path):
    rel = path.relative_to(ROOT).as_posix()
    if rel in exact:
        return exact[rel]
    name = humanize(path.stem)
    if "/Items/" in rel:
        return f"Reune valores, receitas e efeitos dos itens de {name}."
    if "/Projectiles/" in rel:
        return f"Controla movimento, dano e efeitos visuais dos projecteis de {name}."
    if "/NPCs/" in rel:
        return f"Implementa a IA, animacao e regras dos NPCs de {name}."
    if "/Bosses/" in rel:
        return f"Contem uma parte do combate e dos recursos do boss {name}."
    if "/Buffs/" in rel:
        return f"Regista os buffs e debuffs agrupados em {name}."
    if "/Tiles/" in rel:
        return f"Define os blocos, colocacao e interacoes de {name}."
    if rel.startswith("Common/Systems/"):
        return f"Mantem o estado global e o ciclo de vida de {name}."
    if rel.startswith("Common/Graphics/") or rel.startswith("Content/Backgrounds/"):
        return f"Desenha e atualiza os elementos visuais de {name}."
    if "/Scenes/" in rel or "/SceneEffects/" in rel:
        return f"Decide quando ativar a musica e os efeitos de cena de {name}."
    if "/Players/" in rel:
        return f"Guarda e atualiza efeitos aplicados ao jogador em {name}."
    if "/Progression/" in rel:
        return f"Define regras e etapas de progressao relacionadas com {name}."
    if "/Menus/" in rel:
        return f"Personaliza a apresentacao do menu atraves de {name}."
    if "/BossBars/" in rel:
        return f"Desenha a barra de vida especial de {name}."
    if "/BossConcepts/" in rel:
        return f"Documenta em codigo o conceito futuro de {name}, ainda sem spawn no jogo."
    if rel.startswith("tmp/") or rel.startswith("tools/"):
        return f"Ferramenta auxiliar usada no desenvolvimento de {name}."
    return f"Reune a implementacao e os tipos auxiliares de {name}."

common = {
    "SetStaticDefaults": "Regista metadados que nao mudam durante a execucao.",
    "SetDefaults": "Define os valores usados quando esta entidade e criada.",
    "AI": "Atualiza o comportamento desta entidade a cada tick.",
    "PreAI": "Executa a logica principal antes da IA padrao do Terraria.",
    "FindFrame": "Escolhe o frame da animacao apresentado neste tick.",
    "PreDraw": "Desenha o recurso manualmente quando o desenho padrao nao e suficiente.",
    "AddRecipes": "Regista as receitas deste conteudo.",
    "ModifyNPCLoot": "Define as recompensas entregues ao derrotar o NPC.",
    "OnKill": "Atualiza o estado do mundo quando a entidade e derrotada.",
    "SendExtraAI": "Envia o estado adicional necessario no modo multijogador.",
    "ReceiveExtraAI": "Recebe o estado adicional enviado pela rede.",
    "NetSend": "Serializa o estado global para os clientes.",
    "NetReceive": "Reconstrui o estado global recebido do servidor.",
    "Load": "Regista os recursos criados ao carregar o mod.",
    "Unload": "Liberta referencias para permitir recarregar o mod.",
    "IsSceneEffectActive": "Confirma se esta cena deve ficar ativa para o jogador.",
    "SpecialVisuals": "Liga ou desliga o filtro e o ceu desta cena.",
}

specific = {
    "ChaoticDimensions.cs": {
        "HandlePacket": "Distribui cada pacote pelo sistema certo e valida o remetente.",
    },
    "Common/Systems/KrakenEventSystem.cs": {
        "StartEvent": "Inicializa o encontro, escolhe o jogador responsavel e inicia a tempestade.",
        "StopEvent": "Encerra o encontro e devolve o clima ao estado normal.",
        "PostUpdateEverything": "Avanca o evento e sincroniza chuva, fases e spawn do boss.",
        "ModifyScreenPosition": "Mantem o enquadramento no Kraken sem bloquear o jogador.",
        "PostDrawInterface": "Desenha titulo, flash, tinta e vinheta sobre a interface.",
        "SpawnKraken": "Cria o NPC no servidor e marca o evento como iniciado.",
    },
    "Content/NPCs/Kraken/KrakenBoss.cs": {
        "AI": "Escolhe o estado atual, atualiza a defesa e executa o ataque certo.",
        "GetDefenseForLife": "Aplica a defesa inicial, a quebra aos 80% e o regresso moderado.",
        "UpdateVisualMotion": "Troca entre idle, movimento horizontal e subida com transicao suave.",
        "DoChase": "Move o Kraken em ondas ao redor do jogador entre ataques.",
        "RunAttackPattern": "Agenda ataques por janelas para evitar varias familias ao mesmo tempo.",
        "PatternTick": "Acelera os tempos finais sem multiplicar a quantidade de projecteis.",
        "SpawnCoordinatedCombo": "Escolhe uma unica ameaca principal para manter o padrao legivel.",
        "PreDraw": "Seleciona o atlas, mistura a transicao e desenha a Ruby.",
        "GetAnimationFrame": "Le o frame correto nos atlas de ida e regresso.",
    },
    "Content/Bosses/Monthra/MonthraBoss.cs": {
        "AI": "Atualiza a raiva pela vida perdida e executa um dos seis estados.",
        "Duration": "Encurta cada estado gradualmente quando a Monthra perde vida.",
        "RunHoverVolley": "Mantem voo lateral e dispara leques previstos.",
        "RunDashChain": "Alterna preparacao, dash e uma janela curta de recuperacao.",
        "RunPrismaticPursuit": "Cria leques de raios com aviso antes do dano.",
        "RunLightLattice": "Cria a grelha de luz deixando uma zona segura.",
        "RunSolarSpiral": "Mantem uma onda giratoria de bolas de fogo.",
        "RunButterflySwarm": "Orbita o jogador enquanto cria os minions borboleta.",
        "SwitchState": "Muda de ataque e reinicia o temporizador sincronizado.",
    },
    "Content/Bosses/CrystalineDevourer/CrystalineDevourerHead.cs": {
        "InitializeIfNeeded": "Define o verme lider e prepara a arena partilhada.",
        "SpawnSegmentsIfNeeded": "Cria os segmentos ligados que formam o corpo.",
        "SpawnTwinIfNeeded": "Cria o segundo verme e liga a sua vida ao lider.",
        "SyncSharedLife": "Mantem os dois vermes com a mesma proporcao de vida.",
        "RunLeaderState": "Escolhe entre orbita, dash e laser supremo.",
        "UpdateProjectilePressure": "Controla a frequencia dos shards.",
        "EndEncounterImmediately": "Remove NPCs, projecteis e arena sem jogadores validos.",
    },
    "Common/Systems/CrystalineDevourerArenaSystem.cs": {
        "EnsureArena": "Calcula o centro e guarda os tiles que serao substituidos.",
        "CreateBarrier": "Constroi o limite temporario em redor dos jogadores.",
        "RestoreBarrier": "Repoe exatamente os tiles guardados antes da luta.",
        "KillPlayersOutsideArena": "Aplica a regra de limite aos participantes vivos.",
        "ShutdownEncounter": "Inicia a limpeza segura quando o encontro termina.",
    },
    "Common/Systems/CrystalineDevourerIntroSystem.cs": {
        "StartIntro": "Inicia a apresentacao e guarda o jogador que invocou.",
        "PostUpdateEverything": "Avanca a apresentacao e cria o boss no final.",
        "DrawIntro": "Desenha o cartao com entrada, permanencia e fade.",
    },
}

core = set(specific) | {
    "Common/Graphics/MonthraGalaxySky.cs",
    "Common/Graphics/CrystalineDevourerSky.cs",
    "Content/Scenes/MonthraScene.cs",
    "Content/Scenes/CrystalineDevourerScene.cs",
    "Content/SceneEffects/KrakenSceneEffect.cs",
}
pattern = re.compile(r"^(?P<i>\s*)(?:public|private|protected|internal)\s+(?:static\s+)?(?:override\s+)?(?:sealed\s+)?[\w<>,\[\]?\.]+\s+(?P<n>\w+)\s*\(")

BACKUP.mkdir(parents=True, exist_ok=True)
changed = detailed = 0
for path in files:
    rel = path.relative_to(ROOT)
    destination = BACKUP / rel
    destination.parent.mkdir(parents=True, exist_ok=True)
    shutil.copy2(path, destination)
    original = path.read_text(encoding="utf-8")
    header = "// " + purpose(path)
    text = original if header in original.splitlines()[:4] else header + "\n\n" + original
    if rel.as_posix() in core:
        output = []
        local = specific.get(rel.as_posix(), {})
        for line in text.splitlines():
            match = pattern.match(line)
            if match:
                comment = local.get(match.group("n")) or common.get(match.group("n"))
                previous = next((x.strip() for x in reversed(output) if x.strip()), "")
                if comment and not previous.startswith("//"):
                    output.append(match.group("i") + "// " + comment)
                    detailed += 1
            output.append(line)
        text = "\n".join(output) + ("\n" if original.endswith("\n") else "")
    if text != original:
        path.write_text(text, encoding="utf-8")
        changed += 1

print(f"Annotated {changed} files; inserted {detailed} detailed comments")
print(f"Backup: {BACKUP}")
