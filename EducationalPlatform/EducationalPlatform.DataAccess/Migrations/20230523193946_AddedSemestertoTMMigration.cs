using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPlatform.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSemestertoTMMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "TeachingMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semester",
                table: "TeachingMaterials");
        }
    }
}
