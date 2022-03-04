using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    public enum BuildingPlacement
    {
        Valid,
        Invalid,
        Fixed
    }
    
    public class Building
    {
        private BuildingData _date;
        private Transform _transform;
        private int _currentHealth;
        private BuildingPlacement _placement;
        private List<Material> _materials;
        private BuildingManager _buildingManager;
        
        public Building(BuildingData data)
        {
            _date = data;
            _currentHealth = data.HP;

            GameObject g = GameObject.Instantiate(Resources.Load($"Prefabs/Buildings/{_date.Code}")) as GameObject;
            _transform = g.transform;
            
            _materials = new List<Material>();
            foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
            {
                _materials.Add(new Material(material));
            }
            
            _buildingManager = g.GetComponent<BuildingManager>();
            _placement = BuildingPlacement.Valid;
            SetMaterials();
        }

        public void SetMaterials()
        {
            SetMaterials(_placement);
        }

        public void SetMaterials(BuildingPlacement placement)
        {
            List<Material> materials;
            if (placement == BuildingPlacement.Valid)
            {
                Material refMaterial = Resources.Load("Materials/Valid") as Material;
                materials = new List<Material>();
                for (int i = 0; i < _materials.Count; i++)
                {
                    materials.Add(refMaterial);
                }
            }
            else if (placement == BuildingPlacement.Invalid)
            {
                Material refMaterial = Resources.Load("Materials/Invalid") as Material;
                materials = new List<Material>();
                for (int i = 0; i < _materials.Count; i++)
                {
                    materials.Add(refMaterial);
                }
            }
            else if (placement == BuildingPlacement.Fixed)
            {
                materials = _materials;
            }
            else
            {
                return;
            }

            _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
        }
        
        public void Place()
        {
            _placement = BuildingPlacement.Fixed;

            SetMaterials();
            
            _transform.GetComponent<BoxCollider>().isTrigger = false;

            foreach (KeyValuePair<string, int> pair in _date.Cost)
            {
                Globals.Game_Resources[pair.Key].AddAmount(-pair.Value);
            }
        }

        public bool CanBuy()
        {
            return _date.CanBuy();
        }
        
        public void CheckValidPlacement()
        {
            if (_placement == BuildingPlacement.Fixed)
            {
                return;
            }

            _placement = _buildingManager.CheckPlacement() ? BuildingPlacement.Valid : BuildingPlacement.Invalid;
        }
        
        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public string Code => _date.Code;
        public Transform Transform => _transform;
        public int HP { get => _currentHealth; set => _currentHealth = value; }
        public int MaxHp => _date.HP;
        public bool IsFixed => _placement == BuildingPlacement.Fixed;
        public bool HasValidPlacement => _placement == BuildingPlacement.Valid;
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
