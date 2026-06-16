using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeatured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "RealEstateObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Vg2P2.T4Ar7Aa9b.GqEGLON37/uZLtX0AvrPYEsqGh5Cn1SZRMqzm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "RealEstateObjects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$iRzSE7zQZAz75XGrduHbveFvjQbQ3QOayB9Ti80k1KyJ57yLJwbby");
        }
    }
}
