using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.Text;
using ConsoleCommon.Entities;
using ConsoleCommon.Parsing;
using ConsoleCommon.HelpText;
using ConsoleCommon.Parsing.TypeParsers;
using ConsoleCommon.Helpers;

namespace ConsoleCommon
{
    public abstract class ParamsObject
    {
        #region Fields and Properties
        protected string[] args;
        private Dictionary<Func<bool>, string> _paramExceptionDictionary;
        private IEnumerable<PropertyInfo> _switchMembers
        {
            get
            {
                return this.GetType().GetProperties().Where(pi => pi.GetCustomAttributes<SwitchAttribute>().Count() > 0);
            }
        }

        #region Help Options
        public virtual int HelpTextLength
        {
            get { return 160; }
        }
        public virtual int HelpTextIndentLength
        {
            get { return 22; }
        }
        public virtual List<string> HelpCommands
        {
            get { return new List<string> { "/?", "help", "/help" }; }
        }
        protected virtual IHelpTextParser HelpTextParser { get { return _defaultHelpTextParser; } }
        HelpTextOptions _helpOptions;
        IHelpTextParser _defaultHelpTextParser;
        #endregion

        #region Parsing Options
        SwitchOptions _defaultSwitchOptions;
        public virtual SwitchOptions Options { get { return _defaultSwitchOptions; } }
        
        ISwitchParser _defaultSwitchParser;
        protected virtual ISwitchParser SwitchParser { get { return _defaultSwitchParser; } }

        ITypeParserContainer _defaultTypeParserContainer;
        protected virtual ITypeParserContainer TypeParser { get { return _defaultTypeParserContainer; } }

        IArgumentCreator _defaultArgMaker;
        protected virtual IArgumentCreator ArgMaker { get { return _defaultArgMaker; } }
        #endregion

        #endregion

        #region Constructors
        private void Initialize()
        {
            _defaultSwitchOptions = new SwitchOptions(new List<char> { '/' }, new List<char> { ':' }, "[_A-Za-z]+[_A-Za-z0-9]*");
            _defaultTypeParserContainer = new DefaultTypeContainer();
            _defaultSwitchParser = new SwitchParser(TypeParser, this);
            _helpOptions = new HelpTextOptions(HelpTextLength, HelpTextIndentLength, HelpCommands);
            _defaultHelpTextParser = new BasicHelpTextParser(_helpOptions, TypeParser);
            _defaultArgMaker = new DefaultArgumentCreator();
        }
        private void PostInitialize()
        {
            if (GetHelpIfNeeded() == string.Empty)
            {
                SwitchParser.ParseSwitches(args);
            }
            _paramExceptionDictionary = new Dictionary<Func<bool>, string>();
            AddAdditionalParamChecks();
            foreach (var item in GetParamExceptionDictionary()) _paramExceptionDictionary.Add(item.Key, item.Value);
        }
        public ParamsObject() : this(
            CommandLineHelpers.RemoveAppNameFromArgs(Environment.CommandLine, Environment.CurrentDirectory))
        {
        }
        public ParamsObject(string commandText)
        {
            try
            {
                Initialize();
                this.args = ArgMaker.GetArgs(commandText, Options, HelpTextParser);
                PostInitialize();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ParamsObject(string[] args)
        {
            try
            {
                Initialize();
                this.args = args;
                PostInitialize();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Fill Methods
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
            Func<bool> isRequiredFilled = new Func<bool>(() => mySwitch.Required && switchMember.GetValue(this, null) == null);
            _paramExceptionDictionary.Add(isRequiredFilled, string.Format("Parameter {0} is required!", switchMember.Name));
        }
        private void AddValueListCheck(PropertyInfo switchMember)
        {
            SwitchAttribute mySwitch = switchMember.GetCustomAttribute<SwitchAttribute>();
            Func<bool> isRestrictedValue = new Func<bool>(() => mySwitch.SwitchValues.Length != 0 &
                (
                    //is null?
                    switchMember.GetValue(this, null) == null ||
                        /*//or is a Type and Type.Name = a switch value?
                        mySwitch.SwitchValues.Where(s=> 
                            switchMember.PropertyType.Equals(typeof(Type)) && 
                            ((Type)switchMember.GetValue(this,null)).UnderlyingSystemType.MatchesAttributeValueOrName<TypeParamAttribute>(s,attr=> attr == null ? "" : attr.FriendlyName)).Count() == 0 ||
                        //or is something else and object.ToString() = a switch value?*/
                        mySwitch.SwitchValues.Where(s =>
                            s.ToLower() == switchMember.GetValue(this, null).ToString().ToLower()).Count() == 0
                ));
            _paramExceptionDictionary.Add(isRestrictedValue, string.Format("Invalid value for parameter {0}!", switchMember.Name));
        }

        public void CheckParams()
        {
            try
            {
                if (GetHelpIfNeeded() != string.Empty) return;
                Exception _ex = SwitchParser.ExceptionList.FirstOrDefault();
                if (_ex != null) throw _ex;
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
            return HelpTextParser.GetHelpIfNeeded(args, this);
        }
        /// <summary>
        /// Override this method to write custom help text. This will over ride automatic help text generation.
        /// </summary>
        /// <returns></returns>
        public virtual string GetHelp()
        {
            return GetHelp2();
        }

        #endregion

        #region Public Methods
        public string GetHelp2()
        {

            return HelpTextParser.GetHelp(this);
        }

        #endregion

        #region Default Help Text Methods

        public virtual string Usage
        {
            get
            {
                return HelpTextParser.GetUsage(this);
            }
        }
        public virtual string SwitchHelp
        {
            get
            {
                return HelpTextParser.GetSwitchHelp(this);
            }
        }

        #endregion
    }
}