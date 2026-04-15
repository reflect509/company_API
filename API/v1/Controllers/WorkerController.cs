using API.v1.Models;
using API.v1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("Workers/{workerId}/generate-credentials")]
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

        [HttpGet("Workers/{workerId}")]
        public async Task<IActionResult> GetWorker(int workerId)
        {
            var worker = await workerService.GetWorkerByIdAsync(workerId);
            if (worker is null)
            {
                return NotFound();
            }
            return Ok(worker);
        }
        [HttpPatch("Workers/{workerId}")]
        public async Task<IActionResult> UpdateWorker(int workerId, [FromBody] Worker worker)
        {
            if (worker == null || workerId != worker.WorkerId)
                return BadRequest("Неверные данные работника.");

            
            var result = await workerService.UpdateWorkerAsync(worker);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Данные успешно обновлены.");
            
        }
        [HttpPost("Workers/{workerId}/events")]
        public async Task<IActionResult> CreateWorkerEvent(int workerId,[FromBody] Event ev)
        {
            var createdEvent = await workerService
                .AddEventToWorkerAsync(workerId, ev);

            if (createdEvent == null)
                return BadRequest("Worker не найден");

            return Ok(new 
            { 
                createdEvent.EventId,
                createdEvent.EventName,
                createdEvent.EventType,
                createdEvent.Status,
                createdEvent.Date,
                createdEvent.Description
            });
        }
        [HttpGet("Workers/{workerId}/events")]
        public async Task<IActionResult> GetWorkerEvents(int workerId)
        {
            var events = await workerService
                .GetWorkerEventsAsync(workerId);

            return Ok(events);
        }
    }
}
