using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleCommon
{
    public class SwitchProperty
    {
        public string SwitchName { get; set; }
        public object SwitchValue { get; set; }
        public PropertyInfo SwitchPropertyInfo { get; set; }
        public SwitchAttribute SwitchAttribute { get; set; }
    }
}
