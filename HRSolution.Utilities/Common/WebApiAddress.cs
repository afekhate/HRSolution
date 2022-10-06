using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Utilities.Common
{
    public static class WebApiAddress
    {
        public static string GetAllGlobusUsers = "GetAllUsers";
        public static string GetAllGlobusBranches = "default";
        public static string GetAllGlobusStaffs = "GetAllUsers";     
        public static string AddBranchApi = "api/Branch/AddBranch";
        public static string GetBranchApi = "api/Branch/GetBranches";
        public static string GetBranchByCode = "api/Branch/GetBranchByCode/";
        public static string GetStaffById = "api/Staff/GetStaffById/";
        public static string AddStaffApi = "api/Staff/AddStaff";
        public static string GetUnsentMails = "api/EmailLog/GetEmailLogByStatus?IsSent=false";





    }

    public static class ProcessStatus
    {
        public static int Pending = 1;
        public static int Processed = 2;
        public static int BusinessMananger = 3;

    }

    public static class Cables
    {
        public static string DSTV = "DSTV";
        public static string GOTV = "GOTV";
        public static string STARTIMES = "STARTIMES";
    }
}
