using Newtonsoft.Json;

namespace PayFlow.application.Crosscuttint.Dtos;

public class PaginationResponse<TResultado>
{
    [JsonProperty("mensagem")]

    public string Mensagem { get; set; } = "Operação realizada com sucesso!";

    [JsonProperty("resultado")]

    public TResultado Resultado { get; set; }

    public int TamanhoPagina { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPaginas
    {
        get
        {
            return (TotalRegistros + TamanhoPagina - 1) / TamanhoPagina;
        }
    }

    public int TotalRegistros { get; set; }
    /// <summary>
    /// A página da paginação da pesquisa
    /// </summary>
    public int pagina { get; set; }

    //retirado devido as novas regras de negocio
    //public bool TemConteudo
    //{
    //    get
    //    {
    //        // Verifica se o tipo implementa IEnumerable
    //        if (Resultado is IEnumerable enumerable)
    //        {
    //            // Verifica se a coleção tem algum conteúdo
    //            foreach (var item in enumerable)
    //                return true; // Se encontrar algum item, retorna true
    //            return false; // Se a coleção estiver vazia, retorna false
    //        }
    //        return Resultado is not null; // Não é uma coleção
    //    }
    //    set { value = true; }
    //}
}
