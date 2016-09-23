using System;

using Game;

namespace Snow.Core
{
    public class SnowGameContext : GameContext, IGameContext
    {
        public SnowGameContext(IConfigurationDataProvider dataProvider, 
            GroundObject ground,
            SnowInTheAirObject snowInTheAir,
            SnowOnTheGroundObject snowOnTheGround)
            : base(dataProvider, ground, snowInTheAir, snowOnTheGround)
        {
            Console.Title = "SNOW";
        }
    }
}
