using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
Gameplay Mechanics:
    - Player Action: Navigates orthogonally (WASD) and attacks (Spacebar).
    - Boss Pacing: Operates on a turn-based cadence (Act -> Wait -> Act) to provide the player with a reaction window.
    - Boss Combat: Movement is constrained to 1 tile per turn. Ranged attacks utilize an "Empress" pattern (orthogonal lines + Knight L-shapes).

Fuzzy Inference System (Mamdani Model):
    The boss's decision-making engine is driven by a 3-input Mamdani fuzzy logic controller, 
    adapting foundational fuzzy implementation concepts discussed by Sir Alliac.

    1. Crisp Inputs: 
       Boss HP, Manhattan Distance, and Player Aggression.
    
    2. Fuzzification: 
       Inputs are mapped to overlapping fuzzy sets using Triangular Membership functions. 
       (Note: Boss HP triangles cross completely at 50 to prevent logic dead-zones).
    
    3. Rule Evaluation:
       - Rule 1 (Defensive): IF Boss HP is Low AND Distance is Near. 
       - Rule 2 (Neutral):   IF Distance is Mid AND Player Aggression is Passive. 
       - Rule 3 (Aggressive):IF Boss HP is High OR Distance is Far. 
    
    4. Defuzzification (Centroid): 
       Evaluated rule strengths clip their respective output triangles. These are aggregated, 
       and a final crisp behavioral stance (0-100) is calculated via discrete Riemann sum integration.

    5. Crisp Action Thresholds:
       [ 0 - 44 ] Defensive  -> Flee or Panic Laser
       [ 45 - 55] Neutral    -> Hold Position / Strafe
       [ 56 - 100] Aggressive-> Hunt, Melee, or Ranged Laser

====================================================================
RULE MATRIX TABLE
====================================================================
Rule | Boss HP | Logic | Distance | Logic | Player Aggression | Output Stance | Math Operator
---------------------------------------------------------------------------------------------
  1  | Low     | AND   | Near     |   -   | (Any)             | Defensive     | Math.Min
  2  | (Any)   |   -   | Mid      | AND   | Passive           | Neutral       | Math.Min
  3  | High    | OR    | Far      |   -   | (Any)             | Aggressive    | Math.Max
====================================================================
*/

namespace FuzzyLogicAct
{
    public partial class Form1 : Form
    {
        int playerX = 0, playerY = 0;
        int bossX = 9, bossY = 9;
        int player_dmg = 5, boss_melee_dmg = 15, boss_ranged_dmg = 10;
        double playerHP = 100, bossHP = 100;
        double playerAggression = 50;
        bool bossIsWaiting = false;

        double rule1_strength = 0;
        double rule2_strength = 0;
        double rule3_strength = 0;

        // 0 = Dist/BossHP | 1 = Dist/Aggression | 2 = BossHP/Aggression
        int graphMode = 0;

        Panel[,] gridPanels = new Panel[10, 10];

        public Form1()
        {
            InitializeComponent();
            this.Text = "Fuzzy Logic Boss Fight";
            this.DoubleBuffered = true;

            SetupArena();
            UpdateUI("PLAYER MOVES FIRST.");
        }

        //UI & INPUT HANDLING
        private void SetupArena()
        {
            int availableWidth = pnlArena.ClientSize.Width;
            int availableHeight = pnlArena.ClientSize.Height;

            int tileSize = Math.Min(availableWidth, availableHeight) / 10;
            pnlArena.ClientSize = new Size(tileSize * 10, tileSize * 10);

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Panel p = new Panel();
                    p.Bounds = new Rectangle(x * tileSize, y * tileSize, tileSize - 1, tileSize - 1);
                    p.BackColor = Color.LightGray;

                    pnlArena.Controls.Add(p);
                    gridPanels[x, y] = p;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Toggle Graph Mode on Tab
            if (keyData == Keys.Tab)
            {
                graphMode = (graphMode + 1) % 3; // Cycles 0, 1, 2
                pnl3DSurface.Invalidate();       // Force instant redraw
                return true;
            }

            if (playerHP <= 0 || bossHP <= 0) return base.ProcessCmdKey(ref msg, keyData);

            double oldDistance = GetManhattanDistance(playerX, playerY, bossX, bossY);
            bool playerAttacked = false;
            bool validMove = false;

            if (keyData == Keys.W && playerY > 0) { playerY--; validMove = true; }
            else if (keyData == Keys.S && playerY < 9) { playerY++; validMove = true; }
            else if (keyData == Keys.A && playerX > 0) { playerX--; validMove = true; }
            else if (keyData == Keys.D && playerX < 9) { playerX++; validMove = true; }
            else if (keyData == Keys.Space)
            {
                playerAttacked = true;
                validMove = true;
                playerAggression += 20;

                if (oldDistance == 1) bossHP -= player_dmg;
            }

            if (validMove)
            {
                ExecuteBossTurn(playerAttacked, oldDistance);
                return true; // Input handled
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //FUZZY LOGIC CORE. [THIS IS WHERE THE MAGIC HAPPENS]

        private void ExecuteBossTurn(bool playerAttacked, double oldDistance)
        {
            double newDistance = GetManhattanDistance(playerX, playerY, bossX, bossY);

            // 1. Crisp Aggression Tracking
            if (!playerAttacked)
            {
                if (newDistance < oldDistance) playerAggression += 15;
                else if (newDistance > oldDistance) playerAggression -= 15;
                else playerAggression = CalculateDecay(playerAggression, 50, 5);
            }
            playerAggression = Math.Max(0, Math.Min(100, playerAggression));

            double crispStance = CalculateFuzzyStance(newDistance, bossHP, playerAggression);

            // 5. Boss Execution & Wait Pacing
            string bossAction = "";

            if (bossIsWaiting)
            {
                bossAction = "Evaluating... (Waiting Turn)";
                bossIsWaiting = false;
            }
            else
            {
                int pdx = Math.Abs(playerX - bossX);
                int pdy = Math.Abs(playerY - bossY);

                bool canRangedAttack = (playerX == bossX || playerY == bossY) ||
                                       ((pdx == 2 && pdy == 1) || (pdx == 1 && pdy == 2));

                if (crispStance < 45)
                {
                    // Defensive Stance
                    if (canRangedAttack)
                    {
                        bossAction = "Defensive (PANIC LASER!)";
                        playerHP -= boss_ranged_dmg;
                    }
                    else
                    {
                        bossAction = "Defensive (Moved Away)";
                        MoveBoss(false);
                    }
                }
                else if (crispStance >= 45 && crispStance <= 55)
                {
                    bossAction = "Neutral (Positioning)";
                    MoveBoss(true);
                }
                else
                {
                    // Aggressive Stance
                    if (GetManhattanDistance(playerX, playerY, bossX, bossY) == 1)
                    {
                        bossAction = "Aggressive (MELEE ATTACK!)";
                        playerHP -= boss_melee_dmg;
                    }
                    else if (canRangedAttack)
                    {
                        bossAction = "Aggressive (RANGED LASER!)";
                        playerHP -= boss_ranged_dmg;
                    }
                    else
                    {
                        bossAction = "Aggressive (Moved Towards)";
                        MoveBoss(true);
                    }
                }
                bossIsWaiting = true;
            }

            // Generate Debug String
            string debugTxt = $"Distance: {newDistance} | BossHP: {bossHP} | Player Aggression: {playerAggression}\n";
            debugTxt += $"Rules -> Def: {rule1_strength:F2}, Neu: {rule2_strength:F2}, Agg: {rule3_strength:F2}\n";
            debugTxt += $"Centroid Stance: {crispStance:F2}/100\n";
            debugTxt += $"Action Taken: {bossAction}";

            if (playerHP <= 0) debugTxt += "\n\n*** YOU DIED ***";
            else if (bossHP <= 0) debugTxt += "\n\n*** BOSS DEFEATED ***";

            UpdateUI(debugTxt);
            pnl3DSurface.Invalidate();
            pnlCurves.Invalidate();
        }

        private double CalculateFuzzyStance(double dist, double bHP, double pAgg)
        {
            // Fuzzification
            double distNear = TriangularMembership(dist, 0, 0, 4);
            double distMid = TriangularMembership(dist, 2, 5, 8);
            double distFar = TriangularMembership(dist, 6, 18, 18);

            double bossHpLow = TriangularMembership(bHP, 0, 0, 50);
            double bossHpHigh = TriangularMembership(bHP, 50, 100, 100);

            double aggPassive = TriangularMembership(pAgg, 0, 0, 50);
            double aggAggressive = TriangularMembership(pAgg, 50, 100, 100);

            // Rule Evaluation
            rule1_strength = Math.Min(bossHpLow, distNear);
            rule2_strength = Math.Min(distMid, aggPassive);
            rule3_strength = Math.Max(bossHpHigh, distFar);

            // Defuzzification
            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 2.0; // Slightly larger step for faster rendering

            for (double y = 0.0; y <= 100.0; y += step)
            {
                double outDefensive = TriangularMembership(y, 0, 0, 40);
                double outNeutral = TriangularMembership(y, 30, 50, 70);
                double outAggressive = TriangularMembership(y, 60, 100, 100);

                double clippedDef = Math.Min(rule1_strength, outDefensive);
                double clippedNeu = Math.Min(rule2_strength, outNeutral);
                double clippedAgg = Math.Min(rule3_strength, outAggressive);

                double aggregatedY = Math.Max(clippedDef, Math.Max(clippedNeu, clippedAgg));

                sumNumerator += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            return (sumDenominator > 0.0) ? (sumNumerator / sumDenominator) : 50.0;
        }

        // --- HELPERS & GRAPHICS ---

        private void UpdateUI(string debugText)
        {
            lblPlayerStats.Text = $"Player HP: {playerHP}\nMelee DMG: {player_dmg}";
            lblBossStats.Text = $"Boss HP: {bossHP}\nMelee DMG: {boss_melee_dmg}\n Laser DMG: {boss_ranged_dmg}";
            lblFuzzyDebug.Text = debugText;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    int dx = Math.Abs(x - bossX);
                    int dy = Math.Abs(y - bossY);

                    //Straight lines and Knight L-shapes only
                    bool isStraight = (x == bossX || y == bossY);
                    bool isLShape = ((dx == 2 && dy == 1) || (dx == 1 && dy == 2));

                    if (isStraight || isLShape)
                    {
                        gridPanels[x, y].BackColor = Color.LightCoral;
                    }
                    else
                    {
                        gridPanels[x, y].BackColor = Color.LightGray;
                    }

                    //Draw Entities on top
                    if (x == playerX && y == playerY)
                        gridPanels[x, y].BackColor = Color.DodgerBlue;
                    else if (x == bossX && y == bossY)
                        gridPanels[x, y].BackColor = Color.Crimson;
                }
            }
        }

        private void pnl3DSurface_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(18, 22, 28));

            int gridSteps = 16;
            PointF[,] projected = new PointF[gridSteps + 1, gridSteps + 1];
            double[,] zValues = new double[gridSteps + 1, gridSteps + 1];

            float originX = pnl3DSurface.Width / 2f;
            float originY = pnl3DSurface.Height * 0.70f;
            float scaleX = 15.5f;
            float scaleY = 8.5f;
            float scaleZ = 2.2f;

            double actualDistance = GetManhattanDistance(playerX, playerY, bossX, bossY);
            string xAxisLabel = "";
            string yAxisLabel = "";
            float curGridX = 0;
            float curGridY = 0;

            // Generate grid points and map the axes based on the current mode
            for (int i = 0; i <= gridSteps; i++)
            {
                double simX = (double)i / gridSteps;
                for (int j = 0; j <= gridSteps; j++)
                {
                    double simY = (double)j / gridSteps;
                    double z = 50.0;

                    if (graphMode == 0) // X: Distance, Y: Boss HP, Live: Aggression
                    {
                        z = CalculateFuzzyStance(simX * 18.0, simY * 100.0, playerAggression);
                        if (i == 0 && j == 0)
                        {
                            xAxisLabel = "Distance [0 -> 18] ->";
                            yAxisLabel = "<- Boss HP [100% <- 0%]";
                            curGridX = (float)(actualDistance / 18.0 * gridSteps) - (gridSteps / 2f);
                            curGridY = (float)(bossHP / 100.0 * gridSteps) - (gridSteps / 2f);
                        }
                    }
                    else if (graphMode == 1) // X: Distance, Y: Aggression, Live: Boss HP
                    {
                        z = CalculateFuzzyStance(simX * 18.0, bossHP, simY * 100.0);
                        if (i == 0 && j == 0)
                        {
                            xAxisLabel = "Distance [0 -> 18] ->";
                            yAxisLabel = "<- Aggression [100% <- 0%]";
                            curGridX = (float)(actualDistance / 18.0 * gridSteps) - (gridSteps / 2f);
                            curGridY = (float)(playerAggression / 100.0 * gridSteps) - (gridSteps / 2f);
                        }
                    }
                    else // X: Boss HP, Y: Aggression, Live: Distance
                    {
                        z = CalculateFuzzyStance(actualDistance, simX * 100.0, simY * 100.0);
                        if (i == 0 && j == 0)
                        {
                            xAxisLabel = "Boss HP [0% -> 100%] ->";
                            yAxisLabel = "<- Aggression [100% <- 0%]";
                            curGridX = (float)(bossHP / 100.0 * gridSteps) - (gridSteps / 2f);
                            curGridY = (float)(playerAggression / 100.0 * gridSteps) - (gridSteps / 2f);
                        }
                    }

                    zValues[i, j] = z;
                    float gridX = i - (gridSteps / 2f);
                    float gridY = j - (gridSteps / 2f);
                    projected[i, j] = new PointF(originX + (gridX - gridY) * scaleX, originY + (gridX + gridY) * scaleY - (float)(z * scaleZ));
                }
            }

            // Draw Wireframe Mesh
            for (int i = 0; i < gridSteps; i++)
            {
                for (int j = 0; j < gridSteps; j++)
                {
                    PointF[] quad = { projected[i, j], projected[i + 1, j], projected[i + 1, j + 1], projected[i, j + 1] };
                    double avgZ = (zValues[i, j] + zValues[i + 1, j] + zValues[i + 1, j + 1] + zValues[i, j + 1]) / 4.0;
                    using (var brush = new SolidBrush(Get3DHeightColor(avgZ))) { g.FillPolygon(brush, quad); }
                    using (var pen = new Pen(Color.FromArgb(45, 255, 255, 255), 1)) { g.DrawPolygon(pen, quad); }
                }
            }

            // Dynamic Operating Point Beacon
            double actualZ = CalculateFuzzyStance(actualDistance, bossHP, playerAggression);
            float beaconSx = originX + (curGridX - curGridY) * scaleX;
            float beaconFloorSy = originY + (curGridX + curGridY) * scaleY;
            float beaconSy = beaconFloorSy - (float)(actualZ * scaleZ);

            using (var linePen = new Pen(Color.Yellow, 1.5f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
            {
                g.DrawLine(linePen, beaconSx, beaconFloorSy, beaconSx, beaconSy);
            }
            g.FillEllipse(Brushes.Yellow, beaconSx - 6, beaconSy - 6, 12, 12);

            // Axis & UI Labels
            g.DrawString(xAxisLabel, new Font("Segoe UI", 8.5f, FontStyle.Bold), Brushes.LightSkyBlue, projected[gridSteps, 0].X - 60, projected[gridSteps, 0].Y + 12);
            g.DrawString(yAxisLabel, new Font("Segoe UI", 8.5f, FontStyle.Bold), Brushes.LightGreen, projected[0, gridSteps].X - 85, projected[0, gridSteps].Y + 12);
            g.DrawString("Press [TAB] to swap axes", new Font("Segoe UI", 9f, FontStyle.Bold), Brushes.White, 10, 10);
        }

        private void pnlCurves_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            double currentDist = GetManhattanDistance(playerX, playerY, bossX, bossY);
            int plotHeight = (pnlCurves.Height / 4) - 15; // Auto-scale to fit 4 graphs

            // 1. Boss HP Input
            DrawUniversePlot(g, 10, 10, pnlCurves.Width - 20, plotHeight, "Input 1: Boss HP", bossHP, 100.0,
                ("Low", Color.Crimson, x => TriangularMembership(x, 0, 0, 50)),
                ("High", Color.SeaGreen, x => TriangularMembership(x, 50, 100, 100)));

            // 2. Distance Input
            DrawUniversePlot(g, 10, 10 + plotHeight + 10, pnlCurves.Width - 20, plotHeight, "Input 2: Distance", currentDist, 18.0,
                ("Near", Color.Crimson, x => TriangularMembership(x, 0, 0, 4)),
                ("Mid", Color.DarkOrange, x => TriangularMembership(x, 2, 5, 8)),
                ("Far", Color.SeaGreen, x => TriangularMembership(x, 6, 18, 18)));

            // 3. Player Aggression Input
            DrawUniversePlot(g, 10, 10 + (plotHeight * 2) + 20, pnlCurves.Width - 20, plotHeight, "Input 3: Player Aggression", playerAggression, 100.0,
                ("Passive", Color.RoyalBlue, x => TriangularMembership(x, 0, 0, 50)),
                ("Aggressive", Color.Firebrick, x => TriangularMembership(x, 50, 100, 100)));

            // 4. Output Stance & Clipped Area
            double crispStance = CalculateFuzzyStance(currentDist, bossHP, playerAggression);
            DrawOutputPlot(g, 10, 10 + (plotHeight * 3) + 30, pnlCurves.Width - 20, plotHeight + 10, crispStance);
        }

        private void DrawUniversePlot(Graphics g, int x, int y, int w, int h, string title, double currentVal, double maxVal,
            params (string Label, Color Col, Func<double, double> Func)[] sets)
        {
            g.DrawString(title, new Font("Segoe UI", 8.5f, FontStyle.Bold), Brushes.Black, x, y);
            int plotY = y + 20;
            int plotH = h - 25;
            g.FillRectangle(Brushes.WhiteSmoke, x, plotY, w, plotH);
            g.DrawRectangle(Pens.LightGray, x, plotY, w, plotH);

            foreach (var set in sets)
            {
                using (var pen = new Pen(set.Col, 2))
                {
                    PointF prev = PointF.Empty;
                    for (int px = 0; px <= w; px++)
                    {
                        double input = (double)px / w * maxVal;
                        double mu = set.Func(input);
                        float sx = x + px;
                        float sy = plotY + (float)((1.0 - mu) * plotH);

                        if (px > 0) g.DrawLine(pen, prev, new PointF(sx, sy));
                        prev = new PointF(sx, sy);
                    }
                }
            }

            // Draw the dashed line tracking the live variable
            if (currentVal >= 0)
            {
                float markerX = x + (float)(Math.Min(currentVal, maxVal) / maxVal * w);
                using (var pen = new Pen(Color.Black, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    g.DrawLine(pen, markerX, plotY, markerX, plotY + plotH);
                }
                g.DrawString($"{currentVal:F1}", new Font("Segoe UI", 7.8f, FontStyle.Bold), Brushes.Black, markerX - 10, plotY - 15);
            }
        }

        private void DrawOutputPlot(Graphics g, int x, int y, int w, int h, double centroid)
        {
            g.DrawString("Output: Boss Stance [Shaded = Clipped Area, Blue Line = Centroid]", new Font("Segoe UI", 8.5f, FontStyle.Bold), Brushes.Black, x, y);
            int plotY = y + 20;
            int plotH = h - 25;
            g.FillRectangle(Brushes.WhiteSmoke, x, plotY, w, plotH);
            g.DrawRectangle(Pens.LightGray, x, plotY, w, plotH);

            // Calculate and draw the shaded Mamdani clipping area
            PointF[] polygon = new PointF[w + 2];
            polygon[0] = new PointF(x, plotY + plotH);
            for (int px = 0; px <= w; px++)
            {
                double input = (double)px / w * 100.0;
                double outDef = TriangularMembership(input, 0, 0, 40);
                double outNeu = TriangularMembership(input, 30, 50, 70);
                double outAgg = TriangularMembership(input, 60, 100, 100);

                // rule1_strength, rule2_strength, etc., are read from your global variables
                double clippedDef = Math.Min(rule1_strength, outDef);
                double clippedNeu = Math.Min(rule2_strength, outNeu);
                double clippedAgg = Math.Min(rule3_strength, outAgg);

                double agg = Math.Max(clippedDef, Math.Max(clippedNeu, clippedAgg));

                float sx = x + px;
                float sy = plotY + (float)((1.0 - agg) * plotH);
                polygon[px + 1] = new PointF(sx, sy);
            }

            using (var brush = new SolidBrush(Color.FromArgb(90, 100, 149, 237))) { g.FillPolygon(brush, polygon); }

            // Draw Outline Curves
            DrawCurve(g, x, plotY, w, plotH, 100.0, Color.RoyalBlue, val => TriangularMembership(val, 0, 0, 40));
            DrawCurve(g, x, plotY, w, plotH, 100.0, Color.Goldenrod, val => TriangularMembership(val, 30, 50, 70));
            DrawCurve(g, x, plotY, w, plotH, 100.0, Color.Firebrick, val => TriangularMembership(val, 60, 100, 100));

            // Draw Mamdani Centroid Line
            float markerX = x + (float)(centroid / 100.0 * w);
            using (var pen = new Pen(Color.Navy, 2.5f)) { g.DrawLine(pen, markerX, plotY, markerX, plotY + plotH); }
            g.DrawString($"Centroid: {centroid:F1}%", new Font("Segoe UI", 7.8f, FontStyle.Bold), Brushes.Navy, markerX - 25, plotY + plotH + 2);
        }

        private void DrawCurve(Graphics g, int x, int y, int w, int h, double maxVal, Color col, Func<double, double> func)
        {
            using (var pen = new Pen(col, 1.5f))
            {
                PointF prev = PointF.Empty;
                for (int px = 0; px <= w; px++)
                {
                    double input = (double)px / w * maxVal;
                    double mu = func(input);
                    float sx = x + px;
                    float sy = y + (float)((1.0 - mu) * h);
                    if (px > 0) g.DrawLine(pen, prev, new PointF(sx, sy));
                    prev = new PointF(sx, sy);
                }
            }
        }

        private Color Get3DHeightColor(double z)
        {
            double norm = Math.Max(0.0, Math.Min(1.0, z / 100.0));
            int red = (int)(norm * 220 + 20);
            int blue = (int)((1.0 - norm) * 220 + 20);
            int green = (int)(Math.Sin(norm * Math.PI) * 160);
            return Color.FromArgb(190, red, green, blue);
        }



        private void MoveBoss(bool moveTowards)
        {
            int dx = playerX - bossX;
            int dy = playerY - bossY;

            if (moveTowards)
            {
                // Chase by cutting across the largest gap, pushing the boss into the center
                if (Math.Abs(dx) >= Math.Abs(dy) && dx != 0)
                {
                    bossX += Math.Sign(dx);
                }
                else if (dy != 0)
                {
                    bossY += Math.Sign(dy);
                }
            }
            else
            {
                // Flee in the opposite direction, with fallback logic to slide along walls if trapped
                if (Math.Abs(dx) >= Math.Abs(dy) && dx != 0)
                {
                    if (dx > 0 && bossX > 0) bossX--;
                    else if (dx < 0 && bossX < 9) bossX++;
                    else if (dy > 0 && bossY > 0) bossY--;
                    else if (dy < 0 && bossY < 9) bossY++;
                }
                else if (dy != 0)
                {
                    if (dy > 0 && bossY > 0) bossY--;
                    else if (dy < 0 && bossY < 9) bossY++;
                    else if (dx > 0 && bossX > 0) bossX--;
                    else if (dx < 0 && bossX < 9) bossX++;
                }
            }
        }

        private double GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }

        private double CalculateDecay(double current, double target, double rate)
        {
            if (current > target) return Math.Max(target, current - rate);
            if (current < target) return Math.Min(target, current + rate);
            return current;
        }

        private double TriangularMembership(double x, double a, double b, double c)
        {
            if (x <= a || x >= c) return 0.0;
            if (x == b) return 1.0;
            if (x > a && x < b) return (x - a) / (b - a);
            return (c - x) / (c - b);
        }
    }

    // Declared after the Form class so the Visual Studio Form Designer opens cleanly
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
    }
}