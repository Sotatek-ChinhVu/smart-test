using Domain.Models.CalculateModel;
using Helper.Enum;
using Interactor.CalculateService;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;

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

        public async Task<CalculateResponse> CallCalculate(CalculateApiPath path, object inputData)
        {
            var content = JsonContent.Create(inputData);

            var basePath = _configuration.GetSection("CalculateApi")["BasePath"]!;
            string functionName = string.Empty;
            switch (path)
            {
                case CalculateApiPath.GetSinMeiList:
                    functionName = "SinMei/GetSinMeiList";
                    break;
                case CalculateApiPath.RunCalculate:
                    functionName = "Calculate/RunCalculate";
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
                    return new CalculateResponse(responseContent, ResponseStatus.Successed);
                }

                return new CalculateResponse(response.StatusCode.ToString(), ResponseStatus.Successed);

            }
            catch (HttpRequestException ex)
            {
                return new CalculateResponse("Failed: Could not connect to Calculate API", ResponseStatus.ConnectFailed);
            }
        }

        public SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.GetSinMeiList, inputData);

                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                    return new();

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SinMeiDataModelDto>(task.Result.ResponseMessage);
                return result;
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public bool RunCalculate(RecaculationInputDto inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.RunCalculate, inputData);
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
