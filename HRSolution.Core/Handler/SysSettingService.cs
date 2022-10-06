using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Core.Handler;
using HRSolution.Infrastructure.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Handler
{
    public class SysSettingService : RepositoryBase<SystemSetting>, ISysSettingService
    {
        public SysSettingService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
