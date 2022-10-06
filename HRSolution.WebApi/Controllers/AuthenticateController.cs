
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain;
using HRSolution.Infrastructure.Domain.Authentication;
using HRSolution.Infrastructure.DTOs;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEmailService _emailManager;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthenticateController> logger, IEmailService emailManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _emailManager = emailManager;   
        }

        [HttpPost]
        [Route(nameof(Login))]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                       
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(
                      new ApiResult<MessageOut>
                      {
                          HasError = false,
                          Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                          StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code,
                          Token = new JwtSecurityTokenHandler().WriteToken(token),
                          expiration = token.ValidTo

                      });


                }

                return Ok(
                      new ApiResult<MessageOut>
                      {
                          HasError = true,
                          Message = ApplicationResponseCode.LoadErrorMessageByCode("606").Name,
                          StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("606").Code
                      });
              
            }
            catch (Exception ex)
            {


                var u = new ApiResult<MessageOut>
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
        [Route(nameof(Register))]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            try
            {

                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    var checkExist = await roleManager.RoleExistsAsync(model.RoleName);
                        if(!checkExist)
                        return Ok(
                         new ApiResult<MessageOut>
                         {
                             HasError = true,
                             Info = ApplicationResponseCode.LoadErrorMessageByCode("809").Name,
                             Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                             StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code
                         });

                }

                    var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("700").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("700").Code
                       });


                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)

                    return Ok(
                      new ApiResult<MessageOut>
                      {
                          HasError = true,
                          Info = result.ToString(),
                          Message = ApplicationResponseCode.LoadErrorMessageByCode("500").Name,
                          StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("500").Code
                      });


                //Initializing the Roles

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.HRAdmin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.HRAdmin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Audit))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Audit));

                if (!await roleManager.RoleExistsAsync(UserRoles.SysAdmin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.SysAdmin));


                //Assigning Role to User
                if(string.IsNullOrEmpty(model.RoleName))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, model.RoleName);
                }

                // Send Mail
                var emailtemplate = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.ACCOUNT_CREATION);

               
                var emailtemplateAudit = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.APPROVAL_REQUEST);

                //// Notify Audit

                //Multiple auditor
                //var approver = _context.ApplicationUsers.Where(a => a.RoleId == DefineRoles.AccountManagerRole && a.IsActive == true).ToList();


                var emailTokens = new List<EmailTokenDTO>
                {
                    new EmailTokenDTO { Token = EmailTokenConstants.FULLNAME, TokenValue = model.Username },
                    new EmailTokenDTO { Token = EmailTokenConstants.USERNAME, TokenValue = user.UserName },
                    new EmailTokenDTO { Token = EmailTokenConstants.PASSWORD, TokenValue = model.Password },
                    new EmailTokenDTO {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                    new EmailTokenDTO {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalURL"] },

                };
                bool mailresponse = await _emailManager.PrepareEmailLog(EmailTemplateCode.ACCOUNT_CREATION, user.Email, _configuration["cc"], "", emailTokens, user.Id,DateTime.Now, false);

                //single auidtor


                var emailToken = new List<EmailTokenDTO>
                    {
                        
                        new EmailTokenDTO { Token = EmailTokenConstants.USERNAME, TokenValue = "Auditor" },
                        new EmailTokenDTO {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                        new EmailTokenDTO {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalURL"] },

                    };

                await _emailManager.PrepareEmailLog(EmailTemplateCode.APPROVAL_REQUEST, "fredrickafekhare@gmail.com", _configuration["cc"], "", emailTokens, user.Id, DateTime.Now, false);

                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                        Info = ApplicationResponseCode.LoadErrorMessageByCode("3013").Name,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    });
            }
            catch (Exception ex)
            {

                var u = new ApiResult<MessageOut>
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
