using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Parsing
{
    public class PrimitiveParser : ITypeParserContainer
    {
        #region Implementation
        public object Parse(string toParse, Type type)
        {
            if (type.IsInterface) throw new Exception("Cannot parse interfaces. Must use concrete types");
            if (type.IsArray)
            {
                return ParseArray(toParse, type);
            }
            else return ParseElement(toParse, type);
        }
        public string[] GetAcceptedValues(Type type)
        {
            string[] _acceptedVals = new string[0];
            Type myUnderLyingType = type;
            if (type.IsArray) myUnderLyingType = type.GetElementType();
            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable) myUnderLyingType = Nullable.GetUnderlyingType(myUnderLyingType);

            if (myUnderLyingType.IsAssignableFrom(typeof(bool)))
            {
                _acceptedVals = GetBoolAcceptedValues();
            }
            else if (myUnderLyingType.IsEnum)
            {
                _acceptedVals = GetEnumAcceptedValues(myUnderLyingType);
            }
            return _acceptedVals;
        }
        #endregion

        #region Parse Methods
        private object ParseArray(string toParse, Type type)
        {
            Type elementType = type.GetElementType();
            //Comma delimited, or comma+space delimited
            string[] splits = toParse.Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries);
            splits = splits.Select(s => s.Trim()).ToArray();
            Type[] myTypes = new Type[] { typeof(int) };
            object[] myReqs = new object[] { splits.Length };

            Array returnVals = type.GetConstructor(myTypes).Invoke(myReqs) as Array;

            //object[] returnVals = new object[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                string split = splits[i];
                object myElement = Parse(split, elementType);
                returnVals.SetValue(myElement, i);
            }
            return returnVals;
        }
        private object ParseElement(string toParse, Type type)
        {
            object myVal = null;
            Type myPropType = type;
            Type myUnderLyingType = myPropType;
            bool isNullable = myPropType.IsGenericType && myPropType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable)
            {
                myUnderLyingType = Nullable.GetUnderlyingType(myPropType);
            }
            if (myUnderLyingType.IsEnum)
            {
                if (!_parseEnumAsArray && myUnderLyingType.GetCustomAttribute<FlagsAttribute>() != null)
                {
                    _parseEnumAsArray = true;
                    Type _enumArrayType = myUnderLyingType.MakeArrayType();
                    Array _enumArray = ParseArray(toParse, _enumArrayType) as Array;
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
                    myVal = _totalVal;
                    _parseEnumAsArray = false;
                }
                else myVal = Enum.Parse(myUnderLyingType, toParse, true);
            }
            else if (typeof(System.Security.SecureString).Equals(myUnderLyingType))
            {
                var secure = new System.Security.SecureString();

                foreach (var v in toParse.ToCharArray())
                {
                    secure.AppendChar(v);
                }
                myVal = secure;
            }
            else if(typeof(KeyValuePair<,>).Name.Equals(myUnderLyingType.Name))
            {
                Type[] _genericTypes = myUnderLyingType.GetGenericArguments();
                string[] _keyValArr = toParse.Split(':');
                if (_keyValArr == null || _keyValArr.Length != 2) throw new Exception("Failed to parse key value pair");
                object _key = Parse(_keyValArr[0], _genericTypes[0]);
                object _val = Parse(_keyValArr[1], _genericTypes[1]);
                if (_key == null || _val == null) throw new Exception("Failed to parse key value pair");
                object[] _keyValPair = new object[] { _key, _val };
                myVal = myUnderLyingType.GetConstructor(_genericTypes).Invoke(_keyValPair);
            }
            else if (typeof(IConvertible).IsAssignableFrom(myUnderLyingType))
            {
                if (myUnderLyingType == typeof(bool))
                {
                    if (BoolFalseValues.Contains(toParse?.ToLower().Trim()))
                    {
                        return false;
                    }
                    else if (BoolTrueValues.Contains(toParse?.ToLower().Trim()))
                    {
                        return true;
                    }
                }
                else myVal = Convert.ChangeType(toParse, myUnderLyingType);
            }
            else if (typeof(Type).IsAssignableFrom(myUnderLyingType))
            {
                IEnumerable<Type> _types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes());
                myVal = _types.SkipWhile(t =>
                                    (t.GetCustomAttribute<TypeParamAttribute>() != null &&
                                    t.GetCustomAttribute<TypeParamAttribute>().Ignore))
                                    .Where(t => t.MatchesAttributeValueOrName<TypeParamAttribute>(toParse, attr => (attr == null || string.IsNullOrWhiteSpace(attr.FriendlyName)) ? "" : attr.FriendlyName.ToLower())).FirstOrDefault();
            }
            return myVal;
        }
        #endregion

        #region Private Fields
        bool _parseEnumAsArray = false;
        #endregion

        #region Helper Methods
        private string[] BoolFalseValues
        {
            get
            {
                return new string[] { "n", "no", "false", "f", "off" };
            }
        }
        private string[] BoolTrueValues
        {
            get
            {
                return new string[] { "y", "yes", "true", "t", "on", null,"" };
            }
        }
        
        private string[] GetBoolAcceptedValues()
        {
            return BoolFalseValues.Where(s=>s!=null).Concat(BoolTrueValues.Where(s=>s!=null)).ToArray();
        }
        private string[] GetEnumAcceptedValues(Type enumType)
        {
            return Enum.GetNames(enumType);
        }

        public ITypeParser GetParser(Type typeToParse)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
