using Domain.SuperAdminModels.Notification;
using Interactor.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Notification;
using SuperAdminAPI.Reponse.Notification;
using SuperAdminAPI.Request.Notification;
using UseCase.Core.Sync;
using UseCase.SetMst.SaveSetMst;
using UseCase.SuperAdmin.GetNotification;
using UseCase.SuperAdmin.UpdateNotification;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;

    public NotificationController(UseCaseBus bus, IWebSocketService webSocketService)
    {
        _bus = bus;
        _webSocketService = webSocketService;
    }

    [HttpGet("GetNotification")]
    public ActionResult<Response<GetNotificationResponse>> GetNotification([FromQuery] GetNotificationRequest request)
    {
        var input = new GetNotificationInputData(request.Skip, request.Take, request.OnlyUnreadNotifications);
        var output = _bus.Handle(input);
        var presenter = new GetNotificationPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetNotificationResponse>>(presenter.Result);
    }

    [HttpPost("UpdateNotification")]
    public async Task<ActionResult<Response<UpdateNotificationResponse>>> UpdateNotification([FromBody] UpdateNotificationRequest request)
    {
        var input = new UpdateNotificationInputData(request.NotificationList.Select(item => new NotificationModel(item.Id, item.IsDeleted, item.IsRead)).ToList(), request.IsRealAllNotifications);
        var output = _bus.Handle(input);
        if (output.Status == UpdateNotificationStatus.Successed && output.NotificationList.Any())
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.UpdateNotification, output.NotificationList);
        }
        var presenter = new UpdateNotificationPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<UpdateNotificationResponse>>(presenter.Result);
    }
}
