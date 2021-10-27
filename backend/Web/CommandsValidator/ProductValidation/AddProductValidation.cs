using FluentValidation;
using Web.Commands.Products;

namespace Web.CommandsValidator.ProductValidation
{
    public class AddProductValidation : AbstractValidator<AddProductCommand>
    {
        public AddProductValidation()
        {
            RuleFor(_ => _.Product.Name).NotEmpty().NotNull();
            RuleFor(_ => _.Product.Price).NotEmpty().NotNull();
            RuleFor(_ => _.Product.Size).NotEmpty().NotNull();
        }
    }
}