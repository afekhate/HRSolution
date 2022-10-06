
using System;
using System.Globalization;

namespace HRSolution.Utilities
{

    public class ExceptionHandlerHelper : Exception
    {
        public ExceptionHandlerHelper() : base() { }

        public ExceptionHandlerHelper(string message) : base(message) { }

        public ExceptionHandlerHelper(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }


        //#region Database Update Exception Helper
        //public static string HandleDatabaseUpdateException(Exception ex)
        //{
        //    string err = null;
        //    if (ex is DbUpdateException dbUpdateException)
        //    {
        //        if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.InnerException != null)
        //        {
        //            if (dbUpdateException.InnerException.InnerException is SqlException sqlException)
        //            {
        //                switch (sqlException.Number)
        //                {
        //                    case 2627:  // Unique constraint error
        //                        err = "A Unique Constraint Error Has Occured While Updating the record! Duplicate Record cannot be inserted in the System.";
        //                        break;
        //                    case 544:   // Cannot insert explicit value for identity column in table 'Departments' when IDENTITY_INSERT is set to OFF
        //                        err = "Cannot insert explicit value for identity column in the system when the id is set to OFF";
        //                        break;
        //                    case 547:   // Constraint check violation, Conflict in the database
        //                        err = "A Constraint Check violation Error Has Occured While Updating the record(s)!";
        //                        break;
        //                    case 2601:  // Duplicated key row error // Constraint violation exception // A custom exception of yours for concurrency issues           
        //                        err = "A Duplicate Key Error Has Occured While Updating the record(s)!";
        //                        break;
        //                    case 201:  // Proceudre missing parameters            
        //                        err = "A Missing Parameter has led to Error  While Creating the record(s)!";
        //                        break;
        //                    default:
        //                        err = null;
        //                        break;
        //                        // A custom exception of yours for other DB issues
        //                        // throw new DatabaseAccessException(dbUpdateException.Message, dbUpdateException.InnerException);
        //                }
        //            }
        //        }
        //    }
        //    return err;
        //}
        //#endregion

        //#region Regex Exception Handling
        //public static string NullInnerExceptionHandling(Exception ex)
        //{
        //    if (ex.InnerException == null)
        //    {
        //        return ex.Message;
        //    }
        //    return string.Empty;
        //}
        //#endregion
    }
}
