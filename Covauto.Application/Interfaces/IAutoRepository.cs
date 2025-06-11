using Covauto.Shared.DTOs;

public interface IAutoRepository
{
    Task<IEnumerable<AutoDTO>> GetAllAsync();
    Task<AutoDTO?> GetByIdAsync(int id);
    Task<AutoDTO> AddAsync(AutoDTO auto);
    Task<bool> UpdateAsync(int id, AutoDTO updatedAuto);
    Task<bool> DeleteAsync(int id);
}