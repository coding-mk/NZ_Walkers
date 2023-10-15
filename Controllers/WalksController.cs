using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers;

//  https://localhost:port/api/walks
[ApiController]
[Route("api/[controller]")]
public class WalksController : ControllerBase
{
  private readonly IMapper _mapper;
  private readonly IWalkRepository _walkRepository;

  public WalksController(IMapper mapper, IWalkRepository walkRepository)
  {
    this._mapper = mapper;
    this._walkRepository = walkRepository;
  }

  // Create Walk
  // Post: /api/walks
  [HttpPost]
  public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
  {
    // Map DTO to Domain Model
    var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

    await _walkRepository.CreateAsync(walkDomainModel); 

    // Map Domain Model to DTO
    return Ok(_mapper.Map<WalkDto>(walkDomainModel));
  }

  // GET walks
  // GET: /api/walks
  [HttpGet]
  public async Task<IActionResult> GetAllWalks()
  {
    var walksDomainModel = await _walkRepository.GetAllWalkAsync();

    //Map Domain Model to DTO
    return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
  }

  // GET walk by Id
  // GET: /api/walks/{id}
  [HttpGet]
  [Route("{id:Guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid id)
  {
    var walkDomainModel = await _walkRepository.GetWalkByIdAsync(id);

    if(walkDomainModel == null)
    {
      return NotFound();
    }

    //Map Domain Model to Dto
    return Ok(_mapper.Map<WalkDto>(walkDomainModel));
  }

  //Update Walk By Id
  // PUT: /api/Walks/{id}
  [HttpPut]
  [Route("{id:Guid}")]
  public async Task<IActionResult> UpdateWalks([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
  {
    var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

    walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

    if(walkDomainModel == null){
        return NotFound();
    }
      //Map Domain Model to DTO
    return Ok(_mapper.Map<WalkDto>(walkDomainModel));
  }

  //DELETE Walk By Id
  // DELETE: /api/Walks/{id}
  [HttpDelete]
  [Route("{id:Guid}")]
  public async Task<IActionResult> DeleteWalks([FromRoute] Guid id)
  {
    var deleteWalkDomainModel = await _walkRepository.DeleteWalkAsync(id);

    if(deleteWalkDomainModel == null){
        return NotFound();
    }
    //Map Domain model to Dto
    return Ok(_mapper.Map<WalkDto>(deleteWalkDomainModel));
  }
}