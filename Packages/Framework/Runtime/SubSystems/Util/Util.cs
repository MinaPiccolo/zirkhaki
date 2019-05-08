using UnityEngine;
using Revy.Framework;

namespace Revy.Framework
{
    public sealed class Util : ISubsystem, IUtil
    {
        #region Fields

        private static readonly ISchedule _schedule = new Schedule();

        #endregion Fields

        #region Properties

        public System.Type ServiceType => typeof(IUtil);

        public ISchedule Schedule => _schedule;

        #endregion Properties

        #region Constructor & Destructor

        public Util()
        {
            MFramework.Register(this);
        }

        ~Util()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        #region Public Methods

        #endregion Public Methods

        #region Helpers     

        #endregion Helpers
    }
}