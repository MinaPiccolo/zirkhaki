
using System;

namespace Revy.Framework
{
	public abstract class CallbackOperation : IOperation
	{
		public void Abort()
		{
			this.OnAbort();
			this._result = default(Result);
			this._state = OperationState.done;
			this._progress = 0f;
		}

		public float GetProgress()
		{
			return this._progress;
		}

		public Result GetResult()
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
			this._result = default(Result);
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
			this._result = Result.MakeValid();
		}

		protected void OnError()
		{
			this._state = OperationState.done;
			this._progress = 0f;
			this._result = Result.MakeError();
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnAbort()
		{
		}

		protected OperationState _state;

		protected Result _result;

		protected float _progress;
	}
}
