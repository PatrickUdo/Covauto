using Covauto.Domain.Entities;

namespace Covauto.Application.Interfaces
{
    public interface ILeenAutoRitRepository
    {
        Task<IEnumerable<LeenAutoRit>> GetAllAsync();
        Task<LeenAutoRit?> GetByIdAsync(int id);
        Task<LeenAutoRit> AddAsync(LeenAutoRit rit);
        Task<bool> UpdateAsync(int id, LeenAutoRit updatedRit);
        Task<bool> DeleteAsync(int id);
    }
}