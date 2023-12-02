using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Application.Interfaces;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Application
{
    public class TarefaService : ITarefaService
    {
        public async Task Add(Tarefa entidade)
        {
            if (entidade.Status.Value.Equals(null))
            {
                throw new Exception("Status deve ser 0 (Pendente), 1 (Andamento) ou 2 (Concluida).");
            }
        }

        public async Task Delete(Tarefa entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tarefa>> GetAll(Expression<Func<Tarefa, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Tarefa> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tarefa>> ListarByProjeto(Projeto projeto)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Tarefa entidade)
        {
            Tarefa tarefaAnterior = await GetById(entidade.Id);

            if (tarefaAnterior.Prioridade != entidade.Prioridade)
            {
                throw new Exception("Não é permitido alterar a prioridade de uma tarefa!");
            }
        }
    }
}
