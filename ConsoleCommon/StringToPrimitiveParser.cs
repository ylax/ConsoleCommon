using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace ConsoleCommon
{
    public static class StringToPrimitiveParser
    {
        public static object Parse(string toParse, Type type)
        {
            if (type.IsArray)
            {
                return ParseArray(toParse, type);
            }
            else return ParseElement(toParse, type);
        }
        public static object ParseArray(string toParse, Type type)
        {
            Type elementType = type.GetElementType();
            //Comma delimited, space delimited, or comma+space delimited
            string[] splits = toParse.Split(new string[] { ", ", ",", " " }, StringSplitOptions.None);
            Type[] myTypes = new Type[] { typeof(int) };
            object[] myReqs = new object[]{splits.Length};

            Array returnVals = type.GetConstructor(myTypes).Invoke(myReqs) as Array;

            //object[] returnVals = new object[splits.Length];
            for(int i =0; i < splits.Length; i++)
            {
                string split = splits[i];
                object myElement = Parse(split, elementType);
                returnVals.SetValue(myElement, i);
            }
            return returnVals;
        }
        public static object ParseElement(string toParse, Type type)
        {
            object myVal = null;
            Type myPropType = type;
            Type myUnderLyingType = myPropType;
            toParse = toParse.ToLower();
            bool isNullable = myPropType.IsGenericType && myPropType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable)
            {
                myUnderLyingType = Nullable.GetUnderlyingType(myPropType);
            }
            if (myUnderLyingType.IsEnum)
            {
                myVal = Enum.Parse(myUnderLyingType, toParse, true);
            }
            else if(typeof(System.Security.SecureString).Equals(myUnderLyingType))
            {
                var secure = new System.Security.SecureString();

                foreach (var v in toParse.ToCharArray())
                {
                    secure.AppendChar(v);
                }
                myVal = secure;
            }
            else if (typeof(IConvertible).IsAssignableFrom(myUnderLyingType))
            {
                if (myUnderLyingType == typeof(bool))
                {
                    switch (toParse)
                    {
                        case "y":
                        case "yes":
                            myVal = true;
                            break;
                        case "n":
                        case "no":
                            myVal = false;
                            break;
                        default:
                            break;
                    }
                }
                else myVal = Convert.ChangeType(toParse, myUnderLyingType);
            }
            else if(typeof(Type).IsAssignableFrom(myUnderLyingType))
            {
                IEnumerable<Type> _types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes());
                myVal = _types.SkipWhile(t=>
                                    (t.GetCustomAttribute<TypeParamAttribute>()!=null && 
                                    t.GetCustomAttribute<TypeParamAttribute>().Ignore))
                                    .Where(t=> t.MatchesAttributeValueOrName<TypeParamAttribute>(toParse, attr=> (attr == null || string.IsNullOrWhiteSpace(attr.FriendlyName)) ? "" : attr.FriendlyName.ToLower())).FirstOrDefault();
            }
            return myVal;
        }
        public static string[] GetSpecificTypeAcceptedValues(Type propType)
        {
            string[] _acceptedVals = new string[0];
            Type myUnderLyingType = propType;
            if (propType.IsArray) myUnderLyingType = propType.GetElementType();
            bool isNullable = propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>);
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
        private static string[] GetBoolAcceptedValues()
        {
            return new string[] { "Y", "N", "YES", "NO" };
        }
        private static string[] GetEnumAcceptedValues(Type enumType)
        {
            return Enum.GetNames(enumType);
        }
    }
}