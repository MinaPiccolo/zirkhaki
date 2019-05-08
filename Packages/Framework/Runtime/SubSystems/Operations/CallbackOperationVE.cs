// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public abstract class CallbackOperationVE<TValue, TError> : IOperationVE<TValue, TError>
	{
		public void Abort()
		{
			this.OnAbort();
			this._result = default(ResultVE<TValue, TError>);
			this._state = OperationState.done;
			this._progress = 0f;
		}

		public float GetProgress()
		{
			return this._progress;
		}

		public ResultVE<TValue, TError> GetResult()
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
			this._result = default(ResultVE<TValue, TError>);
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

		protected void OnSuccess(TValue value)
		{
			this._state = OperationState.done;
			this._progress = 1f;
			this._result = ResultVE<TValue, TError>.MakeValid(value);
		}

		protected void OnError(TError error)
		{
			this._state = OperationState.done;
			this._progress = 0f;
			this._result = ResultVE<TValue, TError>.MakeError(error);
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnAbort()
		{
		}

		protected OperationState _state;

		protected ResultVE<TValue, TError> _result;

		protected float _progress;
	}
}
