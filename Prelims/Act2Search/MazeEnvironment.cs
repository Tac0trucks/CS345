using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Act2Search
{
    public struct Node
    {
        public int Row { get; }
        public int Col { get; }

        public Node(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            return $"({Row}, {Col})";
        }
    }

    public static class MazeSolver
    {

        
        // 0 = Path, 1 = Wall
        private static int[,] Grid =
        {
        { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }, // 1-10
    { 0, 0, 1, 0, 0, 0, 1, 1, 0, 0 }, // 11-20
    { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0 }, // 21-30  (Cell 24 'S' is [2, 3])
    { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 }, // 31-40
    { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 }, // 41-50
    { 0, 0, 0, 1, 0, 1, 0, 0, 1, 0 }, // 51-60
    { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0 }, // 61-70
    { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, // 71-80
    { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0 }, // 81-90  (Cell 87 'G' is [8, 6])
    { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, // 101-110
    { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, // 111-120
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // 121-130
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }  // 131-130
        };

        private static int Rows = Grid.GetLength(0);
        private static int Cols = Grid.GetLength(1);
        // Simulates an Adjacency List by checking Up, Down, Left, Right
        public static void RandomizeGrid(int rows, int cols, double wallProbability = 0.3, int? seed = null)
        {
            if (rows <= 0 || cols <= 0) throw new ArgumentException("rows and cols must be > 0");
            Rows = rows;
            Cols = cols;
            Grid = new int[Rows, Cols];
            var rnd = seed.HasValue ? new Random(seed.Value) : new Random();
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Grid[r, c] = (rnd.NextDouble() < wallProbability) ? 1 : 0;
                }
            }
        }
        public static IEnumerable GetNeighbors(Node n)
        {
            int[] dRow = { -1, 1, 0, 0 }; // Up, Down
            int[] dCol = { 0, 0, -1, 1 }; // Left, Right

            for (int i = 0; i < 4; i++)
            {
                int newRow = n.Row + dRow[i];
                int newCol = n.Col + dCol[i];

                // Check grid boundaries and ensure it's not a wall
                if (newRow >= 0 && newRow < Rows &&
                    newCol >= 0 && newCol < Cols &&
                    Grid[newRow, newCol] == 0)
                {
                    yield return new Node(newRow, newCol);
                }
            }
        }
        public static int[,] GetGrid() {

            return Grid;
        }
    };

       

        
       
    
        internal class MazeEnvironment
    {
    }
}
