using API.v1.Models;
using API.v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.v1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubdepartmentController : ControllerBase
    {
        private readonly ISubdepartmentWorkerService subdepartmentWorkerService;

        public SubdepartmentController(ISubdepartmentWorkerService subdepartmentWorkerService)
        {
            this.subdepartmentWorkerService = subdepartmentWorkerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubdepartments()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new ApiError
                {
                    Message = "Неправильный запрос"
                });
            }

            var subdepartments = await subdepartmentWorkerService.GetSubdepartmentsHierarchyAsync();

            if (subdepartments.Count == 0)
            {
                return StatusCode(404, new ApiError
                {
                    Message = "Подразделения не найдены"
                });
            }
            return Ok(subdepartments);
        }
        
    }
}
