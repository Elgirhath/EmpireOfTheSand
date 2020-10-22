using System.Linq;
using Assets.Building;
using Assets.Map;
using Assets.Unit.ResourceGathering;
using Assets.Unit.StateManagement;
using UnityEngine;

namespace Assets.Unit.Building.StateControllers
{
    public class MoveToStorageStateController : AbstractStateController
    {
        private new readonly BuildingStateManager context;

        public MoveToStorageStateController(BuildingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            if (context.targetStorage == null)
            {
                var resourcesToDeliver = context.targetBuilding.GetRemainingResourcesToDeliver();
                context.targetStorage = context.targetStorageProvider.GetTargetStorage(resourcesToDeliver.Select(kvp => kvp.Key).First());
                if (context.targetStorage == null)
                {
                    Debug.LogWarning("No storage found");
                    context.State = BuildingState.None;
                    return;
                }
            }

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(context.targetStorage.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(context.targetStorage.transform.position);

            var isInStorageRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, context.transform.position, 0.3f);

            if (isInStorageRange)
            {
                context.movementController.IsMoving = false;
                Collect(context.targetStorage);
                context.State = BuildingState.GoingToConstructionSite;
            }
        }
        private void Collect(Storage storage)
        {
            var amountToCollect = Mathf.Max(Mathf.Min(context.resourceHolder.resourceCapacity[storage.Type] - context.resourceHolder.resourceCounts[storage.Type], storage.Size), 0);
            storage.Size -= amountToCollect;
            context.resourceHolder.resourceCounts[storage.Type] += amountToCollect;
        }
    }
}