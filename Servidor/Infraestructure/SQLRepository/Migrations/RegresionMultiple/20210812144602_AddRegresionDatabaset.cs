using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLRepository.Migrations.RegresionMultiple
{
    public partial class AddRegresionDatabaset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegresionMultipleHistorico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnidadId = table.Column<string>(type: "TEXT", nullable: true),
                    FranjaHorariaId = table.Column<string>(type: "TEXT", nullable: true),
                    RecorridoId = table.Column<string>(type: "TEXT", nullable: true),
                    LineaId = table.Column<string>(type: "TEXT", nullable: true),
                    Tiempo = table.Column<float>(type: "REAL", nullable: false),
                    DiferenciaCeldas = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegresionMultipleHistorico", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegresionMultipleHistorico");
        }
    }
}
