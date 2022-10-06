using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Job
{
    public class JobHistory : BaseObject
    {
        public int JobId { get; set; }
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}
