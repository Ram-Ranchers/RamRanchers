using UnityEngine;

namespace DecisionMakingAI
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuildingManager : UnitManager
    {
        public AudioSource ambientSource;
        
        private Building _building = null;
        private int _nCollisions = 0;

        public override Unit Unit
        {
            get { return _building; }
            set { _building = value is Building ? (Building)value : null; }
        }
        
        protected override bool IsActive()
        {
            return _building.IsFixed;
        }
        
        public void Initialise(Building building)
        {
            _collider = GetComponent<BoxCollider>();
            _building = building;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Terrain")
            {
                return;
            }

            _nCollisions++;
            CheckPlacement();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Terrain")
            {
                return;
            }

            _nCollisions--;
            CheckPlacement();
        }

        public bool CheckPlacement()
        {
            if (_building == null)
            {
                return false;
            }

            if (_building.IsFixed)
            {
                return false;
            }

            bool validPlacement = HasValidPlacement();

            if (!validPlacement)
            {
                _building.SetMaterials(BuildingPlacement.Invalid);
            }
            else
            {
                _building.SetMaterials(BuildingPlacement.Valid);
            }

            return validPlacement;
        }

        public bool HasValidPlacement()
        {
            if (_nCollisions > 0)
            {
                return false;
            }

            Vector3 p = transform.position;
            Vector3 c = _collider.center;
            Vector3 e = _collider.size / 2f;
            float bottomHeight = c.y - e.y + 0.5f;
            Vector3[] bottomCorners = new Vector3[]
            {
                new Vector3(c.x - e.x, bottomHeight, c.z - e.z),
                new Vector3(c.x - e.x, bottomHeight, c.z + e.z),
                new Vector3(c.x + e.x, bottomHeight, c.z - e.z),
                new Vector3(c.x + e.x, bottomHeight, c.z + e.z),
            };

            int invalidCornersCount = 0;
            foreach (Vector3 corner in bottomCorners)
            {
                if (!Physics.Raycast(p + corner, Vector3.up * -1f, 2f, Globals.Terrain_Layer_Mask))
                {
                    invalidCornersCount++;
                }
            }

            return invalidCornersCount < 3;
        }
    }
}
