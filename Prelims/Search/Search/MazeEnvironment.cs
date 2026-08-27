using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
    // Define a Node
    public struct Node
    {
        public int row { get; }
        public int col { get;}
       
        public Node(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public override string ToString()
        {
            return $"({row}, {col})";
        }
    }

    public static class Maze
    {
        public static int[,] grid = new int[,]
        {
            {0, 0, 1, 0, 0},
            {0, 1, 1, 0, 1},
            {0, 0, 0, 0, 0},
            {1, 1, 1, 1, 0},
            {0, 0, 0, 1, 0}
        };

    }


    internal class MazeEnvironment
    {
        internal static object getNeighbors(Node current)
        {
           
        }
    }
}
