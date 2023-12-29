namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

/// <summary>
/// Define uma interface para a obtenção assíncrona de uma pessoa.
/// </summary>
public interface IPessoaObterQuery
{
    /// <summary>
    /// Obtém assíncronamente os detalhes de uma pessoa com base no ID fornecido.
    /// </summary>
    /// <param name="pessoaId">O ID da pessoa a ser obtida.</param>
    /// <returns>Um resultado contendo os detalhes da pessoa correspondente ao ID fornecido.</returns>
    Task<ResultadoConsulta<PessoaModel>> ObterPessoaAsync(Guid pessoaId);
}
