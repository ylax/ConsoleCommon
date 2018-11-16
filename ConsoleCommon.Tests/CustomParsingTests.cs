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
using System.Globalization;
using ConsoleCommon.Tests.Helpers;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class CustomParsingTests
    {
        public class DateTimeParser : TypeParserBase<DateTime>
        {
            public DateTimeParser() : base()
            {

            }
            public override object Parse(string toParse, Type typeToParse, ITypeParserContainer parserContainer)
            {
                CultureInfo _culture = CultureInfo.CreateSpecificCulture("");
                return DateTime.ParseExact(toParse, "dd-MM-yyyy", _culture);
            }
        }
        [Test]
        public void Test_Override_DateTimeParsing_Bad()
        {

            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<DateTime>("Bday")
                .AddSwitch<int>("Age")
                .AddSwitch<string>("Name")
                .OverrideTypeParsers(()=>new TypeParserContainer(true, new DefaultTypeContainer(), new DateTimeParser()))
                .FinishBuilding("/Bday:28/11/1987", "/Age:30", "/Name:Yisrael Lax");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj, 
                "Parsing should have failed b/c incorrect DateTime format", 
                true,
                "string was not recognized as a valid datetime");
            Assert.IsTrue(_paramObj.GetPropertyValue<DateTime>("Bday") == default(DateTime));
            Assert.IsTrue(_paramObj.GetPropertyValue<int>("Age") == 30);
            Assert.IsTrue(_paramObj.GetPropertyValue<string>("Name") == "Yisrael Lax");
        }
        [Test]
        public void Test_Override_DateTimeParsing_Good()
        {
            ParamsObject _paramObj =
                DynamicParamsCreator
                .Create()
                .AddSwitch<DateTime>("Bday")
                .AddSwitch<int>("Age")
                .AddSwitch<string>("Name")
                .OverrideTypeParsers(() => new TypeParserContainer(true, new DefaultTypeContainer(), new DateTimeParser()))
                .FinishBuilding("/Bday:28-11-1987", "/Age:30", "/Name:Yisrael Lax");

            ParamsObjectTestHelpers.AssertCheckParams(_paramObj);
            Assert.IsTrue(_paramObj.GetPropertyValue<DateTime>("Bday") == new DateTime(1987, 11, 28));
            Assert.IsTrue(_paramObj.GetPropertyValue<int>("Age") == 30);
            Assert.IsTrue(_paramObj.GetPropertyValue<string>("Name") == "Yisrael Lax");
        }
    }
}
