using System.Collections.Generic;

using Game;

namespace Snow.Core
{
    public abstract class GameObject : IGameObject
    {
        protected HashSet<ICell> _body;
        protected readonly IList<IInfo> _info;
        protected readonly ICellFactory _cellFactory;
        protected readonly IGameEventFactory _eventFactory;

        public GameObject(ICellFactory cellFactory, IGameEventFactory eventFactory)
        {
            _body = new HashSet<ICell>();
            _info = new List<IInfo>();
            _cellFactory = cellFactory;
            _eventFactory = eventFactory;
            BePainted = true;
        }

        public IGameEvent CurrentEvent { get; set; }
        public virtual IEnumerable<ICell> Body { get { return _body; } }
        public virtual IEnumerable<IInfo> Info { get { return _info; } }
        public virtual bool BePainted { get; set; }
        public virtual void Execute(IGameEvent gameEvent)
        {
            switch (gameEvent.Name)
            {
                case "Start": CurrentEvent = Start(gameEvent); break;
                case "Move": CurrentEvent = Move(gameEvent); break;
                default: CurrentEvent = Start(gameEvent);
                    break;
            }
        }

        public virtual void Add(ICell cell)
        {
            _body.Add(cell);
        }

        public virtual void Remove(ICell cell)
        {
            _body.Remove(cell);
        }

        protected abstract IGameEvent Start(IGameEvent currentEvent);
        protected abstract IGameEvent Move(IGameEvent currentEvent);

        protected virtual IGameEvent GetNextEvent(string name)
        {
            IGameEvent next = _eventFactory.Create(name, CurrentEvent.GameContext);
            next.UserInput = CurrentEvent.UserInput;
            return next;
        }
    }
}

