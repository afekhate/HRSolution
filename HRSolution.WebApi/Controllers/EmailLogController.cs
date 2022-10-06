using HRSolution.Core.Contract;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailLogController : ControllerBase
    {

        private readonly IEmailLog _EmailLog;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
       

        public EmailLogController(IConfiguration configuration, ILogger<EmailLogController> logger, IEmailLog EmailLog)
        {

            _configuration = configuration;
            _logger = logger;
            _EmailLog = EmailLog;
            
        }

       
        [HttpGet]
        [Route(nameof(GetEmailLog))]
        [ProducesResponseType(typeof(OutPutResult<IList<EmailTemplateDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailLog()
        {
            try
            {
               
                   
                  
                    var EmailLog = await _EmailLog.GetAllAsync();
                    var allEmailLog = EmailLog.Select(x => new EmailLogDTO
                    {
                        Sender = x.Sender,
                        Receiver = x.Receiver,
                        CC = x.CC,
                        BCC = x.BCC,
                        Subject = x.Subject,
                        MessageBody = x.MessageBody,
                        HasAttachment = x.HasAttachment,
                        IsSent = x.IsSent,
                        Retires = x.Retires,
                        DateSent = x.DateSent,
                        DateToSend = x.DateToSend,
                        EmailAttachments = GenericQuery($"SELECT * FROM EmailAttachment where EmailLogId = {x.Id}") 

                    }).ToList();


                    if (allEmailLog.Count != 0)
                    {
                        return Ok(
                            new OutPutResult<List<EmailLogDTO>>
                            {
                                HasError = false,
                                Result = allEmailLog,
                                Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                            });
                    }

                    if (allEmailLog.Count == 0)
                    {
                        return Ok(
                            new OutPutResult<IList<EmailTemplateDTO>>
                            {
                                HasError = false,
                                Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                                Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                            });
                    }
                    else
                    {
                        var response = new OutPutResult<IList<EmailTemplateDTO>>
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
                var u = new OutPutResult<List<EmailTemplateDTO>>
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
        private List<EmailAttachment> GenericQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                conn.Open();
                var data =  conn.Query<EmailAttachment>(sql);
                return (List<EmailAttachment>)data;
            }
        }

        [HttpGet]
        [Route(nameof(GetEmailLogByStatus))]
        [ProducesResponseType(typeof(OutPutResult<IList<EmailTemplateDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailLogByStatus(bool IsSent)
        {
            try
            {
                int status = 0;

                if (IsSent == false)
                { status = 0; }
                else { status = 1; }
                    
                

                string sql = $"where IsSent = {status}";

                var EmailLog = await _EmailLog.Query(sql);
                var allEmailLog = EmailLog.Select(x => new EmailLogDTO
                {
                    Id = x.Id,
                    Sender = x.Sender,
                    Receiver = x.Receiver,
                    CC = x.CC,
                    BCC = x.BCC,
                    Subject = x.Subject,
                    MessageBody = x.MessageBody,
                    HasAttachment = x.HasAttachment,
                    IsSent = x.IsSent,
                    Retires = x.Retires,
                    DateSent = x.DateSent,
                    DateToSend = x.DateToSend,
                    EmailAttachments = GenericQuery($"SELECT * FROM EmailAttachment where EmailLogId = {x.Id}")

                }).ToList();


                if (allEmailLog.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<EmailLogDTO>>
                        {
                            HasError = false,
                            Result = allEmailLog,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allEmailLog.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<EmailTemplateDTO>>
                        {
                            HasError = false,
                            Result = new List<EmailTemplateDTO>(),
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        }); ;
                }
                else
                {
                    var response = new OutPutResult<IList<EmailTemplateDTO>>
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
                var u = new OutPutResult<List<EmailTemplateDTO>>
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
        [Route(nameof(AddEmailLog))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmailLog(EmailLogDTO model)
        {

            try
            {
                var EmailLog = new EmailLog()
                {

                    Sender = model.Sender,
                    Receiver = model.Receiver,
                    CC = model.CC,
                    BCC = model.BCC,
                    Subject = model.Subject,
                    MessageBody = model.MessageBody,
                    HasAttachment = model.HasAttachment,
                    IsSent = model.IsSent,
                    Retires = model.Retires,
                    DateSent = model.DateSent,
                    DateToSend = model.DateToSend,
                    EmailAttachments = model.EmailAttachments.ToList(),
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _EmailLog.AddAsync(EmailLog);
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



    }

}
