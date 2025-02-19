namespace API.v1.Models.DTOs
{
    public class WorkerDto
    {
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string Office { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string JobPosition { get; set; } = null!;
        public string SubdepartmentName { get; set; } = null!;
    }
}
