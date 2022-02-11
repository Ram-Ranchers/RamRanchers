using System;
using UnityEngine;

namespace RTS
{
    public class EnemyManager : MonoBehaviour
    {
        private int _healthpoints;

        private void Awake()
        {
            _healthpoints = 30;
        }

        public bool TakeHit()
        {
            _healthpoints -= 10;
            bool isDead = _healthpoints <= 0;
            if (isDead)
            {
                Die();
            }

            return isDead;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
