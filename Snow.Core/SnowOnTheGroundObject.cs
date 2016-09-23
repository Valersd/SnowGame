using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;

namespace Snow.Core
{
    public class SnowOnTheGroundObject : GameObject, IGameObject
    {
        protected new Stack<ICell>[] _body;
        //protected List<ICell> _bodyForDrawing;
        protected int _width;
        protected int _meltingIntensity;
        protected readonly Random _random;
        protected int[] _meltingOrder;
        protected int _currentPosition;

        public SnowOnTheGroundObject(ICellFactory cellFactory, IGameEventFactory eventFactory)
            :base(cellFactory, eventFactory)
        {
            _width = 3;
            _meltingIntensity = 3;
            _random = new Random();
            _currentPosition = 0;
            //_bodyForDrawing = new List<ICell>();
            BePainted = false;
        }

        //public override IEnumerable<ICell> Body { get { return _bodyForDrawing; } }

        
        protected override IGameEvent Start(IGameEvent currentEvent)
        {
            CurrentEvent = currentEvent;
            _body = new Stack<ICell>[currentEvent.GameContext.PlayWidth];
            for (int i = 0; i < _body.Length; i++)
            {
                _body[i] = new Stack<ICell>();
            }
            _meltingOrder = new int[currentEvent.GameContext.PlayWidth];
            _meltingOrder.Fill();
            _meltingOrder.Shuffle(_random);
            return GetNextEvent("Move");
        }

        protected override IGameEvent Move(IGameEvent currentEvent)
        {
            //BePainted = false;
            switch (currentEvent.UserInput)
            {
                case ConsoleKey.M:
                    _width += 3;
                    _width = _width > 9 ? 9 : _width;
                    _meltingIntensity += 2;
                    _meltingIntensity = _meltingIntensity > 7 ? 7 : _meltingIntensity;
                    break;
                case ConsoleKey.L:
                    _width -= 3;
                    _width = _width < 0 ? 0 : _width;
                    _meltingIntensity -= 2;
                    _meltingIntensity = _meltingIntensity < 3 ? 3 : _meltingIntensity;
                    break;
            }
            var fallenSnow = currentEvent.GameContext.RecycleBin.Where(c => c is Snowflake && _body[c.Left].Count < _width).ToList();
            currentEvent.GameContext.RecycleBin.ExceptWith(fallenSnow);
            currentEvent.GameContext.Used.UnionWith(fallenSnow);
            foreach (var snowflake in fallenSnow)
            {
                _body[snowflake.Left].Push(snowflake);
                //_bodyForDrawing.Add(snowflake);
                //BePainted = true;
            }

            for (int i = 0; i < _meltingIntensity; i++)
            {
                int left = GetNextMeltingPosition(); //_random.Next(0, currentEvent.GameContext.PlayWidth);
                if (_body[left].Count > _width)
                {
                    var melted = _body[left].Pop();
                    //_bodyForDrawing.Remove(melted);
                    currentEvent.GameContext.Used.Remove(melted);
                    currentEvent.GameContext.RecycleBin.Add(melted);
                }
            }
            return GetNextEvent("Move");
        }

        protected int GetNextMeltingPosition()
        {
            int result = _meltingOrder[_currentPosition];
            _currentPosition++;
            if (_currentPosition >= _meltingOrder.Length)
            {
                _currentPosition = 0;
            }
            return result;
        }
    }
}
