using UnityEngine;

namespace Revy.Framework
{
	public interface IStateSystem 
	{
        /// <summary>
        /// Set state for given context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contextState"></param>
        void SetState(string context, string contextState);

        /// <summary>
        /// Get state of given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetState(string context);
	}
}