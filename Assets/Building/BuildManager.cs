﻿using Assets.Map;
using Assets.Util;
using UnityEngine;

namespace Assets.Building
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        public Color blueprintColor = Color.white;
        public GameObject constructionSitePrefab;
        private Building prefab = null;
        private Building blueprint = null;

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
            var constructionSiteObj = Instantiate(constructionSitePrefab, blueprint.transform.position, Quaternion.identity);
            var constructionSite = constructionSiteObj.GetComponent<ConstructionSite>();
            constructionSite.buildPrefab = prefab;
            Destroy(blueprint);
            prefab = null;
            blueprint = null;
        }

        public void StartBuilding(Building buildingPrefab)
        {
            var position = CalculateBlueprintPosition();
            prefab = buildingPrefab;
            blueprint = Instantiate(buildingPrefab, position, Quaternion.identity);
            DisableBuilding(blueprint.gameObject);
            Colorize(blueprint.gameObject, blueprintColor);
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