// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationV<TValue>
	{
		OperationState GetState();

		OperationState Update();

		float GetProgress();

		void Abort();

		void Reset();

		ResultV<TValue> GetResult();
	}
}
