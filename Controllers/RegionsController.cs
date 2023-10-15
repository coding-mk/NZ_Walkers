using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers;


// https://localhost:port/api/regions
[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{

  private readonly NZWalksDbContext _dbContext;

  public RegionsController(NZWalksDbContext dbContext)
  {
    this._dbContext = dbContext;
  }

  // GET All Regions
  // GET : https://localhost:port/api/regions
  [HttpGet]
  public IActionResult GetAllRegions()
  {
      // Get Data from database - Domain models
      var regionDomain = _dbContext.Regions.ToList();

      //Map Domain models to DTOs
      var regionDto = new List<RegionDto>();
      foreach(var region in regionDomain)
      {
        regionDto.Add(new RegionDto()
        {
          Id = region.Id,
          Code = region.Code,
          Name = region.Name,
          RegionImageUrl = region.RegionImageUrl
        });
      }

      // Return DTOs back to client
      return Ok(regionDto);
  }

  // GET All Regions
  // GET : https://localhost:port/api/regions/{id}
  [HttpGet]
  [Route("{id:Guid}")]
  public IActionResult GetByRegionsId([FromRoute] Guid id)
  {
      var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

      // Get Region Domain Model from Database
      if(regionDomain == null)
      {
        return NotFound();
      }

      //Map/Convert Region Domain Model to region DTO
      var regionDto = new RegionDto
      {
        Id = regionDomain.Id,
        Code = regionDomain.Code,
        Name = regionDomain.Name,
        RegionImageUrl = regionDomain.RegionImageUrl
      };
      
      // Return DTOs back to client
      return Ok(regionDto);
  }

  // POST to Create New Region
  // POST : http://localhost:port/api/regions
  [HttpPost]
  public IActionResult CreateNewRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
  {
    // Map or Convert DTO to Domain Model
    var regionDomainModel = new Region
    {
      Code = addRegionRequestDto.Code,
      Name = addRegionRequestDto.Name,
      RegionImageUrl = addRegionRequestDto.RegionImageUrl
    };

    // Use Domain Model to create region
    _dbContext.Regions.Add(regionDomainModel);
    _dbContext.SaveChanges();

    //Map Domain model back to Region
    var regionDto = new RegionDto
    {
      Id = regionDomainModel.Id,
      Code = regionDomainModel.Code,
      Name = regionDomainModel.Name,
      RegionImageUrl = regionDomainModel.RegionImageUrl
    };

    return CreatedAtAction(nameof(CreateNewRegion), new { id = regionDomainModel.Id}, regionDto);
  }

  // Update region
  // PUT: https//localhost:port/api/regions/{id}
  [HttpPut]
  [Route("{id:Guid}")]
  public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
  {
    // Check if region exists
    var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
    
    if(regionDomainModel == null)
    {
      return NotFound();
    }

    //Map DTO to Domain model
    regionDomainModel.Code = updateRegionRequestDto.Code;
    regionDomainModel.Name = updateRegionRequestDto.Name;
    regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

    _dbContext.SaveChanges();

    //Convert Domain Model to DTO
    var regionDto = new RegionDto
    {
      Id = regionDomainModel.Id,
      Code = regionDomainModel.Code,
      Name = regionDomainModel.Name,
      RegionImageUrl = regionDomainModel.RegionImageUrl
    };

    return Ok(regionDto);
  }

  // Delete Region
  // DELETE: https://localhost:port/api/region/{id}
  [HttpDelete]
  [Route("{id:Guid}")]
  public IActionResult DeleteRegion([FromRoute] Guid id)
  {
    // Check if region exists
    var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
    
    if(regionDomainModel == null)
    {
      return NotFound();
    }

    _dbContext.Regions.Remove(regionDomainModel);
    _dbContext.SaveChanges();

    // return deleted Region back
    // map Domain Model to DTO
    var regionDto = new RegionDto
    {
      Id = regionDomainModel.Id,
      Code = regionDomainModel.Code,
      Name = regionDomainModel.Name,
      RegionImageUrl = regionDomainModel.RegionImageUrl
    };

    return Ok(regionDto);
  }
}
