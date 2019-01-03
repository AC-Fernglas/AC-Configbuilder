using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    class ParserVariables
    {
        public const string identifier = "configure";

        public const string netidentifier = "network";
        public const string devlistident = "network-dev";
        public const string interlistident = "interface network-if";
        
        public const string voipidentifier = "voip";
        public const string prosetlistident = "proxy-set";
        public const string proiplistident = "proxy-ip";
        
        public const char tap = "\t";
        public const string exit = "exit";
        public const string activate = "activate";
        public const string lz = "\n";
    }
}
