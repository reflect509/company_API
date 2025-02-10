using API.v1.Models;
using API.v1.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.v1.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApiDbContext dbContext;
        public DocumentService(ApiDbContext apiDbContext)
        {
            dbContext = apiDbContext;
        }
        public async Task<List<DocumentDto>> GetAllDocuments()
        {
            return await dbContext.Documents
                .AsNoTracking()
                .Select(d => new DocumentDto
                {
                    Id = d.DocumentId,
                    Title = d.Title,
                    DateCreated = d.DateApproval.ToString("yyyy-MM-dd HH:mm:ss"),
                    DateUpdated = d.DateEdit.ToString("yyyy-MM-dd HH:mm:ss"),
                    Category = d.DocumentType,
                    HasComments = d.DocumentComments.FirstOrDefault() != null
                })
                .ToListAsync();
        }

        public async Task<List<CommentDto>?> GetCommentsByDocumentId(int id)
        {
            return await dbContext.Documents
                .AsNoTracking()
                .Where(d => d.DocumentId == id)
                .SelectMany(d => d.DocumentComments
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        DocumentId = c.DocumentId,
                        Text = c.Text,
                        DateCreated = d.DateApproval.ToString("yyyy-MM-dd HH:mm:ss"),
                        DateUpdated = d.DateEdit.ToString("yyyy-MM-dd HH:mm:ss"),
                        Author = new AuthorDto
                        {
                            FullName = c.Author.FullName,
                            JobPosition = c.Author.JobPosition
                        }
                    }))
                .ToListAsync();
        }

        public async Task<bool> CheckDocumentById(int id)
        {
            return await dbContext.Documents
                .AsNoTracking()
                .AnyAsync(d => d.DocumentId == id);
        }

        public async Task<int> AddComment(DocumentComment documentComment)
        {
            dbContext.DocumentComments.Add(documentComment);
            await dbContext.SaveChangesAsync();
            return documentComment.CommentId;
        }

        public async Task<CommentDto> GetComment(int commentId)
        {
            var comment = await dbContext.DocumentComments
                .AsNoTracking()
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);

            CommentDto commentDto = new CommentDto
                {
                    Id = comment.CommentId,
                    DocumentId = (int)comment.DocumentId,
                    Text = comment.Text,
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DateUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Author = new AuthorDto
                    {
                        FullName = comment.Author.FullName,
                        JobPosition = comment.Author.JobPosition
                    }
                };

            return commentDto;
        }

    }
}
