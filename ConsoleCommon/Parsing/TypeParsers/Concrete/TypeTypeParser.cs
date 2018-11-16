using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class TypeTypeParser : TypeParserBase<Type>
    {
        public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            IEnumerable<Type> _types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes());
            Type _returnType = _types.SkipWhile(t =>
                    (t.GetCustomAttribute<TypeParamAttribute>() != null &&
                    t.GetCustomAttribute<TypeParamAttribute>().Ignore))
                    .Where(t => t.MatchesAttributeValueOrName<TypeParamAttribute>(toParse, attr => (attr == null || string.IsNullOrWhiteSpace(attr.FriendlyName)) ? "" : attr.FriendlyName.ToLower())).FirstOrDefault();

            return _returnType;
        }
    }
}
