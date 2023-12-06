using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class HistoricoRepository : BaseRepository<Historico>, IHistoricoRepository
    {
        public HistoricoRepository(DBContext context) : base(context)
        {

        }
        public async Task<List<Historico>> GetAllByTarefa(int id)
        {
            return await GetAll(p => p.TarefaId == id);
        }
    }
}
