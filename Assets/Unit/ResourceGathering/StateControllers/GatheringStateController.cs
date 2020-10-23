﻿using Assets.Map;
using System.Collections;
using UnityEngine;

namespace Assets.Unit.ResourceGathering.StateControllers
{
    public class GatheringStateController : BaseResourceGatheringStateController
    {
        public GatheringStateController(ResourceGatheringStateManager context) : base(context)
        {
        }

        private Coroutine coroutine;
        private TileType resourceType;

        public override void Execute()
        {
            resourceType = context.targetResource.type;

            if (coroutine == null)
            {
                coroutine = context.StartCoroutine(Gather());
            }
        }

        internal override void Dispose()
        {
            if (coroutine != null)
            {
                context.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator Gather()
        {
            context.movementController.IsMoving = false;

            if (context.targetResource != null && context.resourceHolder.resourceCounts[resourceType] < context.resourceHolder.maxResourceCount)
            {
                yield return new WaitForSeconds(context.gatheringFrequency);
                context.resourceHolder.resourceCounts[resourceType]++;
            }

            context.State = ResourceGatheringState.GoingToStorage;
        }
    }
}