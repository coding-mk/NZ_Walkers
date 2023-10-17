using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories;

public interface IImageRepository
{
  Task<Image> UploadImageAsync(Image image);
}