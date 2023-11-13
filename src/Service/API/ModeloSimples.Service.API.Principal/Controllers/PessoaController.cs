namespace ModeloSimples.Service.API.Principal.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModeloSimples.Application.Commands;
using ModeloSimples.Application.Queries;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Service.API.Principal.Common;

[Route(ConstantGlobal.RouteApiController)]
[ApiController]
public class PessoaController : SingleBaseController 
{
    private const string NenhumaPessoaEncontrada = "Nenhuma pessoa encontrada.";
    private const string ErroAoExecutarOComando = "Erro ao executar o comando.";
    private const string HttpPostCriar = "Criar";
    private const string HttpPutEditar = "Editar";
    private const string HttpPostBuscar = "Buscar";
    private const string HttpGetPessoaId = "{pessoaId}";
    private const string HttpDeletePessoaId = "{pessoaId}";
    private const string HttpGetAuditarPessoaId = "Auditar/{pessoaId}";
    private const string HttpPostBloquear = "Bloquear";
    private const string HttpPostDesbloquear = "Desbloquear";

    private readonly IMediator _mediator;

    public PessoaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(HttpPostCriar)]
    public async Task<IActionResult> Criar([FromBody] PessoaModel entidade)
    {
        var comando = new PessoaCriarCommand(entidade);

        var resultadoDoComando = await _mediator.Send(comando);

        if (resultadoDoComando is ResultadoOperacao<PessoaModel> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return BadRequest(new Resposta<string>(ErroAoExecutarOComando));
        }
    }

    [HttpPut(HttpPutEditar)]
    public async Task<IActionResult> Editar(Guid id, [FromBody] PessoaModel entidade)
    {
        var comando = new PessoaEditarCommand(id, entidade);

        var resultadoDoComando = await _mediator.Send(comando);

        if (resultadoDoComando is ResultadoOperacao<PessoaModel> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return BadRequest(new Resposta<string>(ErroAoExecutarOComando));
        }
    }

    [HttpPost(HttpPostBuscar)]
    public async Task<IActionResult> Buscar([FromBody] PessoasBuscarCommandQuery consulta)
    {
        var resultadoConsulta = await _mediator.Send(consulta);

        if (resultadoConsulta is ResultadoConsulta<IEnumerable<PessoaModel>> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return NotFound(new Resposta<string>(NenhumaPessoaEncontrada));
        }
    }

    [HttpGet(HttpGetPessoaId)]
    public async Task<IActionResult> Obter(Guid pessoaId)
    {
        var consulta = new PessoaObterCommandQuery(pessoaId);
        var resultadoConsulta = await _mediator.Send(consulta);

        if (resultadoConsulta is ResultadoConsulta<PessoaModel> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return NotFound(new Resposta<string>(NenhumaPessoaEncontrada));
        }
    }

    [HttpDelete(HttpDeletePessoaId)]
    public async Task<IActionResult> Remover(Guid pessoaId)
    {
        var comando = new PessoaRemoverCommand(pessoaId);
        var resultadoComando = await _mediator.Send(comando);

        if (resultadoComando is ResultadoOperacao<Unit> resultado)
        {
            return NoContent();
        }
        else
        {
            return NotFound(new Resposta<string>(NenhumaPessoaEncontrada));
        }
    }

    [HttpGet(HttpGetAuditarPessoaId)]
    public async Task<IActionResult> Auditar(Guid pessoaId)
    {
        var consulta = new PessoaAuditarCommandQuery(pessoaId);
        var resultadoConsulta = await _mediator.Send(consulta);

        if (resultadoConsulta is ResultadoConsulta<List<PessoaAuditadaModel>> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return NotFound(new Resposta<string>(NenhumaPessoaEncontrada));
        }
    }

    [HttpPost(HttpPostBloquear)]
    public async Task<IActionResult> Bloquear(Guid pessoaId)
    {
        var comando = new PessoaBloquearCommand(pessoaId);

        var resultadoDoComando = await _mediator.Send(comando);

        if (resultadoDoComando is ResultadoOperacao<bool> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return BadRequest(new Resposta<string>(ErroAoExecutarOComando));
        }
    }

    [HttpPost(HttpPostDesbloquear)]
    public async Task<IActionResult> Desbloquear(Guid pessoaId)
    {
        var comando = new PessoaDesbloquearCommand(pessoaId);

        var resultadoDoComando = await _mediator.Send(comando);

        if (resultadoDoComando is ResultadoOperacao<bool> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return BadRequest(new Resposta<string>(ErroAoExecutarOComando));
        }
    }
}