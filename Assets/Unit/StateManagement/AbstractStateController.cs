namespace Assets.Unit.StateManagement
{
    public abstract class AbstractStateController
    {
        protected AbstractStateManager context;

        protected AbstractStateController(AbstractStateManager context)
        {
            this.context = context;
        }

        public abstract void Execute();

        internal virtual void OnStart() { }
        internal virtual void Dispose() { }
    }
}