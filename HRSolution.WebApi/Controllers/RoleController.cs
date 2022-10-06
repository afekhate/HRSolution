using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain;
using HRSolution.Infrastructure.Domain.AuditLogs;
using HRSolution.Infrastructure.Domain.Authentication;
using HRSolution.Infrastructure.DTOs;
using HRSolution.Infrastructure.Enums;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSolution.WebApi.Controllers
{
    //[Authorize(Roles = $"{UserRoles.SysAdmin},{UserRoles.HRAdmin},{UserRoles.User}")]
    
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
       

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<RoleController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(GetRoles))]
        [ProducesResponseType(typeof(ApiResult<IList<RoleDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRoles()
        {

            try
            {
                var result = new ApiResult<IList<RoleDTO>>();
                var existingRoles = await roleManager.Roles.Select(x => new RoleDTO
                {
                    RoleID = x.Id,
                    RoleName = x.Name
                }).ToListAsync();

                if (existingRoles.Count != 0)
                {
                    return Ok(
                        new ApiResult<IList<RoleDTO>>
                        {
                            HasError = false,
                            Result = existingRoles,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (existingRoles.Count == 0)
                {
                    return Ok(
                        new ApiResult<IList<RoleDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                var response = new ApiResult<IList<RoleDTO>>
                {
                    HasError = true,
                    Info = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                    Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code

                };
                return Ok(response);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<IList<RoleDTO>>
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
        [Route(nameof(AddRole))]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddRole(string RoleName)
        {
            try
            {  
                if (!await roleManager.RoleExistsAsync(UserRoles.Audit))
                {
                   var result =  await roleManager.CreateAsync(new IdentityRole(RoleName));
                    if(result.Succeeded)
                    {
                        return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = false,
                           Info = RoleName,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                       });
                    }
                    else
                    {
                       return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Info = result.Errors.ToString(),
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code
                           
                       });

                    }

                   
                }
                else
                {
                    return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = false,
                         Info = ApplicationResponseCode.LoadErrorMessageByCode("602").Name,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code

                     });

                   
                }
            }
            catch (Exception ex)
            {

                var u = new ApiResult<IList<RoleDTO>>
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
