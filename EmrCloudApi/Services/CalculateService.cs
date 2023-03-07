using Interactor.CalculateService;
using System.Text;

namespace EmrCloudApi.Services
{
    public class CalculateService : ICalculateService
    {
        public async Task<string> CallCalculate(string apiUrl, string jsonContent)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

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
    }
}
