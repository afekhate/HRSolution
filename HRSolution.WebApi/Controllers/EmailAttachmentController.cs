using HRSolution.Core.Contract;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailAttachmentController : ControllerBase
    {

        private readonly IEmailAttachment _EmailAttachment;
        private readonly IDepartment _department;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EmailAttachmentController(IConfiguration configuration, ILogger<AuthenticateController> logger, IEmailAttachment EmailAttachment, IDepartment department)
        {

            _configuration = configuration;
            _logger = logger;
            _EmailAttachment = EmailAttachment;
            _department = department;
        }

        [HttpGet]
        [Route(nameof(GetEmailAttachments))]
        [ProducesResponseType(typeof(OutPutResult<IList<EmailAttachmentDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailAttachments()
        {
            try
            {

                var EmailAttachments = await _EmailAttachment.GetAllAsync();
                var allEmailAttachments = EmailAttachments.Select(x => new EmailAttachmentDTO
                {
                    EmailLogId = x.EmailLogId,
                    FolderOnServer = x.FolderOnServer,
                    FileNameOnServer = x.FileNameOnServer,
                    EmailFileName = x.EmailFileName
                   
                }).ToList();


                if (allEmailAttachments.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<EmailAttachmentDTO>>
                        {
                            HasError = false,
                            Result = allEmailAttachments,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allEmailAttachments.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<EmailAttachmentDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<EmailAttachmentDTO>>
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
                var u = new OutPutResult<List<EmailAttachmentDTO>>
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
        [Route(nameof(AddEmailAttachment))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmailAttachment(EmailAttachmentDTO model)
        {

            try
            {
                var EmailAttachment = new EmailAttachment()
                {
                    EmailLogId = model.EmailLogId,
                    FolderOnServer = model.FolderOnServer,
                    FileNameOnServer = model.FileNameOnServer,
                    EmailFileName = model.EmailFileName,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _EmailAttachment.AddAsync(EmailAttachment);
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






        [HttpDelete]
        [Route(nameof(DeleteEmailAttachment))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmailAttachment(int ID)
        {

            try
            {
                var existingEmailAttachment = _EmailAttachment.GetByIdAsync(ID);
                if (existingEmailAttachment == null)
                {
                    return Ok(
                    new OutPutResult<MessageOut>
                    {
                        HasError = true,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("810").Name,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code
                    });
                }

                await _EmailAttachment.DeleteAsync(ID);

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


    }

}
