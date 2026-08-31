using PayFlow.domain.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PayFlow.application.Helpers;

[ExcludeFromCodeCoverage]
public static class Helpers
{
    public static bool DocumentValidator(string Numero)
    {
        Numero = ApenasNumeros(Numero);

        if (Numero?.Length > 11)
            return false;

        while (Numero?.Length != 11)
            Numero = '0' + Numero;

        var igual = true;
        for (var i = 1; i < 11 && igual; i++)
            if (Numero[i] != Numero[0])
                igual = false;

        if (igual || Numero == "12345678909")
            return false;

        var numeros = new int[11];

        for (var i = 0; i < 11; i++)
            numeros[i] = int.Parse(Numero[i].ToString());

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += (10 - i) * numeros[i];

        var resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[9] != 0)
                return false;
        }
        else if (numeros[9] != 11 - resultado)
            return false;

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += (11 - i) * numeros[i];

        resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[10] != 0)
                return false;
        }
        else if (numeros[10] != 11 - resultado)
            return false;

        return true;
    }

    public static string ApenasNumeros(this string palavra)
    {
        if (palavra == null)
        {
            return "";
        }
        var apenasDigitos = new Regex(@"[^\d]");
        return apenasDigitos.Replace(palavra, "");
    }

    public static bool EmailValidator(this string email)
    {
        try
        {
            if (email.Trim().EndsWith(".")) return false;
            return new System.Net.Mail.MailAddress(email).Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }

    public static void NameValidator(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ExceptionValidation("O nome é obrigatório.");

        if (!Regex.IsMatch(name, @"^[\p{L}\s'-]+$"))
            throw new ExceptionValidation("O nome contém caracteres inválidos.");
    }

}
