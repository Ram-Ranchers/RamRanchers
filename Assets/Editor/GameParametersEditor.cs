using DecisionMakingAI;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Editor
{
    [CustomEditor(typeof(GameParameters), true)]
    public class GameParametersEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GameParameters parameters = (GameParameters)target;

            EditorGUILayout.LabelField($"Name: {parameters.GetParametersName()}", EditorStyles.boldLabel);

            System.Type ParametersType = parameters.GetType();
            FieldInfo[] fields = ParametersType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(HideInInspector), false))
                {
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical(GUILayout.Width(20f));
                bool hasHeader = System.Attribute.IsDefined(field, typeof(HeaderAttribute), false);
                if (hasHeader)
                {
                    GUILayout.FlexibleSpace();
                }

                if (GUILayout.Button(parameters.ShowsField(field.Name) ? "-" : "+", GUILayout.Width(20f)))
                {
                    parameters.ToggleShowField(field.Name);
                    EditorUtility.SetDirty(parameters);
                    AssetDatabase.SaveAssets();
                }
                
                EditorGUILayout.EndVertical();
                GUILayout.Space(16);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name), true);
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
