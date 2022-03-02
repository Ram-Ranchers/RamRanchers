using UnityEngine;

namespace DecisionMakingAI
{
    public class Building
    {
        private BuildingData _date;
        private Transform _transform;
        private int _currentHealth;

        public Building(BuildingData data)
        {
            _date = data;
            _currentHealth = data.HP;
            
            GameObject g = GameObject.Instantiate(Resources.Load($"Prefabs/RTS/{_date.Code}")) as GameObject;
            _transform = g.transform;
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public string Code => _date.Code;
        public Transform Transform => _transform;
        public int HP { get => _currentHealth; set => _currentHealth = value; }
        public int MaxHp => _date.HP;

        public int DataIndex
        {
            get
            {
                for (int i = 0; i < Globals.Building_Data.Length; i++)
                {
                    if (Globals.Building_Data[i].Code == _date.Code)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }
    }
}
