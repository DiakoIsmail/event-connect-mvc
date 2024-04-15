

using EventConnect.Dtos.Likes;
using EventConnect.Entities;
using EventConnect.Entities.Like;
using EventConnect.Models.ServiceResponce;
using Microsoft.AspNetCore.Mvc;

namespace EventConnect.Services.Likes;

public interface ILikeServices
{
    public Task<ActionResult<ServiceResponse<Like>>> CreateLike(PostLikeDto entity);
    
    public Task<ActionResult<ServiceResponse<Like>>> DeleteLike(string likeId);
    public Task<ActionResult<ServiceResponse<List<Like>>>> GetAllLikesByPostId(string postId);
    
    public Task<ActionResult<ServiceResponse<List<User>>>> GetAllLikesUsersByPostId(string postId);
    
}