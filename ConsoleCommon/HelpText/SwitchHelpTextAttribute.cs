using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommon
{
    /// <summary>
    /// Attribute used to decorate a switch property on a ParamsObject to specify help text to return for the switch property descriptions' section.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchHelpTextAttribute : HelpTextAttribute
    {
        private string _description;
        public string Description
        {
            get { return _description; }
        }
        public SwitchHelpTextAttribute(string description, string name = "")
            : base(0, name)
        {
            _description = description;
        }
    }
}