using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tarefas.API.Application.Interfaces;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Application.Implementations
{
    public class ProjetosService : IProjetoService
    {
        public Task Add(Projeto entidade)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Projeto entidade)
        {
            throw new NotImplementedException();
        }

        public Task<List<Projeto>> GetAll(Expression<Func<Projeto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Projeto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Projeto>> ListarByUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task Update(Projeto entidade)
        {
            throw new NotImplementedException();
        }
    }
}
