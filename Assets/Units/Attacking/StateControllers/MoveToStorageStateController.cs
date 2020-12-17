using Build;
using Map;
using Units.Interaction;
using Units.ResourceGathering;
using Units.StateManagement;
using UnityEngine;

namespace Units.Attacking.StateControllers
{
    public class MoveToStorageStateController : AbstractStateController
    {
        private new readonly AttackingStateManager context;
        private TargetStorageProvider storageProvider;
        private Storage storage;

        public MoveToStorageStateController(AttackingStateManager context) : base(context)
        {
            this.context = context;
        }

        internal override void OnStart()
        {
            storageProvider = new TargetStorageProvider(context.transform);
        }

        public override void Execute()
        {
            if (storage == null)
            {
                storage = storageProvider.GetTargetStorage(TileType.Water, TargetStorageProvider.ActionType.CollectFrom);
                if (storage == null)
                {
                    Debug.LogWarning($"No storage found: Water");
                    context.State = AttackingState.None;
                    return;
                }
            }

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(storage.transform.position);
            }

            if (context.movementController.IsInInteractionRange(storage.tile, 0.3f))
            {
                context.movementController.Stop();
                Collect(storage);
                context.State = AttackingState.MovingToEnemy;
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