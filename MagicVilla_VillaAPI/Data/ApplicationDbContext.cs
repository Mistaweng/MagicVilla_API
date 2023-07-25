using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Connecting to the database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }


        //Seeding data to the database (i.e writing data into the database)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Welcome to the villa",
                    ImageUrl = "https://images.app.goo.gl/nTF5fSMMYdDhF3oZ6",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 500,
                    Amenity = ""
                });
        }
    }
}
