# TicTacToeMinimax

A classic Windows Forms Tic-Tac-Toe game featuring an unbeatable AI opponent powered by the **Minimax algorithm with Alpha-Beta pruning**. Developed as an interactive desktop project targeting **.NET Framework 4.8**.

---

## Features

* **Adversarial AI Opponent**: Play as **X** against an AI (**O**) that makes optimal decisions using Minimax search enhanced by Alpha-Beta pruning.
* **Interactive 3x3 Grid**: Responsive button-based board with automatic cell locking once marked.
* **Dynamic Status Updates**: Real-time turn notifications, victory highlights, and game-over state banners (Win, Loss, Draw).
* **Instant Reset**: Reset the board and internal game state without restarting the executable.

---

## Technical Details

* **Board State**: 1D array representation (`char[] board`) mapping cells indices `0` through `8`.
* **Player Definitions**:
* `HUMAN = 'X'`
* `AI = 'O'`
* `EMPTY = ' '`


* **Move Decision Engine**: `FindBestMove()` recursively calls `MinimaxAlphaBeta(...)` to prune redundant decision branches while finding the optimal utility score.
* **Depth-Weighted Utility Scoring**:
* **AI Win**: `+10 - depth` (incentivizes fast victories)
* **Human Win**: `depth - 10` (incentivizes prolonged defense)
* **Draw / Tie**: `0`


* **UI Responsiveness**: Keeps the interface reactive during recursive calculations using `Application.DoEvents()`.

---

## Prerequisites

* **Operating System**: Windows 10 / 11
* **IDE**: Microsoft Visual Studio (Community, Professional, or Enterprise) with the **.NET desktop development** workload
* **Target Runtime**: .NET Framework 4.8 Developer Pack / Runtime

---

## Project Structure

```text
TicTacToeMinimax/
├── Properties/
│   └── AssemblyInfo.cs       # Assembly metadata
├── Form1.cs                  # Board logic, Minimax algorithm, and click handlers
├── Form1.Designer.cs         # Form layout and control declarations
├── Form1.resx                # UI resources
├── Program.cs                # Application entry point
├── App.config                # Runtime configuration
├── TicTacToeMinimax.csproj   # Project file (.NET Framework 4.8)
└── TicTacToeMinimax.slnx     # Visual Studio Solution file

```

---

## Build and Run

1. **Open the Solution**: Double-click `TicTacToeMinimax.slnx` (or open it within Visual Studio).
2. **Build the Solution**: Press `Ctrl + Shift + B` (or navigate to **Build > Build Solution**).
3. **Launch the Game**: Press `F5` to debug or `Ctrl + F5` to run directly.

---

## How to Play

1. Click any available empty button to place your mark (**X**).
2. The AI will evaluate optimal paths, mark its move (**O**), and pass the turn back.
3. Once three matching marks align horizontally, vertically, or diagonally—or the board fills—the status label will declare the final outcome.
4. Click **Reset** at any time to clear the board and start a new match.