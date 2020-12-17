using Build;
using Units.Interaction;
using Units.StateManagement;
using UnityEngine;

namespace Units.Attacking.StateControllers
{
    public class MoveToEnemyStateController : AbstractStateController
    {
        private new readonly AttackingStateManager context;

        public MoveToEnemyStateController(AttackingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            if (context.enemy == null || context.enemy.IsDestroyed)
            {
                context.State = AttackingState.None;
                return;
            }

            if (!context.movementController.IsMoving)
            {
                context.movementController.SetDestination(context.enemy.Position);
            }

            if (context.enemy is Structure structure)
            {
                if (!context.movementController.IsInInteractionRange(structure, context.attackDistance)) return;
            } 
            else if (Vector2.Distance(context.transform.position, context.enemy.Position) > context.attackDistance)
            {
                return;
            }

            context.movementController.Stop();
            context.State = AttackingState.Attacking;
        }
    }
}