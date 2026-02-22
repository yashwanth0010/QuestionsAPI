using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CategoryandOptionsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "QuestionsAndAnswers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "QuestionsAndAnswers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "QuestionsAndAnswers");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "QuestionsAndAnswers");
        }
    }
}
