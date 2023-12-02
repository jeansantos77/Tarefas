using System.Collections.Generic;

namespace Tarefas.API.Domain.Entities
{
    public class Usuario: Base
    {
        public string Nome { get; set; }
        public bool IsGerente { get; set; }
        public virtual List<Projeto> Projetos { get; set; }
    }
}
