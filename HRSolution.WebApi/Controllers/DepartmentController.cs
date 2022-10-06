using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.Departments;
using HRSolution.Infrastructure.DTOs.Department;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace HRSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartment _department;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public DepartmentController(IConfiguration configuration, ILogger<DepartmentController> logger, IDepartment department)
        {
           
            _configuration = configuration;
            _logger = logger;
            _department = department;
        }

        [HttpGet]
        [Route(nameof(GetDepartments))]
        [ProducesResponseType(typeof(OutPutResult<IList<DepartmentDTO>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {

                var departments = await _department.GetAllAsync();
                var allDepartments = departments.Select(x => new DepartmentDTO
                { 
                    Id = x.Id,
                    Name = x.Name,
                    HOD = x.HOD,
                    
                }).ToList();


                if (allDepartments.Count != 0)
                {
                    return Ok(
                        new OutPutResult<List<DepartmentDTO>>
                        {
                            HasError = false,
                            Result = allDepartments,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }

                if (allDepartments.Count == 0)
                {
                    return Ok(
                        new OutPutResult<IList<DepartmentDTO>>
                        {
                            HasError = false,
                            Info = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                        });
                }
                else
                {
                    var response = new OutPutResult<IList<DepartmentDTO>>
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
                var u = new OutPutResult<List<DepartmentDTO>>
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
        [Route(nameof(AddDepartment))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddDepartment(DepartmentDTO model)
        {

            try
            {
                var dept = new Department()
                {
                    Name = model.Name,
                    HOD = model.HOD,
                    CreatedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                };

                var result = await _department.AddAsync(dept);
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
        [Route(nameof(UpdateDepartment))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateDepartment(DepartmentDTO model)
        {

            try
            {
                var dept = new Department()
                {
                    Id = model.Id,
                    Name = model.Name,
                    HOD = model.HOD,
                    ModifiedBy = User.Identity.Name == null ? "Admin" : User.Identity.Name,
                    LastModified = DateTime.Now
                };

                await _department.UpdateAsync(dept);
                
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
        [Route(nameof(DeactivateDepartment))]
        [ProducesResponseType(typeof(OutPutResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeactivateDepartment(int deptID)
        {

            try
            {
               

                await _department.DeleteAsync(deptID);

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
