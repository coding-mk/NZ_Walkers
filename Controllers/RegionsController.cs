using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers;


// https://localhost:port/api/regions
[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
  private readonly IRegionRepository _regionRepository;

  private readonly IMapper _mapper;

  public RegionsController(IRegionRepository regionRepository, IMapper mapper)
  {
    this._regionRepository = regionRepository;
    this._mapper = mapper;
  }

  // GET All Regions
  // GET : https://localhost:port/api/regions
  [HttpGet]
  public async Task<IActionResult> GetAllRegions()
  {
      // Get Data from database - Domain models
      var regionDomain = await _regionRepository.GetAllRegionsAsync();

      //Map Domain models to DTOs
      // var regionDto = new List<RegionDto>();
      // foreach(var region in regionDomain)
      // {
      //   regionDto.Add(new RegionDto()
      //   {
      //     Id = region.Id,
      //     Code = region.Code,
      //     Name = region.Name,
      //     RegionImageUrl = region.RegionImageUrl
      //   });
      // }

      // Return DTOs back to client
      return Ok(_mapper.Map<List<RegionDto>>(regionDomain));
  }

  // GET All Regions
  // GET : https://localhost:port/api/regions/{id}
  [HttpGet]
  [Route("{id:Guid}")]
  public async Task<IActionResult> GetByRegionsId([FromRoute] Guid id)
  {
      var regionDomain = await _regionRepository.GetByRegionsIdAsync(id);

      // Get Region Domain Model from Database
      if(regionDomain == null)
      {
        return NotFound();
      }

      //Map/Convert Region Domain Model to region DTO
      // var regionDto = new RegionDto
      // {
      //   Id = regionDomain.Id,
      //   Code = regionDomain.Code,
      //   Name = regionDomain.Name,
      //   RegionImageUrl = regionDomain.RegionImageUrl
      // };
      
      // Return DTOs back to client
      return Ok(_mapper.Map<RegionDto>(regionDomain));
  }

  // POST to Create New Region
  // POST : http://localhost:port/api/regions
  [HttpPost]
  public async Task<IActionResult> CreateNewRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
  {
    // Map or Convert DTO to Domain Model
    // var regionDomainModel = new Region
    // {
    //   Code = addRegionRequestDto.Code,
    //   Name = addRegionRequestDto.Name,
    //   RegionImageUrl = addRegionRequestDto.RegionImageUrl
    // };
    var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

    // Use Domain Model to create region
    regionDomainModel = await _regionRepository.CreateNewRegionAsync(regionDomainModel);

    //Map Domain model back to Region
    // var regionDto = new RegionDto
    // {
    //   Id = regionDomainModel.Id,
    //   Code = regionDomainModel.Code,
    //   Name = regionDomainModel.Name,
    //   RegionImageUrl = regionDomainModel.RegionImageUrl
    // };
    var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

    return CreatedAtAction(nameof(CreateNewRegion), new { id = regionDomainModel.Id}, regionDto);
  }

  // Update region
  // PUT: https//localhost:port/api/regions/{id}
  [HttpPut]
  [Route("{id:Guid}")]
  public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
  {
    //Map DTO to Domain model
    // var regionDomainModel = new Region{
    //   Code = updateRegionRequestDto.Code,
    //   Name = updateRegionRequestDto.Name,
    //   RegionImageUrl = updateRegionRequestDto.RegionImageUrl
    // };
    var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
    
    //Check if region exists
    regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

    if(regionDomainModel == null)
    {
      return NotFound();
    }

    //Convert Domain Model to DTO
    // var regionDto = new RegionDto
    // {
    //   Id = regionDomainModel.Id,
    //   Code = regionDomainModel.Code,
    //   Name = regionDomainModel.Name,
    //   RegionImageUrl = regionDomainModel.RegionImageUrl
    // };

    return Ok(_mapper.Map<RegionDto>(regionDomainModel));
  }

  // Delete Region
  // DELETE: https://localhost:port/api/region/{id}
  [HttpDelete]
  [Route("{id:Guid}")]
  public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
  {
    // Check if region exists
    var regionDomainModel = await _regionRepository.DeleteRegionAsync(id);
    
    if(regionDomainModel == null)
    {
      return NotFound();
    }

    // return deleted Region back
    // map Domain Model to DTO
    // var regionDto = new RegionDto
    // {
    //   Id = regionDomainModel.Id,
    //   Code = regionDomainModel.Code,
    //   Name = regionDomainModel.Name,
    //   RegionImageUrl = regionDomainModel.RegionImageUrl
    // };

    return Ok(_mapper.Map<RegionDto>(regionDomainModel));
  }
}
