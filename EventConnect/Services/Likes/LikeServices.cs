
using EventConnect.Data;
using EventConnect.Data.Auth;
using EventConnect.Dtos.Likes;
using EventConnect.Models.ServiceResponce;
using EventConnect.Repositories.LikeRepositories;
using EventConnect.Repositories.PostRepositories;
using EventConnect.Services.Likes;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventConnect.Entities.Like;


public class LikeServices:  ILikeServices
{
   
    private readonly IAuthRepository authRepo;
    private readonly ILikeRepositories _likeRepositories;
    private readonly IPostRepositories _postRepositories;


    public LikeServices(IAuthRepository authRepo, ILikeRepositories likeRepositories, IPostRepositories postRepositories)
    {
        this.authRepo = authRepo;
        _likeRepositories = likeRepositories;
        _postRepositories = postRepositories;
    }

    public async Task<ActionResult<ServiceResponse<Like>>> CreateLike(PostLikeDto entity)
    {
        var serviceResponse = new ServiceResponse<Like>();
     
        try
        {
            //get user by id
            User user = await authRepo.GetUser(entity.email);
        
            if (user is null)
                throw new Exception($"User with email  '{entity.email}' not found.");
            //get post by id
            
            //cheek if lLike exist on this post
            Like likeExist = await _likeRepositories.GetLikeByPostIdAndUserId(entity.PostId, user.Id);
            if (likeExist  != null)
                throw new Exception("like already exist.");     
            
            Post.Post post = await  _postRepositories.GetPostById(entity.PostId);
            //create like
            if (post is null)
                throw new Exception($"User with Post  '{entity.PostId}' not found.");
        
            Like like = new Like
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                //  User = null,
                PostId = post.Id,
                // Post = null
                PostName = post.Name
            };
            
            //add like to db
            serviceResponse.Data = await _likeRepositories.CreateAsync(like);
            
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }

    public async Task<ActionResult<ServiceResponse<Like>>> DeleteLike(string likeId)
    {
        var serviceResponse = new ServiceResponse<Like>();
        try
        {
            Like like = await _likeRepositories.GetByIdAsync(likeId);
          
            if (like is null)
                throw new Exception($"Like with Id '{likeId}' not found.");
            await _likeRepositories.DeleteAsync(like);
            serviceResponse.Data = like;
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public  async Task<ActionResult<ServiceResponse<List<Like>>>> GetAllLikesByPostId(string postId)
    {
        var serviceResponse = new ServiceResponse<List<Like>>();
        try
        {
            serviceResponse.Data = await _likeRepositories.GetAllLikeByPostId(postId);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ActionResult<ServiceResponse<List<User>>>> GetAllLikesUsersByPostId(string postId)
    {
       var serviceResponse = new ServiceResponse<List<User>>();
       try
       {
           List<Like> likes= await _likeRepositories.GetAllLikeByPostId(postId);
           List<string > userIds = likes.Select(q => q.UserId).ToList();
           serviceResponse.Data =   await authRepo.GetAllUsersWithIds(userIds);
       }
       catch (Exception ex)
       {
           serviceResponse.Success = false;
           serviceResponse.Message = ex.Message;
       }

       return serviceResponse;
    }
}