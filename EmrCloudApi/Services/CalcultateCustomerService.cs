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

        public CalcultateCustomerService(IConfiguration configuration)
        {
            _httpClient.BaseAddress = new Uri(configuration.GetSection("CalculateApi")["BasePath"] ?? "");
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
                    T resultContent = JsonConvert.DeserializeObject<T>(resultContentStr) ?? Activator.CreateInstance<T>();
                    return new CalcultateCustomerResponse<T>(resultContent, result.StatusCode, result.IsSuccessStatusCode);
                }
                else return new CalcultateCustomerResponse<T>(Activator.CreateInstance<T>(), result.StatusCode, result.IsSuccessStatusCode);
            }
            catch
            {
                return new CalcultateCustomerResponse<T>(Activator.CreateInstance<T>(), HttpStatusCode.BadRequest, false);
            }
        }
    }
}
