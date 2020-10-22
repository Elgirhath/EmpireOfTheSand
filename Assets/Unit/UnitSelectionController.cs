using Assets.Unit.Managers;
using UnityEngine;

namespace Assets.Unit
{
    public class UnitSelectionController : MonoBehaviour
    {
        private bool isSelected = false;

        private void OnMouseUp()
        {
            var selectionManager = UnitSelectionManager.Instance;
            selectionManager.HandleSelection(this, !isSelected);
        }

        public void MarkSelected(bool selected)
        {
            GetComponent<SpriteRenderer>().color = selected ? Color.green : Color.white;
            isSelected = selected;
        }
    }
}
