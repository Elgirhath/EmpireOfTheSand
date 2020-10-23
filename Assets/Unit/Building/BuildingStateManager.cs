using Assets.Building;
using Assets.Unit.Building.StateControllers;
using Assets.Unit.ResourceGathering;
using Assets.Unit.StateManagement;
using System;
using System.Collections.Generic;

namespace Assets.Unit.Building
{
    public class BuildingStateManager : AbstractStateManager
    {
        internal ConstructionSite targetBuilding;
        internal UnitMovementController movementController;
        internal Storage targetStorage;
        internal TargetStorageProvider targetStorageProvider;
        internal ResourceHolder resourceHolder;

        protected override void OnStart()
        {
            State = BuildingState.None;
            movementController = GetComponent<UnitMovementController>();
            targetStorageProvider = new TargetStorageProvider(transform);
            resourceHolder = GetComponent<ResourceHolder>();
        }

        public override IDictionary<Enum, Type> StateControllerAssignments => new Dictionary<Enum, Type>
        {
            {BuildingState.Building, typeof(BuildStateController)},
            {BuildingState.GoingToConstructionSite, typeof(MoveToConstructionSiteStateController)},
            {BuildingState.GoingToStorage, typeof(MoveToStorageStateController)}
        };

        public void AssignToBuild(ConstructionSite building)
        {
            targetBuilding = building;
            State = BuildingState.GoingToConstructionSite;
        }

        public void CleanCommands()
        {
            State = BuildingState.None;
        }
    }
}