using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Act2Search
{
    public partial class Form1 : Form
    {
        Queue<Node> frontier;
        HashSet<Node> visited;
        List<Node> origin;
        Node current;
        List<Node> path = new List<Node>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public  void RunBFS(Node start, Node goal)
        {
            richTextBox1.Text = "Running BFS";
            frontier = new Queue<Node>();
            visited = new HashSet<Node>();
            origin = new List<Node>();
            var parent = new Dictionary<Node, Node>();
            path.Clear();

            frontier.Enqueue(start);
            visited.Add(start);
            parent[start] = start;
            origin.Add(start);

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();

                richTextBox1.Text += "\nExpanding: " + current.ToString();

                if (current.Row == goal.Row && current.Col == goal.Col)
                {
                    richTextBox1.Text += "\nGoal Found!";

                    // reconstruct path
                    var rev = new List<Node>();
                    var cur = current;
                    while (!cur.Equals(start))
                    {
                        rev.Add(cur);
                        if (!parent.TryGetValue(cur, out cur)) break;
                    }
                    rev.Add(start);
                    rev.Reverse();
                    path = rev;

                    richTextBox4.Text = string.Join("\n", path.Select(n => n.ToString()));
                    this.Refresh();
                    return;
                }

                foreach (Node next in MazeSolver.GetNeighbors(current))
                {
                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        frontier.Enqueue(next);
                        if (!parent.ContainsKey(next))
                            parent[next] = current;
                        origin.Add(current);
                    }
                }

                richTextBox2.Text = string.Join("\n", frontier.ToArray());
                richTextBox3.Text = string.Join("\n", visited.ToArray());

                this.Refresh();
            }
            richTextBox1.Text += "\nGoal not reachable.";

        }
        public void runDFS(Node start, Node goal)
        {
            richTextBox1.Text = "Running DFS";
            var frontierStack = new Stack<Node>();
            visited = new HashSet<Node>();
            origin = new List<Node>();
            var parent = new Dictionary<Node, Node>();
            path.Clear();

            frontierStack.Push(start);
            visited.Add(start);
            parent[start] = start;
            origin.Add(start);

            while (frontierStack.Count > 0)
            {
                current = frontierStack.Pop();
                richTextBox1.Text += "\nExpanding: " + current.ToString();

                if (current.Row == goal.Row && current.Col == goal.Col)
                {
                    richTextBox1.Text += "\nGoal Found! Reconstructing path...";

                    // reconstruct path
                    var rev = new List<Node>();
                    var cur = current;
                    while (!cur.Equals(start))
                    {
                        rev.Add(cur);
                        if (!parent.TryGetValue(cur, out cur)) break;
                    }
                    rev.Add(start);
                    rev.Reverse();
                    path = rev;

                    richTextBox4.Text = string.Join("\n", path.Select(n => n.ToString()));
                    this.Refresh();
                    return;
                }

                foreach (Node next in MazeSolver.GetNeighbors(current))
                {
                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        frontierStack.Push(next);
                        if (!parent.ContainsKey(next))
                            parent[next] = current;
                        origin.Add(current);
                    }
                }

                richTextBox2.Text = string.Join("\n", frontierStack.ToArray());
                richTextBox3.Text = string.Join("\n", visited.ToArray());
                richTextBox4.Text = string.Join("\n", origin.ToArray());
                this.Refresh();
            }
            richTextBox1.Text += "\nGoal not reachable.";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RunBFS(new Node(0, 0), new Node(4, 4));
            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int[,] grid = MazeSolver.GetGrid();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int cellW = Math.Max(1, pictureBox1.ClientSize.Width / cols);
            int cellH = Math.Max(1, pictureBox1.ClientSize.Height / rows);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Rectangle rect = new Rectangle(c * cellW, r * cellH, cellW, cellH);
                    if (grid[r, c] == 1)
                        e.Graphics.FillRectangle(Brushes.Black, rect);
                    else
                        e.Graphics.FillRectangle(Brushes.White, rect);

                    e.Graphics.DrawRectangle(Pens.Gray, rect);
                }
            }

            // draw path if available (yellow)
            if (path != null && path.Count > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(180, Color.Blue)))
                {
                    foreach (var p in path)
                    {
                        if (p.Row >= 0 && p.Row < rows && p.Col >= 0 && p.Col < cols)
                        {
                            Rectangle rectP = new Rectangle(p.Col * cellW, p.Row * cellH, cellW, cellH);
                            e.Graphics.FillRectangle(brush, rectP);
                            e.Graphics.DrawRectangle(Pens.Orange, rectP);
                        }
                    }
                }
            }

            // draw current cell on top
            Rectangle rect1 = new Rectangle(current.Col * cellW, current.Row * cellH, cellW, cellH);
            e.Graphics.FillRectangle(Brushes.Red, rect1);
            e.Graphics.DrawString("" + current.Row + "," + current.Col, new Font("Arial", 8), Brushes.Black, rect1);

        }
        private void buttonRandomize_Click(object sender, EventArgs e)
        {
           
            MazeSolver.RandomizeGrid(20, 20, 0.3);
            current = new Node(0, 0);
           
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            runDFS(new Node(0, 0), new Node(4, 4));
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); 
        }
    }
   
    }
