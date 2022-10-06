using HRSolution.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.AuditLogs
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }
        public string Description { get; set; }
        public string? Record { get; set; }
        public string? OldRecord { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } 

    }

    
}
