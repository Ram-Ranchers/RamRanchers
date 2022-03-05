using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    public class UnitManager : MonoBehaviour
    {
        public GameObject selectionCircle;

        private Transform _canvas;
        private GameObject _healthbar;

        protected BoxCollider _collider;
        protected virtual Unit Unit { get; set; }

        private void Awake()
        {
            _canvas = GameObject.Find("Canvas").transform;
        }

        protected virtual bool IsActive()
        {
            return true;
        }
        
        private void OnMouseDown()
        {
            if (IsActive())
            {
                Select(true, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            }
        }

        private void SelectUtil()
        {
            if (Globals.Selected_Units.Contains(this))
            {
                return;
            }
            Globals.Selected_Units.Add(this);
            selectionCircle.SetActive(true);

            if (_healthbar == null)
            {
                _healthbar = GameObject.Instantiate(Resources.Load("Prefabs/UI/Healthbar")) as GameObject;
                _healthbar.transform.SetParent(_canvas);
                Healthbar h = _healthbar.GetComponent<Healthbar>();
                Rect boundingBox = Utils.GetBoundingBoxOnScreen(transform.Find("Mesh").GetComponent<Renderer>().bounds,
                    Camera.main);
                h.Initialise(transform, boundingBox.height);
                h.SetPosition();
            }
        }
        
        public void Select()
        {
            Select(false, false);
        }
        
        public void Select(bool singleClick, bool holdingShift)
        {
            // Basic case: using the selection box
            if (!singleClick)
            {
                SelectUtil();
                return;
            }
            
            // Single click: check for shift click
            if (!holdingShift)
            {
                List<UnitManager> selectedUnits = new List<UnitManager>(Globals.Selected_Units);
                foreach (UnitManager um in selectedUnits)
                {
                    um.Deselect();
                }
                SelectUtil();
            }
            else
            {
                if (!Globals.Selected_Units.Contains(this))
                {
                    SelectUtil();
                }
                else
                {
                    Deselect();
                }
            }
        }

        public void Deselect()
        {
            if (!Globals.Selected_Units.Contains(this))
            {
                return;
            }

            Globals.Selected_Units.Remove(this);
            selectionCircle.SetActive(false);
            Destroy(_healthbar);
            _healthbar = null;
        }

        public void Initialise(Unit unit)
        {
            _collider = GetComponent<BoxCollider>();
            Unit = unit;
        }
        
        //private bool _selected = false;
        //public bool IsSelected => _selected;

        //public void Attack(Transform target)
        //{
        //    UnitManager um = target.GetComponent<UnitManager>();
        //    if (um == null)
        //    {
        //        return;
        //    }

        //    //um.TakeHit(Unit.Data.attackDamage);
        //}

        //public void TakeHit(int attackPoints)
        //{
        //    //Unit.HP -= attackPoints;
        //    UpdateHealthbar();
        //    //if (Unit.HP <= 0)
        //    //{
        //    //    Die();
        //    //}
        //}

        //private void Die()
        //{
        //    if (_selected)
        //    {
        //        Deselect();
        //    }
        //    
        //    Destroy(gameObject);
        //}

        //private void UpdateHealthbar()
        //{
        //    //if (!healthbar)
        //    //{
        //    //    return;
        //    //}
//
        //    //Transform fill = healthbar.trasform.Find("Fill");
        //    //fill.GetComponent<UnityEngine.UI.Image>().fillAmount = Unit.HP / (float)Unit.MaxHP;
        //}
        //
        //public void Deselect()
        //{
        //    _selected = false;
        //}

        //private void SelectUtil()
        //{
        //    _selected = true;

        //   // if (healthbar == null)
        //   // {
        //   //     UpdateHealthbar();
        //   // }
        //}
    }
}
