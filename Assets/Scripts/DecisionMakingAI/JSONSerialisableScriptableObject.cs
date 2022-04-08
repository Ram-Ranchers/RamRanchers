using UnityEngine;
using System.IO;

namespace DecisionMakingAI
{
    public class JSONSerialisableScriptableObject : ScriptableObject
    {
        #if UNITY_EDITOR
        private static string _scriptableObjectDataDirectory = "ScriptableObjects_Dev";
        #else
        private static string _scriptableObjectDataDirectory = "ScriptableObjects";
        #endif
        
        public void SaveToFile()
        {
            string dirPath = System.IO.Path.Combine(Application.persistentDataPath, _scriptableObjectDataDirectory);
            string filePath = System.IO.Path.Combine(dirPath, $"{name}.json");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            string json = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile()
        {
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, _scriptableObjectDataDirectory,
                $"{name}.json");

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"File \"{filePath}\" not found! Getting default values.", this);
                return;
            }

            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
