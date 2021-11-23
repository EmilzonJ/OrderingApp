using System;
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
        private readonly IMapper _mapper;
        private readonly IIdentityGenerator<Guid> _identityGenerator;
        private readonly IMediator _mediator;

        public AddProductCommandHandler(IMapper mapper, IIdentityGenerator<Guid> identityGenerator, IMediator mediator, IUnitOfWork db)
        {
            _mapper = mapper;
            _identityGenerator = identityGenerator;
            _mediator = mediator;
            _db = db;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            product.Id = _identityGenerator.Generate();
            await _db.Products.AddAsync(product);
            await _db.SaveAsync();
            return await Unit.Task;
        }
    }
}