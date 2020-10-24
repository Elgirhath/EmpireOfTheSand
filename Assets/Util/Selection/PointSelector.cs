using Assets.Unit;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class PointSelector
    {
        public static void HandlePointSelection(Vector2 point, SelectionManager selectionManager)
        {
            var control = Input.GetKey(KeyCode.LeftControl);

            var selected = PointSelect(point);
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

        public static GameObject PointSelect(Vector2 point)
        {
            var ray = Camera.main.ScreenPointToRay(point);
            var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (!hit) return null; // check for map hit

            var selectionHandler = hit.transform.GetComponent<SelectionHandler>();

            return selectionHandler ? selectionHandler.target : hit.transform.gameObject;
        }
    }
}