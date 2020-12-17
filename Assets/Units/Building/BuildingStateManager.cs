using System;
using System.Collections.Generic;
using Build;
using Units.Building.StateControllers;
using Units.Movement;
using Units.ResourceGathering;
using Units.StateManagement;

namespace Units.Building
{
    public class BuildingStateManager : AbstractStateManager
    {
        internal Construction construction;
        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;
        internal Storage storage;

        protected override void OnStart()
        {
            State = BuildingState.None;
            movementController = GetComponent<UnitMovementController>();
            resourceHolder = GetComponent<ResourceHolder>();
        }

        protected override IDictionary<Enum, Type> StateControllerBindings => new Dictionary<Enum, Type>
        {
            {BuildingState.Building, typeof(BuildStateController)},
            {BuildingState.GoingToConstructionSite, typeof(MoveToConstructionSiteStateController)},
            {BuildingState.GoingToStorage, typeof(MoveToStorageStateController)}
        };

        public void AssignToBuild(Construction building)
        {
            construction = building;
            State = BuildingState.GoingToConstructionSite;
        }

        public void CleanCommands()
        {
            State = BuildingState.None;
        }
    }
}