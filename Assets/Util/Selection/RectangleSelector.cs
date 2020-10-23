using Assets.Unit;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class RectangleSelector
    {
        public static void HandleRectSelection(Vector2 startPoint, Vector2 endPoint, SelectionManager selectionManager)
        {
            var control = Input.GetKey(KeyCode.LeftControl);

            var x = Mathf.Min(startPoint.x, endPoint.x);
            var y = Mathf.Min(startPoint.y, endPoint.y);
            var width = Mathf.Abs(endPoint.x - startPoint.x);
            var height = Mathf.Abs(endPoint.y - startPoint.y);

            var selectedUnits = SelectedObjectProvider.RectangleSelect(new Rect(x, y, width, height));

            if (!control)
            {
                selectionManager.ClearSelections();
            }

            foreach (var unit in selectedUnits)
            {
                selectionManager.unitSelectionManager.AddToSelection(unit.GetComponent<UnitSelectionController>());
            }
        }
    }
}