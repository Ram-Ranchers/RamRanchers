using System;
using System.Collections;
using UnityEngine;

namespace DecisionMakingAI
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : MonoBehaviour
    {
        public float translationSpeed = 60f;
        public float altitude = 40f;
        public float zoomSpeed = 30f;
        
        
        private Camera _camera;
        private RaycastHit _hit;
        private Ray _ray;
        private Vector3 _forwardDir;
        private int _mouseOnScreenBorder;
        private Coroutine _mouseOnScreenCoroutine;
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            _mouseOnScreenBorder = -1;
            _mouseOnScreenCoroutine = null;
        }

        private void Update()
        {
            if (_mouseOnScreenBorder >= 0)
            {
                TranslateCamera(_mouseOnScreenBorder);
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    TranslateCamera(0);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    TranslateCamera(1);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    TranslateCamera(2);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    TranslateCamera(3);
                }
            }

            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
            {
                Zoom(Input.mouseScrollDelta.y > 0f ? 1 : -1);
            }
        }

        private void TranslateCamera(int dir)
        {
            if (dir == 0) // Top
            {
                transform.Translate(_forwardDir * Time.deltaTime * translationSpeed, Space.World);
            }
            if (dir == 1) // Right
            {
                transform.Translate(transform.right * Time.deltaTime * translationSpeed);
            }
            if (dir == 2) // Bottom
            {
                transform.Translate(-_forwardDir * Time.deltaTime * translationSpeed, Space.World);
            }
            if (dir == 3) // Left
            {
                transform.Translate(-transform.right * Time.deltaTime * translationSpeed);
            }
            
            // Translate camera at proper altitude: cast a ray to the ground
            // and move up the hit point
            _ray = new Ray(transform.position, Vector3.up * -1000f);
            if (Physics.Raycast(_ray, out _hit, 1000f, Globals.Terrain_Layer_Mask))
            {
                transform.position = _hit.point + Vector3.up * altitude;
            }
        }

        public void OnMouseEnterScreenBorder(int borderIndex)
        {
            _mouseOnScreenCoroutine =  StartCoroutine(SetMouseOnScreenBorder(borderIndex));
        }

        public void OnMouseExitScreenBorder()
        {
            StopCoroutine(_mouseOnScreenCoroutine);
            _mouseOnScreenBorder = -1;
        }

        private IEnumerator SetMouseOnScreenBorder(int borderIndex)
        {
            yield return new WaitForSeconds(0.3f);
            _mouseOnScreenBorder = borderIndex;
        }

        private void Zoom(int zoomDir)
        {
            // Apply zoom
            _camera.orthographicSize += zoomDir * Time.deltaTime * zoomSpeed;
            // Clamp camera distance
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 8f, 26f);
        }
    }
}
