using UnityEngine;

namespace DecisionMakingAI
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        public Node Root => _root;
        protected abstract Node SetupTree();
    }
}
