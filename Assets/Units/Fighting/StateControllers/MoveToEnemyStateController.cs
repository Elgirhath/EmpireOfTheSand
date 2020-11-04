using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Fighting.StateControllers
{
    public class MoveToEnemyStateController : AbstractStateController
    {
        private new readonly FightingStateManager context;

        public MoveToEnemyStateController(FightingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(context.enemy.transform.position);
            }

            if (Vector2.Distance(context.transform.position, context.enemy.transform.position) < context.attackDistance)
            {
                context.movementController.Stop();
                context.State = FightingState.Attacking;
            }
        }
    }
}