using API.v1.Models;
using API.v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.v1.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("News")]
        public async Task<IActionResult> GetNews()
        {
            var items = await _newsService.GetNewsAsync();
            if (items == null || items.Count == 0)
                return NotFound(new ApiError { Message = "News not found", ErrorCode = 2001 });

            return Ok(items);
        }

        [HttpPost("News/{newsId}/Reaction")]
        public async Task<IActionResult> SubmitReaction(int newsId, [FromBody] ReactionRequest request)
        {
            if (!ModelState.IsValid || request.NewsId != newsId)
                return BadRequest(new ApiError { Message = "Invalid request", ErrorCode = 2002 });

            try
            {
                await _newsService.SubmitReactionAsync(newsId, request.IsPositive);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiError { Message = $"News {newsId} not found", ErrorCode = 2003 });
            }
        }
    }
}

