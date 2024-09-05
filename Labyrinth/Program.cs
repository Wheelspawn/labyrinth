using System;
using System.Collections.Generic;

namespace Labyrinth
{
    class Program
    {
        static void Main(string[] args)
        {
            String border_top = "  ~~~~~~~~~~~~~~~~~\n";
            String border_side = "  ~               ~\n";
            String name = "  ~   LABYRINTH   ~\n";
            Console.Write("\n" + border_top + border_side + name + border_side + border_top);
            Console.Write("\n Press any key to continue.");

            GameLoop g = new GameLoop();
            g.Loop();
        }
    }
}

