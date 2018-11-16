using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommon;
using ConsoleCommon.Parsing;
using ConsoleCommon.Parsing.TypeParsers;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class ParamsObjectCreatorTests
    {
        [Test]
        public void CreateParamsObject()
        {
            /*string _switchName = name;
            bool _required = false;
            int _defaultOrdinal = -1;
            string[] _switchValues = new string[0] { }; */
            ParamsObject _paramsObject = (ParamsObject)DynamicParamsCreator
                .Create()
                .AddSwitch("fName", typeof(string),"f")
                .AddSwitch("lName", typeof(string),"l")
                .FinishBuilding("/F:Yisrael", "/L:Lax");

            Assert.IsTrue(TestValue(_paramsObject, "fName", "Yisrael"));
            Assert.IsTrue(TestValue(_paramsObject, "lName", "Lax"));
            _paramsObject.CheckParams();
        }
        private bool TestValue(object obj, string propName, object propVal)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(propName);
            if (_prop == null) throw new Exception($"Property '{propName}' does not exist on type '{obj.GetType().Name}'");
            object _foundPropVal = _prop.GetValue(obj);

            if (_prop.PropertyType==typeof(string))
            {
                string _propValStr = propVal?.ToString();
                string _foundPropValStr = _foundPropVal?.ToString();
                return _foundPropValStr == _propValStr;
            }
            return _foundPropVal == propVal;
        }
    }
}
