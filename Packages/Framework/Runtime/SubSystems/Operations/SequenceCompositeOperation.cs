// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace Revy.Framework
{
	public class SequenceCompositeOperation : IOperationE<int>
	{
		public void Add(IOperation operation, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = operation;
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TError>(IOperationE<TError> operation, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorE<TError>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue>(IOperationV<TValue> operation, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorV<TValue>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue, TError>(IOperationVE<TValue, TError> operation, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorVE<TValue, TError>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add(IOperationObserver observer, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptor(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TError>(IOperationObserverE<TError> observer, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptorE<TError>(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue>(IOperationObserverV<TValue> observer, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptorV<TValue>(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue, TError>(IOperationObserverVE<TValue, TError> observer, float weight = 1f)
		{
			SequenceCompositeOperation.BaseInfo baseInfo = new SequenceCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptorVE<TValue, TError>(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public OperationState Update()
		{
			if (this.state == OperationState.done)
			{
				return this.state;
			}
			if (this.index >= this.operations.Count)
			{
				this.state = OperationState.done;
				return this.state;
			}
			OperationState operationState = this.operations[this.index]._operation.Update();
			if (operationState != OperationState.done)
			{
				this.state = OperationState.running;
			}
			else
			{
				ResultState resultState = this.operations[this.index]._operation.GetResult().GetState();
				if (resultState != ResultState.valid)
				{
					if (resultState != ResultState.error)
					{
						if (resultState == ResultState.uninitialized)
						{
							this.result = default(ResultE<int>);
							this.state = OperationState.done;
						}
					}
					else
					{
						this.result = ResultE<int>.MakeError(this.index);
						this.state = OperationState.done;
					}
				}
				else
				{
					this.index++;
					if (this.index < this.operations.Count)
					{
						this.state = OperationState.running;
					}
					else
					{
						this.result = ResultE<int>.MakeValid();
						this.state = OperationState.done;
					}
				}
			}
			return this.state;
		}

		public OperationState GetState()
		{
			return this.state;
		}

		public float GetProgress()
		{
			if (this.state == OperationState.done)
			{
				ResultState resultState = this.result.GetState();
				if (resultState == ResultState.valid)
				{
					return 1f;
				}
				if (resultState == ResultState.error || resultState == ResultState.uninitialized)
				{
					return 0f;
				}
			}
			if (this.index >= this.operations.Count)
			{
				return 1f;
			}
			float num = 0f;
			for (int i = 0; i < this.index; i++)
			{
				num += this.operations[i]._weight;
			}
			num += this.operations[this.index]._weight * this.operations[this.index]._operation.GetProgress();
			return num / this.totalWeight;
		}

		public ResultE<int> GetResult()
		{
			return this.result;
		}

		public void Abort()
		{
			if (this.state == OperationState.done)
			{
				return;
			}
			if (this.index < this.operations.Count)
			{
				this.operations[this.index]._operation.Abort();
				this.result = default(ResultE<int>);
			}
			this.state = OperationState.done;
		}

		public void Reset()
		{
			if (this.index < this.operations.Count)
			{
				this.operations[this.index]._operation.Reset();
			}
			this.index = 0;
			this.result = default(ResultE<int>);
			this.state = OperationState.waiting;
		}

		public float GetTotalWeight()
		{
			return this.totalWeight;
		}

		private List<SequenceCompositeOperation.BaseInfo> operations = new List<SequenceCompositeOperation.BaseInfo>();

		private int index;

		private float totalWeight;

		private ResultE<int> result;

		private OperationState state;

		private class BaseInfo
		{
			public IOperation _operation;

			public float _weight;
		}
	}
}
