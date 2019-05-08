using System;

namespace Revy.Framework
{
    public interface IDIRegister
    {
        /// <summary>
        /// The type of service that instance of object are register for.
        /// </summary>
        Type ServiceType { get; }
    }
}