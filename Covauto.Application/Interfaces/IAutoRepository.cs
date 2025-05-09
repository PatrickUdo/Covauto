using Covauto.Domain.Entities;

namespace Covauto.Application.Interfaces
{
    public interface IAutoRepository
    {
        Task<IEnumerable<Auto>> GetAllAsync();
        Task<Auto?> GetByIdAsync(int id);
        Task<Auto> AddAsync(Auto auto);
        Task<bool> UpdateAsync(int id, Auto updatedAuto);
        Task<bool> DeleteAsync(int id);
    }
}