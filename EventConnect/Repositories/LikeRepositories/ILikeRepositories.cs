using EventConnect.Dtos.Likes;
using EventConnect.Entities.Like;
using EventConnect.Models.ServiceResponce;
using EventConnect.Repositories.GenericRepositories;
using Microsoft.AspNetCore.Mvc;

namespace EventConnect.Repositories.LikeRepositories;

public interface ILikeRepositories:IGenericRepositories<Like>
{
  //get Like bu user and post id  
  Task<Like> GetLikeByPostIdAndUserId(string postId, string userId);
  Task<List<Like>> GetAllLikeByPostId(string postId);
  
}