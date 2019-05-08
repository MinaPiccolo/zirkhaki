// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public class OperationObserverAdaptor : IOperation
	{
		public OperationObserverAdaptor(IOperationObserver operation)
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
			return this._operation.GetResult();
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
			return this._operation.GetState();
		}

		private IOperationObserver _operation;
	}
}
