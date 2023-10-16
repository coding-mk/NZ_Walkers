using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories;

public interface IWalkRepository
{
  Task<List<Walk>> GetAllWalkAsync(string? filterOn=null, string? filterQuery = null,
   string? sortBy=null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);
  Task<Walk?> GetWalkByIdAsync(Guid id);
  Task<Walk> CreateAsync(Walk walk);
  Task<Walk?> UpdateWalkAsync(Guid id,Walk walk);
  Task<Walk?> DeleteWalkAsync(Guid id);
}