using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Authentication;
using HRSolution.Infrastructure.Domain.Job;
using HRSolution.Infrastructure.DTOs.Job;
using HRSolution.Utilities.Common;
using HRSolution.Utilities.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobDetailsController : ControllerBase
    {

        private readonly IJobSpec _jobSpec;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public JobDetailsController( IConfiguration configuration, ILogger<JobDetailsController> logger, IJobSpec jobSpec, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _jobSpec = jobSpec;
        }

        [HttpGet]
        [Route(nameof(GetJobSpecifications))]
        [ProducesResponseType(typeof(OutPutResult<IList<JobSpecificationDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetJobSpecifications()
        {
            try
            {
                var jobSpecs = await _jobSpec.GetAllAsync();
                var jobSpecifications = jobSpecs.Select(x => new JobSpecificationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    code = x.Code
                }).ToList();


                if (jobSpecifications.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<JobSpecificationDTO>>
                        {
                            HasError = false,
                            Result = jobSpecifications,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (jobSpecifications.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<JobSpecificationDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<JobSpecificationDTO>>
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
                var u = new OutPutResult<List<JobSpecificationDTO>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u); throw;
            }
            
        }



        [HttpPost]
        [Route(nameof(AddJobSpecifications))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddJobSpecifications(JobSpecificationDTO model)
        {


            try
            {
                var jobSpec = new JobSpecification ()
                { 
                    Name = model.Name, 
                    Code = model.code,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _jobSpec.AddAsync(jobSpec);
                if(result == 1)
                {
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
    }
}
