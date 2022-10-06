using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Job
{
    public class JobHistoryDTO 
    {
        public int JobId { get; set; }
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class JobOperationDTO
    {
        public int JobId { get; set; }
        //public int ActorId { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
       

    }
}
