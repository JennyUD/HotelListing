using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name", "ShortName" },
                values: new object[] { 1, "Jamaica", "JM" });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name", "ShortName" },
                values: new object[] { 2, "United States", "US" });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name", "ShortName" },
                values: new object[] { 3, "Biafra", "BF" });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "ID", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 1, "Negril", 1, "Sandals Resort and Spa", 4.5 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "ID", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 2, "Nassau", 2, "Grand Palldium", 4.0 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "ID", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 3, "Enugu", 3, "Marriott", 4.5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
