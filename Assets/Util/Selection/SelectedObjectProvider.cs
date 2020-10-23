using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class SelectedObjectProvider
    {
        private static GameObject[] allUnits;

        public static GameObject PointSelect(Vector2 point)
        {
            var ray = Camera.main.ScreenPointToRay(point);
            var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (!hit) return null; // check for map hit

            var selectionHandler = hit.transform.GetComponent<SelectionHandler>();

            return selectionHandler ? selectionHandler.target : hit.transform.gameObject;
        }

        public static IList<GameObject> RectangleSelect(Rect rect)
        {
            if (allUnits == null)
            {
                allUnits = GameObject.FindGameObjectsWithTag("Unit");
            }

            return allUnits.Where(unit => IsUnitInRect(unit, rect)).ToList();
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