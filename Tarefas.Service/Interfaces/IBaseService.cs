using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tarefas.API.Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task Add(T entidade);
        Task Update(T entidade);
        Task Delete(T entidade);
        Task<T> GetById(int id);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
    }
}
