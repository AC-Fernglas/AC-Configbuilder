using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public class JsonObjects
    {
        public string userpath { get; set; }
        public string Kundenkuerzel { get; set; }
        public ConfigureNetwork configureNetwork { get; set; }
        public ConfigureNetwork2 configure_network { get; set; }
        public Configureviop configureviop { get; set; }
    }
    public class ConfigureNetwork
    {
        public int networkdev { get; set; }
        public int vlanip { get; set; }
        public string underlyingif { get; set; }
        public string name { get; set; }
        public tag tagging { get; set; }
    }

    public class ConfigureNetwork2
    {
        public int interfacenetworkif { get; set; }
        public applicationtype applicationtype { get; set; }
        public int ipadress { get; set; }
        public int prefixlength { get; set; }
        public int gateway { get; set; }
        public string name { get; set; }
        public string underlyingdev { get; set; }
    }

    public class Proxyredundancymode
    {
        public proxyredundancymode proxyredundancymode { get; set; }
        public int isproxyhotswap { get; set; }
        public int proxyloadbalancingmethod { get; set; }
        public int minactiveservlb { get; set; }
    }

    public class Configureviop
    {
        public int proxyset { get; set; }
        public string proxyname { get; set; }
        public proxyenablekeepalive proxyenablekeepalive { get; set; }
        public string srdname { get; set; }
        public string sbcipv4sipintname { get; set; }
        public int keepalivefailresp { get; set; }
        public int successdetectretries { get; set; }
        public int successdetectint { get; set; }
        public Proxyredundancymode proxyredundancymode { get; set; }
        public int proxyip { get; set; }
        public int proxyadress { get; set; }
        public transporttype transporttype { get; set; }
    }

    public enum tag
    {
        tagged,
        untagged
    };
    public enum transporttype
    {
        udp,
        tcp
    };
    public enum proxyenablekeepalive
    {
        usingoptions,
        otheroptions
    };
    public enum proxyredundancymode
    {
        homing,
        other
    };
    public enum applicationtype
    {
        control,
        other
    }
}
