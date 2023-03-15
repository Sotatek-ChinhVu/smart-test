using Helper;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace EmrCloudApi.Services
{
    public class CalcultateCustomerResponse<T>
    {
        public CalcultateCustomerResponse(T data, HttpStatusCode status)
        {
            Data = data;
            Status = status;
        }

        public T Data { get; private set; }

        public HttpStatusCode Status { get; private set; }
    }

    public enum TypeCalculate
    {
        [Description("ReceFutan/ReceFutanCalculateMain")]
        ReceFutanCalculateMain
    }

    public interface ICalcultateCustomerService
    {
        /// <summary>
        /// Call Httpclient Customer Calculation app
        /// </summary>
        /// <typeparam name="T">TData get from res HttpClient</typeparam>
        /// <param name="type">type calculate</param>
        /// <param name="input">object json</param>
        /// <returns></returns>
        Task<CalcultateCustomerResponse<T>> RunCaculationPostAsync<T>(TypeCalculate type, object input);
    }

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
            StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(type.GetDescription(), content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                string resultContentStr = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentStr);
                return new CalcultateCustomerResponse<T>(resultContent, result.StatusCode);
            }
            else return new CalcultateCustomerResponse<T>(Activator.CreateInstance<T>(), result.StatusCode);
        }
    }
}
