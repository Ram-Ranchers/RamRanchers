using UnityEngine;

namespace DecisionMakingAI
{
    public class UnitManager : MonoBehaviour
    {
        private bool _selected = false;
        public bool IsSelected => _selected;

        public void Attack(Transform target)
        {
            UnitManager um = target.GetComponent<UnitManager>();
            if (um == null)
            {
                return;
            }

            //um.TakeHit(Unit.Data.attackDamage);
        }

        public void TakeHit(int attackPoints)
        {
            //Unit.HP -= attackPoints;
            UpdateHealthbar();
            //if (Unit.HP <= 0)
            //{
            //    Die();
            //}
        }

        private void Die()
        {
            if (_selected)
            {
                Deselect();
            }
            
            Destroy(gameObject);
        }

        private void UpdateHealthbar()
        {
            //if (!healthbar)
            //{
            //    return;
            //}
//
            //Transform fill = healthbar.trasform.Find("Fill");
            //fill.GetComponent<UnityEngine.UI.Image>().fillAmount = Unit.HP / (float)Unit.MaxHP;
        }
        
        public void Deselect()
        {
            _selected = false;
        }

        private void SelectUtil()
        {
            _selected = true;

           // if (healthbar == null)
           // {
           //     UpdateHealthbar();
           // }
        }
    }
}
