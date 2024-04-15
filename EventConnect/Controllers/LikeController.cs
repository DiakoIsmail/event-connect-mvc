using System.Security.Claims;
using EventConnect.Data;
using EventConnect.Data.Auth;
using EventConnect.Dtos.Likes;
using EventConnect.Entities;
using EventConnect.Entities.Like;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;
using EventConnect.Services.Likes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventConnect.Controllers;
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LikeController:ControllerBase
{
    private readonly ILikeServices _likeServices;

    

    public LikeController(ILikeServices likeServices)
    {
        _likeServices = likeServices;
 
    }

    // crate Like  
    [HttpPost("CreateLike/{email}")]
    public async Task<ActionResult<ServiceResponse<Like>>> CreateLike(PostLikeDto entity)
    {
          return Ok(await _likeServices.CreateLike(entity));

    }
    
 
    //Delete Like
    [HttpDelete("DeleteLike/{likeId}")]
    public async Task<ActionResult<ServiceResponse<Like>>> DeleteLike(string likeId)
    {
        return Ok(await _likeServices.DeleteLike(likeId));
    }
 
    //Get All Likes
    [HttpGet("GetAllLikes")]
    public async Task<ActionResult<ServiceResponse<List<Like>>> >GetAllLikes(string postId)
    {
        return Ok(await _likeServices.GetAllLikesByPostId(postId));
    }
    
    [HttpGet("GetAllInterestedUsers/{postId}")]
    public async Task<ActionResult<ServiceResponse<List<Like>>> >GetAllInterestedUsers(string postId)
    {
        return Ok(await _likeServices.GetAllLikesUsersByPostId(postId));
    }
    //Get  interested User by like 
    
}