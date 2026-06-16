using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceTypeAndProjectFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriceType",
                table: "RealEstateObjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProjectQuarter",
                table: "RealEstateObjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectYear",
                table: "RealEstateObjects",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$G.TJHArHwENP7L0HSOACxe1zqHRubzSTA3sNAPe1e7vpJ8NmdQ6wO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "ProjectQuarter",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "ProjectYear",
                table: "RealEstateObjects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$jaCJXi6Qc/ppAuDn/dV7xuyqNOAT0K0j68RWeKRVGoAC/mQvMMPDi");
        }
    }
}
