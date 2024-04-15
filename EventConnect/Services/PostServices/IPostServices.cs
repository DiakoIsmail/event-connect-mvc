using EventConnect.Dtos.Posts;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;


namespace EventConnect.Services.PostServices;

public interface IPostServices
{
    Task<ServiceResponse<List<GetPostDto>>> GetAllPosts();
    Task<ServiceResponse<GetPostDto>> GetPostById(string id);
    Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto entity, string emai);
    Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto entity);
    Task<ServiceResponse<List<GetPostDto>>> DeletePost(string id, string userId);

    
}