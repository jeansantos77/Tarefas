using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Infra.Data.Repository
{
    public class ProjetoRepository : BaseRepository<Projeto>, IProjetoRepository
    {
        public async Task<List<Projeto>> GetAllByUsuario(int usuarioId)
        {
            return await GetAll(p => p.Usuario.Id == usuarioId);
        }
    }
}
