using Assets.Unit.ResourceGathering;
using Assets.Unit.StateManagement;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Unit.Building.StateControllers
{
    public class BuildStateController : AbstractStateController
    {
        private new readonly BuildingStateManager context;

        public BuildStateController(BuildingStateManager context) : base(context)
        {
            this.context = context;
        }

        private Coroutine coroutine = null;

        public override void Execute()
        {
            if (coroutine == null)
            {
                coroutine = context.StartCoroutine(BuildCoroutine(context, 1f));
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

        private IEnumerator BuildCoroutine(BuildingStateManager ctx, float buildDelay)
        {
            var resourceController = ctx.GetComponent<ResourceHolder>();

            yield return new WaitForSeconds(buildDelay);

            foreach (var resource in ctx.targetBuilding.requiredResources.Keys)
            {
                var amountToDeliver = Mathf.Min(resourceController.resourceCounts[resource], ctx.targetBuilding.requiredResources[resource] - ctx.targetBuilding.deliveredResources[resource]);
                ctx.targetBuilding.deliveredResources[resource] += amountToDeliver;
                resourceController.resourceCounts[resource] -= amountToDeliver;

                if (!ctx.targetBuilding.GetRemainingResourcesToDeliver().Any())
                {
                    ctx.targetBuilding.Build();
                    ctx.State = BuildingState.None;
                    yield break;
                }
            }

            if (ctx.targetStorage != null && !ctx.targetBuilding.GetRemainingResourcesToDeliver().ContainsKey(ctx.targetStorage.Type))
            {
                ctx.targetStorage = null; //forces target storage recalculation
            }

            ctx.State = BuildingState.GoingToStorage;
        }
    }
}