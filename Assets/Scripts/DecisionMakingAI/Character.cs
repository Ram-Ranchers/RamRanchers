using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Character : Unit
    {
        public Character(CharacterData data) : base(data, new List<ResourceValue>() {}) {}
    }
}
