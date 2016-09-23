using System;
using System.Linq;

using Game;

namespace Snow.Core
{
    public class SnowGameEngine : GameEngine, IGameEngine
    {
        protected int _gameSpeed;
        protected int _slowFactor;

        public SnowGameEngine
            (IGameContext gameContext, IPainter painter, IGameEventFactory eventFactory)
            :base(gameContext, painter, eventFactory)
        {
            _gameSpeed = 80;
            _slowFactor = 0;
        }

        public override int GameSpeed { get { return _gameSpeed; } protected set { _gameSpeed = value; } }
        public override int SlowFactor { get { return _slowFactor; } protected set { _slowFactor = value; } }

        public override void StartGame()
        {
            while (true)
            {
                ShowGameRules();
                if (!Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                }
            }
        }

        public override void ProceedCommand(ConsoleKey input)
        {
            if (CurrentEvent.Name == "Move")
            {
                switch (input)
                {
                    case ConsoleKey.DownArrow:
                        CurrentEvent.UserInput = ConsoleKey.DownArrow;
                        break;
                    case ConsoleKey.LeftArrow:
                        CurrentEvent.UserInput = ConsoleKey.LeftArrow;
                        break;
                    case ConsoleKey.RightArrow:
                        CurrentEvent.UserInput = ConsoleKey.RightArrow;
                        break;
                    case ConsoleKey.M:
                        CurrentEvent.UserInput = ConsoleKey.M;
                        _slowFactor -= 20;
                        if (_slowFactor < -60)
                        {
                            _slowFactor = -60;
                        }
                        break;
                    case ConsoleKey.L:
                        CurrentEvent.UserInput = ConsoleKey.L;
                        _slowFactor += 20;
                        if (_slowFactor > 0)
                        {
                            _slowFactor = 0;
                        }
                        break;
                    case ConsoleKey.P:
                        while (true)
                        {
                            if (!Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.P)
                            {
                                break;
                            }
                        }
                        break;
                }
            }
        }

        public override void Run()
        {
            foreach (var gameObject in _gameContext.GameObjects)
            {
                gameObject.Execute(CurrentEvent);
            }
            CurrentEvent = _gameContext.GameObjects.Max(o => o.CurrentEvent);
            CurrentEvent.UserInput = default(ConsoleKey);
            Painter.Draw();
            //Console.WriteLine(_slowFactor);
        }


        protected void ShowGameRules()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\tGAME RULES:");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tLEFT ARROW - Wind from right");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tRIGHT ARROW - Wind from left");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tDOWN ARROW - No wind");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tM - More snow");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tL - Less snow");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tP - Pause"); 
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\tPress ENTER to start ...");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t\t\t\t\tSophisticated - 2016");
        }
    }
}
