using System.ComponentModel.DataAnnotations.Schema;

public class Enrollment
{
    public int Id { get; set; }

    [ForeignKey("Student")]
    public int StudentId { get; set; }
    public Student Student { get; set; } = new();

    [ForeignKey("Course")]
    public int CourseId { get; set; }
    public Course Course { get; set; } = new();

    public string Status { get; set; } = "Active";
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}