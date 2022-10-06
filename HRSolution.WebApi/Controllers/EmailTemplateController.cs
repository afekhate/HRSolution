using HRSolution.Core.Contract;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {

        private readonly IEmailTemplate _EmailTemplate;
        private readonly IDepartment _department;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EmailTemplateController(IConfiguration configuration, ILogger<EmailTemplateController> logger, IEmailTemplate EmailTemplate, IDepartment department)
        {

            _configuration = configuration;
            _logger = logger;
            _EmailTemplate = EmailTemplate;
            _department = department;
        }

        [HttpGet]
        [Route(nameof(GetEmailTemplates))]
        [ProducesResponseType(typeof(OutPutResult<IList<EmailTemplateDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailTemplates()
        {
            try
            {

                var EmailTemplates = await _EmailTemplate.GetAllAsync();
                var allEmailTemplates = EmailTemplates.Select(x => new EmailTemplateDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Subject = x.Subject,
                    MailBody = x.MailBody,
                    Description = x.Description
                }).ToList();


                if (allEmailTemplates.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<EmailTemplateDTO>>
                        {
                            HasError = false,
                            Result = allEmailTemplates,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allEmailTemplates.Count == 0)
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



        [HttpPost]
        [Route(nameof(AddEmailTemplate))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmailTemplate(EmailTemplateDTO model)
        {

            try
            {
                var EmailTemplate = new EmailTemplate()
                {
                   
                    Code = model.Code,
                    Subject = model.Subject,
                    MailBody = model.MailBody,
                    Description = model.Description,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _EmailTemplate.AddAsync(EmailTemplate);
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



        [HttpPost]
        [Route(nameof(UpdateEmailTemplate))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmailTemplate(EmailTemplateDTO model)
        {

            var existingEmailTemplate = _EmailTemplate.GetByIdAsync(model.Id);
            if (existingEmailTemplate == null)
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
            try
            {
                var dept = new EmailTemplate()
                {
                    Id = model.Id,
                    Code = model.Code,
                    Subject = model.Subject,
                    MailBody = model.MailBody,
                    Description = model.Description,
                    ModifiedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    CreatedBy = existingEmailTemplate.Result.CreatedBy,
                    LastModified = DateTime.Now
                };

                await _EmailTemplate.UpdateAsync(dept);

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


        [HttpDelete]
        [Route(nameof(DeleteEmailTemplate))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmailTemplate(int ID)
        {

            try
            {
                var existingEmailTemplate = _EmailTemplate.GetByIdAsync(ID);
                if (existingEmailTemplate == null)
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

                await _EmailTemplate.DeleteAsync(ID);

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
