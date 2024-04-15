namespace EventConnect.Dtos.Likes;

public class PostLikeDto
{
    
    // Foreign key for the User who liked the post
    public string email { get; set; } = "";
    
    //Post id
    public string PostId { get; set; }
}