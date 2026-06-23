using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return Ok(await _context.Students.Include(s => s.Enrollments).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.Include(s => s.Enrollments).FirstOrDefaultAsync(s => s.Id == id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto dto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        student.FullName = dto.FullName;
        student.Department = dto.Department;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    
}
