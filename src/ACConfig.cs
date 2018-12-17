using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public class ACConfig
    {
        public string userpath { get; set; }
        public string Kundenkuerzel { get; set; }
        public ConfigureNetwork configureNetwork { get; set; }
        public Configureviop configureviop { get; set; }
    }
    public class ConfigureNetwork
    {
        public List<Networkdev> networkdev { get; set; }
        public List<Interfacenetworkif> interfacenetworkif { get; set; }
    }
   public class Networkdev { 
        public int? listid { get; set; }
        public int? vlanip { get; set; }
        public string underlyingif { get; set; }
        public string name { get; set; }
        public tag tagging { get; set; }
        
        public bool activate { get; set; }
    }

    public class  Interfacenetworkif {
        public int? listid { get; set; }
        public applicationtype Applicationtype { get; set; }
        public string ipadress { get; set; }
        public int? prefixlength { get; set; }
        public string gateway { get; set; }
        public string name { get; set; }
        public string underlyingdev { get; set; }

        public bool activate { get; set; }
    }

    public class Configureviop
    {
        public List<Proxyset> proxyset { get; set; }
        public List<Proxyip> proxyip { get; set; }

    }
    public class Proxyset
    {
        public int? listid { get; set; }
        public string proxyname { get; set; }
        public proxyenablekeepalive? Proxyenablekeepalive { get; set; }
        public string srdname { get; set; }
        public string sbcipv4sipintname { get; set; }
        public string keepalivefailresp { get; set; }
        public int? successdetectretries { get; set; }
        public int? successdetectint { get; set; }
        public proxyredundancymode? Proxyredundancymode { get; set; }
        public int? isproxyhotswap { get; set; }
        public int? proxyloadbalancingmethod { get; set; }
        public int? minactiveservlb { get; set; }
        public bool activate { get; set; }
    }


    public class Proxyip {
        public string ip { get; set; }
        public string proxyadress { get; set; }
        public transporttype? Transporttype { get; set; }
        public bool activate { get; set; }
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
        uoptions,
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
