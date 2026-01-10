# ARChess – Jogos de tabuleiro em Realidade Aumentada

ARChess é um projeto desenvolvido em Unity que traz o clássico jogo de damas para o universo da Realidade Aumentada (AR), utilizando o AR Foundation. O objetivo é demonstrar habilidades em desenvolvimento de jogos, programação orientada a objetos, interação com AR e boas práticas de arquitetura de código para portfólio.

## Funcionalidades
- **Tabuleiro em AR:** O tabuleiro é posicionado automaticamente sobre uma superfície plana detectada pelo dispositivo.
- **Peças Interativas:** As peças podem ser arrastadas e soltas usando toque (mobile) ou mouse (editor), com validação automática de movimentos válidos e capturas.
- **Regras de Damas:** Implementação das regras básicas de damas, incluindo movimentos diagonais, capturas e coroação de peças (dama/king).
- **Gerenciamento de Partida:** O GameManager controla o estado do tabuleiro, movimentação, capturas e reinício do jogo.
- **Interface Intuitiva:** UI simples para iniciar a experiência após a detecção de uma superfície adequada.

## Estrutura do Projeto
- **Assets/Script/**: Scripts principais do jogo (Board, Piece, GameManager, InitialSetup, StartExperience).
- **Assets/Scenes/**: Cenas do Unity.
- **Assets/Models/**: Modelos 3D das peças e tabuleiro.
- **Assets/Materials/**: Materiais e texturas.
- **Packages/**: Dependências do AR Foundation e XR Toolkit.

## Como Funciona
1. **Detecção de Superfície:** O app utiliza AR Foundation para detectar planos no ambiente real.
2. **Início da Experiência:** Após encontrar uma superfície grande o suficiente, o usuário pode iniciar a experiência.
3. **Tabuleiro em AR:** O tabuleiro é instanciado e alinhado sobre o plano detectado.
4. **Movimentação das Peças:** O usuário pode selecionar e arrastar peças. O GameManager valida e executa os movimentos conforme as regras.
5. **Captura e Coroação:** Capturas são automáticas ao pular peças adversárias. Peças que chegam ao final do tabuleiro são promovidas a dama.

## Tecnologias Utilizadas
- **Unity 2021+**
- **AR Foundation**
- **XR Interaction Toolkit**
- **C#**

## Como Rodar
1. Clone este repositório.
2. Abra a pasta no Unity (versão recomendada: 2021.3 ou superior).
3. Certifique-se de que o AR Foundation e XR Toolkit estão instalados.
4. Construa e rode no dispositivo móvel compatível com ARCore (Android) ou ARKit (iOS).

## Diferenciais Técnicos
- Código modular e bem documentado.
- Separação clara entre lógica de jogo, interação AR e UI.
- Suporte a múltiplas plataformas (Editor e Mobile).
- Fácil expansão para outras regras ou jogos de tabuleiro.

## Screenshots
(Adicione aqui imagens do app rodando no dispositivo)

## Autor
Desenvolvido por [Seu Nome].

---
Este projeto é open source e pode ser utilizado como referência para estudos ou portfólio.
# ARChess
Esse é um projeto que criei como auto desafio a partir da minha trilha de desenvolvimento AR pela NexVisual. O objetivo é criar uma sala de jogos de tabuleiro, começando pelos mais basicos como Damas, até os mais avançados como Xadrez, utilizando tudo que aprendi e além, na trilha de desenvolvimento AR
