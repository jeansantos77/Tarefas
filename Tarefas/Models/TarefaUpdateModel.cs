using System;

namespace Tarefas.API.Models
{
    public class TarefaUpdateModel : TarefaModel
    {
        public int Id { get; set; }
        public DateTime Conclusao { get; set; }
    }
}
