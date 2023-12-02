namespace Tarefas.API.Domain.Entities
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int TarefaId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }
}
