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
            return BadRequest(new ErrorResponse { Message = "Full name, email, and password are required." });
        }

        var existingStudent = _context.Students.FirstOrDefault(s => s.Email == request.Email);
        if (existingStudent != null)
        {
            return BadRequest(new ErrorResponse { Message = "A student with this email already exists." });
        }

        var selectedRole = string.IsNullOrWhiteSpace(request.Role) ? "Student" : request.Role.Trim();

        var student = new Student
        {
            FullName = request.FullName,
            Email = request.Email,
            Department = request.Department,
            PasswordHash = HashPassword(request.Password),
            Role = selectedRole,
            JoinedAt = DateTime.UtcNow
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Message = "Signup successful.",
            Status = true,
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Role))
        {
            return BadRequest(new ErrorResponse { Message = "Email, password, and role are required." });
        }

        var student = _context.Students.FirstOrDefault(s => s.Email == request.Email);
        if (student == null || !VerifyPassword(request.Password, student.PasswordHash))
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid email or password." });
        }

        var requestedRole = string.IsNullOrWhiteSpace(request.Role) ? student.Role : request.Role.Trim();
        if (!string.Equals(student.Role, requestedRole, StringComparison.OrdinalIgnoreCase))
        {
            student.Role = requestedRole;
            await _context.SaveChangesAsync();
        }

        return Ok(new AuthResponse
        {
            Message = "Login successful.",
            Status = true,
            token = GenerateToken(student)
        });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

private static string GenerateToken(Student student)
    {
        // In a real application, you would generate a JWT or similar token here.
        // For simplicity, we're just returning a placeholder string.
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{student.Email}:{student.Role}:{DateTime.UtcNow}"));
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
