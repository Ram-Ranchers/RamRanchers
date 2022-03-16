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
        public Material MinimapIndicatorMaterial;
        public Transform groundTarget;

        private Camera _camera;
        private RaycastHit _hit;
        private Ray _ray;
        private Vector3 _forwardDir;
        private int _mouseOnScreenBorder;
        private Coroutine _mouseOnScreenCoroutine;
        private float _minimapIndicatorStrokeWidth = 0.1f;
        private Transform _minimapIndicator;
        private Mesh _minimapIndicatorMesh;
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            _mouseOnScreenBorder = -1;
            _mouseOnScreenCoroutine = null;
            PrepareMapIndicator();
            groundTarget.position = Utils.MiddleOfScreenPointToWorld();
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

            FixAltitude();
            ComputeMinimapIndicator(false);
        }

        private void FixAltitude()
        {
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
            ComputeMinimapIndicator(true);
        }

        private void PrepareMapIndicator()
        {
            GameObject g = new GameObject("MinimapIndicator");
            _minimapIndicator = g.transform;
            g.layer = 10;
            _minimapIndicator.position = Vector3.zero;
            _minimapIndicatorMesh = CreateMinimapIndicatorMesh();
            MeshFilter mf = g.AddComponent<MeshFilter>();
            mf.mesh = _minimapIndicatorMesh;
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            mr.material = new Material(MinimapIndicatorMaterial);
            ComputeMinimapIndicator(true);
        }

        private Mesh CreateMinimapIndicatorMesh()
        {
            Mesh m = new Mesh();
            Vector3[] verices = new Vector3[]
            {
                Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero,
                Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero,
            };

            int[] triangles = new int[]
            {
                0, 4, 1, 4, 5, 1,
                0, 2, 6, 6, 4, 0,
                6, 2, 7, 2, 3, 7,
                5, 7, 3, 3, 1, 5
            };
            
            m.vertices = verices;
            m.triangles = triangles;
            return m;
        }

        private void ComputeMinimapIndicator(bool zooming)
        {
            Vector3 middle = Utils.MiddleOfScreenPointToWorld();
            groundTarget.position = middle;
            
            if (zooming)
            {
                Vector3[] viewCorners = Utils.ScreenCornersToWorldPoints();
                float w = viewCorners[1].x - viewCorners[0].x;
                float h = viewCorners[2].z - viewCorners[0].z;

                for (int i = 0; i < 4; i++)
                {
                    viewCorners[i].x -= middle.x;
                    viewCorners[i].z -= middle.z;
                }

                Vector3[] innerCorners = new Vector3[]
                {
                    new Vector3(viewCorners[0].x + _minimapIndicatorStrokeWidth * w, 0f,
                        viewCorners[0].z + _minimapIndicatorStrokeWidth * h),
                    new Vector3(viewCorners[1].x + _minimapIndicatorStrokeWidth * w, 0f,
                        viewCorners[1].z + _minimapIndicatorStrokeWidth * h),
                    new Vector3(viewCorners[2].x + _minimapIndicatorStrokeWidth * w, 0f,
                        viewCorners[2].z + _minimapIndicatorStrokeWidth * h),
                    new Vector3(viewCorners[3].x + _minimapIndicatorStrokeWidth * w, 0f,
                        viewCorners[3].z + _minimapIndicatorStrokeWidth * h)
                };

                Vector3[] allCorners = new Vector3[]
                {
                    viewCorners[0], viewCorners[1], viewCorners[2], viewCorners[3],
                    innerCorners[0], innerCorners[1], innerCorners[2], innerCorners[3]
                };

                for (int i = 0; i < 8; i++)
                {
                    allCorners[i].y = 100f;
                    _minimapIndicatorMesh.vertices = allCorners;
                    _minimapIndicatorMesh.RecalculateNormals();
                    _minimapIndicatorMesh.RecalculateBounds();
                }
            }

            _minimapIndicator.position = middle;
        }

        private void OnEnable()
        {
            EventManager.AddListener("MoveCamera", OnMoveCamera);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener("MoveCamera", OnMoveCamera);
        }

        private void OnMoveCamera(object data)
        {
            Vector3 pos = (Vector3)data;
            float indicatorW = _minimapIndicatorMesh.vertices[1].x - _minimapIndicatorMesh.vertices[0].x;
            float indicatorH = _minimapIndicatorMesh.vertices[2].z - _minimapIndicatorMesh.vertices[0].z;
            pos.x -= indicatorW / 2f;
            pos.z -= indicatorH / 2f;
            Vector3 off = transform.position - Utils.MiddleOfScreenPointToWorld();
            Vector3 newPos = pos + off;
            newPos.y = 100f;
            transform.position = newPos;
            
            FixAltitude();
            ComputeMinimapIndicator(false);
        }
    }
}
