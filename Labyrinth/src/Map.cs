using System;
using System.Collections.Generic;

namespace Labyrinth
{
    class Map
    {
        int m;
        int n;
        Cell[][] _map;
        public Player player;
        public List<Agent> npcs;
        public Map(String[] initString, Player p) {

            _map = new Cell[initString.Length][];

            // Dictionary<(int,int),Entity> agents = new Dictionary<(int,int),Entity>();
            // agents.Add((player.M,player.N),player);
            // agents.Remove((1,1));

            this.m = initString.Length;
            this.n = initString[0].Length;
            this.player = p;
            this.npcs = new List<Agent>{};
            
            for (int m = 0; m < initString.Length; m++)
            {
                _map[m] = new Cell[initString[m].Length];

                for (int n = 0; n < initString[m].Length; n++)
                {
                    _map[m][n] = new Cell(initString[m][n], m, n);

                    if (initString[m][n] == 'L')
                    {
                        this.npcs.Add(new Agent('L', m, n, ConsoleColor.Red));
                        // _map[m][n].Occupant = this.npcs.Last();
                        _map[m][n].Occupant = new Item(' ', m, n);
                    }
                    else if (initString[m][n] == '╬')
                    {
                        _map[m][n].Occupant = new Item(initString[m][n], m, n, ConsoleColor.DarkYellow, false);
                    }
                    else if (initString[m][n] == '*')
                    {
                        _map[m][n].Occupant = new Item(initString[m][n], m, n, ConsoleColor.Yellow, true);
                    }
                    else if (initString[m][n] == '┐')
                    {
                        _map[m][n].Occupant = new Item(initString[m][n], m, n, ConsoleColor.Gray, true);
                    }
                    else
                    {
                        _map[m][n].Occupant = new Item(' ', m, n);
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
            for (int i = 0; i < this._map.Length; i++)
            {
                for (int j = 0; j < this._map[i].Length; j++)
                {
                    Cell v = this._map[i][j];
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
                        catch (IndexOutOfRangeException e)
                        {
                            continue;
                        }
                    }
                }
            }

            Cell target = this._map[this.player.M][this.player.N];

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
                    if (this._map[m][n].Occupant != null)
                    {
                        // Console.ForegroundColor = this._map[m][n].Occupant.Color;
                        Console.Write(this._map[m][n].Occupant.Ascii);
                        // Console.ResetColor();
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
            return this._map[m][n].IsPassable;
        }

        
    }
}

