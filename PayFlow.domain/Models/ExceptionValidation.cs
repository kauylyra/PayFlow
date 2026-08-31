using System.ComponentModel.DataAnnotations;

namespace PayFlow.domain.Models;

public class ExceptionValidation: ValidationException
{
    public ExceptionValidation(string message) : base(message){}
}
