using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Job
{
    public class JobCreation : BaseObject   
    {
        public string Position { get; set; }
        public string Objective { get; set; }
        public string Accountablities { get; set; }
        public int Department { get; set; }
        public int Unit { get; set; }
        public string ReportTo { get; set; }
        public string? Supervises { get; set; }
        public int Location { get; set; }
        public DateTime Deadline { get; set; }
        public string Grade { get; set; } // Graduate Trainee, ABO, BO
        public int Type { get; set; }   //Experience Hire, Collegial
        public string Category { get; set; } // External, Internal
        public int Slot { get; set; }
        public string Status { get; set; }
        public bool IsTestRequired { get; set; }
        public bool IsInterviewRequired { get; set; }



    }
}
