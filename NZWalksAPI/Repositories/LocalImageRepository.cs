using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories;

public class LocalImageRepository : IImageRepository
{
  private readonly IWebHostEnvironment _webHostEnvironment;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly NZWalksDbContext _dbContext;

  public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor,
    NZWalksDbContext dbContext)
  {
    this._webHostEnvironment = webHostEnvironment;
    this._httpContextAccessor = httpContextAccessor;
    this._dbContext = dbContext;
  }

    public async Task<Image> UploadImageAsync(Image image)
    {
      var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
      $"{image.FileName}{image.FileExtension}");

      // Upload the image to the local path
      using var stream = new FileStream(localFilePath, FileMode.Create);
      await image.File.CopyToAsync(stream);

      // https://localhost:7174/images/imsge.jpg

      var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

      image.FilePath = urlFilePath;

      // Add Image to the Image table
      await _dbContext.Images.AddAsync(image);
      await _dbContext.SaveChangesAsync();

      return image;
    }
}