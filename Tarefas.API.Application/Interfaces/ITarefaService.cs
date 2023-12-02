using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Application.Models;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Application.Interfaces
{
    public interface ITarefaService : IBaseService<Tarefa>
    {
        Task<List<Tarefa>> GetAllByProjeto(int id);
        Task<List<Comentario>> GetComentariosByTarefa(int id);
        Task<List<Historico>> GetHistoricosByTarefa(int id);
        Task<Tarefa> GetByIdAsNoTracking(int id);
        Task<List<TarefaConcluidaModel>> GetMediaTarefasConcluidasByUsuario(int numeroDias, int usuarioId );
        Task<List<Tarefa>> GetTarefasConcluidas(DateTime dataInicial, DateTime dataFinal, int usuarioId);
        Task AddComentario(Comentario entidade);
        Task DeleteComentario(int id);
    }
}
