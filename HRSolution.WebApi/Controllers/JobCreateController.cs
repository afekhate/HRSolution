using Dapper;
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Branches;
using HRSolution.Infrastructure.Domain.Departments;
using HRSolution.Infrastructure.Domain.GStaff;
using HRSolution.Infrastructure.Domain.Job;
using HRSolution.Infrastructure.DTOs;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Infrastructure.DTOs.Job;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCreateController : ControllerBase
    {

        private readonly IJobCreate _JobCreate;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEmailService _emailManager;
        private readonly IDepartment _department;
        private readonly IStaff _staff;
        private readonly IBranches _branch;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Department department = new Department();
        Staff staff = new Staff();
        Branch branch = new Branch();

        public JobCreateController(IConfiguration configuration, ILogger<JobCreateController> logger, IJobCreate JobCreate, IEmailService emailManager, IDepartment department, IStaff staff, IHttpContextAccessor httpContextAccessor, IBranches branch)
        {

            _configuration = configuration;
            _logger = logger;
            _JobCreate = JobCreate;
            _emailManager = emailManager;
            _department = department;
            _staff = staff;
            _httpContextAccessor = httpContextAccessor;
            _branch = branch;       
        }

        [HttpGet]
        [Route(nameof(GetJobs))]
        [ProducesResponseType(typeof(OutPutResult<IList<JobCreation>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetJobs(string? Category)
        {
            try
            {
                var allJobs = new List<JobCreationDTO>();
                var JobCreates = await _JobCreate.GetAllAsync();

                var jobs = JobCreates.Select(job => new JobCreationDTO
                {                  
                    Objective = job.Objective,
                    Accountablities = job.Accountablities,
                    Position = job.Position,
                    Department = job.Department,
                    Unit = job.Unit,
                    ReportTo = job.ReportTo,
                    Supervises = job.Supervises,
                    Location = job.Location,
                    Deadline = job.Deadline,
                    Type = job.Type,
                    Grade = job.Grade,
                    Category = job.Category,
                    Slot = job.Slot,
                    IsTestRequired = job.IsTestRequired,
                    IsInterviewRequired = job.IsInterviewRequired,
                    IsActive = job.IsActive,
                    CreatedDate = job.CreatedDate,
                    Status = job.Status,
                    DepartmentName = _department.DetailsGenerator(department, job.Department).Result.Name,
                    TypeName = JobType.getType(job.Type),
                    LocationName = _branch.DetailsGenerator(branch, job.Location).Result.BranchName
                }).ToList();

                if (string.IsNullOrEmpty(Category))
                {
                    allJobs = jobs;
                }
                if (Category == JobCategory.Internal)
                {
                    allJobs = jobs.Where(x => x.Category == Category).ToList();
                }
                if (Category == JobCategory.External)
                {
                    allJobs = jobs.Where(x => x.Category == Category).ToList();
                }


                if (allJobs.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<JobCreationDTO>>
                        {
                            HasError = false,
                            Result = allJobs,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allJobs.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<JobCreationDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<JobCreationDTO>>
                    {
                        HasError = true,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code

                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var u = new OutPutResult<List<JobCreation>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }

        }



        
        [HttpPost]
        [Authorize]
        [Route(nameof(AddJob))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddJob(JobCreationModel job)
        {

            var CurrentUser = _httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;

            try
            {
                var newJob = new JobCreation
                {
                    Objective = job.Objective,
                    Accountablities = job.Accountablities,
                    Position = job.Position,
                    Department = job.Department,
                    Unit = job.Unit,
                    ReportTo = job.ReportTo,
                    Supervises = job.Supervises,
                    Location = job.Location,
                    Deadline = job.Deadline,
                    Type = job.Type,
                    Grade = job.Grade,
                    Category = job.Category,
                    Slot = job.Slot,
                    IsTestRequired = job.IsTestRequired,
                    IsInterviewRequired = job.IsInterviewRequired,
                    CreatedBy = CurrentUser,
                    Status = JobStatus.getStatus(1)
                };


                var result = await _JobCreate.AddAsync(newJob);
                if (result == 1)
                {
                   
                    //Send Approval Email to HR Admin
                    var emailtemplate = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.JOB_APPROVAL_REQUEST);

                    var emailTokens = new List<EmailTokenDTO>
                    {
                        new EmailTokenDTO { Token = EmailTokenConstants.FULLNAME, TokenValue =  _configuration["HR_Admin_Name"]},
                        new EmailTokenDTO { Token = EmailTokenConstants.DEPARTMENT, TokenValue = _department.DetailsGenerator(department, newJob.Department).Result.Name},
                        new EmailTokenDTO { Token = EmailTokenConstants.POSITION, TokenValue = newJob.Position },
                        new EmailTokenDTO { Token = EmailTokenConstants.SLOT,  TokenValue = newJob.Slot.ToString() },
                        new EmailTokenDTO { Token = EmailTokenConstants.CATEGORY,  TokenValue = newJob.Category },
                        new EmailTokenDTO { Token = EmailTokenConstants.INITIATOR,  TokenValue = newJob.CreatedBy },
                        new EmailTokenDTO { Token = EmailTokenConstants.DateRequested,  TokenValue = DateTime.Now.ToString("g")},
                        new EmailTokenDTO { Token = EmailTokenConstants.PORTALNAME,  TokenValue =  _configuration["PortalName"]},
                        new EmailTokenDTO { Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalURL"] },

                    };
                    bool mailresponse = await _emailManager.PrepareEmailLog(EmailTemplateCode.JOB_APPROVAL_REQUEST, _configuration["HR_Admin_Email"], _configuration["cc"], "", emailTokens, newJob.CreatedBy, DateTime.Now, false);

                    return Ok(
                      new OutPutResult<MessageOut>
                      {
                          HasError = false,
                          Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                          StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                      });
                }
                else
                {

                    var response = new OutPutResult<MessageOut>
                    {
                        HasError = false,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("408").Name,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                var u = new OutPutResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }



        [HttpPost]
        [Route(nameof(UpdateJobCreate))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateJobCreate(JobCreation JobCreate)
        {

            try
            {

                await _JobCreate.UpdateAsync(JobCreate);

                var response = new OutPutResult<MessageOut>
                {

                    HasError = false,
                    Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                };

                return Ok(response);

            }
            catch (Exception ex)
            {

                var u = new OutPutResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }


        [HttpGet]
        [Route(nameof(GetJobById))]
        [ProducesResponseType(typeof(OutPutResult<JobCreationDTO>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetJobById(int JobId)
        {
            try
            {
                var existingJobCreate = new List<JobCreation>();

                var checkExist = await _JobCreate.Query($"where Id = {JobId}");
                existingJobCreate = checkExist.ToList();

                if (existingJobCreate.Any())
                {

                    var job = existingJobCreate.FirstOrDefault();
                    var jobDto =  new JobCreationDTO
                    {
                        Objective = job.Objective,
                        Accountablities = job.Accountablities,
                        Position = job.Position,
                        Department = job.Department,
                        Unit = job.Unit,
                        ReportTo = job.ReportTo,
                        Supervises = job.Supervises,
                        Location = job.Location,
                        Deadline = job.Deadline,
                        Type = job.Type,
                        Grade = job.Grade,
                        Category = job.Category,
                        Slot = job.Slot,
                        Status = job.Status,
                        IsTestRequired = job.IsTestRequired,
                        IsInterviewRequired = job.IsInterviewRequired,
                        IsActive = job.IsActive,
                        CreatedDate = job.CreatedDate
                       
                    };


                    var response = new OutPutResult<JobCreationDTO>
                    {

                        HasError = false,
                        Result = jobDto,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    };

                    return Ok(response);

                }
                else
                {
                    return Ok(
                          new OutPutResult<JobCreation>
                          {
                              HasError = false,
                              Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                              Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                              StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                          });
                }
            }
            catch (Exception ex)
            {

                var u = new OutPutResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }


        [HttpPost]
        [Authorize]
        [Route(nameof(ApproveJob))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ApproveJob(JobOperationDTO job)
        {

            try
            {
                var CurrentUser = _httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
                string jobSql= string.Empty;
                string historysql = string.Empty;
                string _connectionString = ConfigHelper.GetCurrentSettings("DefaultConnection", "ConnectionStrings").appSettingValue;
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {

                    //Check external job
                    //var userid = _staff.Query("where ")
                   
                    //update job and History
                    if (JobStatus.getStatus(job.Status) == JobStatusString.Approve)
                    {
                        jobSql = $"update JobCreation set IsActive = 1,Status = '{JobStatusString.Approve}', LastModified = CAST(N'{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}' AS DateTime)  where Id = {job.JobId}";
                        historysql = $"insert into JobHistory (JobId,Status,Comment,ActorName,CreatedDate,CreatedBy) values ({job.JobId},'{JobStatusString.Approve}','{job.Comment}','{CurrentUser}', CAST(N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' AS DateTime), '{CurrentUser}')";
                    }
                    if (JobStatus.getStatus(job.Status) == JobStatusString.Rejected)
                    {
                        jobSql = $"update JobCreation set IsActive = 0, Status = '{JobStatusString.Rejected}', LastModified = CAST(N'{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}' AS DateTime) where Id = {job.JobId}";
                        historysql = $"insert into JobHistory (JobId,Status,Comment,ActorName,CreatedDate,CreatedBy) values ({job.JobId},'{JobStatusString.Rejected}','{job.Comment}','{CurrentUser}', CAST(N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' AS DateTime)),'{CurrentUser}'";
                    }
                    var entity = await conn.QueryAsync<JobCreation>(jobSql);
                    var entry = await conn.QueryAsync<JobHistory>(historysql);




                    var response = new OutPutResult<MessageOut>
                    {

                        HasError = false,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("202").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                var u = new OutPutResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }

        [HttpDelete]
        [Route(nameof(DeactivateJob))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeactivateJob(int JobID)
        {

            try
            {
                string _connectionString = ConfigHelper.GetCurrentSettings("DefaultConnection", "ConnectionStrings").appSettingValue;
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var sysSql = $"update JobCreation set IsActive = 0 , LastModified = '{DateTime.Now}' where Id = {JobID}";
                    var entity = await conn.QueryAsync<JobCreation>(sysSql);


                    var response = new OutPutResult<MessageOut>
                    {

                        HasError = false,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("202").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                var u = new OutPutResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }


    }

}
