# Alto's Adventure / Alto's Odyssey — referência de comportamento

Resumo de **como o jogo se comporta**, a partir de fontes públicas.

**Regra deste arquivo:** só entra informação que vem de
- **[dev]** declaração dos desenvolvedores (Snowman / Team Alto);
- **[Olavo]** confirmado pelo Olavo, que joga;
- **[várias fontes]** dita por mais de uma fonte independente (reviews/guias).

Palpites, números de uma fonte só e explicações sem fonte **ficam de fora**. Física
interna (altura do pulo, velocidade de giro, tolerância de ângulo, fórmulas) **não é
pública**: esses valores saem do nosso tuning.

---

## 1. Controles
- **Um toque só, na tela inteira.** O personagem avança sozinho; o jogador decide quando
  pular e quando girar. [várias fontes]
- **Toque = pula, na hora.** O pulo não carrega e não existe ação de segurar no chão.
  [Olavo][várias fontes]
- **Segurar no ar = backflip contínuo**, enquanto o dedo estiver na tela. [Olavo][várias
  fontes]
- **Odyssey:** alguns personagens têm pulo duplo (segundo toque no ar). [várias fontes]
- **Filosofia de input:** na prototipagem do Odyssey, tudo que exigia mais de um tipo de
  toque "não parecia certo"; swipes e gestos extras foram cortados, e o wallride foi
  desenhado para caber no toque único. Gancho e sistemas de manobra mais complexos foram
  rejeitados para preservar a "pureza" da experiência. [dev — Pocket Gamer]

## 2. Terreno e velocidade
- Terreno gerado proceduralmente; o trajeto muda a cada partida. [várias fontes]
- **Descer ladeira aumenta a velocidade.** [Olavo; guia iMore cita o mesmo]
- **Rampas são retas**, sobem inclinadas e terminam numa ponta (ex.: rampa de madeira na
  borda de um abismo). [Olavo]
- Abismos exigem velocidade; sem embalo, você cai neles. [várias fontes]

## 3. Rotação no ar
- A única manobra aérea ativa é o **backflip**, controlado pelo tempo de toque. Outras
  fontes de pontos (grind, quique, wallride) vêm de objetos do cenário. [várias fontes]
- O risco é não ter tempo no ar para completar a rotação e cair de cara. [várias fontes]

## 4. Aterrissagem
- **Aterrissar errado = crash = fim da corrida** (no modo normal). [várias fontes]
- **Aterrissar uma manobra dá boost temporário de velocidade** e um "escudo" que quebra
  pedras. [várias fontes, incluindo dicas atribuídas a Ryan Cash — Six Colors]
- **Não se sabe publicamente** se existe gradação de aterrissagem (ex.: "perfeita" vs
  "ok") nem qual é a tolerância de ângulo.

## 5. Pontos e combos
- **Pontos só contam se você aterrissar.** Crash perde a combo em andamento. [várias
  fontes]
- **Manobras encadeadas multiplicam a pontuação**, e o multiplicador é aplicado na
  aterrissagem. A fórmula exata não é pública. [várias fontes]
- **O combo continua enquanto você encadeia objetos** (grind, pedra, balão, parede) sem
  tocar o chão comum. [várias fontes]
- **Cachecol:** cresce enquanto você faz manobras e encolhe quando você para; funciona
  como indicador de velocidade/combo. [várias fontes]

## 6. O que acontece ao errar
- **Modo normal:** crash encerra a corrida. [várias fontes]
- **Adventure:** os "elders" perseguem o jogador; manter velocidade com manobras é como
  escapar. [várias fontes]
- **Zen Mode (2016, também no Odyssey):** sem pontuação, moedas ou power-ups; ao cair,
  você continua de onde parou; trilha sonora própria. [várias fontes — Engadget,
  MobileSyrup]
- Um lêmure que roubava moedas foi cortado do Odyssey porque perder moedas "sempre
  pareceu esmagadoramente frustrante". [dev — Pocket Gamer]

## 7. Fluidez e sensação (segundo os devs)
- O **"DNA" da série**: sensação de *flow*, **movimento suave ao longo de curvas
  parabólicas** e calma vinda da arte e da música. [dev — Ryan Cash, Pocket Gamer]
- Objetivo original: capturar o fluxo e a sensação de fazer snowboard, quando você está
  "no ritmo da montanha" e "todo o resto desaparece". [dev — via Wikipedia/Polygon]
- **"Fácil de aprender, difícil de dominar"**, com influência do Tony Hawk's Pro Skater 2.
  [dev — via Wikipedia]
- A equipe diz que **nunca tentou fazer um jogo "relaxante"**: o relaxamento surgiu da
  combinação de movimento, arte e som. [dev — Red Bull]
- Redução deliberada de complexidade visual para o jogo ser gostoso "num nível
  visceral". [dev]

---

## Fontes
- Pocket Gamer — entrevista com Ryan Cash, Eli Cymet e Harry Nesbitt (2018) — https://www.pocketgamer.com/altos-odyssey/interview-a-closer-look-at-team-altos-stunning-sequel-altos-odyssey
- Wikipedia — *Alto's Adventure* — https://en.wikipedia.org/wiki/Alto%27s_Adventure
- Red Bull — https://www.redbull.com/us-en/alto-s-adventure-snowboarding-game-gets-a-sequel
- Six Colors (2015; parte das dicas veio de Ryan Cash) — https://sixcolors.com/post/2015/02/snow-problem-tips-for-altos-adventure/
- Gotta Be Mobile (2016) — https://www.gottabemobile.com/altos-adventure-tips-tricks-cheats/
- iMore (2018) — https://www.imore.com/altos-adventure-tips-tricks-and-pointers-get-you-past-triple-backflip-and-more
- Vulcan Post (2015) — https://vulcanpost.com/191171/everything-need-know-ace-altos-adventure/
- Engadget (Zen Mode) — https://www.engadget.com/2016-06-01-altos-adventure-zen-mode.html
- MobileSyrup (Zen Mode) — https://mobilesyrup.com/2016/06/01/calming-game-altos-adventure-just-got-more-relaxing/
