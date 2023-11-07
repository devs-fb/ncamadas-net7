namespace ModeloSimples.Infrastructure.Shared.Interfaces;

using MediatR;
using ModeloSimples.Infrastructure.Shared.Common;

public interface ICommand<T> : IRequest<ResultadoOperacao<T>> where T : class
{
}
