namespace DecisionMakingAI
{
    public class GameResource
    {
        private string _name;
        private int _currentAmount;

        public GameResource(string name, int initalAmount)
        {
            _name = name;
            _currentAmount = initalAmount;
        }

        public void AddAmount(int value)
        {
            _currentAmount += value;
            if (_currentAmount < 0)
            {
                _currentAmount = 0;
            }
        }

        public string Name => _name;
        public int Amount => _currentAmount;
    }

    [System.Serializable]
    public class ResourceValue
    {
        public InGameResource code;
        public int amount = 0;

        public ResourceValue(InGameResource code, int amount)
        {
            this.code = code;
            this.amount = amount;
        }
    }
}
