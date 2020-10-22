using System.Collections.Generic;
using UnityEngine;

namespace Assets.Unit.Managers
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

        public ISet<UnitSelectionController> SelectedUnits { get; } = new HashSet<UnitSelectionController>();

        public void HandleSelection(UnitSelectionController unit, bool selected)
        {
            if (selected)
            {
                SelectSingleUnit(unit);
            }
            else
            {
                DeselectUnit(unit);
            }
        }

        private void ClearSelections()
        {
            foreach (var unit in SelectedUnits)
            {
                unit.MarkSelected(false);
            }
            SelectedUnits.Clear();
        }

        private void DeselectUnit(UnitSelectionController unit)
        {
            SelectedUnits.Remove(unit);
            unit.MarkSelected(false);
        }

        private void SelectSingleUnit(UnitSelectionController unit)
        {
            ClearSelections();
            SelectedUnits.Add(unit);
            unit.MarkSelected(true);
        }
    }
}
