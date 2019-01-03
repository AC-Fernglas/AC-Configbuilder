using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public  class  ParserVariables
    {
        public const string identifier = "configure";

        public const string netidentifier = "network";
        public const string devlistident = "network-dev";
        public const string interlistident = "interface network-if";
        
        public const string voipidentifier = "voip";
        public const string prosetlistident = "proxy-set";
        public const string proiplistident = "proxy-ip";
        
        public const string tap = " ";
        public const string exit = "exit";
        public const string activate = "activate";
        public  string lz = Environment.NewLine;
    }
}
