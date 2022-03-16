using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Sound Parameters", menuName = "Scriptable Objects/Game Sound Parameters", order = 11)]
    public class GameSoundParameters : ScriptableObject
    {
        [Header("Ambient Sounds")] 
        public AudioClip onDayStartSound;
        public AudioClip onNightStartSound;
        public AudioClip onBuildingPlacedSound;
    }
}
