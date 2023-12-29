namespace ModeloSimples.Service.API.Principal.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModeloSimples.Application.Commands;
using ModeloSimples.Application.Queries;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.DTO.CommandQuery.LGPD;
using ModeloSimples.Service.API.Principal.Common;

/// <summary>
/// Controller responsável pelas operações relacionadas a Pessoa.
/// </summary>
[ApiExplorerSettings(GroupName = "LGPD", IgnoreApi = false)]
[Route(ConstantGlobal.RouteApiController)]
[ApiController]
public class PessoaController : SingleBaseController 
{
    private const string HttpPostCriar = "Criar";
    private const string HttpPutEditar = "Editar";
    private const string HttpPostBuscar = "Buscar";
    private const string HttpGetPessoaId = "{pessoaId}";
    private const string HttpDeletePessoaId = "{pessoaId}";
    private const string HttpGetAuditarPessoaId = "Auditar/{pessoaId}";
    private const string HttpPostBloquear = "Bloquear";
    private const string HttpPostDesbloquear = "Desbloquear";

    public PessoaController(IMediator mediator, ILogger<PessoaController> logger) : base(mediator, logger)
    {
    }

    /// <summary>
    /// Cria uma nova Pessoa com base nos dados fornecidos.
    /// </summary>
    /// <remarks>
    /// Endpoint para criar uma nova entidade de Pessoa com base nos dados fornecidos.
    /// </remarks>
    /// <param name="entidade">Objeto contendo informações da Pessoa a serem criadas. Os campos específicos variam de acordo com o tipo de Pessoa (Física ou Jurídica).</param>
    /// <returns>O resultado da operação. Retorna a nova Pessoa criada em caso de sucesso.</returns>
    /// <response code="200">Retorna a nova Pessoa criada.</response>
    /// <response code="204">Se a operação não retornar conteúdo.</response>
    /// <response code="302">Se a operação for redirecionada para outro recurso.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [HttpPost(HttpPostCriar), ProducesResponseType(typeof(Resposta<PessoaModel>), StatusCodes.Status200OK), ProducesResponseType(typeof(Resposta<string>), StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status302Found), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Criar([FromBody] PessoaModel entidade) 
        => await ExecutarComando(() => new PessoaCriarCommand(entidade));

    /// <summary>
    /// Edita uma Pessoa existente com base nos dados fornecidos.
    /// </summary>
    /// <remarks>
    /// Endpoint para editar uma entidade de Pessoa existente com base nos dados fornecidos.
    /// </remarks>
    /// <param name="id">O identificador único da Pessoa a ser editada.</param>
    /// <param name="entidade">Objeto contendo informações da Pessoa a serem atualizadas.</param>
    /// <returns>O resultado da operação. Retorna a Pessoa editada em caso de sucesso.</returns>
    /// <response code="200">Retorna a Pessoa editada.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada.</response>
    /// <response code="204">Se a operação não retornar conteúdo.</response>
    /// <response code="302">Se a operação for redirecionada para outro recurso.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpPut(HttpPutEditar), ProducesResponseType(typeof(Resposta<PessoaModel>), StatusCodes.Status200OK), ProducesResponseType(typeof(Resposta<string>), StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status302Found), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Editar([FromQuery] Guid id, [FromBody] PessoaModel entidade) => await ExecutarComando(() => new PessoaEditarCommand(id, entidade));

    /// <summary>
    /// Busca uma lista de Pessoas com base nos critérios fornecidos.
    /// </summary>
    /// <remarks>
    /// Endpoint para buscar uma lista de Pessoas com base nos critérios fornecidos.
    /// </remarks>
    /// <param name="consulta">Objeto contendo os critérios de busca das Pessoas.</param>
    /// <returns>O resultado da operação. Retorna a lista de Pessoas encontradas em caso de sucesso.</returns>
    /// <response code="200">Retorna a lista de Pessoas encontradas.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se nenhuma Pessoa for encontrada com os critérios fornecidos.</response>
    /// <response code="204">Se a operação não retornar conteúdo.</response>
    /// <response code="302">Se a operação for redirecionada para outro recurso.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpPost(HttpPostBuscar)] 
    public async Task<IActionResult> Buscar([FromBody] IPessoasBuscarCommandQuery consulta) => await ExecutarComando(() => consulta);

    /// <summary>
    /// Obtém os detalhes de uma Pessoa com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Endpoint para obter os detalhes de uma Pessoa com base no ID fornecido.
    /// </remarks>
    /// <param name="pessoaId">Identificador único da Pessoa.</param>
    /// <returns>O resultado da operação. Retorna os detalhes da Pessoa encontrada em caso de sucesso.</returns>
    /// <response code="200">Retorna os detalhes da Pessoa encontrada.</response>
    /// <response code="204">Se a operação não retornar conteúdo.</response>
    /// <response code="302">Se a operação for redirecionada para outro recurso.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada com o ID fornecido.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpGet(HttpGetPessoaId)] 
    public async Task<IActionResult> Obter([FromQuery] Guid pessoaId) => await ExecutarComando(() => new PessoaObterCommandQuery(pessoaId));

    /// <summary>
    /// Remove uma Pessoa com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Endpoint para remover uma Pessoa com base no ID fornecido.
    /// </remarks>
    /// <param name="pessoaId">Identificador único da Pessoa a ser removida.</param>
    /// <returns>O resultado da operação. Retorna um status 'NoContent' em caso de remoção bem-sucedida.</returns>
    /// <response code="204">Retorna um status 'NoContent' indicando que a Pessoa foi removida com sucesso.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada com o ID fornecido.</response>

    [HttpDelete(HttpDeletePessoaId)] 
    public async Task<IActionResult> Remover([FromQuery] Guid pessoaId) => await ExecutarComando(() => new PessoaRemoverCommand(pessoaId));

    /// <summary>
    /// Realiza a auditoria de uma Pessoa com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Endpoint para realizar a auditoria de uma Pessoa com base no ID fornecido.
    /// </remarks>
    /// <param name="pessoaId">Identificador único da Pessoa a ser auditada.</param>
    /// <returns>O resultado da operação. Retorna a lista de auditoria da Pessoa em caso de sucesso.</returns>
    /// <response code="200">Retorna a lista de auditoria da Pessoa.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada com o ID fornecido.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpGet(HttpGetAuditarPessoaId)] 
    public async Task<IActionResult> Auditar([FromQuery] Guid pessoaId) => await ExecutarComando(() => new PessoaAuditarCommandQuery(pessoaId));

    /// <summary>
    /// Bloqueia uma Pessoa com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Endpoint para bloquear uma Pessoa com base no ID fornecido.
    /// </remarks>
    /// <param name="pessoaId">Identificador único da Pessoa a ser bloqueada.</param>
    /// <returns>O resultado da operação. Retorna um valor booleano indicando o sucesso do bloqueio.</returns>
    /// <response code="200">Retorna um valor booleano indicando o sucesso do bloqueio.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada com o ID fornecido.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpPost(HttpPostBloquear)] 
    public async Task<IActionResult> Bloquear([FromQuery] Guid pessoaId) => await ExecutarComando(() => new PessoaBloquearCommand(pessoaId));

    /// <summary>
    /// Desbloqueia uma Pessoa com base no ID fornecido.
    /// </summary>
    /// <remarks>
    /// Endpoint para desbloquear uma Pessoa com base no ID fornecido.
    /// </remarks>
    /// <param name="pessoaId">Identificador único da Pessoa a ser desbloqueada.</param>
    /// <returns>O resultado da operação. Retorna um valor booleano indicando o sucesso do desbloqueio.</returns>
    /// <response code="200">Retorna um valor booleano indicando o sucesso do desbloqueio.</response>
    /// <response code="400">Se ocorrer um erro ao executar o comando.</response>
    /// <response code="404">Se a Pessoa não for encontrada com o ID fornecido.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>

    [HttpPut(HttpPostDesbloquear)] 
    public async Task<IActionResult> Desbloquear([FromQuery] Guid pessoaId) => await ExecutarComando(() => new PessoaDesbloquearCommand(pessoaId));

}