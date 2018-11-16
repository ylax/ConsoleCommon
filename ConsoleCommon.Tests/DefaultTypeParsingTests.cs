using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ConsoleCommon;
using ConsoleCommon.Parsing;
using ConsoleCommon.Parsing.TypeParsers;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;
using System.Reflection;
using System.Security;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class DefaultTypeParsingTests
    {
        public enum MyColors
        {
            Orange,
            Blue,
            Yello
        }
        private string[] GetArgs(params string[] args)
        {
            return args;
        }
        [Test]
        public void TestAllDefaultParsing_Is_AllValuesSet()
        {
            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<string[]>("Nums")
                .AddSwitch<bool>("IsItTrue")
                .AddSwitch<KeyValuePair<string, int>>("NameAge")
                .AddSwitch<MyColors>("Color")
                .AddSwitch<int>("Height")
                .AddSwitch<SecureString>("pw")
                .FinishBuilding("/Nums:one,two,three", "/IsItTrue:true", 
                "/NameAge:yiz:30", "/Color:Blue", "/Height:65", "/pw:passw0rd");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsFalse(_parseErr, "Check params failed");
            Assert.IsTrue(_paramObj.GetPropertyValue<string[]>("Nums")[1] == "two");
            Assert.IsTrue(_paramObj.GetPropertyValue<bool>("IsItTrue")==true);
            Assert.IsTrue(_paramObj.GetPropertyValue<KeyValuePair<string, int>>("NameAge").Value == 30);
            Assert.IsTrue(_paramObj.GetPropertyValue<MyColors>("Color") == MyColors.Blue);
            Assert.IsTrue(_paramObj.GetPropertyValue<int>("Height") == 65);
            Assert.IsTrue(_paramObj.GetPropertyValue<SecureString>("pw").Length == 8);
        }
        [Test]
        public void TestAllDefaultParsing_Not_AllValuesSet()
        {
            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<string[]>("Nums")
                .AddSwitch<bool>("IsItTrue")
                .AddSwitch<KeyValuePair<string, int>>("NameAge")
                .AddSwitch<MyColors>("Color")
                .AddSwitch<int>("Height")
                .AddSwitch<SecureString>("pw")
                .FinishBuilding("/IsItTrue:true",
                "/NameAge:yiz:30", "/Color:Blue", "/pw:passw0rd");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsFalse(_parseErr, "Check params failed");
            Assert.IsTrue(_paramObj.GetPropertyValue<string[]>("Nums") == null);
            Assert.IsTrue(_paramObj.GetPropertyValue<bool>("IsItTrue") == true);
            Assert.IsTrue(_paramObj.GetPropertyValue<KeyValuePair<string, int>>("NameAge").Value == 30);
            Assert.IsTrue(_paramObj.GetPropertyValue<MyColors>("Color") == MyColors.Blue);
            Assert.IsTrue(_paramObj.GetPropertyValue<int>("Height") == 0);
            Assert.IsTrue(_paramObj.GetPropertyValue<SecureString>("pw").Length == 8);
        }
        [Test]
        public void TestKeyValueOnlyParsing_Bad()
        {
            ParamsObject _paramObj =
            DynamicParamsCreator
                .Create()
                .OverrideTypeParsers(()=> new TypeParserContainer(false, new KeyValueParser()))
                .AddSwitch<KeyValuePair<string,int>>("NameAge")
                .AddSwitch<string>("FirstName")
                .FinishBuilding("/NameAge:Yizzy:30");

            TypeParserContainer _container = _paramObj.GetPropertyValue<TypeParserContainer>("TypeParser");
            Assert.IsTrue(_container.TypeParsers.Count() == 1, "More than 1 type parser should not have existed");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsTrue(_parseErr, "String and int parsers were not explicated, so this should have failed");
            KeyValuePair<string, int> _namgeAge = 
                _paramObj.GetPropertyValue<KeyValuePair<string, int>>("NameAge");
            Assert.IsNull(_namgeAge.Key, "Name key should have been null");
            Assert.IsTrue(_namgeAge.Value == 0, "Age value should have been null");
        }
        [Test]
        public void TestKeyValueOnlyParsing_Good()
        {
            ParamsObject _paramObj =
            DynamicParamsCreator
                .Create()
                .OverrideTypeParsers(() => new TypeParserContainer(false, new KeyValueParser(), new ObjectParser()))
                .AddSwitch<KeyValuePair<string, int>>("NameAge")
                .AddSwitch<string>("FirstName")
                .FinishBuilding("/NameAge:Yizzy:30");

            TypeParserContainer _container = _paramObj.GetPropertyValue<TypeParserContainer>("TypeParser");
            Assert.IsTrue(_container.TypeParsers.Count() == 2, "Only 2 type parsers should have existed");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsFalse(_parseErr, "String and int parsers were explicated, so this should NOT have failed");

            KeyValuePair<string, int>? _namgeAge = _paramObj.GetPropertyValue<KeyValuePair<string, int>>("NameAge");
            Assert.IsNotNull(_namgeAge);
            Assert.IsTrue(_namgeAge.Value.Key == "Yizzy");
            Assert.IsTrue(_namgeAge.Value.Value == 30);
        }
        [Test]
        public void Test_DateTimeParsing_Bad()
        {
            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<DateTime>("Bday")
                .AddSwitch<int>("Age")
                .AddSwitch<string>("Name")
                .FinishBuilding("/Bday:28/11/1987", "/Age:30", "/Name:Yisrael Lax");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsTrue(_parseErr, "Parsing should have failed b/c incorrect DateTime format");
            Assert.IsTrue(_paramObj.GetPropertyValue<DateTime>("Bday") == default(DateTime));
        }
        [Test]
        public void Test_DateTimeParsing_Good()
        {
            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<DateTime>("Bday")
                .AddSwitch<int>("Age")
                .AddSwitch<string>("Name")
                .FinishBuilding("/Bday:11/28/1987", "/Age:30", "/Name:Yisrael Lax");

            bool _parseErr = false;
            try
            {
                _paramObj.CheckParams();
            }
            catch { _parseErr = true; }
            Assert.IsFalse(_parseErr, "Parsing failed");
            DateTime _bday = _paramObj.GetPropertyValue<DateTime>("Bday");
            Assert.IsTrue(_bday == new DateTime(1987, 11, 28), "Bday is incorrect");
        }
        private T getPropVal<T>(object obj, string propName)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(propName);
            object _val = _prop.GetValue(obj);
            if (_val == null) return default(T);
            return (T)_val;
        }
        private void setPropVal(object obj, string propName, object propVal)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(propName);
            _prop.SetValue(obj, propVal);
        }
    }
}
