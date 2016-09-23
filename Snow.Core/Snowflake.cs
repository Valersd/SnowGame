using System;

using Game;

namespace Snow.Core
{
    public class Snowflake : Cell, ICell
    {
        public Snowflake()
        {
            Symbol = '\u2588';
            Color = ConsoleColor.White;
        }

        public Snowflake(int left, int top)
            :this()
        {
            Left = left;
            Top = top;
        }
    }
}
