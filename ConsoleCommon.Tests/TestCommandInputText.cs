using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ConsoleCommon;
using ConsoleCommon.Tests;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class TestCommandInputText
    {
        [Test]
        public void TestStringInput()
        {
            //string switchName = "", bool required = false, int defaultOrdinal = -1, string helpText = "", bool dontAllowValues = false
            ParamsObject _input = (ParamsObject)DynamicTypeCreator
                .Create<ParamsObject>("MyParams")
                .AddPassThroughCtors()
                .AddAutoProperty<string>("FirstName")
                .AddPropertyAttribute<SwitchAttribute>(new Type[] { typeof(string), typeof(bool), typeof(int), typeof(string), typeof(bool) }, new object[] { "", false, -1, "", false })
                .AddAutoProperty<string>("LastName")
                .AddPropertyAttribute<SwitchAttribute>(new Type[] { typeof(string), typeof(bool), typeof(int), typeof(string), typeof(bool) }, new object[] { "", false, -1, "", false })
                .AddAutoProperty<int>("Age")
                .AddPropertyAttribute<SwitchAttribute>(new Type[] { typeof(string), typeof(bool), typeof(int), typeof(string), typeof(bool) }, new object[] { "", false, -1, "", false })
                .AddAutoProperty<bool>("IsItTrue")
                .AddPropertyAttribute<SwitchAttribute>(new Type[] { typeof(string), typeof(bool), typeof(int), typeof(string), typeof(bool) }, new object[] { "", false, -1, "", false })
                .FinishBuildingType()
                .GetConstructor(new Type[] { typeof(string) })
                .Invoke(new object[] { "/FirstName:Yisrael /LastName:Lax /IsItTrue /Age:30" });

            Assert.IsTrue(_input.GetPropertyValue<string>("FirstName") == "Yisrael");
            Assert.IsTrue(_input.GetPropertyValue<string>("LastName") == "Lax");
            Assert.IsTrue(_input.GetPropertyValue<int>("Age") == 30);
            Assert.IsTrue(_input.GetPropertyValue<bool>("IsItTrue") == true);
        }
    }
}
