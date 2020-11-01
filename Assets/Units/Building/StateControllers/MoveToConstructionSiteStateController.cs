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
            if (ctx.targetBuilding == null)
            {
                ctx.State = BuildingState.None;
                return;
            }

            if (!ctx.movementController.IsMoving)
            {
                ctx.movementController.SetDestination(ctx.targetBuilding.transform.position);
            }

            var tileIndex = GameMap.Instance.GetCellPosition(ctx.targetBuilding.transform.position);

            var isInInteractionRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, ctx.transform.position, 0.5f);

            if (isInInteractionRange)
            {
                ctx.movementController.Stop();
                ctx.State = BuildingState.Building;
            }
        }
    }
}