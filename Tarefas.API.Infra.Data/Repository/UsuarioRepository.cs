using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DBContext context) : base (context)
        {

        }
    }
}
