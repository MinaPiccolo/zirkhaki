// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationObserverV<TValue>
	{
		OperationState GetState();

		float GetProgress();

		void Abort();

		void Reset();

		ResultV<TValue> GetResult();
	}
}
