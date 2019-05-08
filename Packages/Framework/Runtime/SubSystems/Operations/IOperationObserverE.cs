// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationObserverE<TError>
	{
		OperationState GetState();

		float GetProgress();

		void Abort();

		void Reset();

		ResultE<TError> GetResult();
	}
}
