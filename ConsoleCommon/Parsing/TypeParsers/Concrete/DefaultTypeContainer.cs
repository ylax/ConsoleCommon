using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class DefaultTypeContainer : TypeParserContainer
    {
        public DefaultTypeContainer() : base(true,
            new ArrayParser(), new BoolParser(), new EnumParser(),
            new KeyValueParser(), new ObjectParser(),
            new NullableParser(),
            new SecureStringParser(), new TypeTypeParser())
        { }
    }
}
