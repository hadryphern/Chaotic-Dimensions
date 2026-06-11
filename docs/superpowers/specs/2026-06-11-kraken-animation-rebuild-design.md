# Reconstrução da Animação do Alien Kraken

## Objetivo

Reconstruir os quatro atlas visuais do Alien Kraken para que a cabeça, o tronco e os tentáculos pareçam pertencer ao mesmo corpo. A nova animação deve preservar o desenho, as cores e a Ruby existentes, mas eliminar contornos inconsistentes, partes soltas, mudanças súbitas de anatomia e movimentos que parecem recortes deslocados.

## Problemas confirmados

1. A cabeça possui lineart escuro e detalhes internos, enquanto grande parte do tronco e dos tentáculos usa apenas massas de cor.
2. Os frames atuais não deformam uma anatomia estável. Alguns tentáculos centrais mudam de raiz, comprimento, posição e forma entre frames.
3. O movimento dos tentáculos grandes troca silhuetas inteiras em vez de propagar uma onda da base até à ponta.
4. O aumento de escala no jogo torna as diferenças de acabamento e as bordas irregulares mais visíveis.
5. As posições da Ruby dependem de correções manuais por frame, o que torna futuras alterações frágeis.

## Abordagens consideradas

### A. Limpeza dos frames atuais

Corrigir manualmente os 36 frames existentes, mantendo cada silhueta. Seria a alteração mais pequena, mas conservaria diferenças anatómicas e exigiria correções repetidas em quatro atlas.

### B. Deformação contínua das camadas originais

Usar as camadas separadas como componentes visuais, fixar as raízes dos tentáculos e gerar todas as poses através de curvas contínuas. Esta é a abordagem escolhida porque preserva a identidade do desenho e resolve a causa dos saltos.

### C. Redesenho integral

Reconstruir o Kraken com novo lineart, novos volumes e nova anatomia. Teria maior liberdade artística, mas alteraria demasiado o desenho já aprovado.

## Solução escolhida

### Anatomia estável

- A cabeça e o tronco formam a peça central.
- Cada tentáculo recebe uma identificação permanente e uma raiz fixa.
- O número de tentáculos não muda entre frames.
- Nenhuma raiz pode deslocar-se para cima, atravessar o tronco ou desaparecer.
- Os tentáculos centrais ficam sobrepostos à junção entre o tronco e os tentáculos maiores, ocultando a ligação sem parecerem peças flutuantes.

### Movimento

Cada tentáculo é deformado ao longo de uma curva suave. A deslocação é mínima junto à raiz e aumenta progressivamente até à ponta. O ciclo usa funções periódicas, garantindo que o primeiro e o último frame fecham sem salto.

O idle terá:

- oscilação vertical discreta do corpo;
- compressão e expansão leve do tronco;
- movimento amplo dos tentáculos maiores;
- movimento secundário mais rápido, mas controlado, dos tentáculos centrais;
- fases diferentes por tentáculo para evitar simetria mecânica;
- atraso visual nas pontas, dando sensação de peso e fluidez.

O movimento horizontal terá:

- corpo inclinado de forma moderada na direção do deslocamento;
- raízes estáveis;
- tentáculos arrastados para o lado oposto à velocidade;
- retorno gradual ao idle quando a velocidade horizontal diminuir.

O movimento vertical terá:

- tentáculos arrastados para baixo durante a subida;
- leve compressão do tronco;
- recuperação elástica quando a subida terminar.

O atlas de retorno fechará a transição para o idle sem inverter frames de forma artificial.

### Contorno e acabamento

- Todo o corpo recebe um contorno azul-marinho uniforme.
- O contorno não será preto puro.
- A espessura visual será equivalente em cabeça, tronco e tentáculos.
- O lineart interno da cabeça será mantido, mas harmonizado com o contorno exterior.
- As bordas serão produzidas em resolução de trabalho superior e reduzidas com filtragem de alta qualidade.
- Não será aplicado brilho ou aura diretamente no atlas.
- A transparência deve permanecer limpa, sem franjas brancas, azuis ou semitransparentes.

### Ruby

- A Ruby permanece numa camada visual independente.
- A sua âncora será definida relativamente à peça central, não por 36 correções independentes.
- Os estados normal, desligado e partido continuam suportados.
- Durante dash, invisibilidade ou ataques especiais, a lógica atual poderá desenhar apenas a Ruby sem depender da deformação dos tentáculos.

## Estrutura técnica

### Fonte artística

As camadas de `C:\Users\unknown\Downloads\novo-kraken` serão copiadas para uma área de produção dentro do repositório. Os atlas existentes não serão usados como fonte, porque já contêm as deformações defeituosas.

### Gerador offline

Será criado um gerador offline em Python com Pillow e NumPy. O gerador terá:

- definição explícita das raízes e dos eixos de cada tentáculo;
- deformação contínua por curva e envelope;
- composição de camadas em ordem previsível;
- aplicação de contorno uniforme;
- normalização da caixa de desenho e do ponto de origem;
- geração dos quatro atlas 6 por 6;
- geração de GIFs e folhas de comparação;
- validações automáticas de dimensão, transparência e continuidade.

O gerador será determinístico: executar novamente com as mesmas entradas deve produzir os mesmos ficheiros.

### Integração no tModLoader

`KrakenBoss.cs` continuará responsável por selecionar idle, deslocamento horizontal e subida. A integração será simplificada para usar:

- dimensões e enquadramento idênticos nos quatro atlas;
- uma âncora comum;
- Ruby ligada à âncora da cabeça;
- transição suave entre estados visuais;
- filtragem de desenho adequada ao estilo não pixelizado do boss.

A lógica de combate, vida, ataques, fases e colisões não será alterada nesta reconstrução.

## Ficheiros previstos

### Novos

- `Assets/Source/Kraken/` para as camadas de produção.
- `Tools/KrakenAnimation/generate_kraken_atlases.py` para gerar os atlas.
- `Tools/KrakenAnimation/kraken_rig.json` para raízes, ordem das camadas e parâmetros.
- `Tools/KrakenAnimation/validate_kraken_atlases.py` para validações.
- `Tools/KrakenAnimation/output/` apenas para pré-visualizações ignoradas pelo Git.

### Modificados

- `Content/NPCs/Kraken/KrakenBoss.png`
- `Content/NPCs/Kraken/KrakenBossLoopBack.png`
- `Content/NPCs/Kraken/KrakenBossMoveHorizontal.png`
- `Content/NPCs/Kraken/KrakenBossMoveUp.png`
- `Content/NPCs/Kraken/KrakenBoss.cs`
- relatório PAP, depois da validação no jogo.

## Critérios de aceitação

1. Os quatro atlas têm 36 frames, grade 6 por 6 e dimensões idênticas.
2. O primeiro e o último frame do idle fecham visualmente sem salto.
3. Cada tentáculo mantém a mesma raiz e identidade em todos os frames.
4. Nenhum frame contém componentes separados sem intenção.
5. A Ruby permanece estável no centro da cabeça.
6. O contorno exterior é consistente em toda a silhueta.
7. Não existem franjas claras no canal alfa.
8. As animações horizontal e vertical reagem na direção correta.
9. As transições entre atlas não provocam mudança visível de escala ou enquadramento.
10. O mod compila e o boss pode completar idle, perseguição, subida, dash e ataques especiais sem erro.
11. A comparação em escala real do jogo mostra bordas limpas e detalhes legíveis.
12. O relatório PAP descreve a causa, a solução e a verificação em português de Portugal.

## Verificação

- Teste automático das dimensões e do número de frames.
- Comparação do canal alfa entre frames consecutivos para detetar saltos extremos.
- Verificação das posições das raízes dos tentáculos.
- GIF de cada atlas a velocidade de jogo.
- folha com frames 1, 7, 13, 19, 25, 31 e 36.
- compilação do mod com a versão instalada do tModLoader.
- teste visual no jogo em idle, deslocamento horizontal, subida e transições.
- captura antes/depois para o relatório PAP.

## Fora do âmbito

- alterar ataques, dificuldade, vida ou defesa;
- redesenhar a Ruby;
- criar novos tentáculos;
- mudar a escala atual do boss;
- alterar o fundo cósmico;
- refazer minions, clones ou projéteis.
