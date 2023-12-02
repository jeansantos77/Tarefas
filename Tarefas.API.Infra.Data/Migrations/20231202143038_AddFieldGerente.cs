using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tarefas.API.Infra.Data.Migrations
{
    public partial class AddFieldGerente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGerente",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Conclusao",
                table: "Tarefas",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGerente",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Conclusao",
                table: "Tarefas");
        }
    }
}
