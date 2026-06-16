using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionCompany.Migrations
{
    /// <inheritdoc />
    public partial class FixCeilingHeightPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CeilingHeight",
                table: "RealEstateObjects",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$VjTZreMMXxWJudYqzeZm2.eg6cIWhtJcNro8ddBHprz8jzYIstg2q");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CeilingHeight",
                table: "RealEstateObjects",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$ABDm9mk9zQdB8SLuF.bkYuRbVSqQ6SGq0SeHA73FfYej6F7MgQTP.");
        }
    }
}
