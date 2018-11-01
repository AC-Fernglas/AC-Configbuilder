using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

namespace secondtry
{
  public class ParserGrammar
    {
        static string identifier = "configure";

        static string netidentifier = "network";
        static string devlistident = "network-dev";
        static string interlistident = "interface network-if";
        
        static string voipidentifier = "voip";
        static string prosetlistident = "proxy-set";
        static string proiplistident = "proxy-ip";

        static char tap = ' ';
        static string exit = "exit";
        static string ignor = "activate";
        static string lz = "\n";

       public static readonly Parser<string> getidentifier =
            (from net in Parse.String(identifier+" "+netidentifier).Text() 
            select net )
            .Or
            (from voip in Parse.String(identifier + " " + voipidentifier).Text()
           select voip).Or
            (from lz in Parse.String(lz).Text()
             select lz);

        public static readonly Parser<string> subidentvalue =
            (from tap in Parse.Char(tap)
             from subident in Parse.String(devlistident).Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(interlistident).Text()
               from ws in Parse.WhiteSpace
               from value in Parse.AnyChar.Many().Text()
               select value).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(prosetlistident).Text()
               from ws in Parse.WhiteSpace
               from value in Parse.AnyChar.Many().Text()
               select value).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(proiplistident).Text()
               from value in Parse.AnyChar.Many().Text()
               select value).Or
              (from exit in Parse.String(exit).Text()
               select exit);


        public static readonly Parser<string> getsubident =
              (from tap in Parse.Char(tap)
               from subident in Parse.String(devlistident).Text()
               select subident).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(interlistident).Text()
               select subident).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(prosetlistident).Text()
               select subident).Or
              (from tap in Parse.Char(tap)
               from subident in Parse.String(proiplistident).Text()
               select subident).Or
              (from exit in Parse.String(exit).Text()
               select exit);

        public static readonly Parser<string> idc =
              from tap in Parse.Chars(tap).Many()
              from doesntmatter in Parse.String(ignor).Text()
              select doesntmatter;

        public static readonly Parser<string> dev =
            (from tap in Parse.Chars(tap, tap).Many()
             from vlan in Parse.String("vlan-id").Text()
             select vlan).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from underlying in Parse.String("underlying-if").Text()
             select underlying).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from name in Parse.String("name").Text()
             select name).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from tagging in Parse.String("tagging").Text()
             select tagging).Or
            (from idc in idc.Text()
             select idc).Or
            (from tap in Parse.Char(tap)
             from exit in Parse.String(exit).Text()
             select exit);


        public static readonly Parser<string> devvalue =
            (from tap in Parse.Chars(tap, tap).Many()
             from vlan in Parse.String("vlan-id").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from underlying in Parse.String("underlying-if").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from name in Parse.String("name").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap, tap).Many()
             from tagging in Parse.String("tagging").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value);


        public static readonly Parser<string> inif =
            (from tap in Parse.Chars(tap).Many()
             from apptype in Parse.String("application-type").Text()
             select apptype).Or
            (from tap in Parse.Chars(tap).Many()
             from ipadr in Parse.String("ip-address").Text()
             select ipadr).Or
            (from tap in Parse.Chars(tap).Many()
             from prel in Parse.String("prefix-length").Text()
             select prel).Or
            (from tap in Parse.Chars(tap).Many()
             from gateway in Parse.String("gateway").Text()
             select gateway).Or
            (from tap in Parse.Chars(tap).Many()
             from name in Parse.String("name").Text()
             select name).Or
            (from tap in Parse.Chars(tap).Many()
             from undev in Parse.String("underlying-dev").Text()
             select undev).Or
            (from idc in idc.Text()
             select idc).Or
            (from tap in Parse.Char(tap)
             from exit in Parse.String(exit).Text()
             select exit);

        public static readonly Parser<string> inifvalue =
            (from tap in Parse.Chars(tap).Many()
             from apptype in Parse.String("application-type").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap).Many()
             from ipadr in Parse.String("ip-address").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap).Many()
             from prel in Parse.String("prefix-length").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap).Many()
             from gateway in Parse.String("gateway").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap).Many()
             from undev in Parse.String("underlying-dev").Text()
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from tap in Parse.Chars(tap).Many()
             from name in Parse.String("name").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value);

        public static readonly Parser<string> prse =
            (from tap in Parse.Chars(tap).Many()
             from prname in Parse.String("proxy-name").Text()
             select prname).Or
            (from tap in Parse.Chars(tap).Many()
             from peka in Parse.String("proxy-enable-keep-alive").Text()
             select peka).Or
            (from tap in Parse.Chars(tap).Many()
             from srdn in Parse.String("srd-name").Text()
             select srdn).Or
            (from tap in Parse.Chars(tap).Many()
             from sbci in Parse.String("sbcipv4-sip-int-name").Text()
             select sbci).Or
            (from tap in Parse.Chars(tap).Many()
             from kfr in Parse.String("keepalive-fail-resp").Text()
             select kfr).Or
            (from tap in Parse.Chars(tap).Many()
             from sdr in Parse.String("success-detect-retries").Text()
             select sdr).Or
            (from tap in Parse.Chars(tap).Many()
             from sdi in Parse.String("success-detect-int").Text()
             select sdi).Or
            (from tap in Parse.Chars(tap).Many()
             from prm in Parse.String("proxy-redundancy-mode").Text()
             select prm).Or
            (from tap in Parse.Chars(tap).Many()
             from iphs in Parse.String("is-proxy-hot-swap").Text()
             select iphs).Or
            (from tap in Parse.Chars(tap).Many()
             from plbm in Parse.String("proxy-load-balancing-method").Text()
             select plbm).Or
            (from tap in Parse.Chars(tap).Many()
             from masl in Parse.String("min-active-serv-lb").Text()
             select masl).Or
            (from idc in idc.Text()
             select idc).Or
            (from tap in Parse.Char(tap)
             from exit in Parse.String(exit).Text()
             select exit);

        public static readonly Parser<string> prsevalue =
           (from tap in Parse.Chars(tap).Many()
            from prname in Parse.String("proxy-name").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from peka in Parse.String("proxy-enable-keep-alive").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from srdn in Parse.String("srd-name").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from sbci in Parse.String("sbcipv4-sip-int-name").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from kfr in Parse.String("keepalive-fail-resp").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from sdr in Parse.String("success-detect-retries").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from sdi in Parse.String("success-detect-int").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from prm in Parse.String("proxy-redundancy-mode").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from iphs in Parse.String("is-proxy-hot-swap").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from plbm in Parse.String("proxy-load-balancing-method").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value).Or
           (from tap in Parse.Chars(tap).Many()
            from masl in Parse.String("min-active-serv-lb").Text()
            from ws in Parse.WhiteSpace
            from value in Parse.AnyChar.Many().Text()
            select value);

          public static readonly Parser<string> prip =
            (from tap in Parse.Chars(tap).Many()
             from prad in Parse.String("proxy-address").Text()
             select prad).Or(
             from tap in Parse.Chars(tap).Many()
             from taty in Parse.String("transport-type").Text()
             select taty).Or
            (from idc in idc.Text()
             select idc).Or
            (from tap in Parse.Char(tap)
             from exit in Parse.String(exit).Text()
             select exit);

        public static readonly Parser<string> pripvalue =
            (from tap in Parse.Chars(tap).Many()
             from prad in Parse.String("proxy-address").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or(
             from tap in Parse.Chars(tap).Many()
             from taty in Parse.String("transport-type").Text()
             from ws in Parse.WhiteSpace
             from value in Parse.AnyChar.Many().Text()
             select value).Or
            (from idc in idc.Text()
             select idc).Or
            (from tap in Parse.Char(tap)
             from exit in Parse.String(exit).Text()
             select exit);

    }
}
