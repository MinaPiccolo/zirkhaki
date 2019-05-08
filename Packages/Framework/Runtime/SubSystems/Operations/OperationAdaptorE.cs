// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public class OperationAdaptorE<TError> : IOperation
	{
		public OperationAdaptorE(IOperationE<TError> operation)
		{
			this._operation = operation;
		}

		public void Abort()
		{
			this._operation.Abort();
		}

		public float GetProgress()
		{
			return this._operation.GetProgress();
		}

		public Result GetResult()
		{
			switch (this._operation.GetResult().GetState())
			{
			case ResultState.valid:
				return Result.MakeValid();
			case ResultState.error:
				return Result.MakeError();
			}
			return default(Result);
		}

		public OperationState GetState()
		{
			return this._operation.GetState();
		}

		public void Reset()
		{
			this._operation.Reset();
		}

		public OperationState Update()
		{
			return this._operation.Update();
		}

		private IOperationE<TError> _operation;
	}
}
