using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Ninject;
using Ninject.Extensions.Factory;

using Game;

using Snow.Core;
using Snow.Core.GameEvents;

namespace Snow.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            SnowGameFactory snowGameFactory = new SnowGameFactory();
            IGameEngine engine = snowGameFactory.GameEngine;

            engine.StartGame();

            while (true)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    engine.ProceedCommand(key);
                }
                engine.Run();
                Thread.Sleep(engine.GameSpeed + engine.SlowFactor);
            }
        }
    }
}
