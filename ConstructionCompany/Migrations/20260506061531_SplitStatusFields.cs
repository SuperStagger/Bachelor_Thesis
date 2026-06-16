using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class SplitStatusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RealEstateObjects",
                newName: "SaleStatus");

            migrationBuilder.AddColumn<string>(
                name: "BuildingStatus",
                table: "RealEstateObjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "RealEstateObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$iRzSE7zQZAz75XGrduHbveFvjQbQ3QOayB9Ti80k1KyJ57yLJwbby");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingStatus",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "RealEstateObjects");

            migrationBuilder.RenameColumn(
                name: "SaleStatus",
                table: "RealEstateObjects",
                newName: "Status");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$G.TJHArHwENP7L0HSOACxe1zqHRubzSTA3sNAPe1e7vpJ8NmdQ6wO");
        }
    }
}
