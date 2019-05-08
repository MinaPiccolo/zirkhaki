// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace Revy.Framework
{
	public class ParallelCompositeOperation : IOperation
	{
		public void Add(IOperation operation, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = operation;
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TError>(IOperationE<TError> operation, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorE<TError>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue>(IOperationV<TValue> operation, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorV<TValue>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue, TError>(IOperationVE<TValue, TError> operation, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationAdaptorVE<TValue, TError>(operation);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add(IOperationObserver observer, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptor(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TError>(IOperationObserverE<TError> observer, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptorE<TError>(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue>(IOperationObserverV<TValue> observer, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
			baseInfo._operation = new OperationObserverAdaptorV<TValue>(observer);
			baseInfo._weight = weight;
			this.totalWeight += weight;
			this.operations.Add(baseInfo);
		}

		public void Add<TValue, TError>(IOperationObserverVE<TValue, TError> observer, float weight = 1f)
		{
			ParallelCompositeOperation.BaseInfo baseInfo = new ParallelCompositeOperation.BaseInfo();
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
			bool flag = true;
			foreach (ParallelCompositeOperation.BaseInfo operation in this.operations)
			{
				if (operation._operation.Update() != OperationState.done)
				{
					flag = false;
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				foreach (ParallelCompositeOperation.BaseInfo operation in this.operations)
				{
					ResultState resultState = operation._operation.GetResult().GetState();
					if (resultState != ResultState.error)
					{
						if (resultState != ResultState.uninitialized)
						{
							if (resultState != ResultState.valid)
							{
							}
						}
						else
						{
							flag3 = true;
						}
					}
					else
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					this.result = Result.MakeError();
				}
				else if (flag3)
				{
					this.result = default(Result);
				}
				else
				{
					this.result = Result.MakeValid();
				}
				this.state = OperationState.done;
				return this.state;
			}
			this.state = OperationState.running;
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
			float num = 0f;
			foreach (ParallelCompositeOperation.BaseInfo baseInfo in this.operations)
			{
				num += baseInfo._operation.GetProgress() * baseInfo._weight;
			}
			return num / this.totalWeight;
		}

		public Result GetResult()
		{
			return this.result;
		}

		public void Abort()
		{
			if (this.state == OperationState.done)
			{
				return;
			}
			foreach (ParallelCompositeOperation.BaseInfo baseInfo in this.operations)
			{
				baseInfo._operation.Abort();
			}
			this.result = default(Result);
			this.state = OperationState.done;
		}

		public void Reset()
		{
			foreach (ParallelCompositeOperation.BaseInfo baseInfo in this.operations)
			{
				baseInfo._operation.Reset();
			}
			this.result = default(Result);
			this.state = OperationState.waiting;
		}

		public float GetTotalWeight()
		{
			return this.totalWeight;
		}

		private List<ParallelCompositeOperation.BaseInfo> operations = new List<ParallelCompositeOperation.BaseInfo>();

		private float totalWeight;

		private Result result;

		private OperationState state;

		private class BaseInfo
		{
			public IOperation _operation;

			public float _weight;
		}
	}
}
