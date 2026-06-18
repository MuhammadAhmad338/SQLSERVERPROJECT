using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Courses.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        _context.Courses.Add(course);

        await _context.SaveChangesAsync();

        return Ok(course);
    }
}