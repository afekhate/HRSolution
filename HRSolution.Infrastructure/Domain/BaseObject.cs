using HRSolution.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain
{
    public abstract class BaseObject
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; } 
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string IPAddress { get; set; } = IPAddressUtil.GetLocalIPAddress();
    }

    public class JobBaseObject
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; } 
        public bool? IsDeleted { get; set; }

       




    }

    public abstract class StaffBaseObject
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public string? ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public string IPAddress { get; set; } = IPAddressUtil.GetLocalIPAddress();
    }

}
