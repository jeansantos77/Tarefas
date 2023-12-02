using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface ITarefaRepository : IBaseRepository<Tarefa>
    {
        Task<List<Tarefa>> GetAllByProjeto(int id);
        Task<List<Tarefa>> GetTarefasConcluidas(DateTime dataInicial, DateTime dataFinal);
        Task<Tarefa> GetByIdAsNoTracking(int id);
        
    }
}
