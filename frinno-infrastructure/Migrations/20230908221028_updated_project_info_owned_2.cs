using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace frinno_infrastructure.Migrations
{
    public partial class updated_project_info_owned_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyAgencyPublicLink",
                table: "Projects",
                newName: "CompanyAgencyInfo_CompanyAgencyPublicLink");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyName",
                table: "Projects",
                newName: "CompanyAgencyInfo_CompanyAgencyName");

            migrationBuilder.RenameColumn(
                name: "ClientPublicLink",
                table: "Projects",
                newName: "ClientInfo_ClientPublicLink");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Projects",
                newName: "ClientInfo_ClientName");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyAddress_Mobile",
                table: "Projects",
                newName: "CompanyAgencyInfo_CompanyAgencyMobile");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyAddress_Country",
                table: "Projects",
                newName: "CompanyAgencyInfo_CompanyAgencyCountry");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyAddress_City",
                table: "Projects",
                newName: "CompanyAgencyInfo_CompanyAgencyCity");

            migrationBuilder.RenameColumn(
                name: "ClientAddress_Mobile",
                table: "Projects",
                newName: "ClientInfo_ClientMobile");

            migrationBuilder.RenameColumn(
                name: "ClientAddress_Country",
                table: "Projects",
                newName: "ClientInfo_ClientCountry");

            migrationBuilder.RenameColumn(
                name: "ClientAddress_City",
                table: "Projects",
                newName: "ClientInfo_ClientCity");

            migrationBuilder.AddColumn<int>(
                name: "ProjectType",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectType",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyInfo_CompanyAgencyPublicLink",
                table: "Projects",
                newName: "CompanyAgencyPublicLink");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyInfo_CompanyAgencyName",
                table: "Projects",
                newName: "CompanyAgencyName");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_ClientPublicLink",
                table: "Projects",
                newName: "ClientPublicLink");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_ClientName",
                table: "Projects",
                newName: "ClientName");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyInfo_CompanyAgencyMobile",
                table: "Projects",
                newName: "CompanyAgencyAddress_Mobile");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyInfo_CompanyAgencyCountry",
                table: "Projects",
                newName: "CompanyAgencyAddress_Country");

            migrationBuilder.RenameColumn(
                name: "CompanyAgencyInfo_CompanyAgencyCity",
                table: "Projects",
                newName: "CompanyAgencyAddress_City");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_ClientMobile",
                table: "Projects",
                newName: "ClientAddress_Mobile");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_ClientCountry",
                table: "Projects",
                newName: "ClientAddress_Country");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_ClientCity",
                table: "Projects",
                newName: "ClientAddress_City");
        }
    }
}
