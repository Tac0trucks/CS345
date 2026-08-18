using Act1f1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Act1f1
{

    public abstract class Environment
    {
        public abstract void ExecuteAction(Agent agent, string action);
        public abstract Tuple<int, int, bool> Percept(Agent agent);
    }

    public abstract class Agent
    {
        // Tracks the performance measure

        public abstract object Program(Tuple<int, int, bool> percept);
        public int Performance { get; set; } = 0;

        // The brain of the agent: takes a percept and returns an action.public abstract object Program(object percept);
    }

    public class VacuumEnvironment : Environment
    {
        // 2x2 grid: 1 = dirty, 0 = clean

        private int[,] grid = new int[2, 2];

        private int agentX = 0;

        private int agentY = 0;

        private Random rand = new Random();


        public VacuumEnvironment()

        {
            // Randomly initialize grid cells to 0 or 1
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)

                {
                    grid[i, j] = rand.Next(0, 2); // 0 or 1 dirt randomization
                }
            }


            agentX = 0; // start at top-left
            agentY = 0;
        }

        public int[] AgentLoc()

        {
            int[] loc = new int[2] { agentX, agentY };

            return loc;
        }


        public override string ToString()

        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Grid state:");

            for (int i = 0; i < 2; i++)

            {
                for (int j = 0; j < 2; j++)

                {
                    sb.Append(grid[i, j] + " ");
                }

                sb.AppendLine();
            }

            sb.AppendLine($"Agent position: ({agentX}, {agentY})");

            return sb.ToString();
        }


        // Returns (x, y, isDirty) as a Tuple<int,int,bool>
        public override Tuple<int, int, bool> Percept(Agent agent)

        {
            //_ = agent; // mark parameter as intentionally unused
            bool isDirty = grid[agentX, agentY] == 1;

            return Tuple.Create<int, int, bool>(agentX, agentY, isDirty);
        }


        // Applies an action string: "Suck", "Up", "Down", "Left", "Right"
        public override void ExecuteAction(Agent agent, string action)
        {
            string act = action as string;

            if (act == null)

            {
                agent.Performance -= 1; // invalid action
                return;
            }


            if (act == "Suck")
            {
                if (grid[agentX, agentY] == 1) // assuming nga hugaw            {

                    grid[agentX, agentY] = 0;

                agent.Performance += 10; // reward for cleaning            }
            }

            else if (act == "Up" && agentX > 0)
            {
                agentX -= 1;

                agent.Performance -= 1; // cost for moving
            }

            else if (act == "Down" && agentX < 1)
            {
                agentX += 1;

                agent.Performance -= 1;
            }

            else if (act == "Left" && agentY > 0)

            {
                agentY -= 1;

                agent.Performance -= 1;
            }

            else if (act == "Right" && agentY < 1)

            {
                agentY += 1;

                agent.Performance -= 1;
            }

            else
            {
                agent.Performance -= 1; // bumped into wall or invalid move
            }
        }
    }

    public class SimpleReflexAgent : Agent
    {

        private readonly Random rand = new Random();


        // percept is expected to be Tuple<int,int,bool> (x, y, isDirty)
        public override object Program(Tuple<int, int, bool> percept)
        {

            var tup = percept;
            if (tup == null)
                return null;

            bool isDirty = tup.Item3;
            if (isDirty)

                return "Suck";


            string[] choices = { "Up", "Down", "Left", "Right" };

            return choices[rand.Next(choices.Length)];

        }

    }
}