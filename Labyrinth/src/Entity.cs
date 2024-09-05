using System;
using System.Collections.Generic;

namespace Labyrinth
{
    public class Entity
    {
        protected char ascii;
        protected ConsoleColor color;
        protected int m;
        protected int n;

        public bool IsPassable { get; set; }
        public Entity() {}

        public Entity(char a, int m, int n, ConsoleColor c=ConsoleColor.White, bool p=true) {
            this.ascii = a;
            this.m = m;
            this.n = n;
            this.color = c;
            this.IsPassable = p;
        }
        
        public char Ascii { get { return this.ascii; } }
        public ConsoleColor Color { get { return this.color; } }

        public int M
        {
            get { return this.m; }
            set { this.m = value; }
        }
        
        public int N
        {
            get { return this.n; }
            set { this.n = value; }
        }
    }

    public class Agent : Entity
    {
        public Agent() {}

        public Agent(char a, int m, int n, ConsoleColor c=ConsoleColor.White, bool p=false) {
            this.ascii = a;
            this.M = m;
            this.N = n;
            this.color = c;
            this.IsPassable = p;
        }
    }

    public class Player : Agent
    {
        public Player() {}

        public Player(char a, int m, int n, ConsoleColor c=ConsoleColor.White, bool p=false) {
            this.ascii = a;
            this.M = m;
            this.N = n;
            this.color = c;
            this.IsPassable = p;
        }

        /*
        public void Move(ConsoleKey input)
        {
        
        movementKeyMappings.TryGetValue(input, out var value);
        this.M += value.Item1;
        this.N += value.Item2;
        } */
    }

    public class Item : Entity
    {

        public Item() {}

        public Item(char a, int m, int n, ConsoleColor c=ConsoleColor.White, bool p=true) {
            this.ascii = a;
            this.M = m;
            this.N = n;
            this.color = c;
            this.IsPassable = p;
        }
    }
}

