using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing
{
    public interface ITypeParserContainer
    {
        object Parse(string toParse, Type type);
        string[] GetAcceptedValues(Type type);
        ITypeParser GetParser(Type typeToParse);
    }
}
