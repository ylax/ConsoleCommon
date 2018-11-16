using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public class ArrayParser : TypeParserBase<Array>
    {
        public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
        {
            Type elementType = typeToParse.GetElementType();
            //Comma delimited, or comma+space delimited
            string[] splits = toParse.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            splits = splits.Select(s => s.Trim()).ToArray();
            Type[] myTypes = new Type[] { typeof(int) };
            object[] myReqs = new object[] { splits.Length };

            Array returnVals = typeToParse.GetConstructor(myTypes).Invoke(myReqs) as Array;

            //object[] returnVals = new object[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                string split = splits[i];
                object myElement = ParseElement(split, elementType, parserContainer);
                returnVals.SetValue(myElement, i);
            }
            return returnVals;
        }
        private object ParseElement(string toParse, Type type, ITypeParserContainer parserContainer)
        {
            ITypeParser _parser = parserContainer.GetParser(type);
            if (_parser == null) throw new Exception($"Parsing type '{type.Name}' not handled");
            return _parser.Parse(toParse, type, parserContainer);
        }
    }
}
