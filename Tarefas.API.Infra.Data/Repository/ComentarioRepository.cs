using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class ComentarioRepository : BaseRepository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(DBContext context) : base(context)
        {

        }
        public async Task<List<Comentario>> GetAllByTarefa(int id)
        {
            return await GetAll(p => p.TarefaId == id);
        }
    }
}
