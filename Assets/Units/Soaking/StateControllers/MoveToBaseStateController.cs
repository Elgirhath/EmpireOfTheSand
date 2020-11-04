using Assets.Map;
using Assets.Units.Fighting;
using Assets.Units.StateManagement;

namespace Assets.Units.Soaking.StateControllers
{
    public class MoveToBaseStateController : AbstractStateController
    {
        private new readonly SoakingStateManager context;

        public MoveToBaseStateController(SoakingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            var unitBase = Base.GetBase(context.GetComponent<PlayerProperty>().playerColor);

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(unitBase.transform.position);
            }

            if (context.movementController.IsInInteractionRange(unitBase, 0.3f))
            {
                context.movementController.Stop();
                context.State = SoakingState.Drying;
            }
        }
    }
}