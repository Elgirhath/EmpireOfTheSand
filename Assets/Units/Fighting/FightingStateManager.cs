using System;
using System.Collections.Generic;
using Assets.Map;
using Assets.Units.Fighting.StateControllers;
using Assets.Units.Movement;
using Assets.Units.ResourceGathering;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Fighting
{
    public class FightingStateManager : AbstractStateManager
    {
        internal UnitMovementController movementController;
        internal ResourceHolder resourceHolder;
        internal Unit enemy;

        public float attackDistance;

        public override IDictionary<Enum, Type> StateControllerBindings => new Dictionary<Enum, Type>
        {
            {FightingState.Attacking, typeof(AttackStateController)},
            {FightingState.MovingToEnemy, typeof(MoveToEnemyStateController)},
            {FightingState.MovingToStorage, typeof(MoveToStorageStateController)}
        };

        protected override void OnStart()
        {
            movementController = GetComponent<UnitMovementController>();
            resourceHolder = GetComponent<ResourceHolder>();
        }

        public void Attack(Unit unit)
        {
            enemy = unit;
            if (resourceHolder.resourceCounts[TileType.Water] > 0)
            {
                State = FightingState.MovingToEnemy;
            }
            else
            {
                State = FightingState.MovingToStorage;
            }
        }

        public void CleanCommands()
        {
            State = FightingState.None;
            enemy = null;
        }
    }
}
