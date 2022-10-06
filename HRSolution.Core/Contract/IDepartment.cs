
using HRSolution.Infrastructure.Domain.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Contract
{
   

    public interface IDepartment : IAsyncGenericRepository<Department>
    {

    }

}
