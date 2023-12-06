using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Enumerators;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(DBContext context) : base(context)
        {

        }
        public async Task<List<Tarefa>> GetAllByProjeto(int id)
        {
            return await GetAll(p => p.ProjetoId == id);
        }

        public async Task<Tarefa> GetByIdAsNoTracking(int id)
        {
            return await _dbContext.Set<Tarefa>().Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Tarefa>> GetTarefasConcluidas(DateTime dataInicial, DateTime dataFinal)
        {
            return await GetAll(p => p.Status == Status.Concluida && (p.Conclusao >= dataInicial && p.Conclusao <= dataFinal));
        }

        public override async Task Update(Tarefa entidade)
        {
            Tarefa tarefa = await GetByIdAsNoTracking(entidade.Id);

            PropertyInfo[] properties = typeof(Tarefa).GetProperties();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Set<Tarefa>().Update(entidade);

                    foreach (var property in properties)
                    {
                        if (!property.Name.Equals("Historicos"))
                        {
                            object value1 = property.GetValue(tarefa);
                            object value2 = property.GetValue(entidade);

                            if (!object.Equals(value1, value2))
                            {

                                Historico hist = new Historico
                                {
                                    TarefaId = entidade.Id,
                                    ValorAnterior = $"{property.Name} = {((value1 == null) ? string.Empty : value1.ToString())}",
                                    ValorAtual = $"{property.Name} = {((value2 == null) ? string.Empty : value2.ToString())}",
                                    DataModificacao = DateTime.Now,
                                    UsuarioId = entidade.UsuarioId
                                };

                                await _dbContext.Set<Historico>().AddAsync(hist);
                            }
                        }
                    }

                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }

    }
}
