using System.ComponentModel.DataAnnotations;

namespace API.v1.Models
{
    public class CommentCreateRequest
    {
        [Required(ErrorMessage = "Текст комментария обязателен")]
        [StringLength(500, ErrorMessage = "Комментарий не может быть длиннее 500 символов")]
        public string Text { get; set; }
    }
}