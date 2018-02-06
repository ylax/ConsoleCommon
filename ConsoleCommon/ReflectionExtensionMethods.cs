using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleCommon
{
    public static class ReflectionExtensionMethods
    {
        public static T GetCustomAttribute<T>(this PropertyInfo pi) where T: Attribute
        {
            return pi.GetCustomAttributes(true).Cast<Attribute>().Where(atr => atr.GetType().IsEquivalentTo(typeof(T))).FirstOrDefault() as T;
        }
        public static IEnumerable<T> GetCustomAttributes<T>(this PropertyInfo pi) where T : Attribute
        {
            List<T> _list =  
                pi.GetCustomAttributes(true)
                .Cast<Attribute>()
                .Where(atr => atr.GetType().IsEquivalentTo(typeof(T)))
                .Cast<T>()
                .ToList();
            return _list as IEnumerable<T>;
        }
        public static T GetCustomAttribute<T>(this Type srcType) where T : Attribute
        {
            object[] _attrs = srcType.GetCustomAttributes(true);
            if (_attrs != null && _attrs.Length > 0)
            {
                return srcType.GetCustomAttributes(true).Cast<Attribute>().Where(atr => atr.GetType().IsEquivalentTo(typeof(T))).FirstOrDefault() as T;
            }
            else return null;
        }
        public static bool MatchesAttributeValueOrName<TAttribute>(this Type srcType, string fieldName, Func<TAttribute, string> KeySelector)
            where TAttribute : Attribute
        {
            try
            {
                bool _match = false;
                TAttribute _attr = null;
                try
                {
                    _attr = srcType.GetCustomAttribute<TAttribute>();
                }
                catch(Exception ex)
                {
                    throw new Exception("The error occured while GetCustomAttribute happened", ex);
                }
                if (_attr != null)
                {
                    string _attrFieldName = "";
                    try
                    {
                        if (KeySelector == null) throw new Exception("Your key selector is null!");
                        _attrFieldName = KeySelector(_attr);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("The error happened while selecting a key", ex);
                    }
                    _match = !string.IsNullOrWhiteSpace(_attrFieldName) && _attrFieldName.ToLower() == fieldName.ToLower();
                }
                if (!_match) _match = fieldName.ToLower() == srcType.Name.ToLower();
                return _match;
            }
            catch(Exception ex)
            {
                throw new Exception("Error while reflecting", ex);
            }
        }
        public static PropertyInfo GetPropertyByAttribute(this object src, Attribute attr)
        {
            return src.GetType().GetProperties().Where(pi => pi.GetCustomAttributes(true).Contains(attr)).FirstOrDefault();
        }
    }
}
