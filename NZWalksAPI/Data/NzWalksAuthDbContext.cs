using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data;

public class NzWalksAuthDbContext: IdentityDbContext
{
  public NzWalksAuthDbContext(DbContextOptions<NzWalksAuthDbContext> options): base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    var readerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
    var writerRoleId = "c309fa92-2123-47be-b379-a1c77abd502c";


    var roles = new List<IdentityRole>
    {
      new IdentityRole
      {
        Id = readerRoleId,
        ConcurrencyStamp = readerRoleId,
        Name = "Reader",
        NormalizedName = "Reader".ToUpper()
      },
      new IdentityRole
      {
        Id = writerRoleId,
        ConcurrencyStamp = writerRoleId,
        Name = "Writer",
        NormalizedName = "Writer".ToUpper()
      }
    };

    modelBuilder.Entity<IdentityRole>().HasData(roles);
  }
}