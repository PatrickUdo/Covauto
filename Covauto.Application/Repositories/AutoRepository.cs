using Covauto.Application.Interfaces;
using Covauto.Domain.Data;
using Covauto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Covauto.Domain
{
    public class AutoRepository : IAutoRepository
    {
        private readonly AppDbContext _context;

        public AutoRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Auto>> GetAllAsync() =>
            await _context.Autos.ToListAsync();

        public async Task<Auto?> GetByIdAsync(int id) =>
            await _context.Autos.FindAsync(id);

        public async Task<Auto> AddAsync(Auto auto)
        {
            _context.Autos.Add(auto);
            await _context.SaveChangesAsync();
            return auto;
        }

        public async Task<bool> UpdateAsync(int id, Auto updatedAuto)
        {
            var existing = await _context.Autos.FindAsync(id);
            if (existing == null) return false;

            existing.Kenteken = updatedAuto.Kenteken;
            existing.Kilometerstand = updatedAuto.Kilometerstand;
            existing.Naam = updatedAuto.Naam;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var auto = await _context.Autos.FindAsync(id);
            if (auto == null) return false;

            _context.Autos.Remove(auto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}