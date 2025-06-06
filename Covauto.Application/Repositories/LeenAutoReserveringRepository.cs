using Covauto.Application.Interfaces;
using Covauto.Domain.Data;
using Covauto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Covauto.Domain
{
    public class LeenAutoReserveringRepository : ILeenAutoReserveringRepository
    {
        private readonly AppDbContext _context;

        public LeenAutoReserveringRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<LeenAutoReservering>> GetAllAsync() =>
            await _context.LeenAutoReserveringen.ToListAsync();

        public async Task<LeenAutoReservering?> GetByIdAsync(int id) =>
            await _context.LeenAutoReserveringen.FindAsync(id);

        public async Task<LeenAutoReservering> AddAsync(LeenAutoReservering reservering)
        {
            _context.LeenAutoReserveringen.Add(reservering);
            await _context.SaveChangesAsync();
            return reservering;
        }

        public async Task<bool> UpdateAsync(int id, LeenAutoReservering updated)
        {
            var existing = await _context.LeenAutoReserveringen.FindAsync(id);
            if (existing == null) return false;

            existing.AutoId = updated.AutoId;
            existing.WerknemerId = updated.WerknemerId;
            existing.StartDatum = updated.StartDatum;
            existing.EindDatum = updated.EindDatum;
            existing.RitVoltooid = updated.RitVoltooid;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.LeenAutoReserveringen.FindAsync(id);
            if (entity == null) return false;

            _context.LeenAutoReserveringen.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}