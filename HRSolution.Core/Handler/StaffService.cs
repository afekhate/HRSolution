using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Core.Handler;
using HRSolution.Infrastructure.Domain.Departments;
using HRSolution.Infrastructure.Domain.GStaff;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Handler
{
    public class StaffService : RepositoryBase<Staff>, IStaff
    {
        public StaffService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
