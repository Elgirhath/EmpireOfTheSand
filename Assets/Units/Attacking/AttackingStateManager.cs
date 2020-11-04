using System;
using System.Collections.Generic;
using Assets.Map;
using Assets.Units.Attacking;
using Assets.Units.Fighting.StateControllers;
using Assets.Units.Movement;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;

namespace Assets.Units.Fighting
{
    public class AttackingStateManager : AbstractStateManager
    {
        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;
        internal IAttackable enemy;

        public float attackDistance;

        public override IDictionary<Enum, Type> StateControllerBindings => new Dictionary<Enum, Type>
        {
            {AttackingState.Attacking, typeof(AttackStateController)},
            {AttackingState.MovingToEnemy, typeof(MoveToEnemyStateController)},
            {AttackingState.MovingToStorage, typeof(MoveToStorageStateController)}
        };

        protected override void OnStart()
        {
            movementController = GetComponent<UnitMovementController>();
            resourceHolder = GetComponent<ResourceHolder>();
        }

        public void Attack(IAttackable unit)
        {
            enemy = unit;
            if (resourceHolder.resourceCounts[TileType.Water] > 0)
            {
                State = AttackingState.MovingToEnemy;
            }
            else
            {
                State = AttackingState.MovingToStorage;
            }
        }

        public void CleanCommands()
        {
            State = AttackingState.None;
            enemy = null;
        }
    }
}
