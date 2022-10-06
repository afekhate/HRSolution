using HRSolution.Core.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Handler
{
    public class UnitOfWork : IUnitOfWork
    {
        private IServiceProvider _serviceProvider;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IHttpContextAccessor _httpContextAccessor => _serviceProvider.GetService<IHttpContextAccessor>();
        public IConfiguration _configuration => _serviceProvider.GetService<IConfiguration>();
        public ILogger _logger => _serviceProvider.GetService<ILogger>();


        //public IStudentRepository StudentRepository => _serviceProvider.GetService<IStudentRepository>();
        //public ICourseRepository CourseRepository => _serviceProvider.GetService<ICourseRepository>();
        //public IRegisteredCourseRepository RegisteredCourseRepository => _serviceProvider.GetService<IRegisteredCourseRepository>();
    }
}
