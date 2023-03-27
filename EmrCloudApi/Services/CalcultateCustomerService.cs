using Helper;
using Interactor.CalculateService;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EmrCloudApi.Services
{
    public class CalcultateCustomerService : ICalcultateCustomerService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public CalcultateCustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("CalculateApi")["BasePath"] ?? "");
        }

        public async Task<CalcultateCustomerResponse<T>> RunCaculationPostAsync<T>(TypeCalculate type, object input)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync(type.GetDescription(), content);
                result.EnsureSuccessStatusCode();

                if (result.IsSuccessStatusCode)
                {
                    string resultContentStr = await result.Content.ReadAsStringAsync();
                    var resultContent = JsonConvert.DeserializeObject<T>(resultContentStr);
                    return new CalcultateCustomerResponse<T>(resultContent ?? Activator.CreateInstance<T>(), result.StatusCode);
                }
                else return new CalcultateCustomerResponse<T>(Activator.CreateInstance<T>(), result.StatusCode);
            }
            catch
            {
                return new CalcultateCustomerResponse<T>(Activator.CreateInstance<T>(), HttpStatusCode.BadRequest);
            }
        }
    }
}
