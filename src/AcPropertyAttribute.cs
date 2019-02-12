using System;

namespace ACConfigBuilder
{
    public class AcPropertyAttribute : Attribute
    {

        public string PropertyName { get; set; }
    }
}