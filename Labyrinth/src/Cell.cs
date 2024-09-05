using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Labyrinth
{
    class Cell
    {
        char ascii;
        int m;
        int n;

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

        public bool IsPassable
        {
            get { return this.Occupant.IsPassable && !(this.Occupant is Agent); }
        }

        Entity? occupant;
        public Entity Occupant
                {
            get { return this.occupant; }
            set { this.occupant = value; }
        }

        public Cell(char a, int m, int n) {
            ascii = a;
            this.m = m;
            this.n = n;
            this.occupant = new Item(' ', -1, -1);
        }

        public override string ToString()
        {
            return "<" + this.m + "," + this.n + ">: " + this.occupant.Ascii??"(null)";
        }  
    }
}

