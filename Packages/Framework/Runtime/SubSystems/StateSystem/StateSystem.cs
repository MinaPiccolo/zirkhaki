namespace Revy.Framework
{
    public class StateSystem : GenericStateSystem<string>, ISubsystem, IStateSystem
    {
        #region Properties

        public System.Type ServiceType => typeof(IStateSystem);

        #endregion Properties

        #region Constructor & Destructor

        public StateSystem()
        {
            MFramework.Register(this);
        }

        ~StateSystem()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        #region IStateSystem Interface

        public string GetState(string context)
        {
            return GetState<string>(context);
        }

        public void SetState(string context, string contextState)
        {
            SetState<string>(context, contextState);
        }

        #endregion Public Methods
    }
}