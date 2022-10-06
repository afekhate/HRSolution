using HRSolution.Core.Contract;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using HRSolution.Infrastructure.DTOs;
using HRSolution.Infrastructure.Domain;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingController : ControllerBase
    {

        private readonly ISysSettingService _SysSetting;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public SystemSettingController(IConfiguration configuration, ILogger<SystemSettingController> logger, ISysSettingService SysSetting)
        {

            _configuration = configuration;
            _logger = logger;
            _SysSetting = SysSetting;
            
        }

        [HttpGet]
        [Route(nameof(GetSysSettings))]
        [ProducesResponseType(typeof(OutPutResult<IList<SysSettingDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSysSettings()
        {
            try
            {

                var SysSettings = await _SysSetting.GetAllAsync();
                var allSysSettings = SysSettings.Select(x => new SysSettingDTO
                {
                    LookUpCode = x.LookUpCode,
                    ItemName = x.ItemName,
                    ItemValue = x.ItemValue
                }).ToList();


                if (allSysSettings.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<SysSettingDTO>>
                        {
                            HasError = false,
                            Result = allSysSettings,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allSysSettings.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<SysSettingDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<SysSettingDTO>>
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
                var u = new OutPutResult<List<SysSettingDTO>>
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
        [Route(nameof(AddSysSetting))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSysSetting(SysSettingDTO model)
        {

            try
            {
                var systemSetting = new SystemSetting()
                {

                    LookUpCode = model.LookUpCode,
                    ItemName = model.ItemName,
                    ItemValue = model.ItemValue,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _SysSetting.AddAsync(systemSetting);
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
        [Route(nameof(UpdateSysSetting))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSysSetting(SysSettingDTO model)
        {

            var existingSysSetting = _SysSetting.GetByIdAsync(model.SysSettingId);
            if (existingSysSetting == null)
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
                var dept = new SystemSetting()
                {
                    Id = model.SysSettingId,
                    LookUpCode = model.LookUpCode,
                    ItemName = model.ItemName,
                    ItemValue = model.ItemValue,
                    ModifiedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    CreatedBy = existingSysSetting.Result.CreatedBy,
                    LastModified = DateTime.Now
                };

                await _SysSetting.UpdateAsync(dept);

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
        [Route(nameof(DeleteSysSetting))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSysSetting(int ID)
        {

            try
            {
                var existingSysSetting = _SysSetting.GetByIdAsync(ID);
                if (existingSysSetting == null)
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

                await _SysSetting.DeleteAsync(ID);

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
