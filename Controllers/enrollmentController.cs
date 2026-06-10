using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
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
        return Ok(await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Enrollment>> CreateEnrollment(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEnrollments), new { id = enrollment.Id }, enrollment);
    }
}
