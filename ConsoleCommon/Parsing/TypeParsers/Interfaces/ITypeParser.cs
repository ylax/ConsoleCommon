using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommon.Parsing.TypeParsers.Interfaces
{
    public interface ITypeParser
    {
        Type GetTypeToParse(params Type[] genericTypeParams);
        object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer);
        string[] GetAcceptedValues(Type typeToParse);
    }
}
