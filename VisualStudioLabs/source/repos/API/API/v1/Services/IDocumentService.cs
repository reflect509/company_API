using API.v1.Models;
using API.v1.Models.DTOs;

namespace API.v1.Services
{
    public interface IDocumentService
    {
        Task<List<DocumentDto>> GetAllDocuments();
        Task<List<CommentDto>?> GetCommentsByDocumentId(int id);
        Task<bool> CheckDocumentById(int id);
        Task<int> AddComment(DocumentComment DocumentComment);
        Task<CommentDto> GetComment(int commentId);        
    }
}
