using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;

namespace Snow.Core
{
    public class SnowInTheAirObject : GameObject, IGameObject
    {
        protected readonly Random _random;
        protected Direction _direction;
        protected int _intensity;
        protected int[] _appearingOrder;
        protected int _currentPosition;

        public SnowInTheAirObject(ICellFactory cellFactory, IGameEventFactory eventFactory)
            : base(cellFactory, eventFactory)
        {
            _random = new Random();
            _direction = Direction.Down;
            _intensity = 1;
            _currentPosition = 0;
        }

        protected override IGameEvent Start(IGameEvent currentEvent)
        {
            CurrentEvent = currentEvent;
            _appearingOrder = new int[currentEvent.GameContext.PlayWidth];
            _appearingOrder.Fill();
            _appearingOrder.Shuffle(_random);
            return GetNextEvent("Move");
        }

        protected override IGameEvent Move(IGameEvent currentEvent)
        {
            switch (currentEvent.UserInput)
            {
                case ConsoleKey.LeftArrow:
                    _direction = Direction.Left;
                    break;
                case ConsoleKey.DownArrow:
                    _direction = Direction.Down;
                    break;
                case ConsoleKey.RightArrow:
                    _direction = Direction.Right;
                    break;
                case ConsoleKey.M:
                    _intensity++;
                    _intensity = _intensity > 3 ? 3 : _intensity;
                    break;
                case ConsoleKey.L:
                    _intensity--;
                    _intensity = _intensity < 0 ? 0 : _intensity;
                    break;

            }

            List<ICell> inTheAir = new List<ICell>();
            List<ICell> onTheGround = new List<ICell>();
            foreach (var snowflake in _body)
            {
                MoveToNextPosition(currentEvent, snowflake, ref inTheAir, ref onTheGround);
            }
            currentEvent.GameContext.Used.ExceptWith(_body);
            _body.Clear();
            currentEvent.GameContext.Used.UnionWith(inTheAir);
            _body.UnionWith(inTheAir);

            int newSnowflakesNumber = _intensity * 1;
            List<ICell> newSnowflakes = new List<ICell>();
            for (int i = 0; i < newSnowflakesNumber; i++)
            {
                int left = GetNextAppearingPosition(); //_random.Next(0, currentEvent.GameContext.PlayWidth);
                ICell snowflake = _cellFactory.Create("FlyingSnowflake", left, 0);
                newSnowflakes.Add(snowflake);
            }
            _body.UnionWith(newSnowflakes);
            currentEvent.GameContext.Used.UnionWith(newSnowflakes);

            return GetNextEvent("Move");
        }

        protected void MoveToNextPosition
            (IGameEvent currentEvent, ICell cell, ref List<ICell> inTheAir, ref List<ICell> onTheGround)
        {
            ICell newCell = default(ICell);
            int newLeft = cell.Left;
            int newTop = cell.Top + 1;
            switch (_direction)
            {
                case Direction.Left:
                    newLeft -= 1;
                    newLeft = newLeft < 0 ? currentEvent.GameContext.PlayWidth - 1 : newLeft;
                    break;
                case Direction.Right:
                    newLeft += 1;
                    newLeft = newLeft > currentEvent.GameContext.PlayWidth - 1 ? 0 : newLeft;
                    break;
                default:
                    break;

            }
            ICell existing = currentEvent.GameContext.Used.FirstOrDefault(c => c.Left == newLeft && c.Top == newTop);
            if (existing != null && !(existing is FlyingSnowflake))
            {
                if (!(_direction == Direction.Down))
                {
                    var temp = _direction;
                    _direction = Direction.Down;
                    MoveToNextPosition(currentEvent, cell, ref inTheAir, ref onTheGround);
                    _direction = temp;
                }
                else
                {
                    newCell = _cellFactory.Create("Snowflake", cell.Left, cell.Top);
                    currentEvent.GameContext.RecycleBin.Add(newCell);
                    onTheGround.Add(newCell);
                }
            }
            else
            {
                newCell = _cellFactory.Create("FlyingSnowflake", newLeft, newTop);
                currentEvent.GameContext.RecycleBin.Add(cell);
                inTheAir.Add(newCell);
            }
        }

        protected int GetNextAppearingPosition()
        {
            int result = _appearingOrder[_currentPosition];
            _currentPosition++;
            if (_currentPosition >= _appearingOrder.Length)
            {
                _currentPosition = 0;
            }
            return result;
        }
    }
}
