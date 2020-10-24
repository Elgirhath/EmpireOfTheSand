using System.Collections.Generic;
using System.Linq;
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

            var selectedUnits = RectangleSelect(new Rect(x, y, width, height));

            if (!control)
            {
                selectionManager.ClearSelections();
            }

            foreach (var unit in selectedUnits)
            {
                selectionManager.unitSelectionManager.AddToSelection(unit.GetComponent<UnitSelectionController>());
            }
        }

        private static IList<GameObject> RectangleSelect(Rect rect)
        {
            return GameObject.FindGameObjectsWithTag("Unit").Where(unit => IsUnitInRect(unit, rect)).ToList();
        }


        private static bool IsUnitInRect(GameObject unit, Rect rect)
        {
            var colliderBounds = unit.GetComponent<Collider2D>().bounds;
            var startScreenSpace = Camera.main.WorldToScreenPoint(colliderBounds.min);
            var endScreenSpace = Camera.main.WorldToScreenPoint(colliderBounds.max);

            if (endScreenSpace.x < rect.xMin) return false;
            if (endScreenSpace.y < rect.yMin) return false;
            if (startScreenSpace.x > rect.xMax) return false;
            if (startScreenSpace.y > rect.yMax) return false;

            return true;
        }
    }
}