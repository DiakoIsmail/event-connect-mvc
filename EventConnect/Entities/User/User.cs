


using EventConnect.Entities.Security;


namespace EventConnect.Entities;

public class User:PasswordValues
{
    public string  Email { get; set; }= String.Empty;
    public string Firstname { get; set; }= String.Empty; 
    public string Lastname { get; set; }= String.Empty;
    public string DisplayName { get; set; }= String.Empty;

    public List<Post.Post>? Posts { get; set; }
    
    
}