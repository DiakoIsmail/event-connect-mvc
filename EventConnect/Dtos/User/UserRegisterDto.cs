namespace EventConnect.Dtos.User;

public class UserRegisterDto
{
    public string  Email { get; set; }= String.Empty;
    public string Firstname { get; set; }= String.Empty;
    public string Lastname { get; set; }= String.Empty;
    public string DisplayName { get; set; }= String.Empty;
    public string Password { get; set; } = string.Empty;
}