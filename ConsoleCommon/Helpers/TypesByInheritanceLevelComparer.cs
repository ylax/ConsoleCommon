using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommon.Helpers
{
    public class TypesByInheritanceLevelComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            Type _yBaseType = y;
            int _compare = 0;
            int _counter = 0;
            bool _foundMatch = false;
            while (_yBaseType != null)
            {
                if (x == _yBaseType)
                {
                    _foundMatch = true;
                    break;
                }
                _yBaseType = _yBaseType.BaseType;
                _counter++;
            }
            //before or same
            if (_foundMatch)
            {
                if (_counter != 0) _compare = -1;
            }
            else _compare = 1; //after
            return _compare;
        }
    }
}
