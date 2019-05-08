using System;

namespace Revy.Framework
{
    public class CExecutionOrderAttribute : Attribute
    {
        public int Order { get; private set; }

        public CExecutionOrderAttribute(int order)
        {
            Order = order;
        }
    }
}