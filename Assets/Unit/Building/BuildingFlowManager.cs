using System.Linq;
using Assets.Building;
using Assets.Map;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Unit.Building
{
    public class BuildingFlowManager : MonoBehaviour
    {
        public BuildingState state = BuildingState.None;
        private ConstructionSite targetBuilding;
        private UnitMovementController movementController;
        private Storage targetStorage;
        private TargetStorageProvider targetStorageProvider;
        private ResourceGatheringController resourceGatheringController;

        private void Start()
        {
            movementController = GetComponent<UnitMovementController>();
            targetStorageProvider = new TargetStorageProvider(transform);
            resourceGatheringController = GetComponent<ResourceGatheringController>();
        }

        private void Update()
        {
            if (state == BuildingState.Building)
            {
                Build();
            }

            if (state == BuildingState.GoingToConstructionSite)
            {
                MoveToConstructionSite();
            }

            if (state == BuildingState.GoingToStorage)
            {
                MoveToStorage();
            }
        }

        private void MoveToStorage()
        {
            if (targetStorage == null)
            {
                var resourcesToDeliver = targetBuilding.GetRemainingResourcesToDeliver();
                targetStorage = targetStorageProvider.GetTargetStorage(resourcesToDeliver.Select(kvp => kvp.Key).First());
                if (targetStorage == null)
                {
                    Debug.LogWarning("No storage found");
                    state = BuildingState.None;
                    return;
                }
            }

            if (!movementController.IsMoving)
            {
                movementController.SetDestination(targetStorage.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(targetStorage.transform.position);

            var isInStorageRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, transform.position, 0.3f);

            if (isInStorageRange)
            {
                movementController.IsMoving = false;
                resourceGatheringController.Collect(targetStorage);
                state = BuildingState.GoingToConstructionSite;
            }
        }

        private void MoveToConstructionSite()
        {
            if (targetBuilding == null)
            {
                state = BuildingState.None;
                return;
            }

            if (!movementController.IsMoving)
            {
                movementController.SetDestination(targetBuilding.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(targetBuilding.transform.position);

            var isInInteractionRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, transform.position, 0.3f);

            if (isInInteractionRange)
            {
                movementController.IsMoving = false;
                state = BuildingState.Building;
            }
        }

        private void Build()
        {
            var resourceController = GetComponent<ResourceGatheringController>();
            foreach (var resource in targetBuilding.requiredResources.Keys)
            {
                var amountToDeliver = Mathf.Min(resourceController.resourceCounts[resource], targetBuilding.requiredResources[resource] - targetBuilding.deliveredResources[resource]);
                targetBuilding.deliveredResources[resource] += amountToDeliver;
                resourceController.resourceCounts[resource] -= amountToDeliver;

                if (!targetBuilding.GetRemainingResourcesToDeliver().Any())
                {
                    targetBuilding.Build();
                    state = BuildingState.None;
                    return;
                }
            }

            if (targetStorage != null && !targetBuilding.GetRemainingResourcesToDeliver().ContainsKey(targetStorage.Type))
            {
                targetStorage = null; //forces target storage recalculation
            }

            state = BuildingState.GoingToStorage;
        }

        public void AssignToBuild(ConstructionSite building)
        {
            targetBuilding = building;
            state = BuildingState.GoingToConstructionSite;
        }

        public void CleanCommands()
        {
            state = BuildingState.None;
        }
    }
}