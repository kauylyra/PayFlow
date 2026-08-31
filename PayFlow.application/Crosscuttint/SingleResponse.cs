using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PayFlow.application.Crosscuttint;

public class SingleResponse
{
    [JsonProperty("mensagem")]
    public string Mensagem { get; set; } = "Operação realizada com sucesso!";
    [JsonProperty("resultado")]
    public List<string> Resultado { get; set; } = new List<string>();

    public StatusCodeResult StatusCode { get; set; }
}
