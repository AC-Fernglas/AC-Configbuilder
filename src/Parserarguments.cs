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

    }
}
