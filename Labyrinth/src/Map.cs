using System;
using System.Collections.Generic;

namespace Labyrinth
{
    class Map
    {
        int m;
        int n;
        Dictionary<(int,int),Entity> map;
        // Cell[][] _map;
        public Player player;
        public List<Agent> npcs;
        public Map(String[] initString, Player p) {

            map = new Dictionary<(int,int),Entity>();

            // Dictionary<(int,int),Entity> agents = new Dictionary<(int,int),Entity>();
            // agents.Add((player.M,player.N),player);
            // agents.Remove((1,1));

            this.m = initString.Length;
            this.n = initString[0].Length;
            this.player = p;
            this.npcs = new List<Agent>{};
            
            for (int m = 0; m < initString.Length; m++)
            {
                for (int n = 0; n < initString[m].Length; n++)
                {
                    if (initString[m][n] == 'L')
                    {
                        this.npcs.Add(new Agent('L', m, n, ConsoleColor.Red));
                        // _map[m][n].Occupant = this.npcs.Last();
                        map.Add((m,n), new Item(' ', m, n));
                    }
                    else if (initString[m][n] == '╬')
                    {
                        map.Add((m,n), new Item(initString[m][n], m, n, ConsoleColor.DarkYellow, false));
                    }
                    else if (initString[m][n] == '*')
                    {
                        map.Add((m,n), new Item(initString[m][n], m, n, ConsoleColor.Yellow, true));
                    }
                    else if (initString[m][n] == '┐')
                    {
                        map.Add((m,n), new Item(initString[m][n], m, n, ConsoleColor.Gray, true));
                    }
                    else
                    {
                        map.Add((m,n), new Item(' ', m, n));
                    }
                }
            }
        }

        public void MoveNPCs()
        {
            HashSet<(int,int)> q = new HashSet<(int,int)>();
            Dictionary<(int,int),double> dist = new Dictionary<(int,int),double>();
            Dictionary<(int,int),(int,int)?> prev = new Dictionary<(int,int),(int,int)?>();

            // Djikstra's algorithm with player as source node
            for (int i = 0; i < this.m; i++)
            {
                for (int j = 0; j < this.n; j++)
                {
                    if (this.map.ContainsKey((i,j)))
                    {
                        Entity v = this.map[(i,j)];
                        if (v.IsPassable)
                        {
                            if (this.player.M == i && this.player.N == j)
                            {
                                dist.Add((v.M,v.N),0.0);
                            }
                            else
                            {
                                dist.Add((v.M,v.N),System.Double.MaxValue);
                            }
                            prev.Add((v.M,v.N),null);
                            q.Add((v.M,v.N));
                        }
                    }
                    else
                    {
                        dist.Add((i,j),System.Double.MaxValue);
                        prev.Add((i,j),null);
                        q.Add((i,j));
                    }
                }
            }

            while (q.Count > 0)
            {
                (int,int) u = dist.Where(x => q.Contains(x.Key)).MinBy(kvp => kvp.Value).Key;
                q.Remove(u);

                for (int m = -1; m < 2; m++)
                {
                    for (int n = -1; n < 2; n++)
                    {
                        try
                        {
                            (int,int) v = (u.Item1+m,u.Item2+n);
                            if (!(m == 0 && n == 0) && q.Contains(v))
                            {
                                double alt = dist[u] + Math.Sqrt(Math.Pow(v.Item1-u.Item1,2.0)+Math.Pow(v.Item2-u.Item2,2.0));
                                if (alt < dist[v])
                                {
                                    dist[v] = alt;
                                    prev[v] = u;
                                }
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
            }

            for (int k = 0; k < this.npcs.Count; k++)
            {
                Agent npc = this.npcs[k];
                (int,int)? source = (npc.M,npc.N);
                List<(int,int)> path = new List<(int,int)>{};
                // move NPC towards player by backtracking
                
                while (source != null)
                {
                    path.Add(((int,int))source);
                    source = prev[((int,int))source];
                }

                if (path.Count > 1 && !(path[1].Item1 == this.player.M && path[1].Item2 == this.player.N))
                {
                    npc.M = path[1].Item1;
                    npc.N = path[1].Item2;
                }

                for (int j = 1; j < path.Count-1; j++)
                {
                    Console.SetCursorPosition(path[j].Item2, path[j].Item1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("~");
                    Console.ResetColor();
                }
            }
        }

        public void Display()
        {
            for (int m = 0; m < this.m; m++)
            {
                for (int n = 0; n < this.n; n++)
                {
                    Console.SetCursorPosition(n,m);
                    if (this.map.ContainsKey((m,n)))
                    {
                        // Console.ForegroundColor = this._map[m][n].Occupant.Color;
                        Console.Write(this.map[(m,n)].Ascii);
                        // Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
            for (int i = 0; i < this.npcs.Count; i++)
            {
                Console.SetCursorPosition(this.npcs[i].N,this.npcs[i].M);
                Console.Write(this.npcs[i].Ascii);
            }
            Console.SetCursorPosition(this.player.N,this.player.M);
            Console.Write('@');
        }

        public bool IsPassable(int m, int n)
        {
            if (this.map.ContainsKey((m,n)))
            {
                return this.map[(m,n)].IsPassable;
            }
            return true;
        }
    }
}

