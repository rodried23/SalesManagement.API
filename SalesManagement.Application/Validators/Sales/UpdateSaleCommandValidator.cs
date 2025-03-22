using FluentValidation;
using SalesManagement.Application.Commands.Sales;

namespace SalesManagement.Application.Validators.Sales
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da venda é obrigatório");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("A venda deve ter pelo menos um item");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Id)
                    .NotEmpty().WithMessage("O ID do item é obrigatório");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero")
                    .LessThanOrEqualTo(20).WithMessage("Não é possível vender mais de 20 itens idênticos");
            });
        }
    }
}
