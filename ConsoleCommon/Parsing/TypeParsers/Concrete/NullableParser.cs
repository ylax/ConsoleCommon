using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class NullableParser : ITypeParser
    {
        public string[] GetAcceptedValues(Type typeToParse)
        {
            return new string[0];
        }

        public Type GetTypeToParse(params Type[] genericTypeParams)
        {
            Type _returnType = typeof(Nullable<>);
            if (genericTypeParams.Count() == 1) _returnType = typeof(Nullable<>).MakeGenericType(genericTypeParams[0]);
            return _returnType;
            
        }

        public object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            Type _myUnderLyingType = Nullable.GetUnderlyingType(typeToParse);
            object _rtrnVal = parserContainer.GetParser(_myUnderLyingType)?.Parse(toParse, _myUnderLyingType, parserContainer);
            return _rtrnVal;
        }
    }
}
