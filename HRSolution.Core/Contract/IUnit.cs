using HRSolution.Infrastructure.Domain.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace HRSolution.Core.Contract
{
    [ScopedService]
    public interface IUnit : IAsyncGenericRepository<Unit>
    {

    }
}
