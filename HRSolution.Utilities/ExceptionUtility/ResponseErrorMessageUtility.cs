
using System;
using System.Text.RegularExpressions;

namespace HRSolution.Utilities.ExceptionUtility
{
    public class ResponseErrorMessageUtility
    {

        // Responder Resolution Messages
        public const string SupportingDocument = "true";

        //File Upload
        public const string EXCEEDED_FILE_SIZE = "The file is too large.";

        //Assign Incident type to agencies
        public const string ASSIGN_INCIDENT_TYPE_TO_AGENCY = "Assigned an incident type to an agency";
        public const string UPDATE_INCIDENT_TYPE_ASSIGNED_TO_AGENCY = "Updated an existing incident type assigned to an agency";
        public const string DELETE_INCIDENT_TYPE_ASSIGNED_TO_AGENCY = "Deleted an existing incident type assigned to an agency";

        //Role name
        public const string Reviewer = "Reviewer";


        public const string SuccessfulFileUpload = "You have successfully attached an evidence";
        public const string NotExistProtectedId = "Operation Failed. The {0} supplied is not valid or does not exist.";
        public const string NotExistRecord = "Operation Failed. The {0} provided does not exist on the system.";
        public const string NotValidProtectedId = "Operation Failed. The {0} supplied is not valid.";
        public const string NotFoundMerchantUser = "Your merchant record cannot be found.";
        public const string CurrentBranchName = "This is your current branch.";
        public const string BranchSelection = "Set your Branch to continue";
        public const string PermissionCheck = "This is record is system generated records(s). You do not have the permission to edit this record. Only User-defined record(s) can be edited";
        public const string PasswordChanged = "Password change was successful, please logIn with your new detail";


        public const string LogInSuccessful = "You have logged in successfully";
        public const string PasswordMatchError = "Password doesn't match.";
        public const string ExistingPassword= "You can't use any of your old password(s), please try with a new password";
        public const string ControllerRequested = "Please select controller";
        public const string UserRequested = "Please select a user";
        public const string DataRequired = "Please select a validate start and end date";
        public const string DatePeriodCheck = "Your start date can't be greater than your end date";
        public const string ResponderRole = "68a1511c-0b83-4fa8-ba16-08fdb8d2e876";

        /// <summary>
        /// {0} - Record Column Name
        /// </summary>
        public const string RecordExistBefore = "The {0} record you are trying to create already exists on the system.";
        public const string RecordExistBeforeWithoutParameter = "The record you are trying to create already exists on the system.";
        public const string RecordExistBeforeII = "The {0} record you are trying to {1} already exists on the system.";
        public const string RecordExistBeforeDeactivated = "The {0} record you are trying to create already exists but deactivated on the system. Contact your administrator.";
        public const string RecordAttachedToOthers = "The {0} record you are trying to delete has an attached {1} on the system.";
        public const string NotAuthorizedUser = "Your request cannot be processed. Please Login to Continue";
        public const string RecordNotFound = "Your request record(s) don't exist, please try again";
        public const string OperationFailed = "The operation was not successful.";
        public const string ReOpenIncident = "Incident has been reopened";

        public const string DeletedAccount = "Your account is turn off on this portal, kindly contact the system adminstrator";
        public const string disabledAccount = "Your account is disabled on this portal, kindly contact the system adminstrator";
        public const string PendingEmailConfirmation = "You are yet to confirm your email, kindly check your email for the confirmation email";
        public const string AgencyDisabled = "Your agency is currently disabled on this platform, kindly contact the system administrator";
        public const string InvalidAccount = "Invalid UserName or Password";
        public const string LoginNotAllowed = "Your are not allowed to use the web. Kindly log in using the mobile";
        public const string ChangeSystemGeneratedPassword = "For security reasons, you would be required to change your system generated password";

        public const string IllegalOperation= "You are not permitted to perform this operation";
        public const string EvidenceComment= "Your evidence comment is required to upload this file";
        public const string ExpiredActivationLink= "Your activation link has expired, please contact the administrator";
        public const string ExpiredResetPasswordLink= "Your password reset link has expired, kindly regenerate any link and try again";
        public const string ValidateAccount= "Your account was activated successful, kindly check your register email for your login credentails";
        public const string Errorconfirming = "Error while confirming your email!";
        public const string LogoutSuccessful = "You have logged out successfully";

        public const string InvalidExcelHeader = "The Excel file uploaded contains invalid column header(s). Please ensure that the excel contains valid column headers in the format in the excel template";
        public const string EmptyExcel = "Unable to continue, there is no data in the excel uploaded";
        public const string SelectExcelFile = "Please choose Excel file";



         public const string ForgetPassword = "Please check your email to reset your password";
        public const string SystemPasswordNotMatch = "Your old password didn't match, please try again";
        public const string PreviousPasswordError = "Your can't use your previous password";
        public const string ChangePasswordSuccessful = "Your password changed was successful, a copy of your credentails was sent to your registered email";
        public const string ProfileUpdate = "Your profile update was successfully";
      
        
        public const string AssignRequest = "Assign";
        public const string ViewRequest = "View";
        public const string ReAssignRequest = "ReAssign";
        public const string CloseRequest = "Close";
        public const string ReOpenRequest = "ReOpen";
        public const string NoResultFound = "There is no result for the incident Id";




        /// <summary>
        /// {0} - Record Column Name
        /// {1} - Incoming Record Taken
        /// </summary>
        public const string PASS = "PASS";
        public const string FAIL = "FAIL";
        public const string TakenRecord = "{0} : {1} is already taken";
        public const string SelectRecord = "Please select {0} from the dropdown list";
        public const string CannotCreateMoreThanOneRecord = "You cannot create more than one record on this system";
        public const string RecordNotSaved = "Operation Failed. We encountered an error saving your data on the system";
        public const string ExistingCode = "Exist Code was found,please try again";
        public const string ExistingSubjectDescription = "Exist Subject Description was found,please try again";
        public const string RecordSaved = "1";
        public const string RecordNotFetched = "Operation Failed. We encountered an error fetching your data on the system";
        public const string LoginUsernamePasswordNotEmpy = "Username or Password cannot be empty";
        public const string DateRange = "Select start and end date";


        public const string InvalidLoginAttempt = "Invalid login attempt";
        public const string LockedOutUser = "User account locked out.";
        public const string StatusBool = "Status can only be true or false";
        public const string MappingRecordCannotBeEmpty = "Operation Failed. {0} or Associated {1} cannot be empty";
        public const string InvalidToken = "Invalid token";
        public const string InvalidUser = "Invalid user";
        public const string CannotContainPassword = "Your password cannot contain element of {0}";
        public const string EmptyFormFile = "FormFile is Empty.";
        public const string SupportedFileFormat = "The system currently supports only {0} format.";
        public const string RequiredColumnsFromTemplate = "Operation Failed. Ensure that the column arrangement conforms to the provided template";
        public const string DuplicateRecordFromFile = "There are duplicate records in the file uploaded as follows \n {0}";
        public const string PlanBenefitOperationFailed = "Operation Failed. Unable to {0} associated services to the plan coverage";
        public const string BenefitCoveredAlreadyPlan = "The Service Type benefit you are trying to create is already covered by the plan.";
        public const string BenefitCoveredAlreadyPlanDeactivated = "The Service Type benefit you are trying to create is already covered by the plan but deactivated.";
        public const string GroupMappingRemoveOperationFailed = "Operation Failed. Unable to Remove associated {0} from group.";
        public const string RequiredFields = "Kindly provide all required fields before processing";
        public const string ScheduleTime= "The Schedule  date time can't be greater than the current date";
        public const string DataIntegrity_OK = "OK";
        public const string DataIntegrity_Fail = "Fail";
        public const string SMSLive247FailedLogin = "Invalid";
        public const string SMSSessionId = "SMS Provider Session Id not generated";
        public const string IncompleteRequirement = "Your request was not complete, you are missing one or more parameter(s)";
        public const string PasswordNotMatch = "Password and confirm password must match";
        public const string RecordFetched = "You record(s) was fetched successful.";
        public const string UpdateRecord = "Your update request was successful";
        public const string ScheduleRecord = "Your  scheduling message was successful and request will be processed shortly";
        public const string SuccessfulAndUpdated = "1";
        public const string InvalidDate = "The selected date must be valid, and end date can't be greater than start date";
        public const string OperationFailed_Ex = "Operation Failed. Unable to complete your request, please try again";
        public const string MISSING_RESPONDERS = "Kindly select responders before proceeding";
        public const string UNDEFINED = "undefined";
        public const string MISSING_LOCATION = "Kindly fill in the location before processing";

        public const string DeleteOperation = "Your delete operation was successful";
        public const string DeleteOperationFail = "Your delete operation was not successful";
        public const string succesful = "Your  request was successful";
        public const string succesfulSub = "Thank you for subscribing to our alerting system";
        public const string ActiveAccount = "1";
        public const long RecordExistAndEqualToOne = 1;
        public const string InActiveAccount = "0";
        public const string MESSAGETYPE_EMAIL = "EMAIL";
        public const string MESSAGETYPE_SMS = "SMS";
        public const int PermissionParentId = 0;


        public const string ApplySuperAdminRole = "Super Administrator";



        // Notifications for FILE UPLOAD
        public const string FILE_UPLOAD_FAILED = "FAIL|";
        public const string NO_FILE_UPLOADED = FILE_UPLOAD_FAILED + "No File Uploaded";
        public const string FILE_SIZE_EXCEEDED = FILE_UPLOAD_FAILED + "File size is larger than the accepted maximum size. Maximum file size is ";
        public const string NO_FILE_CONTENT = FILE_UPLOAD_FAILED + "File contain no content!";
        public const string INVALID_FILE_EXTENSION = FILE_UPLOAD_FAILED + "File Extension Is InValid - Only Upload ";
        public const string FILE_SIZE_UNIT = " MB";
        // END Notifications for FILE UPLOAD

        // Audit
        public const string AUDIT_SUCCESS = "Successful";
        public const string AUDIT_FAILED = "Failed";
        // End Audit

        //Executives
        public const long ZERO= 0;
        public const int THREE_ZERO= 1001;
        public const int PLEASE_SELECT= 1001;
        //public const string SiturationOfficerRole = "2c0935dc-e157-44cc-900b-57c51a9ac505";

        // Incidents Messages
        public const string INCIDENT_STATE = "ekiti";
        public const string CREATED_INCIDENT = "A new incident was created with name: {0}";
        public const string CREATED_INCIDENT_WITH_HISTORY = "A new incident was created with comment: {0}";
        public const string ASSIGNED_AGENT = "Call was handled by: {0}";
        public const string ASSIGN_INCIDENT_WITH_HISTORY = "An incident was assigned to responder(s) with comment: {0}";
       
        public const string CREATED_RE_ASSIGN_INCIDENT_WITH_HISTORY = "An existing incident was reassigned with comment: {0}";
        public const string ASSIGN__RESPONDER_TO_INCIDENT = "A new incident was assigned to responder with comment:{0}";
        public const string RESPONDER_ACCEPTED_INCIDENT = "A new incident assigned to responder was accepted with comment :{0}";
        public const string RESPONDER_RESOLVED_INCIDENT = "A new incident assigned to responder was made as resolved with comment :{0}";
        public const string RESPONDER_REJECTED_INCIDENT = "A new incident assigned to responder was rejected with comment :{0}";
        public const string APPROVAL_COMMENT = "A new data bank request was converted to an incident with comment: {0}";
        public const string REJECT_COMMENT = "A new data bank request was not converted to an incident with comment {0}";
        public const string HISTORICAL_COMMENT = "Kindly fill the Comment field before processing";
        public const string RESOLVE_CASE_EVIDENCE = "For a resolved case, you are expected to upload evidence";

        // Report submission from the web
        public const string SUBMISSION_SUCCESSFUL = "Your request has been submitted successfully";
        public const string SUBMISSION_FAILED = "Your request was not submitted successfully";
        public const string PHONE_OR_EMAIL = "Please provide your phone number or email address";
        public const string INVALIDEMAIL = "Your email is invalid please try again";
        public const string INVALIDPHONE = "Your phone number is invalid please try again";

        // Responders
        public const int RESPONDERS_COUNT = 1;
        public const string RESPONDERS_SUPPORTING_DOCUMENTS = "Responder Evidence(s) Upload ";
        public const string RESPONDERS_EVIDENCE_UPLOAD = "Upload supporting document(s)";
        public const string RESPONDERS_REJECTION = "Reject Request";
        public const string RESPONDERS_ACCEPT_REJECT = "Accept/Reject Request";

        // Incident Authorizer Messages
        public const bool REJECT_INCIDENT = false;
        public const bool ACCEPT_INCIDENT = true;
        public const string ACCEPT_INCIDENT_MESSAGE = "A request was approved by an authorizer";
        public const string REJECT_INCIDENT_MESSAGE = "A request was rejected by an authorizer";

        //Short Code Message 

        public const string INCOMPLETE_MESSAGE = "INCOMPLETE MESSAGE";
        public const string INVALID_LOCATION = "INVALID LOCATION";
        public const string RESPONSE_MESSAGE = "RESPONSE MESSAGE";
        public const string DETERMINE_INCIDENT = "DETERMINE INCIDENT";
        public const string SHORT_CODE_PARAMETER= "SHORT CODE";
        public const string SHORT_CODE_API= "SHORT CODE API";
        public const string PARAMETER_NOT_SET= "Parameter Not Set up";
        public const string NO_REQUEST= "No Request";
        public const string NOT_SENT= "Message not sent";
        public const string SHORTCODEAPI = "SHORTCODEAPI";
        public const string SHORTCODEUSERNAME = "SHORTCODEUSERNAME";
        public const string SHORTCODEPASSWORD = "SHORTCODEPASSWORD";


        //  Sending of Emails
        public const string _smtpusername = "SMTPNAME";
        public const string _smtpHost = "SMTPHOST";
        public const string _emailFrom = "SMTPFROM";
        public const string _smtpEnableSsl = "SMTPENABLESSL";
        public const string _smtppassword = "SMTPPASSWORD";
        public const string _smtpPort = "SMTPPORT";
        public const string _SenderId = "SMTPSENDER";
        public const string SMTP_LOOKUP = "SMTP";

        // Fetching of Emails (HangFire)
        public const string _smtpEmailAddressSpool = "EMAILADDRESS";
        public const string _smtpHostSpool = "HOST";
        public const string _smtpEnableSslSpool = "ENABLESSL";
        public const string _smtppasswordSpool = "PASSWORD";
        public const string _smtpPortSpool = "PORT";
        public const string SMTP_LOOKUPSpool = "SMTP_SPOOL";

        public const string DELETE_UNPROCESS_DATA = "DELETE_UNPROCESS_DATA";
        public const string RESPONSE_TIME = "RESPONSE_TIME";
        public const string UNACKNOWLEDGED_INCIDENT = "UNACKNOWLEDGED_INCIDENT";


        // sms provider names
        public const string SMS_LIVE_247 = "SMS Live 247";
        public const string SMS_PROVIDER_NIGERIA = "SMS Provider Nigeria";
        public const string BETA_SMS = "Beta SMS";


        //stored procedure name  
        #region Stored Procedure Name
        public const string IncidentNotification = "IncidentNotification";
        public const string FetchIncidentsByReporter = "FetchIncidentsByReporter";
        public const string FetchIncidentsByAgent = "FetchIncidentsByAgent";
        public const string FetchIncidentByResponderId = "FetchIncidentByResponderId";
        public const string FetchIncidentsBySearchCriteria = "FetchIncidentsBySearchCriteria";
        public const string FetchIncidentsByLocality = "FetchIncidentsByLocality";
        public const string FetchIncidentsByLGA = "FetchIncidentsByLGA";
        public const string FetchIncidentsByResponder = "FetchIncidentsByResponder";
        public const string CreateSupportingDocument = "CreateSupportingDocument";
        public const string DeleteUnProcessedDataBank = "DeleteUnProcessedDataBank";
        public const string UpdateIncidentStatus = "UpdateIncidentStatus";
        public const string UpdateIncidentInDatabank = "UpdateIncidentInDatabank";
        public const string UpdateResponderStatus = "UpdateResponderStatus";
        public const string DeleteAllFromTempDocument = "DeleteAllFromTempDocument";
        public const string FetchLatestFromTempDocument = "FetchLatestFromTempDocument";
        public const string GenerateCaseNote = "spCaseNoteNumber";
        public const string SpGetActivityLog = "SpGetActivityLog";
        public const string FetchIncidentReportByLGAToday = "FetchIncidentReportByLGAToday";
        public const string FetchIncidentReportByToday = "FetchIncidentReportByToday";
        public const string FetchIncidentReportByYear = "FetchIncidentReportByYear";
        public const string FetchIncidentReportByMonthly = "FetchIncidentReportByMonthly";
        public const string SpGetAllActivityLog = "SpGetAllActivityLog";
        public const string SpMiniPlantDashboard = "MiniPlantDashboard";
        public const string spPlantdminDashboard = "PlantdminDashboard";
        public const string spCentralAdminDashboard = "CentralAdminDashboard";
        public const string FetchIncidentReportByDate = "FetchIncidentReportByDate";
        public const string FetchFullIncidentReportToday = "FetchFullIncidentReportToday";
        public const string GetGoogleMapByDateRange = "GetGoogleMapByDateRange";
        public const string GetGoogleMapByParameter = "GetGoogleMapByParameter";
        public const string GetGoogleMapByDateRangeByResponder = "GetGoogleMapByDateRangeByResponder";
        public const string FetchIncidentReportByLGAWeekly = "FetchIncidentReportByLGAWeekly";
        public const string FetchIncidentReportByLGAYear = "FetchIncidentReportByLGAYear";
        public const string FetchIncidentReportByLGAMonthly = "FetchIncidentReportByLGAMonthly";
        public const string FetchIncidentReportMessageTypeWeekly = "FetchIncidentReportMessageTypeWeekly";
        public const string FetchIncidentReportMessageTypeYear = "FetchIncidentReportMessageTypeYear";
        public const string FetchIncidentReportMessageTypeMonth = "FetchIncidentReportMessageTypeMonth";
        public const string FetchTotalIncidentReport = "FetchTotalIncidentReport";
        public const string FetchPendingRequestByAuthorizer = "FetchPendingRequestByAuthorizer";
        public const string FetchTotalRequestByResponder = "FetchTotalRequestByResponder";
        public const string FetchIncidentReportMessageTypeToday = "FetchIncidentReportMessageTypeToday";
        public const string FetchIncidentReportByWeek = "FetchIncidentReportByWeek";
        public const string GetIncidentResponderUsersByIncidentId = "GetIncidentResponderUsersByIncidentId";
        public const string FetchUserPermissionAndRole = "FetchUserPermissionAndRole";
        public const string FetchIncidentByChannels = "FetchIncidentByChannels";
        public const string FetchIncidentTypeByDateReport = "FetchIncidentTypeByDateReport";
        public const string FetchTotalResolvedResponderRequestByIncidentType = "FetchTotalResolvedResponderRequestByIncidentType";
        public const string FetchTotalPendingResponderRequestByIncidentType = "FetchTotalPendingResponderRequestByIncidentType";
        public const string FetchTotalAcceptedResponderRequestByIncidentType = "FetchTotalAcceptedResponderRequestByIncidentType";
        public const string FetchResponderUnTreatedIncident = "FetchResponderUnTreatedIncident";
        public const string FetchUnclosedInident = "FetchUnclosedInident";
        public const string FetchIncidentUnAssigned = "FetchIncidentUnAssigned";
        public const string UpdateIncidentUnAssignedAfterEmail = "UpdateIncidentUnAssignedAfterEmail";
        public const string UpdateResponderUnTreatedIncident = "UpdateResponderUnTreatedIncident";
        //new
        public const string FetchMiniPlantRequestByPlantId = "FetchMiniPlantRequestByPlantId";
        #endregion


        public const string MESSAGE_TYPE = "TEXT";

        #region Beta SMS Response Code

        public const string SUCCESS = "SUCCESS";
        public const string SUCCESS_SENDING = "1701";
        public const string INVALID​_URL​_OR_RECIPIENTS_TOO_LONG = "1705";
        public const string INVALID​_​USER​NAME_OR_​PASSWORD = "1702";
        public const string INSUFFICIENT_CREDIT = "1025";
        public const string INSUFFICIENT_CREDIT_2 = "1704";
        public const string INTERNAL_SERVER_ERROR = "1706";
        #endregion

        public const int Approved =1;
        public const int Rejected = 2;
        public const int Resolved = 4;
        public const int NONE_SELECTED = 0;
        public const string None = "";


        public const string SystemAdministrator = "3951b0f5-cf16-4007-be43-b260171f8797";
        public const string CentralAdmin = "f893c609-4974-4078-a5a7-2efdbafebd54";
        public const string PlantAdmin = "c4081fb6df764e16b1ae4ba554291bbd";
        public const string PlantStoreOfficer = "dafa26bea2a541eead15f9783da8e379";
        public const string MiniAdmin = "ad2cb3e5fb3841bb81a5d8edef3862a5";
        public const string Plant_Blanking = "06636feae4cf44c49f9d9f05e998a703";
        public const string Screen_Printing = "cd670349bac24d778a0791b27516dc9b";
        public const string EMBOSSING_Role = "7940d00528d743639e7c90e95be19de8";
        public const string PackagingRequest = "f096c5628ec44d1a814a0e8a1bad0fb3";
        public const string SiturationOfficerRole = "2c0935dc-e157-44cc-900b-57c51a9ac505";
        public const string ExecutiveRole = "d763fd27-6c70-4b0c-b2f8-161bd360fee8";
        public const string TensionManager = "5507d0a9-4dbc-41f6-83ae-832939b1bf4d";

        //push notifiation message
        public const string MessageTitle = "Incident Alert";
        public const string MessageBody = "You have new update, kindly open your application for detail";
        public const string Private_Key = "private_key.json";
        public const string PrivateDocumentFolder = "SupportingDocument";


        #region Approval Status
        public static string MINI_PLANT_REQUEST = "Pending";
        public static string MINI_PLANT_REQUEST_REJECTED = "Rejected";
        public static string PLANT_BLANKING_REVIEWER = "Awaiting Blanking";
        public static string PRINTING_REVIEWER = "Awaiting Printing";
        public static string EMBOSSING_REVIEWER = "Awaiting Embossing";
        public static string PACKAGING_DELIVERY_REVIEWER = "Awaiting Packaging & Delivery";
        public static string MINI_PLANT_CONFIRMATION = "Requestor Confirmation";
        public static string MINI_PLANT_RECEIVE_PLATES = "Requestor Get Plates";
        public static string RECEIVED_PLATES = "Received";
        public static string AWAITING_PLATES = "Awaiting";

        #endregion
        #region  

        #endregion
        //public static int HandleDatabaseUpdateException(Exception ex)
        //{
        //    int? err = null;
        //    if (ex is DbUpdateException dbUpdateException)
        //    {
        //        if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.InnerException != null)
        //        {
        //            if (dbUpdateException.InnerException.InnerException is SqlException sqlException)
        //            {
        //                switch (sqlException.Number)
        //                {
        //                    case 2627:  // Unique constraint error
        //                        err = 2627;
        //                        break;
        //                    case 547:   // Constraint check violation, Conflict in the database
        //                        err = 547;
        //                        break;
        //                    case 2601:  // Duplicated key row error // Constraint violation exception // A custom exception of yours for concurrency issues           
        //                        err = 2601;
        //                        break;
        //                    default:
        //                        err = 0;
        //                        break;
        //                        // A custom exception of yours for other DB issues
        //                        // throw new DatabaseAccessException(dbUpdateException.Message, dbUpdateException.InnerException);
        //                }
        //            }
        //        }
        //    }
        //    if (ex is DbUpdateConcurrencyException concurrencyException)
        //    {

        //    }
        //    return err.Value;
        //}



    }

    
}
