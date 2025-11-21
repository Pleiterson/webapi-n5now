using Microsoft.EntityFrameworkCore;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly AppDbContext _context;

        public PermissionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PermissionType> AddAsync(PermissionType entity)
        {
            _context.PermissionTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<PermissionType>> GetAllAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionType?> GetByIdAsync(int id)
        {
            return await _context.PermissionTypes.FindAsync(id);
        }

        public async Task UpdateAsync(PermissionType entity)
        {
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PermissionType entity)
        {
            _context.PermissionTypes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
