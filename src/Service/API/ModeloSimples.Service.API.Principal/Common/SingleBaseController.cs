namespace ModeloSimples.Service.API.Principal.Common;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public class SingleBaseController : ControllerBase
{
    private const string ErroAoExecutarOComando = "Erro ao executar o comando.";

    //private readonly IMediator _mediator;

    //public SingleBaseController(IMediator mediator)
    //{
    //    _mediator = mediator;
    //}

    protected IActionResult TratarResposta<T>(ResultadoOperacao<T> resultado)
    {
        if (resultado.ComSucesso)
        {
            return Ok(new Resposta<T>(resultado.Dados));
        }

        return BadRequest(new Resposta<string>(resultado.MensagemErro));
    }

    protected IActionResult TratarResposta<T>(ResultadoConsulta<T> resultado)
    {
        if (resultado.ComSucesso)
        {
            return Ok(new Resposta<T>(resultado.Dados));
        }

        return BadRequest(new Resposta<string>(resultado.MensagemErro));
    }

    //TODO melhorar a base para permitir uma linha na controller
    // ex.: [HttpPost(HttpPostCriar)] public async Task<IActionResult> Criar([FromBody] PessoaModel entidade) => await SendCommand<PessoaModel, PessoaCriarCommand, ResultadoOperacao<PessoaModel>>(new(entidade));
    //protected async Task<IActionResult> SendCommand<T,TC,R>(TC Command)
    //{
    //    var result = await _mediator.Send(Command);

    //    if (result is R r)
    //    {
    //        if (r is ResultadoConsulta<T> consult)
    //        {
    //            return TratarResposta(consult);
    //        }

    //        if (r is ResultadoOperacao<T> operation)
    //        {
    //            return TratarResposta(operation);
    //        }
    //    }

    //    return BadRequest(new Resposta<string>(ErroAoExecutarOComando));
    //}
}
