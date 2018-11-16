using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class SecureStringParser : TypeParserBase<System.Security.SecureString>
    {
        public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            var secure = new System.Security.SecureString();
            foreach (var v in toParse.ToCharArray())
            {
                secure.AppendChar(v);
            }
            return secure;
        }
    }
}
