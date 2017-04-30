using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.Text;

namespace ConsoleCommon
{
    public abstract class ParamsObject
    {
        #region Fields and Properties
        private Dictionary<string, string> _switchValues = new Dictionary<string, string>();
        protected string[] args;
        protected List<SwitchProperty> SwitchProperties = new List<SwitchProperty>();
        private Dictionary<Func<bool>, string> _paramExceptionDictionary;
        private IEnumerable<PropertyInfo> _switchMembers
        {
            get
            {
                return this.GetType().GetProperties().Where(pi => pi.GetCustomAttributes<SwitchAttribute>().Count() > 0);
            }
        }
        public Dictionary<string, string> SwitchValues { get { return _switchValues; } }
        public virtual int HelpTextLength
        {
            get { return 160; }
        }
        public virtual int HelpTextIndentLength
        {
            get { return 22; }
        }
        #endregion

        #region Constructors
        public ParamsObject(string[] args)
        {
            try
            {
                this.args = args;
                if (GetHelpIfNeeded() == string.Empty)
                {
                    _switchValues = GetInputValuesFromArgs(args);
                    SwitchProperties = GetSwitchPropertyList(SwitchValues).ToList();
                }
                _paramExceptionDictionary = new Dictionary<Func<bool>, string>();
                AddAdditionalParamChecks();
                foreach(var item in GetParamExceptionDictionary()) _paramExceptionDictionary.Add(item.Key, item.Value);
                FillParamProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Fill Methods
        private Dictionary<string, string> GetInputValuesFromArgs(string[] args)
        {
            //if all required switch properties have a default ordinal
            //and any switches specified do not have a default ordinal
            //then goto GetParamValuesFromArgs
            //otherwise, goto GetSwitchValuesFromArgs
            //
            //search for no switches
            bool _hasInvalidSwitches = false;
            bool _hasSwitches = false;

            Dictionary<string, string> InputValues = new Dictionary<string, string>();
            foreach(string arg in args)
            {
                Regex rx = new Regex("/[A-Za-z]*:");
                Match found = rx.Match(arg);
                _hasInvalidSwitches = found.Success;
                if (_hasInvalidSwitches) //check if switch property does not have default ordinal
                {
                    if (!_hasSwitches) _hasSwitches = true;
                    string myName = found.Value.Substring(1, found.Value.Length - 2).ToUpper();
                    PropertyInfo sPi =
                        this.GetType().GetProperties()
                        .Where(pi =>
                            pi.GetCustomAttribute<SwitchAttribute>() != null &&
                            pi.GetCustomAttribute<SwitchAttribute>().SwitchName.ToUpper() == myName)
                        .FirstOrDefault();
                    //If switch prop does not exist goto GetSwitchValuesFromArgs
                    _hasInvalidSwitches = sPi == null;
                    if (_hasInvalidSwitches) break;
                    SwitchAttribute switchAtr = sPi.GetCustomAttribute<SwitchAttribute>();
                    //If switch prop has a default ordinal throw error
                    //_hasInvalidSwitches = switchAtr.DefaultOrdinal != null;
                    if (_hasInvalidSwitches) throw new Exception("Cannot use switches for parameters with a default ordinal while using default parameter order!");
                }
                else
                {
                    //If a switch appears before a non switch goto GetSwitchValuesFromArgs
                    if (_hasSwitches)
                    {
                        throw new Exception("Switches must be placed after parameters with default ordinal while using default parameter order!");
                    }
                }
            }
            if (_hasInvalidSwitches)
            {
                InputValues= GetSwitchValuesFromArgs(args);
            }
            else
            {
                //If all required props have a default ordinal goto GetSwitchValuesFromParams
                bool allRequiredPropsHaveDefaultOrdinal = false;
                foreach (PropertyInfo switchProp in this.GetType().GetProperties().Where(p => p.GetCustomAttribute<SwitchAttribute>() != null))
                {
                    SwitchAttribute switchAttr = switchProp.GetCustomAttribute<SwitchAttribute>();
                    allRequiredPropsHaveDefaultOrdinal = !switchAttr.Required || (switchAttr.Required && switchAttr.DefaultOrdinal != null);
                    if (!allRequiredPropsHaveDefaultOrdinal) break;
                }
                if (!allRequiredPropsHaveDefaultOrdinal)
                {
                    InputValues= GetSwitchValuesFromArgs(args);
                }
                //Otherwise goto GetSwitchValuesFromArgs
                else InputValues=GetParamValuesFromArgs(args);
            }
            return InputValues;
        }
        private Dictionary<string, string> GetSwitchValuesFromArgs(string[] args)
        {
            Dictionary<string, string> switchValues = new Dictionary<string, string>();
            foreach (string arg in args)
            {
                // looking for '/ABC:'
                Regex rx = new Regex("/[A-Za-z]*:");
                Match found = rx.Match(arg);
                if (!found.Success || found.Index != 0) throw new Exception("Invalid switch!");
                string myName = found.Value.Substring(1, found.Value.Length - 2).ToUpper();
                if (this.GetType().GetProperties()
                    .Where(pi => 
                        pi.GetCustomAttribute<SwitchAttribute>() != null &&
                        pi.GetCustomAttribute<SwitchAttribute>().SwitchName.ToUpper()==myName)
                    .Count() == 0
                    ) throw new Exception("Switch invalid!");
                string myValue = arg.Substring(found.Length);
                switchValues.Add(myName, myValue);
            }
            return switchValues;
        }
        private Dictionary<string,string> GetParamValuesFromArgs(string[] args)
        {
            ///Used when no switches specified in inputs
            ///User is relying on default param order
            Dictionary<string, string> switchValues = new Dictionary<string, string>();
            //Get attributes sorted by ordinal
            List<SwitchAttribute> sortedAttributes =
                this.GetType().GetProperties()
                .Where(pi => pi.GetCustomAttribute<SwitchAttribute>() != null)
                .Select(pi => pi.GetCustomAttribute<SwitchAttribute>())
                .Where(sa => sa.DefaultOrdinal != null)
                .OrderBy(sa => sa.DefaultOrdinal)
                .ToList();

            //Loop through args, associate non switch values with switch names by ordinal
            for (int i = 0; i < args.Length;i++)
            {
                string arg = args[i];
                // looking for '/ABC:'
                Regex rx = new Regex("/[A-Za-z]*:");
                Match found = rx.Match(arg);
                string myName = "";
                string myValue = "";
                //Not a switch
                if(!found.Success || found.Index!=0)
                {
                    if (sortedAttributes.Count < i + 1) throw new Exception("Parameter mismatch!");
                    myName = sortedAttributes[i].SwitchName.ToUpper();
                    myValue = arg;
                }
                //Is a switch
                else
                {
                    myName = found.Value.Substring(1, found.Value.Length - 2).ToUpper();
                    if (this.GetType().GetProperties()
                        .Where(pi =>
                            pi.GetCustomAttribute<SwitchAttribute>() != null &&
                            pi.GetCustomAttribute<SwitchAttribute>().SwitchName.ToUpper() == myName)
                        .Count() == 0
                        ) throw new Exception("Invalid switch!");
                    myValue = arg.Substring(found.Length);
                }
                switchValues.Add(myName, myValue);
            }
            return switchValues;
        }
        private void FillParamProperties()
        {
            foreach (PropertyInfo pi in _switchMembers)
            {
                SwitchAttribute mySwitch = pi.GetCustomAttribute<SwitchAttribute>();
                KeyValuePair<string, string> switchVal = SwitchValues.Where(t => t.Key.ToLower() == mySwitch.SwitchName.ToLower()).FirstOrDefault();
                if (!switchVal.Equals(default(KeyValuePair<string, string>)))
                {
                    object propVal = switchVal.Value;
                    object myVal = StringToPrimitiveParser.Parse(propVal as string, pi.PropertyType);

                    if (myVal != null) pi.SetValue(this, myVal,null);
                }
            }
        }
        private IEnumerable<SwitchProperty> GetSwitchPropertyList(Dictionary<string, string> mySwitchValues)
        {
            List<PropertyInfo> switchPropInfos = _switchMembers.ToList();
            List<SwitchProperty> switchProps = new List<SwitchProperty>();
            foreach(var switchVal in mySwitchValues)
            {
                SwitchProperty sProp = new SwitchProperty();
                sProp.SwitchName = switchVal.Key;
                sProp.SwitchValue = switchVal.Value;
                sProp.SwitchPropertyInfo = switchPropInfos.Where(pi => pi.GetCustomAttribute<SwitchAttribute>().SwitchName.ToUpper() == sProp.SwitchName.ToUpper()).FirstOrDefault();
                if(sProp.SwitchPropertyInfo!=null) sProp.SwitchAttribute = sProp.SwitchPropertyInfo.GetCustomAttribute<SwitchAttribute>();
                switchProps.Add(sProp);
            }
            return switchProps;
        }
        public virtual Dictionary<Func<bool>, string> GetParamExceptionDictionary()
        {
            return new Dictionary<Func<bool>, string>();
        }
        #endregion

        #region Process Flow Methods
        private void AddAdditionalParamChecks()
        {
            foreach (PropertyInfo pi in _switchMembers)
            {
                AddRequiredCheck(pi);
                AddValueListCheck(pi);
            }
        }
        private void AddRequiredCheck(PropertyInfo switchMember)
        {
            SwitchAttribute mySwitch = switchMember.GetCustomAttribute<SwitchAttribute>();
            Func<bool> isRequiredFilled = new Func<bool>(() => mySwitch.Required && switchMember.GetValue(this,null) == null);
            _paramExceptionDictionary.Add(isRequiredFilled, string.Format("Parameter {0} is required!", switchMember.Name));
        }
        private void AddValueListCheck(PropertyInfo switchMember)
        {
            SwitchAttribute mySwitch = switchMember.GetCustomAttribute<SwitchAttribute>();
            Func<bool> isRestrictedValue = new Func<bool>(() => mySwitch.SwitchValues.Length!=0 &
                (
                    //is null?
                    switchMember.GetValue(this,null)==null ||
                    /*//or is a Type and Type.Name = a switch value?
                    mySwitch.SwitchValues.Where(s=> 
                        switchMember.PropertyType.Equals(typeof(Type)) && 
                        ((Type)switchMember.GetValue(this,null)).UnderlyingSystemType.MatchesAttributeValueOrName<TypeParamAttribute>(s,attr=> attr == null ? "" : attr.FriendlyName)).Count() == 0 ||
                    //or is something else and object.ToString() = a switch value?*/
                        mySwitch.SwitchValues.Where(s=> 
                            s.ToLower() == switchMember.GetValue(this, null).ToString().ToLower()).Count() == 0
                ));
            _paramExceptionDictionary.Add(isRestrictedValue, string.Format("Invalid value for parameter {0}!", switchMember.Name));
        }

        public void CheckParams()
        {
            try
            {
                if (GetHelpIfNeeded() != string.Empty) return;
                foreach (var item in _paramExceptionDictionary)
                {
                    if (item.Key()) throw new Exception(item.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetHelpIfNeeded()
        {
            if (args[0] == "/?" || args[0] == "help" || args[0] == "/help")
            {
                return GetHelp();
            }
            else return string.Empty;
        }
        /// <summary>
        /// Override this method to write custom help text. This will over ride automatic help text generation.
        /// </summary>
        /// <returns></returns>
        public virtual string GetHelp()
        {
            return GetHelp2();
        }
        /// <summary>
        /// Ordinals for properties defined on base class take precedence
        /// </summary>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetHelpMethodsSorted()
        {
            //Sort
            List<PropertyInfo> helpMethods = this.GetType().GetProperties()
                .Where(p => p.PropertyType.IsAssignableFrom(
                    typeof(String)) &&
                    p.GetCustomAttribute<HelpTextAttribute>() != null)
                 .OrderBy(s => s.GetCustomAttribute<HelpTextAttribute>().Ordinal)
                 .ToList();

            //base class props don't move in ordinal
            IEnumerable<PropertyInfo> baseClassMethods = this.GetType().BaseType.GetProperties()
                .Where(p => p.PropertyType.IsAssignableFrom(
                    typeof(String)) &&
                    p.GetCustomAttribute<HelpTextAttribute>() != null);

            foreach(PropertyInfo pi in baseClassMethods)
            {
                //find pi in helpMethods
                //remove it, then place it in its correct position
                HelpTextAttribute attr = pi.GetCustomAttribute<HelpTextAttribute>();
                PropertyInfo helpMethodPi = helpMethods.Where(p => p.Name==pi.Name).FirstOrDefault();
                bool success = helpMethods.Remove(helpMethodPi);
                helpMethods.Insert(attr.Ordinal, helpMethodPi);
            }
            return helpMethods;
        }

        #endregion

        #region Public Methods
        public string GetHelp2()
        {

            string _helptext="";
            string _dash = "";
            for (int i = 0; i < HelpTextLength; i++) _dash += "-";
            _helptext += _dash;
            //Sort
            IEnumerable<PropertyInfo> helpMethods = GetHelpMethodsSorted();
            foreach(PropertyInfo prop in helpMethods)
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
                    _propText = prop.GetValue(this,null).ToString();
                }
                _name = _name.ToUpper();
                _helptext += Environment.NewLine + string.Format("{0," + (HelpTextIndentLength * -1).ToString() + "} {1}", _name + ":", _propText);
            }
            _helptext += _dash;
            return string.Join(Environment.NewLine, GetLines(_helptext,28));
        }

        #endregion

        #region Default Help Text Methods

        public virtual string Usage
        {
            get
            {
                string _usage=System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location) + " ";
                IEnumerable<SwitchAttribute> switchAttrs = GetOrderedSwitches();
                foreach(SwitchAttribute switchAttr in switchAttrs)
                {
                    string _name = switchAttr.SwitchName;
                    _usage += string.Format("/{0}:\"{1}\" ", _name.ToUpper(), GetPropertyByAttribute(switchAttr).Name);
                }
                return _usage;
            }
        }
        public virtual string SwitchHelp
        {
            get
            {
                int _atLeastOne = 0;
                IEnumerable<SwitchAttribute> switchAttrs = GetOrderedSwitches();
                ///If allowed to call with no default ordinal
                bool defaultOrdinalAllowed=switchAttrs.Where(sa=>sa.Required && sa.DefaultOrdinal==null).Count() == 0;
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
                foreach(SwitchAttribute switchAttr in switchAttrs)
                {
                    SwitchHelpTextAttribute helpText = GetPropertyByAttribute(switchAttr).GetCustomAttribute<SwitchHelpTextAttribute>();
                    if (helpText == null) continue;
                    _atLeastOne++;
                    string _name = helpText.Name;
                    string _description = helpText.Description;
                    string _isoptional = switchAttr.Required ? "Required" : "Optional";
                    string _acceptedValues = "";
                    string _noDefaultOrdinal = defaultOrdinalAllowed && switchAttr.DefaultOrdinal.HasValue ? "" : "No default ordinal. ";
                    string[] _acceptedValArr = StringToPrimitiveParser.GetSpecificTypeAcceptedValues(GetPropertyByAttribute(switchAttr).PropertyType);
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
                    _help += Environment.NewLine + string.Format("/{0," + (HelpTextIndentLength * -1).ToString() + "} {1}. {2}{3}. {4}", _name + ": ", _isoptional, _noDefaultOrdinal, _description, _acceptedValues);
                }
                _help += Environment.NewLine;
                return _help;
            }
        }
        
        #endregion

        #region Helper Methods
        public string GetLines(string text, int wrapMargin)
        {
            int pos, next;
            int width = this.HelpTextLength;
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
        private int BreakLine(string text, int pos, int max)
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

        private IEnumerable<SwitchAttribute> GetOrderedSwitches()
        {
            IEnumerable<SwitchAttribute> allAttrs =
                this.GetType().GetProperties()
                .Where(pi => pi.GetCustomAttribute<SwitchAttribute>() != null)
                .Select(pi => pi.GetCustomAttribute<SwitchAttribute>());

            IEnumerable<SwitchAttribute> withOrdinals = allAttrs.Where(sa => sa.DefaultOrdinal != null).OrderBy(sa => sa.DefaultOrdinal);
            IEnumerable<SwitchAttribute> noOrdinals = allAttrs.Where(sa => sa.DefaultOrdinal == null);
            return withOrdinals.Concat(noOrdinals);
        }
        private PropertyInfo GetPropertyByAttribute(Attribute attr)
        {
            return this.GetType().GetProperties().Where(pi => pi.GetCustomAttributes(true).Contains(attr)).FirstOrDefault();
        }
        #endregion
    }
}