using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.Online;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Online;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using UseCase.Core.Sync;
using UseCase.Online;
using UseCase.Online.GetListOnlineConfirmationHistoryModel;
using UseCase.Online.GetOnlineConsent;
using UseCase.Online.GetRegisterdPatientsFromOnline;
using UseCase.Online.InsertOnlineConfirmation;
using UseCase.Online.InsertOnlineConfirmHistory;
using UseCase.Online.SaveAllOQConfirmation;
using UseCase.Online.SaveOnlineConfirmation;
using UseCase.Online.SaveOQConfirmation;
using UseCase.Online.UpdateOnlineConfirmationHistory;
using UseCase.Online.UpdateOnlineConsents;
using UseCase.Online.UpdateOnlineHistoryById;
using UseCase.Online.UpdateOnlineInRaiinInf;
using UseCase.Online.UpdateOQConfirmation;
using UseCase.Online.UpdatePtInfOnlineQualify;
using UseCase.Online.UpdateRefNo;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class OnlineController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;
    public OnlineController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
    {
        _bus = bus;
        _webSocketService = webSocketService;
    }

    [HttpPost(ApiPath.InsertOnlineConfirmHistory)]
    public ActionResult<Response<InsertOnlineConfirmHistoryResponse>> InsertOnlineConfirmHistory([FromBody] InsertOnlineConfirmHistoryRequest request)
    {
        var onlineList = request.OnlineConfirmList.Select(item => new OnlineConfirmationHistoryItem(
                                                                      item.PtId,
                                                                      DateTime.MinValue,
                                                                      item.ConfirmationType,
                                                                      string.Empty,
                                                                      item.ConfirmationResult,
                                                                      0,
                                                                      item.UketukeStatus))
                                                  .ToList();

        var input = new InsertOnlineConfirmHistoryInputData(UserId, onlineList);
        var output = _bus.Handle(input);

        var presenter = new InsertOnlineConfirmHistoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<InsertOnlineConfirmHistoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetRegisterdPatientsFromOnline)]
    public ActionResult<Response<GetRegisterdPatientsFromOnlineResponse>> GetRegisterdPatientsFromOnline([FromQuery] GetRegisterdPatientsFromOnlineRequest request)
    {
        var input = new GetRegisterdPatientsFromOnlineInputData(request.SinDate, request.ConfirmType, request.Id);
        var output = _bus.Handle(input);

        var presenter = new GetRegisterdPatientsFromOnlinePresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetRegisterdPatientsFromOnlineResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateOnlineConfirmationHistory)]
    public ActionResult<Response<UpdateOnlineConfirmationHistoryResponse>> UpdateOnlineConfirmationHistory([FromBody] UpdateOnlineConfirmationHistoryRequest request)
    {
        var input = new UpdateOnlineConfirmationHistoryInputData(request.Id, UserId, request.IsDeleted);
        var output = _bus.Handle(input);

        var presenter = new UpdateOnlineConfirmationHistoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineConfirmationHistoryResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateOnlineHistoryById)]
    public ActionResult<Response<UpdateOnlineHistoryByIdResponse>> UpdateOnlineHistoryById([FromBody] UpdateOnlineHistoryByIdRequest request)
    {
        var input = new UpdateOnlineHistoryByIdInputData(HpId, UserId, request.Id, request.PtId, request.UketukeStatus, request.ConfirmationType);
        var output = _bus.Handle(input);

        var presenter = new UpdateOnlineHistoryByIdPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineHistoryByIdResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveOQConfirmation)]
    public async Task<ActionResult<Response<SaveOQConfirmationResponse>>> SaveOQConfirmation([FromBody] SaveOQConfirmationRequest request)
    {
        var input = new SaveOQConfirmationInputData(HpId, UserId, request.OnlineHistoryId, request.PtId, request.ConfirmationResult, request.OnlineConfirmationDate, request.ConfirmationType, request.InfConsFlg, request.UketukeStatus, request.IsUpdateRaiinInf);
        var output = _bus.Handle(input);

        if (output.Status == SaveOQConfirmationStatus.Successed)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, new()));
        }

        var presenter = new SaveOQConfirmationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveOQConfirmationResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateRefNo)]
    public ActionResult<Response<UpdateRefNoResponse>> UpdateRefNo([FromBody] UpdateRefNoRequest request)
    {
        var input = new UpdateRefNoInputData(HpId, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new UpdateRefNoPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateRefNoResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateOnlineInRaiinInf)]
    public async Task<ActionResult<Response<UpdateOnlineInRaiinInfResponse>>> UpdateOnlineInRaiinInf([FromBody] UpdateOnlineInRaiinInfRequest request)
    {
        var input = new UpdateOnlineInRaiinInfInputData(HpId, UserId, request.PtId, request.OnlineConfirmationDate, request.ConfirmationType, request.InfConsFlg);
        var output = _bus.Handle(input);

        if (output.Status == UpdateOnlineInRaiinInfStatus.Successed)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, new()));
        }

        var presenter = new UpdateOnlineInRaiinInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineInRaiinInfResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateOQConfirmation)]
    public ActionResult<Response<UpdateOQConfirmationResponse>> UpdateOQConfirmation([FromBody] UpdateOQConfirmationRequest request)
    {
        Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict = new();
        foreach (var item in request.OnlQuaConfirmationTypeDict)
        {
            onlQuaConfirmationTypeDict.Add(item.Key, (item.Value.ConfirmationType, item.Value.InfConsFlg));
        }
        var input = new UpdateOQConfirmationInputData(HpId, UserId, request.OnlineHistoryId, request.OnlQuaResFileDict, onlQuaConfirmationTypeDict);
        var output = _bus.Handle(input);

        var presenter = new UpdateOQConfirmationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOQConfirmationResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveAllOQConfirmation)]
    public ActionResult<Response<SaveAllOQConfirmationResponse>> SaveAllOQConfirmation([FromBody] SaveAllOQConfirmationRequest request)
    {
        Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict = new();
        foreach (var item in request.OnlQuaConfirmationTypeDict)
        {
            onlQuaConfirmationTypeDict.Add(item.Key, (item.Value.ConfirmationType, item.Value.InfConsFlg));
        }
        var input = new SaveAllOQConfirmationInputData(HpId, UserId, request.PtId, request.OnlQuaResFileDict, onlQuaConfirmationTypeDict);
        var output = _bus.Handle(input);

        var presenter = new SaveAllOQConfirmationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveAllOQConfirmationResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdatePtInfOnlineQualify)]
    public ActionResult<Response<UpdatePtInfOnlineQualifyResponse>> UpdatePtInfOnlineQualify([FromBody] UpdatePtInfOnlineQualifyRequest request)
    {
        var resultList = request.ResultList.Select(item => new PtInfConfirmationItem(item.AttributeName, item.CurrentValue, item.XmlValue)).ToList();
        var input = new UpdatePtInfOnlineQualifyInputData(HpId, UserId, request.PtId, resultList);
        var output = _bus.Handle(input);

        var presenter = new UpdatePtInfOnlineQualifyPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdatePtInfOnlineQualifyResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListOnlineConfirmationHistoryByPtId)]
    public ActionResult<Response<GetListOnlineConfirmationHistoryModelResponse>> GetListOnlineConfirmationHistoryByPtId([FromQuery] GetListOnlineConfirmationHistoryByPtIdRequest request)
    {
        var input = new GetListOnlineConfirmationHistoryModelInputData(HpId, UserId, request.PtId, new(), new());
        var output = _bus.Handle(input);

        var presenter = new GetListOnlineConfirmationHistoryModelPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListOnlineConfirmationHistoryModelResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetOnlineConsent)]
    public ActionResult<Response<GetOnlineConsentResponse>> GetOnlineConsent([FromQuery] GetOnlineConsentRequest request)
    {
        var input = new GetOnlineConsentInputData(request.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetOnlineConsentPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetOnlineConsentResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.GetListOnlineConfirmationHistoryModel)]
    public ActionResult<Response<GetListOnlineConfirmationHistoryModelResponse>> GetListOnlineConfirmationHistoryModel([FromBody] GetListOnlineConfirmationHistoryModelRequest request)
    {
        Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict = new();
        foreach (var item in request.OnlQuaConfirmationTypeDict)
        {
            onlQuaConfirmationTypeDict.Add(item.Key, (item.Value.ConfirmationType, item.Value.InfConsFlg));
        }
        var input = new GetListOnlineConfirmationHistoryModelInputData(HpId, UserId, 0, request.OnlQuaResFileDict, onlQuaConfirmationTypeDict);
        var output = _bus.Handle(input);

        var presenter = new GetListOnlineConfirmationHistoryModelPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListOnlineConfirmationHistoryModelResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.ConvertXmlToQCXmlMsg)]
    public ActionResult<Response<ConvertXmlToQCXmlMsgResponse>> ConvertXmlToQCXmlMsgResponse([FromBody] ConvertXmlToQCXmlMsgRequest request)
    {
        Response<ConvertXmlToQCXmlMsgResponse> response = new();
        try
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(request.XmlString);

            response.Data = new ConvertXmlToQCXmlMsgResponse(request.XmlString);
            response.Message = ResponseMessage.Success;
            response.Status = 1;
        }
        catch
        {
            response.Message = ResponseMessage.InvalidConfirmationResult;
            response.Status = 2;
        }
        return new ActionResult<Response<ConvertXmlToQCXmlMsgResponse>>(response);
    }

    [HttpPost(ApiPath.UpdateOnlineConsents)]
    public ActionResult<Response<UpdateOnlineConsentsResponse>> UpdateOnlineConsents([FromBody] UpdateOnlineConsentsRequest request)
    {
        var input = new UpdateOnlineConsentsInputData(HpId, UserId, request.PtId, request.ResponseList);
        var output = _bus.Handle(input);

        var presenter = new UpdateOnlineConsentsPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineConsentsResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateOnlineConfirmation)]
    public async Task<ActionResult<Response<UpdateOnlineConfirmationResponse>>> UpdateOnlineConfirmation([FromBody] UpdateOnlineConfirmationRequest request)
    {
        var input = new UpdateOnlineConfirmationInputData(HpId, UserId, request.ReceptionNumber, request.YokakuDate, request.QCBIDXmlMsgResponse);
        var output = _bus.Handle(input);

        if (output.Status == UpdateOnlineConfirmationStatus.Successed)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.Receptions, new()));
        }

        var presenter = new UpdateOnlineConfirmationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineConfirmationResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.InsertOnlineConfirmation)]
    public ActionResult<Response<InsertOnlineConfirmationResponse>> InsertOnlineConfirmation([FromBody] InsertOnlineConfirmationRequest request)
    {
        var input = new InsertOnlineConfirmationInputData(UserId, request.SinDate, request.ArbitraryFileIdentifier, request.QCBIXmlMsgResponse);
        var output = _bus.Handle(input);

        var presenter = new InsertOnlineConfirmationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<InsertOnlineConfirmationResponse>>(presenter.Result);
    }
}
