﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Requests.ReceiptCheck;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.Receipt.Recalculation;
using UseCase.ReceiptCheck.Recalculation;
using UseCase.ReceiptCheck.ReceiptInfEdit;
using EmrCloudApi.Responses.Receipt.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using Infrastructure.Interfaces;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class RecalculationController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private CancellationToken? _cancellationToken;
    private readonly ITenantProvider _tenantProvider;
    private readonly IConfiguration _configuration;
    private readonly IMessenger _messenger;
    private HubConnection connection;
    private string uniqueKey;
    private bool stopCalculate = false;

    public RecalculationController(UseCaseBus bus, IConfiguration configuration, IUserService userService, ITenantProvider tenantProvider, IMessenger messenger) : base(userService)
    {
        _bus = bus;
        _tenantProvider = tenantProvider;
        _configuration = configuration;
        uniqueKey = string.Empty;
        _messenger = messenger;
    }

    [HttpPost]
    public void Recalculation([FromBody] RecalculationRequest request, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        try
        {
            _messenger.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
            _messenger.Register<StopCalcStatus>(this, StopCalculation);

            HttpContext.Response.ContentType = "application/json";
            //HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            //response.StatusCode = 202;

            uniqueKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var input = new RecalculationInputData(HpId, UserId, request.SinYm, request.PtIdList, request.IsRecalculationCheckBox, request.IsReceiptAggregationCheckBox, request.IsCheckErrorCheckBox, uniqueKey, cancellationToken, _messenger);
            _bus.Handle(input);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception Cloud:" + ex.Message);
            SendMessage(new RecalculationStatus(true, 0, 0, 0, "再計算にエラーが発生しました。\n\rしばらくしてからもう一度お試しください。", string.Empty));
        }
        finally
        {
            _messenger.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
            _messenger.Deregister<StopCalcStatus>(this, StopCalculation);
            HttpContext.Response.Body.Close();
        }
    }

    private void StopCalculation(StopCalcStatus stopCalcStatus)
    {
        if (stopCalculate)
        {
            stopCalcStatus.CallFailCallback(stopCalculate);
        }
        else if (!_cancellationToken.HasValue)
        {
            stopCalcStatus.CallFailCallback(false);
        }
        else
        {
            stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
        }
    }

    private void UpdateRecalculationStatus(RecalculationStatus status)
    {
        if (!status.UniqueKey.Equals("NotConnectSocket") && (status.Type == 1 || status.Type == 2))
        {
            if (status.Message.Equals("StartCalculateMonth") || status.Message.Equals("StartFutanCalculateMain"))
            {
                string domain = _tenantProvider.GetDomainFromHeader();
                string socketUrl = _configuration.GetSection("CalculateApi")["WssPath"]! + domain;
                connection = new HubConnectionBuilder()
                 .WithUrl(socketUrl)
                 .Build();

                var connect = connection.StartAsync();
                connect.Wait();
            }

            connection.On<string, string>("ReceiveMessage", (function, data) =>
            {
                if (function.Equals(FunctionCodes.RunCalculate))
                {
                    try
                    {
                        var objectStatus = JsonSerializer.Deserialize<RecalculationStatus>(data);
                        if (objectStatus != null && objectStatus.UniqueKey.Equals(uniqueKey))
                        {
                            if (objectStatus.Type == -1)
                            {
                                stopCalculate = true;
                            }
                            SendMessage(objectStatus);
                            if (objectStatus.Done)
                            {
                                connection.DisposeAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception Calculate:" + data);
                        SendMessage(new RecalculationStatus(true, 0, 0, 0, "再計算にエラーが発生しました。\n\rしばらくしてからもう一度お試しください。", string.Empty));
                    }
                }
            });
        }
        else
        {
            SendMessage(status);
        }
    }

    private void SendMessage(RecalculationStatus status)
    {
        var dto = new RecalculationDto(status);
        string result = "\n" + JsonSerializer.Serialize(dto);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.FlushAsync();
    }

    [HttpPost(ApiPath.ReceiptCheck)]
    public void ReceiptCheckRecalculation([FromBody] ReceiptCheckRecalculationRequest request)
    {
        try
        {
            _messenger.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
            _messenger.Register<StopCalcStatus>(this, StopCalculation);

            HttpContext.Response.ContentType = "application/json";
            //HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            //response.StatusCode = 202;

            var input = new ReceiptCheckRecalculationInputData(HpId, UserId, request.PtIds, request.SeikyuYm, request.ReceStatus, _messenger);
            _bus.Handle(input);
        }
        catch (Exception ex)
        {
            SendMessage(new RecalculationStatus(true, 0, 0, 0, "再計算にエラーが発生しました。\n\rしばらくしてからもう一度お試しください。", string.Empty));
        }
        finally
        {
            _messenger.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
            _messenger.Deregister<StopCalcStatus>(this, StopCalculation);
            HttpContext.Response.Body.Close();
        }
    }

    [HttpGet(ApiPath.DeleteReceiptInfEdit)]
    public ActionResult<Response<DeleteReceiptInfResponse>> DeleteReceiptInfEdit([FromQuery] DeleteReceiptInfEditRequest request)
    {
        var input = new DeleteReceiptInfEditInputData(HpId, UserId, request.PtId, request.SeikyuYm, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new DeleteReceiptInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<DeleteReceiptInfResponse>>(presenter.Result);
    }
}
