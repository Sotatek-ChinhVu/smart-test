using Microsoft.Extensions.Configuration;
using Reporting.ReadRseReportFile.Model;
using System.Text;
using System.Text.Json;

namespace Reporting.ReadRseReportFile.Service;

public class ReadRseReportFileService : IReadRseReportFileService
{
    private static HttpClient _httpClient = new();
    private readonly IConfiguration _configuration;

    public ReadRseReportFileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JavaOutputData ReadFileRse(CoCalculateRequestModel inputModel)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        using (HttpResponseMessage response = _httpClient.PostAsync($"{basePath}{"calculate-param"}", jsonContent).Result)
        {
            response.EnsureSuccessStatusCode();
            var contentResult = response.Content.ReadAsStringAsync().Result;
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JavaOutputData>(contentResult);
            return result ?? new();
        }
    }

    public bool CheckExistTemplate(CheckExistTemplateRequestModel inputModel)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        using (HttpResponseMessage response = _httpClient.PostAsync($"{basePath}{"checkExistTemplate"}", jsonContent).Result)
        {
            response.EnsureSuccessStatusCode();
            var contentResult = response.Content.ReadAsStringAsync().Result;
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(contentResult);
            return result;
        }
    }
}
