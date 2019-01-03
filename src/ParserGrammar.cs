using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

namespace secondtry
{
    public class ParserGrammar
    {
        static ParserVariables NewLine = new ParserVariables();

        public static readonly Parser<string> getidentifier =
             (from net in Parse.String(ParserVariables.identifier + ParserVariables.tap + ParserVariables.netidentifier).Text()
              select net)
             .Or
             (from voip in Parse.String(ParserVariables.identifier + ParserVariables.tap + ParserVariables.voipidentifier).Text()
              select voip).Or
             (from lz in Parse.String(NewLine.lz).Text()
              select lz);

        public static readonly Parser<string> getsubident =
              (Parserarguments.devlistident).Or
              (Parserarguments.interlistident).Or
              (Parserarguments.prosetlistident).Or
              (Parserarguments.proiplistident).Or
                (Parserarguments.exit);

        public static readonly Parser<string> subidentvalue =
                     findValue(ParserVariables.devlistident)
                 .Or(findValue(ParserVariables.interlistident))
                 .Or(findValue(ParserVariables.prosetlistident))
                 .Or(findValue(ParserVariables.proiplistident))
                 .Or(Parserarguments.exit);
        static Parser<string> findValue(string search)
        {
            return from localTab in Parse.String(ParserVariables.tap)
                   from searchResult in Parse.String(search).Text()
                   select searchResult;
        }

        public static readonly Parser<string> dev =
            (Parserarguments.vlan).Or
            (Parserarguments.under).Or
            (Parserarguments.name).Or
            (Parserarguments.tag).Or
            (Parserarguments.active).Or
            (Parserarguments.exit);

       public static readonly Parser<string> devvalue = 
            from tap in Parse.String(ParserVariables.tap).Many()
            from searchstring in dev
            from value in Parse.AnyChar.Many().Text()
            select value;


        public static readonly Parser<string> inif =
            (Parserarguments.apptype).Or
            (Parserarguments.ipaddress).Or
            (Parserarguments.prefixlength).Or
            (Parserarguments.gateway).Or
            (Parserarguments.name).Or
            (Parserarguments.underlyingdev).Or
            (Parserarguments.active).Or
            (Parserarguments.exit);

        public static readonly Parser<string> inifvalue =
            from tap in Parse.String(ParserVariables.tap).Many()
            from searchstring in inif
            from value in Parse.AnyChar.Many().Text()
            select value;

        public static readonly Parser<string> prse =
            (Parserarguments.proxyname).Or
            (Parserarguments.proxyenablekeepalive).Or
            (Parserarguments.srdname).Or
            (Parserarguments.sbcipv4sipintname).Or
            (Parserarguments.keepalivefailresp).Or
            (Parserarguments.successdetectretries).Or
            (Parserarguments.successdetectint).Or
            (Parserarguments.proxyredundancymode).Or
            (Parserarguments.isproxyhotswap).Or
            (Parserarguments.proxyloadbalancingmethod).Or
            (Parserarguments.minactiveservlb).Or
            (Parserarguments.active).Or
            (Parserarguments.exit);

        public static readonly Parser<string> prsevalue =
           from tap in Parse.String(ParserVariables.tap).Many()
           from searchstring in prse
           from value in Parse.AnyChar.Many().Text()
           select value;

       
        public static readonly Parser<string> prip =
            (Parserarguments.proxyaddress).Or
            (Parserarguments.transporttype).Or
            (Parserarguments.active).Or
            (Parserarguments.exit);

        public static readonly Parser<string> pripvalue =
             from tap in Parse.String(ParserVariables.tap).Many()
             from searchstring in prip
             from value in Parse.AnyChar.Many().Text()
             select value;
    }
}
