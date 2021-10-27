using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Web.Commands.Products;
using Domain.Entities;
using Domain.Repositories;
using Web.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Web.Commands.Shared;
using Web.DTOs.Products.Responses;
using Web.Errors;
using Web.Hubs;

namespace Web.CommandHandlers.Products
{
    public class ProductHandler :
        IRequestHandler<GetProductsCommand, IEnumerable<GetProductResponse>>,
        IRequestHandler<GetProductByIdCommand, GetProductResponse>,
        IRequestHandler<AddProductCommand, AddProductReponse>,
        IRequestHandler<UpdateProductCommand, GetProductResponse>,
        IRequestHandler<DeleteCommand, bool>
    {
        private readonly IWritableRepository<Product, Guid> _writableRepository;
        private readonly IReadOnlyRepository<Product, Guid> _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ProductHub> _productHubContext;
        private readonly IIdentityGenerator<Guid> _identityGenerator;

        public ProductHandler(IWritableRepository<Product, Guid> writableRepository,
            IReadOnlyRepository<Product, Guid> readOnlyRepository, IIdentityGenerator<Guid> identityGenerator,
            IMapper mapper, IHubContext<ProductHub> productHubContext)
        {
            _writableRepository = writableRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _productHubContext = productHubContext;
            _identityGenerator = identityGenerator;
        }

        public async Task<IEnumerable<GetProductResponse>> Handle(GetProductsCommand request,
            CancellationToken cancellationToken)
        {
            var data = await _readOnlyRepository.GetAll();
            var products = data.ToList();

            if (!products.Any())
                throw new
                    ApiException(HttpStatusCode.NotFound, new {message = "No existen productos"});
            var response = _mapper.Map<IEnumerable<GetProductResponse>>(products);
            return response;
        }

        public async Task<GetProductResponse> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _readOnlyRepository.GetById(request.Id);
            if (product == null || product.IsDeleted)
                throw new ApiException(HttpStatusCode.NotFound,
                    new {message = $"No existe el producto con Id = {request.Id}"});

            return _mapper.Map<GetProductResponse>(product);
        }

        public async Task<AddProductReponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.Product);
            var id = _identityGenerator.Generate();
            product.Id = id;
            var response = await _writableRepository.Create(product);
            
            await _productHubContext.Clients.All.SendAsync("ReceiveProduct", response, cancellationToken);

            return _mapper.Map<AddProductReponse>(response);
        }

        public async Task<GetProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            var response = await _writableRepository.Update(product);

            if (response == null)
                throw new ApiException(HttpStatusCode.NotFound,
                    new {message = $"No existe el producto con Id = {request.Product.Id}"});

            await _productHubContext.Clients.All.SendAsync("UpdateProduct", response, cancellationToken);

            return _mapper.Map<GetProductResponse>(response);
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