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
        public UpdateDataTenantResult(bool done, string currentItem, int length, int successCount, string message, byte status)
        {
            Done = done;
            CurrentItem = currentItem;
            Length = length;
            SuccessCount = successCount;
            Message = message;
            Status = status;
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

        [JsonPropertyName("status")]

        // 1: running, 0 fail
        public byte Status { get; private set; }
    }
}
