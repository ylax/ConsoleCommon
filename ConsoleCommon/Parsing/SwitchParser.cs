using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using ConsoleCommon.Entities;

namespace ConsoleCommon.Parsing
{
    public class SwitchParser : ISwitchParser
    {
        ITypeParser _typeParser;
        List<SwitchParameterEntity> _availableSwitchProps;
        List<SwitchParameterEntity> _inputSwitches;
        public List<SwitchParameterEntity> InputSwitches { get { return _inputSwitches; } }
        SwitchOptions _options;
        ParamsObject _paramsObject;

        public SwitchParser(ITypeParser typeParser, ParamsObject paramsObject)
        {
            _typeParser = typeParser;
            _paramsObject = paramsObject;
            GetSwitchOptions();
            GetAvailableSwitchProperties();
        }

        public ParamsObject ParseSwitches(string[] args)
        {
            SeparateSwitchesFlagsDefaults(args);
            SetPropertyValues();
            return _paramsObject;
        }

        public IEnumerable<Exception> ExceptionList
        {
            get
            {
                List<Exception> _exes = new List<Exception>();
                foreach(SwitchParameterEntity _switch in _inputSwitches)
                {
                    if (_switch.MatchException != null) _exes.Add(_switch.MatchException);
                    if (_switch.PropertySetException != null) _exes.Add(_switch.PropertySetException);
                }
                return _exes;
            }
        }
        private void SetPropertyValues()
        {
            foreach(SwitchParameterEntity _switch in _inputSwitches)
            {
                if (_switch.MatchException != null) continue;
                try
                {
                    object _propVal = _typeParser.Parse(_switch.InputValue, _switch.SwitchProperty.PropertyType);
                    _switch.SwitchProperty.SetValue(_paramsObject, _propVal, new object[0]);
                }
                catch(Exception ex)
                {
                    _switch.PropertySetException = ex;
                }
            }
        }

        #region Separation
        private void SeparateSwitchesFlagsDefaults(string[] args)
        {
            //Three types of args:
            //1) no switch (using default ordinal)
            //2) has switch-value pair
            //3) has only switch, no value (ex: /SearchByName)
            _inputSwitches = new List<SwitchParameterEntity>();
            foreach (string _arg in args)
            {
                SwitchParameterEntity _switchProp = null;
                //is switch?
                Match _match = Regex.Match(_arg, _options.SwitchRegex);
                if (_match.Success)
                {
                    _switchProp = GetSwitchOrFlag(_arg, _match, false);
                }
                else
                {
                    //is flag?
                    _match = Regex.Match(_arg, _options.FlagSwitchRegex);
                    if (_match.Success) _switchProp = GetSwitchOrFlag(_arg, _match, true);
                }
                //default
                if(!_match.Success) _switchProp = GetDefault(_arg);
                _inputSwitches.Add(_switchProp);
            }
            //GetDefault() doesn't get the properties for default args.
            GetDefaults();
        }
        private SwitchParameterEntity GetSwitchOrFlag(string arg, Match match, bool isFlag)
        {
            SwitchParameterEntity _newSwitch = new SwitchParameterEntity()
            {
                InputString = arg,
                ParameterType = isFlag ? InputParameterType.Flag : InputParameterType.Switch
            };
            int _length = match.Value.Length - 1;
            if (!isFlag)
            {
                _length--;
                _newSwitch.InputValue = arg.Substring(match.Length);
            }
            _newSwitch.SwitchNameFromInput = match.Value.Substring(1, _length).ToUpper();
            SwitchParameterEntity _pe = _availableSwitchProps.FirstOrDefault(s => s.SwitchAttribute.SwitchName.ToUpper() == _newSwitch.SwitchNameFromInput);
            if (_pe != null)
            {
                _newSwitch.SwitchAttribute = _pe.SwitchAttribute;
                _newSwitch.SwitchProperty = _pe.SwitchProperty;
            }
            else _newSwitch.MatchException = new Exception("Invalid switch!");
            return _newSwitch;
        }     
        private SwitchParameterEntity GetDefault(string arg)
        {
            SwitchParameterEntity _newSwitch = new SwitchParameterEntity()
            {
                InputString = arg,
                ParameterType = InputParameterType.Default,
                InputValue = arg
            };
            return _newSwitch;
        }
        private void GetDefaults()
        {
            try
            {
                IEnumerable<SwitchParameterEntity> _defaults =
                    _availableSwitchProps
                    .Where(s => s.SwitchAttribute.DefaultOrdinal != null)
                    .OrderBy(s => s.SwitchAttribute.DefaultOrdinal);

                IEnumerable<SwitchParameterEntity> _inputDefaults = _inputSwitches.Where(s => s.ParameterType == InputParameterType.Default);

                //if any default ordinals are used, all switches must appear after default ordinals
                //All parameter default ordinal properties must have an ordinal value that precedes the ordinal value of any switches
                bool _hasDefaults = _inputDefaults.Count() > 0;
                if (!_hasDefaults) return;
                int _default = -1;
                bool _nonDefaultParamAppeared = false;
                foreach (SwitchParameterEntity _switch in _inputSwitches)
                {
                    if (_switch.ParameterType != InputParameterType.Default)
                    {
                        _nonDefaultParamAppeared = true;
                        if (_default == -1)
                        {
                            _switch.MatchException = new Exception("Parameters with default ordinals must precede switches!");
                        }
                        else if (
                            _switch.DefaultOrdinal.HasValue &&
                            _inputDefaults.Any(i => i.DefaultOrdinal.HasValue &&
                            i.DefaultOrdinal.Value >= _switch.DefaultOrdinal.Value))
                        {
                            _switch.MatchException = new Exception(
                                "Switched parameters with default ordinals can only be used along with non-switched parameters when their default ordinal is greater than the non-switched parameters' default ordinals");
                        }
                    }
                    else if (_switch.ParameterType == InputParameterType.Default)
                    {
                        if(_nonDefaultParamAppeared)
                        {
                            _switch.MatchException = new Exception("Parameters with default ordinals must precede switches!");
                            break;
                        }
                        _default++;
                        if (_default > _inputSwitches.Count - 1)
                        {
                            _switch.MatchException = new Exception("Too many switches specified!");
                        }
                        else
                        {
                            SwitchParameterEntity _defaultParam = _defaults.ElementAt(_default);
                            _switch.SwitchAttribute = _defaultParam.SwitchAttribute;
                            _switch.SwitchProperty = _defaultParam.SwitchProperty;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Initialization
        private void GetAvailableSwitchProperties()
        {
            _availableSwitchProps = new List<SwitchParameterEntity>();
            foreach(PropertyInfo _pi in _paramsObject.GetType().GetProperties())
            {
                SwitchAttribute _attr = _pi.GetCustomAttribute<SwitchAttribute>();
                if (_attr != null) _availableSwitchProps.Add(new SwitchParameterEntity { SwitchProperty = _pi, SwitchAttribute = _attr });
            }
        }
        private void GetSwitchOptions()
        {
            _options = new SwitchOptions(_paramsObject.Options.SwitchStartChars, _paramsObject.Options.SwitchEndChars, _paramsObject.Options.SwitchNameRegex);
        }
        #endregion
    }
}
