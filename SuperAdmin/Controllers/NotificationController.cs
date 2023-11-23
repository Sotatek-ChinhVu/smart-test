using Domain.SuperAdminModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Notification;
using SuperAdminAPI.Reponse.Notification;
using SuperAdminAPI.Request.Notification;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.GetNotification;
using UseCase.SuperAdmin.UpdateNotification;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public NotificationController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet("GetNotification")]
    public ActionResult<Response<GetNotificationResponse>> GetNotification([FromQuery] GetNotificationRequest request)
    {
        var input = new GetNotificationInputData(request.Skip, request.Take);
        var output = _bus.Handle(input);
        var presenter = new GetNotificationPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetNotificationResponse>>(presenter.Result);
    }

    [HttpPost("UpdateNotification")]
    public ActionResult<Response<UpdateNotificationResponse>> UpdateNotification([FromBody] UpdateNotificationRequest request)
    {
        var input = new UpdateNotificationInputData(request.NotificationList.Select(item => new NotificationModel(item.Id, item.IsDeleted, item.IsRead)).ToList());
        var output = _bus.Handle(input);
        var presenter = new UpdateNotificationPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<UpdateNotificationResponse>>(presenter.Result);
    }
}
