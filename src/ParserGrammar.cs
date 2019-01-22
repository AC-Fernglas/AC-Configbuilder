using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

namespace secondtry
{
    public class ParserGrammar
    {

        public static readonly Parser<string> getidentifier =
             (from net in Parse.String(ParserVariables.identifier + ParserVariables.tap + ParserVariables.netidentifier).Text()
              select net)
             .Or
             (from voip in Parse.String(ParserVariables.identifier + ParserVariables.tap + ParserVariables.voipidentifier).Text()
              select voip).Or
             (from lz in Parse.String(ParserVariables.lz).Text()
              select lz);

        public static readonly Parser<string> getsubident =
              (Parserarguments.devlistident).Or
              (Parserarguments.interlistident).Or
              (Parserarguments.prosetlistident).Or
              (Parserarguments.proiplistident).Or
               (findName(ParserVariables.exit));

        public static readonly Parser<string> subidentvalue =
                     findValue(ParserVariables.devlistident)
                 .Or(findValue(ParserVariables.interlistident))
                 .Or(findValue(ParserVariables.prosetlistident))
                 .Or(findValue(ParserVariables.proiplistident))
                 .Or(findName(ParserVariables.exit));

        public static Parser<string> findValue(string search)
        {
            return from localTab in Parse.String(ParserVariables.tap).Many()
                   from searchResult in Parse.String(search).Text()
                   from Tab in Parse.String(ParserVariables.tap)
                   from value in Parse.AnyChar.Many().Text()
                   select value;
        }
        public static Parser<string> findName(string search)
        {
            return from localTab in Parse.String(ParserVariables.tap).Many()
                   from searchResult in Parse.String(search).Text()
                   select search;
        }

        public static Parser<string> NameParser =
    findName(ParserVariables.apptype).Or
    (findName(ParserVariables.gateway)).Or
    (findName(ParserVariables.ipaddress)).Or
    (findName(ParserVariables.isproxyhotswap)).Or
    (findName(ParserVariables.keepalivefailresp)).Or
    (findName(ParserVariables.minactiveservlb)).Or
    (findName(ParserVariables.Name)).Or
    (findName(ParserVariables.prefixlength)).Or
    (findName(ParserVariables.proxyaddress)).Or
    (findName(ParserVariables.proxyenablekeepalive)).Or
    (findName(ParserVariables.proxyloadbalancingmethod)).Or
    (findName(ParserVariables.proxyname)).Or
    (findName(ParserVariables.proxyredundancymode)).Or
    (findName(ParserVariables.sbcipv4sipintname)).Or
    (findName(ParserVariables.srdname)).Or
    (findName(ParserVariables.successdetectint)).Or
    (findName(ParserVariables.successdetectretries)).Or
    (findName(ParserVariables.tag)).Or
    (findName(ParserVariables.transporttype)).Or
    (findName(ParserVariables.underlyingdev)).Or
    (findName(ParserVariables.underlyingif)).Or
    (findName(ParserVariables.vlan)).Or
    (findName(ParserVariables.activate)).Or
    (findName(ParserVariables.exit));

        public static Parser<string> ValueParser =
           findValue(ParserVariables.apptype).Or
           (findValue(ParserVariables.gateway)).Or
           (findValue(ParserVariables.ipaddress)).Or
           (findValue(ParserVariables.isproxyhotswap)).Or
           (findValue(ParserVariables.keepalivefailresp)).Or
           (findValue(ParserVariables.minactiveservlb)).Or
           (findValue(ParserVariables.Name)).Or
           (findValue(ParserVariables.prefixlength)).Or
           (findValue(ParserVariables.proxyaddress)).Or
           (findValue(ParserVariables.proxyenablekeepalive)).Or
           (findValue(ParserVariables.proxyloadbalancingmethod)).Or
           (findValue(ParserVariables.proxyname)).Or
           (findValue(ParserVariables.proxyredundancymode)).Or
           (findValue(ParserVariables.sbcipv4sipintname)).Or
           (findValue(ParserVariables.srdname)).Or
           (findValue(ParserVariables.successdetectint)).Or
           (findValue(ParserVariables.successdetectretries)).Or
           (findValue(ParserVariables.tag)).Or
           (findValue(ParserVariables.transporttype)).Or
           (findValue(ParserVariables.underlyingdev)).Or
           (findValue(ParserVariables.underlyingif)).Or
           (findValue(ParserVariables.vlan));
    }
}
