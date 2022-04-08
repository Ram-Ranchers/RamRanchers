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
    
    public class Building : Unit
    {
        private BuildingPlacement _placement;
        private List<Material> _materials;
        private BuildingManager _buildingManager;

        public Building(BuildingData data, int owner) : this(data, owner, new List<ResourceValue>() { })
        {
        }


        public Building(BuildingData data, int owner, List<ResourceValue> production) : base(data, owner, production)
        {
            _buildingManager = _transform.GetComponent<BuildingManager>();
            _materials = new List<Material>();
            foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
            {
                _materials.Add(new Material(material));
            }
            
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
        
        public override void Place()
        {
            base.Place();
            _placement = BuildingPlacement.Fixed;
            SetMaterials();
        }
        
        public void CheckValidPlacement()
        {
            if (_placement == BuildingPlacement.Fixed)
            {
                return;
            }

            _placement = _buildingManager.CheckPlacement() ? BuildingPlacement.Valid : BuildingPlacement.Invalid;
        }

        public bool IsFixed => _placement == BuildingPlacement.Fixed;
        public bool HasValidPlacement => _placement == BuildingPlacement.Valid;
        public int DataIndex
        {
            get
            {
                for (int i = 0; i < Globals.Building_Data.Length; i++)
                {
                    if (Globals.Building_Data[i].code == _data.code)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }
    }
}
