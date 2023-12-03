using Microsoft.EntityFrameworkCore.Migrations;

namespace Tarefas.API.Infra.Data.Migrations
{
    public partial class DeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Tarefas_TarefaId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Historicos_Tarefas_TarefaId",
                table: "Historicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Projetos_ProjetoId",
                table: "Tarefas");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Tarefas_TarefaId",
                table: "Comentarios",
                column: "TarefaId",
                principalTable: "Tarefas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Historicos_Tarefas_TarefaId",
                table: "Historicos",
                column: "TarefaId",
                principalTable: "Tarefas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Projetos_ProjetoId",
                table: "Tarefas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Tarefas_TarefaId",
                table: "Comentarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Historicos_Tarefas_TarefaId",
                table: "Historicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Projetos_ProjetoId",
                table: "Tarefas");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Tarefas_TarefaId",
                table: "Comentarios",
                column: "TarefaId",
                principalTable: "Tarefas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Historicos_Tarefas_TarefaId",
                table: "Historicos",
                column: "TarefaId",
                principalTable: "Tarefas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Projetos_ProjetoId",
                table: "Tarefas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id");
        }
    }
}
