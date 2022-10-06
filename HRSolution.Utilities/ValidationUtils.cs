using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HRSolution.Utilities
{
    public class ValidationUtils
    {

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneNo)
        {

            bool test = Regex.Match(phoneNo, @"^([0-9]{11})$").Success;
            return test;
        }


        public static bool ValidateGrade(string _grade)
        {
            List<string> Grade = new List<string>() { "A", "B", "C", "P", "F", "PP", "NA", "AB" };
            return Grade.Contains(_grade);
        }

        public static Boolean ValidateNumericValue(String Value)
        {
            return Int32.TryParse(Value, out int number);
        }

        public static Boolean ValidateDecimalValue(String Value)
        {
            return Decimal.TryParse(Value, out decimal number);
        }

        public bool IsAlphaNumeric(string input) => Regex.IsMatch(input, "^[a-zA-Z0-9 ]+$");
        public bool IsAlphabetsOnly(string input) => Regex.IsMatch(input, "^[a-zA-Z ]+$");
        public bool IsNumberOnly(string input) => Regex.IsMatch(input, "^[0-9]*$");
        public bool IsValidEmailCombination(string input) => Regex.IsMatch(input, "");
    }
}
