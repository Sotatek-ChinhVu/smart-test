using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Online;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Online;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UseCase.Core.Sync;
using UseCase.Online;
using UseCase.Online.GetRegisterdPatientsFromOnline;
using UseCase.Online.InsertOnlineConfirmHistory;
using UseCase.Online.SaveAllOQConfirmation;
using UseCase.Online.SaveOQConfirmation;
using UseCase.Online.UpdateOnlineConfirmationHistory;
using UseCase.Online.UpdateOnlineHistoryById;
using UseCase.Online.UpdateOQConfirmation;
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
                                                                      TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(item.OnlineConfirmationDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)),
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
        var input = new UpdateOnlineHistoryByIdInputData(UserId, request.Id, request.PtId, request.UketukeStatus, request.ConfirmationType);
        var output = _bus.Handle(input);

        var presenter = new UpdateOnlineHistoryByIdPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateOnlineHistoryByIdResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveOQConfirmation)]
    public ActionResult<Response<SaveOQConfirmationResponse>> SaveOQConfirmation([FromBody] SaveOQConfirmationRequest request)
    {
        var input = new SaveOQConfirmationInputData(HpId, UserId, request.OnlineHistoryId, request.PtId, request.ConfirmationResult, request.OnlineConfirmationDate, request.ConfirmationType, request.InfConsFlg, request.UketukeStatus, request.IsUpdateRaiinInf);
        var output = _bus.Handle(input);

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
}
