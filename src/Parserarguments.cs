using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

namespace ACConfigBuilder
{
    class Parserarguments
    {
        public static readonly Parser<string> devlistident = from tap in Parse.String(ParserVariables.tap)
                                                             from subident in Parse.String(ParserVariables.networkDev).Text()
                                                             select subident;

        public static readonly Parser<string> interlistident = from tap in Parse.String(ParserVariables.tap)
                                                               from subident in Parse.String(ParserVariables.interfaceNetwokIf).Text()
                                                               select subident;

        public static readonly Parser<string> prosetlistident = from tap in Parse.String(ParserVariables.tap)
                                                                from subident in Parse.String(ParserVariables.proxySet).Text()
                                                                select subident;

        public static readonly Parser<string> proiplistident = from tap in Parse.String(ParserVariables.tap)
                                                               from subident in Parse.String(ParserVariables.proxyIp).Text()
                                                               select subident;
    }
}
