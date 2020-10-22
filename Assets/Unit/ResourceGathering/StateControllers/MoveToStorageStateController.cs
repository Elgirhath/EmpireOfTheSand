using Assets.Building;
using Assets.Map;
using UnityEngine;

namespace Assets.Unit.ResourceGathering.StateControllers
{
    public class MoveToStorageStateController : BaseResourceGatheringStateController
    {
        public MoveToStorageStateController(ResourceGatheringStateManager context) : base(context)
        {
        }

        private TargetStorageProvider storageProvider;

        internal override void OnStart()
        {
            storageProvider = new TargetStorageProvider(context.transform);
        }

        public override void Execute()
        {
            if (context.storage == null)
            {
                context.storage = storageProvider.GetTargetStorage(context.targetResource.type);
                if (context.storage == null)
                {
                    Debug.LogWarning("No storage found");
                    context.State = ResourceGatheringState.None;
                    return;
                }
                context.movementController.SetDestination(context.storage.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(context.storage.transform.position);

            var isInStorageRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, context.transform.position, context.gatheringRange);

            if (isInStorageRange)
            {
                context.movementController.IsMoving = false;
                Deliver(context.storage);
                context.State = ResourceGatheringState.GoingToResource;
            }
        }

        public void Deliver(Storage storage)
        {
            var amountToDeliver = Mathf.Min(context.resourceHolder.resourceCounts[storage.Type], storage.Capacity - storage.Size);
            storage.Size += amountToDeliver;
            context.resourceHolder.resourceCounts[storage.Type] -= amountToDeliver;
        }
    }
}