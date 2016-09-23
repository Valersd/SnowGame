
using System;

using Game;

namespace Snow.Core
{
    public class GroundPiece : Cell, ICell
    {
        public GroundPiece()
        {
            Symbol = '\u2593';
            Color = ConsoleColor.DarkYellow;
        }

        public GroundPiece(int left, int top)
            :this()
        {
            Left = left;
            Top = top;
        }
    }
}
