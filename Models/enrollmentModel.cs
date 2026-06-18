using System.ComponentModel.DataAnnotations.Schema;

public class Enrollment
{
    public int Id { get; set; }

    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    public string Status { get; set; } = "Active";

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}