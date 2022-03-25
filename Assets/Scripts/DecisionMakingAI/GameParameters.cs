using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    public abstract class GameParameters : JSONSerialisableScriptableObject
    {
        public abstract string GetParametersName();

        [SerializeField] 
        protected List<string> _fieldsToShowInGame;

        public bool ShowsField(string fieldName)
        {
            if (_fieldsToShowInGame == null)
            {
                return false;
            }

            return _fieldsToShowInGame.Contains(fieldName);
        }

        public void ToggleShowField(string fieldName)
        {
            if (_fieldsToShowInGame == null)
            {
                _fieldsToShowInGame = new List<string>();
            }

            if (ShowsField(fieldName))
            {
                _fieldsToShowInGame.Remove(fieldName);
            }
            else
            {
                _fieldsToShowInGame.Add(fieldName);
            }
        }

        public List<string> FieldsToShowInGame => _fieldsToShowInGame;
    }
}
