using Covauto.Shared.DTOs;

namespace Covauto.Application.Interfaces
{
    public interface ILeenAutoReserveringRepository
    {
        Task<IEnumerable<LeenAutoReserveringDTO>> GetAllAsync();
        Task<LeenAutoReserveringDTO?> GetByIdAsync(int id);
        Task<LeenAutoReserveringDTO> AddAsync(LeenAutoReserveringDTO reservering);
        Task<bool> UpdateAsync(int id, LeenAutoReserveringDTO updatedReservering);
        Task<bool> DeleteAsync(int id);
    }
}