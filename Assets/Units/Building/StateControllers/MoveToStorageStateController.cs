﻿using System.Linq;
using Assets.Building;
using Assets.Map;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Building.StateControllers
{
    public class MoveToStorageStateController : AbstractStateController
    {
        private new readonly BuildingStateManager context;
        private TargetStorageProvider storageProvider;

        public MoveToStorageStateController(BuildingStateManager context) : base(context)
        {
            this.context = context;
        }

        internal override void OnStart()
        {
            storageProvider = new TargetStorageProvider(context.transform);
        }

        public override void Execute()
        {
            if (context.storage == null)
            {
                var resourcesToDeliver = context.constructionSite.GetRemainingResourcesToDeliver();
                context.storage = storageProvider.GetTargetStorage(resourcesToDeliver.Select(kvp => kvp.Key).First(), TargetStorageProvider.ActionType.CollectFrom);
                if (context.storage == null)
                {
                    Debug.LogWarning($"No storage found: {resourcesToDeliver.Select(kvp => kvp.Key).First()}");
                    context.State = BuildingState.None;
                    return;
                }
            }

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(context.storage.transform.position);
            }

            if (context.movementController.IsInInteractionRange(context.storage, 0.3f))
            {
                context.movementController.Stop();
                Collect(context.storage);
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