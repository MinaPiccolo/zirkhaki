// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

namespace Revy.Framework
{
	public struct ResultVE<TValue, TError>
	{
		private ResultVE(TError error, bool isError)
		{
			this._value = default(TValue);
			this._error = error;
			this._state = ResultVE<TValue, TError>.State.checkedError;
		}

		private ResultVE(TValue newValue)
		{
			this._value = newValue;
			this._error = default(TError);
			this._state = ResultVE<TValue, TError>.State.checkedValue;
		}

		public static ResultVE<TValue, TError> MakeValid(TValue value)
		{
			return new ResultVE<TValue, TError>(value);
		}

		public static ResultVE<TValue, TError> MakeError(TError error)
		{
			return new ResultVE<TValue, TError>(error, true);
		}

		public ResultState GetState()
		{
			switch (this._state)
			{
			default:
				return ResultState.uninitialized;
			case ResultVE<TValue, TError>.State.uncheckedValue:
			case ResultVE<TValue, TError>.State.checkedValue:
				return ResultState.valid;
			case ResultVE<TValue, TError>.State.uncheckedError:
			case ResultVE<TValue, TError>.State.checkedError:
				return ResultState.error;
			}
		}

		public TValue Value()
		{
			switch (this._state)
			{
			case ResultVE<TValue, TError>.State.uncheckedValue:
			case ResultVE<TValue, TError>.State.uncheckedError:
				throw new InvalidOperationException("Result state must be checked prior to accessing the value");
			case ResultVE<TValue, TError>.State.checkedValue:
				return this._value;
			case ResultVE<TValue, TError>.State.checkedError:
				throw new InvalidOperationException("Attempting to access the value from an Result with error");
			}
			throw new InvalidOperationException("Cannot access value from an uninitialized Result");
		}

		public TError Error()
		{
			switch (this._state)
			{
			case ResultVE<TValue, TError>.State.uncheckedValue:
			case ResultVE<TValue, TError>.State.uncheckedError:
				throw new InvalidOperationException("Result state must be checked prior to accessing the error");
			case ResultVE<TValue, TError>.State.checkedValue:
				throw new InvalidOperationException("Attempting to access the error from an Result with value");
			case ResultVE<TValue, TError>.State.checkedError:
				return this._error;
			}
			throw new InvalidOperationException("Cannot access error from an uninitialized Result");
		}

		[Conditional("ENABLE_STRICT_RESULT")]
		private void SetAsChecked()
		{
			if (this._state == ResultVE<TValue, TError>.State.uncheckedError)
			{
				this._state = ResultVE<TValue, TError>.State.checkedError;
			}
			else if (this._state == ResultVE<TValue, TError>.State.uncheckedValue)
			{
				this._state = ResultVE<TValue, TError>.State.checkedValue;
			}
		}

		private ResultVE<TValue, TError>.State _state;

		private TError _error;

		private TValue _value;

		private enum State : byte
		{
			unitialized,
			uncheckedValue,
			uncheckedError,
			checkedValue,
			checkedError
		}
	}
}
