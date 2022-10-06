using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Core.Handler;
using HRSolution.Infrastructure.Domain.Job;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Handler
{
    
    public class JobHistoryService : RepositoryBase<JobHistory>, IJobHistory
    {
        public JobHistoryService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
