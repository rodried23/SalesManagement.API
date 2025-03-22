using FluentValidation;
using SalesManagement.Application.Commands.Sales;

namespace SalesManagement.Application.Validators.Sales
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("O ID do cliente é obrigatório");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório")
                .MaximumLength(100).WithMessage("O nome do cliente não pode exceder 100 caracteres");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("O ID da filial é obrigatório");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("O nome da filial é obrigatório")
                .MaximumLength(100).WithMessage("O nome da filial não pode exceder 100 caracteres");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("A venda deve ter pelo menos um item");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("O ID do produto é obrigatório");

                item.RuleFor(i => i.ProductName)
                    .NotEmpty().WithMessage("O nome do produto é obrigatório")
                    .MaximumLength(100).WithMessage("O nome do produto não pode exceder 100 caracteres");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero")
                    .LessThanOrEqualTo(20).WithMessage("Não é possível vender mais de 20 itens idênticos");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero");
            });
        }
    }
}
