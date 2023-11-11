namespace SuperAdmin.Requests.Admin;

public class LoginRequest
{
    public int LoginId { get; set; }

    public string Password { get; set; } = string.Empty;
}
