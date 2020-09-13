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
        private GatheringRangeResolver gatheringRangeResolver;
        private global::Map map;
        private ResourceGatheringController resourceGatheringController;
        private TargetStorageProvider targetStorageProvider;
        private Tilemap tilemap;

        private CustomTile targetResource = null;
        private Storage storage = null;

        private void Start()
        {
            movementController = GetComponent<UnitMovementController>();
            tilemap = FindObjectOfType<Tilemap>();
            map = tilemap.GetComponent<global::Map>();
            gatheringRangeResolver = new GatheringRangeResolver(tilemap);
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

            var tileIndex = map.GetCellPosition(targetResource.position);

            var isInGatheringRange =
                gatheringRangeResolver.IsPointInGatheringRange(tileIndex, transform.position, gatheringRange);

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

            var tileIndex = map.GetCellPosition(storage.transform.position);

            var isInStorageRange =
                gatheringRangeResolver.IsPointInGatheringRange(tileIndex, transform.position, gatheringRange);

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
        }
    }
}