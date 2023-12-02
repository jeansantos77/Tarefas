using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface IComentarioRepository : IBaseRepository<Comentario>
    {
        Task<List<Comentario>> GetAllByTarefa(int id);
    }
}
