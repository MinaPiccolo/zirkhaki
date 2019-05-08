// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public interface IOperationObserver
	{
		OperationState GetState();

		float GetProgress();

		void Abort();

		void Reset();

		Result GetResult();
	}
}
