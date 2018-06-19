using System.Collections.Generic;
using Util;

namespace Action
{
    public class SequnceAction : GameAction
    {
        private readonly List<GameAction> _sequence;
        private int _remainRound;
        private int _current;

        public SequnceAction(IEnumerable<GameAction> sequence, int repeatTime = 1)
        {
            _sequence = new List<GameAction>(sequence);
            _remainRound = repeatTime - 1;
        }

        public override void Start()
        {
            foreach (var action in _sequence)
            {
                action.TargetGameObject = TargetGameObject;
                action.Callback = Callback;
                action.Start();
            }
        }

        public override void Update()
        {
            if (_sequence.Count == 0 || _current >= _sequence.Count) return;

            var currentAction = _sequence[_current];
            if (currentAction.State == ActionState.Completed && ++_current == _sequence.Count)
            {
                if (_remainRound == 0)
                {
                    State = ActionState.Completed;
                    return;
                }

                _current = 0;
                foreach (var action in _sequence)
                {
                    action.State = ActionState.Started;
                }
                _remainRound = _remainRound < 0 ? -1 : _remainRound - 1;
            }
            else
            {
                currentAction.Update();
            }
        }
    }
}