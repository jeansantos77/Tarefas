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
    [ApiController]
    [Route("[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projetoService;
        private readonly IMapper _mapper;

        public ProjetoController( IMapper mapper
                               , IProjetoService projetoService)
        {
            _projetoService = projetoService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("ListaTodosProjetos")]
        public async Task<IActionResult> GetAllProjetos()
        {
            return Ok(await _projetoService.GetAllProjetos());
        }

        [HttpGet]
        [Route("ListaTodosProjetos/{usuarioId}")]
        public async Task<IActionResult> GetAllByUsuario([FromRoute] int usuarioId)
        {
            return Ok(await _projetoService.GetAllByUsuario(usuarioId));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProjetoModel projetoModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _projetoService.Add(_mapper.Map<Projeto>(projetoModel));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, projetoModel);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjetoUpdateModel projetoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _projetoService.Update(_mapper.Map<Projeto>(projetoModel));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return Ok(projetoModel);
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
                await _projetoService.Delete(id);
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return Ok();
        }
    }
}
