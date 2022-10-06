using HRSolution.Core.Contract;
using HRSolution.Core.Handler;
using HRSolution.Infrastructure.Core.Handler;
using HRSolution.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


//using VatFramework.Web.Areas.Admin.Controllers;

namespace HRSolution.Services
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<DbContext, HRContext>();
            services.AddTransient(typeof(DbContextOptions<HRContext>));
            
            //// Services
            services.AddTransient<IJobSpec, JobSpecService>();
            services.AddTransient<IDepartment, DepartmentService>();
            services.AddTransient<IUnit, UnitService>();
            services.AddTransient<IBranches, BranchService>();
            services.AddTransient<IStaff, StaffService>();
            services.AddTransient<ISysSettingService, SysSettingService>();
            services.AddTransient<IEmailLog, EmailLogService>();
            services.AddTransient<IEmailAttachment, EmailAttachmentService>();
            services.AddTransient<IEmailService, EmailingService>();
            services.AddTransient<IEmailTemplate, EmailTemplateService>();
            services.AddTransient<IJobCreate, JobCreateService>();
            services.AddTransient<IJobHistory, JobHistoryService>();





            // Domain.Core - Identity
            //services.AddScoped<IAuthUser, AuthUser>();
            //services.AddScoped<IAuthenication, AuthenticationService>();


        }
    }
}
