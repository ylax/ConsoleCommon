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
        public static object GetPropertyValue(this object src, string propName, int index = -1)
        {
            object[] _index = getPropIndex(index);
            return getProp(src, propName).GetValue(src, _index);
        }
        public static T GetPropertyValue<T>(this object src, string propName, int index = -1)
        {
            PropertyInfo _prop = getProp(src, propName);
            if (!_prop.PropertyType.IsAssignableFrom(typeof(T))) throw new Exception($"Property '{propName}' does not match type '{typeof(T).Name}'");
            object[] _index = getPropIndex(index);
            object _propVal = _prop.GetValue(src, _index);

            //return null if null
            if (typeof(T).IsClass && _propVal == null) return default(T);

            //return
            return (T)_propVal;
        }
        public static bool CompareValues(object val1, object val2)
        {
            bool _isEqual = false;
            if (val1 == null || val2 == null)
            {
                _isEqual = val1 == null && val2 == null;
            }
            else if(val1!=null && val2!=null && val1.GetType().IsAssignableFrom(typeof(string)))
            {
                _isEqual = val1.ToString() == val2.ToString();
            }
            else
            {
                _isEqual = val1 == val2;
            }
            return _isEqual;
        }
        private static PropertyInfo getProp(object src, string propName)
        {
            PropertyInfo _prop = src.GetType().GetProperties().FirstOrDefault(p=>p.Name == propName);
            if (_prop == null) throw new Exception($"Property '{propName}' not found");
            return _prop;
        }
        private static object[] getPropIndex(int index = -1)
        {
            object[] _index = null;
            if (index > 0) _index = new object[] { index };
            return _index;
        }
    }
}
