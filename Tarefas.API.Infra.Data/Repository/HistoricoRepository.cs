using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class HistoricoRepository : BaseRepository<Historico>, IHistoricoRepository
    {
        public async Task<List<Historico>> GetAllByTarefa(int id)
        {
            return await GetAll(p => p.TarefaId == id);
        }
    }
}
