using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommon
{
    /// <summary>
    /// Attribute used to decorate a property on a ParamsObject that returns a help text element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]

    public class HelpTextAttribute : Attribute
    {
        string _name="";
        int _ordinal = -1;
        public string Name
        {
            get { return _name; }
        }
        public int Ordinal
        {
            get { return _ordinal; }
        }
        public HelpTextAttribute(int ordinal, string name="")
        {
            _ordinal = ordinal;
            _name = name;
        }
    }
}
