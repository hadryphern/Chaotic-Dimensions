using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

var workspace = @"C:\Users\happi\Documents\My Games\Terraria\tModLoader\ModSources\ChaoticDimensions";
var templatePath = @"C:\Users\happi\Documents\Modelo_Relatório_PAP.docx";
var outputDirectory = Path.Combine(workspace, "output", "doc");
Directory.CreateDirectory(outputDirectory);

var outputPath = Path.Combine(outputDirectory, "Relatorio_PAP_Chaotic_Dimensions_Hadryan_Aguiar.docx");
File.Copy(templatePath, outputPath, true);

var figures = new[]
{
    new FigureSpec(
        1,
        "Figura 1 - Identidade visual principal do projeto Chaotic Dimensions.",
        Path.Combine(workspace, ".github", "readme", "banner.png"),
        "Fonte: elaboração própria, a partir dos assets oficiais do projeto."
    ),
    new FigureSpec(
        2,
        "Figura 2 - Title card do boss Crystaline Devourer dentro do mod.",
        Path.Combine(workspace, ".github", "readme", "gallery", "crystaline-devour-title.png"),
        "Fonte: captura e composição do projeto Chaotic Dimensions."
    ),
    new FigureSpec(
        3,
        "Figura 3 - Exemplo da arena e da leitura visual do encontro Crystaline Devourer.",
        Path.Combine(workspace, ".github", "readme", "gallery", "crystaline-devour-ring.png"),
        "Fonte: captura e composição do projeto Chaotic Dimensions."
    ),
    new FigureSpec(
        4,
        "Figura 4 - Wiki pública do mod usada como base de documentação contínua.",
        Path.Combine(workspace, "tmp", "figures", "wiki-home.png"),
        "Fonte: captura do site oficial da wiki do projeto."
    )
}.Where(fig => File.Exists(fig.Path)).ToArray();

using (var document = WordprocessingDocument.Open(outputPath, true))
{
var mainPart = document.MainDocumentPart ?? throw new InvalidOperationException("O documento não possui MainDocumentPart.");
var mainDocument = mainPart.Document ?? throw new InvalidOperationException("O documento não possui documento principal.");
var body = mainDocument.Body ?? throw new InvalidOperationException("O documento não possui Body.");

ReplaceParagraphText(body, "2020/ 2023", "2023 / 2026");
ReplaceParagraphText(body, "Nome do aluno", "Hadryan Aguiar");
ReplaceParagraphText(body, "Turma", "Curso Profissional Técnico de Gestão e Programação de Sistemas Informáticos");
ReplaceParagraphText(body, "Agualva-Cacém, julho de 2023", "Agualva-Cacém, 31 de março de 2026");
ReplaceParagraphText(body, "Tema", "Modding, desenvolvimento de videojogos e documentação técnica");
ReplaceParagraphText(body, "Nome da PAP", "Chaotic Dimensions Mod");
ReplaceParagraphText(body, "Orientador", "Orientadoras");
ReplaceParagraphText(body, "Nome do orientador", "Professora Rute e Professora Fernanda");
ReplaceParagraphText(body, "Diretor de Curso", "Diretora do Curso");
ReplaceParagraphText(body, "Carlos Duarte Oliveira Domingues", "Professora Fernanda");

var coverPlaceholder = FindParagraph(body, "Título da PAP e respetivo logo");
if (coverPlaceholder is not null)
{
    ReplaceParagraphRuns(coverPlaceholder, string.Empty);
    var bannerPath = Path.Combine(workspace, ".github", "readme", "banner.png");
    if (File.Exists(bannerPath))
    {
        InsertAfter(coverPlaceholder, new[] { CreateImageParagraph(mainPart, bannerPath, 520, "Banner do projeto Chaotic Dimensions") });
    }
}

var tocEntries = new[]
{
    "Agradecimentos",
    "Siglas e Acrónimos",
    "Glossário",
    "Índice de Figuras",
    "Introdução",
    "  - Enquadramento",
    "  - Apresentação do projeto",
    "  - Fundamentação da escolha do projeto",
    "  - Finalidades",
    "  - Tecnologias utilizadas",
    "Protótipo do Projeto",
    "Modelo lógico",
    "Modelo Físico",
    "Desenvolvimento",
    "Testes",
    "Melhorias e correção de erros",
    "Reflexão Crítica",
    "Conclusão",
    "Webgrafia",
    "Anexos"
};

ReplaceSection(body, "Agradecimentos", "Siglas e Acrónimos", new OpenXmlElement[]
{
    P("Em primeiro lugar, agradeço às professoras Rute e Fernanda pelo acompanhamento, pela exigência e pela disponibilidade demonstradas ao longo do desenvolvimento desta Prova de Aptidão Profissional. O projeto Chaotic Dimensions foi ganhando forma com muitas iterações, dúvidas, correções e decisões técnicas complexas, e esse acompanhamento foi importante para manter o foco e transformar uma ideia criativa num trabalho real, documentado e funcional."),
    P("Agradeço também à minha família e às pessoas próximas que acompanharam este percurso entre 2023 e 2026, em especial nos momentos de maior carga de trabalho, testes repetidos, criação de sprites, correção de erros e preparação da documentação. Este relatório representa não apenas o resultado final, mas também um processo longo de aprendizagem prática."),
    P("Por fim, agradeço à comunidade de Terraria e tModLoader, porque a existência de documentação, exemplos, sprites vanilla, ferramentas de modding e referências técnicas tornou possível estudar soluções, comparar comportamentos e desenvolver um mod com uma identidade própria.")
});

ReplaceSection(body, "Siglas e Acrónimos", "Glossário", new OpenXmlElement[]
{
    Bullet("PAP – Prova de Aptidão Profissional."),
    Bullet("IDE – Integrated Development Environment, ambiente de desenvolvimento utilizado para programar e organizar o projeto."),
    Bullet("UI – User Interface, elementos de interface apresentados ao jogador."),
    Bullet("UX – User Experience, experiência de utilização e legibilidade do mod."),
    Bullet("PNG – formato de imagem utilizado para sprites, ícones, tiles e elementos visuais."),
    Bullet("XML – formato utilizado internamente em ficheiros do ecossistema Office Open XML e em parte da estrutura documental."),
    Bullet("DOCX – formato do relatório oficial da PAP."),
    Bullet("API – conjunto de interfaces usadas pelo Terraria, tModLoader e bibliotecas auxiliares."),
    Bullet("Worldgen – processo de geração procedural do mundo no Terraria."),
    Bullet("Placeholder – arte temporária usada até existir uma sprite final.")
});

ReplaceSection(body, "Glossário", "Índice", new OpenXmlElement[]
{
    Bullet("Mod – extensão criada por terceiros para alterar ou expandir um videojogo."),
    Bullet("Boss – inimigo principal com padrões de ataque próprios, maior vida, drops únicos e papel importante na progressão."),
    Bullet("Biome / Bioma – região do mapa com comportamento, visuais, música, blocos e spawns próprios."),
    Bullet("Tile – textura e lógica associadas a blocos colocados no mundo do Terraria."),
    Bullet("Sprite sheet – imagem que contém múltiplos frames de animação de um mob, projétil ou efeito."),
    Bullet("Projectile / Projétil – entidade disparada por armas, bosses ou minions."),
    Bullet("Minion – criatura invocada pela classe summoner para atacar de forma automática."),
    Bullet("Buff / Debuff – efeito positivo ou negativo aplicado ao jogador ou a um NPC."),
    Bullet("Scene – camada do mod responsável por música, atmosfera e apresentação contextual de determinados encontros."),
    Bullet("Asset pipeline – fluxo de criação, substituição, teste e sincronização das imagens e outros recursos do projeto.")
});

ReplaceSection(body, "Índice", "Índice de Figuras", tocEntries.Select(TocLine).ToArray());

var figureIndexContent = figures
    .Select(fig => Bullet($"{fig.Title}"))
    .Cast<OpenXmlElement>()
    .ToArray();

ReplaceSection(body, "Índice de Figuras", "Introdução", figureIndexContent);

ReplaceSubsection(body, "Enquadramento", "Apresentação do projeto", new OpenXmlElement[]
{
    P("A presente PAP enquadra-se na área do desenvolvimento de software aplicado ao entretenimento interativo, mais concretamente no modding de videojogos. O projeto desenvolvido, Chaotic Dimensions Mod, tem como objetivo expandir o Terraria através do tModLoader com bosses, biomas, itens, sistemas, documentação e identidade visual próprias."),
    P("Ao contrário de um exercício meramente académico, este trabalho parte de um projeto real em evolução contínua. Ao longo do desenvolvimento foram tomadas decisões de arquitetura, de game design, de integração visual, de organização documental e de correção de erros, semelhantes às de um projeto prático de software com utilizadores, testes e versões intermédias."),
    P("O relatório pretende documentar esse processo de forma clara: o contexto do projeto, os protótipos iniciais, a passagem de ideias descartadas para soluções estáveis, o conteúdo atualmente implementado e os problemas técnicos que tiveram de ser resolvidos.")
});

ReplaceSubsection(body, "Apresentação do projeto", "Fundamentação da escolha do projeto", new OpenXmlElement[]
{
    P("Chaotic Dimensions Mod é um mod autoral para Terraria / tModLoader. A visão do projeto é construir um universo próprio dentro do jogo, com forte identidade visual, bosses marcantes, progressão diferenciada, biomas personalizados, música específica, title cards, armas e sistemas exclusivos."),
    P("Durante o período de desenvolvimento, o projeto passou por várias fases. Em 2024 existiu uma base embrionária em Minecraft, posteriormente usada como referência criativa e de conteúdo. Mais tarde, o conceito foi transportado para Terraria, aproveitando melhor a linguagem visual em pixel art, a estrutura de bosses em 2D e a flexibilidade do ecossistema tModLoader."),
    P("Na versão documentada neste relatório, o mod já possui dois grandes marcos: a Monthra, como boss de progressão pré-hardmode, e o Crystaline Devourer, como boss pós-Moon Lord com intro cinemática, arena forçada, música própria e recompensas de alto nível. Em paralelo, encontra-se implementado o ShadowBiome, que introduz progressão de risco, worldgen próprio e novo tier de conteúdo.")
});

ReplaceSubsection(body, "Fundamentação da escolha do projeto", "Finalidades", new OpenXmlElement[]
{
    P("A escolha deste projeto foi motivada pela vontade de unir programação, game design, criação visual e documentação num trabalho único. Terraria e tModLoader permitiram trabalhar várias áreas ao mesmo tempo: lógica em C#, estrutura de software, integração de assets, testes no jogo e publicação de documentação num site próprio."),
    P("A decisão de desenvolver um mod, em vez de um software mais tradicional, tornou o processo mais desafiante e também mais completo. Cada funcionalidade precisava de ser pensada a dois níveis: tecnicamente, para funcionar no motor do jogo; e ludicamente, para fazer sentido na progressão, no equilíbrio e na leitura visual do jogador."),
    P("Outro fator importante foi a possibilidade de desenvolver um projeto verdadeiramente pessoal. Chaotic Dimensions não depende de um tema genérico; ele foi sendo moldado com identidade própria, o que permitiu trabalhar criatividade, coerência interna e capacidade de revisão crítica ao longo de três anos letivos.")
});

ReplaceSubsection(body, "Finalidades", "Tecnologias utilizadas", new OpenXmlElement[]
{
    Bullet("Desenvolver um mod autoral para Terraria com conteúdo jogável, progressão própria e base técnica sólida."),
    Bullet("Aplicar programação orientada a objetos em C#, integrando sistemas do tModLoader, worldgen, entidades, UI, música e assets."),
    Bullet("Criar bosses, biomas, itens, blocos e regras de progressão com identidade visual coerente."),
    Bullet("Documentar detalhadamente o desenvolvimento, incluindo problemas encontrados, soluções aplicadas e decisões de projeto."),
    Bullet("Organizar uma wiki pública capaz de acompanhar a evolução do mod mesmo quando determinados assets ainda se encontram em placeholder."),
    Bullet("Concluir a PAP com um relatório formal, tecnicamente detalhado e fiel ao trabalho efetivamente realizado.")
});

ReplaceSubsection(body, "Tecnologias utilizadas", "Protótipo do Projeto", new OpenXmlElement[]
{
    Bullet("Terraria – jogo base sobre o qual o mod é executado."),
    Bullet("tModLoader – framework principal para criação, compilação e execução do mod."),
    Bullet("C# e .NET – linguagem e plataforma usadas em todo o código do projeto."),
    Bullet("Open XML SDK – biblioteca utilizada para manipulação programática do presente relatório em formato DOCX."),
    Bullet("PowerShell – automação local, cópia de assets, sincronização de ficheiros e tarefas auxiliares."),
    Bullet("PNG / pixel art – formato base de sprites, itens, projéteis, tiles e capturas utilizadas no mod e na wiki."),
    Bullet("GitHub Pages – publicação da wiki do projeto em ambiente web."),
    Bullet("Estrutura documental própria do repositório – markdowns, auditorias de sprites, referências de wiki e workflows internos.")
});

ReplaceSection(body, "Protótipo do Projeto", "Modelo lógico", new List<OpenXmlElement>
{
    P("O protótipo do projeto não surgiu de forma linear. A primeira fase relevante foi uma versão experimental de Chaotic Dimensions concebida para Minecraft em 2024. Essa fase serviu sobretudo para definir nomes, materiais, criaturas e direção criativa. Contudo, as limitações dessa versão, aliadas ao interesse crescente pelo ecossistema do Terraria, levaram à decisão de redesenhar o projeto para tModLoader."),
    P("Na migração para Terraria, o objetivo não foi copiar o protótipo anterior de forma mecânica. O trabalho passou por reinterpretar sistemas inteiros para um jogo 2D com outra linguagem de combate, outra leitura visual e outra forma de estruturar bosses, worldgen e progressão. Isso permitiu abandonar soluções que não funcionavam bem e aproveitar apenas o que fazia sentido na nova plataforma."),
    P("Também existiram protótipos que foram conscientemente rejeitados. Entre eles estiveram experiências com port direto de conteúdo OreSpawn, tentativas de pseudo-dimensões em regiões do mapa e integração de subworlds externas. Embora essas experiências tenham sido úteis como exploração técnica, acabaram removidas por não garantirem a compatibilidade, a clareza de progressão e a estabilidade multiplayer pretendidas."),
    FigureParagraphs(mainPart, figures, 1).ToArray()[0],
    FigureParagraphs(mainPart, figures, 1).ToArray()[1],
    FigureParagraphs(mainPart, figures, 1).ToArray()[2],
    P("A partir desse processo, o protótipo deixou de ser apenas um conjunto de ideias e passou a ser uma fundação técnica concreta: bosses com scene própria, materiais com recipes definidas, biome com worldgen real, wiki viva e pipeline de sprites substituíveis sem quebrar o código.")
}.ToArray());

ReplaceSection(body, "Modelo lógico", "Modelo Físico", new OpenXmlElement[]
{
    P("O modelo lógico do projeto foi estruturado para separar claramente sistemas, conteúdo jogável, documentação e assets. Essa separação permite crescer o mod por blocos, sem concentrar toda a lógica num único ponto e sem misturar ficheiros de runtime com ficheiros de apoio."),
    H2("Arquitetura principal"),
    Bullet("Pasta Content: concentra bosses, NPCs, itens, projéteis, buffs, tiles e scenes."),
    Bullet("Pasta Common: reúne sistemas globais, lógica de worldgen, jogadores auxiliares, globais de NPC e utilitários gráficos."),
    Bullet("Pasta Assets: guarda recursos de interface e imagens auxiliares."),
    Bullet("Pasta Sounds: contém a música própria dos bosses e restantes efeitos sonoros."),
    Bullet("Pasta docs: centraliza a wiki local, auditorias de sprites, referências de conteúdo e documentação de manutenção."),
    H2("Fluxo de progressão implementado"),
    Bullet("Pré-hardmode: Monthra surge como boss intermédio, introduz Monthra Scale e um primeiro salto de equipamentos."),
    Bullet("Hardmode intermédio: RosalitaOre é libertado depois dos três mecânicos e abre o tier Rosalita."),
    Bullet("Pós-Moon Lord: Crystaline Devourer funciona como gate duro do late game do mod."),
    Bullet("Late game próprio: ShadowBiome, ShadowOre, ChaosCrystalPickaxe e o tier Shadow introduzem o conteúdo mais extremo atualmente em código."),
    H2("Fluxo documental"),
    Bullet("Toda a alteração importante no mod deve ser refletida também na wiki pública e nos markdowns do repositório."),
    Bullet("Sprites placeholders não impedem publicação de páginas; o estado visual é registado com auditorias próprias."),
    Bullet("A documentação serve simultaneamente como apoio ao desenvolvimento, à validação e à apresentação pública do projeto.")
});

ReplaceSection(body, "Modelo Físico", "Desenvolvimento", new OpenXmlElement[]
{
    P("Fisicamente, o projeto está organizado dentro da pasta de mods do tModLoader, em ModSources, permitindo compilação direta no ambiente do jogo. A estrutura física acompanha a separação lógica definida anteriormente e facilita tanto o desenvolvimento como a manutenção."),
    Bullet(@"Repositório principal: C:\Users\happi\Documents\My Games\Terraria\tModLoader\ModSources\ChaoticDimensions"),
    Bullet(@"Modelo do relatório: C:\Users\happi\Documents\Modelo_Relatório_PAP.docx"),
    Bullet(@"Normas do relatório: C:\Users\happi\Documents\Normas do relatório.pdf"),
    Bullet("Imagens de apresentação e galeria do mod guardadas em .github/readme."),
    Bullet("Capturas e assets auxiliares guardados em tmp, docs/assets e .archive/.structure durante o processo artístico."),
    Bullet("Wiki publicada em GitHub Pages a partir da pasta docs."),
    P("A vertente física do projeto também inclui a gestão de sprites, tiles, cenas, música e documentação paralela. Isso significa que o trabalho não se resume ao código-fonte: envolve igualmente diretórios de recursos, convenções de nomeação, ficheiros de apoio à wiki, screenshots, auditorias de sprites e guias internos para atualização consistente do site."),
    FigureParagraphs(mainPart, figures, 4).ToArray()[0],
    FigureParagraphs(mainPart, figures, 4).ToArray()[1],
    FigureParagraphs(mainPart, figures, 4).ToArray()[2]
});

ReplaceSection(body, "Desenvolvimento", "Testes", new OpenXmlElement[]
{
    P("O desenvolvimento do Chaotic Dimensions decorreu entre 2023 e 2026, com várias mudanças de rumo até chegar a uma base consistente. O processo não foi uma sequência reta de funcionalidades concluídas; houve fases de experimentação, refatoração, remoção de ideias e reconstrução de sistemas que não respondiam bem às exigências do projeto."),
    H2("Fase 1 – Conceção e identidade"),
    P("Na primeira fase foram definidos o nome do projeto, a ideia de um universo próprio e a intenção de criar bosses, progressão e biomas com forte assinatura visual. Ainda sem uma implementação final estável, esta fase foi importante para consolidar a direção criativa: uma estética cósmica, cristalina e sombria, com títulos de boss, música própria e progressão marcada por encontros de alto impacto."),
    H2("Fase 2 – Protótipo original e material de referência"),
    P("Em 2024 existiu uma base em Minecraft que serviu como primeira biblioteca de ideias, materiais, criaturas e nomes. Parte desse conteúdo foi depois auditada e portada para Terraria, enquanto outra parte foi descartada por não encaixar na nova linguagem de jogo. Esta fase foi útil sobretudo como arquivo criativo e não como destino técnico final."),
    H2("Fase 3 – Migração e fundação em Terraria"),
    P("A migração para Terraria trouxe uma mudança de foco: em vez de reproduzir sistemas de outro jogo, foi necessário redesenhar encontros, armas, biomas, tiles e regras de progressão para funcionarem corretamente em 2D. Esta transição levou à criação das classes principais do mod, da sua estrutura de pastas e dos primeiros sistemas sólidos em C#."),
    H2("Fase 4 – Primeiro boss de grande escala: Crystaline Devourer"),
    P("O Crystaline Devourer tornou-se o primeiro grande marco oficial do mod. Foi criado como boss pós-Moon Lord com arena forçada, scene própria, música dedicada, boss bar personalizada, escurecimento de ecrã, title card, múltiplas fases e drops exclusivos. Ao longo do desenvolvimento, a vida total foi reajustada para 5 000 000 e a luta passou por várias correções visuais e de balanceamento."),
    FigureParagraphs(mainPart, figures, 2).ToArray()[0],
    FigureParagraphs(mainPart, figures, 2).ToArray()[1],
    FigureParagraphs(mainPart, figures, 2).ToArray()[2],
    FigureParagraphs(mainPart, figures, 3).ToArray()[0],
    FigureParagraphs(mainPart, figures, 3).ToArray()[1],
    FigureParagraphs(mainPart, figures, 3).ToArray()[2],
    H2("Fase 5 – Monthra e progressão pré-hardmode"),
    P("A Monthra foi introduzida como boss mais simples e mais acessível, situada depois do conteúdo evil do vanilla. A sua invocação acontece através da morte da MonthraButterfly, e o encontro foi pensado para funcionar como gate intermédia antes de conteúdos mais agressivos. O boss recebeu intro cinemática própria, música exclusiva e armas baseadas em Monthra Scale para várias classes."),
    H2("Fase 6 – ShadowBiome, Rosalita e Shadow tier"),
    P("Uma das adições mais ambiciosas foi o ShadowBiome. Em vez de trabalhar com dimensões externas instáveis, optou-se por um bioma secreto integrado no mundo principal. Este bioma substitui o segmento evil esquerdo, cria uma zona hostil com escuridão, drenagem de vida, worldgen próprio e minérios específicos, e exige o acessório ShadowTotem para exploração segura."),
    P("A partir daí foram implementados dois grandes tiers de progressão: Rosalita, desbloqueado após os três mecânicos, e Shadow, dependente do Crystaline Devourer e do ShadowBiome. Estes tiers incluem armaduras, ferramentas, armas, consumíveis, munições, acessórios, whips, minions e upgrades específicos como a ChaosCrystalPickaxe e a Godness Anvil."),
    H2("Fase 7 – Pipeline visual e documentação viva"),
    P("Ao longo do projeto foi criada uma rotina de substituição de placeholders, auditoria de sprites e sincronização da wiki. Sempre que novos assets eram criados, os respetivos tamanhos reais eram alinhados com o código para evitar bugs de resolução, hitbox, animação ou colocação de tiles. Em paralelo, a wiki passou a ser tratada como fonte oficial pública do estado do mod.")
});

ReplaceSection(body, "Testes", "Melhorias e correção de erros", new OpenXmlElement[]
{
    P("Os testes do projeto foram feitos de forma contínua e prática, combinando compilação local, arranque do mod no tModLoader, testes manuais dentro do jogo, análise de logs e validação visual das sprites e tiles."),
    Bullet("Compilação com dotnet build do projeto para validar classes, referências, recursos e scripts auxiliares."),
    Bullet("Testes ingame para verificar spawns, drops, recipes, worldgen, música, cenas, UI e comportamento de bosses."),
    Bullet("Consulta dos logs do tModLoader para identificar MissingResourceException, falhas de assets, problemas de scene e carregamento de conteúdo."),
    Bullet("Testes de worldgen em mundos novos para validar a colocação do ShadowBiome, dos minérios RosalitaOre e ShadowOre e das mensagens de progressão do mundo."),
    Bullet("Testes visuais a tiles, incluindo comparação direta com minérios vanilla como Adamantite para validar a forma correta de terrain sheet."),
    Bullet("Testes de spritesheets animadas, contagem de frames e orientação de minions e NPCs."),
    Bullet("Verificação da wiki local e da wiki publicada para garantir que o conteúdo documental acompanha o conteúdo efetivo do mod."),
    P("Este método de teste foi essencial porque o desenvolvimento combinou lógica, arte e runtime. Muitos erros só se tornavam visíveis ao abrir o jogo, colocar blocos, iniciar lutas, mudar músicas ou usar itens específicos.")
});

ReplaceSection(body, "Melhorias e correção de erros", "Reflexão Crítica", new OpenXmlElement[]
{
    P("Ao longo da construção do mod foram encontrados vários problemas técnicos e visuais. A correção desses problemas foi uma parte central do trabalho, porque muitos deles afetavam diretamente a estabilidade, a leitura do jogo ou a coerência visual do projeto."),
    H2("Principais problemas resolvidos"),
    Bullet("Vida total do Crystaline Devourer corrigida para 5 000 000, eliminando inconsistências entre cabeça, corpo e cauda."),
    Bullet("Sistema de introdução da Monthra aproximado à lógica do Crystaline Devourer, com música, overlay e title card mais coerentes."),
    Bullet("Minion da Monthra corrigido quando se deslocava de ré, com ajuste de direção e rotação."),
    Bullet("Sprites de minérios revistas: separação correta entre ícone do item em 16x16 e terrain sheet do tile em 288x270."),
    Bullet("RosalitaOreTile e ShadowOreTile refeitos com base correta da Adamantite, corrigindo blocos pretos, cortes e falta de ligação entre tiles."),
    Bullet("Godness Anvil corrigida ao nível do TileObjectData, removendo cortes na sprite e ajustando a posição para deixar de flutuar."),
    Bullet("Alinhamento do tamanho real de sprites novas no código para evitar bugs de resolução, hitbox desajustada e leitura visual incorreta."),
    Bullet("Correção de MissingResourceException causada por assets em falta, através da criação e auditoria de placeholders consistentes."),
    Bullet("ShadowBiome redimensionado após gerar área demasiado grande e invadir metade da selva em testes iniciais."),
    Bullet("Remoção de sistemas experimentais que criavam mais instabilidade do que valor, incluindo tentativas de dimensões externas e integração de conteúdos que não se enquadravam na direção final."),
    H2("Impacto das correções"),
    P("Estas correções não foram apenas cosméticas. Em vários casos, um problema aparentemente visual escondia uma falha estrutural maior, como no caso dos minérios, em que a diferença entre sprite de item e terrain sheet alterava completamente a forma como o jogo colocava e ligava blocos no mundo."),
    P("Do mesmo modo, a organização do pipeline de sprites e a criação de auditorias detalhadas permitiram reduzir erros repetidos e criar um processo mais profissional de integração de arte, algo muito importante num projeto que depende fortemente de recursos visuais próprios.")
});

ReplaceSection(body, "Reflexão Crítica", "Conclusão", new OpenXmlElement[]
{
    P("Este projeto mostrou claramente que desenvolver um mod não é apenas 'adicionar itens ao jogo'. Foi necessário planear arquitetura, aprender a trabalhar com sistemas específicos do tModLoader, compreender o impacto das sprites no runtime, interpretar erros de logs, corrigir worldgen, documentar continuamente o estado do projeto e aceitar que algumas ideias tinham de ser abandonadas para o trabalho evoluir de forma sólida."),
    P("Uma das maiores aprendizagens foi perceber o valor da iteração. Muitas soluções iniciais pareciam funcionar, mas revelavam-se erradas quando testadas em contexto real. Isso aconteceu com as tentativas de dimensões, com certos conteúdos herdados de protótipos antigos e com várias integrações de sprites. O projeto melhorou quando as decisões passaram a ser reavaliadas com mais honestidade técnica."),
    P("Também ficou evidente a importância da documentação. A existência de markdowns internos, auditorias de sprites, referência de wiki e histórico de correções tornou o projeto mais organizado e mais sustentável. Para um trabalho desta dimensão, documentar não foi um extra: foi uma necessidade."),
    P("Se este projeto continuasse para além da PAP, as áreas com maior potencial de crescimento seriam o fecho dos bosses do tier Shadow, a expansão do conteúdo pós-Crystaline Devourer, a finalização das sprites equipáveis ainda em falta e o refinamento do equilíbrio geral entre progressão, dificuldade e recompensas.")
});

ReplaceSection(body, "Conclusão", "Webgrafia", new OpenXmlElement[]
{
    P("A PAP Chaotic Dimensions Mod permitiu transformar uma ideia criativa num projeto técnico real, jogável e documentado. O trabalho produzido ao longo de 2023 a 2026 resultou numa fundação sólida para um mod de Terraria com identidade própria, conteúdo original, pipeline visual em crescimento e documentação pública através de uma wiki."),
    P("O relatório deixa registado não apenas o que foi implementado, mas também a forma como o projeto evoluiu: as tentativas falhadas, as refatorações, as correções de bugs, as decisões de design e a organização necessária para manter coerência entre código, arte e documentação."),
    P("Considero que os objetivos principais da PAP foram atingidos: desenvolver conteúdo funcional, estruturar tecnicamente o mod, resolver problemas reais de implementação e apresentar o projeto de forma organizada e profissional, respeitando o modelo oficial do relatório e as normas definidas.")
});

ReplaceSection(body, "Webgrafia", "Anexos", new OpenXmlElement[]
{
    P("Chaotic Dimensions Wiki Oficial. Acedido em 31 de março de 2026, em: https://hadryphern.github.io/Chaotic-Dimensions/?lang=pt-BR"),
    P("Microsoft. C# documentation. Acedido em 31 de março de 2026, em: https://learn.microsoft.com/en-us/dotnet/csharp/"),
    P("Microsoft. .NET documentation. Acedido em 31 de março de 2026, em: https://learn.microsoft.com/en-us/dotnet/"),
    P("Open XML SDK. Acedido em 31 de março de 2026, em: https://github.com/dotnet/Open-XML-SDK"),
    P("tModLoader Team. tModLoader Wiki. Acedido em 31 de março de 2026, em: https://github.com/tModLoader/tModLoader/wiki"),
    P("Terraria Wiki. Acedido em 31 de março de 2026, em: https://terraria.wiki.gg/wiki/Terraria_Wiki")
});

ReplaceSection(body, "Anexos", null, new OpenXmlElement[]
{
    H2("Anexo A – Conteúdo principal implementado"),
    Bullet("Bosses jogáveis do build atual: Monthra e Crystaline Devourer."),
    Bullet("Entidades e suporte do Crystaline Devourer: cabeça, corpo, cauda, shards, beams, sky beams, sigilo, arena cristalina e scene própria."),
    Bullet("Bioma principal já implementado: ShadowBiome, com ShadowDirt, ShadowGrass, ShadowStone, ShadowWood e geração própria."),
    Bullet("Critter de invocação do boss pré-hardmode: MonthraButterfly."),
    Bullet("Mobs exclusivos do ShadowBiome: Phantasm, ShadowEye, ShadowSlime, ShadowWorm e KrakenSquid."),
    H2("Anexo B – Itens, materiais e progressão"),
    Bullet("Linha Crystaline: CrystalineSigil, CrystalineTear, CrystalineEye, CrystalineSword, CrystalineStaff, CrystalineGun, CrystalineBarrierBlock e equipamento relacionado."),
    Bullet("Linha Monthra: MonthraScale, MonthraBlade, MonthraBow, MonthraWand e MonthraButterflyStaff."),
    Bullet("Linha Rosalita: RosalitaOre, RosalitaGem, RosalitaShield, RosalitaHelmet, RosalitaBreastplate, RosalitaGreaves, RosalitaPickaxe, RosalitaAxe, RosalitaHammer, RosalitaBlade, RosalitaBow, RosalitaWand e RosalitaWhip."),
    Bullet("Linha Eclipsed Monthra: EclipsedMonthraPickaxe, EclipsedMonthraAxe, EclipsedMonthraHammer, EclipsedMonthraBlade, EclipsedMonthraBow, EclipsedMonthraWand e EclipsedMonthraWhip."),
    Bullet("Linha Shadow: ShadowOre, ShadowScrap, SoulOfShadow, ShadowTotem, ChaosCrystalPickaxe, ShadowHelmet, ShadowBreastplate, ShadowGreaves, ShadowSummonStaff, ShadowWhip, ShadowBow, ShadowArrow, ShadowBullet, ShadowStaff, ShadowManaPotion, ShadowMeleePotion, HeartOfShadows, GloryBoots, HeartOfTheGod, GodnessAnvil e ShadowZenith."),
    Bullet("Materiais adicionais oriundos do legado do projeto: GlassStick, IronStick, RatrixStick, ChaosCrystal, ShadowBar, VortexGem e outros recursos que continuaram úteis na progressão e crafting."),
    H2("Anexo C – Erros, substituições e decisões de remoção"),
    Bullet("Tentativa de integração de conteúdo OreSpawn posteriormente removida por não representar a direção final do mod."),
    Bullet("Tentativas de pseudo-dimensões e de subworlds descartadas por problemas de coerência, compatibilidade e estabilidade multiplayer."),
    Bullet("Vários mobs legacy removidos quando deixaram de encaixar na visão atual do projeto."),
    Bullet("Revisão do pipeline de minérios após identificação do erro entre sprite de item e terrain sheet."),
    Bullet("Revisão da Godness Anvil após corte de sprite e posicionamento incorreto no mundo."),
    H2("Anexo D – Documentação e website"),
    Bullet("Wiki pública mantida em GitHub Pages para documentar itens, bosses, biomas e sistemas."),
    Bullet("Markdowns internos criados para auditoria de sprites, referência de conteúdos, manutenção do site e inventário de elementos do mod."),
    Bullet("Fluxo de trabalho definido: conteúdo novo no jogo implica atualização documental e sincronização dos assets da wiki."),
    H2("Anexo E – Observação final"),
    P("O presente relatório descreve fielmente o estado do projeto à data da sua geração. Como se trata de um mod em desenvolvimento contínuo, parte do conteúdo já está implementada em código mas ainda depende de sprites finais, balanceamento adicional ou bosses futuros para completar a rota de obtenção natural dentro do jogo.")
});

mainPart.Document.Save();
}

var documentsCopy = @"C:\Users\happi\Documents\Relatorio_PAP_Chaotic_Dimensions_Hadryan_Aguiar.docx";
File.Copy(outputPath, documentsCopy, true);

Console.WriteLine(outputPath);
Console.WriteLine(documentsCopy);

static void ReplaceParagraphText(Body body, string oldText, string newText)
{
    var paragraph = FindParagraph(body, oldText);
    if (paragraph is not null)
    {
        ReplaceParagraphRuns(paragraph, newText);
    }
}

static Paragraph? FindParagraph(Body body, string text) =>
    body.Elements<Paragraph>().FirstOrDefault(p => GetParagraphText(p).Trim() == text);

static string GetParagraphText(Paragraph paragraph) =>
    string.Concat(paragraph.Descendants<Text>().Select(t => t.Text));

static void ReplaceParagraphRuns(Paragraph paragraph, string text)
{
    paragraph.RemoveAllChildren<Run>();
    paragraph.Append(CreateRun(text));
}

static void ReplaceSection(Body body, string headingText, string? nextHeadingText, IReadOnlyList<OpenXmlElement> newContent)
{
    var heading = FindParagraph(body, headingText) ?? throw new InvalidOperationException($"Não foi possível encontrar a secção '{headingText}'.");
    Paragraph? nextHeading = nextHeadingText is null ? null : FindParagraph(body, nextHeadingText);
    RemoveContentBetween(heading, nextHeading);
    InsertAfter(heading, newContent);
}

static void ReplaceSubsection(Body body, string headingText, string nextHeadingText, IReadOnlyList<OpenXmlElement> newContent)
{
    var heading = FindParagraph(body, headingText) ?? throw new InvalidOperationException($"Não foi possível encontrar a subsecção '{headingText}'.");
    var nextHeading = FindParagraph(body, nextHeadingText) ?? throw new InvalidOperationException($"Não foi possível encontrar a subsecção seguinte '{nextHeadingText}'.");
    RemoveContentBetween(heading, nextHeading);
    InsertAfter(heading, newContent);
}

static void RemoveContentBetween(Paragraph start, Paragraph? end)
{
    var current = start.NextSibling();
    while (current is not null && current != end)
    {
        var next = current.NextSibling();
        current.Remove();
        current = next;
    }
}

static void InsertAfter(OpenXmlElement anchor, IEnumerable<OpenXmlElement> elements)
{
    var current = anchor;
    foreach (var element in elements)
    {
        var node = (OpenXmlElement)element.CloneNode(true);
        current.InsertAfterSelf(node);
        current = node;
    }
}

static Paragraph P(string text) => CreateParagraph(text, justify: true, firstLineTwips: 340);

static Paragraph Bullet(string text) => CreateParagraph($"• {text}", justify: false, firstLineTwips: 0, leftTwips: 360);

static Paragraph TocLine(string text) => CreateParagraph(text, justify: false, firstLineTwips: 0, leftTwips: text.StartsWith("  - ") ? 360 : 0);

static Paragraph H2(string text)
{
    var paragraph = new Paragraph(
        new ParagraphProperties(
            new ParagraphStyleId { Val = "Ttulo2" }
        ),
        CreateRun(text, fontName: null, fontSizeHalfPoints: null, bold: false)
    );
    return paragraph;
}

static Paragraph CreateParagraph(
    string text,
    bool justify,
    int firstLineTwips,
    int leftTwips = 0,
    bool center = false,
    int fontSizeHalfPoints = 22,
    bool italic = false)
{
    var paragraphProperties = new ParagraphProperties(
        new SpacingBetweenLines
        {
            After = "240",
            Line = "360",
            LineRule = LineSpacingRuleValues.Auto
        },
        new Indentation
        {
            FirstLine = firstLineTwips.ToString(CultureInfo.InvariantCulture),
            Left = leftTwips.ToString(CultureInfo.InvariantCulture)
        }
    );

    if (center)
    {
        paragraphProperties.Append(new Justification { Val = JustificationValues.Center });
    }
    else if (justify)
    {
        paragraphProperties.Append(new Justification { Val = JustificationValues.Both });
    }
    else
    {
        paragraphProperties.Append(new Justification { Val = JustificationValues.Left });
    }

    return new Paragraph(paragraphProperties, CreateRun(text, "Arial", fontSizeHalfPoints, italic: italic));
}

static Run CreateRun(
    string text,
    string? fontName = "Arial",
    int? fontSizeHalfPoints = 22,
    bool bold = false,
    bool italic = false)
{
    var runProperties = new RunProperties();
    if (!string.IsNullOrWhiteSpace(fontName))
    {
        runProperties.Append(new RunFonts
        {
            Ascii = fontName,
            HighAnsi = fontName,
            ComplexScript = fontName
        });
    }

    if (fontSizeHalfPoints.HasValue)
    {
        runProperties.Append(new FontSize { Val = fontSizeHalfPoints.Value.ToString(CultureInfo.InvariantCulture) });
        runProperties.Append(new FontSizeComplexScript { Val = fontSizeHalfPoints.Value.ToString(CultureInfo.InvariantCulture) });
    }

    if (bold)
    {
        runProperties.Append(new Bold());
    }

    if (italic)
    {
        runProperties.Append(new Italic());
    }

    return new Run(runProperties, new Text(text) { Space = SpaceProcessingModeValues.Preserve });
}

static IReadOnlyList<OpenXmlElement> FigureParagraphs(MainDocumentPart mainPart, IReadOnlyList<FigureSpec> figures, int number)
{
    var figure = figures.FirstOrDefault(f => f.Number == number);
    if (figure is null)
    {
        return Array.Empty<OpenXmlElement>();
    }

    return new OpenXmlElement[]
    {
        CreateParagraph(figure.Title, justify: false, firstLineTwips: 0, center: true, fontSizeHalfPoints: 20),
        CreateImageParagraph(mainPart, figure.Path, 500, figure.Title),
        CreateParagraph(figure.Source, justify: false, firstLineTwips: 0, center: true, fontSizeHalfPoints: 20, italic: true)
    };
}

static Paragraph CreateImageParagraph(MainDocumentPart mainPart, string imagePath, int maxWidthPx, string description)
{
    var (widthPx, heightPx) = GetPngSize(imagePath);
    if (widthPx <= 0 || heightPx <= 0)
    {
        widthPx = maxWidthPx;
        heightPx = maxWidthPx / 2;
    }

    if (widthPx > maxWidthPx)
    {
        var ratio = maxWidthPx / (double)widthPx;
        widthPx = maxWidthPx;
        heightPx = Math.Max(1, (int)Math.Round(heightPx * ratio));
    }

    var imagePart = mainPart.AddImagePart(ImagePartType.Png);
    using (var stream = File.OpenRead(imagePath))
    {
        imagePart.FeedData(stream);
    }

    var relationshipId = mainPart.GetIdOfPart(imagePart);
    const long emusPerPixel = 9525L;
    long cx = widthPx * emusPerPixel;
    long cy = heightPx * emusPerPixel;
    uint drawingId = (uint)(mainPart.ImageParts.Count() + 1000);

    var drawing = new Drawing(
        new DW.Inline(
            new DW.Extent { Cx = cx, Cy = cy },
            new DW.EffectExtent
            {
                LeftEdge = 0L,
                TopEdge = 0L,
                RightEdge = 0L,
                BottomEdge = 0L
            },
            new DW.DocProperties
            {
                Id = drawingId,
                Name = Path.GetFileName(imagePath),
                Description = description
            },
            new DW.NonVisualGraphicFrameDrawingProperties(
                new A.GraphicFrameLocks { NoChangeAspect = true }
            ),
            new A.Graphic(
                new A.GraphicData(
                    new PIC.Picture(
                        new PIC.NonVisualPictureProperties(
                            new PIC.NonVisualDrawingProperties
                            {
                                Id = 0U,
                                Name = Path.GetFileName(imagePath)
                            },
                            new PIC.NonVisualPictureDrawingProperties()
                        ),
                        new PIC.BlipFill(
                            new A.Blip
                            {
                                Embed = relationshipId,
                                CompressionState = A.BlipCompressionValues.Print
                            },
                            new A.Stretch(new A.FillRectangle())
                        ),
                        new PIC.ShapeProperties(
                            new A.Transform2D(
                                new A.Offset { X = 0L, Y = 0L },
                                new A.Extents { Cx = cx, Cy = cy }
                            ),
                            new A.PresetGeometry(new A.AdjustValueList())
                            {
                                Preset = A.ShapeTypeValues.Rectangle
                            }
                        )
                    )
                )
                {
                    Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
                }
            )
        )
        {
            DistanceFromTop = 0U,
            DistanceFromBottom = 0U,
            DistanceFromLeft = 0U,
            DistanceFromRight = 0U
        }
    );

    return new Paragraph(
        new ParagraphProperties(
            new Justification { Val = JustificationValues.Center },
            new SpacingBetweenLines
            {
                After = "200",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            }
        ),
        new Run(drawing)
    );
}

static (int Width, int Height) GetPngSize(string path)
{
    using var stream = File.OpenRead(path);
    using var reader = new BinaryReader(stream);
    var signature = reader.ReadBytes(24);
    if (signature.Length < 24)
    {
        return (0, 0);
    }

    if (signature[0] != 0x89 || signature[1] != 0x50 || signature[2] != 0x4E || signature[3] != 0x47)
    {
        return (0, 0);
    }

    int width = ReadBigEndianInt(signature, 16);
    int height = ReadBigEndianInt(signature, 20);
    return (width, height);
}

static int ReadBigEndianInt(byte[] bytes, int start) =>
    (bytes[start] << 24) | (bytes[start + 1] << 16) | (bytes[start + 2] << 8) | bytes[start + 3];

file sealed record FigureSpec(int Number, string Title, string Path, string Source);
