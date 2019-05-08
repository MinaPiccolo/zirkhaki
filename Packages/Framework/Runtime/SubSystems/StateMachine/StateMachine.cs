namespace Revy.Framework
{
    public class StateMachine<TSelf, TState> where TSelf : StateMachine<TSelf, TState> where TState : StateMachine<TSelf, TState>.State
    {
        protected TState CurrentState { get; private set; }

        protected TState NextState { get; private set; }

        protected void SwitchTo(TState newState)
        {
            SwitchState switchState = _switchState;
            if (switchState == SwitchState.enter)
            {
                NextState = newState;
                return;
            }
            if (switchState != SwitchState.exit)
            {
                _SwitchTo(newState);
                return;
            }
        }

        protected virtual void RenderInspector()
        {
        }

        private void _SwitchTo(TState newState)
        {
            TState tstate = newState;
            do
            {
                NextState = tstate;
                _switchState = SwitchState.exit;
                CurrentState?.OnExit();

                NextState = (TState)((object)null);
                _switchState = SwitchState.enter;
                CurrentState = tstate;
                if (CurrentState != null)
                {
                    CurrentState.OnEnter();
                }
                tstate = NextState;
            }
            while (tstate != null);
            _switchState = SwitchState.idle;
        }

        private SwitchState _switchState;

        private enum SwitchState
        {
            idle,
            exit,
            enter
        }

        public class State
        {
            protected State(TSelf newParentStateMachine)
            {
                ParentStateMachine = newParentStateMachine;
            }

            protected TSelf ParentStateMachine { get; private set; }

            public virtual void OnEnter()
            {
            }

            public virtual void OnExit()
            {
            }

            protected void SwitchTo(TState newState)
            {
                ParentStateMachine.SwitchTo(newState);
            }
        }
    }
}