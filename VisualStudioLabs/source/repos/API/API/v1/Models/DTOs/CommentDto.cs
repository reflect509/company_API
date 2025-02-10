namespace API.v1.Models.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string Text { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public AuthorDto Author { get; set; }
    }
}
