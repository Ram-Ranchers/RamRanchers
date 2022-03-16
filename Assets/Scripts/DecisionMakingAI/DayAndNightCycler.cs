using System.Collections;
using UnityEngine;

namespace DecisionMakingAI
{
    public class DayAndNightCycler : MonoBehaviour
    {
        public Transform starsTransform;

        private float _starsRefreshRate;
        private float _rotationAngleStep;
        private Vector3 _rotationAxis;
        
        private void Start()
        {
            starsTransform.rotation = Quaternion.Euler(GameManager.instance.gameGlobalParameters.dayInitialRatio * 360f, -30f, 0f);
            _starsRefreshRate = 0.1f;
            _rotationAxis = starsTransform.right;
            _rotationAngleStep = 360f * _starsRefreshRate / GameManager.instance.gameGlobalParameters.dayLengthInSeconds;
            
            StartCoroutine("UpdateStars");
        }

        private IEnumerator UpdateStars()
        {
            float rotation = 0f;
            while (true)
            {
                rotation = (rotation + _rotationAngleStep) % 360f;
                starsTransform.Rotate(_rotationAxis, _rotationAngleStep, Space.World);

                if (rotation <= 90f && rotation + _rotationAngleStep > 90f)
                {
                    EventManager.TriggerEvent("PlaySoundByName", "onNightStartSound");
                }

                if (rotation <= 270f && rotation + _rotationAngleStep > 270f)
                {
                    EventManager.TriggerEvent("PlaySoundByName", "onDayStartSound");
                }
                
                yield return new WaitForSeconds(_starsRefreshRate);
            }
        }
    }
}
