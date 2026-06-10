using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        return Ok(await _context.Courses.Include(c => c.Enrollments).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        var course = await _context.Courses.Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == id);
        return course == null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> CreateCourse(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }
}
