using System;

namespace Revy.Framework
{
    [Flags]
    public enum EEventFilter
    {
        None = 1,
        Local = 2,
        Remote = 4,
        Server = 8
    }
}