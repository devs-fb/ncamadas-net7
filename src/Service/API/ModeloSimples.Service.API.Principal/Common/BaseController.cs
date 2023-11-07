namespace ModeloSimples.Service.API.Principal.Common;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.Interfaces;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntidade, TCriarCommand, TEditarCommand, TRemoverCommand, TObterQuery, TBuscarQuery>
    : ControllerBase
    where TEntidade : class
    where TCriarCommand : IRequest<ResultadoOperacao<TEntidade>>
    where TEditarCommand : IRequest<ResultadoOperacao<TEntidade>>
    where TRemoverCommand : IRequest<ResultadoOperacao<Unit>>
    where TObterQuery : IRequest<ResultadoOperacao<TEntidade>>
    where TBuscarQuery : IRequest<ResultadoOperacao<List<TEntidade>>>
{
    private readonly IMediator _mediator;
    private readonly ICommandFactory<TCriarCommand, TEntidade> _criarCommandFactory;

    protected BaseController(IMediator mediator, ICommandFactory<TCriarCommand, TEntidade> criarCommandFactory)
    {
        _mediator = mediator;
        _criarCommandFactory = criarCommandFactory;
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] TEntidade entidade)
    {
        var comando = _criarCommandFactory.CriarComando(entidade);

        var resultadoDoComando = await _mediator.Send(comando);

        if (resultadoDoComando is ResultadoOperacao<TEntidade> resultado)
        {
            return TratarResposta(resultado);
        }
        else
        {
            return BadRequest(new Resposta<string>("Erro ao executar o comando."));
        }
    }

    [HttpPut("Editar")]
    public async Task<IActionResult> Editar([FromBody] TEntidade entidade)
    {
        if (Activator.CreateInstance(typeof(TEditarCommand), entidade) is not TEditarCommand command)
        {
            return BadRequest(new { Mensagem = "Comando de edição inválido." });
        }

        var resultado = await _mediator.Send(command);
        return TratarResposta(resultado);
    }

    [HttpDelete("Remover/{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        if (Activator.CreateInstance(typeof(TRemoverCommand), id) is not TRemoverCommand command)
        {
            return BadRequest(new { Mensagem = "Comando de remoção inválido." });
        }

        var resultado = await _mediator.Send(command);
        return TratarResposta(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        if (Activator.CreateInstance(typeof(TObterQuery), id) is not TObterQuery query)
        {
            return BadRequest(new { Mensagem = "Consulta de obtenção inválida." });
        }

        var resultado = await _mediator.Send(query);
        return TratarResposta(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] TBuscarQuery query)
    {
        var resultado = await _mediator.Send(query); 
        return TratarResposta(resultado);
    }

    private IActionResult TratarResposta<T>(ResultadoOperacao<T> resultado)
    {
        if (resultado.ComSucesso)
        {
            return Ok(new Resposta<T>(resultado.Dados));
        }

        return BadRequest(new Resposta<string>(resultado.MensagemErro));
    }
}