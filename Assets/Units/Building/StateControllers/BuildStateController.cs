using System.Collections;
using System.Linq;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Building.StateControllers
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

            foreach (var resource in ctx.constructionSite.requiredResources.Keys)
            {
                var amountToDeliver = Mathf.Min(resourceController.resourceCounts[resource], ctx.constructionSite.requiredResources[resource] - ctx.constructionSite.deliveredResources[resource]);
                ctx.constructionSite.deliveredResources[resource] += amountToDeliver;
                resourceController.resourceCounts[resource] -= amountToDeliver;

                if (!ctx.constructionSite.GetRemainingResourcesToDeliver().Any())
                {
                    ctx.constructionSite.Build();
                    ctx.State = BuildingState.None;
                    yield break;
                }
            }

            if (ctx.storage != null && !ctx.constructionSite.GetRemainingResourcesToDeliver().ContainsKey(ctx.storage.Type))
            {
                ctx.storage = null; //forces target storage recalculation
            }

            ctx.State = BuildingState.GoingToStorage;
        }
    }
}