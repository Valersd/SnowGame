using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;

namespace Snow.Core
{
    public class PainterWithLabel : Painter, IPainter
    {
        protected int _counter;
        public PainterWithLabel(IGameContext gameContext)
            :base(gameContext)
        {
            _counter = 0;
        }

        public override void Draw()
        {
            base.Draw();
            if (_counter < 1)
            {
                string label = " Sophisticated - 2016";
                Console.SetCursorPosition(_gameContext.PlayWidth - 1 - label.Length, _gameContext.Height - 1);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(label);
                _counter++;
            }
            
        }
    }
}
