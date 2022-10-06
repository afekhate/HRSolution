using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Contract
{
   
    public interface IUnitOfWork
    {
        IHttpContextAccessor _httpContextAccessor { get; }
        IConfiguration _configuration { get; }
        ILogger _logger { get; }

    }

}
