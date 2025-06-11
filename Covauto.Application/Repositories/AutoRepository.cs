using Covauto.Domain.Data;
using Covauto.Domain.Entities;
using Covauto.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

public class AutoRepository : IAutoRepository
{
    private readonly AppDbContext _context;

    public AutoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<AutoDTO>> GetAllAsync() =>
        await _context.Autos
            .Select(a => new AutoDTO
            {
                Id = a.Id,
                Kenteken = a.Kenteken,
                Kilometerstand = a.Kilometerstand,
                Naam = a.Naam
            })
            .ToListAsync();

    public async Task<AutoDTO?> GetByIdAsync(int id)
    {
        var auto = await _context.Autos.FindAsync(id);
        return auto == null ? null : new AutoDTO
        {
            Id = auto.Id,
            Kenteken = auto.Kenteken,
            Kilometerstand = auto.Kilometerstand,
            Naam = auto.Naam
        };
    }

    public async Task<AutoDTO> AddAsync(AutoDTO autoDto)
    {
        var auto = new Auto
        {
            Kenteken = autoDto.Kenteken,
            Kilometerstand = autoDto.Kilometerstand,
            Naam = autoDto.Naam
        };

        _context.Autos.Add(auto);
        await _context.SaveChangesAsync();

        return new AutoDTO
        {
            Id = auto.Id,
            Kenteken = auto.Kenteken,
            Kilometerstand = auto.Kilometerstand,
            Naam = auto.Naam
        };
    }

    public async Task<bool> UpdateAsync(int id, AutoDTO updatedAutoDto)
    {
        var existing = await _context.Autos.FindAsync(id);
        if (existing == null) return false;

        existing.Kenteken = updatedAutoDto.Kenteken;
        existing.Kilometerstand = updatedAutoDto.Kilometerstand;
        existing.Naam = updatedAutoDto.Naam;

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