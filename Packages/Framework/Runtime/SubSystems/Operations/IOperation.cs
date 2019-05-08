
using System;

namespace Revy.Framework
{
	public interface IOperation
	{
		OperationState GetState();

		OperationState Update();

		float GetProgress();

		void Abort();

		void Reset();

		Result GetResult();
	}
}
