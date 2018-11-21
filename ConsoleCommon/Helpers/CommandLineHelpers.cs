using System;
using System.IO;
using System.Reflection;

namespace ConsoleCommon.Helpers
{
    public static class CommandLineHelpers
    {
        public static string RemoveAppNameFromArgs(string CommandLine, string CallingDirectory)
        {
            //return var
            string _newCommandLine = "";

            //Get current dir, app path, app name, app extension:
            Assembly _curAssm = Assembly.GetEntryAssembly();
            string _appPath = _curAssm.Location.ToLower();
            string _curDir = CallingDirectory;
            string _appName = _curAssm.GetName().Name.ToLower();
            string _ext = Path.GetExtension(_appPath).ToLower();

            //Find app name at beginning of commandLine text
            string _firstParam = "";
            bool _isApp = false;
            int _totalChars = 0;
            foreach (char _c in CommandLine)
            {
                _totalChars++;
                //ignore double quotes
                if (_c == '"') continue;

                _firstParam += _c;

                _isApp =
                    //4 ways a user might call app from command line
                    _appPath == $"{Path.Combine(_curDir, _firstParam).ToLower()}" ||
                    _appPath == $"{Path.Combine(_curDir, _firstParam).ToLower()}{_ext}" ||
                    _appPath == _firstParam.ToLower() ||
                    _appPath == $"{_firstParam.ToLower()}{_ext}";
                if (_isApp) break;
            }
            //If app name is found
            if (_isApp)
            {
                //check for extension
                if (_ext.Length > 0
                    && CommandLine.Length > _totalChars + _ext.Length
                    && CommandLine.Substring(_totalChars, _ext.Length).ToLower() == _ext)
                {
                    _totalChars += _ext.Length;
                }
                ////check for end double quote
                if (CommandLine.Length > _totalChars
                    && CommandLine.Substring(_totalChars, 1) == "\"")
                {
                    _totalChars++;
                }
                if (CommandLine.Length > _totalChars) _newCommandLine = CommandLine.Substring(_totalChars);
            }
            else _newCommandLine = CommandLine;
            return _newCommandLine;
        }
    }
}
