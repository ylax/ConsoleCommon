using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Security;

namespace ConsoleCommon.Models.Example
{
    class SourceControlHistoryParams : ParamsObject
    {
        public SourceControlHistoryParams(string[] args) 
            : base(args)
        {
            
        }
        [Switch("Frm",true,1)]
        [SwitchHelpText("Start date of query range")]
        public DateTime? startDate { get; set; }
        [Switch("To",true,2)]
        [SwitchHelpText("End date of query range")]
        public DateTime? endDate { get; set; }
        [Switch("SRC", true,3)]
        [SwitchHelpText("Source control type")]
        public SourceControlType? SourceControl { get; set; }
        /// <summary>
        /// Path of CSV file, if GIT is selected. Otherwise, specifies path of TFS branch.
        /// </summary>
        [Switch("PTH",false,4)]
        [SwitchHelpText("Specifies either remote TFS branch location, or local GIT repository. If TFS, begin branch name with '$'")]
        public string path { get; set; }
        [Switch("U",false,5)]
        [SwitchHelpText("Specifies if report will be uploaded to remote server")]
        public bool? Upload { get; set; }

        //[Switch("SRC",)]
        public Type ReportType { get; set; }

        public override Dictionary<Func<bool>, string> GetParamExceptionDictionary()
        {
            Dictionary<Func<bool>, string> checkException = new Dictionary<Func<bool>, string>();
            DateTime startDate, endDate;

            Func<bool> validDate = () => DateTime.TryParse(SwitchValues["FRM"], out startDate) && DateTime.TryParse(SwitchValues["TO"], out endDate);
            Func<bool> validSwitch = () => SwitchValues["SRC"].ToLower() == "tfs_history" || SwitchValues["SRC"].ToLower() == "git_history";

            checkException.Add(validDate, "Must enter a valid date for start and end date!");
            checkException.Add(validSwitch, "Invalid source control!");

            return checkException;
        }
        
        #region help section
        [HelpText(1)]
        public override string Usage
        {
            get
            {
                return base.Usage;
            }
        }
        [HelpText(0)]
        public string Description
        {
            get
            {
                return "Pulls check-in/commit report for a branch/repository in TFS/GIT.";
            }
        }
        
        [HelpText(2)]
        public string Example1
        {
            get
            {
                string _example1 = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                _example1+= "/FRM:\"10/1/2016\" \"/TO:10/31/2016\" \"/SRC:TFS_HISTORY\" \"/PTH:$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava\" \"/U:N\"";
                return _example1;
            }
        }
        [HelpText(3)]
        public string Example2
        {
            get
            {
                string _example2 = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                _example2 += " \"/FRM:10/1/2016\" \"/TO:10/31/2016\" \"/SRC:TFS_HISTORY\" \"/PTH:$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava\" \"/U:N\"";
                return _example2;
            }
        }
        [HelpText(4,"Options")]
        public override string SwitchHelp
        {
            get
            {
                return base.SwitchHelp;
            }
        }

        /*public override string GetHelp()
        {
            //Test parameters for TFS Histroy
            //args = new string[] { "10/1/2016", "10/31/2016", "TFS_HISTORY", "$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava" };

            //Test parameters for Git Histroy
            //args = new string[] { "10/20/2016", "10/31/2016", "GIT_HISTORY", @"C:\fitnessedev3" };
            string _final = "";
            string description = "Pulls check-in/commit report for a branch/repository in TFS/GIT.";
            StringBuilder usageSB = new StringBuilder(string.Format("{0} [startDate] [endDate] [sourceControl] [branch | local repository path] [Optional: Upload Request]", System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location)));
            usageSB
                .AppendLine("[startDate], [endDate]: Specifies date range for report")
                .AppendLine("[sourceControl]: TFS_HISTORY | GIT_HISTORY")
                .AppendLine("[branch | local repository path]: If TFS, reference server branch with $. Else, if GIT, reference local repository")
                .AppendLine("[Optional: Upload Request]: Y | N. Designates whether or not the results will be uploaded. Default is Y.");
            string usage = usageSB.ToString();
            string example1 = "\"10/1/2016\", \"10/31/2016\", \"TFS_HISTORY\", \"$/USCIS/Releases/R22.0.0/Source/JPMC.TSS.USCIS.IntegrationServicesJava\" \"N\"";
            string example2 = "\"10/1/2016\", \"10/31/2016\", \"GIT_HISTORY\", \"C:\fitnessedev3\"";
            string dash = new Func<string>(() =>
            {
                string s = "";
                for (int i = 0; i < 150; i++) s += "-";
                return s;
            })();
            Dictionary<string, string> Lines = new Dictionary<string, string>();
            Lines.Add("description", description);
            Lines.Add("usage", usage);
            Lines.Add("example1", example1);
            Lines.Add("example2", example2);

            _final = dash;
            foreach (var line in Lines)
            {
                StringBuilder sbEntry = new StringBuilder();

                string[] sublines = GetLines(line.Value);
                for (int i = 0; i < sublines.Length; i++)
                {
                    string subline = sublines[i];
                    string sublineformat = "";
                    if (i == 0)
                    {
                        sublineformat = line.Key.ToUpper() + ":";
                    }
                    else sublineformat = "";
                    sbEntry.AppendFormat("\r\n{0,-22}{1}", sublineformat, subline);
                    if (i < sublines.Length - 1) sbEntry.AppendLine();
                }
                _final += sbEntry.ToString();
            }
            _final += Environment.NewLine + dash;
            return _final;
        }*/
        private string[] GetLines(string AllStrings)
        {
            string[] seperators = new string[]
            {
                Environment.NewLine,
                "\n\r",
                "\r\n",
                "\r",
                "\n"
            };
            string[] lines = AllStrings.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }
        #endregion
    }
    public class ExampleSourceControlParams : ParamsObject
    {
        public ExampleSourceControlParams(string[] args)
            : base(args)
        {

        }
        public ExampleSourceControlParams(string SourceControl, string Path, string StartDate = "",
            string EndDate = "")
            : this(new string[] { "/SRC:" + SourceControl, "/Path:" + Path, 
                StartDate == "" ? "" : "/StartDate:" + StartDate, 
                EndDate == "" ? "" : "/EndDate:" + EndDate})
        {
        }
        public override int HelpTextIndentLength
        {
            get
            {
                return 30;
            }
        }
        //[Switch("SRC", true, -1, "TFS_ITSM", "TFS_WORK_ITEMS", "TFS_UPLOAD", "GIT_COMMIT_HISTORY", "GIT_UPLOAD", "UPLOAD")]
        [Switch("SRC", true)]
        [SwitchHelpText("Report Type")]
        public Type ReportType { get; set; }

        [Switch("StartDate", false)]
        [SwitchHelpText("Start Date")]
        public DateTime StartDate { get; set; }

        [Switch("EndDate", false)]
        [SwitchHelpText("End Date")]
        public DateTime EndDate { get; set; }

        [Switch("TargetMonth", false)]
        [SwitchHelpText("End Date")]
        public DateTime TargetMonth { get; set; }

        [Switch("Path", false)]
        [SwitchHelpText("TFS Path or Git Repository Path")]
        public string Path { get; set; }

        [Switch("UserID", false)]
        public string UserName { get; set; }

        [Switch("Password", false)]
        public SecureString Password { get; set; }

        [Switch("WorkSpacePath", true)]
        public string WorkSpacePath { get; set; }

        [HelpText(1)]
        public string Description
        {
            get { return "This generates a report from TFS or GIT"; }
        }

        [HelpText(2, "TFS ITSM Report Example")]
        public string ITSMReportExample
        {
            get { return "SourceControlHistoryGenerator /SRC:TFS_ITSM /StartDate:09/01/2016 /EndDate:10/01/2016 /Path:$/USCIS/Releases/R21.0.0"; }
        }

        [HelpText(3, "TFS Work Items Report Example")]
        public string WorkItemReportExample
        {
            get { return "SourceControlHistoryGenerator /SRC:TFS_WORK_ITEMS /StartDate:01/01/2015 /EndDate:12/31/2015 /Path:$/USCIS/Releases/R21.0.0"; }
        }

        [HelpText(4, "Git Bank Upload Example")]
        public string GitUploadExample
        {
            get { return "SourceControlHistoryGenerator /SRC:GIT_UPLOAD /Path:@C:\\GitRepositories\\uscis_tools"; }
        }

        [HelpText(5, "Git Commit History Example")]
        public string GitCommitsExample
        {
            get { return "SourceControlHistoryGenerator /SRC:GIT_COMMIT_HISTORY /StartDate:01/01/2015 /EndDate:12/31/2015 /Path:@C:\\GitRepositories\\uscis_tools"; }
        }

        [HelpText(6, "Options")]
        public override string SwitchHelp
        {
            get { return base.SwitchHelp; }
        }

        public override Dictionary<Func<bool>, string> GetParamExceptionDictionary()
        {
            Dictionary<Func<bool>, string> checkException = new Dictionary<Func<bool>, string>();
            //Func<bool> datesCheck = () => (this.SourceControl != SourceControlType.UPLOAD) & ((this.StartDate == default(DateTime) ||
            //        this.EndDate == default(DateTime)) || this.StartDate > this.EndDate);

            Func<bool> tfsLoginCheck = () =>( (this.ReportType.Equals(typeof(TFS_ItsmReport)) ||
                this.ReportType.Equals(typeof(Tfs_Work_ItemsReport)) ||
                this.ReportType.Equals(typeof(UploadReport))) &
                ((string.IsNullOrEmpty(this.UserName) || this.Password == null ||
                   this.Password.Length == 0)));

            Func<bool> uploadDateCheck = () => (this.ReportType.Equals(typeof(UploadReport))) & ((this.StartDate != default(DateTime) || this.EndDate != default(DateTime)));

            checkException.Add(tfsLoginCheck, "Must enter username and passowrd for TFS query");
            checkException.Add(uploadDateCheck, "Cannot use StartDate or EndDate parameters for Upload Report.  Please use TargetDate Parameter, or leave TargetDate Parameter blank to default to last month");
            return checkException;
        }
    }
    public class UploadReport { }
    public abstract class TfsReport { }
    [TypeParam("TFS_WORK_ITEMS")]
    public class Tfs_Work_ItemsReport : TfsReport { }
    [TypeParam("TFS_ITSM")]
    public class TFS_ItsmReport : TfsReport { }
    [TypeParam("TFS_UPLOAD")]
    public class TFS_UploadReport : TfsReport { }
}