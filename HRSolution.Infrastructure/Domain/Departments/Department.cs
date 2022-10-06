using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Departments
{
    public class Department : BaseObject
    {
        public string Name { get; set; }
        public string? HOD { get; set; }

    }
}
