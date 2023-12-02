using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface IProjeto : IBase<Projeto>
    {
        Task<List<Projeto>> ListarByUsuario(Usuario usuario);
    }
}
