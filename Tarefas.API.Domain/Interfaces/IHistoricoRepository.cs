using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface IHistoricoRepository
    {
        Task<List<Historico>> GetAllByTarefa(int id);
    }
}
