using System.Collections.Generic;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Unit.Managers
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        void Start()
        {
            Instance = this;
        }

        public ISet<GameObject> SelectedUnits { get; } = new HashSet<GameObject>();

        public void NotifyUnitSelection(GameObject unit, bool selected)
        {
            if (selected)
            {
                SelectedUnits.Add(unit);
            }
            else
            {
                SelectedUnits.Remove(unit);
            }
        }
    }
}
