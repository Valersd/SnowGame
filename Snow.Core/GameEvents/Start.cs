﻿using Game;

namespace Snow.Core.GameEvents
{
    public class Start : GameEvent, IGameEvent
    {
        public Start(IGameContext gameContext)
            : base(gameContext)
        {
        }
        public override int Priority { get { return 100; } }
    }
}
