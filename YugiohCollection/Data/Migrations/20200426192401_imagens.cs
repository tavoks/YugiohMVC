using Microsoft.EntityFrameworkCore.Migrations;

namespace YugiohCollection.Data.Migrations
{
    public partial class imagens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Duelistas",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Efeito",
                table: "Cartas",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Cartas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Duelistas");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Cartas");

            migrationBuilder.AlterColumn<string>(
                name: "Efeito",
                table: "Cartas",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
