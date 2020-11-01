using System.Collections;
using Assets.Map;
using UnityEngine;

namespace Assets.Units.ResourceGathering.StateControllers
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
            context.movementController.Stop();

            if (context.targetResource != null && context.resourceHolder.resourceCounts[resourceType] < context.resourceHolder.maxResourceCount)
            {
                yield return new WaitForSeconds(context.gatheringFrequency);
                context.resourceHolder.resourceCounts[resourceType]++;
            }

            context.State = ResourceGatheringState.GoingToStorage;
        }
    }
}