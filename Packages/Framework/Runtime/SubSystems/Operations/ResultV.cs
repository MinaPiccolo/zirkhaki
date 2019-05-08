// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

namespace Revy.Framework
{
	public struct ResultV<TValue>
	{
		private ResultV(bool isError)
		{
			this._value = default(TValue);
			this._state = ResultV<TValue>.State.checkedError;
		}

		private ResultV(TValue newValue)
		{
			this._value = newValue;
			this._state = ResultV<TValue>.State.checkedValue;
		}

		public static ResultV<TValue> MakeValid(TValue value)
		{
			return new ResultV<TValue>(value);
		}

		public static ResultV<TValue> MakeError()
		{
			return new ResultV<TValue>(false);
		}

		public ResultState GetState()
		{
			switch (this._state)
			{
			default:
				return ResultState.uninitialized;
			case ResultV<TValue>.State.uncheckedValue:
			case ResultV<TValue>.State.checkedValue:
				return ResultState.valid;
			case ResultV<TValue>.State.uncheckedError:
			case ResultV<TValue>.State.checkedError:
				return ResultState.error;
			}
		}

		public TValue Value()
		{
			switch (this._state)
			{
			case ResultV<TValue>.State.uncheckedValue:
			case ResultV<TValue>.State.uncheckedError:
				throw new InvalidOperationException("Result state must be checked prior to accessing the value");
			case ResultV<TValue>.State.checkedValue:
				return this._value;
			case ResultV<TValue>.State.checkedError:
				throw new InvalidOperationException("Attempting to access the value from an Result with error");
			}
			throw new InvalidOperationException("Cannot access value from an uninitialized Result");
		}

		[Conditional("ENABLE_STRICT_RESULT")]
		private void SetAsChecked()
		{
			if (this._state == ResultV<TValue>.State.uncheckedError)
			{
				this._state = ResultV<TValue>.State.checkedError;
			}
			else if (this._state == ResultV<TValue>.State.uncheckedValue)
			{
				this._state = ResultV<TValue>.State.checkedValue;
			}
		}

		private ResultV<TValue>.State _state;

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
