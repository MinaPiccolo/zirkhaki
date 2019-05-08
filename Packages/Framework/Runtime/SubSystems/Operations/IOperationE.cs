// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationE<TError>
	{
		OperationState GetState();

		OperationState Update();

		float GetProgress();

		void Abort();

		void Reset();

		ResultE<TError> GetResult();
	}
}
