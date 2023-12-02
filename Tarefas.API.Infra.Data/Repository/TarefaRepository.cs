using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Enumerators;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        public async Task<List<Tarefa>> GetAllByProjeto(int id)
        {
            return await GetAll(p => p.Projeto.Id == id);
        }

        public async Task<Tarefa> GetByIdAsNoTracking(int id)
        {
            return await _dbContext.Set<Tarefa>().Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Tarefa>> GetTarefasConcluidas(DateTime dataInicial, DateTime dataFinal)
        {
            return await GetAll(p => p.Status == Status.Concluida && (p.Conclusao >= dataInicial && p.Conclusao <= dataFinal));
        }

        public async override Task Add(Tarefa entidade)
        {
            await _dbContext.Set<Tarefa>().AddAsync(entidade);

            foreach (var item in entidade.Comentarios)
            {
                await _dbContext.Set<Comentario>().AddAsync(item);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
