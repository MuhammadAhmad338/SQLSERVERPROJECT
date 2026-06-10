using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> Signup([FromBody] SignupRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse { Message = "Full name, email, and password are required." });
        }

        var existingStudent = _context.Students.FirstOrDefault(s => s.Email == request.Email);
        if (existingStudent != null)
        {
            return BadRequest(new AuthResponse { Message = "A student with this email already exists." });
        }

        var student = new Student
        {
            FullName = request.FullName,
            Email = request.Email,
            Department = request.Department,
            PasswordHash = HashPassword(request.Password),
            Role = "Student",
            JoinedAt = DateTime.UtcNow
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Message = "Signup successful.",
            StudentId = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            Role = student.Role
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse { Message = "Email and password are required." });
        }

        var student = _context.Students.FirstOrDefault(s => s.Email == request.Email);
        if (student == null || !VerifyPassword(request.Password, student.PasswordHash))
        {
            return Unauthorized(new AuthResponse { Message = "Invalid email or password." });
        }

        return Ok(new AuthResponse
        {
            Message = "Login successful.",
            StudentId = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            Role = student.Role
        });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
