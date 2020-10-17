using Assets.Map;
using Assets.Util;
using UnityEngine;

namespace Assets.Building
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        public Color blueprintColor = Color.white;
        public GameObject constructionSitePrefab;
        private IBuilding prefab = null;
        private GameObject blueprint = null;

        private void Start()
        {
            Instance = this;
        }

        void Update()
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
            var constructionSiteObj = Instantiate(constructionSitePrefab, blueprint.transform.position, Quaternion.identity);
            var constructionSite = constructionSiteObj.GetComponent<ConstructionSite>();
            constructionSite.buildPrefab = prefab;
            Destroy(blueprint);
            prefab = null;
            blueprint = null;
        }

        public void StartBuilding(GameObject buildingPrefab)
        {
            var position = CalculateBlueprintPosition();
            prefab = buildingPrefab;
            blueprint = Instantiate(buildingPrefab, position, Quaternion.identity);
            DisableBuilding(blueprint);
            Colorize(blueprint, blueprintColor);
        }

        private void Colorize(GameObject obj, Color color)
        {
            foreach (var renderer in obj.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.color = color;
            }
        }

        private Vector3 CalculateBlueprintPosition()
        {
            return GameMap.Instance.SnapToGrid(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
        }

        private void DisableBuilding(GameObject obj)
        {
            foreach (var component in obj.GetComponentsInChildren<Component>())
            {
                if (component is Transform || component is SpriteRenderer) continue;

                Destroy(component);
            }
        }
    }
}