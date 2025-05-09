using Covauto.Application.Interfaces;
using Covauto.Domain.Data;
using Covauto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Covauto.Domain
{
    public class LeenAutoRitRepository : ILeenAutoRitRepository
    {
        private readonly AppDbContext _context;

        public LeenAutoRitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeenAutoRit>> GetAllAsync()
        {
            return await _context.LeenAutoRitten.ToListAsync();
        }

        public async Task<LeenAutoRit?> GetByIdAsync(int id)
        {
            return await _context.LeenAutoRitten.FindAsync(id);
        }

        public async Task<LeenAutoRit> AddAsync(LeenAutoRit rit)
        {
            _context.LeenAutoRitten.Add(rit);
            await _context.SaveChangesAsync();
            return rit;
        }

        public async Task<bool> UpdateAsync(int id, LeenAutoRit updatedRit)
        {
            var existing = await _context.LeenAutoRitten.FindAsync(id);
            if (existing == null) return false;

            existing.WerknemerId = updatedRit.WerknemerId;
            existing.AutoId = updatedRit.AutoId;
            existing.VanAdres = updatedRit.VanAdres;
            existing.NaarAdres = updatedRit.NaarAdres;
            existing.VertrekTijd = updatedRit.VertrekTijd;
            existing.AankomstTijd = updatedRit.AankomstTijd;
            existing.KilometerstandBegin = updatedRit.KilometerstandBegin;
            existing.KilometerstandEind = updatedRit.KilometerstandEind;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rit = await _context.LeenAutoRitten.FindAsync(id);
            if (rit == null) return false;

            _context.LeenAutoRitten.Remove(rit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}