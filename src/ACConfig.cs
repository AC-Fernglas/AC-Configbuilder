using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACConfigBuilder
{
    public class ACConfig
    {
        public string userpath { get; set; }
        public string changeDirectory { get; set; }
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
        [JsonProperty(PropertyName = "network-dev")]
        [AcProperty(PropertyName = "network-dev")]
        public int? listid { get; set; }
        [JsonProperty(PropertyName = "vlan-id")]
        [AcProperty(PropertyName = "vlan-id")]
        public int? vlan { get; set; }
        [JsonProperty(PropertyName = "underlying-if")]
        [AcProperty(PropertyName = "underlying-if")]
        public string underlyingif { get; set; }
        [JsonProperty(PropertyName ="name")]
        [AcProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "tagging")]
        [AcProperty(PropertyName = "tagging")]
        public tag tag { get; set; }
        public bool activate { get; set; }
    }
    public class  Interfacenetworkif {
        [JsonProperty(PropertyName = "interface network-if")]
        [AcProperty(PropertyName ="interface network-if")]
        public int? listid { get; set; }
        [JsonProperty(PropertyName = "application-type")]
        [AcProperty(PropertyName = "application-type")]
        public applicationtype apptype { get; set; }
        [JsonProperty(PropertyName = "ip-adress")]
        [AcProperty(PropertyName = "ip-adress")]
        public string ipaddress { get; set; }
        [JsonProperty(PropertyName = "prefix-length")]
        [AcProperty(PropertyName = "prefix-length")]
        public int? prefixlength { get; set; }
        [JsonProperty(PropertyName = "gateway")]
        [AcProperty(PropertyName = "gateway")]
        public string gateway { get; set; }
        [JsonProperty(PropertyName = "name")]
        [AcProperty(PropertyName = "name")]
        public string Name{ get; set; }
        [JsonProperty(PropertyName = "underlying-dev")]
        [AcProperty(PropertyName = "underlying-dev")]
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
        [JsonProperty(PropertyName = "proxy-set")]
        [AcProperty(PropertyName = "proxy-set")]
        public int? listid { get; set; }
        [JsonProperty(PropertyName = "proxy-name")]
        [AcProperty(PropertyName = "proxy-name")]
        public string proxyname { get; set; }
        [JsonProperty(PropertyName = "proxy-enable-keep-alive")]
        [AcProperty(PropertyName = "proxy-enable-keep-alive")]
        public proxyenablekeepalive proxyenablekeepalive { get; set; }
        [JsonProperty(PropertyName = "srd-name")]
        [AcProperty(PropertyName = "srd-name")]
        public string srdname { get; set; }
        [JsonProperty(PropertyName = "sbcipv4-sip-int-name")]
        [AcProperty(PropertyName = "sbcipv4-sip-int-name")]
        public string sbcipv4sipintname { get; set; }
        [JsonProperty(PropertyName = "keepalive-fail-resp")]
        [AcProperty(PropertyName = "keepalive-fail-resp")]
        public string keepalivefailresp { get; set; }
        [JsonProperty(PropertyName = "success-detect-retries")]
        [AcProperty(PropertyName = "success-detect-retries")]
        public int? successdetectretries { get; set; }
        [JsonProperty(PropertyName = "success-detect-int")]
        [AcProperty(PropertyName = "success-detect-int")]
        public int? successdetectint { get; set; }
        [JsonProperty(PropertyName = "proxy-redundancy-mode")]
        [AcProperty(PropertyName = "proxy-redundancy-mode")]
        public proxyredundancymode proxyredundancymode { get; set; }
        [JsonProperty(PropertyName = "is-proxy-hot-swap")]
        [AcProperty(PropertyName = "is-proxy-hot-swap")]
        public int? isproxyhotswap { get; set; }
        [JsonProperty(PropertyName = "proxy-load-balancing-method")]
        [AcProperty(PropertyName = "proxy-load-balancing-method")]
        public int? proxyloadbalancingmethod { get; set; }
        [JsonProperty(PropertyName = "min-active-serv-lb")]
        [AcProperty(PropertyName = "min-active-serv-lb")]
        public int? minactiveservlb { get; set; }
        public bool activate { get; set; }
    }
    public class Proxyip {
        [JsonProperty(PropertyName = "proxy-ip")]
        [AcProperty(PropertyName = "proxy-ip")]
        public string ip { get; set; }
        [JsonProperty(PropertyName = "proxy-adress")]
        [AcProperty(PropertyName = "proxy-adress")]
        public string proxyaddress { get; set; }
        [JsonProperty(PropertyName = "transport-type")]
        [AcProperty(PropertyName = "transport-type")]
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
