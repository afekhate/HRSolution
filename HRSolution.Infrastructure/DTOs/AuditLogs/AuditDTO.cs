using HRSolution.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.AuditLogs
{
    public class AuditDTO
    {
        public string? UserId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }
        public string Description { get; set; }
        public string Record { get; set; }
        public string OldRecord { get; set; }
        public ActionType ActionType { get; set; }

    }
}
