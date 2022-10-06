using HRSolution.Infrastructure.Domain.AuditLogs;
using HRSolution.Infrastructure.Domain.Authentication;
using HRSolution.Infrastructure.Domain.Branches;
using HRSolution.Infrastructure.Domain.Departments;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Infrastructure.Domain.GStaff;
using HRSolution.Infrastructure.Domain.Job;
using HRSolution.Infrastructure.Domain.Units;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain
{
    public class HRContext : IdentityDbContext<ApplicationUser>
    {
        public HRContext()
        {

        }
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connect = config.GetSection("ConnectionStrings").Get<List<string>>().FirstOrDefault();
            optionsBuilder.UseSqlServer(connect);

        }
        public DbSet<JobSpecification> JobSpecification { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<EmailAttachment> EmailAttachment { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }
        public DbSet<JobCreation> JobCreation { get; set; }
        public DbSet<JobHistory> JobHistory { get; set; }

    }
}
