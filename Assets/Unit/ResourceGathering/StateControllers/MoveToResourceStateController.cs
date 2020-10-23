﻿using Assets.Map;

namespace Assets.Unit.ResourceGathering.StateControllers
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

            var tileIndex = GameMap.Instance.GetCellPosition(context.targetResource.position);

            var isInGatheringRange =
                InteractionRangeResolver.Instance.IsPointInInteractionRange(tileIndex, context.transform.position, context.gatheringRange);

            if (isInGatheringRange)
            {
                context.movementController.IsMoving = false;
                context.State = ResourceGatheringState.Gathering;
            }
        }
    }
}