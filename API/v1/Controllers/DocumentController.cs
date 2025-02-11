using API.v1.Models;
using API.v1.Models.DTOs;
using API.v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace API.v1.Controllers
{
    [ApiController]
    [Route("api/v1/")]

    [Authorize] 
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;
        private readonly ApiDbContext dbContext;
        public DocumentController(IDocumentService documentService, ApiDbContext apiDbContext)
        {
            this.documentService = documentService;
            this.dbContext = apiDbContext;
        }

        [HttpGet("Documents")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await documentService.GetAllDocuments();

            if (documents.Count == 0)
            {
                return NotFound(new ApiError
                {
                    Message = $"Документы не найдены",
                    ErrorCode = 1003
                });
            }

            return Ok(documents);
        }

        [HttpGet("Document/{documentId}/Comments")]
        public async Task<IActionResult> GetDocumentComments(int documentId)
        {
            var documentExists = await documentService.CheckDocumentById(documentId);           

            if (!documentExists)
            {
                return NotFound(new ApiError
                {
                    Message = $"Документ с ID {documentId} не найден",
                    ErrorCode = 1004
                });
            }

            var comments = await documentService.GetCommentsByDocumentId(documentId);

            if (comments.Count == 0)
            {
                return NotFound(new ApiError
                {
                    Message = $"Для документа с ID {documentId} отсутствуют комментарии",
                    ErrorCode = 1005
                });
            }

            return Ok(comments);
        }

        [HttpPost("Document/{documentId}/Comment")]
        public async Task<IActionResult> AddComment(int documentId, [FromBody] CommentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError
                {
                    Message = "Некорректные данные комментария",
                    ErrorCode = 1006
                });
            }
            
            var documentExists = await documentService.CheckDocumentById(documentId);

            if (!documentExists)
            {
                return NotFound(new ApiError
                {
                    Message = $"Документ с ID {documentId} не найден",
                    ErrorCode = 1007
                });
            }

            var documentComment = new DocumentComment
            {
                DocumentId = documentId,
                Text = request.Text,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Author = await GetCurrentUser()
            };

            var commentId = await documentService.AddComment(documentComment);
            CommentDto comment = await documentService.GetComment(commentId);

            return CreatedAtAction(nameof(AddComment), comment);
        }
        private async Task<Worker> GetCurrentUser()
        {
            Worker? worker = await dbContext.Workers.FirstOrDefaultAsync(w => w.FullName == User.Identity.Name);

            return worker == null ? throw new UnauthorizedAccessException("Пользователь не найден") : worker;
        }
    }
}
