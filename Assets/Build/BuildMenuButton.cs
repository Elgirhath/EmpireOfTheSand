using UnityEngine;

namespace Build
{
    public class BuildMenuButton : MonoBehaviour
    {
        public Building buildingPrefab;
        private BuildMenu buildMenu;

        private void Start()
        {
            buildMenu = GetComponentInParent<BuildMenu>();
        }

        public void OnClick()
        {
            BuildManager.Instance.StartBuilding(buildingPrefab);
            buildMenu.Close();
        }
    }
}
