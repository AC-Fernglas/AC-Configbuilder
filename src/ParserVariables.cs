using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public static class  ParserVariables
    {
        public const string identifier = "configure";

        public const string netidentifier = "network";

        
        public const string voipidentifier = "voip";

        public const string tap = " ";
        public  static string lz = Environment.NewLine;


        public const string prosetlistident = "proxy-set";
        public const string proiplistident = "proxy-ip";
        public const string devlistident = "network-dev";
        public const string interlistident = "interface network-if";
        public const string exit = "exit";
        public const string activate = "activate";
        public const string vlan = ("vlan-id");
        public const string underlyingif = ("underlying-if");
        public const string Name = ("name");
        public const string tag = ("tagging");
        public const string  apptype =("application-type");
        public const string  ipaddress = "ip-address";
        public const string  prefixlength =("prefix-length");
        public const string  gateway =("gateway");
        public const string  underlyingdev ="underlying-dev";
        public const string  proxyname ="proxy-name";
        public const string  proxyenablekeepalive ="proxy-enable-keep-alive";
        public const string  srdname ="srd-name";
        public const string  sbcipv4sipintname ="sbcipv4-sip-int-name";
        public const string  keepalivefailresp ="keepalive-fail-resp";
        public const string successdetectretries ="success-detect-retries";
        public const string successdetectint ="success-detect-int";
        public const string proxyredundancymode ="proxy-redundancy-mode";
        public const string isproxyhotswap ="is-proxy-hot-swap";
        public const string proxyloadbalancingmethod ="proxy-load-balancing-method";
        public const string minactiveservlb ="min-active-serv-lb";
        public const string proxyaddress ="proxy-address";
        public const string transporttype = "transport-type";
    }
}
