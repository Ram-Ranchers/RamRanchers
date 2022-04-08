using UnityEngine;
using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Timer : Node
    {
        private float _delay;
        private float _time;

        public delegate void TickEnded();
        public event TickEnded onTickEnded;

        public Timer(float delay, TickEnded onTickEnded = null) : base()
        {
            _delay = delay;
            _time = _delay;
            this.onTickEnded = onTickEnded;
        }

        public Timer(float delay, List<Node> children, TickEnded onTickEndeed = null) : base(children)
        {
            _delay = delay;
            _time = _delay;
            this.onTickEnded = onTickEndeed;
        }

        public override bool IsFlowNode => true;
        
        public override NodeState Evaluate()
        {
            if (!HasChildren)
            {
                return NodeState.Failure;
            }

            if (_time <= 0)
            {
                _time = _delay;
                _state = _children[0].Evaluate();

                if (onTickEnded != null)
                {
                    onTickEnded();
                }

                _state = NodeState.Success;
            }
            else
            {
                _time -= Time.deltaTime;
                _state = NodeState.Running;
            }

            return _state;
        }
    }
}
