using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class KeyValueParser : ITypeParser
    {
        public Type GetTypeToParse(params Type[] genericTypeParams)
        {
            Type _returnType = typeof(KeyValuePair<,>);
            if (genericTypeParams.Count() == 2) _returnType = _returnType.MakeGenericType(genericTypeParams);
            return _returnType;
        }

        public string[] GetAcceptedValues(Type typeToParse)
        {
            return new string[0];
        }

        public object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            Type[] _genericTypes = typeToParse.GetGenericArguments();
            string[] _keyValArr = toParse.Split(':');
            if (_keyValArr == null || _keyValArr.Length < 1 || _keyValArr.Length > 2) throw new Exception("Failed to parse key value pair");
            object _key = ParseElement(_keyValArr[0], _genericTypes[0], parserContainer);
            object _val = null;
            if(_keyValArr.Length > 1) _val = ParseElement(_keyValArr[1], _genericTypes[1], parserContainer);
            if (_key == null) throw new Exception("Failed to parse key value pair");
            object[] _keyValPair = new object[] { _key, _val };
            object myVal = typeToParse.GetConstructor(_genericTypes).Invoke(_keyValPair);
            return myVal;
        }

        private object ParseElement(string toParse, Type type, ITypeParserContainer parserContainer)
        {
            ITypeParser _parser = parserContainer.GetParser(type);
            if (_parser == null) throw new Exception($"Parsing type '{type.Name}' not handled");
            return _parser.Parse(toParse, type, parserContainer);
        }
    }
}
