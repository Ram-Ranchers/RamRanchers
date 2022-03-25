using System.Collections.Generic;

namespace DecisionMakingAI
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public class Node
    {
        protected NodeState state;
        public NodeState State => state;

        public Node _parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>(); 

        public Node()
        {
            _parent = null;
        }

        public Node(List<Node> children) : this()
        {
            SetChildren(children);
        }

        public virtual NodeState Evaluate() => NodeState.Failure;

        public void SetChildren(List<Node> children)
        {
            foreach (Node c in children)
            {
                Attach(c);
            }
        }
        
        public void Attach(Node child)
        {
            children.Add(child);
            child._parent = this;
        }

        public void Detach(Node child)
        {
            children.Remove(child);
            child._parent = null;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = _parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }

                node = node._parent;
            }

            return null;
        }
        
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = _parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                node = node._parent;
            }

            return false;
        }
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public Node Parent => _parent;
        public List<Node> Children => children;
        public bool HasChildren => children.Count > 0;
    }
}
