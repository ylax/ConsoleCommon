using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCommon.Parsing;

namespace ConsoleCommon
{
    /// <summary>
    /// This is an example program
    /// To run this, modify the <OutputType> tag in the .csproj file from "Library" to "Exe".
    /// </summary>
    public static class program
    {
        class Program
        {
            static void Main(string[] args)
            {
                try
                {
                    args = new string[] { "Yisrael", "Lax", "/T:pizza shop", "/DOB:11-28-1987", "/Case", "/Regions:Northeast,Central", "/Int:Pizza,Parachuting" };
                    //This step will do type validation
                    //and automatically cast the string args to a strongly typed object:
                    CustomerParamsObject _customer = new CustomerParamsObject(args);
                    //This step does additional validation
                    _customer.CheckParams();
                    //Get help if user requested it
                    string _helptext = _customer.GetHelpIfNeeded();
                    //Print help to console if requested
                    if (!string.IsNullOrEmpty(_helptext))
                    {
                        Console.WriteLine(_helptext);
                        Environment.Exit(0);
                    }
                    string _fname = _customer.firstName;
                    string _lname = _customer.lastName.ToString();
                    string _dob = _customer.DOB.ToString("MM-dd-yyyy");
                    string _ctype = _customer.CustomerType == null ? "None" : _customer.CustomerType.Name;
                    string _caseSensitive = _customer.CaseSensitiveSearch ? "Yes" : "No";
                    string _regions = string.Concat(_customer.CustRegions.Select(r => "," + r.ToString())).Substring(1);
                    string _interests = _interests = _customer.Interests.ToString();

                    Console.WriteLine();
                    Console.WriteLine("First Name: {0}", _fname);
                    Console.WriteLine("Last Name: {0}", _lname);
                    Console.WriteLine("DOB: {0}", _dob);
                    Console.WriteLine("Customer Type: {0}", _ctype);
                    Console.WriteLine("Case sensitive: {0}", _caseSensitive);
                    Console.WriteLine("Regions: {0}", _regions);
                    Console.WriteLine("Interests: {0}", _interests);

                    //Get help
                    args = new string[1] { "/?" };
                    _customer = new CustomerParamsObject(args);
                    _customer.CheckParams();
                    _helptext = _customer.GetHelpIfNeeded();
                    //Print help to console if requested
                    if (!string.IsNullOrEmpty(_helptext))
                    {
                        Console.WriteLine(_helptext);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadKey();
            }
        }

        public enum CustomerRegion
        {
            None,
            Northeast,
            Southwest,
            Central,
            Midwest
        }

        [Flags]
        public enum CustomerInterests
        {
            Pizza = 1,
            Crabcakes = 2,
            Parachuting = 4,
            Biking = 8
        }

        [TypeParam("pizza shop")]
        public class PizzaShopCustomer { }
        [TypeParam("bodega")]
        public class BodegaCustomer { }

        public enum LastNameEnum
        {
            Smith,
            Johnson,
            Nixon,
            Lax
        }
        public class CustomerParamsObject : ParamsObject
        {
            public CustomerParamsObject(string[] args)
                : base(args)
            {

            }

            #region Switch Properties
            [Switch("F", true, 1)]
            [SwitchHelpText("First name of customer")]
            public string firstName { get; set; }

            [Switch("L", true, 2)]
            [SwitchHelpText("Last name of customer")]
            public LastNameEnum lastName { get; set; }

            [SwitchHelpText("The date of birth of customer")]
            [Switch("DOB", false, 3)]
            public DateTime DOB { get; set; }

            [Switch("T", false, 4)]
            [SwitchHelpText("Customer type")]
            public Type CustomerType { get; set; }

            [Switch("Case")]
            [SwitchHelpText("Specifies whether to do a case sensitive search")]
            public bool CaseSensitiveSearch { get; set; }

            [Switch("Regions")]
            [SwitchHelpText("Specifies the regions the customer does business in")]
            public CustomerRegion[] CustRegions { get; set; }

            [Switch("Int")]
            [SwitchHelpText("Specifies customer's interests")]
            public CustomerInterests Interests { get; set; }
            #endregion

            #region Other Functions
            public override Dictionary<Func<bool>, string> GetParamExceptionDictionary()
            {
                Dictionary<Func<bool>, string> _exceptionChecks = new Dictionary<Func<bool>, string>();

                Func<bool> _isDateInFuture = new Func<bool>(() => DateTime.Now <= this.DOB);

                _exceptionChecks.Add(_isDateInFuture, "Please choose a date of birth that is not in the future!");
                return _exceptionChecks;
            }

            [HelpText(0)]
            public string Description
            {
                get { return "Finds a customer in the database."; }
            }
            [HelpText(1, "Example")]
            public string ExampleText
            {
                get { return "This is an example: CustomerFinder.exe Yisrael Lax 11-28-1987"; }
            }
            [HelpText(2)]
            public override string Usage
            {
                get { return base.Usage; }
            }
            [HelpText(3, "Parameters")]
            public override string SwitchHelp
            {
                get { return base.SwitchHelp; }
            }
            #endregion
        }
    }
}
