using UnityEngine;

namespace Assets.Units
{
    public class UnitSelectionController : MonoBehaviour
    {
        public bool IsSelected { get; private set; } = false;

        public void MarkSelected(bool selected)
        {
            GetComponent<SpriteRenderer>().color = selected ? Color.green : Color.white;
            IsSelected = selected;
        }
    }
}
