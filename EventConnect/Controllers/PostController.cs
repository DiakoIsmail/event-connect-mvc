using System.Security.Claims;
using EventConnect.Dtos.Posts;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;
using EventConnect.Services.PostServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventConnect.Controllers;
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PostController:ControllerBase
{
    private readonly IPostServices _postServices;

    public PostController(IPostServices postServices)
    {
        _postServices = postServices;
    }
    
    // crate Post  
    [HttpPost("CreatePost/{email}")]
    public async Task<ActionResult<ServiceResponse<List<Post>>>> CreatePost(AddPostDto entity, string email)
    {
        return Ok(await _postServices.AddPost(entity, email));
    }
    
    // Get All Posts
    [HttpGet("GetAllPosts")]
    public async Task<ActionResult<ServiceResponse<List<GetPostDto>>>> GetAllPosts()
    {
        return Ok(await _postServices.GetAllPosts());
    }
    
    //get post by id
    [HttpGet("GetPostById/{id}")]
    public async Task<ActionResult<ServiceResponse<GetPostDto>>>GetPostById(string id)
    {
        return Ok(await _postServices.GetPostById(id));
    }
    
    //update post
    [HttpPut("UpdatePost")]
    public async Task<ActionResult<ServiceResponse<GetPostDto>>> UpdatePost(UpdatePostDto entity)
    {
        return Ok(await _postServices.UpdatePost(entity));
    }
    
    //Delete Post
    [HttpDelete("DeletePost/{id}/{userId}")]
    public async Task<ActionResult<ServiceResponse<List<GetPostDto>>>> DeletePost(string id, string userId)
    {
        return Ok(await _postServices.DeletePost(id, userId));
    }
    
}