public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public string Level { get; set; } = "Beginner";

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}