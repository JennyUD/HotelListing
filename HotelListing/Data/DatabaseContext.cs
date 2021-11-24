using HotelListing.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HotelListing.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new HotelsConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            base.OnModelCreating(builder);

            //base.OnModelCreating(builder);
            //builder.Entity<Country>().HasData
            // (

            //     new Country
            //     {
            //         ID = 1,
            //         Name = "Jamaica",
            //         ShortName = "JM"
            //     },
            //      new Country
            //      {
            //          ID = 2,
            //          Name = "United States",
            //          ShortName = "US"
            //      },
            //    new Country
            //    {
            //        ID = 3,
            //        Name = "Biafra",
            //        ShortName = "BF"
            //    }

            //);



        }
        public DbSet<Country>Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}
