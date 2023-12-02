using System;

namespace Tarefas.API.Domain.Entities
{
    class Historico
    {
        public int Id { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorAtual { get; set; }
        public DateTime DataModificacao { get; set; }
        public int TarefaId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
