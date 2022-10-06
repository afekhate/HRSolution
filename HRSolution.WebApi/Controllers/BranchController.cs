using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Branches;
using HRSolution.Infrastructure.DTOs.Branches;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {

        private readonly IBranches _Branch;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public BranchController(IConfiguration configuration, ILogger<AuthenticateController> logger, IBranches Branch)
        {

            _configuration = configuration;
            _logger = logger;
            _Branch = Branch;
        }

        [HttpGet]
        [Route(nameof(GetBranches))]
        [ProducesResponseType(typeof(OutPutResult<IList<BranchDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBranches()
        {
            try
            {

                var Branchs = await _Branch.GetAllAsync();
                var allBranchs = Branchs.Select(x => new BranchDTO
                {
                    BankCode = x.BankCode,
                    BranchCode = x.BranchCode,
                    BranchName = x.BranchName,
                    BranchAddress = x.BranchAddress,
                    ClearingBranchCode = x.ClearingBranchCode,
                    RegionalOffice = x.RegionalOffice,
                    RoutingNo = x.RoutingNo


                }).ToList();


                if (allBranchs.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<BranchDTO>>
                        {
                            HasError = false,
                            Result = allBranchs,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allBranchs.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<BranchDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<BranchDTO>>
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
                var u = new OutPutResult<List<BranchDTO>>
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
        [Route(nameof(AddBranch))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddBranch(BranchDTO model)
        {

            try
            {
                var branch = new Branch()
                {
                    BankCode = model.BankCode,
                    BranchCode = model.BranchCode,
                    BranchName = model.BranchName,
                    BranchAddress = model.BranchAddress,
                    ClearingBranchCode = model.ClearingBranchCode,
                    RegionalOffice = model.RegionalOffice,
                    RoutingNo = model.RoutingNo,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name
                };

                var result = await _Branch.AddAsync(branch);
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
        [Route(nameof(UpdateBranch))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBranch(BranchDTO model)
        {

            try
            {
                var branch = new Branch()
                {
                    BankCode = model.BankCode,
                    BranchCode = model.BranchCode,
                    BranchName = model.BranchName,
                    BranchAddress = model.BranchAddress,
                    ClearingBranchCode = model.ClearingBranchCode,
                    RegionalOffice = model.RegionalOffice,
                    RoutingNo = model.RoutingNo,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    ModifiedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    LastModified = DateTime.Now
                };

                await _Branch.UpdateAsync(branch);

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
        [Route("GetBranchByCode/{BranchCode:Int}")]
        [ProducesResponseType(typeof(OutPutResult<BranchDTO>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBranchByCode(int BranchCode)
        {
            try
            {
                var existingBranch = await _Branch.Query($"where BranchCode = {BranchCode}");

                if (existingBranch.Any())
                {
                    var result = await _Branch.GetByIdAsync(existingBranch.FirstOrDefault().Id);

                    if (result != null)
                    {
                        var branch = new BranchDTO
                        {
                            BankCode = result.BankCode,
                            BranchCode = result.BranchCode,
                            BranchName = result.BranchName,
                            BranchAddress = result.BranchAddress,
                            ClearingBranchCode = result.ClearingBranchCode,
                            RegionalOffice = result.RegionalOffice,
                            RoutingNo = result.RoutingNo

                        };

                        var response = new OutPutResult<BranchDTO>
                        {

                            HasError = false,
                            Result = branch,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        };

                        return Ok(response);
                    }
                    else
                    {
                        return Ok(
                           new OutPutResult<BranchDTO>
                           {
                               HasError = false,
                               Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                               Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                               StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                           });
                    }
                }
                else
                {
                    return Ok(
                          new OutPutResult<BranchDTO>
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
