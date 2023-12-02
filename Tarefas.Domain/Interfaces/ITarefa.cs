using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Domain.Interfaces
{
    public interface ITarefa : IBase<Tarefa>
    {
        Task<List<Tarefa>> ListarByProjeto(Projeto projeto);
    }
}
