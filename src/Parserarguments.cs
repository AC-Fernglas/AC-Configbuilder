using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

namespace secondtry
{
    class Parserarguments
    {
        public static readonly Parser<string> devlistident = from tap in Parse.String(ParserVariables.tap)
                                                             from subident in Parse.String(ParserVariables.devlistident).Text()
                                                             select subident;

        public static readonly Parser<string> interlistident = from tap in Parse.String(ParserVariables.tap)
                                                               from subident in Parse.String(ParserVariables.interlistident).Text()
                                                               select subident;

        public static readonly Parser<string> prosetlistident = from tap in Parse.String(ParserVariables.tap)
                                                                from subident in Parse.String(ParserVariables.prosetlistident).Text()
                                                                select subident;

        public static readonly Parser<string> proiplistident = from tap in Parse.String(ParserVariables.tap)
                                                               from subident in Parse.String(ParserVariables.proiplistident).Text()
                                                               select subident;

        public static readonly Parser<string> vlan = from tap in Parse.String(ParserVariables.tap).Many()
                                                     from vlan in Parse.String("vlan-id").Text()
                                                     select vlan;

        public static readonly Parser<string> under = from tap in Parse.String(ParserVariables.tap).Many()
                                                      from underlying in Parse.String("underlying-if").Text()
                                                      select underlying;

        public static readonly Parser<string> name = from tap in Parse.String(ParserVariables.tap).Many()
                                                     from name in Parse.String("name").Text()
                                                     select name;

        public static readonly Parser<string> tag = from tap in Parse.String(ParserVariables.tap).Many()
                                                    from tagging in Parse.String("tagging").Text()
                                                    select tagging;

        public static readonly Parser<string> apptype = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                 from apptype in Parse.String("application-type").Text()
                                                 select apptype;

        public static readonly Parser<string> ipaddress = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                   from ipadr in Parse.String("ip-address").Text()
                                                   select ipadr;

        public static readonly Parser<string> prefixlength = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                      from prel in Parse.String("prefix-length").Text()
                                                      select prel;

        public static readonly Parser<string> gateway = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                 from gateway in Parse.String("gateway").Text()
                                                 select gateway;

        public static readonly Parser<string> name2 = from tap in Parse.Chars(ParserVariables.tap).Many()
                                              from name in Parse.String("name").Text()
                                              select name;

        public static readonly Parser<string> underlyingdev = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                       from undev in Parse.String("underlying-dev").Text()
                                                       select undev;

        public static readonly Parser<string> proxyname = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                   from prname in Parse.String("proxy-name").Text()
                                                   select prname;

        public static readonly Parser<string> proxyenablekeepalive = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                              from peka in Parse.String("proxy-enable-keep-alive").Text()
                                                              select peka;

        public static readonly Parser<string> srdname = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                 from srdn in Parse.String("srd-name").Text()
                                                 select srdn;

        public static readonly Parser<string> sbcipv4sipintname = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                           from sbci in Parse.String("sbcipv4-sip-int-name").Text()
                                                           select sbci;

        public static readonly Parser<string> keepalivefailresp = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                           from kfr in Parse.String("keepalive-fail-resp").Text()
                                                           select kfr;

        public static readonly Parser<string> successdetectretries = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                              from sdr in Parse.String("success-detect-retries").Text()
                                                              select sdr;

        public static readonly Parser<string> successdetectint = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                          from sdi in Parse.String("success-detect-int").Text()
                                                          select sdi;

        public static readonly Parser<string> proxyredundancymode = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                             from prm in Parse.String("proxy-redundancy-mode").Text()
                                                             select prm;

        public static readonly Parser<string> isproxyhotswap = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                        from iphs in Parse.String("is-proxy-hot-swap").Text()
                                                        select iphs;

        public static readonly Parser<string> proxyloadbalancingmethod = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                                  from plbm in Parse.String("proxy-load-balancing-method").Text()
                                                                  select plbm;

        public static readonly Parser<string> minactiveservlb = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                         from masl in Parse.String("min-active-serv-lb").Text()
                                                         select masl;

        public static readonly Parser<string> proxyaddress = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                      from prad in Parse.String("proxy-address").Text()
                                                      select prad;

        public static readonly Parser<string> transporttype = from tap in Parse.Chars(ParserVariables.tap).Many()
                                                       from taty in Parse.String("transport-type").Text()
                                                       select taty;

        public static readonly Parser<string> active =
              from tap in Parse.Chars(ParserVariables.tap).Many()
              from active in Parse.String(ParserVariables.activate).Text()
              select active;

        public static readonly Parser<string> exit =
              from tap in Parse.Chars(ParserVariables.tap).Many()
              from exit in Parse.String(ParserVariables.exit).Text()
              select exit;
    }
}
