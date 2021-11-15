using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Products;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IWritableRepository<Product, Guid> _productRepository;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IWritableRepository<Product, Guid> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            _productRepository.AddAsync(product);
            return Unit.Task;
        }
    }
}