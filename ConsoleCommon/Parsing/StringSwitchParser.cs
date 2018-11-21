using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using ConsoleCommon.Entities;

namespace ConsoleCommon.Parsing
{
    public interface IArgumentCreator
    {
        string[] GetArgs(string args, SwitchOptions _options);
    }
    public class DefaultArgumentCreator : IArgumentCreator
    {
        #region Separation
        public string[] GetArgs(string args, SwitchOptions _options)
        {
            //Two types of args:
            //1) no switch (using default ordinal)
            //2) has switch-value pair

            List<string> _matchArgs = new List<string>();

            string _newFlagRegex = _options.FlagSwitchRegex;
            if (_newFlagRegex.EndsWith("\\z")) _newFlagRegex = _newFlagRegex.Substring(0, _newFlagRegex.Length - 2);

            string _switchOrFlagRegex = $"({_options.SwitchRegex}|{_newFlagRegex})";
            Match _match = Regex.Match(args, _switchOrFlagRegex);
            //_match = Regex.Match(args, $"({_options.FlagSwitchRegex})");
            int _lastStartIndex = 0;
            int _lastEndIndex = 0;
            Match _lastMatch = _match;

            while(_match.Success)
            {
                _match = _match.NextMatch();
                if (_match.Success)
                {
                    _lastEndIndex = _match.Index;
                    _matchArgs.Add(args.Substring(_lastStartIndex, _lastEndIndex-_lastStartIndex).Trim());
                    _lastStartIndex = _lastEndIndex;
                    _lastMatch = _match;
                }
                else
                {
                    _matchArgs.Add(args.Substring(_lastStartIndex, args.Length - _lastStartIndex).Trim());
                }
            }
            return _matchArgs.ToArray();
        }

        #endregion
    }
}
