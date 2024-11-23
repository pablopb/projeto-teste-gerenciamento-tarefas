using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using GerenciamentoTarefasApi.DTOs;
using GerenciamentoTarefasApi.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoTarefasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TarefaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-tarefas-by-projetoId")]
        public async Task<ActionResult<List<ProjetoDto>>> GetTarefasByProjetoId(int projetoId)
        {
            var tarefas = await _mediator.Send(new GetTarefasByProjetoQuery(projetoId));
            return Ok(tarefas);
        }

        [HttpGet("get-media-tarefas-by-user")]
        public async Task<ActionResult<List<ProjetoDto>>> GetMediaTarefasPorUsuario(int userId)
        {
            var usuario = await _mediator.Send(new GetUserByIdQuery(userId));
            if (usuario == null || usuario.Perfil != Perfil.Gerente)
                return Unauthorized("Usuário não autorizao a visualizar este relatório");
            var media = await _mediator.Send(new GetMediaTarefasPorUsuarioQuery());
            return Ok(new { Media = media });
        }

        [HttpPost("create-tarefa")]
        public async Task<IActionResult> CreateTarefa([FromBody] TarefaDto tarefaDto)
        {
            try
            {
                if (tarefaDto == null)
                {
                    return BadRequest("Tarefa não pode ser nula.");
                }

                var comando = new HandleCreateTarefaCommand(tarefaDto);
                var tarefaCriada = await _mediator.Send(comando);

                return CreatedAtAction(nameof(CreateTarefa), new { id = tarefaCriada.Id }, tarefaCriada);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
 
        }


        [HttpPost("update-tarefa")]
        public async Task<IActionResult> UpdateTarefa([FromBody] TarefaDto tarefaDto)
        {
            try
            {
                if (tarefaDto == null)
                {
                    return BadRequest("Tarefa não pode ser nula.");
                }

                var comando = new HandleUpdateTarefaCommand(tarefaDto);
                var tarefa = await _mediator.Send(comando);

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-tarefa")]
        public async Task<IActionResult> DeletTarefa(int tarefaId)
        {
            try
            {
               

                var comando = new HandleDeleteTarefaCommand(tarefaId);
                await _mediator.Send(comando);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
