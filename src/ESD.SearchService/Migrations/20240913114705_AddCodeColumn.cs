using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchService.Migrations;

/// <inheritdoc />
public partial class AddCodeColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "code",
            table: "transactions",
            type: "longtext",
            nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "code",
            table: "transactions");
    }
}
