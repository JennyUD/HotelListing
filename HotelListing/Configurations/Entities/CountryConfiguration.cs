using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
       
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
                    ID = 3,
                    Name = "Biafra",
                    ShortName = "BF"
                }
                );
                    
        }
    }
}
