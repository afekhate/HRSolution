using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.GStaff;
using HRSolution.Infrastructure.DTOs.GStaff;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {

        private readonly IStaff _Staff;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffController(IConfiguration configuration, ILogger<StaffController> logger, IStaff Staff, IHttpContextAccessor httpContextAccessor)
        {

            _configuration = configuration;
            _logger = logger;
            _Staff = Staff;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route(nameof(GetStaffs))]
        [ProducesResponseType(typeof(OutPutResult<IList<Staff>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {

                var Staffs = await _Staff.GetAllAsync();
                var allStaffs = Staffs.ToList();


                if (allStaffs.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<Staff>>
                        {
                            HasError = false,
                            Result = allStaffs,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allStaffs.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<Staff>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<Staff>>
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
                var u = new OutPutResult<List<Staff>>
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
        [Route(nameof(AddStaff))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddStaff(Staff staff)
        {

            try
            {

                var checkExist = await _Staff.Query($"where mailNickname = '{staff.mailNickname}'");

                if(checkExist.Any())
                {
                    return Ok(
                          new OutPutResult<MessageOut>
                          {
                              HasError = false,
                              Info = ApplicationResponseCode.LoadErrorMessageByCode("700").Name,
                              Message = ApplicationResponseCode.LoadErrorMessageByCode("602").Name,
                              StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("602").Code
                          });
                }
                var result = await _Staff.AddAsync(staff);
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
        [Route(nameof(UpdateStaff))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateStaff(Staff staff)
        {

            try
            {

                await _Staff.UpdateAsync(staff);

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
        [Route("GetStaffById/{StaffIDorName}")]
        [ProducesResponseType(typeof(OutPutResult<Staff>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStaffById(string StaffIDorName)
        {
            try
            {
                var existingStaff = new List<Staff>();

                if (StaffIDorName == "0")
                {
                    var checkExist = await _Staff.Query($"where employeeId = {StaffIDorName}");
                    existingStaff = checkExist.ToList();
                    
                }
                else
                {
                    var checkExist = await _Staff.Query($"where mailNickname = '{StaffIDorName}'");
                    existingStaff = checkExist.ToList();
                }

                if (existingStaff.Any())
                {

                    var result = existingStaff.FirstOrDefault();
                    var staff = new StaffDTO
                    {
                        stringType = result.stringType == null ? "" : result.stringType,
                        stringId = result.stringId,
                        deletionTimestamp = result.deletionTimestamp,
                        accountEnabled = result.accountEnabled,
                        city = result.city,
                        companyName = result.companyName,
                        country = result.country,
                        createdDateTime = result.createdDateTime == null ? DateTime.Now :(DateTime)result.createdDateTime,
                        creationType = result.creationType,
                        department = result.department,
                        displayName = result.displayName,
                        employeeId = result.employeeId,
                        givenName = result.givenName,
                        immutableId = result.immutableId,
                        isCompromised = result.isCompromised,
                        jobTitle = result.jobTitle,
                        lastDirSyncTime = result.lastDirSyncTime,
                        mail = result.mail,
                        mailNickname = result.mailNickname,
                        mobile = result.mobile,
                        postalCode = result.postalCode,
                        preferredLanguage = result.preferredLanguage,
                        state = result.state,
                        streetAddress = result.streetAddress,
                        surname = result.surname,
                        telephoneNumber = result.telephoneNumber,
                        usageLocation = result.usageLocation
   
                    };

                    var response = new OutPutResult<StaffDTO>
                    {

                        HasError = false,
                        Result = staff,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    };

                    return Ok(response);
                   
                }
                else
                {
                    return Ok(
                          new OutPutResult<StaffDTO>
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


        //[HttpDelete]
        //[Route(nameof(DeactivateStaff))]
        //[ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        //[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> DeactivateStaff(int deptID)
        //{

        //    try
        //    {


        //        await _Staff.DeleteAsync(deptID);

        //        var response = new OutPutResult<MessageOut>
        //        {

        //            HasError = false,
        //            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
        //            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
        //        };

        //        return Ok(response);

        //    }
        //    catch (Exception ex)
        //    {

        //        var u = new OutPutResult<MessageOut>
        //        {
        //            HasError = true,
        //            Result = null,
        //            Message = ex.Message,
        //            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
        //        };
        //        return BadRequest(u);
        //    }
        //}

        //public string CurrentUser()
        //{
        //    return _httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
        //}


    }

}
