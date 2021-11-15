using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Notifications.AddProduct;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Web.Services.Interfaces;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IWritableRepository<Product, Guid> _productRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityGenerator<Guid> _identityGenerator;
        private readonly IMediator _mediator;

        public AddProductCommandHandler(IWritableRepository<Product, Guid> productRepository, IMapper mapper, IIdentityGenerator<Guid> identityGenerator, IMediator mediator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _identityGenerator = identityGenerator;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            product.Id = _identityGenerator.Generate();
            await _productRepository.AddAsync(product);

            await _mediator.Publish(new AddProductNotification(product), cancellationToken);
            
            return await Unit.Task;
        }
    }
}