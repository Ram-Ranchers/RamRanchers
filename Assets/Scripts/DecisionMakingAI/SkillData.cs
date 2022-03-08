using UnityEngine;
using UnityEngine.AI;

namespace DecisionMakingAI
{
    public enum SkillType
    {
        Instantiate_Character
    }
    
    [CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill", order = 4)]
    public class SkillData : ScriptableObject
    {
        public string code;
        public string skillName;
        public string description;
        public SkillType type;
        public UnitData unitReference;
        public float castTime;
        public float cooldown;
        public Sprite sprite;

        public void Trigger(GameObject source, GameObject target = null)
        {
            switch (type)
            {
                case SkillType.Instantiate_Character:
                {
                    BoxCollider coll = source.GetComponent<BoxCollider>();
                    Vector3 instantiationPosition = new Vector3(source.transform.position.x - coll.size.x * 0.7f,
                        source.transform.position.y, source.transform.position.z - coll.size.z * -0.7f);
                    CharacterData d = (CharacterData)unitReference;
                    Character c = new Character(d);
                    c.Transform.GetComponent<NavMeshAgent>().Warp(instantiationPosition);
                    c.Transform.GetComponent<CharacterManager>().Initialise(c);
                }
                    break;
                default:
                    break;
            }   
        }
    }
}
