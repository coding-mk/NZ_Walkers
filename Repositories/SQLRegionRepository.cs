
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Repositories;

public class SQLRegionRepository : IRegionRepository
{

  private readonly NZWalksDbContext _dbContext;

  public SQLRegionRepository(NZWalksDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

    public async Task<Region> CreateNewRegionAsync(Region region)
    {
      await _dbContext.Regions.AddAsync(region);
      await _dbContext.SaveChangesAsync();
      
      return region;
    }

    public async Task<Region?> DeleteRegionAsync(Guid id)
    {
      var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    
      if(regionDomainModel == null)
      {
        return null;
      }

      _dbContext.Regions.Remove(regionDomainModel);
      await _dbContext.SaveChangesAsync();

      return regionDomainModel;
    }

    public async Task<List<Region>> GetAllRegionsAsync()
    {
      return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region> GetByRegionsIdAsync(Guid id)
    {
      return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
    {
      var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

      if(existingRegion == null)
      {
        return null;
      }

      existingRegion.Code = region.Code;
      existingRegion.Name = region.Name;
      existingRegion.RegionImageUrl = region.RegionImageUrl;

      await _dbContext.SaveChangesAsync();
      
      return existingRegion;
    }
}