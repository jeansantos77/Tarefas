using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Application.Interfaces;
using Tarefas.API.Application.Models;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Interfaces;

namespace Tarefas.API.Application
{
    public class TarefaService : ITarefaService
    {
        private ITarefaRepository _tarefaRepository;
        private IUsuarioRepository _usuarioRepository;
        private IComentarioRepository _comentarioRepository;
        private IHistoricoRepository _historicoRepository;

        public TarefaService(ITarefaRepository tarefaRepository
                           , IUsuarioRepository usuarioRepository
                           , IComentarioRepository comentarioRepository
                           , IHistoricoRepository historicoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _usuarioRepository = usuarioRepository;
            _comentarioRepository = comentarioRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task Add(Tarefa entidade)
        {
            if (entidade.Status.Value.Equals(null))
            {
                throw new Exception("Status deve ser 0 (Pendente), 1 (Andamento) ou 2 (Concluida).");
            }

            if ((await GetAllByProjeto(entidade.ProjetoId)).Count() > 20)
            {
                throw new Exception("O limite máximo para o projeto são 20 tarefas! Não será possível inserir a tarefa.");
            }

            await _tarefaRepository.Add(entidade);
        }

        public async Task Delete(int id)
        {
            Tarefa tarefa = await GetById(id);

            if (tarefa == null)
            {
                throw new Exception($"Tarefa [ Id = {id}] não encontrado.");
            }

            await _tarefaRepository.Delete(tarefa);
        }

        public async Task<List<Tarefa>> GetAll(Expression<Func<Tarefa, bool>> predicate)
        {
            return await _tarefaRepository.GetAll(predicate);
        }

        public async Task<Tarefa> GetById(int id)
        {
            return await _tarefaRepository.GetById(id);
        }

        public async Task<List<Tarefa>> GetAllByProjeto(int id)
        {
            return await _tarefaRepository.GetAllByProjeto(id);
        }

        public async Task Update(Tarefa entidade)
        {
            Tarefa tarefa = await GetByIdAsNoTracking(entidade.Id);

            if (tarefa.Prioridade != entidade.Prioridade)
            {
                throw new Exception("Não é permitido alterar a prioridade de uma tarefa!");
            }

            await _tarefaRepository.Update(entidade);
        }

        public async Task<List<TarefaConcluidaModel>> GetMediaTarefasConcluidasByUsuario(int numeroDias, int usuarioId)
        {
            Usuario usuario = await _usuarioRepository.GetById(usuarioId);

            if (usuario == null)
            {
                throw new Exception($"Usuário [{usuarioId}] não encontrado!");
            }
            else if (!usuario.IsGerente)
            {
                throw new Exception($"Somente Gerentes tem permissão para visualizar tarefas concluídas!");
            }

            DateTime dataInicial = DateTime.Today.AddDays(numeroDias * -1);

            List<Tarefa> tarefas = await _tarefaRepository.GetTarefasConcluidas(dataInicial, DateTime.Now);

            var groups = tarefas.GroupBy(n => n.UsuarioId)
                                     .Select(n => new
                                     {
                                         UsuarioId = n.Key,
                                         Count = n.Count()
                                     })
                                     .OrderBy(n => n.UsuarioId);

            List<TarefaConcluidaModel> tarefasConcluidas = new List<TarefaConcluidaModel>();

            foreach (var item in groups)
            {
                TarefaConcluidaModel tc = new TarefaConcluidaModel
                {
                    Usuario = (await _usuarioRepository.GetById(item.UsuarioId)).Nome,
                    MediaTarefasConcluidas = (double)item.Count / (double) numeroDias
                };
                
                tarefasConcluidas.Add(tc);
            }

            return tarefasConcluidas;
        }


        public async Task<List<Tarefa>> GetTarefasConcluidas(DateTime dataInicial, DateTime dataFinal, int usuarioId)
        {
            Usuario usuario = await _usuarioRepository.GetById(usuarioId);

            if (usuario == null)
            {
                throw new Exception($"Usuário [{usuarioId}] não encontrado!");
            }
            else if (!usuario.IsGerente)
            {
                throw new Exception($"Somente Gerentes tem permissão para visualizar tarefas concluídas!");
            }

            List<Tarefa> tarefas = await _tarefaRepository.GetTarefasConcluidas(dataInicial, dataFinal);

            return tarefas;
        }


        public async Task<Tarefa> GetByIdAsNoTracking(int id)
        {
            return await _tarefaRepository.GetByIdAsNoTracking(id);
        }

        public async Task AddComentario(Comentario entidade)
        {
            await _comentarioRepository.Add(entidade);
        }

        public async Task DeleteComentario(int id)
        {
            Comentario comentario = await _comentarioRepository.GetById(id);

            if (comentario == null)
            {
                throw new Exception($"Comentário [ Id = {id}] não encontrado.");
            }

            await _comentarioRepository.Delete(comentario);
        }

        public async Task<List<Comentario>> GetComentariosByTarefa(int id)
        {
            return await _comentarioRepository.GetAllByTarefa(id);
        }

        public async Task<List<Historico>> GetHistoricosByTarefa(int id)
        {
            return await _historicoRepository.GetAllByTarefa(id);
        }

    }
}
