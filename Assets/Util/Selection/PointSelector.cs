using Assets.Unit;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class PointSelector
    {
        public static void HandlePointSelection(Vector2 point, SelectionManager selectionManager)
        {
            var control = Input.GetKey(KeyCode.LeftControl);

            var selected = SelectedObjectProvider.PointSelect(point);
            if (!selected)
            {
                if (!control)
                {
                    selectionManager.ClearSelections();
                }
                return;
            }

            var unitSelectionController = selected.GetComponent<UnitSelectionController>();
            if (unitSelectionController)
            {
                selectionManager.unitSelectionManager.HandleSelection(unitSelectionController, control);
            }
        }
    }
}