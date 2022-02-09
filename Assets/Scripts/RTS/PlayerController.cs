using UnityEngine;
using UnityEngine.AI;

namespace RTS
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        
        public NavMeshAgent playerAgent;
        
        void Awake()
        {
            _camera = Camera.main;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                playerAgent.SetDestination(GetPointUnderCursor());
            }
        }

        private Vector3 GetPointUnderCursor()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitPosition;
            Physics.Raycast(ray, out hitPosition);

            return hitPosition.point;
        }
    }
}
