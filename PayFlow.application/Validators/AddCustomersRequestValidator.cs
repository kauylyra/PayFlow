using FluentValidation;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Helpers;

namespace PayFlow.application.Validators;

public sealed class AddCustomersRequestValidator : AbstractValidator<AddCustomersRequest>
{
    public AddCustomersRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 150).Matches(@"^[\p{L}\s'-]+$").WithMessage("O nome contém caracteres inválidos.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Document).NotEmpty().Must(PayFlow.application.Helpers.Helpers.DocumentValidator).WithMessage("Documento inválido.");
    }
}
