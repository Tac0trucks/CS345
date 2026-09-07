using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeMinimax
{
    public partial class Form1 : Form
    {
        private char[] board = new char[9];
        private Button[] buttons;
        private const char HUMAN = 'X';
        private const char AI = 'O';
        private const char EMPTY = ' ';
        public Form1()
        {
            InitializeComponent();
           InitializeGame();    
            
        }
        private void InitializeGame()
        {
            buttons = new Button[]
            {
                button1 , button2 , button3 , button4 , button5 , button6 , button7 , button8 , button9
            };
            for(int i = 0;i < 9; i++)
            {
                board[i] = EMPTY;
                buttons[i].Text = "";
                buttons[i].Enabled = true;

                //Wire up click event for each button
                buttons[i].Click -= Cell_Click;
                buttons[i].Click += Cell_Click;

            }
            lblStatus.Text = "Your turn (X)";
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            Button clickButton = (Button)sender;
            int index = Array.IndexOf(buttons, clickButton);
            if (board[index] == EMPTY)
            {
                MakeMove(index, HUMAN);

                if (CheckGameEnd()) return;
                // AI MOVE via Minimax
                lblStatus.Text = "Ai is thinking...";
                Application.DoEvents(); // Update UI before AI move
                int bestMove = FindBestMove();
                if(bestMove != -1)
                {
                    MakeMove(bestMove, AI);
                    if (CheckGameEnd()) return;
                }
            }
        }
        private void MakeMove(int index, char player)
        {
            board[index] = player;
            buttons[index].Text = player.ToString();
            buttons[index].Enabled = false;
        }
        private void DisableGrid()
        {
            foreach (Button btn in buttons)
            {
                btn.Enabled = false;
            }
        }
        private bool CheckGameEnd() 
            {
                if (CheckWin(HUMAN))
                {
                    lblStatus.Text = "You win!";
                    DisableGrid();
                    return true;
                }
                if (CheckWin(AI))
                {
                    lblStatus.Text = "AI wins!";
                    DisableGrid();
                    return true;
                }
                if (IsBoardFull())
                {
                lblStatus.Text = "It's a draw!";
                     return true;
                }
                lblStatus.Text = "Your turn (X)";
                return false;
            }
        private bool IsBoardFull()
        {
            foreach(char cell in board)
            {
                if (cell == EMPTY)
                    return false;
            } 
        return true; 
        }
        private bool CheckWin(char p)
        {
            int[,] winPatterns = new int[,]
            {
                {0,1,2}, {3,4,5}, {6,7,8}, // Rows
                {0,3,6}, {1,4,7}, {2,5,8}, // Columns
                {0,4,8}, {2,4,6}           // Diagonals
            };
            for(int i = 0; i < 8; i++)
            {
                if (board[winPatterns[i, 0]] == p && board[winPatterns[i, 1]] == p && board[winPatterns[i, 2]] == p)
                {
                    return true;
                }
            }
            return false;
        }


        // MINIMAX ALGORITHM

        private int FindBestMove()
        {
            int bestVal = int.MinValue;
            int bestMove = -1; 
            for(int i = 0; i < 9; i++)
            {
                if (board[i] == EMPTY)
                {
                    board[i] = AI;
                    //int moveVal = Minimax(board, 0, false);
                     int moveVal = MinimaxAlphaBeta(board, 0, false, int.MinValue, int.MaxValue);
                    board[i] = EMPTY;
                    if (moveVal > bestVal)
                    {
                        bestMove = i;
                        bestVal = moveVal;
                    }
                }
            }
            return bestMove;
        }
        private int Minimax(char[] currentBoard, int depth, bool isMax)
        {
            if(CheckWin(AI)) return 10 - depth;
            if(CheckWin(HUMAN)) return depth - 10;
            if(IsBoardFull()) return 0;

            if (isMax)
            {
                int best = int.MinValue;
                for(int i = 0; i < 9; i++)
                {
                    if (currentBoard[i] == EMPTY)
                    {
                        currentBoard[i] = AI;
                        best = Math.Max(best, Minimax(currentBoard, depth + 1, false));
                        currentBoard[i] = EMPTY;
                    }
                }
                return best; 
            }
            else
            {
                int best = int.MaxValue;
                for(int i = 0; i < 9; i++)
                {
                    if (currentBoard[i] == EMPTY)
                    {
                        currentBoard[i] = HUMAN;
                        best = Math.Min(best, Minimax(currentBoard, depth + 1, true));
                        currentBoard[i] = EMPTY;
                    }
                }
                return best;
            }
        }

        // Implementing Alpha-Beta Pruning for optimization

        private int MinimaxAlphaBeta(char[] currentBoard, int depth, bool isMax, int alpha, int beta)
        {
            if (CheckWin(AI)) return 10 - depth;
            if (CheckWin(HUMAN)) return depth - 10;
            if (IsBoardFull()) return 0;
            if (isMax){
                int best = int.MinValue;
                    for (int i = 0; i < 9; i++){
                        if (currentBoard[i] == EMPTY){
                        currentBoard[i] = AI;
                        best = Math.Max(best, MinimaxAlphaBeta(currentBoard, depth + 1, false, alpha, beta));
                        currentBoard[i] = EMPTY;

                        alpha = Math.Max(alpha, best);
                    if (beta <= alpha)
                        break; // Prune remaining branches
                    }
                }
                return best;
            }else{
            int best = int.MaxValue;
                for (int i = 0; i < 9; i++){
                    if (currentBoard[i] == EMPTY){
                        currentBoard[i] = HUMAN;
                        best = Math.Min(best, MinimaxAlphaBeta(currentBoard, depth + 1, true, alpha, beta));
                        currentBoard[i] = EMPTY;

                        beta = Math.Min(beta, best);
                    if (beta <= alpha)
                        break; // Prune remaining branches
                    }
                }
                return best;
            }
        }
        




        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset the game
            InitializeGame();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
