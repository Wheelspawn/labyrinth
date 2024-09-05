using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinth
{
    class GameLoop
    {
        Map map;

        Player player;

        static Dictionary<ConsoleKey, (int, int)> movementKeyMappings = new Dictionary<ConsoleKey, (int, int)>
        {
            // Horizontal/vertical movement
            { ConsoleKey.W, (-1, 0) },
            { ConsoleKey.A, (0, -1) },
            { ConsoleKey.S, (1, 0) },
            { ConsoleKey.D, (0, 1) },
            { ConsoleKey.Q, (-1, -1) },
            { ConsoleKey.E, (-1, 1) },
            { ConsoleKey.Z, (1, -1) },
            { ConsoleKey.C, (1, 1) },
            
            // Diagonal movement
            { ConsoleKey.NumPad8, (-1, 0) },
            { ConsoleKey.NumPad4, (0, -1) },
            { ConsoleKey.NumPad2, (1, 0) },
            { ConsoleKey.NumPad6, (0, 1) },
            { ConsoleKey.NumPad7, (-1, -1) },
            { ConsoleKey.NumPad9, (-1, 1) },
            { ConsoleKey.NumPad3, (1, -1) },
            { ConsoleKey.NumPad1, (1, 1) },

            // Loiter
            { ConsoleKey.L, (0, 0) },
            { ConsoleKey.NumPad5, (0, 0) }
        };


        public GameLoop()
        {

            String[] initString = new String[]{
            "╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬",
            "╬                                  ╬",
            "╬                                  ╬",
            "╬      ╬╬╬╬╬╬╬╬╬╬      ╬  ╬╬   ╬╬  ╬",
            "╬          ╬       ╬   ╬  ╬     ╬  ╬",
            "╬  ╬╬╬╬╬   ╬   ╬╬╬╬╬   ╬  ╬╬   ╬╬  ╬",
            "╬  ╬ * ╬   ╬   ╬       ╬           ╬",
            "╬  ╬   ╬ L ╬   ╬       ╬           ╬",
            "╬  ╬╬ ╬╬   ╬╬ ╬╬╬   ╬╬╬╬  ╬╬   ╬╬  ╬",
            "╬              ╬          ╬  ┐  ╬  ╬",
            "╬      ╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬  ╬╬   ╬╬  ╬",
            "╬                                  ╬",
            "╬                                  ╬",
            "╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬╬"
            };

            this.player = new Player('@', 4, 29);
            this.map = new Map(initString,player);

        }

        public void Loop()
        {
            Console.CursorVisible = false;
            ConsoleKey input;
            Random r = new Random();

            Dictionary<char, ConsoleKey> stuff = new Dictionary<char, ConsoleKey>();
            stuff.Add('w',ConsoleKey.W);
            stuff.Add('a',ConsoleKey.A);
            stuff.Add('s',ConsoleKey.S);
            stuff.Add('d',ConsoleKey.D);
            stuff.Add('l',ConsoleKey.L);

            if (Console.IsInputRedirected)
            { input = stuff[(char)Console.In.Peek()]; }
            else
            { input = Console.ReadKey(true).Key; }

            while (input != ConsoleKey.Escape)
            {
                if (movementKeyMappings.ContainsKey(input))
                {
                    // Move player
                    movementKeyMappings.TryGetValue(input, out var value);
                    int new_m = player.M + value.Item1;
                    int new_n = player.N + value.Item2;

                    if (map.IsPassable(new_m,new_n))
                    {
                        player.M = new_m;
                        player.N = new_n;
                    }


                    // Render map to console
                    map.Display();

                    map.MoveNPCs();
                }

                if (Console.IsInputRedirected)
                { input = stuff[(char)Console.In.Peek()]; }
                else
                { input = Console.ReadKey(true).Key; }

                Console.Clear();
            }
        }
    }
}
