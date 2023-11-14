using Helper;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using Newtonsoft.Json;

namespace EmrCloudApi.Services
{
    public class CalcultateCustomerService : ICalcultateCustomerService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ITenantProvider _tenantProvider;
        private readonly ILoggingHandler _loggingHandler;

        public CalcultateCustomerService(IConfiguration configuration, ITenantProvider tenantProvider)
        {
            _httpClient.BaseAddress = new Uri(configuration.GetSection("CalculateApi")["BasePath"] ?? "");
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public async Task<CalcultateCustomerResponse<T>> RunCaculationPostAsync<T>(TypeCalculate type, object input)
        {
            try
            {
                var content = JsonContent.Create(input);
                content.Headers.Add("domain", _tenantProvider.GetDomainFromHeader());
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
            catch (Exception ex)
            {
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _tenantProvider.DisposeDataContext();
                _loggingHandler.Dispose();
            }
        }


        public async Task RunCaculationPostAsync(TypeCalculate type, object input)
        {
            try
            {
                var content = JsonContent.Create(input);
                content.Headers.Add("domain", _tenantProvider.GetDomainFromHeader());
                HttpResponseMessage result = await _httpClient.PostAsync(type.GetDescription(), content);
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Err when run calculatePost api . details : " + ex.Message + " " + ex.InnerException);
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }
    }
}
