using EventConnect.Data;
using EventConnect.Models.ServiceResponce;
using Microsoft.EntityFrameworkCore;

namespace EventConnect.Repositories.GenericRepositories;

public class GenericRepositories<T> : IGenericRepositories<T> where T : class
{

    private readonly AppDbContext _context;

    public GenericRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?>UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> DeleteAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T?>> GetAllAsync()
    {
        var data = await _context.Set<T>().AsNoTracking().ToListAsync();
        return data;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        return entity;
    }
}
