using System;

namespace HRSolution.Utilities
{
    public class DateUtility
    {
        //Get Month Difference
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static DateTime? GetDateNullValue(Object obj)
        {
            DateTime? retVal = null;
            try
            {
                if (!(obj is DBNull))
                {
                    retVal = (DateTime)obj;
                }
                else
                {
                    retVal = null;
                }
            }
            catch { }
            return retVal;
        }

        public static DateTime GetDateValue(Object obj)
        {
            DateTime retVal = DateTime.Now;
            try
            {
                if (!(obj is DBNull))
                {
                    retVal = (DateTime)obj;
                }
            }
            catch { }
            return retVal;
        }

        public static DateTime ConvertDateValue(Object obj)
        {
            var succeeded = DateTime.TryParse(obj.ToString(), out DateTime result);
            if (succeeded == true)
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static DateTime ConvertDateValueWithCulture(String obj)
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");

            var succeeded = DateTime.TryParse(obj, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out DateTime result);
            if (succeeded == true)
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static string GetDateStringValue(Object obj)
        {
            string retVal = DateTime.Now.ToShortDateString();
            try
            {
                retVal = (!(obj is DBNull) ? ((DateTime)obj).ToShortDateString() : "");
            }
            catch { }
            return retVal;
        }

        public static bool IsDate(String date)
        {
            return DateTime.TryParse(date, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out DateTime temp) &&
                   temp.Hour == 0 &&
                   temp.Minute == 0 &&
                   temp.Second == 0 &&
                   temp.Millisecond == 0 &&
                   temp > DateTime.MinValue;
        }

        public static string FormatDate_Long(DateTime dt)
        {
            if (dt != null)
            {
                return string.Format("{0:dd MMMM yyyy}", dt);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDate_Long1(DateTime dt)
        {
            if (dt != null)
            {
                return string.Format("{0:MMMM dd, yyyy}", dt);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDate(DateTime? dt)
        {
            if (dt != null)
            {
                return string.Format("{0:dd-MMM-yyyy}", dt);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDate_Short(DateTime? dt)
        {
            if (dt != null)
            {
                return string.Format("{0:dd/MM/yyyy}", dt);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatDate(DateTime dt)
        {
            if (dt != null)
            {
                return string.Format("{0:dd-MMM-yyyy}", dt);
            }
            else
            {
                return string.Empty;
            }
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            {
                age -= 1;
            }
            return age;
        }

        /// <summary>  
        /// For calculating age  
        /// </summary>  
        /// <param name="Dob">Enter Date of Birth to Calculate the age</param>  
        /// <returns> years, months,days, hours...</returns>  
        public static string CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            //int Hours = Now.Subtract(PastYearDate).Hours;
            //int Minutes = Now.Subtract(PastYearDate).Minutes;
            //int Seconds = Now.Subtract(PastYearDate).Seconds;
            return string.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s)",
            Years, Months, Days);
            //return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            //Years, Months, Days, Hours, Seconds);
        }
    }
}
