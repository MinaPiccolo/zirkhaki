using System;
using UnityEngine;
using Revy.Framework;

namespace Revy.Framework
{
	public interface ISchedule
	{
		void DoNextFrame(Action action);
	}
}