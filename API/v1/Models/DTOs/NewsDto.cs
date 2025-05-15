namespace API.v1.Models.DTOs
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Date { get; set; } = null!;
        public int PositiveReactions { get; set; }
        public int NegativeReactions { get; set; }
    }
}
