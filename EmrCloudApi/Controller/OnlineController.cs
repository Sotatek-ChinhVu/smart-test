using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.AccountDue;
using EmrCloudApi.Presenters.Online;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.AccountDue;
using EmrCloudApi.Requests.Online;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.AccountDue;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.IsNyukinExisted;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Core.Sync;
using UseCase.Online;
using UseCase.Online.InsertOnlineConfirmHistory;

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

}
