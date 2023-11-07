namespace ModeloSimples.Service.API.Principal.Common;

using Microsoft.AspNetCore.Mvc;
using ModeloSimples.Infrastructure.Shared.Common;

public class SingleBaseController : ControllerBase
{
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
}
