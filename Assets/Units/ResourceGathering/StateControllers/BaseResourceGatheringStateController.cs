using Units.StateManagement;

namespace Units.ResourceGathering.StateControllers
{
    public abstract class BaseResourceGatheringStateController : AbstractStateController
    {
        protected new ResourceGatheringStateManager context;

        protected BaseResourceGatheringStateController(ResourceGatheringStateManager context) : base(context)
        {
            this.context = context;
        }

        public abstract override void Execute();
    }
}