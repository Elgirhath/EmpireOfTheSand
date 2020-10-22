using System;

namespace Assets.Unit.StateManagement
{
    public class StateControllerFactory
    {
        public static AbstractStateController GetController(Type type, AbstractStateManager context)
        {
            return (AbstractStateController)Activator.CreateInstance(type, new object[] { context });
        }
    }
}