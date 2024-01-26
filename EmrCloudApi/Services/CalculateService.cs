using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Domain.Models.CalculateModel;
using Helper.Enum;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.Recalculation;

namespace EmrCloudApi.Services
{
    public class CalculateService : ICalculateService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILoggingHandler _loggingHandler;

        public CalculateService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITenantProvider tenantProvider)
        {
            _configuration = configuration;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
                case CalculateApiPath.GetListReceInf:
                    functionName = "ReceFutan/GetListReceInf";
                    break;
                case CalculateApiPath.RunTrialCalculate:
                    functionName = "Calculate/RunTrialCalculate";
                    break;
                case CalculateApiPath.RunCalculateOne:
                    functionName = "Calculate/RunCalculateOne";
                    break;
                case CalculateApiPath.ReceFutanCalculateMain:
                    functionName = "ReceFutan/ReceFutanCalculateMain";
                    break;
                case CalculateApiPath.RunCalculateMonth:
                    functionName = "Calculate/RunCalculateMonth";
                    break;
                default:
                    throw new NotImplementedException("The Api Path Is Incorrect: " + path.ToString());
            }

            try
            {
                content.Headers.Add("domain", _tenantProvider.GetDomainFromHeader());

                var timer = new Stopwatch();
                timer.Start();
                using (var cts = new CancellationTokenSource(new TimeSpan(0, 5, 0)))
                {
                    var response = await _httpClient.PostAsync($"{basePath}{functionName}", content, cts.Token).ConfigureAwait(false);
                    timer.Stop();
                    TimeSpan timeTaken = timer.Elapsed;
                    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                    Console.WriteLine(foo);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return new CalculateResponse(responseContent, ResponseStatus.Successed);
                    }

                    return new CalculateResponse(response.StatusCode.ToString(), ResponseStatus.Successed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function CallCalculate " + ex);
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public async Task<CalculateResponse> CallCalculate(CalculateApiPath path, object inputData, CancellationToken cancellationToken)
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
                case CalculateApiPath.GetListReceInf:
                    functionName = "ReceFutan/GetListReceInf";
                    break;
                case CalculateApiPath.RunTrialCalculate:
                    functionName = "Calculate/RunTrialCalculate";
                    break;
                case CalculateApiPath.RunCalculateOne:
                    functionName = "Calculate/RunCalculateOne";
                    break;
                case CalculateApiPath.ReceFutanCalculateMain:
                    functionName = "ReceFutan/ReceFutanCalculateMain";
                    break;
                case CalculateApiPath.RunCalculateMonth:
                    functionName = "Calculate/RunCalculateMonth";
                    break;
                default:
                    throw new NotImplementedException("The Api Path Is Incorrect: " + path.ToString());
            }

            try
            {
                content.Headers.Add("domain", _tenantProvider.GetDomainFromHeader());
                var response = await _httpClient.PostAsync($"{basePath}{functionName}", content, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return new CalculateResponse(responseContent, ResponseStatus.Successed);
                }

                return new CalculateResponse(response.StatusCode.ToString(), ResponseStatus.Successed);

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Function CallCalculate " + ex);
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.GetSinMeiList, inputData);

                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                    return new();

                var result = JsonConvert.DeserializeObject<SinMeiDataModelDto>(task.Result.ResponseMessage);
                return result ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function GetSinMeiList " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
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
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculate " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public ReceInfModelDto GetListReceInf(GetInsuranceInfInputData inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.GetListReceInf, inputData);

                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                    return new();

                var result = JsonConvert.DeserializeObject<ReceInfModelDto>(task.Result.ResponseMessage);
                return result ?? new();
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public RunTraialCalculateResponse RunTrialCalculate(RunTraialCalculateRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.RunTrialCalculate, inputData);
                if (task.Result.ResponseStatus == ResponseStatus.Successed)
                {
                    var result = JsonConvert.DeserializeObject<RunTraialCalculateResponse>(task.Result.ResponseMessage);
                    return new RunTraialCalculateResponse(result?.SinMeiList ?? new(), result?.KaikeiInfList ?? new(), result?.CalcLogList ?? new());
                }
                else
                {
                    return new RunTraialCalculateResponse(new(), new(), new());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunTrialCalculate " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public bool RunCalculateOne(CalculateOneRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.RunCalculateOne, inputData);
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculateOne " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        /// <summary>
        /// function calls ReceFutanCalculateMain to other functions
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public bool ReceFutanCalculateMain(ReceCalculateRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.ReceFutanCalculateMain, inputData);
                task.Wait();
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function ReceFutanCalculateMain " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        /// <summary>
        /// function calls ReceFutanCalculateMain only to calculate runs in month Rece
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ReceFutanCalculateMain(ReceCalculateRequest inputData, CancellationToken cancellationToken)
        {
            try
            {
                await CallCalculate(CalculateApiPath.ReceFutanCalculateMain, inputData, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function ReceFutanCalculateMain " + ex);
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        /// <summary>
        /// function calls RunCalculateMonth to other functions
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public bool RunCalculateMonth(CalculateMonthRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.RunCalculateMonth, inputData);
                task.Wait();
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculateMonth " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        /// <summary>
        /// function calls RunCalculateMonth only to calculate runs in month Rece
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RunCalculateMonth(CalculateMonthRequest inputData, CancellationToken cancellationToken)
        {
            try
            {
                await CallCalculate(CalculateApiPath.RunCalculateMonth, inputData, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculateMonth " + ex);
                await _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public SinMeiDataModelDto GetSinMeiInMonthList(GetSinMeiDtoInputData inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.GetSinMeiList, inputData);
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                {
                    return new();
                }
                var result = JsonConvert.DeserializeObject<SinMeiDataModelDto>(task.Result.ResponseMessage);
                return result ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculateMonth " + ex);
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }

        public void ReleaseSource()
        {
            _loggingHandler.Dispose();
            _tenantProvider.DisposeDataContext();
        }
    }
}
