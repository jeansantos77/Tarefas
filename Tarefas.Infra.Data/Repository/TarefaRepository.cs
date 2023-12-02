using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefa
    {
        public async Task<List<Tarefa>> ListarByProjeto(Projeto projeto)
        {
            return await this.GetAll(p => p.Projeto.Id == projeto.Id);
        }
    }
}
