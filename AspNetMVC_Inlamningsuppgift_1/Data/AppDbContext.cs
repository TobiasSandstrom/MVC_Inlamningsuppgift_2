using AspNetMVC_Inlamningsuppgift_2.Entities;
using AspNetMVC_Inlamningsuppgift_2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_Inlamningsuppgift_2.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<UserProfileEntity> UserProfiles { get; set; }
        public virtual DbSet<ProfilePictureEntity> ProfilePictures { get; set; }
    }
}
