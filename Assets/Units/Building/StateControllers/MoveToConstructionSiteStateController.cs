using Assets.Map;
using Assets.Units.StateManagement;

namespace Assets.Units.Building.StateControllers
{
    public class MoveToConstructionSiteStateController : AbstractStateController
    {
        private new readonly BuildingStateManager context;

        public MoveToConstructionSiteStateController(BuildingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            Move(context);
        }

        private void Move(BuildingStateManager ctx)
        {
            if (ctx.constructionSite == null)
            {
                ctx.State = BuildingState.None;
                return;
            }

            if (!ctx.movementController.IsMoving)
            {
                ctx.movementController.SetDestination(ctx.constructionSite.transform.position);
            }

            if (context.movementController.IsInInteractionRange(ctx.constructionSite, 0.5f))
            {
                ctx.movementController.Stop();
                ctx.State = BuildingState.Building;
            }
        }
    }
}