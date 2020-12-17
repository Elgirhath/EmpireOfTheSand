using System.Collections.Generic;
using Build;
using Units;
using Units.Building;
using Units.Managers;
using UnityEngine;
using Util;

namespace Ai
{
    public class AiBuildingManager : MonoBehaviour
    {
        public Construction constructionPrefab;

        private PlayerColor playerColor;

        private void Start()
        {
            playerColor = GetComponent<AiManager>().playerColor;
        }

        public void Build(Building prefab, Vector2Int position)
        {
            var constructionSite = StructureBuildManager.Instance.Build(constructionPrefab, (Vector3Int)position, GetComponent<Player>().GetBuildingParent());
            constructionSite.GetComponent<PlayerProperty>().playerColor = playerColor;
            constructionSite.buildPrefab = prefab;

            var unitToAssign = GetClosestFreeUnit(constructionSite.Position);

            if (unitToAssign == null)
            {
                BuildAssignmentManager.CreateUnassigned(constructionSite);
                return;
            }

            UnitActionManager.CleanCommands(unitToAssign.gameObject);

            var buildingStateManager = unitToAssign.GetComponent<BuildingStateManager>();

            if (!BuildAssignmentManager.IsUnitAssigned(buildingStateManager))
            {
                buildingStateManager.AssignToBuild(constructionSite);
                BuildAssignmentManager.AssignUnitToBuild(constructionSite, buildingStateManager);
            }
        }

        private Unit GetClosestFreeUnit(Vector2 position)
        {
            var units = UnitManager.GetFreeUnits(playerColor);

            var minDistance = Mathf.Infinity;
            Unit closestUnit = null;

            foreach (var unit in units)
            {
                var distance = Vector2.Distance(unit.transform.position, position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestUnit = unit;
                }
            }

            return closestUnit;
        }
    }
}