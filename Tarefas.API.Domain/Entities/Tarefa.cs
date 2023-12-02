using System;
using System.Collections.Generic;
using Tarefas.API.Domain.Enumerators;

namespace Tarefas.API.Domain.Entities
{
    public class Tarefa: Base
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Vencimento { get; set; }
        public Status? Status { get; set; }
        public Prioridade Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? Conclusao { get; set; }
        public virtual Projeto Projeto { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual List<Comentario> Comentarios { get; set; }
        public virtual List<Historico> Historicos { get; set; }

    }
}
