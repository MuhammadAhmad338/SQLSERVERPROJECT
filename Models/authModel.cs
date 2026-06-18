public class SignupRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public string? Token { get; set; }
    public string? StudentId { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
