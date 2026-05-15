using Cwiczenia7.DTOs;

namespace Cwiczenia7.Services;

public interface IPcService
{
    Task<List<PcDto>> GetAllAsync();

    Task<PcComponentsDto> GetComponentsAsync(int id);

    Task<PcDto> CreateAsync(CreatePcDto dto);

    Task<PcDto> UpdateAsync(int id, CreatePcDto dto);

    Task DeleteAsync(int id);
}