using AutoMapper;
using EventConnect.Data;
using EventConnect.Data.Auth;
using EventConnect.Dtos.Posts;
using EventConnect.Entities;
using EventConnect.Entities.Post;
using EventConnect.Models.ServiceResponce;
using Microsoft.EntityFrameworkCore;

namespace EventConnect.Repositories.PostRepositories;

public class PostRepositories:IPostRepositories
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IAuthRepository _authRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostRepositories(IMapper mapper, AppDbContext context, IAuthRepository authRepo, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _authRepo = authRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Post>> GetAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post> GetPostById(string id)
    {
       return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Post>> GetPostsByUserId(string id)
    {
        return await  _context.Posts.
            Where(p => p.User.Id == id)
            .ToListAsync();
    }

    public async Task<Post> AddPost(Post entity)
{
   await _context.Posts.AddAsync(entity);
   await _context.SaveChangesAsync();
   return entity;
}
//----------------------------
    public async Task<Post>UpdatePost(Post entity)
    {
        _context.Posts.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }



    public async Task<List<Post>> DeletePost(Post post, string id)
    {
        _context.Posts.Remove(post);
        await  _context.SaveChangesAsync();

        return await _context.Posts
            .Where(c => c.User.Id == id).ToListAsync();
    }
}