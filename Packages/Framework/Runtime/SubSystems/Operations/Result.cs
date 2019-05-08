// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace Revy.Framework
{
	public struct Result
	{
		private Result(bool isError)
		{
			if (isError)
			{
				this._state = Result.State.error;
			}
			else
			{
				this._state = Result.State.valid;
			}
		}

		public static Result MakeValid()
		{
			return new Result(false);
		}

		public static Result MakeError()
		{
			return new Result(true);
		}

		public ResultState GetState()
		{
			switch (this._state)
			{
			default:
				return ResultState.uninitialized;
			case Result.State.valid:
				return ResultState.valid;
			case Result.State.error:
				return ResultState.error;
			}
		}

		private Result.State _state;

		private enum State : byte
		{
			unitialized,
			valid,
			error
		}
	}
}
