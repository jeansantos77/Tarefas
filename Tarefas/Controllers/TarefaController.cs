using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tarefas.API.Application.Interfaces;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Models;

namespace Tarefas.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public TarefaController(IMapper mapper
                               , ITarefaService projetoService)
        {
            _tarefaService = projetoService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("ListaTodasTarefas/{projetoId}")]
        public async Task<IActionResult> GetAllByProjeto([FromRoute] int projetoId)
        {
            return Ok(await _tarefaService.GetAllByProjeto(projetoId));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TarefaModel tarefaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.Add(_mapper.Map<Tarefa>(tarefaModel));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, tarefaModel);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TarefaUpdateModel tarefaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.Update(_mapper.Map<Tarefa>(tarefaModel));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message); 
            }

            return Ok(tarefaModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.Delete(id);
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("ListaMediaTarefasConcluidasPorUsuario")]
        public async Task<IActionResult> GetMediaTarefasConcluidasByUsuario([FromQuery] int numeroDias, [FromQuery] int usuarioId)
        {
            try
            {
                return Ok(await _tarefaService.GetMediaTarefasConcluidasByUsuario(numeroDias, usuarioId));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet]
        [Route("ListaTarefasConcluidas")]
        public async Task<IActionResult> GetTarefasConcluidas([FromQuery] DateTime dataInicial, [FromQuery] DateTime dataFinal, [FromQuery] int usuarioId)
        {
            try
            {
                return Ok(await _tarefaService.GetTarefasConcluidas(dataInicial, dataFinal, usuarioId));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpPost]
        [Route("AddComentario")]
        public async Task<IActionResult> AddComentario([FromBody] ComentarioModel comentarioModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.AddComentario(_mapper.Map<Comentario>(comentarioModel));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, comentarioModel);
        }

        [HttpGet]
        [Route("ListaTodosComentarios/{tarefaId}")]
        public async Task<IActionResult> GetComentariosByTarefa([FromRoute] int tarefaId)
        {
            return Ok(await _tarefaService.GetComentariosByTarefa(tarefaId));
        }

    }
}
