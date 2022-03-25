using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Character : Unit
    {
        public Character(CharacterData data, int owner) : base(data, owner, new List<ResourceValue>() {}) {}
    }
}
