using System;

namespace ACConfigBuilder
{
    internal class NameAttribute : Attribute
    {
        public string Name;

        public NameAttribute(string v)
        {
            this.Name = v;
        }
    }
}