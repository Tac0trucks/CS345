using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Search
{
    public partial class Form1 : Form
    {
        Queue<Node> frontier;
        HashSet<Node> visited;
        List<Node> origin;
        Node current;

        public Form1()
        {
            InitializeComponent();
        }
        public void runBFS(Node start, Node goal)
        {
            richTextBox1.Text = "running BFS";
            frontier = new Queue<Node>();
            visited = new HashSet<Node>();
            origin = new List<Node>();  
            frontier.Enqueue(start);
            visited.Add(start);
            origin.Add(start);
            while(frontier.Count > 0)
            {
                current= frontier.Dequeue();
                richTextBox1.Text += "\nCurrent Node: " + current.ToString();

                //check if goal is reached
                if(current.row == goal.row && current.col == goal.col)
                {
                    richTextBox1.Text += "\nGoal Reached!";
                    return;
                }
                foreach(Node in MazeEnvironment.getNeighbors(current))
                {
                   if(!visited.Contains(next))
                    {
                        visited.Add(next);
                        frontier.Enqueue(next);
                        origin.Add(next);

                    }
                }

                richTextBox1.Text += "Goal has not been reached yet, continuing search...";



            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
