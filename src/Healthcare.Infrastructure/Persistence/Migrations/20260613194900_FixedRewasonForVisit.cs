using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Healthcare.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedRewasonForVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReasonForVisitc",
                table: "Patient",
                newName: "ReasonForVisit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReasonForVisit",
                table: "Patient",
                newName: "ReasonForVisitc");
        }
    }
}
