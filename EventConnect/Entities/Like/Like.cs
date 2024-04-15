namespace EventConnect.Entities.Like;

public class Like
{  
    public string Id { get; set; } // Primary key for the Like entity
    public string PostName { get; set; } // Name of the post that is liked
    // Foreign key for the User who liked the post
    public string UserId { get; set; }
        
    // Navigation property for the User who liked the post
  //  public User User { get; set; }

    // Foreign key for the Post that is liked
   public  string PostId { get; set; }


    // Navigation property for the Post that is liked
 //   public Post.Post Post { get; set; }
}