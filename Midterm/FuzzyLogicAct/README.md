# Fuzzy Logic Boss Fight

A small Windows Forms demo that demonstrates a simple MAMDANI-style fuzzy logic controller driving a boss AI in a 10x10 tile arena. The boss decides between defensive, neutral, and aggressive stances using three crisp inputs: `Boss HP`, `Distance` (Manhattan), and `Player Aggression`. The project is implemented in C# targeting .NET Framework 4.8.

## Project structure
- `FuzzyLogicAct.slnx` — Visual Studio solution
- `FuzzyLogicAct\Program.cs` — application entry
- `FuzzyLogicAct\Form1.cs` — main UI, arena, and fuzzy logic implementation
- `FuzzyLogicAct\DoubleBufferedPanel` — small helper for smoother rendering

## Features
- 10x10 tile arena with player and boss
- Player movement and attack (W/A/S/D + Space)
- Live visualization of input membership functions and Mamdani clipped output
- Tab to cycle graph modes and a small 3D surface visualization
- Simple rule set:
  1. Rule 1 (Defensive): IF `Boss HP` is Low AND `Distance` is Near. 
  2. Rule 2 (Neutral):   IF `Distance` is Mid AND `Player Aggression` is Passive. 
  3. Rule 3 (Aggressive):IF `Boss HP` is High OR `Distance` is Far.
- Centroid defuzzification produces final crisp stance (0–100)

## Controls
- W/A/S/D — Move player (4 directions)
- Space — Attack (increases `Player Aggression`; melee deals damage at distance 1)
- Tab — Cycle graph mode / swap graph axes
- The boss alternates between acting and waiting to give player reaction windows.

## How the fuzzy logic works (brief)
- Inputs mapped to triangular membership functions (triangular fuzzification).
- Rules combined using `Math.Min` for AND and `Math.Max` for OR.
- Mamdani clipping produces a combined output fuzzy set.
- Centroid (weighted average) defuzzification yields a crisp stance used for boss behavior (move towards / away, attack choices).

## Crisp Action Thresholds:
 - [ 0 - 44 ] Defensive  -> Flee or Panic Laser
 - [ 45 - 55] Neutral    -> Hold Position / Strafe
 - [ 56 - 100] Aggressive-> Hunt, Melee, or Ranged Laser

## Prerequisites
- Windows
- Visual Studio capable of opening and building `.slnx` (project targets `.NET Framework 4.8`)
- .NET Framework 4.8 developer targeting pack installed (usually included with Visual Studio workloads)

## Build & Run
1. Open the solution `FuzzyLogicAct.slnx` in Visual Studio.
2. Ensure the startup project is `FuzzyLogicAct`.
3. Build the project via the __Build Solution__ command.
4. Run with __Start Debugging__ or __Start Without Debugging__.

If you prefer CLI tools, ensure MSBuild for .NET Framework is available and run:
