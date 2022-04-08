using DecisionMakingAI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(PlayerData))]
    public class PlayerDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            label.text = label.text.Replace("Element", "Player");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float fullWidth = EditorGUIUtility.labelWidth;
            float nameWidth = fullWidth * 0.7f;
            float colourWidth = fullWidth * 0.3f;
            Rect nameRect = new Rect(position.x, position.y, nameWidth, position.height);
            Rect colourRect = new Rect(position.x + nameWidth + 5, position.y, colourWidth, position.height);

            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(colourRect, property.FindPropertyRelative("colour"), GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
