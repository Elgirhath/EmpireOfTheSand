using Map;
using UnityEngine;
using Util;

namespace Build
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        public Color blueprintColor = Color.white;
        public GameObject constructionSitePrefab;

        private Building prefab = null;
        private GameObject blueprint = null;

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            if (blueprint)
            {
                blueprint.transform.position = CalculateBlueprintPosition();
                if (Input.GetMouseButtonDown(0))
                {
                    Confirm();
                }
            }
        }

        private void Confirm()
        {
            var constructionSiteObj = StructureBuildManager.Instance.Build(constructionSitePrefab, blueprint.transform.position, GetComponent<Player>().GetBuildingParent());
            constructionSiteObj.GetComponent<PlayerProperty>().playerColor = GameManager.Instance.playerColor;
            var constructionSite = constructionSiteObj.GetComponent<Construction>();
            constructionSite.buildPrefab = prefab;
            Destroy(blueprint);
            prefab = null;
            blueprint = null;
        }

        public void StartBuilding(Building buildingPrefab)
        {
            var position = CalculateBlueprintPosition();
            prefab = buildingPrefab;
            blueprint = Instantiate(buildingPrefab.gameObject, position, Quaternion.identity); //using GameObject as other components will be removed
            DisableBuilding(blueprint);
            Colorize(blueprint, blueprintColor);
        }

        private static void Colorize(GameObject obj, Color color)
        {
            foreach (var renderer in obj.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.color = color;
            }
        }

        private static Vector3 CalculateBlueprintPosition()
        {
            return GameMap.Instance.SnapToGrid(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
        }

        private static void DisableBuilding(GameObject obj)
        {
            foreach (var component in obj.GetComponentsInChildren<Component>())
            {
                if (component is Transform || component is SpriteRenderer) continue;

                Destroy(component);
            }
        }
    }
}