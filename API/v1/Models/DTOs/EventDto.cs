namespace API.v1.Models.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}
