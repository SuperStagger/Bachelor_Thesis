using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddIsProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProject",
                table: "RealEstateObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$OheeGbRsvv/DcaJzFnlrh.wxflEr3dYz1taVvw1L/gMZD.X8pZqBu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProject",
                table: "RealEstateObjects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Vg2P2.T4Ar7Aa9b.GqEGLON37/uZLtX0AvrPYEsqGh5Cn1SZRMqzm");
        }
    }
}
