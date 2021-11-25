using System;
using MediatR;

namespace Simple.Application.WriteModel
{
    public class RegisterProductCommand : IRequest<Guid>, ICommand
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
    }
}