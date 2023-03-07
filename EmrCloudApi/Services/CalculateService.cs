using Domain.Models.CalculateModel;
using Helper.Enum;
using Interactor.CalculateService;
using System.Text;
using System.Text.Json;

namespace EmrCloudApi.Services
{
    public class CalculateService : ICalculateService
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public CalculateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CallCalculate(CalculateApiPath path, object inputData)
        {
            var jsonContent = JsonSerializer.Serialize(inputData);

            var basePath = _configuration.GetSection("CalculateApi")["BasePath"]!;
            string functionName = string.Empty;
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            switch (path)
            {
                case CalculateApiPath.GetSinMeiList:
                    functionName = "SinMei/GetSinMeiList";
                    break;
                default:
                    throw new NotImplementedException("The Api Path Is Incorrect: " + path.ToString());
            }

            var response = await _httpClient.PostAsync($"{basePath}{functionName}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                return "Failed: " + response.StatusCode;
            }
        }

        public SinMeiDataModelDto GetSinMeiList(CalculateApiPath path, object inputData)
        {
            Task<string> task = CallCalculate(path, inputData);
            if (!task.IsCompletedSuccessfully)
                return new();

            var result = task.Result;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<SinMeiDataModelDto>(result);
        }
    }
}
