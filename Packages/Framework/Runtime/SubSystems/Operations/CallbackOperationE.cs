namespace Revy.Framework
{
	public abstract class CallbackOperationE<TError> : IOperationE<TError>
	{
		public void Abort()
		{
			this.OnAbort();
			this._result = default(ResultE<TError>);
			this._state = OperationState.done;
			this._progress = 0f;
		}

		public float GetProgress()
		{
			return this._progress;
		}

		public ResultE<TError> GetResult()
		{
			return this._result;
		}

		public OperationState GetState()
		{
			return this._state;
		}

		public void Reset()
		{
			this.OnReset();
			this._state = OperationState.waiting;
			this._progress = 0f;
			this._result = default(ResultE<TError>);
		}

		public OperationState Update()
		{
			if (this._state == OperationState.waiting)
			{
				this._state = OperationState.running;
				this.OnStart();
			}
			return this._state;
		}

		protected abstract void OnStart();

		protected void OnSuccess()
		{
			this._state = OperationState.done;
			this._progress = 1f;
			this._result = ResultE<TError>.MakeValid();
		}

		protected void OnError(TError error)
		{
			this._state = OperationState.done;
			this._progress = 0f;
			this._result = ResultE<TError>.MakeError(error);
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnAbort()
		{
		}

		protected OperationState _state;

		protected ResultE<TError> _result;

		protected float _progress;
	}
}
