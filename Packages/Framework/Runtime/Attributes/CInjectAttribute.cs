using System;

/// <summary>
/// CInject is used to inject dependency into the fields and properties.
/// </summary>

namespace Revy.Framework
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CInjectAttribute : Attribute
    {

    }
}
