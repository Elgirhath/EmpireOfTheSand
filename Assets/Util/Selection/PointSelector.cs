using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Assets.Units;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class PointSelector
    {
        public static void HandlePointSelection(Vector2 point, SelectionManager selectionManager)
        {
            var control = Input.GetKey(KeyCode.LeftControl);

            var selected = PointSelect(point);

            if (selected == null)
            {
                if (control) return;
                selectionManager.ClearSelections();
                return;
            }

            var unit = selected.GetComponent<Unit>();

            if (!control)
            {
                selectionManager.ClearSelections();
            }

            if (unit == null) return; // TODO: select an object which is not an own unit

            if (unit.GetComponent<PlayerProperty>().playerColor !=
                GameManager.Instance.playerColor) return; // TODO: select an enemy unit for lookup

            selectionManager.unitSelectionManager.HandleSelection(unit.GetComponent<UnitSelectionController>(), control);
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