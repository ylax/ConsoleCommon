using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class EnumParser : TypeParserBase<Enum>
    {
        bool _parseEnumAsArray = false;
        public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            object _returnVal;
            if (!_parseEnumAsArray && typeToParse.GetCustomAttribute<FlagsAttribute>() != null)
            {
                _parseEnumAsArray = true;
                Type _enumArrayType = typeToParse.MakeArrayType();
                ITypeParser _arrayParser = parserContainer.GetParser(_enumArrayType);
                Array _enumArray = _arrayParser.Parse(toParse, _enumArrayType, parserContainer) as Array;
                int _totalVal = 0;
                int _iter = 0;

                foreach (object enVal in _enumArray)
                {
                    if (_iter == 0) _totalVal = (int)enVal;
                    else
                    {
                        _totalVal = _totalVal | (int)enVal;
                    }
                    _iter++;
                }
                _returnVal = _totalVal;
                _parseEnumAsArray = false;
            }
            else _returnVal = Enum.Parse(typeToParse, toParse, true);
            return _returnVal;
        }
    }
}
