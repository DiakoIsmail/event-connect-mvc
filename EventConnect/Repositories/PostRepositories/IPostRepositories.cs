using EventConnect.Dtos.Posts;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;

namespace EventConnect.Repositories.PostRepositories;

public interface IPostRepositories
{
    Task<List<Post>> GetAllPosts();
    Task<Post> GetPostById(string id);
    Task<List<Post>> GetPostsByUserId(string id);
    Task<Post> AddPost(Post entity );
    Task<Post> UpdatePost(Post entity);
    Task<List<Post>> DeletePost(Post post, string userId);
}