using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPlatform.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHasThesisToSubjectMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasThesis",
                table: "Subjects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasThesis",
                table: "Subjects");
        }
    }
}
