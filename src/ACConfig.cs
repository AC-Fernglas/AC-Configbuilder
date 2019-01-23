using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public class ACConfig
    {
        public string userpath { get; set; }
         public string Customer{ get; set; }
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
        public int? vlan { get; set; }
        public string underlyingif { get; set; }
        public string Name { get; set; }
        public tag tag { get; set; }
        
        public bool activate { get; set; }
    }

    public class  Interfacenetworkif {
        public int? listid { get; set; }
        public applicationtype apptype { get; set; }
        public string ipaddress { get; set; }
        public int? prefixlength { get; set; }
        public string gateway { get; set; }
        public string Name{ get; set; }
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
        public proxyenablekeepalive proxyenablekeepalive { get; set; }
        public string srdname { get; set; }
        public string sbcipv4sipintname { get; set; }
        public string keepalivefailresp { get; set; }
        public int? successdetectretries { get; set; }
        public int? successdetectint { get; set; }
        public proxyredundancymode proxyredundancymode { get; set; }
        public int? isproxyhotswap { get; set; }
        public int? proxyloadbalancingmethod { get; set; }
        public int? minactiveservlb { get; set; }
        public bool activate { get; set; }
    }


    public class Proxyip {
        public string ip { get; set; }
        public string proxyadress { get; set; }
        public transporttype transporttype { get; set; }
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
        [Name("using-options")]
        usingOptions,
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
