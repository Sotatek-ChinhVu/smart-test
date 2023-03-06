using EmrCalculateApi.Interface;
using Infrastructure.Interfaces;
using System.Text.Json;
using System.Text;
using UseCase.Accounting.Recaculate;

namespace Interactor.Accounting
{
    public class RecaculationInteractor : IRecaculationInputPort
    {
       

        public RecaculationOutputData Handle(RecaculationInputData inputData)
        {
            var input = new RecaculationInputData(inputData.HpId, inputData.PtId, inputData.SinDate);
            var result =  Recaculate("https://localhost:7146/api/Calculate/RunCalculate", input);
            
            //IkaCalculateViewModel ikaCalculateViewModel = new IkaCalculateViewModel(_futancalcViewModel, _tenantProvider, _systemConfigProvider, _emrLogger);
            //ikaCalculateViewModel.RunCalculate(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "SAI_");

            return new RecaculationOutputData(RecaculationStatus.Successed);
        }

        public async Task<string> Recaculate(string apiUrl, RecaculationInputData request)
        {
            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    throw new Exception("Failed: " + response.StatusCode);
                }
            }
        }
    }
}
