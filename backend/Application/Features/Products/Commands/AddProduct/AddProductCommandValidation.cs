using Application.Commands.Products;
using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandValidation : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidation()
        {
            RuleFor(_ => _.Name)
                .NotEmpty().WithMessage("El campo Nombre del producto no puede ser vacío")
                .NotNull();

            RuleFor(_ => _.Price)
                .NotEmpty().WithMessage("El campo Precio del producto no puede ser vacío")
                .NotNull();

            RuleFor(_ => _.Size)
                .NotEmpty().WithMessage("El campo Size del producto no puede ser vacío")
                .NotNull();
        }
    }
}