using System.Collections.Generic;

namespace Tarefas.API.Domain.Entities
{
    public class Projeto: Base
    {
        public string Nome { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual List<Tarefa> Tarefas { get; set; }

        public Projeto()
        {
            Tarefas = new List<Tarefa>();
        }
    }

}
