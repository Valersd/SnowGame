using System;

using Game;

namespace Snow.Core
{
    public class FlyingSnowflake : Cell, ICell
    {
        public FlyingSnowflake()
        {
            Symbol = '*';
            Color = ConsoleColor.White;
        }

        public FlyingSnowflake(int left, int top)
            : this()
        {
            Left = left;
            Top = top;
        }
    }
}
