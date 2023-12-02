using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class ProjetoRepository : BaseRepository<Projeto>, IProjeto
    {
        public async Task<List<Projeto>> ListarByUsuario(Usuario usuario)
        {
            return await this.GetAll(p => p.Usuario.Id == usuario.Id);
        }
    }
}
