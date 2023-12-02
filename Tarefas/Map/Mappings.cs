using AutoMapper;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Models;

namespace Tarefas.API.Map
{
    public class Mappings: Profile
    {
        public Mappings()
        {
            CreateMap<Projeto, ProjetoModel>()
                .ReverseMap();
            CreateMap<Projeto, ProjetoUpdateModel>()
                .ReverseMap();
            CreateMap<Tarefa, TarefaModel>()
                .ReverseMap();
            CreateMap<Tarefa, TarefaUpdateModel>()
                .ReverseMap();
            CreateMap<Comentario, ComentarioModel>()
                .ReverseMap();
        }
    }
}
