using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Branches
{
    public class BranchDTO
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string? RegionalOffice { get; set; }
        public string BankCode { get; set; }
        public string? RoutingNo { get; set; }
        public string? ClearingBranchCode { get; set; }
    }
}
