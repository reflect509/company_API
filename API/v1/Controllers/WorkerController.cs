using API.v1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.v1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService workerService;

        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }

        [HttpPost("Worker/{workerId}/generate-credentials")]
        public async Task<IActionResult> GenerateCredentials(int workerId)
        {
            try
            {
                var (username, password) = await workerService.GenerateCredentialsAsync(workerId);

                return Ok(new { username, password });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Worker/{workerId}")]
        public async Task<IActionResult> GetWorker(int workerId)
        {
            var worker = await workerService.GetWorkerByIdAsync(workerId);
            if (worker is null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

    }
}
