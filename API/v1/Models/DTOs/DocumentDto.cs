namespace API.v1.Models.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public string Category { get; set; }
        public bool HasComments { get; set; }
    }
}
    