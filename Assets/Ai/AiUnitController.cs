using System;
using System.Linq;
using Map;
using Units;
using Units.Building;
using Units.Managers;
using Units.ResourceGathering;
using Units.StateManagement;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Ai
{
    public class AiUnitController : MonoBehaviour
    {
        private BuildingStateManager buildingStateManager;
        private ResourceGatheringStateManager resourceGatheringStateManager;
        private ResourceManager resourceManager;

        private void Start()
        {
            buildingStateManager = GetComponent<BuildingStateManager>();
            resourceGatheringStateManager = GetComponent<ResourceGatheringStateManager>();
            resourceManager = GetComponentInParent<ResourceManager>();
        }

        private bool IsGathering =>
            !Equals(resourceGatheringStateManager.State, ResourceGatheringState.None);

        public bool IsBuilding => !Equals(buildingStateManager.State, BuildingState.None);

        private CustomTile GetResourceToGather(TileType resourceType = TileType.Default)
        {
            var minDist = float.PositiveInfinity;
            CustomTile resource = null;

            if (resourceType == TileType.Default)
            {
                resourceType = Random.value > 0.5f ? TileType.Sand : TileType.Water;
            }

            foreach (CustomTile cell in GameMap.Instance.Matrix)
            {
                if (cell.type != resourceType) continue;

                var distance = Vector2.Distance(cell.position, transform.position);
                if (distance < minDist)
                {
                    resource = cell;
                    minDist = distance;
                }
            }

            return resource;
        }

        private void Update()
        {
            if (IsBuilding) return;

            var construction = BuildAssignmentManager.GetUnassignedConstructionSites().ArgMin(building => Vector2.Distance(building.Position, transform.position));

            if (construction != null && resourceManager.AreRequiredResourcesAvailable(construction))
            {
                UnitActionManager.CleanCommands(gameObject);
                buildingStateManager.AssignToBuild(construction);
                BuildAssignmentManager.AssignUnitToBuild(construction, buildingStateManager);
                return;
            }

            if (IsGathering) return;

            var resource = construction == null ? GetResourceToGather() : GetResourceToGather(construction.GetRemainingResourcesToDeliver().First().Key);

            UnitActionManager.CleanCommands(gameObject);
            GetComponent<ResourceGatheringStateManager>().AssignToResource(resource);
        }
    }
}