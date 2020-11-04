using Assets.Map;

namespace Assets.Units.ResourceGathering.StateControllers
{
    public class MoveToResourceStateController : BaseResourceGatheringStateController
    {
        public MoveToResourceStateController(ResourceGatheringStateManager context) : base(context)
        {
        }

        public override void Execute()
        {
            if (context.targetResource == null) return;

            if (!context.movementController.IsMoving)
            {
                context.SetDestinationResource(context.targetResource);
            }

            if (context.movementController.IsInInteractionRange(context.targetResource, context.gatheringRangeInTilemapCoordinates))
            {
                context.movementController.Stop();
                context.State = ResourceGatheringState.Gathering;
            }
        }
    }
}