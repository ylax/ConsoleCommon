using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleCommon.Entities
{
    public class SwitchOptions
    {
        IEnumerable<char> _switchStartChars;
        public IEnumerable<char> SwitchStartChars { get { return _switchStartChars; } }
        IEnumerable<char> _switchEndChars;
        public IEnumerable<char> SwitchEndChars { get { return _switchEndChars; } }
        string _switchNameRegex;
        public string SwitchNameRegex { get { return _switchNameRegex; } }
        string _switchRegex;
        public string SwitchRegex { get { return _switchRegex; } }
        string _flagSwitchRegex;
        public string FlagSwitchRegex { get { return _flagSwitchRegex; } }
        public SwitchOptions(IEnumerable<char> switchStartChars, IEnumerable<char> switchEndChars, string switchNameRegex)
        {
            _switchStartChars = switchStartChars;
            _switchEndChars = switchEndChars;
            _switchNameRegex = switchNameRegex;
            SetSwitchRegex();
        }
        private void SetSwitchRegex()
        {
            if (SwitchStartChars.Count() == 0 || SwitchEndChars.Count() == 0 || SwitchNameRegex.Length == 0)
            {
                throw new Exception("Switch name regex properties must all be set");
            }
            string _startChar = "", _endChar = "";
            //start char
            foreach (char c in SwitchStartChars) _startChar += "," + c;
            _startChar = "[" + _startChar.Substring(1) + "]{1}";
            //end char
            foreach (char c in SwitchEndChars) _endChar += "," + c;
            _endChar = "[" + _endChar.Substring(1) + "]{1}";
            //middle
            string _middle = string.Format("({0})+", SwitchNameRegex);
            //ensure middle can't match start or end char regex:
            bool _middleFails = Regex.Match(SwitchStartChars.ElementAt(0).ToString(), _middle).Success;
            _middleFails = _middleFails && Regex.Match(SwitchEndChars.ElementAt(0).ToString(), _middle).Success;
            if (_middleFails) throw new Exception("Switch name regex cannot match switch start or end characters");
            //whole regex
            _switchRegex = string.Format("{0}{1}{2}", _startChar, _middle, _endChar);
            _flagSwitchRegex = string.Format("{0}{1}\\z", _startChar, _middle);
        }
    }
}
