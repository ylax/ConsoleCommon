using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ConsoleCommon.Tests;

namespace ConsoleCommon.Tests
{
    [TestFixture]
    public class InlineHelpTextAndDefaultSwitchNameTest
    {
        class InlineHelpParams : ParamsObject
        {
            public InlineHelpParams(string[] args) : base(args) { }
            [Switch(required: true, defaultOrdinal: 0, helpText: "This is help text")]
            public string FirstName { get; set; }
            [HelpText(0)]
            public override string SwitchHelp => base.SwitchHelp;
        }
        [Test]
        public void Test_InlineHelpText_And_DefaultSwitchName()
        {
            //string switchName = "", bool required = false, int defaultOrdinal = -1, string helpText, bool dontAllowValues = false
            InlineHelpParams _input = new InlineHelpParams(new string[] { "/FirstName:Yisrael" });
            _input.CheckParams();
            string _help = _input.GetHelp();
            Assert.IsTrue(_help.Contains("This is help"));
        }
    }
}
