using UnityEngine;

namespace DecisionMakingAI
{
    [System.Serializable]
    public struct PlayerData
    {
        public string name;
        public Color colour;
    }
    
    [CreateAssetMenu(fileName = "Players Parameters", menuName = "Scriptable Objects/Game Players Parameters", order = 12)]
    public class GamePlayersParameters : ScriptableObject
    {
        public PlayerData[] players;
        public int myPlayerId;
    }
}
