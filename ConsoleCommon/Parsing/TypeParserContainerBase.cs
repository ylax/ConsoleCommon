using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Helpers;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing.TypeParsers
{
    public abstract class TypeParserContainerBase : ITypeParserContainer
    {
        protected abstract List<ITypeParser> _typeParsers { get; }
        public IEnumerable<ITypeParser> TypeParsers => _typeParsers.AsReadOnly();

        #region Implementation
        public object Parse(string toParse, Type type)
        {
            if (type.IsInterface) throw new Exception("Cannot parse interfaces. Must use concrete types");
            return GetParser(type).Parse(toParse, type, this);
        }
        public string[] GetAcceptedValues(Type type)
        {
            return GetParser(type).GetAcceptedValues(type);
        }
        public ITypeParser GetParser(Type typeToParse)
        {
            var _comparer = new TypesByInheritanceLevelComparer();
            Type[] _genArgs = typeToParse.IsGenericType ? typeToParse.GetGenericArguments() : new Type[0];
            ITypeParser _parser =
                TypeParsers
                .Where(t => t.GetTypeToParse(_genArgs).IsAssignableFrom(typeToParse))
                .OrderBy(t => t.GetTypeToParse(_genArgs), _comparer)
                .LastOrDefault();
            if (_parser == null) throw new Exception($"Parsing type '{typeToParse.Name}' not handled");
            return _parser;
        }
        #endregion

        #region Helpers
        private void addTypeParser(ITypeParser typeParser, bool overwriteDupes)
        {
            ITypeParser _match = TypeParsers.FirstOrDefault(tp => tp.GetTypeToParse().Equals(typeParser.GetTypeToParse()));
            if (_match != null)
            {
                if (overwriteDupes)
                {
                    _typeParsers.Remove(_match);
                }
                else throw new Exception($"Type parser of type '{typeParser.GetTypeToParse()}' already exists");
            }
            _typeParsers.Add(typeParser);
        }
        #endregion
    }
}
