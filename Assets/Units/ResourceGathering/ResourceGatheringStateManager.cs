using System;
using System.Collections.Generic;
using Build;
using Map;
using Units.Movement;
using Units.ResourceGathering.StateControllers;
using Units.StateManagement;

namespace Units.ResourceGathering
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

        protected override IDictionary<Enum, Type> StateControllerBindings { get; } = new Dictionary<Enum, Type>
        {
            {ResourceGatheringState.GoingToResource, typeof(MoveToResourceStateController)},
            {ResourceGatheringState.Gathering, typeof(GatheringStateController)},
            {ResourceGatheringState.GoingToStorage, typeof(MoveToStorageStateController)}
        };

        public void AssignToResource(CustomTile tile)
        {
            targetResource = tile;
            State = ResourceGatheringState.GoingToResource;
            storage = null;
            movementController.SetDestination(targetResource.position);
        }

        public void CleanCommands()
        {
            movementController.Stop();
            targetResource = null;
            State = ResourceGatheringState.None;
        }
    }
}