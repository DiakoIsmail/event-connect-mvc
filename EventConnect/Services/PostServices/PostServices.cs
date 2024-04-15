using System.Security.Claims;

using AutoMapper;
using EventConnect.Data;
using EventConnect.Data.Auth;
using EventConnect.Dtos.Posts;
using EventConnect.Entities;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;
using EventConnect.Repositories.PostRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventConnect.Services.PostServices;

public class PostServices:IPostServices
{
    private readonly IMapper _mapper;
    private readonly IPostRepositories _postRepositories;
    private readonly IAuthRepository _authRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public PostServices(IMapper mapper, IAuthRepository authRepo, IHttpContextAccessor httpContextAccessor, IPostRepositories postRepositories)
    {
        _mapper = mapper;
        _authRepo = authRepo;
        _httpContextAccessor = httpContextAccessor;
        _postRepositories = postRepositories;
    }

    public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
    {
        var serviceResponse = new ServiceResponse<List<GetPostDto>>();
        var posts = await _postRepositories.GetAllPosts();
        serviceResponse.Data = posts.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
        
       return serviceResponse;
    }

    public  async Task<ServiceResponse<GetPostDto>> GetPostById(string id)
    {
        var serviceResponse = new ServiceResponse<GetPostDto>();
        //dont forget to att include likes and comments-----------
        Post post = await _postRepositories.GetPostById(id);
    
        serviceResponse.Data = _mapper.Map<GetPostDto>(post);

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto entity, string email)
    {
       // Initialize a new response object
    var response = new ServiceResponse<List<GetPostDto>>();

    try
    {
        // Map the incoming entity to a Post object
        Post post = _mapper.Map<Post>(entity);

        // Fetch the user associated with the provided email
        User user = await _authRepo.GetUser(email);

        // Set the User property of the post to the fetched user
        post.User = await _authRepo.GetUser( email);

        // If the user is not found, return an error response
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
            return response;
        }

        // Generate a new unique identifier for the post
        string id = Guid.NewGuid().ToString();

        // Set the properties of the post
        post.Id = id;
        post.DateCreated = DateTime.UtcNow;
        post.CreatedBy = id; 
        post.DateModified = DateTime.UtcNow;
        post.ModifiedBy = id;
        post.Date ??= DateTime.UtcNow.ToString("yyyy-MM-dd");
        post.Time ??= DateTime.UtcNow.ToString("hh:mm tt");

        // If the user's Posts property is null, initialize it
        user.Posts ??= new List<Post>();

        // Add the new post to the user's Posts list
        user.Posts.Add(post);

        // Add the new post to the Posts DbSet
        await _postRepositories.AddPost(post);
        

        // Fetch the posts of the user and map them to GetPostDto objects
        List<Post> posatList = await _postRepositories.GetPostsByUserId(user.Id);
        response.Data = posatList.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
    }
    catch (Exception e)
    {
        // If an exception occurs, return an error response
        response.Success = false;
        response.Message = "An error occurred while adding the post: " + e.Message;
    }

    // Return the response
    return response;
    }

    public async Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto entity)
    {
        var serviceResponse = new ServiceResponse<GetPostDto>();
        try
        {
            
            Post post =   await  _postRepositories.GetPostById(entity.Id);

            if (post is null)
                throw new Exception($"Post with Id '{entity.Id}' not found.");

            post.Name = entity.Name;
            post.Description = entity.Description;
            post.ImageUrl = entity.ImageUrl;
            post.CategoryId = entity.CategoryId;
            post.Location = entity.Location;
            post.City = entity.City;
            post.Date = entity.Date;
            post.Time = entity.Time;
            post.DateModified = DateTime.UtcNow;
     //       post.ModifiedBy = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      
            await _postRepositories.UpdatePost(post);
            
            serviceResponse.Data = _mapper.Map<GetPostDto>(post);
        }
        catch (Exception e)
        {
            // If an exception occurs, return an error response
            serviceResponse.Success = false;
            serviceResponse.Message = "An error occurred while adding the post: " + e.Message;
        }
    
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPostDto>>> DeletePost(string id, string userId)
    {
        var  serviceResponse= new ServiceResponse<List<GetPostDto>>();
        try
        {
            Post post = await _postRepositories.GetPostById(id);
        if (post is null)
            throw new Exception($"Post with Id '{id}' not found.");

          List<Post> postList = await _postRepositories.DeletePost(post, userId);
             serviceResponse.Data = postList.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return serviceResponse;
    }
}