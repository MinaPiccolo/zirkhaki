// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationVE<TValue, TError>
	{
		OperationState GetState();

		OperationState Update();

		float GetProgress();

		void Abort();

		void Reset();

		ResultVE<TValue, TError> GetResult();
	}
}
