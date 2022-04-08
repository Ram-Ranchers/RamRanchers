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
    public class GamePlayersParameters : GameParameters
    {
        public override string GetParametersName() => "Players";

        public PlayerData[] players;
        public int myPlayerId;
    }
}
