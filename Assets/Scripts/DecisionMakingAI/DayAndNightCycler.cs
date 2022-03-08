using System.Collections;
using UnityEngine;

namespace DecisionMakingAI
{
    public class DayAndNightCycler : MonoBehaviour
    {
        public GameParameters gameParameters;
        public Transform starsTransform;

        private float _starsRefreshRate;
        private float _rotationAngleStep;
        private Vector3 _rotationAxis;

        private void Awake()
        {
            starsTransform.rotation = Quaternion.Euler(gameParameters.dayInitialRatio * 360f, -30f, 0f);
            _starsRefreshRate = 0.1f;
            _rotationAxis = starsTransform.right;
            _rotationAngleStep = 360f * _starsRefreshRate / gameParameters.dayLengthInSeconds;
        }

        private void Start()
        {
            StartCoroutine("UpdateStars");
        }

        private IEnumerator UpdateStars()
        {
            while (true)
            {
                starsTransform.Rotate(_rotationAxis, _rotationAngleStep, Space.World);
                yield return new WaitForSeconds(_starsRefreshRate);
            }
        }
    }
}
