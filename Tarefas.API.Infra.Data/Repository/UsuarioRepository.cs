using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
    }
}
