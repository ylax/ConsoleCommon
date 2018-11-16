using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConsoleCommon.Tests.Helpers
{
    public static class ParamsObjectTestHelpers
    {
        public static Exception AssertCheckParams(ParamsObject paramObj, string errMsg = "", bool shouldFail = false, string expctdErrMsg = "")
        {
            bool _hasExpctdErr = !string.IsNullOrWhiteSpace(expctdErrMsg);
            string _expctdErrMsg = expctdErrMsg;
            string _errMsg = shouldFail ? "Parsing should have failed." : "Parsing failed.";
            if (errMsg.Length > 0) _errMsg += $" {errMsg}.";
            Exception _ex = null;

            try
            {
                paramObj.CheckParams();
            }
            catch (Exception ex)
            {
                _ex = ex;
                if (!shouldFail) _errMsg += $" Ex: {ex.Message}";
                else if (_hasExpctdErr)
                {
                    _errMsg += $" Expected err '{removePeriod(expctdErrMsg)}' does not match actual err '{removePeriod(ex.Message)}'.";
                }
            }

            Assert.IsTrue((shouldFail &&
                (_hasExpctdErr && _ex != null && removePeriod(_ex.Message).ToLower().Trim() == removePeriod(expctdErrMsg).ToLower().Trim()) ||
                (!_hasExpctdErr && _ex != null)
                ) || (!shouldFail && _ex == null), _errMsg);
            return _ex;
        }
        private static string removePeriod(string text)
        {
            string _rtrim = text.TrimEnd();
            string _finalText = text;
            if (_rtrim.Length > 0 && _rtrim.EndsWith("."))
            {
                int _indx = text.Length - (text.Length - _rtrim.Length) - 1;
                _finalText = text.Substring(0, _indx);
                if (_finalText.Length > 0 && text.Length > 1) _finalText += text.Substring(_indx + 1);
            }
            return _finalText;
        }
    }
}
