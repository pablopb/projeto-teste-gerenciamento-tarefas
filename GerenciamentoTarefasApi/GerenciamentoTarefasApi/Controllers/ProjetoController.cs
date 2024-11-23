using GerenciamentoTarefasApi.Commands;
using GerenciamentoTarefasApi.DTOs;
using GerenciamentoTarefasApi.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoTarefasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjetoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-projeto")]
        public async Task<IActionResult> CreateProjeto([FromBody] ProjetoDto projetoDto)
        {
            if (projetoDto == null)
            {
                return BadRequest("Projeto não pode ser nulo.");
            }

            var comando = new HandleCreateProjectCommand(projetoDto);
            var projetoCriado = await _mediator.Send(comando);

            return CreatedAtAction(nameof(CreateProjeto), new { id = projetoCriado.Id }, projetoCriado);
        }

        [HttpGet("get-projetos-by-userId")]
        public async Task<ActionResult<List<ProjetoDto>>> GetProjetosByUserid(int userId)
        {
            var projetos = await _mediator.Send(new GetProjetosQuery(userId));
            return Ok(projetos);
        }

        [HttpPost("delete-projeto")]
        public async Task<IActionResult> DeletProjeto(int projetoId)
        {
            try
            {
                var comando = new HandleDeleteProjetoCommand(projetoId);
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
