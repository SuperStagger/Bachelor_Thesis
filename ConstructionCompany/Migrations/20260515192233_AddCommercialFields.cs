using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddCommercialFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CeilingHeight",
                table: "RealEstateObjects",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialType",
                table: "RealEstateObjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "RealEstateObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSeparateEntrance",
                table: "RealEstateObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$ABDm9mk9zQdB8SLuF.bkYuRbVSqQ6SGq0SeHA73FfYej6F7MgQTP.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CeilingHeight",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "CommercialType",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "RealEstateObjects");

            migrationBuilder.DropColumn(
                name: "HasSeparateEntrance",
                table: "RealEstateObjects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$OheeGbRsvv/DcaJzFnlrh.wxflEr3dYz1taVvw1L/gMZD.X8pZqBu");
        }
    }
}
