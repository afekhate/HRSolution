using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Core.Handler;
using HRSolution.Infrastructure.Domain.Branches;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Handler
{
    public class BranchService : RepositoryBase<Branch>, IBranches
    {
        public BranchService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
