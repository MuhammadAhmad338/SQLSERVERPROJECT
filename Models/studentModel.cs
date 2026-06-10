public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}