using HRSolution.Infrastructure.Domain.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Units
{
    public class Unit : BaseObject
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
