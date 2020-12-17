using Units.Interaction;
using Units.StateManagement;

namespace Units.Building.StateControllers
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
            Move();
        }

        private void Move()
        {
            if (context.construction == null)
            {
                context.State = BuildingState.None;
                return;
            }

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(context.construction.transform.position);
            }

            if (context.construction.tile == null) return; // constructions Start hasn't been called yet
            if (!context.movementController.IsInInteractionRange(context.construction, 0.5f)) return;

            context.movementController.Stop();
            context.State = BuildingState.Building;
        }
    }
}