using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Util.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public UnitSelectionManager unitSelectionManager;
        private RectangleLasso lasso;

        private void Start()
        {
            unitSelectionManager = new UnitSelectionManager();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            StartCoroutine(HandleClick(Input.mousePosition));
        }

        private IEnumerator HandleClick(Vector2 startPoint)
        {
            var isRectSelection = false;
            while (!Input.GetMouseButtonUp(0))
            {
                if (Vector2.Distance(startPoint, Input.mousePosition) > 10f)
                {
                    isRectSelection = true;
                }

                if (isRectSelection)
                {
                    if (lasso == null)
                    {
                        lasso = RectangleLasso.Instantiate(startPoint);
                    }

                    lasso.SetEndPos(Input.mousePosition);
                }

                yield return null;
            }

            if (isRectSelection)
            {
                Destroy(lasso.gameObject);
                lasso = null;
                RectangleSelector.HandleRectSelection(startPoint, Input.mousePosition, this);
            }
            else
            {
                PointSelector.HandlePointSelection(startPoint, this);
            }
        }

        public ISet<GameObject> SelectedObjects { get; } = new HashSet<GameObject>();

        public void ClearSelections()
        {
            SelectedObjects.Clear();
            unitSelectionManager.ClearSelections();
        }
    }
}
