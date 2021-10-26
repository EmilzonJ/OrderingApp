using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Web.Commands.Products;
using Domain.Entities;
using Domain.Repositories;
using Web.Services.Interfaces;
using MediatR;
using Web.Commands.Shared;
using Web.Errors;

namespace Web.CommandHandlers.Products
{
    public class ProductHandler :
        IRequestHandler<GetProductsCommand, IEnumerable<Product>>,
        IRequestHandler<GetProductByIdCommand, Product>,
        IRequestHandler<AddProductCommand, Product>,
        IRequestHandler<UpdateProductCommand, Product>,
        IRequestHandler<DeleteCommand, bool>
    {
        private readonly IWritableRepository<Product, Guid> _writableRepository;
        private readonly IReadOnlyRepository<Product, Guid> _readOnlyRepository;
        private readonly IIdentityGenerator<Guid> _identityGenerator;

        public ProductHandler(IWritableRepository<Product, Guid> writableRepository,
            IReadOnlyRepository<Product, Guid> readOnlyRepository, IIdentityGenerator<Guid> identityGenerator)
        {
            _writableRepository = writableRepository;
            _readOnlyRepository = readOnlyRepository;
            _identityGenerator = identityGenerator;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var data = await _readOnlyRepository.GetAll();

            var products = data.ToList();

            if (!products.Any())
                throw new
                    ApiException(HttpStatusCode.NotFound, new {message = "No existen productos"});

            return products;
        }

        public async Task<Product> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _readOnlyRepository.GetById(request.Id);

            if (product == null)
                throw new
                    ApiException(HttpStatusCode.NotFound,
                        new {message = $"No existe el producto con Id = {request.Id}"});

            return product;
        }

        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var id = _identityGenerator.Generate();
            request.Product.Id = id;

            return await _writableRepository.Create(request.Product);
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await _writableRepository.Update(request.Product);
        }

        public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _writableRepository.Delete(request.Id, softDelete: true);

            if (!deleted)
                throw new
                    ApiException(HttpStatusCode.InternalServerError,
                        new {Error = "Error de servidor, producto no eliminado"});

            return true;
        }
    }
}