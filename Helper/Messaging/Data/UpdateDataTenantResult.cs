using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helper.Messaging.Data
{
    public class UpdateDataTenantResult
    {
        public UpdateDataTenantResult(bool done, string currentItem, int length, int successCount, string message, string uniqueKey)
        {
            Done = done;
            CurrentItem = currentItem;
            Length = length;
            SuccessCount = successCount;
            Message = message;
            UniqueKey = uniqueKey;
        }

        [JsonPropertyName("done")]
        public bool Done { get; private set; }

        [JsonPropertyName("currentItem")]
        public string CurrentItem { get; private set; }

        [JsonPropertyName("length")]
        public int Length { get; private set; }

        [JsonPropertyName("successCount")]
        public int SuccessCount { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        [JsonPropertyName("uniqueKey")]
        public string UniqueKey { get; private set; }
    }
}
