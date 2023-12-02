using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tarefas.API.Application;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Enumerators;
using Tarefas.API.Domain.Interfaces;
using Xunit;

namespace Tarefas.API.Tests
{
    public class TarefaServiceTests
    {
        TarefaService _tarefaService;
        public TarefaServiceTests()
        {
            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetAllByProjeto(It.IsAny<int>())).Returns(GetTarefasByProjeto());
            mockTarefa.Setup(s => s.GetByIdAsNoTracking(It.IsAny<int>())).Returns(GetTarefaAnterior());
            mockTarefa.Setup(s => s.GetTarefasConcluidas(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(GetTarefasConcluidas());

            var mockUsuario = new Mock<IUsuarioRepository>();
            mockUsuario.Setup(s => s.GetById(It.IsAny<int>())).Returns(GetUsuario());

            _tarefaService = new TarefaService(mockTarefa.Object
                                            , mockUsuario.Object
                                            , new Mock<IComentarioRepository>().Object
                                            , new Mock<IHistoricoRepository>().Object);
        }

        [Fact]
        public void TarefaService_Should_Return_Exception_If_Usuario_is_not_Gerente()
        {
            //Arrange
            Usuario usuario = new Usuario
            {
                Id = 1,
                Nome = "Teste",
                IsGerente = false
            };

            //Assert
            var exception = Assert.Throws<Exception>(() => _tarefaService.ValidateIfUsuarioIsGerente(usuario));
            Assert.Equal("Somente Gerentes tem permissão para visualizar tarefas concluídas!", exception.Message);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Status_Is_Null()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.Add(tarefa));
            Assert.Equal("Status deve ser 0 (Pendente), 1 (Andamento) ou 2 (Concluida).", exception.Message);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_There_Is_20_Tasks()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();
            tarefa.Status = Status.Pendente;

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.Add(tarefa));
            Assert.Equal("O limite máximo para o projeto são 20 tarefas! Não será possível inserir a tarefa.", exception.Message);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Try_Delete_Tarefa_Not_Found()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.Delete(tarefa.Id));
            Assert.Equal($"Tarefa [ Id = {tarefa.Id}] não encontrado.", exception.Message);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Try_Update_Prioridade()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.Update(tarefa));
            Assert.Equal("Não é permitido alterar a prioridade de uma tarefa!", exception.Message);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Usuario_Not_Found_GetTarefasConcluidas()
        {
            //Assert
            var mockUsuario = new Mock<IUsuarioRepository>();
            mockUsuario.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult<Usuario>(null));

            TarefaService tarefaService = new TarefaService(new Mock<ITarefaRepository>().Object, mockUsuario.Object, new Mock<IComentarioRepository>().Object, new Mock<IHistoricoRepository>().Object);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => tarefaService.GetTarefasConcluidas(DateTime.Today, DateTime.Today, 1));
            Assert.Equal($"Usuário [{1}] não encontrado!", exception.Message);
        }

        [Fact]
        public async Task TarefaService_Should_Validate_GetTarefasConcluidas()
        {

            //Act
            List<Tarefa> tarefas = await _tarefaService.GetTarefasConcluidas(DateTime.Today, DateTime.Today, 1);

            //Assert
            Assert.Equal(3, tarefas.Count);

        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Try_Delete_Comentario_Not_Found()
        {
            //Arrange
            Comentario comentario = new Comentario { Id = 1 };

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.DeleteComentario(comentario.Id));
            Assert.Equal($"Comentário [ Id = {comentario.Id}] não encontrado.", exception.Message);

        }


        /*
         
        
        public async Task<List<Comentario>> GetComentariosByTarefa(int id)
        {
            return await _comentarioRepository.GetAllByTarefa(id);
        }

        public async Task<List<Historico>> GetHistoricosByTarefa(int id)
        {
            return await _historicoRepository.GetAllByTarefa(id);
        }


         * */


        private Task<Usuario> GetUsuario()
        {
            Usuario usuario = new Usuario
            {
                Id = 1,
                Nome = "Usuario Teste",
                IsGerente = true
            };

            return Task.FromResult(usuario);

        }
        private Tarefa GetTarefa()
        {
            Tarefa tarefa = new Tarefa
            {
                Id = 1,
                Titulo = "Titulo",
                Vencimento = DateTime.Today.AddDays(10),
                Descricao = "Descricao",
                Prioridade = Prioridade.Media,
                UsuarioId = 1,
                ProjetoId = 1,
            };

            return tarefa;
        }

        private Task<Tarefa> GetTarefaAnterior()
        {
            Tarefa tarefa = new Tarefa
            {
                Id = 1,
                Titulo = "Titulo",
                Vencimento = DateTime.Today.AddDays(10),
                Descricao = "Descricao",
                Prioridade = Prioridade.Baixa,
                UsuarioId = 1,
                ProjetoId = 1,
            };

            return Task.FromResult(tarefa);
        }

        private Task<List<Tarefa>> GetTarefasByProjeto()
        {
            List<Tarefa> tarefas = new List<Tarefa>
            {
                new Tarefa
                {
                    Titulo = "Tarefa 1",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 2",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 3",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 4",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 5",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 6",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 7",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 8",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 9",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 10",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 11",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 12",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 13",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 14",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 15",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 16",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 17",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 18",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 19",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                },
                new Tarefa
                {
                    Titulo = "Tarefa 20",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1
                }
            };

            return Task.FromResult(tarefas);
        }

        private Task<List<Tarefa>> GetTarefasConcluidas()
        {
            List<Tarefa> tarefas = new List<Tarefa>
            {
                new Tarefa
                {
                    Titulo = "Tarefa 1",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                },
                new Tarefa
                {
                    Titulo = "Tarefa 2",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                },
                new Tarefa
                {
                    Titulo = "Tarefa 3",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 1,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                }
            };

            return Task.FromResult(tarefas);
        }


    }
}
