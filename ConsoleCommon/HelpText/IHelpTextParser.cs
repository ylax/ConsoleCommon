using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing;
using System.Reflection;

namespace ConsoleCommon.HelpText
{
    public interface IHelpTextParser
    {
        string GetUsage(ParamsObject InputParams);
        string GetSwitchHelp(ParamsObject InputParams);
        string GetHelp(ParamsObject InputParams);
        string GetHelpIfNeeded(string[] args, ParamsObject InputParams);
    }
}
