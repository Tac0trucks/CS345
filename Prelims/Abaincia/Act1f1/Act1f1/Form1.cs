using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Act1f1
{
    public partial class Form1 : Form
    {
        VacuumEnvironment env = new VacuumEnvironment();
        private Agent agent = new SimpleReflexAgent();
        int cx = 0, cy = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Creating 2x2 world \n";

            richTextBox1.Text += env;


            for (int step = 0; step < 10; step++)

            {


                // Sensor
                var percept = env.Percept(agent);


                // Agent Brain
                var action = agent.Program(percept) as string;


                // Actuator / Environment reaction
                env.ExecuteAction(agent, action);


                // Percept contains (x,y,isDirty)
                var tup = percept as Tuple<int, int, bool>;

                string locationText = "(?, ?)";

                if (tup != null)

                    locationText = $"({tup.Item1}, {tup.Item2})";


                richTextBox1.AppendText($"Step {step + 1}: Action = {action} | Location = {locationText} | Score = {agent.Performance}\r\n");

                this.Refresh();


                Thread.Sleep(1000);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cx += 5;
            cy += 10;
            this.Refresh();
        }

        private void pictureBox1_Paint(Object sender, PaintEventArgs e)
        {
            Console.WriteLine("TEST");
            var percept = env.Percept(agent);
            Graphics g = e.Graphics;
            var tup = percept as Tuple<int, int, bool>;
            g.DrawArc(Pens.White,tup.Item2*200,tup.Item1*200,100,100,0,360);

        }
    }

}