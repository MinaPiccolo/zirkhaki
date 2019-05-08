using System;
using System.Threading.Tasks;

namespace Revy.Framework
{
    /// <summary>
    /// FClass is the base class from which every framework class(not component) script derives.
    /// This class has all framework functionalities but could not attach to a game object.
    /// </summary>
    public abstract class FClass 
    {
        #region Constructor & Destructor

        protected FClass()
        {
            MFramework.Register(this);
        }

        /// <summary>
        /// This method will invoke when Destroy() or DestroyImmediate has invoked for scriptable object.
        /// </summary>
        ~FClass()
        {
            MFramework.UnRegister(this);
        }
        
        #endregion
    }
}
