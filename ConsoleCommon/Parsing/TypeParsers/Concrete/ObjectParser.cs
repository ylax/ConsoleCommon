using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class ObjectParser : TypeParserBase<object>
    {
        public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            return Convert.ChangeType(toParse, typeToParse);
        }
    }
}
