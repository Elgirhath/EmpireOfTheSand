using System.Collections;
using System.Linq;
using Build;
using Units.ResourceGathering;
using Units.StateManagement;
using UnityEngine;

namespace Units.Building.StateControllers
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

            foreach (var resource in ctx.construction.requiredResources.Keys)
            {
                var amountToDeliver = Mathf.Min(resourceController.resourceCounts[resource], ctx.construction.requiredResources[resource] - ctx.construction.deliveredResources[resource]);
                ctx.construction.deliveredResources[resource] += amountToDeliver;
                resourceController.resourceCounts[resource] -= amountToDeliver;

                if (!ctx.construction.GetRemainingResourcesToDeliver().Any())
                {
                    ctx.construction.Build();
                    ctx.State = BuildingState.None;
                    yield break;
                }
            }

            if (ShouldRecalculateStorage(ctx.storage, ctx.construction))
            {
                ctx.storage = null; //forces target storage recalculation
            }

            ctx.State = BuildingState.GoingToStorage;
        }

        private bool ShouldRecalculateStorage(Storage currentStorage, Construction construction)
        {
            return currentStorage != null &&
                   (!construction.GetRemainingResourcesToDeliver().ContainsKey(currentStorage.Type) ||
                    currentStorage.Size == 0);
        }
    }
}