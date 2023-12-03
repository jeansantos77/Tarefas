using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Application;
using Tarefas.API.Application.Models;
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
            mockTarefa.Setup(s => s.GetAll(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(GetTarefasByProjeto());

            var mockUsuario = new Mock<IUsuarioRepository>();
            mockUsuario.Setup(s => s.GetById(It.IsAny<int>())).Returns(GetUsuario());

            var mockComentario = new Mock<IComentarioRepository>();
            mockComentario.Setup(s => s.GetAllByTarefa(It.IsAny<int>())).Returns(GetComentariosByTarefa());
            mockComentario.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(GetComentario()));

            var mockHistorico = new Mock<IHistoricoRepository>();
            mockHistorico.Setup(s => s.GetAllByTarefa(It.IsAny<int>())).Returns(GetHistoricosByTarefa());

            _tarefaService = new TarefaService(mockTarefa.Object
                                            , mockUsuario.Object
                                            , mockComentario.Object
                                            , mockHistorico.Object);
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
        public async Task TarefaService_Should_Return_Exception_If_Prioridade_Is_Null()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();
            tarefa.Status = Status.Pendente;
            tarefa.Prioridade = null;

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.Add(tarefa));
            Assert.Equal("Prioridade deve ser 0 (Baixa), 1 (Média) ou 2 (Alta).", exception.Message);
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
        public void TarefaService_Should_Not_Return_Exception_If_Try_Update_Tarefa()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();
            tarefa.Status = Status.Pendente;
            tarefa.Prioridade = Prioridade.Baixa;

            //Assert
            var exception = Record.ExceptionAsync(() => _tarefaService.Update(tarefa));
            Assert.Null(exception.Result);
        }


        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Usuario_Not_Found_GetTarefasConcluidas()
        {
            //Arrange
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
            Assert.Equal(7, tarefas.Count);
        }

        [Fact]
        public async Task TarefaService_Should_Validate_GetAll_Method()
        {
            //Act
            List<Tarefa> tarefas = await _tarefaService.GetAll(t => t.Id > 0);

            //Assert
            Assert.Equal(20, tarefas.Count);
        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Try_Delete_Comentario_Not_Found()
        {
            //Arrange
            Comentario comentario = GetComentario();

            var mockComentario = new Mock<IComentarioRepository>();
            mockComentario.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult<Comentario>(null));

            TarefaService tarefaService = new TarefaService(new Mock<ITarefaRepository>().Object
                                                          , new Mock<IUsuarioRepository>().Object
                                                          , mockComentario.Object
                                                          , new Mock<IHistoricoRepository>().Object);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => tarefaService.DeleteComentario(comentario.Id));
            Assert.Equal($"Comentário [ Id = {comentario.Id}] não encontrado.", exception.Message);
        }

        [Fact]
        public async Task TarefaService_Should_Return_Exception_If_Usuario_Not_Found_GetMediaTarefasConcluidasByUsuario()
        {
            //Arrange
            var mockUsuario = new Mock<IUsuarioRepository>();
            mockUsuario.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult<Usuario>(null));

            TarefaService tarefaService = new TarefaService(new Mock<ITarefaRepository>().Object, mockUsuario.Object, new Mock<IComentarioRepository>().Object, new Mock<IHistoricoRepository>().Object);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => tarefaService.GetMediaTarefasConcluidasByUsuario(30, 10));
            Assert.Equal($"Usuário [{10}] não encontrado!", exception.Message);
        }

        [Fact]
        public async Task TarefaService_Should_Validate_GetComentariosByTarefa()
        {
            //Act
            List<Comentario> comentarios = await _tarefaService.GetComentariosByTarefa(1);

            //Assert
            Assert.Equal(3, comentarios.Count);
        }

        [Fact]
        public async Task TarefaService_Should_Validate_GetHistoricossByTarefa()
        {
            //Act
            List<Historico> historicos = await _tarefaService.GetHistoricosByTarefa(1);

            //Assert
            Assert.Equal(4, historicos.Count);
        }

        [Fact]
        public void TarefaService_Should_Not_Return_Exception_If_Try_Add_Tarefa()
        {
            //Arrange
            Tarefa tarefa = GetTarefa();
            tarefa.Status = Status.Pendente;
            tarefa.Prioridade = Prioridade.Alta;

            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetAllByProjeto(It.IsAny<int>())).Returns(Task.FromResult(new List<Tarefa>()));

            TarefaService tarefaService = new TarefaService(mockTarefa.Object
                                                          , new Mock<IUsuarioRepository>().Object
                                                          , new Mock<IComentarioRepository>().Object
                                                          , new Mock<IHistoricoRepository>().Object);

            //Assert
            var exception = Record.ExceptionAsync(() => tarefaService.Add(tarefa));
            Assert.Null(exception.Result);
        }

        [Fact]
        public void TarefaService_Should_Not_Return_Exception_If_Try_Add_Comentario()
        {
            //Arrange
            Comentario comentario = GetComentario();

            //Assert
            var exception = Record.ExceptionAsync(() => _tarefaService.AddComentario(comentario));
            Assert.Null(exception.Result);
        }

        [Fact]
        public void TarefaService_Should_Not_Return_Exception_If_Try_Delete_Tarefa()
        {
            //Arrange
            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(GetTarefa()));

            TarefaService tarefaService = new TarefaService(mockTarefa.Object
                                                          , new Mock<IUsuarioRepository>().Object
                                                          , new Mock<IComentarioRepository>().Object
                                                          , new Mock<IHistoricoRepository>().Object);

            //Assert
            var exception = Record.ExceptionAsync(() => tarefaService.Delete(1));
            Assert.Null(exception.Result);
        }

        [Fact]
        public void TarefaService_Should_Not_Return_Exception_If_Try_Delete_Comentario()
        {
            //Assert
            var exception = Record.ExceptionAsync(() => _tarefaService.DeleteComentario(1));
            Assert.Null(exception.Result);
        }


        [Fact]
        public async Task TarefaService_Should_Return_List_GetMediaTarefasConcluidasByUsuario()
        {
            //Act
            List<TarefaConcluidaModel> tarefasConcluidas = await _tarefaService.GetMediaTarefasConcluidasByUsuario(3, 1);

            //Assert
            Assert.Equal(3, tarefasConcluidas.Count);
            Assert.Single(tarefasConcluidas.Where(x => Math.Round(x.MediaTarefasConcluidas, 2) == 0.33));
       }


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
                },
                new Tarefa
                {
                    Titulo = "Tarefa 4",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 2,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                },
                new Tarefa
                {
                    Titulo = "Tarefa 5",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 2,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                },
                new Tarefa
                {
                    Titulo = "Tarefa 6",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 2,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                },
                new Tarefa
                {
                    Titulo = "Tarefa 7",
                    Vencimento = DateTime.Today.AddDays(10),
                    Descricao = "Descricao",
                    Prioridade = Prioridade.Media,
                    UsuarioId = 3,
                    ProjetoId = 1,
                    Conclusao = DateTime.Today,
                    Status = Status.Concluida
                }
            };

            return Task.FromResult(tarefas);
        }

        private Task<List<Comentario>> GetComentariosByTarefa()
        {
            List<Comentario> comentarios = new List<Comentario>
            {
                new Comentario
                {
                    Id = 1,
                    Descricao = "Descricao 1",
                    TarefaId = 1
                },
                new Comentario
                {
                    Id = 2,
                    Descricao = "Descricao 2",
                    TarefaId = 1
                },
                new Comentario
                {
                    Id = 3,
                    Descricao = "Descricao 3",
                    TarefaId = 1
                }
            };

            return Task.FromResult(comentarios);
        }

        private Task<List<Historico>> GetHistoricosByTarefa()
        {
            List<Historico> historicos = new List<Historico>
            {
                new Historico
                {
                    Id = 1,
                    ValorAnterior = "Descrição = Teste",
                    ValorAtual = "Descrição = Teste alterado",
                    DataModificacao = DateTime.Now,
                    UsuarioId = 1,
                    TarefaId = 1
                },
                new Historico
                {
                    Id = 2,
                    ValorAnterior = "Descrição = Teste 2",
                    ValorAtual = "Descrição = Teste alterado 2",
                    DataModificacao = DateTime.Now,
                    UsuarioId = 2,
                    TarefaId = 1
                },
                new Historico
                {
                    Id = 3,
                    ValorAnterior = "Descrição = Teste 3",
                    ValorAtual = "Descrição = Teste alterado 3",
                    DataModificacao = DateTime.Now,
                    UsuarioId = 3,
                    TarefaId = 1
                },
                new Historico
                {
                    Id = 4,
                    ValorAnterior = "Descrição = Teste 4",
                    ValorAtual = "Descrição = Teste alterado 4",
                    DataModificacao = DateTime.Now,
                    UsuarioId = 4,
                    TarefaId = 1
                }
            };

            return Task.FromResult(historicos);
        }

        private static Comentario GetComentario()
        {
            return new Comentario 
            { 
                Id = 1,
                Descricao = "Comentario", 
                TarefaId = 1,
                Tarefa = new Tarefa
                {
                    Id= 1,
                    Descricao = "Tarefa 1",
                }
            };
        }

    }
}
