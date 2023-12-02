using System;

namespace Tarefas.API.Models
{
    public class TarefaModel
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Vencimento { get; set; }
        public int Status { get; set; }
        public int Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
    }
}
