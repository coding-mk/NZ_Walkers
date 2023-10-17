using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Repositories;

public class SQLWalkRepository : IWalkRepository
{

  private readonly NZWalksDbContext _dbContext;
  
  public SQLWalkRepository(NZWalksDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  public async Task<List<Walk>> GetAllWalkAsync(string? filterOn=null, string? filterQuery = null,
   string? sortBy=null, bool isAscending = true,
    int pageNumber = 1, int pageSize = 100)
  {
    var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

    //Filtering
    if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
    {
      if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
      {
        walks = walks.Where(x => x.Name.Contains(filterQuery));
      }
    }

    //Sorting
    if(string.IsNullOrWhiteSpace(sortBy) == false)
    {
      if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
      {
        walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
      }else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
      {
        walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
      }
    }

    //Pagination
    var skipResults = (pageNumber -1) * pageSize;

    return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
    //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
  }

  public async Task<Walk?> GetWalkByIdAsync(Guid id)
  {
    return await _dbContext.Walks
    .Include("Difficulty")
    .Include("Region")
    .FirstOrDefaultAsync(x => x.Id == id);

  }

  public async Task<Walk> CreateAsync(Walk walk)
  {
     await _dbContext.Walks.AddAsync(walk);
     await _dbContext.SaveChangesAsync();
     return walk;
  }

  public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
  {
    var existingWalk = await _dbContext.Walks
    .FirstOrDefaultAsync(x => x.Id == id);

    if(existingWalk == null)
    {
      return null;
    }

    existingWalk.Name = walk.Name;
    existingWalk.Description = walk.Description;
    existingWalk.LengthInKm = walk.LengthInKm;
    existingWalk.WalkImageUrl = walk.WalkImageUrl;
    existingWalk.DifficultyId = walk.DifficultyId;
    existingWalk.RegionId = walk.RegionId;

    await _dbContext.SaveChangesAsync();

    return existingWalk;
  }

  public async Task<Walk?> DeleteWalkAsync(Guid id)
  {
    var existingWalk = await _dbContext.Walks
    .FirstOrDefaultAsync(x => x.Id == id);

    if(existingWalk == null)
    {
      return null;
    }

    _dbContext.Walks.Remove(existingWalk);
    await _dbContext.SaveChangesAsync();
    return existingWalk;
  }
}