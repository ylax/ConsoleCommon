using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommon.Parsing
{
    public interface ITypeParser
    {
        object Parse(string toParse, Type type);
        string[] GetAcceptedValues(Type type);
    }
}
