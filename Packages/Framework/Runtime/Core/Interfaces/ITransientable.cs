using UnityEngine;

namespace Revy.Framework
{
    public interface ITransientable
    {
        /// <summary>
        /// This game object will be child of Transient game object.
        /// </summary>
        GameObject ObjectToTransient{ get; }
    }
}