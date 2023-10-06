using System.Text.Json.Serialization;

namespace EmrCloudApi.Requests.MainMenu;

public class GetListReportRequest
{
    public GetListReportRequest(string path)
    {
        Path = path;
    }

    [JsonPropertyName("path")]
    public string Path { get; private set; }
}
