using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SuperAdmin.Responses
{
    public class Response<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}
