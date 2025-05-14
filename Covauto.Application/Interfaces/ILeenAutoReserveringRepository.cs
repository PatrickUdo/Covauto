using Covauto.Domain.Entities;

namespace Covauto.Application.Interfaces
{
    public interface ILeenAutoReserveringRepository
    {
        Task<IEnumerable<LeenAutoReservering>> GetAllAsync();
        Task<LeenAutoReservering?> GetByIdAsync(int id);
        Task<LeenAutoReservering> AddAsync(LeenAutoReservering reservering);
        Task<bool> UpdateAsync(int id, LeenAutoReservering updatedReservering);
        Task<bool> DeleteAsync(int id);
    }
}