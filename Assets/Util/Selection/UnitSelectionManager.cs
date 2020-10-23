using Assets.Unit;
using System.Collections.Generic;

namespace Assets.Util.Selection
{
    public class UnitSelectionManager
    {
        public ISet<UnitSelectionController> SelectedUnits { get; } = new HashSet<UnitSelectionController>();

        public void HandleSelection(UnitSelectionController unit, bool modifySelectionToggle)
        {
            if (modifySelectionToggle)
            {
                if (!unit.IsSelected)
                {
                    AddToSelection(unit);
                }
                else
                {
                    SubtractFromSelection(unit);
                }
            }
            else
            {
                SelectSingleUnit(unit);
            }
        }

        public void ClearSelections()
        {
            foreach (var unit in SelectedUnits)
            {
                unit.MarkSelected(false);
            }
            SelectedUnits.Clear();
        }


        private void SubtractFromSelection(UnitSelectionController unit)
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

        public void AddToSelection(UnitSelectionController unit)
        {
            SelectedUnits.Add(unit);
            unit.MarkSelected(true);
        }
    }
}