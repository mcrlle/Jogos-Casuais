# Cat Runner — Design

## Pitch
Um gato de patins atravessando um subúrbio. Você só pula, e no ar gira. Aterrissou
bem, a combo cresce. Errou, paga o preço.

## Formato
- **Gênero:** auto-runner casual para mobile.
- **Orientação:** horizontal (landscape).
- **Câmera:** visão lateral 2D.
- **Personagem:** um gato de patins.
- **Cenário:** subúrbio de cidade (ruas em ladeira, calçadas, rampas, telhados baixos,
  cercas, carros parados: o equivalente urbano das encostas do Alto).

## Vibe
Meio-termo entre runner frenético e relaxado. O mesmo jogo serve para dois modos de
jogar, sem mudar as regras:
- **Para relaxar:** pular, deslizar, curtir o ritmo. Sem girar, o risco é baixo.
- **Para bater recorde:** girar mais, encadear combos, arriscar aterrissagens apertadas.

O jogador escolhe a intensidade **pela quantidade de risco que assume**, não por um
menu de dificuldade.

## Emoção-alvo
**Satisfação.** O momento-chave é a aterrissagem limpa depois de um giro arriscado:
o "encaixe" do patins no chão, a combo fechando, a velocidade subindo.
Tudo que reforça esse encaixe (timing, som, câmera, feedback de velocidade) é
prioridade. Tudo que dilui esse momento é corte.

## Verbo central: pular
É **um núcleo de mecânica só**, sem sistemas secundários.

- **Toque:** pula.
- **Segurar no ar:** gira.
- **Soltar:** para de girar.
- **Aterrissagem:** checada pelo **ângulo do gato em relação ao chão** naquele ponto.
  O terreno é inclinado, então o ângulo certo muda conforme a ladeira.
- **Pontos:** cada rotação completa vale pontos.
- **Combo:** manobras encadeadas sem erro acumulam multiplicador.
- **O pulo não carrega:** tocou, pulou. A altura do pulo é sempre a mesma.

### Velocidade
Existe uma **velocidade base de equilíbrio**. Duas coisas somam velocidade a ela, e as
duas são **temporárias**, voltando ao equilíbrio com o tempo:
- **Descida:** descer ladeira acelera o gato. Depois a velocidade volta à base **aos
  poucos**.
- **Aterrissar manobras:** aterrissar com rotação completa dá boost. O boost é **bem
  temporário** e volta mais rápido.

Os valores (aceleração na descida, tamanho do boost, velocidade de retorno de cada um,
efeito da subida, mínimo e máximo) são tuning.

### Terreno
O terreno é parte da mecânica, não cenário. A inclinação define **o ângulo certo de
aterrissagem** naquele ponto e mexe na velocidade (descida acelera). Terreno com encostas
existe desde o primeiro protótipo.

**Como o terreno é feito (decidido):**
- **Malha gerada por código** (não Sprite Shape).
- **Calçada na frente:** é onde o gato anda e vai da superfície até embaixo da tela.
  **Rua atrás**, seguindo a mesma ladeira. Sem "corte de terra" embaixo.
- A arte do chão são **texturas tileable** (um pedaço que emenda nele mesmo),
  repetidas ao longo da ladeira pela distância percorrida, sem esticar.
- Detalhes (bueiro, rachadura etc.) e objetos (casas, postes) são peças avulsas
  espalhadas por código; objetos ficam sempre em pé.

## Em aberto (TBD — não decidir ainda)
- **TBD — Consequência da aterrissagem ruim:** o gato cai (fim da corrida ou queda
  com recuperação) **ou** só quebra a combo ("gato cai de pé")?
- **TBD — Grind:** vai existir grind em superfícies da cidade (corrimão, meio-fio,
  fio de poste)?

Enquanto estiverem em aberto, o código deve permitir trocar entre as opções sem
reescrever o controller.

## O que deve ser diferente do Alto
| Alto | Cat Runner |
|---|---|
| Montanha, natureza, snowboard | Subúrbio urbano, patins |
| Ritmo assumidamente calmo, "zen" | Meio-termo: relaxado **ou** competitivo, conforme o risco que o jogador assume |
| Emoção central: calma, flow | Emoção central: **satisfação** do encaixe |
| Vários sistemas em volta do núcleo: moedas, workshop, power-ups (ímã, wingsuit), perseguição dos elders, objetivos por nível, personagens desbloqueáveis | **Um núcleo só**: pular, girar, aterrissar, pontuar. Sem sistemas secundários |
| Objetos de combo variados (pedras, balões, bandeirolas, paredes, tornados) | Combo baseada no pulo/giro; grind urbano está em aberto (TBD) |
| Aterrissagem errada sempre derruba | Em aberto (TBD): pode só quebrar a combo |
| Identidade visual e nomes do Alto | Nada de arte, personagens, música ou nomes do Alto. A referência é de **sensação e regra**, não de aparência |

## Fora de escopo (por enquanto)
Arte final, menu, progressão, loja, monetização, moedas, power-ups, missões.
