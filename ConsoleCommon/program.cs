using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommon.Models;
using ConsoleCommon.Models.Example;

namespace ConsoleCommon
{
    public static class program
    {
        public static void Main(string[] args)
        {
            //args = new string[] { "10/1/2016", "10/31/2016", "TFS_HISTORY", "$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava" };
            //args = new string[] { "/FRM:10/1/2016", "/TO:10/31/2016", "/SRC:TFS_History", "/U:Y", "/PTH:$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava" };
            //args=new string[] {"/FRM:10/1/2016,4/4/2017", "/TO:10/31/2016", "/SRC:TFS_History", "/U:Y", "/PTH:$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava" };
            args = new string[] { "/SRC:TFS_ITSM", "/StartDate:09/01/2016", "/EndDate:10/01/2016", "/Path:$/USCIS/Releases/R22.0.0", "/WorkspacePath:C:\\testagain", "/UserID:/E189382", "/Password:TYPE THE PASSWORD HERE" };
            //args=new string[] {"10/1/2016", "10/31/2016", "TFS_History", "$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava" };
            //args = new string[] { "/help" };
            try
            {
                //SourceControlHistoryParams myParams = ConsoleTools.CheckParams<SourceControlHistoryParams>(args);
                ExampleSourceControlParams myParams = new ExampleSourceControlParams(args);
                myParams.CheckParams();
                string _help = myParams.GetHelpIfNeeded();
                //string _help = myParams.GetHelp2();
                if (_help != string.Empty)
                {
                    Console.Write(_help);
                }
                else
                {
                    foreach (var switchVal in myParams.SwitchValues)
                    {
                        Console.WriteLine("{0}: {1}", switchVal.Key, switchVal.Value);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
