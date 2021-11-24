using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class HotelsConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
              
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
    }
}
