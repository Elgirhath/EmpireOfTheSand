using Map;
using Units.StateManagement;
using UnityEngine;

namespace Units.Attacking.StateControllers
{
    public class AttackStateController : AbstractStateController
    {
        private new readonly AttackingStateManager context;

        public AttackStateController(AttackingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            if (context.resourceHolder.resourceCounts[TileType.Water] <= 0)
            {
                Debug.LogError("No water in resource holder");
                return;
            }
            context.resourceHolder.resourceCounts[TileType.Water]--;
            context.enemy.Attack(1);
            context.State = AttackingState.MovingToStorage;
        }
    }
}