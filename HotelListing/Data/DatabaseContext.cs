using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options): base (options){}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    ID = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    ID = 2,
                    Name = "United States",
                    ShortName = "US"
                },
                new Country
                {
                   ID=3,
                   Name="Biafra",
                   ShortName="BF"
                }
                ) ;
            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                  ID=1,
                  Name= "Sandals Resort and Spa",
                  Address="Negril",
                  CountryId=1,
                  Rating=4.5
                },
                new Hotel 
                {
                    ID = 2,
                    Name = "Grand Palldium",
                    Address = "Nassau",
                    CountryId = 2,
                    Rating = 4.0
                },
                new Hotel
                {
                    ID = 3,
                    Name = "Marriott",
                    Address = "Enugu",
                    CountryId = 3,
                    Rating = 4.5
                }



                );
        }
        public DbSet<Country>Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}
