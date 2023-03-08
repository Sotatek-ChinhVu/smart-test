using Domain.Models.CalculateModel;
using Helper.Enum;
using Interactor.CalculateService;

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
            var content = JsonContent.Create(inputData);

            var basePath = _configuration.GetSection("CalculateApi")["BasePath"]!;
            string functionName = string.Empty;
            switch (path)
            {
                case CalculateApiPath.GetSinMeiList:
                    functionName = "SinMei/GetSinMeiList";
                    break;
                default:
                    throw new NotImplementedException("The Api Path Is Incorrect: " + path.ToString());
            }

            try
            {
                var response = await _httpClient.PostAsync($"{basePath}{functionName}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }

                return response.StatusCode.ToString();

            }
            catch (HttpRequestException ex)
            {
                return "Failed: Could not connect to Calculate API";
            }
        }

        public SinMeiDataModelDto GetSinMeiList(CalculateApiPath path, object inputData)
        {
            try
            {
                Task<string> task = CallCalculate(path, inputData);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SinMeiDataModelDto>(task.Result);
                return result;
            }
            catch (Exception ex)
            {
                return new();
            }
        }
    }
}
