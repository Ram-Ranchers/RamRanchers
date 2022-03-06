using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    public class Unit
    {
        protected UnitData _data;
        protected Transform _transform;
        protected int _currentHealth;
        protected string _uid;
        protected int _level;
        protected List<ResourceValue> _production;
        protected List<SkillManager> _skillManagers;

        public Unit(UnitData data) : this(data, new List<ResourceValue>() {})
        {
        }
   

        public Unit(UnitData data, List<ResourceValue> production)
        {
            _data = data;
            _currentHealth = data.healthpoints;
            _uid = System.Guid.NewGuid().ToString();
            _level = 1;
            _production = production;
            
            GameObject g = GameObject.Instantiate(data.prefab) as GameObject;
            _transform = g.transform;

            _skillManagers = new List<SkillManager>();
            SkillManager sm;
            foreach (SkillData skill in _data.skills)
            {
                sm = g.AddComponent<SkillManager>();
                sm.Initialise(skill, g);
                _skillManagers.Add(sm);
            }
        }

        public void TriggerSkill(int index, GameObject target = null)
        {
            _skillManagers[index].Trigger(target);
        }
        
        public void LevelUp()
        {
            _level += 1;
        }

        public void ProduceResources()
        {
            foreach (ResourceValue resource in _production)
            {
                Globals.Game_Resources[resource.code].AddAmount(resource.amount);
            }
        }
        
        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public virtual void Place()
        {
            _transform.GetComponent<BoxCollider>().isTrigger = false;
            foreach (ResourceValue resource in _data.cost)
            {
                Globals.Game_Resources[resource.code].AddAmount(-resource.amount);
            }
        }

        public bool CanBuy()
        {
            return _data.CanBuy();
        }
        
        public UnitData Data => _data;
        public string Code => _data.code;
        public Transform Transform => _transform;

        public int HP
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public int MaxHP => _data.healthpoints;
        public string Uid => _uid;
        public int Level => _level;
        public List<ResourceValue> Production => _production;
        public List<SkillManager> SkillManagers => _skillManagers;
    }

        // public virtual void Place()
        // {
        //     if (owner == GameManager.instance.gamePlayersPatameters.myPlayerId)
        //     {
        //         
        //     }
        // }
}
