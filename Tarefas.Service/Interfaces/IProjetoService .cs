using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Application.Interfaces
{
    public interface IProjetoService : IBaseService<Projeto>
    {
        Task<List<Projeto>> ListarByUsuario(Usuario usuario);
    }
}
