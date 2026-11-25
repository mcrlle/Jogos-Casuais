# Cat Runner

Jogo novo do Olavo (elusive fox). Iniciado em 2026-11-09.
Auto-runner casual mobile em **Unity (C#)**, landscape, visão lateral 2D: um gato de
patins atravessando um subúrbio.

## Antes de implementar qualquer mecânica
**Leia `ALTO_REFERENCE.md` e `DESIGN.md` antes de escrever ou alterar código de
mecânica.** Eles são a fonte de verdade:
- `ALTO_REFERENCE.md`: como o Alto's Adventure/Odyssey se comporta (referência de
  sensação e regra, nunca de arte ou nomes).
- `DESIGN.md`: o que é o Cat Runner, o que está decidido, o que está em aberto (TBD) e o
  que deve ser diferente do Alto.

Se uma tarefa entrar em conflito com esses arquivos, aponte o conflito antes de agir.
Não decida itens marcados como TBD: implemente de forma que dê para trocar a opção depois.

## Regra: um núcleo de mecânica só
O jogo tem **um núcleo**: pular → girar no ar → aterrissar pelo ângulo em relação ao
chão → pontuar e fazer combo. O terreno inclinado faz parte desse núcleo.
- **Não** adicionar sistemas secundários (moedas, power-ups, loja, missões, inimigos,
  perseguição, upgrades, personagens desbloqueáveis) sem o Olavo pedir explicitamente.
- Toda feature nova tem que reforçar o núcleo ou não entra.
- Na dúvida, pergunte.

## Objetivo atual
Protótipo greybox do movimento: formas simples, sem arte, menu ou progressão.
Todos os valores de tuning ficam num ScriptableObject para ajustar no Play Mode.
Controles funcionando com toque **e** com mouse/teclado (para testar no editor).

## Contexto do dev
Olavo é game/narrative designer, não programador de carreira. Programa com assistência
de IA e considera isso um diferencial, não algo a esconder. Conhece Unity do Junior
Programmer Pathway e do Lost in the Woods (ação top-down 2D).

## Estado
- [x] Projeto Unity criado: Unity **6000.3.28f1 (6.3 LTS)**, URP 2D, Input System,
      com Android Build Support (+ iOS). Migrado da 6000.3.1f1, que tinha bug marcando
      pacotes válidos como "assinatura inválida".
- [x] Orientação travada em landscape (rotação automática só entre landscape
      esquerda/direita; retrato desligado no ProjectSettings).
- Celular de teste do Olavo: Android (Samsung), modo desenvolvedor ativado.
  Parte da equipe usa iPhone: build iOS exige Mac + Xcode.
- [x] Projeto movido de `C:\Users\olavo_9bp7hdh\Cat Runner` para `D:\GitHub\cat-runner`
      (C: estava cheio). `Library/` e `Temp/` não vieram; o Unity regenera.
- [x] Unity Hub aponta para `D:\GitHub\cat-runner`
- [x] `git init` feito
- [ ] Remote do GitHub (o Olavo vai criar)
- [ ] Controller greybox (etapa 4). Os scripts em `Assets/Scripts/` (`CatBootstrap`,
      `CatRunner`, `CatTerrain`) são um mini-protótipo anterior que **não segue** o
      DESIGN.md e podem ser reescritos à vontade.

## Regra de trabalho
Pedir autorização e apresentar o plano antes de alterar arquivos.
