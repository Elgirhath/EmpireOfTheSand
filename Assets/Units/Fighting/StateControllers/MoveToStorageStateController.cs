using Assets.Building;
using Assets.Map;
using Assets.Units.Building;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Fighting.StateControllers
{
    public class MoveToStorageStateController : AbstractStateController
    {
        private new readonly FightingStateManager context;
        private TargetStorageProvider storageProvider;
        private Storage storage;

        public MoveToStorageStateController(FightingStateManager context) : base(context)
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
                    context.State = FightingState.None;
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
                context.State = FightingState.MovingToEnemy;
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