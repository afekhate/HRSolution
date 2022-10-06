using HRSolution.Infrastructure.Domain.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Contract
{
    public interface IJobCreate : IAsyncGenericRepository<JobCreation>
    {

    }
}
