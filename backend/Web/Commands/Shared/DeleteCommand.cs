using System;
using MediatR;

namespace Web.Commands.Shared
{
    public class DeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteCommand(Guid id)
        {
            Id = id;
        }
    }
}