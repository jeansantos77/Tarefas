using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Application;
using Tarefas.API.Application.Implementations;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Domain.Enumerators;
using Tarefas.API.Domain.Interfaces;
using Xunit;

namespace Tarefas.API.Tests
{
    public class ProjetoServiceTests
    {
        ProjetoService _projetoService;
        public ProjetoServiceTests()
        {
            var mockProjeto = new Mock<IProjetoRepository>();
            mockProjeto.Setup(s => s.GetAll(It.IsAny<Expression<Func<Projeto, bool>>>())).Returns(GetAllProjetos());
            mockProjeto.Setup(s => s.GetAllByUsuario(It.IsAny<int>())).Returns(GetProjetosUsuario2());

            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetAll(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(GetTarefasPendentes());

            _projetoService = new ProjetoService(mockProjeto.Object, mockTarefa.Object);
        }


        [Fact]
        public void ProjetoService_Should_Not_Return_Exception_If_Try_Add_Projeto()
        {
            //Arrange
            Projeto projeto = GetProjeto();

            //Assert
            var exception = Record.ExceptionAsync(() => _projetoService.Add(projeto));
            Assert.Null(exception.Result);
        }

        [Fact]
        public async Task ProjetoService_Should_Return_Exception_If_Try_Delete_With_Pendent_Tasks()
        {
            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _projetoService.Delete(1));
            Assert.Equal("Existem tarefas pendentes associadas a este projeto! Você deve concluir ou remover as tarefas antes de remover esse projeto.", exception.Message);
        }

        [Fact]
        public async Task ProjetoService_Should_Return_Exception_If_Try_Delete_Projeto_Not_Found()
        {
            //Arrange
            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetAll(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(Task.FromResult(new List<Tarefa>()));

            _projetoService = new ProjetoService(new Mock<IProjetoRepository>().Object, mockTarefa.Object);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _projetoService.Delete(1));
            Assert.Equal($"Projeto [ Id = {1}] não encontrado.", exception.Message);
        }

        [Fact]
        public void ProjetoService_Should_Not_Return_Exception_If_Try_Delete_Projeto()
        {
            //Arrange
            var mockProjeto = new Mock<IProjetoRepository>();
            mockProjeto.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(GetProjeto()));

            var mockTarefa = new Mock<ITarefaRepository>();
            mockTarefa.Setup(s => s.GetAll(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(Task.FromResult(new List<Tarefa>()));

            //Act
            ProjetoService projetoService = new ProjetoService(mockProjeto.Object, mockTarefa.Object);

            //Assert
            var exception = Record.ExceptionAsync(() => projetoService.Delete(1));
            Assert.Null(exception.Result);
        }

        [Fact]
        public async Task ProjetoService_Should_Validate_GetAll_Method()
        {
            //Act
            List<Projeto> projetos = await _projetoService.GetAll(t => t.Id > 0);

            //Assert
            Assert.Equal(2, projetos.Count);
        }

        [Fact]
        public async Task ProjetoService_Should_Validate_GetAllByUsuario_Method()
        {
            //Act
            List<Projeto> projetos = await _projetoService.GetAllByUsuario(1);

            //Assert
            Assert.Single(projetos);
        }


        [Fact]
        public async Task ProjetoService_Should_Validate_GetAllProjetos_Method()
        {
            //Act
            List<Projeto> projetos = await _projetoService.GetAllProjetos();

            //Assert
            Assert.Equal(2, projetos.Count);
        }

        [Fact]
        public void ProjetoService_Should_Not_Return_Exception_If_Try_Update_Projeto()
        {
            //Arrange
            Projeto projeto = GetProjeto();

            //Assert
            var exception = Record.ExceptionAsync(() => _projetoService.Update(projeto));
            Assert.Null(exception.Result);
        }

        private Projeto GetProjeto()
        {
            Projeto projeto = new Projeto
            {
                Id = 1,
                Nome = "Projeto Teste",
                UsuarioId = 1
            };

            return projeto;
        }

        private Task<List<Projeto>> GetAllProjetos()
        {
            List<Projeto> projetos = new List<Projeto>
            {
                new Projeto
                {
                    Id = 1,
                    Nome = "Projeto Teste",
                    UsuarioId = 1
                },
                new Projeto
                {
                    Id = 2,
                    Nome = "Projeto Teste 2",
                    UsuarioId = 2
                }
            };

            return Task.FromResult(projetos);
        }

        private Task<List<Projeto>> GetProjetosUsuario2()
        {
            List<Projeto> projetos = new List<Projeto>
            {
                new Projeto
                {
                    Id = 2,
                    Nome = "Projeto Teste 2",
                    UsuarioId = 2
                }
            };

            return Task.FromResult(projetos);
        }

        private Task<List<Tarefa>> GetTarefasPendentes()
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
                    Status = Status.Pendente
                }
            };

            return Task.FromResult(tarefas);
        }

    }
}
