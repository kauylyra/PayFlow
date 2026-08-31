using System.ComponentModel.DataAnnotations;

namespace PayFlow.application.Crosscuttint.Dtos.Request.Customers;

public class UpdateCustomerRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, MinimumLength = 3,
        ErrorMessage = "O nome deve conter entre 3 e 150 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O documento é obrigatório.")]
    public string Document { get; set; }
}