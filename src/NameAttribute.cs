using System;

namespace secondtry
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