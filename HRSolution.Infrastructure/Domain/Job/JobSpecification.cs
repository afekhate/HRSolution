using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Job
{
    public class JobSpecification : BaseObject
    {
   
        public string Name { get; set; }
        public string Code { get; set; }


    }
}
