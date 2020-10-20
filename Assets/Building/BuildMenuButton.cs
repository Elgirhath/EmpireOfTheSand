using UnityEngine;

namespace Assets.Building
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
            Debug.Log("Start Building");
            BuildManager.Instance.StartBuilding(buildingPrefab);
            buildMenu.Close();
        }
    }
}
