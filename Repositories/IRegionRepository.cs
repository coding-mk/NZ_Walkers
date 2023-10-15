using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories;

public interface IRegionRepository
{
  Task<List<Region>> GetAllRegionsAsync();

  Task<Region> GetByRegionsIdAsync(Guid id);

  Task<Region> CreateNewRegionAsync(Region region);

  Task<Region?> UpdateRegionAsync(Guid id, Region region);

  Task<Region?> DeleteRegionAsync(Guid id);
}