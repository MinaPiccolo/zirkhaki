using UnityEngine;

namespace Revy.Framework
{
    public interface IPersistable
    {
        /// <summary>
        /// This game object will be child of Persistent game object.
        /// </summary>
        GameObject ObjectToPersist { get; }
    }
}