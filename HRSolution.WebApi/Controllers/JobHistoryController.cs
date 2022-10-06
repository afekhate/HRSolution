using Dapper;
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Job;
using HRSolution.Infrastructure.DTOs.Job;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobHistoryController : ControllerBase
    {

        private readonly IJobHistory _JobHistory;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public JobHistoryController(IConfiguration configuration, ILogger<JobHistoryController> logger, IJobHistory JobHistory)
        {

            _configuration = configuration;
            _logger = logger;
            _JobHistory = JobHistory;
        }




        [HttpPost]
        [Route(nameof(AddJobHistory))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddJobHistory(JobHistoryDTO JobHistory)
        {

            try
            {

                var jobHistory = new JobHistory
                {
                    JobId = JobHistory.JobId,
                    ActorId = JobHistory.ActorId,
                    ActorName = JobHistory.ActorName,
                    Status = JobHistory.Status,
                    Comment = JobHistory.Comment
                };

                var result = await _JobHistory.AddAsync(jobHistory);
                if (result == 1)
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



       


        [HttpGet]
        [Route(nameof(GetJobHistoryByJobId))]
        [ProducesResponseType(typeof(OutPutResult<JobHistory>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetJobHistoryByJobId(int JobId)
        {
            try
            {
                var existingJobHistory = new List<JobHistory>();              
               
                var checkExist = await _JobHistory.Query($"where JobId = {JobId}");
                existingJobHistory = checkExist.ToList();
               

                if (existingJobHistory.Any())
                {

                    var result = existingJobHistory.FirstOrDefault();
                    var JobHistory = new JobHistoryDTO
                    {
                        JobId = result.JobId,
                        ActorName = result.ActorName,
                        Status = result.Status,
                        Comment = result.Comment,
                        CreatedDate = result.CreatedDate

                    };

                    var response = new OutPutResult<JobHistoryDTO>
                    {

                        HasError = false,
                        Result = JobHistory,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    };

                    return Ok(response);

                }
                else
                {
                    return Ok(
                          new OutPutResult<JobHistoryDTO>
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


     

    }

}
