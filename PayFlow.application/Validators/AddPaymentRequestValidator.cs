using FluentValidation;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;

namespace PayFlow.application.Validators;

public sealed class AddPaymentRequestValidator : AbstractValidator<AddPaymentRequest>
{
    public AddPaymentRequestValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).Must(amount => amount == Math.Round(amount, 2)).WithMessage("O valor deve possuir no máximo 2 casas decimais.");
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
