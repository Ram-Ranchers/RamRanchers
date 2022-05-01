using UnityEngine;

namespace DecisionMakingAI
{
    public static class Utils
    {
        private static Texture2D _whiteTexture;

        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawScreenRect(Rect rect, Color colour)
        {
            GUI.color = colour;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color colour)
        {
            // Top
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), colour);
            // Left
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), colour);
            // Right
            Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), colour);
            // Bottom
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), colour);
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
        {
            var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
            var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;

            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }

        public static Rect GetBoundingBoxOnScreen(Bounds bounds, Camera camera)
        {
            // Get the 8 vertices of the bounding box
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            Vector3[] vertices = new Vector3[]
            {
                center + Vector3.right * size.x / 2f + Vector3.up * size.y / 2f + Vector3.forward * size.z / 2f,
                center + Vector3.right * size.x / 2f + Vector3.up * size.y / 2f - Vector3.forward * size.z / 2f,
                center + Vector3.right * size.x / 2f - Vector3.up * size.y / 2f + Vector3.forward * size.z / 2f,
                center + Vector3.right * size.x / 2f - Vector3.up * size.y / 2f - Vector3.forward * size.z / 2f,
                center - Vector3.right * size.x / 2f + Vector3.up * size.y / 2f + Vector3.forward * size.z / 2f,
                center - Vector3.right * size.x / 2f + Vector3.up * size.y / 2f - Vector3.forward * size.z / 2f,
                center - Vector3.right * size.x / 2f - Vector3.up * size.y / 2f + Vector3.forward * size.z / 2f,
                center - Vector3.right * size.x / 2f - Vector3.up * size.y / 2f - Vector3.forward * size.z / 2f,
            };
            Rect retVal = Rect.MinMaxRect(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
            
            // Iterate through the vertices to get the equivalent screen projection
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = camera.WorldToScreenPoint(vertices[i]);
                if (v.x < retVal.xMin)
                {
                    retVal.xMin = v.x;
                }
                if (v.y < retVal.yMin)
                {
                    retVal.yMin = v.y;
                }
                if (v.x > retVal.xMax)
                {
                    retVal.xMax = v.x;
                }
                if (v.y > retVal.yMax)
                {
                    retVal.yMax = v.y;
                }
            }

            return retVal;
        }

        // This is to assign the number keys to the keys on the keyboard for grouping units together 
        public static int GetAlphaKeyValue(string inputString)
        {
            if (inputString == "0")
            {
                return 0;
            }
            if (inputString == "1")
            {
                return 1;
            }
            if (inputString == "2")
            {
                return 2;
            }
            if (inputString == "3")
            {
                return 3;
            }
            if (inputString == "4")
            {
                return 4;
            }
            if (inputString == "5")
            {
                return 5;
            }
            if (inputString == "6")
            {
                return 6;
            }
            if (inputString == "7")
            {
                return 7;
            }
            if (inputString == "8")
            {
                return 8;
            }
            if (inputString == "9")
            {
                return 9;
            }

            return -1;
        }

        public static Vector3 MiddleOfScreenPointToWorld()
        {
            return MiddleOfScreenPointToWorld(Camera.main);
        }

        public static Vector3 MiddleOfScreenPointToWorld(Camera camera)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(0.5f * new Vector2(Screen.width, Screen.height));
            if (Physics.Raycast(ray, out hit, 1000f, Globals.Terrain_Layer_Mask))
            {
                return hit.point;
            }
            
            return Vector3.zero;
        }

        public static Vector3[] ScreenCornersToWorldPoints()
        {
            return ScreenCornersToWorldPoints(Camera.main);
        }

        public static Vector3[] ScreenCornersToWorldPoints(Camera cam)
        {
            Vector3[] corners = new Vector3[4];
            RaycastHit hit;

            for (int i = 0; i < 4; i++)
            {
                Ray ray = cam.ScreenPointToRay(new Vector2((i % 2) * Screen.width, (int)(i / 2) * Screen.height));
                
                if (Physics.Raycast(ray, out hit, 1000f, Globals.Flat_Terrain_Layer_Mask))
                {
                    corners[i] = hit.point;
                }
            }

            return corners;
        }
    }
}
