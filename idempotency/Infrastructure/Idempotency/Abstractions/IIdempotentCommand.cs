using Infrastructure.CQRS.Abstractions;

namespace Infrastructure.Idempotency.Abstractions
{
    public interface IIdempotentCommand<TResult> : ICommand
    {
        string CommandTypeId { get; }

        string SerializeResult(TResult input);
        TResult DeserializeResult(string input);
    }
}