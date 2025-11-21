using Microsoft.EntityFrameworkCore;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Permission> AddAsync(Permission entity)
        {
            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Permission entity)
        {
            _context.Permissions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity == null) return;
            _context.Permissions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions
                .Include(p => p.PermissionType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions
                .Include(p => p.PermissionType)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Permission entity)
        {
            _context.Permissions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
