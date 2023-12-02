using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface IProjetoRepository : IBaseRepository<Projeto>
    {
        Task<List<Projeto>> GetAllByUsuario(int usuarioId);
    }
}
