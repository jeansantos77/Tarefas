﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class ProjetoRepository : BaseRepository<Projeto>, IProjetoRepository
    {
        public ProjetoRepository(DBContext context) : base(context)
        {

        }
        public async Task<List<Projeto>> GetAllByUsuario(int usuarioId)
        {
            return await GetAll(p => p.UsuarioId == usuarioId);
        }
    }
}
