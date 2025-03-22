using FluentValidation;
using SalesManagement.Application.Commands.Products;

namespace SalesManagement.Application.Validators.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("A categoria é obrigatória")
                .MaximumLength(50).WithMessage("A categoria não pode exceder 50 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero");
        }
    }
}
