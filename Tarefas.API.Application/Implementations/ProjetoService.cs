using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Application.Interfaces;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Enumerators;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Application.Implementations
{
    public class ProjetoService : IProjetoService
    {
        private IProjetoRepository _projetoRepository;
        private ITarefaRepository _tarefaRepository;

        public ProjetoService(IProjetoRepository projetoRepository
                            , ITarefaRepository tarefaRepository)
        {
            _projetoRepository = projetoRepository;
            _tarefaRepository = tarefaRepository;
        }

        public async Task Add(Projeto entidade)
        {
            await _projetoRepository.Add(entidade);
        }

        public async Task Delete(int id)
        {
            List<Tarefa> tarefas = await _tarefaRepository.GetAll(t => t.Status == Status.Pendente && t.ProjetoId == id);

            if (tarefas.Any())
            {
                throw new Exception("Existem tarefas pendentes associadas a este projeto! Você deve concluir ou remover as tarefas antes de remover esse projeto.");
            }

            Projeto projeto = await GetById(id);

            if (projeto == null)
            {
                throw new Exception($"Projeto [ Id = {id}] não encontrado.");
            }

            await _projetoRepository.Delete(projeto);
        }

        public async Task<List<Projeto>> GetAll(Expression<Func<Projeto, bool>> predicate)
        {
            return await _projetoRepository.GetAll(predicate);
        }

        public async Task<Projeto> GetById(int id)
        {
            return await _projetoRepository.GetById(id);
        }

        public async Task<List<Projeto>> GetAllByUsuario(int usuarioId)
        {
            return await _projetoRepository.GetAllByUsuario(usuarioId);
        }

        public async Task<List<Projeto>> GetAllProjetos()
        {
            return await _projetoRepository.GetAll(p => p.Id > 0);
        }

        public async Task Update(Projeto entidade)
        {
            await _projetoRepository.Update(entidade);
        }
    }
}
