using Microsoft.AspNetCore.Authentication;

namespace reactCrudBackend.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
    }
}
