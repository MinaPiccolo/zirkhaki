using System;

namespace Revy.Framework
{
    public class CEventFilterAttribute : Attribute
    {
        public EEventFilter Filter { get; private set; }

        public CEventFilterAttribute(EEventFilter e)
        {
            Filter = e;
        }
    }
}