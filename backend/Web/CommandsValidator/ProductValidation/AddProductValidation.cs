using FluentValidation;
using Web.Commands.Products;

namespace Web.CommandsValidator.ProductValidation
{
    public class AddProductValidation : AbstractValidator<AddProductCommand>
    {
        public AddProductValidation()
        {
            RuleFor(_ => _.Product.Name).NotEmpty();
            RuleFor(_ => _.Product.Price).NotEmpty();
            RuleFor(_ => _.Product.Size).NotEmpty();
        }
    }
}