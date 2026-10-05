# ARChess - Augmented Reality Board Games

ARChess is a Unity project that brings checkers (draughts) to Augmented Reality using AR Foundation. It started as a self-challenge from my AR development track at NexVisual, with the long-term goal of building an AR board game room, starting with simple games like checkers and moving toward more advanced ones like chess. The project is a portfolio piece that shows game programming, AR interaction and code organization.

## Features

- **AR board placement:** the app detects horizontal planes with AR Foundation and, once a plane is large enough (configurable `requiredArea`), shows a button to start the experience. The board is spawned at the center of the largest detected plane, aligned to its normal.
- **Interactive pieces:** pieces are picked up and dragged with touch (Input System `Touchscreen`) or the mouse (Editor/Standalone), then dropped on the nearest board cell.
- **Checkers rules (basic):** diagonal one-step moves (forward only for regular pieces), single-jump captures over an opponent piece, destination must be empty, and promotion to king when a piece reaches the last row. Invalid moves snap the piece back.
- **Match management:** `GameManager` spawns 12 white and 12 black pieces on an 8x8 grid, keeps the board state, removes captured pieces and can reset the game.
- **Custom 3D assets:** board and piece prefabs/materials are included in the project.

Not implemented yet: turn order, mandatory captures, multi-jump chains and win detection.

## Architecture

| Path | Description |
| --- | --- |
| `Assets/Script/` | Game scripts: `Board`, `Piece`, `GameManager`, `InitialSetup`, `StartExperience` |
| `Assets/Scenes/` | Unity scenes (`Main`, `MainScene`) |
| `Assets/Prefab/` | `boardPrefab`, `WhitePiece`, `BlackPiece` |
| `Assets/Materials/` | White and black piece materials |
| `Assets/Models/ARBoard/` | Board model and tile textures |
| `Assets/MobileARTemplateAssets/` | Unity Mobile AR template assets (UI prompts, plane visualizer, shaders) |
| `Packages/` | Package manifest (AR Foundation, ARCore, ARKit, XR Interaction Toolkit, URP) |
| `ProjectSettings/` | Unity project settings |

Main scripts:

- `InitialSetup`: listens to `ARPlaneManager.planesChanged`, enables the start UI when a plane meets the required area and picks the biggest plane.
- `StartExperience`: instantiates the board prefab on the chosen plane and notifies `GameManager`.
- `Board`: 8x8 grid (`cellSize` 0.05 m) with `GetCellCenter(x, y)` to convert cells to world positions.
- `Piece`: team (`White`/`Black`), king flag, touch/mouse selection via raycast, dragging and snapping to the nearest cell.
- `GameManager`: piece spawning, board state, move validation (`TryMovePiece`/`IsValidMove`), capture, crowning and `ResetGame`.

## Tech stack

![Unity](https://img.shields.io/badge/Unity_2022.3-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white)
![AR Foundation](https://img.shields.io/badge/AR_Foundation_5.2-000000?style=for-the-badge&logo=unity&logoColor=white)
![ARCore](https://img.shields.io/badge/ARCore-4285F4?style=for-the-badge&logo=google&logoColor=white)
![ARKit](https://img.shields.io/badge/ARKit-000000?style=for-the-badge&logo=apple&logoColor=white)

Also: XR Interaction Toolkit 3.1.2, Input System 1.14.0 and Universal Render Pipeline 14.0.12.

## Running the project

Requirements:

- Unity 2022.3.62f3 (the version the project was created with)
- Android device with ARCore support (minimum SDK 30) or iOS device with ARKit (iOS 12.0+ target in project settings)

Steps:

1. Clone the repository: `git clone https://github.com/Luan-Aiezza/ARChess.git`
2. Open the folder in Unity Hub with Unity 2022.3.62f3 and let it resolve the packages.
3. Open a scene from `Assets/Scenes/`.
4. In Build Settings, select Android or iOS and build/run on a compatible device.
5. Point the device at a flat surface; when the start button appears, tap it to place the board and play.

## Screenshots

<img width="819" height="430" alt="ARChess screenshot" src="https://github.com/user-attachments/assets/eeaa3da9-33b6-4b5f-b8d5-8f378fc5f505" />

## Author

Developed by [Luan Gabriel Fernandes Aiezza](https://github.com/Luan-Aiezza). The 3D models were also made by me.
