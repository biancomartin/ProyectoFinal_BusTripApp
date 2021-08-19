using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLRepository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diccionarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecorridoId = table.Column<int>(nullable: false),
                    FranjaId = table.Column<int>(nullable: false),
                    PuntoOrigen = table.Column<int>(nullable: false),
                    PuntoDestino = table.Column<int>(nullable: false),
                    Tiempo = table.Column<double>(nullable: false),
                    CantidadDeMuestras = table.Column<int>(nullable: false),
                    Unidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diccionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FranjaHorarias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoraInicio = table.Column<int>(nullable: false),
                    HoraFin = table.Column<int>(nullable: false),
                    FinDeSemana = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranjaHorarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recorridos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Base = table.Column<int>(nullable: false),
                    Linea = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recorridos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 1, false, 5, 0 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 2, false, 7, 5 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 3, false, 9, 7 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 4, false, 11, 9 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 5, false, 13, 11 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 6, false, 15, 13 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 7, false, 17, 15 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 8, false, 19, 17 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 9, false, 21, 19 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 10, false, 24, 21 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 11, true, 5, 0 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 12, true, 12, 5 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 13, true, 17, 12 });

            migrationBuilder.InsertData(
                table: "FranjaHorarias",
                columns: new[] { "Id", "FinDeSemana", "HoraFin", "HoraInicio" },
                values: new object[] { 14, true, 24, 17 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 1, 1, 500 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 2, 2, 500 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 3, 1, 501 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 4, 2, 501 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 5, 1, 502 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 6, 2, 502 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 7, 3, 502 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 8, 1, 503 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 9, 2, 503 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 10, 3, 503 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 11, 4, 503 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 12, 1, 504 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 13, 2, 504 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 14, 1, 505 });

            migrationBuilder.InsertData(
                table: "Recorridos",
                columns: new[] { "Id", "Base", "Linea" },
                values: new object[] { 15, 2, 505 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diccionarios");

            migrationBuilder.DropTable(
                name: "FranjaHorarias");

            migrationBuilder.DropTable(
                name: "Recorridos");
        }
    }
}
