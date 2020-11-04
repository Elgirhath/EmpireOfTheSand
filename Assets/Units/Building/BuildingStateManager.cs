using System;
using System.Collections.Generic;
using Assets.Building;
using Assets.Units.Building.StateControllers;
using Assets.Units.Movement;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;

namespace Assets.Units.Building
{
    public class BuildingStateManager : AbstractStateManager
    {
        internal ConstructionSite constructionSite;
        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;
        internal Storage storage;

        protected override void OnStart()
        {
            State = BuildingState.None;
            movementController = GetComponent<UnitMovementController>();
            resourceHolder = GetComponent<ResourceHolder>();
        }

        public override IDictionary<Enum, Type> StateControllerBindings => new Dictionary<Enum, Type>
        {
            {BuildingState.Building, typeof(BuildStateController)},
            {BuildingState.GoingToConstructionSite, typeof(MoveToConstructionSiteStateController)},
            {BuildingState.GoingToStorage, typeof(MoveToStorageStateController)}
        };

        public void AssignToBuild(ConstructionSite building)
        {
            constructionSite = building;
            State = BuildingState.GoingToConstructionSite;
        }

        public void CleanCommands()
        {
            State = BuildingState.None;
        }
    }
}