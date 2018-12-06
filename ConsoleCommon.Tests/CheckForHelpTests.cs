using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class CheckForHelpTests
    {
        [Test]
        public void TestCStorArgs()
        {
            ParamsObject _input = null;
            Exception _ex = null;
            bool _created = true;
            try
            {
                _input = DynamicParamsCreator
                    .Create()
                    .AddSwitch<string>("FirstName")
                    .FinishBuilding("/?");
            }
            catch(Exception ex)
            {
                _created = false;
                _ex = ex;
            }
            Assert.IsTrue(_created, $"Failed to parse switches. Ex: {handleEx(_ex)}");
            string _helpText = _input.GetHelpIfNeeded();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_helpText), "Help not retrieved");
        }
        [Test]
        public void TestCStorCommandText()
        {
            ParamsObject _input = null;
            bool _created = true;
            Exception _ex = null;
            try
            {
                _input = DynamicParamsCreator
                    .Create()
                    .AddSwitch<string>("FirstName")
                    .FinishBuilding(new StringBuilder("/?"));
            }
            catch(Exception ex)
            {
                _ex = ex;
                _created = false;
            }
            Assert.IsTrue(_created, $"Failed to parse switches. Ex: {handleEx(_ex)}");
            string _helpText = _input.GetHelpIfNeeded();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_helpText), "Help not retrieved");
        }
        private string handleEx(Exception ex)
        {
            int _i = 1;
            Exception _innerEx = ex;
            string _msg = "";
            while(_innerEx!=null)
            {
                _msg += $"{Environment.NewLine}Ex {_i}: {_innerEx.Message}";
                _innerEx = _innerEx.InnerException;
                _i++;
            }
            return _msg;
        }
    }
}
