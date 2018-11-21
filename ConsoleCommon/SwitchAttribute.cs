using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommon
{
    public class SwitchAttribute : Attribute
    {
        private string[] _switchValues;
        private string _switchName;
        private bool _dontallowvalues;
        private bool _required;
        private int? _defaultordinal;
        private string _helpText;

        public string[] SwitchValues { get { return _switchValues; } }
        public string SwitchName { get { return _switchName; } }
        public bool DontAllowValues { get { return _dontallowvalues; } }
        public bool Required { get { return _required; } }
        public int? DefaultOrdinal { get { return _defaultordinal; } }
        public string HelpText { get { return _helpText; } }

        /// <summary>
        /// Specifies properties for a switch property on a console params object.
        /// </summary>
        /// <param name="switchName">The syntax of the switch. Often a single letter.</param>
        /// <param name="required">Specifies whether the switch is required.</param>
        /// <param name="helpText">Help text used in Usage property.</param>
        /// <param name="dontAllowValues">If set to true, then switch cannot contain input values.</param>
        public SwitchAttribute(string switchName = "", bool required = false, int defaultOrdinal = -1, string helpText = "", bool dontAllowValues = false)
            : this(switchName, required, defaultOrdinal, helpText, new string[0])
        {
            _dontallowvalues = dontAllowValues;
        }
        /// <summary>
        /// Specifies properties for a switch property on a console params object.
        /// </summary>
        /// <param name="switchName">The syntax of the switch. Often a single letter.</param>
        /// <param name="required">Specifies whether the switch is required.</param>
        /// <param name="helpText">Help text used in Usage property.</param>
        /// <param name="switchValues">If values specified, the switch can only contain
        /// the values specified. Otherwise, the switch can contain any value.</param>
        public SwitchAttribute(string switchName = "", bool required = false, int defaultOrdinal = -1, string helpText = "", params string[] switchValues)
        {
            _switchName = switchName;
            _switchValues = switchValues;
            _required = required;
            if (defaultOrdinal != -1) _defaultordinal = defaultOrdinal;
            _helpText = helpText;
        }
    }
}