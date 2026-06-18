using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/enrollment")]
[Route("api/enrollments")]
[ApiController]
public class EnrollmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync();

        return Ok(enrollments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnrollment(
        int studentId,
        int courseId)
    {
        var studentExists =
            await _context.Students
                .AnyAsync(s => s.Id == studentId);

        if (!studentExists)
            return NotFound("Student not found");

        var courseExists =
            await _context.Courses
                .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
            return NotFound("Course not found");

        var alreadyEnrolled =
            await _context.Enrollments.AnyAsync(
                e => e.StudentId == studentId &&
                     e.CourseId == courseId);

        if (alreadyEnrolled)
            return BadRequest("Student already enrolled");

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId
        };

        _context.Enrollments.Add(enrollment);

        await _context.SaveChangesAsync();

        var createdEnrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == enrollment.Id);

        return createdEnrollment == null
            ? NotFound("Enrollment could not be created")
            : CreatedAtAction(nameof(GetEnrollments), new { id = createdEnrollment.Id }, createdEnrollment);
    }
}