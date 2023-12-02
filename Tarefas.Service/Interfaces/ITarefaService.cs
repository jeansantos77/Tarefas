using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Application.Interfaces
{
    public interface ITarefaService : IBaseService<Tarefa>
    {
        Task<List<Tarefa>> ListarByProjeto(Projeto projeto);
    }
}
