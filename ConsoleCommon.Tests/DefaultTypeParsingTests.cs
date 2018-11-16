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
using ConsoleCommon.Tests.Helpers;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class DefaultTypeParsingTests
    {
        [TypeParam("Employee")]
        public class EmployeePersonType { }
        [TypeParam("Customer")]
        public class CustomerPersonType { }
        public enum MyColors
        {
            Orange,
            Blue,
            Yellow
        }
        [Flags]
        public enum MyPets
        {
            None = 0,
            Dog = 2,
            Cat = 4,
            Turtle = 8
        }
        #region FullParsing
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
            Assert.IsTrue(_paramObj.GetPropertyValue<bool>("IsItTrue") == true);
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
        #endregion

        #region KeyValue, string, int Parsing
        [Test]
        public void TestKeyValueOnlyParsing_Bad()
        {
            ParamsObject _paramObj =
            DynamicParamsCreator
                .Create()
                .OverrideTypeParsers(() => new TypeParserContainer(false, new KeyValueParser()))
                .AddSwitch<KeyValuePair<string, int>>("NameAge")
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
        #endregion

        #region DateTime parsing (ObjectParser)
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
        #endregion

        #region Type Parsing
        [Test]
        public void Test_TypeTypeParser_FriendName_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<Type>("PersonType")
                .FinishBuilding("/PersonType:Employee");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj, "PersonType parsing failed");
            Assert.IsTrue(_paramObj.GetPropertyValue<Type>("PersonType") == typeof(EmployeePersonType));
        }
        [Test]
        public void Test_TypeTypeParser_FriendName_Bad()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<Type>("PersonType")
                .FinishBuilding("/PersonType:NotReal");

            Assert.IsTrue(_paramObj.GetPropertyValue<Type>("PersonType") == null);
        }
        [Test]
        public void Test_TypeTypeParser_ClassName_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<Type>("PersonType")
                .FinishBuilding("/PersonType:EmployeePersonType");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj, "PersonType parsing failed");
            Assert.IsTrue(_paramObj.GetPropertyValue<Type>("PersonType") == typeof(EmployeePersonType));
        }
        #endregion

        #region Enum Parsing
        [Test]
        public void Test_BasicEnumParsing_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<MyColors>("Color")
                .FinishBuilding("/Color:Yellow");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj);
            Assert.IsTrue(_paramObj.GetPropertyValue<MyColors>("Color") == MyColors.Yellow);
        }

        [Test]
        public void Test_FlagsEnumParsing_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<MyPets>("Pets")
                .FinishBuilding("/Pets:Dog,Turtle");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj);
            Assert.IsTrue(_paramObj.GetPropertyValue<MyPets>("Pets") == (MyPets.Dog | MyPets.Turtle));
        }
        #endregion

        #region Arrays
        [Test]
        public void TestKeyValueArray_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<KeyValuePair<string, int>[]>("NameAges")
                .FinishBuilding("/NameAges:Yisrael:30,Srully:10,Yitschak:40");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj);
            KeyValuePair<string, int>[] _nameAges = _paramObj.GetPropertyValue<KeyValuePair<string, int>[]>("NameAges");
            Assert.IsNotNull(_nameAges);
            Assert.IsTrue(_nameAges.Length == 3);
            Assert.IsTrue(_nameAges[1].Key == "Srully");
            Assert.IsTrue(_nameAges[1].Value == 10);
        }
        [Test]
        public void TestEnumArray_Good()
        {
            ParamsObject _paramObj = DynamicParamsCreator
                .Create()
                .AddSwitch<MyColors[]>("Colors")
                .FinishBuilding("/Colors:Orange,Yellow");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj);
            MyColors[] _colors = _paramObj.GetPropertyValue<MyColors[]>("Colors");
            Assert.IsNotNull(_colors);
            Assert.IsTrue(_colors.Length == 2);
            Assert.IsTrue(_colors[0] == MyColors.Orange);
            Assert.IsTrue(_colors[1] == MyColors.Yellow);
        }
        #endregion
    }
}
