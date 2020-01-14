using System.Text.Json.Serialization;

namespace Blockcoli.Libra.Net.Common
{
    public class FaucetResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }
    }
}