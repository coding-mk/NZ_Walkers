using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{

  private readonly IImageRepository _imageRepository;

  public ImagesController(IImageRepository imageRepository)
  {
    this._imageRepository =imageRepository;
  }

  // Post: /api/Images/Upload
  [HttpPost]
  [Route("Upload")]
  public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto  request)
  {
    ValidateFileUpload(request);

    if(ModelState.IsValid)
    {
      //convert DTO to Domain model
      var imageDomainModel = new Image
      {
        File = request.File,
        FileExtension = Path.GetExtension(request.File.FileName),
        FileSizeInBytes = request.File.Length,
        FileName = request.FileName,
        FileDescription = request.FileDescription
      };
      //User repository to upload image
      await _imageRepository.UploadImageAsync(imageDomainModel);
      return Ok(imageDomainModel);
    }
    return BadRequest(ModelState);
  }

  private void ValidateFileUpload(ImageUploadRequestDto request)
  {
    var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png"};
    if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
    {
      ModelState.AddModelError("file", "UnSupported file extension");
    }

    if(request.File.Length > 10485760)
    {
      ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file");
    }
  }
}