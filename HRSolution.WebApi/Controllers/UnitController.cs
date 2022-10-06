using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Units;
using HRSolution.Infrastructure.DTOs.Unit;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {

        private readonly IUnit _Unit;
        private readonly IDepartment _department;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public UnitController(IConfiguration configuration, ILogger<AuthenticateController> logger, IUnit Unit, IDepartment department)
        {

            _configuration = configuration;
            _logger = logger;
            _Unit = Unit;
            _department = department;
        }

        [HttpGet]
        [Route(nameof(GetUnits))]
        [ProducesResponseType(typeof(OutPutResult<IList<UnitDTOs>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUnits()
        {
            try
            {

                var Units = await _Unit.GetAllAsync();
                var allUnits = Units.Select(x => new UnitDTOs
                {
                    UnitId = x.Id,
                    Name = x.Name,
                    Department = _department.GetByIdAsync(x.Id).Result.Name,
                    DepartmentId = x.Id
                }).ToList();


                if (allUnits.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<UnitDTOs>>
                        {
                            HasError = false,
                            Result = allUnits,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allUnits.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<UnitDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<UnitDTO>>
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
                var u = new OutPutResult<List<UnitDTO>>
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
        [Route(nameof(AddUnit))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUnit(UnitDTO model)
        {

            try
            {
                var unit = new Unit()
                {
                    Name = model.Name,
                    Id = model.DepartmentId,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _Unit.AddAsync(unit);
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
        [Route(nameof(UpdateUnit))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUnit(UnitDTO model)
        {

            var existingUnit = _Unit.GetByIdAsync(model.DepartmentId);
            if(existingUnit == null)
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
                var dept = new Unit()
                {
                    Name = model.Name,
                    Id = model.DepartmentId,
                    ModifiedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    CreatedBy = existingUnit.Result.CreatedBy,
                    LastModified = DateTime.Now
                };

                await _Unit.UpdateAsync(dept);

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
        [Route(nameof(DeactivateUnit))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeactivateUnit(int deptID)
        {

            try
            {
                var existingUnit = _Unit.GetByIdAsync(deptID);
                if (existingUnit == null)
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

                await _Unit.DeleteAsync(deptID);

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
