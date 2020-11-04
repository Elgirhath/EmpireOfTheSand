using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Units.StateManagement
{
    public abstract class AbstractStateManager : MonoBehaviour
    {
        private Enum state;
        public Enum State
        {
            get => state;
            set
            {
                if (value.Equals(state)) return;
                if (state != null && controllers.ContainsKey(state))
                {
                    controllers[state].Dispose();
                }

                state = value;
            }
        }

        public abstract IDictionary<Enum, Type> StateControllerBindings { get; }
        protected IDictionary<Enum, AbstractStateController> controllers;

        private void Start()
        {
            foreach (var controller in controllers.Values)
            {
                controller.OnStart();
            }
            OnStart();
        }

        protected virtual void OnStart() { }

        protected AbstractStateManager()
        {
            Bind();
        }

        private void Bind()
        {
            controllers = StateControllerBindings.ToDictionary(kvp => kvp.Key, kvp => StateControllerFactory.GetController(kvp.Value, this));
        }

        private void Update()
        {
            if (State != null && controllers.ContainsKey(State))
            {
                controllers[State].Execute();
            }
        }
    }
}