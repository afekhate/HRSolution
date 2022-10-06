using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.ApiResponse
{
    public class BranchResponse
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string RegionalOffice { get; set; }
        public string BankCode { get; set; }
        public string RoutingNo { get; set; }
        public string ClearingBranchCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class Result
    {
        public List<BranchResponse> branches { get; set; }
    }

    public class BranchResponseRoot
    {
        public Result Result { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }


}
