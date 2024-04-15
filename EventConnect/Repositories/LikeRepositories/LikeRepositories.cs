using EventConnect.Data;
using EventConnect.Entities.Like;
using EventConnect.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventConnect.Repositories.LikeRepositories;

public class LikeRepositories: GenericRepositories<Like>,ILikeRepositories
{
    private readonly AppDbContext _context;
    public LikeRepositories(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Like> GetLikeByPostIdAndUserId(string postId, string userId)
    {
       return await _context.Likes.FirstOrDefaultAsync(q => q.PostId == postId && q.UserId == userId); 
    }

    public async Task<List<Like>> GetAllLikeByPostId(string postId)
    {
        return await _context.Likes.Where(q => q.PostId == postId).ToListAsync(); 
    }
}