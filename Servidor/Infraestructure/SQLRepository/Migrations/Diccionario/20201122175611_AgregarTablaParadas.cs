using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLRepository.Migrations
{
    public partial class AgregarTablaParadas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paradas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitud = table.Column<double>(nullable: false),
                    Longitud = table.Column<double>(nullable: false),
                    RecorridoId = table.Column<int>(nullable: false),
                    Orden = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paradas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paradas");
        }
    }
}
