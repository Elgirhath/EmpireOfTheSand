using System;
using System.Collections.Generic;
using Assets.Building;
using Assets.Map;
using Assets.Units.Movement;
using Assets.Units.ResourceGathering.StateControllers;
using Assets.Units.StateManagement;

namespace Assets.Units.ResourceGathering
{
    public class ResourceGatheringStateManager : AbstractStateManager
    {
        public float gatheringRangeInTilemapCoordinates;
        public float gatheringFrequency;

        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;

        internal CustomTile targetResource;
        internal Storage storage;

        protected override void OnStart()
        {
            movementController = GetComponent<UnitMovementController>();
            resourceHolder = GetComponent<ResourceHolder>();
            State = ResourceGatheringState.None;
        }

        public override IDictionary<Enum, Type> StateControllerAssignments { get; } = new Dictionary<Enum, Type>
        {
            {ResourceGatheringState.GoingToResource, typeof(MoveToResourceStateController)},
            {ResourceGatheringState.Gathering, typeof(GatheringStateController)},
            {ResourceGatheringState.GoingToStorage, typeof(MoveToStorageStateController)}
        };

        public void SetDestinationResource(CustomTile tile)
        {
            targetResource = tile;
            State = ResourceGatheringState.GoingToResource;
            storage = null;
            movementController.SetDestination(targetResource.position);
        }

        public void CleanDestinationResource()
        {
            targetResource = null;
            State = ResourceGatheringState.None;
        }
    }
}