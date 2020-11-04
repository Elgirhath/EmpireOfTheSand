using System;
using System.Collections.Generic;
using Assets.Units.Attacking;
using Assets.Units.Movement;
using Assets.Units.Soaking.StateControllers;
using Assets.Units.StateManagement;
using UnityEngine;

namespace Assets.Units.Soaking
{
    public class SoakingStateManager : AbstractStateManager, IAttackable
    {
        public float soakingTime;
        internal UnitMovementController movementController;

        public override IDictionary<Enum, Type> StateControllerBindings { get; } = new Dictionary<Enum, Type>
        {
            {SoakingState.MoveToBase, typeof(MoveToBaseStateController)},
            {SoakingState.Drying, typeof(DryingStateController)}
        };
        protected override void OnStart()
        {
            movementController = GetComponent<UnitMovementController>();
            State = SoakingState.None;
        }

        public void StartSoaking()
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            State = SoakingState.MoveToBase;
        }

        public void Attack(int attackStrength)
        {
            StartSoaking();
        }

        public Vector2 Position => transform.position;
        private void OnDestroy()
        {
            IsDestroyed = true;
        }

        public bool IsDestroyed { get; private set; }
    }
}