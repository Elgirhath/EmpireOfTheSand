using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Unit.ResourceGathering
{
    public class ResourceGatheringFlowManager : MonoBehaviour
    {
        public float gatheringRange;

        private UnitMovementController movementController;
        private GatheringRangeResolver gatheringRangeResolver;
        private CellPositionProvider cellPositionProvider;
        private ResourceGatheringController resourceGatheringController;
        private Tilemap tilemap;

        private Vector3? resourcePosition = null;

        private void Start()
        {
            movementController = GetComponent<UnitMovementController>();
            tilemap = FindObjectOfType<Tilemap>();
            cellPositionProvider = tilemap.GetComponent<CellPositionProvider>();
            gatheringRangeResolver = new GatheringRangeResolver(tilemap);
            resourceGatheringController = GetComponent<ResourceGatheringController>();
        }

        private void Update()
        {
            if (resourcePosition == null) return;

            var tileIndex = cellPositionProvider.GetCellPosition(resourcePosition.Value);

            var isInGatheringRange =
                gatheringRangeResolver.IsPointInGatheringRange(tileIndex, transform.position, gatheringRange);

            if (isInGatheringRange && !resourceGatheringController.isGathering)
            {
                movementController.IsMoving = false;
                resourceGatheringController.StartGathering();
            }
        }

        public void SetDestinationResource(Vector3 position)
        {
            resourcePosition = position;
            movementController.SetDestination(resourcePosition.Value);
        }
    }
}