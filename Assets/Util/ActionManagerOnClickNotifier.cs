using Assets.Unit.Managers;
using UnityEngine;

namespace Assets.Util
{
    public class ActionManagerOnClickNotifier : MonoBehaviour
    {
        public UnitActionManager unitActionManager;
        public bool IsMouseOver { get; private set; }

        private void Update()
        {
            Notify();
        }

        private void Notify()
        {
            if (!IsMouseOver) return;

            for (var i = 0; i <= 2; i++)
            {
                if (Input.GetMouseButtonUp(i))
                {
                    //unitActionManager.NotifyOnMouseButtonUp(i, gameObject);
                }
            }
        }

        private void OnMouseEnter()
        {
            IsMouseOver = true;
        }

        private void OnMouseExit()
        {
            IsMouseOver = false;
        }
    }
}
