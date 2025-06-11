using Covauto.Application.Interfaces;
using Covauto.Domain.Data;
using Covauto.Domain.Entities;
using Covauto.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Covauto.Domain
{
    public class LeenAutoReserveringRepository : ILeenAutoReserveringRepository
    {
        private readonly AppDbContext _context;

        public LeenAutoReserveringRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<LeenAutoReserveringDTO>> GetAllAsync() =>
            await _context.LeenAutoReserveringen
                .Select(r => new LeenAutoReserveringDTO
                {
                    Id = r.Id,
                    AutoId = r.AutoId,
                    WerknemerId = r.WerknemerId,
                    StartDatum = r.StartDatum,
                    EindDatum = r.EindDatum,
                    RitVoltooid = r.RitVoltooid,
                    BeginAdres = r.BeginAdres,
                    EindAdres = r.EindAdres
                })
                .ToListAsync();

        public async Task<LeenAutoReserveringDTO?> GetByIdAsync(int id)
        {
            var reservering = await _context.LeenAutoReserveringen.FindAsync(id);
            return reservering == null ? null : new LeenAutoReserveringDTO
            {
                Id = reservering.Id,
                AutoId = reservering.AutoId,
                WerknemerId = reservering.WerknemerId,
                StartDatum = reservering.StartDatum,
                EindDatum = reservering.EindDatum,
                RitVoltooid = reservering.RitVoltooid,
                BeginAdres = reservering.BeginAdres,
                EindAdres = reservering.EindAdres
            };
        }

        public async Task<LeenAutoReserveringDTO> AddAsync(LeenAutoReserveringDTO reserveringDto)
        {
            var reservering = new LeenAutoReservering
            {
                AutoId = reserveringDto.AutoId,
                WerknemerId = reserveringDto.WerknemerId,
                StartDatum = reserveringDto.StartDatum,
                EindDatum = reserveringDto.EindDatum,
                RitVoltooid = reserveringDto.RitVoltooid,
                BeginAdres = reserveringDto.BeginAdres,
                EindAdres = reserveringDto.EindAdres
            };

            _context.LeenAutoReserveringen.Add(reservering);
            await _context.SaveChangesAsync();

            return new LeenAutoReserveringDTO
            {
                Id = reservering.Id,
                AutoId = reservering.AutoId,
                WerknemerId = reservering.WerknemerId,
                StartDatum = reservering.StartDatum,
                EindDatum = reservering.EindDatum,
                RitVoltooid = reservering.RitVoltooid,
                BeginAdres = reservering.BeginAdres,
                EindAdres = reservering.EindAdres
            };
        }

        public async Task<bool> UpdateAsync(int id, LeenAutoReserveringDTO updatedDto)
        {
            var existing = await _context.LeenAutoReserveringen.FindAsync(id);
            if (existing == null) return false;

            existing.AutoId = updatedDto.AutoId;
            existing.WerknemerId = updatedDto.WerknemerId;
            existing.StartDatum = updatedDto.StartDatum;
            existing.EindDatum = updatedDto.EindDatum;
            existing.RitVoltooid = updatedDto.RitVoltooid;
            existing.BeginAdres = updatedDto.BeginAdres;
            existing.EindAdres = updatedDto.EindAdres;

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
