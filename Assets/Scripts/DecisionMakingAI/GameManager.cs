using System;
using UnityEngine;

namespace DecisionMakingAI
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            DataHandler.LoadGameData();
        }

        //public float producingRate = 3f;

       //public void Start()
       //{
       //    instance = this;
       //}

       //private void OnPauseGame()
       //{
       //    gameIsPaused = true;
       //}

       //private void OnResumeGame()
       //{
       //    gameIsPaused = false;
       //}
    }
}
