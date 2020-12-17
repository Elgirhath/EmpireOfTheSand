using System;
using System.Collections.Generic;
using Map;
using Units.Attacking.StateControllers;
using Units.Movement;
using Units.ResourceGathering;
using Units.StateManagement;

namespace Units.Attacking
{
    public class AttackingStateManager : AbstractStateManager
    {
        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;
        internal IAttackable enemy;

        public float attackDistance;

        protected override IDictionary<Enum, Type> StateControllerBindings => new Dictionary<Enum, Type>
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
