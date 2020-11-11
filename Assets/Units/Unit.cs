using UnityEngine;

namespace Assets.Units
{
    public class Unit : MonoBehaviour
    {
        public void Destroy()
        {
            UnitDataManager.RemoveUnit(this);
            Destroy(gameObject);
        }

        private void Start()
        {
            UnitDataManager.AddUnit(this);
        }
    }
}