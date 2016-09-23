using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;

namespace Snow.Core
{
    public class GroundObject : GameObject, IGameObject
    {
        protected readonly Random _random;
        protected int[] _groundPositions;
        protected int _currentPosition;
        public GroundObject(ICellFactory cellFactory, IGameEventFactory eventFactory)
            :base(cellFactory, eventFactory)
        {
            _random = new Random();
            _currentPosition = 0;
        }
        protected override IGameEvent Start(IGameEvent currentEvent)
        {
            CurrentEvent = currentEvent;
            _groundPositions = new int[currentEvent.GameContext.PlayWidth];
            FillGroundPositions();
            _groundPositions.Shuffle(_random);
            int top = currentEvent.GameContext.Height - 9;
            for (int left = currentEvent.GameContext.PlayWidth - 1; left >= 0; left--)
            {
                do
                {
                    top += GetNextGroundPosition(); //_random.Next(-1, 2);
                } while (top >= currentEvent.GameContext.Height - 2);
                for (int currentTop = currentEvent.GameContext.Height - 1; currentTop > top - 1; currentTop--)
                {
                    ICell cell = _cellFactory.Create("GroundPiece", left, currentTop);
                    _body.Add(cell);
                    if (left == 0 || left == currentEvent.GameContext.PlayWidth - 1)
                    {
                        currentEvent.GameContext.Used.Add(cell);
                    }
                    
                }
                for (int currentTop = top - 1; currentTop <= top; currentTop++)
                {
                    ICell topCell = _cellFactory.Create("GroundPiece", left, currentTop);
                    _body.Add(topCell);
                    currentEvent.GameContext.Used.Add(topCell);
                }
                
            }
            return GetNextEvent("Move");
        }

        protected override IGameEvent Move(IGameEvent currentEvent)
        {
            BePainted = false;
            return GetNextEvent("Move");
        }

        protected void FillGroundPositions()
        {
            int[] variations = new int[] { 0, -1, 1 };
            for (int i = 0; i < _groundPositions.Length; i++)
            {
                _groundPositions[i] = variations[i % variations.Length];
            }
        }

        protected int GetNextGroundPosition()
        {
            int result = _groundPositions[_currentPosition];
            _currentPosition++;
            if (_currentPosition >= _groundPositions.Length)
            {
                _currentPosition = 0;
            }
            return result;
        }
    }
}
