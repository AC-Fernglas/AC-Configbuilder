using System;

namespace ACConfigBuilder
{
    internal class AcPropertyAttribute : Attribute
    {

        public string PropertyName { get; set; }
    }
}