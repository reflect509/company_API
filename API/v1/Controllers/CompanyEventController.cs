using API.v1.Models;
using API.v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.v1.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CompanyEventController : ControllerBase
    {
        private readonly ICompanyEventService _eventService;

        public CompanyEventController(ICompanyEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("Events")]
        public async Task<IActionResult> GetEvents()
        {
            var items = await _eventService.GetEventsAsync();
            if (items == null || items.Count == 0)
                return NotFound(new ApiError { Message = "Events not found", ErrorCode = 3001 });

            return Ok(items);
        }
    }
}
