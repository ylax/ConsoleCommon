using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleCommon.Entities
{
    public class SwitchParameterEntity
    {
        public string InputString { get; set; }
        public string InputValue { get; set; }
        public object OutputValue { get; set; }
        public PropertyInfo SwitchProperty { get; set; }
        public SwitchAttribute SwitchAttribute { get; set; }
        public InputParameterType ParameterType { get; set; }
        public string SwitchNameFromInput { get; set; }
        public Exception MatchException { get; set; }
        public Exception PropertySetException { get; set; }
        public int? DefaultOrdinal
        {
            get
            {
                return SwitchAttribute == null ? null : SwitchAttribute.DefaultOrdinal;
            }
        }
    }
}
