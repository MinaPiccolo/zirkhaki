// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

namespace Revy.Framework
{
	public struct ResultE<TError>
	{
		private ResultE(TError error)
		{
			this._error = error;
			this._state = ResultE<TError>.State.checkedError;
		}

		private ResultE(bool isNotError)
		{
			this._error = default(TError);
			this._state = ResultE<TError>.State.checkedValid;
		}

		public static ResultE<TError> MakeValid()
		{
			return new ResultE<TError>(true);
		}

		public static ResultE<TError> MakeError(TError error)
		{
			return new ResultE<TError>(error);
		}

		public ResultState GetState()
		{
			switch (this._state)
			{
			default:
				return ResultState.uninitialized;
			case ResultE<TError>.State.uncheckedValid:
			case ResultE<TError>.State.checkedValid:
				return ResultState.valid;
			case ResultE<TError>.State.uncheckedError:
			case ResultE<TError>.State.checkedError:
				return ResultState.error;
			}
		}

		public TError Error()
		{
			switch (this._state)
			{
			case ResultE<TError>.State.uncheckedValid:
			case ResultE<TError>.State.uncheckedError:
				throw new InvalidOperationException("Result state must be checked prior to accessing the error");
			case ResultE<TError>.State.checkedValid:
				throw new InvalidOperationException("Attempting to access an error from an Result with valid state");
			case ResultE<TError>.State.checkedError:
				return this._error;
			}
			throw new InvalidOperationException("Cannot access error from an uninitialized Result");
		}

		[Conditional("ENABLE_STRICT_RESULT")]
		private void SetAsChecked()
		{
			if (this._state == ResultE<TError>.State.uncheckedError)
			{
				this._state = ResultE<TError>.State.checkedError;
			}
			else if (this._state == ResultE<TError>.State.uncheckedValid)
			{
				this._state = ResultE<TError>.State.checkedValid;
			}
		}

		private ResultE<TError>.State _state;

		private TError _error;

		private enum State : byte
		{
			unitialized,
			uncheckedValid,
			uncheckedError,
			checkedValid,
			checkedError
		}
	}
}
