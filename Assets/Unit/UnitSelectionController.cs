using Assets.Unit.Managers;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Unit
{
    public class UnitSelectionController : MonoBehaviour
    {
        private bool isSelected = false;

        private void OnMouseUp()
        {
            MarkSelected(!isSelected);
        }

        public void MarkSelected(bool selected)
        {
            if (selected == isSelected) return;

            var selectionManager = UnitSelectionManager.Instance;
            selectionManager.NotifyUnitSelection(gameObject, selected);

            GetComponent<SpriteRenderer>().color = Color.green;
            isSelected = selected;
        }
    }
}
