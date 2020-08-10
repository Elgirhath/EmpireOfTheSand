using System.Collections.Generic;
using UnityEngine;

namespace Assets.Unit
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        void Start()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                var destPosition = Input.mousePosition;
                destPosition.z = 0f;
                MoveUnits(Camera.main.ScreenToWorldPoint(destPosition));
            }
        }

        void MoveUnits(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
                unit.SetDestination(position);
            }
        }

        private ISet<UnitMovementController> selectedUnits = new HashSet<UnitMovementController>();

        public void NotifyUnitSelection(UnitMovementController unit, bool selected)
        {
            if (selected)
            {
                selectedUnits.Add(unit);
            }
            else
            {
                selectedUnits.Remove(unit);
            }
            Debug.Log(selectedUnits.Count);
        }
    }
}
