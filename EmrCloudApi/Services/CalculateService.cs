using Domain.Models.CalculateModel;
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

        public async Task<string> CallCalculate(string apiUrl, object inputData)
        {
            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonSerializer.Serialize(inputData);

                var basePath = _configuration.GetSection("CalculateApi")["BasePath"]!;

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{basePath}{apiUrl}", content);

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
        }


        public SinMeiDataModelDto GetSinMeiList(string apiUrl, object inputData)
        {
            Task<string> task = CallCalculate(apiUrl, inputData);

            var result = task.Result;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<SinMeiDataModelDto>(result);
        }
    }
}
