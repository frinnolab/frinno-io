using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace frinno_infrastructure.Migrations
{
    public partial class updated_address_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "AspNetUsers");
        }
    }
}
