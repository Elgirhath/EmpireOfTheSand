using Assets.Building;
using Assets.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Unit.ResourceGathering
{
    public class ResourceGatheringFlowManager : MonoBehaviour
    {
        public float gatheringRange;
        public ResourceGatheringState currentState;

        private UnitMovementController movementController;
        private ResourceGatheringController resourceGatheringController;
        private TargetStorageProvider targetStorageProvider;

        private CustomTile targetResource = null;
        private Storage storage = null;

        private void Start()
        {
            movementController = GetComponent<UnitMovementController>();
            resourceGatheringController = GetComponent<ResourceGatheringController>();
            targetStorageProvider = new TargetStorageProvider(transform);
        }

        private void Update()
        {
            if (currentState == ResourceGatheringState.GoingToResource)
            {
                GoingToResourceController();
            }

            if (currentState == ResourceGatheringState.Gathering)
            {
                GatheringController();
            }

            if (currentState == ResourceGatheringState.GoingToStorage)
            {
                GoingToStorageController();
            }
        }

        private void GoingToResourceController()
        {
            if (targetResource == null) return;

            var tileIndex = GameMap.Instance.GetCellPosition(targetResource.position);

            var isInGatheringRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, transform.position, gatheringRange);

            if (isInGatheringRange && !resourceGatheringController.IsGathering)
            {
                movementController.IsMoving = false;
                resourceGatheringController.StartGathering(targetResource.type);
                currentState = ResourceGatheringState.Gathering;
            }
        }

        private void GatheringController()
        {
            if (resourceGatheringController.IsGathering == false)
            {
                currentState = ResourceGatheringState.GoingToStorage;
            }
        }

        private void GoingToStorageController()
        {
            if (storage == null)
            {
                storage = targetStorageProvider.GetTargetStorage(targetResource.type);
                if (storage == null)
                {
                    Debug.LogWarning("No storage found");
                    currentState = ResourceGatheringState.None;
                    return;
                }
                movementController.SetDestination(storage.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(storage.transform.position);

            var isInStorageRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, transform.position, gatheringRange);

            if (isInStorageRange)
            {
                movementController.IsMoving = false;
                resourceGatheringController.Deliver(storage);
                SetDestinationResource(targetResource);
            }
        }

        public void SetDestinationResource(CustomTile tile)
        {
            targetResource = tile;
            currentState = ResourceGatheringState.GoingToResource;
            storage = null;
            movementController.SetDestination(targetResource.position);
        }

        public void CleanDestinationResource()
        {
            targetResource = null;
            currentState = ResourceGatheringState.None;
        }
    }
}