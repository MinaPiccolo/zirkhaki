// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationObserverVE<TValue, TError>
	{
		OperationState GetState();

		float GetProgress();

		void Abort();

		void Reset();

		ResultVE<TValue, TError> GetResult();
	}
}
