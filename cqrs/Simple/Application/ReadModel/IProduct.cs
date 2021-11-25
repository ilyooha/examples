using System;

namespace Simple.Application.ReadModel
{
    public interface IProduct
    {
        Guid Id { get; }
        string Title { get; }
    }
}