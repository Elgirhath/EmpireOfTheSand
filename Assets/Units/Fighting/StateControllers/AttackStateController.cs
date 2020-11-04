using Assets.Map;
using Assets.Units.Soaking;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Fighting.StateControllers
{
    public class AttackStateController : AbstractStateController
    {
        private new readonly FightingStateManager context;

        public AttackStateController(FightingStateManager context) : base(context)
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
            context.enemy.GetComponent<SoakingStateManager>().StartSoaking();
            context.State = FightingState.MovingToStorage;
        }
    }
}