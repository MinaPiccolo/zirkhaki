/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 10 / 10 / 2017
 * Description: 
 */

using System;

namespace Revy.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CPropertyAttribute : Attribute
    {
#pragma warning disable 414
        private readonly string _name;
#pragma warning restore 414

        public CPropertyAttribute(string name)
        {
            _name = name;
        }
    }
}