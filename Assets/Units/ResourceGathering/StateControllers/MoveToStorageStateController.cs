using Build;
using Units.Interaction;
using UnityEngine;

namespace Units.ResourceGathering.StateControllers
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
                context.storage = storageProvider.GetTargetStorage(context.targetResource.type, TargetStorageProvider.ActionType.DeliverTo);
                if (context.storage == null)
                {
                    Debug.LogWarning("No storage found");
                    context.State = ResourceGatheringState.None;
                    return;
                }
                context.movementController.SetDestination(context.storage.transform.position);
            }

            if (context.movementController.IsInInteractionRange(context.storage, context.gatheringRangeInTilemapCoordinates))
            {
                context.movementController.Stop();
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