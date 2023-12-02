using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class ComentarioRepository : BaseRepository<Comentario>, IComentarioRepository
    {
        public async Task<List<Comentario>> GetAllByTarefa(int id)
        {
            return await GetAll(p => p.Tarefa.Id == id);
        }
    }
}
