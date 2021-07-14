using MediatR;

namespace Infrastructure.CQRS.Abstractions
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}