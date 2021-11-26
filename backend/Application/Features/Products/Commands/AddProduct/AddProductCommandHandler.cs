using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Notifications.AddProduct;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Web.Services.Interfaces;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IUnitOfWork _db;
        private readonly IIdentityGenerator<Guid> _identityGenerator;

        public AddProductCommandHandler(IIdentityGenerator<Guid> identityGenerator, IUnitOfWork db)
        {
            _identityGenerator = identityGenerator;
            _db = db;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var id = _identityGenerator.Generate();
            var product = Product.Create(id, request.Name, request.Price, request.Size);
            await _db.Products.AddAsync(product);
            await _db.SaveAsync();
            return await Unit.Task;
        }
    }
}