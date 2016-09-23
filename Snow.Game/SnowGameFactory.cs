using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;
using Ninject.Extensions.Factory;

using Game;
using Snow.Core;
using Snow.Core.GameEvents;

namespace Snow.Game
{
    public class SnowGameFactory
    {
        private IGameEngine _gameEngine;

        public SnowGameFactory()
        {
            RegisterBindings();
        }

        public IGameEngine GameEngine 
        { 
            get { return _gameEngine; } 
            private set { _gameEngine = value; } 
        }

        private void RegisterBindings()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IConfigurationDataProvider>().To<ConfigurationDataProvider>();

            kernel.Bind<IGameEventFactory>().ToFactory(() => new UseFirstArgumentAsNameInstanceProvider());

            kernel.Bind<ICellFactory>().ToFactory(() => new UseFirstArgumentAsNameInstanceProvider());
            kernel.Bind<ICell>().To<GroundPiece>().Named("GroundPiece");
            kernel.Bind<ICell>().To<Snowflake>().Named("Snowflake");
            kernel.Bind<ICell>().To<FlyingSnowflake>().Named("FlyingSnowflake");

            kernel.Bind<IGameContext>().To<SnowGameContext>();
            IGameContext gameContext = kernel.Get<IGameContext>();

            kernel.Bind<IGameEvent>().To<Start>().Named("Start")
                .WithConstructorArgument("gameContext", gameContext);
            kernel.Bind<IGameEvent>().To<Move>().Named("Move")
                .WithConstructorArgument("gameContext", gameContext);

            kernel.Bind<Start>().ToSelf().InSingletonScope();
            kernel.Bind<Move>().ToSelf().InSingletonScope();

            kernel.Bind<IPainter>().To<PainterWithLabel>()
                .WithConstructorArgument("gameContext", gameContext);
            IPainter painter = kernel.Get<IPainter>();

            kernel.Bind<IGameEngine>().To<SnowGameEngine>()
                .WithConstructorArgument("gameContext", gameContext)
                .WithConstructorArgument("painter", painter);
             _gameEngine = kernel.Get<IGameEngine>();
        }
    }
}
