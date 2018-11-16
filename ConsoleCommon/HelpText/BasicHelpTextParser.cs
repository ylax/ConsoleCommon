using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ConsoleCommon.Parsing;

namespace ConsoleCommon.HelpText
{
    public class BasicHelpTextParser : IHelpTextParser
    {
        HelpTextOptions _options;
        ITypeParserContainer _parser;
        public BasicHelpTextParser(HelpTextOptions options, ITypeParserContainer parser)
        {
            _options = options;
            _parser = parser;
        }
        public virtual string GetUsage(ParamsObject InputParams)
        {
            string _appName = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            if (_appName != null)
            {
                _appName = System.IO.Path.GetFileName(_appName);
            }
            else _appName = "AppName.exe";
            string _usage = _appName + " ";
            IEnumerable<SwitchAttribute> switchAttrs = getOrderedSwitches(InputParams);
            foreach (SwitchAttribute switchAttr in switchAttrs)
            {
                string _name = switchAttr.SwitchName;
                _usage += string.Format("/{0}:\"{1}\" ", _name.ToUpper(), InputParams.GetPropertyByAttribute(switchAttr).Name);
            }
            return _usage;
        }
        public virtual string GetSwitchHelp(ParamsObject InputParams)
        {
            int _atLeastOne = 0;
            IEnumerable<SwitchAttribute> switchAttrs = getOrderedSwitches(InputParams);
            ///If allowed to call with no default ordinal
            bool defaultOrdinalAllowed = switchAttrs.Where(sa => sa.Required && sa.DefaultOrdinal == null).Count() == 0;
            string _defaultOrdinalMessage = "";
            if (!defaultOrdinalAllowed)
            {
                _defaultOrdinalMessage = "Anonymous parameters not allowed.";
            }
            else
            {
                _defaultOrdinalMessage = "Anonymous parameters must be passed in their default ordinal, as listed below.";
            }
            string _help = _defaultOrdinalMessage + Environment.NewLine;
            foreach (SwitchAttribute switchAttr in switchAttrs)
            {
                SwitchHelpTextAttribute helpText = InputParams.GetPropertyByAttribute(switchAttr).GetCustomAttribute<SwitchHelpTextAttribute>();
                if (helpText == null) continue;
                _atLeastOne++;
                string _name = helpText.Name;
                string _description = helpText.Description;
                string _isoptional = switchAttr.Required ? "Required" : "Optional";
                string _acceptedValues = "";
                string _noDefaultOrdinal = defaultOrdinalAllowed && switchAttr.DefaultOrdinal.HasValue ? "" : "No default ordinal. ";
                string[] _acceptedValArr = _parser.GetAcceptedValues(InputParams.GetPropertyByAttribute(switchAttr).PropertyType);
                if (_acceptedValArr.Length == 0) _acceptedValArr = switchAttr.SwitchValues;
                if (_acceptedValArr.Length > 0)
                {
                    _acceptedValues = _acceptedValArr.Length == 0 ? "" : string.Format("Accepted Values: {0}`{1}`.",
                        string.Concat(
                            _acceptedValArr
                            .Take(_acceptedValArr.Length - 1)
                            .Select(s => "`" + s.ToUpper() + "`| ")
                            ),
                        _acceptedValArr.ElementAt(_acceptedValArr.Length - 1).ToUpper());
                }
                if (string.IsNullOrEmpty(_name)) _name = switchAttr.SwitchName;
                _name = _name.ToUpper();
                _help += Environment.NewLine + string.Format("/{0," + (_options.HelpTextIndentLength * -1).ToString() + "} {1}. {2}{3}. {4}", _name + ": ", _isoptional, _noDefaultOrdinal, _description, _acceptedValues);
            }
            _help += Environment.NewLine;
            return _help;
        }
        public virtual string GetHelp(ParamsObject InputParams)
        {
            string _helptext = "";
            string _dash = "";
            for (int i = 0; i < _options.HelpTextLength; i++) _dash += "-";
            _helptext += _dash;
            //Sort
            IEnumerable<PropertyInfo> helpMethods = getHelpMethodsSorted(InputParams);
            foreach (PropertyInfo prop in helpMethods)
            {
                string _propText = "";
                string _name = "";
                HelpTextAttribute attr = prop.GetCustomAttribute<HelpTextAttribute>();
                if (typeof(SwitchHelpTextAttribute).IsAssignableFrom(attr.GetType()))
                {
                    continue;
                }
                else
                {
                    _name = attr.Name;
                    if (string.IsNullOrEmpty(_name)) _name = prop.Name;
                    _propText = prop.GetValue(InputParams, null).ToString();
                }
                _name = _name.ToUpper();
                _helptext += Environment.NewLine + string.Format("{0," + (_options.HelpTextIndentLength * -1).ToString() + "} {1}", _name + ":", _propText);
            }
            _helptext += _dash;
            return string.Join(Environment.NewLine, GetLines(_helptext, 28, _options.HelpTextLength));
        }
        public virtual string GetHelpIfNeeded(string[] args, ParamsObject InputParams)
        {
            bool _needsHelp = args != null && args.Count() > 0 && _options.HelpCommands.Select(c=>c.ToUpper()).Contains(args[0].ToUpper());
            if (_needsHelp) return GetHelp(InputParams);
            else return string.Empty;
        }
        #region Helper Methods
        protected IEnumerable<SwitchAttribute> getOrderedSwitches(ParamsObject inputParams)
        {
            IEnumerable<SwitchAttribute> allAttrs =
                inputParams.GetType().GetProperties()
                .Where(pi => pi.GetCustomAttribute<SwitchAttribute>() != null)
                .Select(pi => pi.GetCustomAttribute<SwitchAttribute>());

            IEnumerable<SwitchAttribute> withOrdinals = allAttrs.Where(sa => sa.DefaultOrdinal != null).OrderBy(sa => sa.DefaultOrdinal);
            IEnumerable<SwitchAttribute> noOrdinals = allAttrs.Where(sa => sa.DefaultOrdinal == null);
            return withOrdinals.Concat(noOrdinals);
        }
        /// <summary>
        /// Ordinals for properties defined on base class take precedence
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<PropertyInfo> getHelpMethodsSorted(ParamsObject inputParams)
        {
            //Sort
            List<PropertyInfo> helpMethods = inputParams.GetType().GetProperties()
                .Where(p => p.PropertyType.IsAssignableFrom(
                    typeof(String)) &&
                    p.GetCustomAttribute<HelpTextAttribute>() != null)
                 .OrderBy(s => s.GetCustomAttribute<HelpTextAttribute>().Ordinal)
                 .ToList();

            //base class props don't move in ordinal
            IEnumerable<PropertyInfo> baseClassMethods = inputParams.GetType().BaseType.GetProperties()
                .Where(p => p.PropertyType.IsAssignableFrom(
                    typeof(String)) &&
                    p.GetCustomAttribute<HelpTextAttribute>() != null);

            foreach (PropertyInfo pi in baseClassMethods)
            {
                //find pi in helpMethods
                //remove it, then place it in its correct position
                HelpTextAttribute attr = pi.GetCustomAttribute<HelpTextAttribute>();
                PropertyInfo helpMethodPi = helpMethods.Where(p => p.Name == pi.Name).FirstOrDefault();
                bool success = helpMethods.Remove(helpMethodPi);
                helpMethods.Insert(attr.Ordinal, helpMethodPi);
            }
            return helpMethods;
        }
        protected string GetLines(string text, int wrapMargin, int helpTextLen)
        {
            int pos, next;
            int width = helpTextLen;
            StringBuilder sb = new StringBuilder();
            string margin = "";
            for (int i = 0; i < wrapMargin; i++) margin += " ";

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        if (pos < eol) sb.Append(margin);
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }
        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        protected int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
        #endregion
    }
}
