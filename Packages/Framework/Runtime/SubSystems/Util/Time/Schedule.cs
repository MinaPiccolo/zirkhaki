using System;
using System.Collections.Generic;

namespace Revy.Framework
{
    public sealed class Schedule : FClass, ISchedule, IEndOfFrameTick, IExecutionOrder
    {
        #region Fields

        private static readonly List<Action> _nextFrameActions = new List<Action>();

        private static int _nextFrameActionsCount; //Cache tasksCount to increase performance.

        #endregion Fields

        #region Properties

        int IExecutionOrder.ExecutionOrder => 12000;

        public bool IsActive { get; set; }

        #endregion Properties

        #region Public Methods

        void ISchedule.DoNextFrame(Action action)
        {
            _nextFrameActions.Add(action);
            _updateNextFramesActionCount();
        }

        void IEndOfFrameTick.EndOfFrameTick()
        {
            if(_nextFrameActionsCount <= 0) return;
            for (int i = 0; i < _nextFrameActionsCount; i++)
            {
                Action action = _nextFrameActions[i];
                action();
            }

            _nextFrameActions.Clear();
            _updateNextFramesActionCount();
        }

        #endregion Public Methods

        #region Helpers     

        private static void _updateNextFramesActionCount()
        {
            _nextFrameActionsCount = _nextFrameActions.Count;
        }
        
        #endregion Helpers
    }
}