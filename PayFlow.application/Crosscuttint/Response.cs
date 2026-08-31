using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.application.Crosscuttint
{
    public class Response<TResultado>
    {
        [JsonProperty("message")]

        public string Message { get; set; } = "Operação realizada com sucesso!";

        [JsonProperty("result")]

        public TResultado result { get; set; }
    }
}
