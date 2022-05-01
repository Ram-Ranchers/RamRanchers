using System.Collections.Generic;
using System.Linq;
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
        protected List<SkillManager> _skillManagers;
        protected float _fieldOfView;
        protected int _owner;
        protected Dictionary<InGameResource, int> _production;

        public Unit(UnitData data, int owner) : this(data, owner, new List<ResourceValue>() {})
        {
        }
   

        public Unit(UnitData data, int owner, List<ResourceValue> production)
        {
            _uid = System.Guid.NewGuid().ToString();
            _data = data;
            _currentHealth = data.healthpoints;
            _level = 1;
            _production = production.ToDictionary(rv => rv.code, rv => rv.amount);
            _fieldOfView = data.fieldOfView;
            _owner = owner;
            
            GameObject g = GameObject.Instantiate(data.prefab) as GameObject;
            _transform = g.transform;
            _transform.GetComponent<UnitManager>().SetOwnerMaterial(owner);
            
            _skillManagers = new List<SkillManager>();
            SkillManager sm;
            foreach (SkillData skill in _data.skills)
            {
                sm = g.AddComponent<SkillManager>();
                sm.Initialise(skill, g);
                _skillManagers.Add(sm);
            }
            
            _transform.GetComponent<UnitManager>().Initialise(this);
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
            foreach (KeyValuePair<InGameResource, int> resource in _production)
            {
                Globals.Game_Resources[resource.Key].AddAmount(resource.Value);
            }
        }
        
        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public virtual void Place()
        {
            _transform.GetComponent<BoxCollider>().isTrigger = false;

            _transform.GetComponent<UnitManager>().EnableFOV(_fieldOfView);

            foreach (ResourceValue resource in _data.cost)
            {
                Globals.Game_Resources[resource.code].AddAmount(-resource.amount);
            }
        }

        public bool CanBuy()
        {
            return _data.CanBuy();
        }

        public Dictionary<InGameResource, int> ComputeProduction()
        {
            if (_data.canProduce.Length == 0)
            {
                return null;
            }

            GameGlobalParameters globalParams = GameManager.instance.gameGlobalParameters;
            GamePlayersParameters playerParams = GameManager.instance.gamePlayersParameters;
            Vector3 pos = _transform.position;

            if (_data.canProduce.Contains(InGameResource.Gold))
            {
                int bonusBuildingsCount = Physics.OverlapSphere(pos, globalParams.goldBonusRange, Globals.Unit_Mask)
                    .Where(
                        delegate(Collider c)
                        {
                            BuildingManager m = c.GetComponent<BuildingManager>();
                            if (m == null)
                            {
                                return false;
                            }

                            return m.Unit.Owner == playerParams.myPlayerId;
                        }).Count();

                _production[InGameResource.Gold] = globalParams.baseGoldProduction +
                                                   bonusBuildingsCount * globalParams.bonusGoldProductionPerBuilding;
            }

            if (_data.canProduce.Contains(InGameResource.Wood))
            {
                int treeScore = Physics.OverlapSphere(pos, globalParams.woodProductionRange, Globals.Tree_Mask)
                    .Select((c) => globalParams.WoodProductionFunc(Vector3.Distance(pos, c.transform.position))).Sum();
                _production[InGameResource.Wood] = treeScore;
            }

            if (_data.canProduce.Contains(InGameResource.Stone))
            {
                int rockScore = Physics.OverlapSphere(pos, globalParams.stoneProductionRange, Globals.Rock_Mask)
                    .Select((c) => globalParams.stoneProductionFunc(Vector3.Distance(pos, c.transform.position))).Sum();
                _production[InGameResource.Stone] = rockScore;
            }

            return _production;
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
        public Dictionary<InGameResource, int> Production => _production;
        public List<SkillManager> SkillManagers => _skillManagers;
        public int Owner => _owner;
    }
}
