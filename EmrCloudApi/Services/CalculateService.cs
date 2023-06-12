﻿using Domain.Models.CalculateModel;
using Helper.Enum;
using Interactor.CalculateService;
using Newtonsoft.Json;
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
        private static HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CalculateService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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
                var httpMessage = new HttpRequestMessage();
                content.Headers.Add("domain", GetDomainFromHeader());
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
                Console.WriteLine("Function CallCalculate " + ex);
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

                var result = JsonConvert.DeserializeObject<SinMeiDataModelDto>(task.Result.ResponseMessage);
                return result ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function GetSinMeiList " + ex);
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
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculate " + ex);
                return false;
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
            catch (Exception)
            {
                return new();
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
                return new RunTraialCalculateResponse(new(), new(), new());
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
                return false;
            }
        }

        public bool ReceFutanCalculateMain(ReceCalculateRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.ReceFutanCalculateMain, inputData);
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function ReceFutanCalculateMain " + ex);
                return false;
            }
        }

        public bool RunCalculateMonth(CalculateMonthRequest inputData)
        {
            try
            {
                var task = CallCalculate(CalculateApiPath.RunCalculateMonth, inputData);
                if (task.Result.ResponseStatus != ResponseStatus.Successed)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function RunCalculateMonth " + ex);
                return false;
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
                return new();
            }
        }

        private string GetDomainFromHeader()
        {
            var headers = _httpContextAccessor?.HttpContext?.Request.Headers;
            if (headers == null || !headers.ContainsKey("domain"))
            {
                return string.Empty;
            }
            string? clientDomain = headers["domain"];

            return clientDomain ?? string.Empty;
        }

    }
}
